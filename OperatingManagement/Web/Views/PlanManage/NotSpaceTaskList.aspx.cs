using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.Security;
using System.Web.UI.WebControls;

using OperatingManagement.WebKernel.Basic;
using OperatingManagement.WebKernel.Route;
using OperatingManagement.Framework.Core;
using OperatingManagement.DataAccessLayer;
using OperatingManagement.Framework;
using OperatingManagement.DataAccessLayer.PlanManage;
using OperatingManagement.DataAccessLayer.BusinessManage;
using ServicesKernel;
using ServicesKernel.File;
using ServicesKernel.DataFrame;
using ServicesKernel.GDFX;
using OperatingManagement.Framework.Storage;
using OperatingManagement.Framework.Components;

namespace OperatingManagement.Web.Views.PlanManage
{
    public partial class NotSpaceTaskList : AspNetPage, IRouteContext
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                txtStartDate.Attributes.Add("readonly", "true");
                txtEndDate.Attributes.Add("readonly", "true");

                pnlDestination.Visible = false;
                pnlData.Visible = true;

                pnlAll1.Visible = false;
                pnlAll2.Visible = false;
                BindCheckBoxDestination();
                DefaultSearch();
                ClientScript.RegisterStartupScript(this.GetType(), "error", "<script type='text/javascript'>hideSelectAll();</script>");
                HideMsg();
               // ClientScript.RegisterStartupScript(this.GetType(), "error", "<script type='text/javascript'>showMsgError();</script>");
            }
            cpPager.PostBackPage += new EventHandler(cpPager_PostBackPage);
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                SaveCondition();
                cpPager.CurrentPage = 1;
                BindGridView(true);
            }
            catch (Exception ex)
            {
                throw (new AspNetException("非空间机动任务列表页面搜索出现异常，异常原因", ex));
            }
            finally { }
        }
        /// <summary>
        /// 默认查询两周内的数据
        /// </summary>
        private void DefaultSearch()
        {
            txtStartDate.Text = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");
            txtEndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            btnSearch_Click(new Object(), new EventArgs());
        }
        private void SaveCondition()
        {
            if (string.IsNullOrEmpty(txtStartDate.Text))
            { ViewState["_StartDate"] = null; }
            else
            { ViewState["_StartDate"] = txtStartDate.Text.Trim(); }
            if (string.IsNullOrEmpty(txtEndDate.Text))
            { ViewState["_EndDate"] = null; }
            else
            { ViewState["_EndDate"] = Convert.ToDateTime(txtEndDate.Text.Trim()).AddDays(1).AddMilliseconds(-1); }
            if (ucGDType.SelectedValue == "-1")
            { ViewState["_ICode"] = null; }
            else
            { ViewState["_ICode"] = ucGDType.SelectedValue; }
            if (ucOutTask1.SelectedValue == "-1")
            { ViewState["_Task"] = null; }
            else
            { ViewState["_Task"] = ucOutTask1.SelectedValue; }
        }
        //绑定列表
        void BindGridView(bool fromSearch)
        {
            DateTime startDate = new DateTime();
            DateTime endDate = new DateTime();
            string iCode = string.Empty;
            string TaskID = "-1";

            if (fromSearch)
            {
                if (!string.IsNullOrEmpty(txtStartDate.Text))
                {
                    startDate = Convert.ToDateTime(txtStartDate.Text);
                }
                if (!string.IsNullOrEmpty(txtEndDate.Text))
                {
                    endDate = Convert.ToDateTime(txtEndDate.Text).AddDays(1).AddMilliseconds(-1);   //查询时可查当天
                }
                TaskID = ucOutTask1.SelectedValue;
                iCode = ucGDType.SelectedValue;
            }
            else
            {
                if (ViewState["_StartDate"] != null)
                {
                    startDate = Convert.ToDateTime(ViewState["_StartDate"].ToString());
                }
                if (ViewState["_EndDate"] != null)
                {
                    endDate = Convert.ToDateTime(ViewState["_EndDate"].ToString());
                }
                if (ViewState["_Task"] != null)
                { TaskID = ViewState["_Task"].ToString(); }
                if (ViewState["_ICode"] != null)
                { iCode = ViewState["_ICode"].ToString(); }
            }

            //外部任务代号
            //List<GD> listDatas = (new GD()).GetListByDate(startDate, endDate);
            List<GD> listDatas = (new GD()).GetList(startDate, endDate, TaskID, iCode);
            cpPager.DataSource = listDatas;
            cpPager.PageSize = this.SiteSetting.PageSize;
            cpPager.BindToControl = rpDatas;
            rpDatas.DataSource = cpPager.DataSourcePaged;
            rpDatas.DataBind();

            if (listDatas.Count > 0)
            {
                pnlAll1.Visible = true;
                pnlAll2.Visible = true;
            }
            else
            {
                pnlAll1.Visible = false;
                pnlAll2.Visible = false;
            }
        }

        protected void cpPager_PostBackPage(object sender, EventArgs e)
        {
            BindGridView(false);
        }

        /// <summary>
        /// 绑定发送目标
        /// </summary>
        void BindCheckBoxDestination()
        {
            List<PlanParameter> targetList;
            ckbDestination.Items.Clear();
            targetList = PlanParameters.ReadParameters("YDSJNOSTTargetList");
            ckbDestination.DataSource = targetList;
            ckbDestination.DataTextField = "Text";
            ckbDestination.DataValueField = "Value";
            ckbDestination.DataBind();
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            BindCheckBoxDestination();
        }
        
        //最终发送
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            ShowMsg("");//清空提示信息
            //ParseFile(@"D:\MapJ_20121122-20121123.txt");
            //return;
            string strTarget = string.Empty;
            List<string> targets = new List<string>();
            string strYKXZ = string.Empty;
            string strResult = string.Empty;

            //如果选了4701/4702，进行轨道预报后再发送文件，但只能选择一个任务号和一个遥科学站
            #region 检查必须项
            try
            {
                foreach (ListItem item in ckbDestination.Items)
                {
                    if (item.Selected)
                    {
                        strTarget = item.Value;
                        targets.Add(strTarget);
                        if (strTarget == PlanParameters.ReadParamValue("YKXZ0471Code") 
                            || strTarget == PlanParameters.ReadParamValue("YKXZ0472Code"))
                            strYKXZ += strTarget + ",";
                    }
                }
                if (!string.IsNullOrEmpty(strYKXZ))
                {
                    strYKXZ = strYKXZ.Substring(0, strYKXZ.Length - 1);
                    if (!CheckForObsPre(strYKXZ))
                        return;
                }
            }
            catch (Exception ex)
            {
                throw (new AspNetException("发送计划出现异常，异常原因", ex));
            }
            #endregion

            try
            {
                #region Declare variant
                PlanFileCreator creater = new PlanFileCreator();
                string SendingFilePaths = "";
                XYXSInfo objXYXSInfo = new XYXSInfo();
                string strYDSJCode = PlanParameters.ReadParamValue("YDSJDataCode");
                //发送协议
                CommunicationWays protocl = (CommunicationWays)Convert.ToInt32(rbtProtocl.SelectedValue);
                //发送方ID （运控中心 YKZX）
                int senderid = objXYXSInfo.GetIdByAddrMark(System.Configuration.ConfigurationManager.AppSettings["ZXBM"]);
                //接收方ID 
                int receiverid;
                //信息类型id
                int infotypeid = (new InfoType()).GetIDByExMark(strYDSJCode);
                string strYKXZ0471 = PlanParameters.ReadParamValue("YKXZ0471Code");
                string strYKXZ0472 = PlanParameters.ReadParamValue("YKXZ0472Code");
                bool blResult = true; //发送结果
                DataFrameBuilder dfBuilder = new DataFrameBuilder();
                DFSender objSender = new DFSender();
                FileSender objFileSender = new FileSender();
                string[] filePaths;
                #endregion
                foreach (string target in targets)
                {
                    receiverid = objXYXSInfo.GetIdByAddrMark(target);
                    if (target == PlanParameters.ReadParamValue("YKXZ0471Code") 
                        || target == PlanParameters.ReadParamValue("YKXZ0472Code"))
                    {
                        #region GDYB & Send File
                        SendingFilePaths = ObsPre(strYKXZ);
                        if (string.IsNullOrEmpty(SendingFilePaths))
                            return;
                        if (protocl == CommunicationWays.FTP)//将文件移至FTP路径中
                        {
                            strResult = DataFileHandle.MoveFile(SendingFilePaths, GetFilePathByFilePath(SendingFilePaths) + @"FTP\");
                            if (!strResult.Equals(string.Empty))
                                lblMessage.Text += GetFileNameByFilePath(SendingFilePaths) + " 路径中已有同名文件。" + "<br />";
                            else
                                SendingFilePaths = GetFilePathByFilePath(SendingFilePaths) + @"FTP\" + GetFileNameByFilePath(SendingFilePaths);
                        }
                        if (strResult.Equals(string.Empty))
                        {
                            blResult = objFileSender.SendFile(DataFileHandle.GetFileName(SendingFilePaths), DataFileHandle.GetFilePath(SendingFilePaths), protocl, senderid, receiverid, infotypeid, true);
                            if (blResult)
                                lblMessage.Text += GetFileNameByFilePath(SendingFilePaths) + " 文件发送请求提交成功。" + "<br />";
                            else
                                lblMessage.Text += GetFileNameByFilePath(SendingFilePaths) + " 文件发送请求提交失败。" + "<br />";
                        }
                        #endregion
                    }
                    else if (target == PlanParameters.ReadParamValue("XSCCCode"))
                    {
                        #region Send DataFrame
                        List<GD> lstYDSJ = (new GD()).SelectByIDS(txtId.Text);
                        for (int i = 0; i < lstYDSJ.Count(); i++)
                        {
                            strResult = objSender.SendDF(dfBuilder.BuildYDSJDF(lstYDSJ[i], DateTime.Now), lstYDSJ[i].TaskID, lstYDSJ[i].SatID, infotypeid, senderid, receiverid, DateTime.Now);
                            if (string.IsNullOrEmpty(strResult))
                                lblMessage.Text += string.Format("第{0}条数据数据包发送请求提交成功。{1}", i + 1, "<br />");
                            else
                                lblMessage.Text += string.Format("第{0}条数据数据包发送请求提交失败。{1}", i + 1, "<br />");
                        }
                        #endregion
                    }
                    else
                    {
                        #region Send File
                        SendingFilePaths = creater.CreateSendingYDSJFileFromGD(txtId.Text, target);
                        filePaths = SendingFilePaths.Split(',');
                        for (int i = 0; i < filePaths.Length; i++)
                        {
                            if (protocl == CommunicationWays.FTP)//将文件移至FTP路径中
                            {
                                strResult = DataFileHandle.MoveFile(filePaths[i], GetFilePathByFilePath(filePaths[i]) + @"FTP\");
                                if (!strResult.Equals(string.Empty))
                                    lblMessage.Text += GetFileNameByFilePath(filePaths[i]) + " 路径中已有同名文件。" + "<br />";
                                else
                                    filePaths[i] = GetFilePathByFilePath(filePaths[i]) + @"FTP\" + GetFileNameByFilePath(filePaths[i]);
                            }
                            if (strResult.Equals(string.Empty))
                            {
                                blResult = objFileSender.SendFile(GetFileNameByFilePath(filePaths[i]), GetFilePathByFilePath(filePaths[i]), protocl, senderid, receiverid, infotypeid, true);
                                if (blResult)
                                {
                                    lblMessage.Text += GetFileNameByFilePath(filePaths[i]) + " 文件发送请求提交成功。" + "<br />";
                                }
                                else
                                {
                                    lblMessage.Text += GetFileNameByFilePath(filePaths[i]) + " 文件发送请求提交失败。" + "<br />";
                                }
                            }
                        }
                        #endregion
                    }
                }
                BindGridView(false);
                    
            }
            catch (Exception ex)
            {
                throw (new AspNetException("发送计划出现异常，异常原因", ex));
            }
            finally { }
            if (lblMessage.Text == "")
                HideMsg();
        }

        private bool CheckForObsPre(string ykxzid)
        {
            if (ykxzid.IndexOf(',') >= 0)
            {
                lblMessage.Text = "只能选择一个遥科学站";
                return false;
            }
            if (txtId.Text.Trim().Equals(string.Empty) || txtId.Text.IndexOf(',') >= 0)
            {
                lblMessage.Text = "只能选择一条数据";
                return false;
            }
            return true;
        }

        /// <summary>
        /// 轨道预报，并返回发送文件的路径
        /// </summary>
        /// <param name="ykxzid"></param>
        /// <returns></returns>
        private string ObsPre(string ykxzCode)
        {
            ObsPrer oPrer = new ObsPrer();
            GD oGD = new GD();
            string strResult = string.Empty;
            string strFullName = string.Empty;
            string resultPath = string.Empty;
            string[] ykxzid = new string[1];
            ykxzid[0] = new XYXSInfo().GetByAddrMark(ykxzCode).INCODE;
            oGD.Id = int.Parse(txtId.Text.Trim());
            oGD = oGD.SelectById();
            if (oGD != null)
            {
                strResult = oPrer.DoCaculate(oGD.Times, 2, 1, oGD.SatID, ykxzid, false, 0, out resultPath);
                if (!string.IsNullOrEmpty(strResult))
                {
                    lblMessage.Text += strResult;
                    return string.Empty;
                }
                else
                {
                    lblMessage.Text = "轨道预报成功，结果路径" + resultPath + "<br>";
                    string strFName = string.Empty;
                    for (int i = 0; i < oPrer.ResultFileNames.Length; i++)
                    {
                        strFName = oPrer.ResultFileNames[i];
                        strFName = strFName.Substring(strFName.LastIndexOf(@"\") + 1);
                        if (strFName.Substring(0, 5).ToUpper() == "MAPJ_")
                        {
                            strResult = resultPath.Substring(resultPath.IndexOf(@"\", 3) + 1);
                            strResult = strResult.Substring(strResult.IndexOf(@"\") + 1);
                            strFullName = Path.Combine(Param.GDYBResultFilePath, strResult, strFName);
                            break;
                        }
                    }
                }
            }
            else
                return string.Empty;

            if (!File.Exists(strFullName))
            {
                lblMessage.Text += "<br>文件不存在";
                return string.Empty;
            }

            if (!string.IsNullOrEmpty(strFullName))
            {
                StreamReader oSReader = new StreamReader(strFullName);
                oSReader.BaseStream.Seek(0, SeekOrigin.Begin);
                string strLine = string.Empty;
                List<string> datas = new List<string>();
                strLine = oSReader.ReadLine();//第一行标题
                strLine = oSReader.ReadLine();
                DateTime dt;
                try
                {
                    while (!string.IsNullOrEmpty(strLine))
                    {
                        strLine = strLine.Trim();
                        strResult = strLine.Substring(0, 10) + " " + strLine.Substring(11, 10).Replace(" ", "0");  //strLine.Substring(0, 23).Trim().Replace(" 0:", "00:");
                        dt = DateTime.ParseExact(strResult, "yyyy MM dd HH:mm:ss.f", System.Globalization.CultureInfo.InvariantCulture);
                        while (strLine.IndexOf("   ") >= 0)
                        {
                            strLine = strLine.Replace("   ", "  ");
                        }
                        datas.Add(dt.ToString("yyyyMMdd") + "  " + dt.ToString("HHmmssffff") + "  " + strLine.Substring(23));
                        strLine = oSReader.ReadLine();
                    }
                    oSReader.Close();
                }
                catch (Exception ex)
                {
                    throw new AspNetException("非空间机动任务列表页面发送数据出现异常", ex);
                }

                if (datas != null && datas.Count() > 0)
                    return new PlanFileCreator().CreateSendingYDSJFile(oGD.TaskID, oGD.SatID, ykxzCode, datas.ToArray());
                else
                    return string.Empty;
            }
            else
                return string.Empty;
        }

        //测试代码
        private string ParseFile(string strFullName)
        {
            string strResult = string.Empty;
            if (!string.IsNullOrEmpty(strFullName))
            {
                StreamReader oSReader = new StreamReader(strFullName);
                oSReader.BaseStream.Seek(0, SeekOrigin.Begin);
                string strLine = string.Empty;
                List<string> datas = new List<string>();
                strLine = oSReader.ReadLine();//第一行标题
                strLine = oSReader.ReadLine();
                DateTime dt;
                try
                {
                    while (!string.IsNullOrEmpty(strLine))
                    {
                        strLine = strLine.Trim();
                        strResult = strLine.Substring(0, 10) + " " + strLine.Substring(11, 10).Replace(" ", "0");  //strLine.Substring(0, 23).Trim().Replace(" 0:", "00:");
                        dt = DateTime.ParseExact(strResult, "yyyy MM dd HH:mm:ss.f", System.Globalization.CultureInfo.InvariantCulture);
                        while (strLine.IndexOf("   ") >= 0)
                        {
                            strLine = strLine.Replace("   ", "  ");
                        }
                        datas.Add(dt.ToString("yyyyMMdd") + "  " + dt.ToString("HHmmssffff") + "  " + strLine.Substring(23));
                        strLine = oSReader.ReadLine();
                    }
                    oSReader.Close();
                }
                catch (Exception ex)
                {
                    throw new AspNetException("非空间机动任务列表页面发送数据出现异常", ex);
                }

                if (datas != null && datas.Count() > 0)
                    return new PlanFileCreator().CreateSendingYDSJFile("0700", "074A", "JYZ1", datas.ToArray());
                else
                    return string.Empty;
            }
            else
                return string.Empty;
        }

        //取消
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            pnlDestination.Visible = false;
            pnlData.Visible = true;
        }

        protected void btnHidden_Click(object sender, EventArgs e)
        {
            pnlDestination.Visible = true;
            pnlData.Visible = false;
        }

        public override void OnPageLoaded()
        {
            this.PagePermission = "OMPLAN_NSpaceTask.View";
            this.ShortTitle = "查看非空间机动任务";
            this.SetTitle();
            this.AddJavaScriptInclude("scripts/pages/PlanManage/NotSpaceTaskList.aspx.js");
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect(Request.CurrentExecutionFilePath);
        }
        /// <summary>
        /// 获取文件完整路径下的文件名
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        private string GetFileNameByFilePath(string filepath)
        {
            return filepath.Substring(filepath.LastIndexOf("\\") + 1);
        }

        /// <summary>
        /// 获取文件完整路径下的文件路径
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        private string GetFilePathByFilePath(string filepath)
        {
            return filepath.Substring(0, filepath.LastIndexOf("\\") + 1);
        }

        private void ShowMsg(string msg)
        {
            lblMessage.Visible = true;
            lblMessage.Text = msg;
        }

        private void HideMsg()
        {
            lblMessage.Visible = false;
            lblMessage.Text = "";
        }
    }
}
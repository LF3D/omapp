using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.UI;
using System.Web.Security;
using System.Web.UI.WebControls;
using System.Configuration;

using OperatingManagement.WebKernel.Basic;
using OperatingManagement.WebKernel.Route;
using OperatingManagement.Framework.Core;
using OperatingManagement.DataAccessLayer;
using OperatingManagement.Framework;
using OperatingManagement.DataAccessLayer.PlanManage;
using OperatingManagement.DataAccessLayer.BusinessManage;
using ServicesKernel.File;
using ServicesKernel.DataFrame;
using ServicesKernel.GDFX;
using OperatingManagement.Framework.Storage;
using OperatingManagement.Framework.Components;

namespace OperatingManagement.Web.Views.PlanManage
{
    public partial class SpaceTaskList : AspNetPage, IRouteContext
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtStartDate.Attributes.Add("readonly", "true");
                txtEndDate.Attributes.Add("readonly", "true");
                pnlAll1.Visible = false;
                pnlAll2.Visible = false;

                pnlDestination.Visible = false;
                pnlData.Visible = true;

                BindCheckBoxDestination();
                DefaultSearch();
                HiddenMsg();
            }
            cpPager.PostBackPage += new EventHandler(cpPager_PostBackPage);
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
                throw (new AspNetException("列表页面搜索出现异常，异常原因", ex));
            }
            finally { }
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
            string TaskID = "-1";
            if (fromSearch)
            {
                if (!string.IsNullOrEmpty(txtStartDate.Text))
                {
                    startDate = Convert.ToDateTime(txtStartDate.Text.Trim());
                }
                //else
                //{
                //    startDate = DateTime.Now.AddDays(-14);
                //}
                if (!string.IsNullOrEmpty(txtEndDate.Text))
                {
                    endDate = Convert.ToDateTime(txtEndDate.Text).AddDays(1).AddMilliseconds(-1);   //查询时可查当天
                }
                //else
                //{
                //    endDate = DateTime.Now.AddDays(1).AddMilliseconds(-1);   //查询时可查当天
                //}
                TaskID = ucOutTask1.SelectedValue;
            }
            else
            {
                if (ViewState["_StartDate"] == null)
                {
                    //startDate = DateTime.Now.AddDays(-14);
                }
                else
                {
                    startDate = Convert.ToDateTime(ViewState["_StartDate"].ToString());
                }
                if (ViewState["_EndDate"] == null)
                {
                    //endDate = DateTime.Now;
                }
                else
                {
                    endDate = Convert.ToDateTime(ViewState["_EndDate"].ToString());
                }
                if (ViewState["_Task"] != null)
                { TaskID = ViewState["_Task"].ToString(); }
            }
            //外部任务代号
            List<YDSJ> listDatas = (new YDSJ()).GetListByDate(startDate, endDate, TaskID);
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
            targetList = PlanParameters.ReadParameters("YDSJSTTargetList");
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
            string taskID = string.Empty;
            string satID = string.Empty;
            string outTaskID = ucOutTask1.SelectedValue;
            string[] datas;
            bool blResult;
            HiddenMsg();
            string msg = GenYDSJData(out datas);
            if (!msg.Equals(string.Empty))
            {
                ShowMsg(msg);
                return;
            }

            try
            {
                FileSender objFileSender = new FileSender();
                XYXSInfo objXYXSInfo = new XYXSInfo();
                string strResultFile = string.Empty;
                //发送协议
                CommunicationWays protocl = (CommunicationWays)Convert.ToInt32(rbtProtocl.SelectedValue);
                string strYDSJCode = PlanParameters.ReadParamValue("YDSJDataCode");
                //信息类型id
                int infotypeid = (new InfoType()).GetIDByExMark(strYDSJCode);
                //发送方ID （运控中心 YKZX）
                int senderid = objXYXSInfo.GetIdByAddrMark(System.Configuration.ConfigurationManager.AppSettings["ZXBM"]);
                //接收方ID 
                int reveiverid;
                string strResult = string.Empty;
                string strMsg = string.Empty;
                new Task().GetTaskNoSatID(outTaskID, out taskID, out satID);
                datas = datas.Where(t => t != null).ToArray();
                foreach (ListItem li in ckbDestination.Items)
                {
                    if (li.Selected)
                    {
                        reveiverid = objXYXSInfo.GetIdByAddrMark(li.Value);
                        strResultFile = new PlanFileCreator().CreateSendingYDSJFile(taskID, satID, li.Value, datas);

                        if (protocl == CommunicationWays.FTP)//将文件移至FTP路径中
                        {
                            strResult = DataFileHandle.MoveFile(strResultFile, GetFilePathByFilePath(strResultFile) + @"FTP\");
                            if (!strResult.Equals(string.Empty))
                                strMsg += GetFileNameByFilePath(strResultFile) + " 路径中已有同名文件。" + "<br />";
                            else
                                strResultFile = GetFilePathByFilePath(strResultFile) + @"FTP\" + GetFileNameByFilePath(strResultFile);
                        }
                        if (strResult.Equals(string.Empty))
                        {
                            blResult = objFileSender.SendFile(GetFileNameByFilePath(strResultFile), GetFilePathByFilePath(strResultFile), protocl, senderid, reveiverid, infotypeid, true);
                            if (blResult)
                            {
                                strMsg += GetFileNameByFilePath(strResultFile) + " 文件发送请求提交成功。" + "<br />";
                            }
                            else
                            {
                                strMsg += GetFileNameByFilePath(strResultFile) + " 文件发送请求提交失败。" + "<br />";
                            }
                        }
                    }
                }
                ShowMsg(strMsg);
            }
            catch (Exception ex)
            {
                throw (new AspNetException("发送引导数据出现异常，异常原因", ex));
            }
            finally
            {
            }

        }

        /// <summary>
        /// 生成引导数据文件
        /// 1、从引导数据文件中挑出符合筛选时间条件的数据
        /// 2、对这些数据进行参数转换
        /// </summary>
        private string GenYDSJData(out string[] datas)
        {
            datas = null;
            int iIdx = 0;
            string twoSpace = "  ";
            string[] fileDatas;
            YDSJ oData = new YDSJ();
            string msg = string.Empty;
            DateTime from;
            DateTime to;
            #region 获取引导数据文件内容
            try
            {
                iIdx ++;
                from = DateTime.Parse(txtSStartDate.Text);
                iIdx ++;
                to = DateTime.Parse(txtSEndDate.Text);
                oData.Id = Convert.ToInt32(txtId.Text);
                iIdx ++;
                oData = oData.SelectById();

                DataFileHandle oHandle = new DataFileHandle(Path.Combine(oData.FilePath, oData.FileName));
                DateTime ctime;
                string source = string.Empty;
                string target = string.Empty;
                string taskid = string.Empty;
                string infotype = string.Empty;
                int lineCount = 0;
                iIdx ++;
                oHandle.GetDataFileBaseInfo(out ctime, out source, out target, out taskid
                    , out infotype, out lineCount, out fileDatas, out msg);
                if (!msg.Equals(string.Empty))
                    return msg;
            }
            catch (Exception ex)
            {
                throw (new AspNetException(string.Format("发送引导数据-读取引导数据文件Step:{0}出现异常，异常原因", iIdx), ex));
            }
            #endregion

            #region 参数转换计算
            int iTimeZone = 8;
            int cvtType = 16;//瞬时Kepler根数-J2000坐标系
            int[] iEmitPath = DataValidator.GetIntPath(@"D:\Deploy\GDFiles\Convert\");//这个参数在计算的时候必须提供，路径必须存在
            int[] iEmitFile = DataValidator.GetIntPath("Launch.dat");//这个参数在计算的时候必须提供，文件也必须存在
            double[] dblResult = null;
            datas = new string[fileDatas.Length];
            string[] ydsj;
            bool blResult = false;
            DateTime times;
            //判断日期是否符合条件，解析行数据，参数转换
            for (int m = 0; m < fileDatas.Length; m++)
            {
                ydsj = fileDatas[m].Split(new char[] { ' ' });
                if (ydsj.Length != 9)//数据格式非法
                {
                    msg = string.Format("行{0}，引导数据格式非法", m);
                    Logger.GetLogger().Error(msg);
                    return msg;
                }
                else
                {
                    times = GetDTTime(ydsj[1], ydsj[2]);
                    if (times >= from && times <= to)
                    {
                        int[] ym = GetYM(times);
                        double[] data = GetData(ydsj);
                        blResult = ParamConvertor.Instance.ParamConvert(true, true, iTimeZone, cvtType
                            , GetYM(times), (times.Second + times.Millisecond / 1000)
                            , GetData(ydsj), iEmitFile, iEmitPath, out dblResult);
                        if (!blResult)
                        {
                            msg = string.Format("数据行{0}参数转换出现错误", m);
                            ShowMsg(msg);
                            return msg;
                        }
                        datas[m] = times.ToString("yyyyMMdd") + twoSpace
                            + times.ToString("HHmmssffff") + twoSpace
                            + dblResult[0].ToString("f4") + twoSpace + dblResult[1].ToString("f6") + twoSpace
                            + dblResult[2].ToString("f4") + twoSpace + dblResult[3].ToString("f6") + twoSpace
                            + dblResult[4].ToString("f6") + twoSpace + dblResult[5].ToString("f6") + twoSpace;
                    }
                }
            }
            #endregion
            return string.Empty;
        }

        //取消
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            pnlDestination.Visible = false;
            pnlData.Visible = true;
        }

        public override void OnPageLoaded()
        {
            this.PagePermission = "OMPLAN_SpaceTask.View";
            this.ShortTitle = "查看空间机动任务";
            this.SetTitle();
            this.AddJavaScriptInclude("scripts/pages/PlanManage/SpaceTaskList.aspx.js");
        }

        protected void btnHidden_Click(object sender, EventArgs e)
        {
            pnlDestination.Visible = true;
            pnlData.Visible = false;
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

        private int[] GetYM(DateTime date)
        {
            int[] ym = new int[5];
            ym[0] = date.Year;
            ym[1] = date.Month;
            ym[2] = date.Day;
            ym[3] = date.Hour;
            ym[4] = date.Minute;
            return ym;
        }

        private double[] GetData(string[] ydsj)
        {
            double[] data = new double[6];
            data[0] = Convert.ToDouble(ydsj[3]);
            data[1] = Convert.ToDouble(ydsj[4]);
            data[2] = Convert.ToDouble(ydsj[5]);
            data[3] = Convert.ToDouble(ydsj[6]);
            data[4] = Convert.ToDouble(ydsj[7]);
            data[5] = Convert.ToDouble(ydsj[8]);
            return data;
        }

        private void HiddenMsg()
        {
            lblMessage.Visible = false;
            lblMessage.Text = "";
        }

        private void ShowMsg(string msg)
        {
            lblMessage.Visible = true;
            lblMessage.Text = msg;
        }

        private DateTime GetDTTime(string D, string T)
        {
            DateTime dt;
            dt = DateTime.ParseExact(D + " " + T
                , "yyyyMMdd HHmmssffff", System.Globalization.CultureInfo.InvariantCulture);
            return dt;
        }
    }
}
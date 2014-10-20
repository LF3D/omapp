using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using OperatingManagement.WebKernel.Basic;
using OperatingManagement.WebKernel.Route;
using OperatingManagement.Framework.Core;
using OperatingManagement.DataAccessLayer;
using OperatingManagement.Framework;
using OperatingManagement.DataAccessLayer.BusinessManage;
using System.Web.Security;
using System.Data;
using ServicesKernel.File;

namespace OperatingManagement.Web.Views.PlanManage
{
    public partial class OribitalQuantityList : AspNetPage, IRouteContext
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtStartDate.Attributes.Add("readonly", "true");
                txtEndDate.Attributes.Add("readonly", "true");
                //btnSend.Attributes.Add("onclick", "javascript:return confirm('确定要发送所选轨道数据吗?');");
                pnlDestination.Visible = false;
                pnlData.Visible = true;

                pnlAll1.Visible = false;
                pnlAll2.Visible = false;
                BindCheckBoxDestination();
                DefaultSearch();
                //ClientScript.RegisterStartupScript(this.GetType(), "error", "<script type='text/javascript'>hideSelectAll();</script>");

                HideMsg();
            }
            cpPager.PostBackPage += new EventHandler(cpPager_PostBackPage);
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                HideMsg();
                SaveCondition();
                cpPager.CurrentPage = 1;
                BindGridView(true);
            }
            catch (Exception ex)
            {
                throw (new AspNetException("轨道根数列表页面搜索出现异常，异常原因", ex));
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
            { ViewState["_EndDate"] = Convert.ToDateTime( txtEndDate.Text.Trim()).AddDays(1).AddMilliseconds(-1); }
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
           
            //List<GD> listDatas = (new GD()).GetListByDate(startDate, endDate);
            List<GD> listDatas = (new GD()).GetList(startDate, endDate,TaskID, iCode);
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
            List<string> targetList;
            ckbDestination.Items.Clear();
            targetList = FileExchangeConfig.GetTgtListForSending("GDGS");
            foreach (string tgt in targetList)
            {
                ckbDestination.Items.Add(new ListItem(FileExchangeConfig.GetNameForType(tgt), tgt));
            }
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            BindCheckBoxDestination();
        }
        //最终发送
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                PlanFileCreator creater = new PlanFileCreator();
                string SendingFilePaths = "";
                HideMsg();
                foreach (ListItem li in ckbDestination.Items)
                {
                    if (li.Selected)
                    {
                        SendingFilePaths = creater.CreateSendingGDGSFile(txtId.Text, li.Value);

                        XYXSInfo objXYXSInfo = new XYXSInfo();
                        //发送协议
                        CommunicationWays protocl = (CommunicationWays)Convert.ToInt32(rbtProtocl.SelectedValue);
                        //发送方ID （运控中心 YKZX）
                        int senderid = objXYXSInfo.GetIdByAddrMark(System.Configuration.ConfigurationManager.AppSettings["ZXBM"]);
                        //接收方ID 
                        int reveiverid = objXYXSInfo.GetIdByAddrMark(li.Value);
                        //信息类型id
                        int infotypeid = (new InfoType()).GetIDByExMark("GD");
                        bool boolResult = true; //文件发送结果
                        FileSender objSender = new FileSender();
                        string[] filePaths = SendingFilePaths.Split(',');
                        string strResult = string.Empty;
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
                                boolResult = objSender.SendFile(GetFileNameByFilePath(filePaths[i]), GetFilePathByFilePath(filePaths[i]), protocl, senderid, reveiverid, infotypeid, true);
                                if (boolResult)
                                {
                                    lblMessage.Text += GetFileNameByFilePath(filePaths[i]) + " 文件发送请求提交成功。" + "<br />";
                                }
                                else
                                {
                                    lblMessage.Text += GetFileNameByFilePath(filePaths[i]) + " 文件发送请求提交失败。" + "<br />";
                                }
                            }
                        }

                    }//li
                }
                ShowMsg(lblMessage.Text);
                BindGridView(false);
            }
            catch (Exception ex)
            {
                throw (new AspNetException("发送计划出现异常，异常原因", ex));
            }
            finally { }
        }
        //取消
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            pnlDestination.Visible = false;
            pnlData.Visible = true;
        }

        public override void OnPageLoaded()
        {
            this.PagePermission = "OMPLAN_OQ.View";
            this.ShortTitle = "查看卫星轨道根数";
            this.SetTitle();
            this.AddJavaScriptInclude("scripts/pages/PlanManage/OribitalQuantityList.aspx.js");
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect(Request.CurrentExecutionFilePath);
        }

        protected void btnHidden_Click(object sender, EventArgs e)
        {
            pnlDestination.Visible = true;
            pnlData.Visible = false;
        }

        protected void btnReset_Click1(object sender, EventArgs e)
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

        private void HideMsg()
        {
            lblMessage.Text = "";
            lblMessage.Visible = false;
        }

        private void ShowMsg(string msg)
        {
            lblMessage.Text = msg;
            lblMessage.Visible = true;
        }
    }
}
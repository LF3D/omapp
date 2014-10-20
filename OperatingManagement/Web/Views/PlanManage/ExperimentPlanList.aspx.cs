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
using OperatingManagement.DataAccessLayer.PlanManage;
using OperatingManagement.DataAccessLayer.BusinessManage;
using System.Web.Security;
using System.Data;
using ServicesKernel.File;

namespace OperatingManagement.Web.Views.PlanManage
{
    public partial class ExperimentPlanList : AspNetPage, IRouteContext
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtCStartDate.Attributes.Add("readonly", "true");
                txtCEndDate.Attributes.Add("readonly", "true");
                txtJHStartDate.Attributes.Add("readonly", "true");
                txtJHEndDate.Attributes.Add("readonly", "true");
                pnlAll2.Visible = false;

                lblMessage.Text = ""; //文件发送消息清空
                lblMessage.Visible = false;

                DefaultSearch();
            }
            cpPager.PostBackPage += new EventHandler(cpPager_PostBackPage);
        }

        /// <summary>
        /// 默认查询两周内的数据
        /// </summary>
        private void DefaultSearch()
        {
            txtCStartDate.Text = DateTime.Now.AddDays(-6).ToString("yyyy-MM-dd");
            txtCEndDate.Text = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
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
                throw (new AspNetException("实验计划列表页面搜索出现异常，异常原因", ex));
            }
            finally { }
        }

        private void SaveCondition()
        {
            if (string.IsNullOrEmpty(txtCStartDate.Text))
                ViewState["_StartDate"] = null;
            else
                ViewState["_StartDate"] = txtCStartDate.Text.Trim();
            if (string.IsNullOrEmpty(txtCEndDate.Text))
                ViewState["_EndDate"] = null;
            else
                ViewState["_EndDate"] = Convert.ToDateTime(txtCEndDate.Text.Trim()).AddDays(1).AddMilliseconds(-1);


            if (string.IsNullOrEmpty(txtJHStartDate.Text))
                ViewState["_JHStartDate"] = null;
            else
                ViewState["_JHStartDate"] = txtJHStartDate.Text.Trim();
            if (string.IsNullOrEmpty(txtJHEndDate.Text))
                ViewState["_JHEndDate"] = null;
            else
                ViewState["_JHEndDate"] = Convert.ToDateTime(txtJHEndDate.Text.Trim()).AddDays(1).AddMilliseconds(-1);
        }

        //绑定列表
        void BindGridView(bool fromSearch)
        {
            DateTime startDate = new DateTime();
            DateTime endDate = new DateTime();
            DateTime jhStartDate = new DateTime();
            DateTime jhEndDate = new DateTime();
            if (fromSearch)
            {
                if (!string.IsNullOrEmpty(txtCStartDate.Text))
                    startDate = Convert.ToDateTime(txtCStartDate.Text);
                if (!string.IsNullOrEmpty(txtCEndDate.Text))
                    endDate = Convert.ToDateTime(txtCEndDate.Text).AddDays(1).AddMilliseconds(-1); //查询时可查当天

                if (!string.IsNullOrEmpty(txtJHStartDate.Text))
                    jhStartDate = Convert.ToDateTime(txtJHStartDate.Text);
                if (!string.IsNullOrEmpty(txtJHEndDate.Text))
                    jhEndDate = Convert.ToDateTime(txtJHEndDate.Text).AddDays(1).AddMilliseconds(-1);
            }
            else
            {
                if (ViewState["_StartDate"] != null)
                    startDate = Convert.ToDateTime(ViewState["_StartDate"].ToString());
                if (ViewState["_EndDate"] != null)
                    endDate = Convert.ToDateTime(ViewState["_EndDate"].ToString());

                if (ViewState["_JHStartDate"] != null)
                    jhStartDate = Convert.ToDateTime(ViewState["_JHStartDate"].ToString());
                if (ViewState["_JHEndDate"] != null)
                    jhEndDate = Convert.ToDateTime(ViewState["_JHEndDate"].ToString());
            }
            

            List<JH> listDatas= (new JH()).GetSYJHList(startDate, endDate,jhStartDate, jhEndDate);
            cpPager.DataSource = listDatas;
            cpPager.PageSize = this.SiteSetting.PageSize;
            if (listDatas.Count > this.SiteSetting.PageSize)
                cpPager.Visible = true;
            cpPager.BindToControl = rpDatas;
            rpDatas.DataSource = cpPager.DataSourcePaged;
            rpDatas.DataBind();

            if (listDatas.Count > 0)
                pnlAll2.Visible = true;
            else
                pnlAll2.Visible = false;
        }

        protected void cpPager_PostBackPage(object sender, EventArgs e)
        {
            BindGridView(false);
        }

        public override void OnPageLoaded()
        {
            this.PagePermission = "OMPLAN_ExPlan.View";
            this.ShortTitle = "查看试验计划";
            this.SetTitle();
            this.AddJavaScriptInclude("scripts/pages/PlanManage/ExperimentPlanList.aspx.js");
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect(Request.CurrentExecutionFilePath);
        }

        /// <summary>
        /// 最终发送
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                PlanFileCreator creater = new PlanFileCreator();
                string SendingFilePaths = "";
                lblMessage.Text = "";//清空发送信息
                lblMessage.Visible = false;
                string runningMode = rbtMode.SelectedValue;
                string strResult = string.Empty;
                foreach (ListItem li in ckbDestination.Items)
                {
                    if (li.Selected)
                    {
                        SendingFilePaths = creater.CreateSendingSYJHFile(txtId.Text, li.Value, runningMode);

                        XYXSInfo objXYXSInfo = new XYXSInfo();
                        //发送协议
                        CommunicationWays protocl = (CommunicationWays)Convert.ToInt32(rbtProtocl.SelectedValue);
                        //发送方ID （运控中心 YKZX）
                        int senderid = objXYXSInfo.GetIdByAddrMark(System.Configuration.ConfigurationManager.AppSettings["ZXBM"]);
                        //接收方ID 
                        int reveiverid = objXYXSInfo.GetIdByAddrMark(li.Value);
                        //信息类型id
                        int infotypeid = (new InfoType()).GetIDByExMark("SYJH");
                        bool boolResult = true; //文件发送结果
                        FileSender objSender = new FileSender();
                        string[] filePaths = SendingFilePaths.Split(',');
                        if ( string.IsNullOrEmpty(SendingFilePaths) || filePaths.Length <= 0)
                        {
                            lblMessage.Visible = true;
                            lblMessage.Text += " 所选试验不包含发送目标\""+li.Text+"\" 的数据。" + "<br />";
                        }
                        else
                        {
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
                                    lblMessage.Visible = true;
                                    if (boolResult)
                                        lblMessage.Text += GetFileNameByFilePath(filePaths[i]) + " 文件发送请求提交成功。" + "<br />";
                                    else
                                        lblMessage.Text += GetFileNameByFilePath(filePaths[i]) + " 文件发送请求提交失败。" + "<br />";
                                }
                            }//endfor
                        }
                    }//li
                }
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
        }

        protected void btnHidden_Click(object sender, EventArgs e)
        {
            string plantype = "SYJH";
            List<string> targetList;
            ckbDestination.Items.Clear();
            targetList = FileExchangeConfig.GetTgtListForSending(plantype);
            foreach (string tgt in targetList)
            {
                ckbDestination.Items.Add(new ListItem(FileExchangeConfig.GetNameForType(tgt), tgt));
            }

            if (ckbDestination.Items.Count > 0)
            {
                ckbDestination.SelectedIndex = 0;
                btnSubmit.Visible = true;
            }
            else
                btnSubmit.Visible = false;
            ClientScript.RegisterStartupScript(this.GetType(), "pop", "<script type='text/javascript'>showPopSendForm();</script>");
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
    }
}
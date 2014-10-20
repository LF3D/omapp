using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

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
    public partial class PlanList : AspNetPage, IRouteContext
    {
        public override void OnPageLoaded()
        {
            this.PagePermission = "OMPLAN_Plan.View";
            this.ShortTitle = "查询计划";
            this.SetTitle();
            this.AddJavaScriptInclude("scripts/pages/PlanManage/PlanList.aspx.js");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    txtCStartDate.Attributes.Add("readonly", "true");
                    txtCEndDate.Attributes.Add("readonly", "true");
                    txtJHStartDate.Attributes.Add("readonly", "true");
                    txtJHEndDate.Attributes.Add("readonly", "true");

                    pnlAll2.Visible = false;

                    lblMessage.Text = ""; //文件发送消息清空
                    lblMessage.Visible = false;
                    DefaultSearch();
                    //由计划详细页面返回时，重新载入之前的查询结果
                    if (Request.QueryString["startDate"] != null || Request.QueryString["endDate"] != null || Request.QueryString["type"] != null
                        || Request.QueryString["jhStartDate"] != null || Request.QueryString["jhEndDate"] != null)
                    {
                        if (Request.QueryString["startDate"] != null)
                            txtCStartDate.Text = Request.QueryString["startDate"];
                        if (Request.QueryString["endDate"] != null)
                            txtCEndDate.Text = Request.QueryString["endDate"];
                        if (Request.QueryString["jhStartDate"] != null)
                            txtJHStartDate.Text = Request.QueryString["jhStartDate"];
                        if (Request.QueryString["jhEndDate"] != null)
                            txtJHEndDate.Text = Request.QueryString["jhEndDate"];
                        ddlType.SelectedValue = Request.QueryString["type"];
                        btnSearch_Click(new object(), new EventArgs());
                    }
                }
                catch (Exception ex)
                {
                    throw (new AspNetException("计划列表页面初始化出现异常，异常原因", ex));
                }
                finally { }
            }
            cpPager.PostBackPage += new EventHandler(cpPager_PostBackPage);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                lblMessage.Text = ""; //文件发送消息清空
                lblMessage.Visible = false;
                SaveCondition();
                cpPager.CurrentPage = 1;
                BindGridView(true);
            }
            catch (Exception ex)
            {
                throw (new AspNetException("计划列表页面搜索出现异常，异常原因", ex));
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

            ViewState["_PlanType"] = ddlType.SelectedItem.Value;
        }
        /// <summary>
        /// 默认查询一周内（明天到6天前）的数据
        /// </summary>
        private void DefaultSearch()
        {
            txtCStartDate.Text = DateTime.Now.AddDays(-6).ToString("yyyy-MM-dd");
            txtCEndDate.Text = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
        }
        /// <summary>
        /// 绑定数据列表
        /// </summary>
        void BindGridView(bool fromSearch)
        {
            DateTime startDate = new DateTime();
            DateTime endDate = new DateTime();
            DateTime jhStartDate = new DateTime();
            DateTime jhEndDate = new DateTime();
            string planType = null;
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

                if (ddlType.SelectedItem.Value != "0")
                    planType = ddlType.SelectedItem.Value;
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

                if (ViewState["_PlanType"].ToString() != "0")
                    planType = ViewState["_PlanType"].ToString();
            }
            

            List<JH> listDatas = (new JH()).GetJHList(planType, startDate, endDate, jhStartDate, jhEndDate);
            cpPager.DataSource = listDatas;
            cpPager.PageSize = this.SiteSetting.PageSize;
            cpPager.BindToControl = rpDatas;
            rpDatas.DataSource = cpPager.DataSourcePaged;
            rpDatas.DataBind();

            if (listDatas.Count > 0)
                pnlAll2.Visible = true;
            else
                pnlAll2.Visible = false;
        }

        /// <summary>
        /// 翻页控件绑定数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cpPager_PostBackPage(object sender, EventArgs e)
        {
            BindGridView(false);
        }

        /// <summary>
        /// 最终发送，按钮提交发送
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                string plantype = ddlType.SelectedValue;
                PlanFileCreator creater = new PlanFileCreator();
                string SendingFilePaths = "";
                lblMessage.Text = "";//清空发送信息
                lblMessage.Visible = false;
                string runningMode = rbtMode.SelectedValue;
                string targets = string.Empty;
                JH oJH = new JH();

                //应用研究工作计划\DMZ工作计划，自动进行批发送
                if (plantype == "YJJH" || plantype == "GZJH")
                {
                    if (plantype == "YJJH")
                        SendingFilePaths = creater.CreateSendingYJJHFile(txtId.Text, out targets, false);
                    else
                        SendingFilePaths = creater.CreateSendingGZJHFile(txtId.Text, out targets, false);
                    if (!SendingFilePaths.Equals(string.Empty))
                    {
                        string[] filePaths = SendingFilePaths.Split(new char[] { ',' });
                        string[] tgtMarks = targets.Split(new char[] { ',' });
                        for (int i = 0; i < filePaths.Length; i++)
                        {
                            SendFiles(filePaths[i], null, tgtMarks[i]);
                        }
                        oJH.SENDSTATUS = 1;
                        oJH.UpdateSendStatusByIds(txtId.Text);
                    }
                    else
                    {
                        lblMessage.Visible = true;
                        lblMessage.Text = "计划不包含指定发送目标的数据。";
                    }   
                }

                //非YJJH\GZJH才能走到foreach中
                foreach (ListItem li in ckbDestination.Items)
                {
                    if (li.Selected)
                    {
                        switch (plantype)
                        {
                            case "XXXQ":
                                SendingFilePaths = creater.CreateSendingXXXQFile(txtId.Text, li.Value);
                                break;
                            case "DJZYSQ":
                                SendingFilePaths = creater.CreateSendingDJZYSQFile(txtId.Text, li.Value, runningMode);
                                break;
                            case "TYSJ":
                                SendingFilePaths = creater.CreateSendingTYSJFile(txtId.Text, li.Value);
                                break;
                        }
                        SendFiles(SendingFilePaths, li, string.Empty);
                        oJH.SENDSTATUS = 1;
                        oJH.UpdateSendStatusByIds(txtId.Text);   
                    }//li
                }
                BindGridView(false);
            }
            catch (Exception ex)
            {
                throw (new AspNetException("发送计划出现异常，异常原因", ex));
            }
            finally { }
        }

        /// <summary>
        /// 发送文件，应用研究计划自动映射发送目标
        /// </summary>
        /// <param name="sendingFilePaths"></param>
        /// <param name="liTarget"></param>
        /// <param name="targetAddrMark"></param>
        private void SendFiles(string sendingFilePaths, ListItem liTarget, string targetAddrMark)
        {
            string strResult = string.Empty;
            XYXSInfo objXYXSInfo = new XYXSInfo();
            //发送协议
            CommunicationWays protocl = (CommunicationWays)Convert.ToInt32(rbtProtocl.SelectedValue);
            //发送方ID （运控中心 YKZX）
            int senderid = objXYXSInfo.GetIdByAddrMark(System.Configuration.ConfigurationManager.AppSettings["ZXBM"]);
            //接收方ID 
            int reveiverid;
            string targetName = string.Empty;
            if (liTarget != null)
            {
                reveiverid = objXYXSInfo.GetIdByAddrMark(liTarget.Value);
                targetName = liTarget.Text;
            }
            else
            {
                reveiverid = objXYXSInfo.GetByAddrMark(targetAddrMark).Id;
                targetName = objXYXSInfo.GetByAddrMark(targetAddrMark).ADDRName;
            }
            //信息类型id
            int infotypeid = (new InfoType()).GetIDByExMark(txtPlanType.Text);
            bool boolResult = true; //文件发送结果
            FileSender objSender = new FileSender();
            string[] filePaths = sendingFilePaths.Split(',');
            if (string.IsNullOrEmpty(sendingFilePaths) || filePaths.Length <= 0)
            {
                lblMessage.Visible = true;
                lblMessage.Text += " 所选计划不包含发送目标\"" + targetName + "\" 的数据。" + "<br />";
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
        }

        //取消
        protected void btnCancel_Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 弹出发送小窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnHidden_Click(object sender, EventArgs e)
        {
            string plantype = ddlType.SelectedValue;
            if (plantype =="DMJH")
            { plantype = "GZJH"; }
            txtPlanType.Text = plantype;
            switch (plantype)
            {
                case "YJJH":
                    trSendTarget.Visible = false;
                    break;
                case "GZJH":
                    trSendTarget.Visible = false;
                    break;
                default:
                    trSendTarget.Visible = true;
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
                    break;
            }
            BindGridView(true);
            ClientScript.RegisterStartupScript(this.GetType(), "pop", "<script type='text/javascript'>showPopSendForm();</script>");
            
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
            return filepath.Substring(filepath.LastIndexOf("\\")+1);
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

        protected void rpDatas_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    JH oJH = (JH)e.Item.DataItem;
                    HtmlInputCheckBox chkDelete = (e.Item.FindControl("chkDelete") as HtmlInputCheckBox);;
                    chkDelete.Disabled = !GetChkboxStatus(oJH.USESTATUS, oJH.SENDSTATUS);
                    chkDelete.Value = oJH.ID.ToString();
                }
            }
            catch (Exception ex)
            {
                throw (new AspNetException("计划列表数据绑定出现异常，异常原因", ex));
            }
            finally { }

        }

        protected bool GetChkboxStatus(int useStatus, int sendStatus)
        {
            switch (ddlType.SelectedValue)
            {
                case "ZXJH":
                    return false;
                case "DJZYJH":
                    return false;
                default:
                    break;
            }
            if (useStatus == 0)
                return false;
            else if (useStatus == 1)
                return true;
            else
                return false;
        }
    }
}
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
    public partial class PlanTempList : AspNetPage, IRouteContext
    {
        public override void OnPageLoaded()
        {
            this.PagePermission = "OMPLAN_Plan.View";
            this.ShortTitle = "查看初步计划";
            this.SetTitle();
            this.AddJavaScriptInclude("scripts/pages/PlanManage/PlanTempList.aspx.js");
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
                    //由计划页面返回时，重新载入之前的查询结果
                    if (Request.QueryString["startDate"] != null || Request.QueryString["endDate"] != null || Request.QueryString["type"] != null)
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
        /// 默认查询两周内的数据
        /// </summary>
        private void DefaultSearch()
        {
            txtCStartDate.Text = DateTime.Now.AddDays(-6).ToString("yyyy-MM-dd");
            txtCEndDate.Text = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
        }
        /// <summary>
        /// 绑定列表
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
            JH oJH = new JH();
            oJH.isTempJH = true;
            List<JH> listDatas = oJH.GetJHList(planType, startDate, endDate, jhStartDate, jhEndDate);
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

        protected void cpPager_PostBackPage(object sender, EventArgs e)
        {
            BindGridView(false);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect(Request.CurrentExecutionFilePath);
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using OperatingManagement.Framework.Core;
using OperatingManagement.DataAccessLayer.BusinessManage;
using OperatingManagement.WebKernel.Basic;

namespace OperatingManagement.Web.Views.BusinessManage
{
    public partial class CResourceMan : AspNetPage
    {
        #region Properties
        /// <summary>
        /// 查询条件，中心资源状态
        /// </summary>
        protected string ResourceStatus
        {
            get
            {
                if (ViewState["ResourceStatus"] == null)
                {
                    ViewState["ResourceStatus"] = dplResourceStatus.SelectedValue;
                }
                return ViewState["ResourceStatus"].ToString();
            }
            set { ViewState["ResourceStatus"] = value; }
        }
        /// <summary>
        /// 查询条件，开始时间
        /// </summary>
        protected DateTime BeginTime
        {
            get
            {
                DateTime beginTime = DateTime.Now;
                if (ViewState["BeginTime"] == null)
                {
                    ViewState["BeginTime"] = Request.QueryString["begintime"] != null ? Request.QueryString["begintime"] : beginTime.ToString();
                }
                DateTime.TryParse(ViewState["BeginTime"].ToString(), out beginTime);
                return beginTime;
            }
            set { ViewState["BeginTime"] = value; }
        }
        /// <summary>
        /// 结束时间
        /// </summary>
        protected DateTime EndTime
        {
            get
            {
                DateTime endTime = DateTime.Now.AddDays(7);
                if (ViewState["EndTime"] == null)
                {
                    ViewState["EndTime"] = Request.QueryString["endtime"] != null ? Request.QueryString["endtime"] : endTime.ToString();
                }
                DateTime.TryParse(ViewState["EndTime"].ToString(), out endTime);
                return endTime;
            }
            set { ViewState["EndTime"] = value; }
        }

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    txtBeginTime.Attributes.Add("readonly", "true");
                    txtEndTime.Attributes.Add("readonly", "true");
                    BindDataSource();
                    BindRepeater();
                }

                cpCenterResourcePager.PostBackPage += new EventHandler(cpCenterResourcePager_PostBackPage);
            }
            catch (Exception ex)
            {
                throw (new AspNetException("查询中心资源页面初始化出现异常，异常原因", ex));
            }
        }
        /// <summary>
        /// 查询中心资源
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtBeginTime.Text.Trim()))
                {
                    //起始时间不能为空
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alert", "alert(\"起始时间不能为空。\")", true);
                    return;
                }
                if (string.IsNullOrEmpty(txtEndTime.Text.Trim()))
                {
                    //结束时间不能为空
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alert", "alert(\"结束时间不能为空。\")", true);
                    return;
                }
                DateTime beginTime = DateTime.Now;
                DateTime endTime = DateTime.Now.AddDays(7);
                if (!DateTime.TryParse(txtBeginTime.Text, out beginTime))
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alert", "alert(\"起始时间格式错误，请正确输入时间（yyyy-MM-dd）。\")", true);
                    return;
                }
                if (!DateTime.TryParse(txtEndTime.Text, out endTime))
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alert", "alert(\"结束时间格式错误，请正确输入时间（yyyy-MM-dd）。\")", true);
                    return;
                }
                endTime = endTime.AddSeconds(86399.9);//23:59:59
                if (beginTime > endTime)
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alert", "alert(\"结束时间应大于起始时间。\")", true);
                    return;
                }
                ResourceStatus = dplResourceStatus.SelectedValue;
                BeginTime = beginTime;
                EndTime = endTime;
                cpCenterResourcePager.CurrentPage = 1;
                BindRepeater();
            }
            catch (Exception ex)
            {
                throw (new AspNetException("查询中心资源页面btnSearch_Click方法出现异常，异常原因", ex));
            }
        }
        protected void cpCenterResourcePager_PostBackPage(object sender, EventArgs e)
        {
            try
            {
                BindRepeater();
            }
            catch (Exception ex)
            {
                throw (new AspNetException("查询中心资源页面cpCenterResourcePager_PostBackPage方法出现异常，异常原因", ex));
            }
        }
        /// <summary>
        /// 添加中心资源
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string url = @"~/Views/BusinessManage/CenterResourceAdd.aspx";//中心中心资源添加页面
                Response.Redirect(url);
            }
            catch (System.Threading.ThreadAbortException ex1)
            { }
            catch (Exception ex)
            {
                throw (new AspNetException("查询中心资源页面btnAdd_Click方法出现异常，异常原因", ex));
            }
        }
        /// <summary>
        /// 管理中心资源状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnManageResourceStatus_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton lbtnManageResourceStatus = (sender as LinkButton);
                if (lbtnManageResourceStatus == null)
                {
                    BindRepeater();
                    return;
                }
                string resourceCode = lbtnManageResourceStatus.CommandArgument;
                //1-地面站资源；2-通信资源；3-中心资源
                string url = string.Format(@"~/Views/BusinessManage/ResourceStatusManage.aspx?resourcecode={0}&resourcetype={1}", Server.UrlEncode(resourceCode), Server.UrlEncode("3"));

                Response.Redirect(url);
            }
            catch (System.Threading.ThreadAbortException ex1)
            { }
            catch (Exception ex)
            {
                throw (new AspNetException("查询中心资源页面lbtnManageResourceStatus_Click方法出现异常，异常原因", ex));
            }
        }
        /// <summary>
        /// 编辑中心资源
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnEditResource_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton lbtnEdit = (sender as LinkButton);
                if (lbtnEdit == null)
                {
                    BindRepeater();
                    return;
                }

                string url = @"~/Views/BusinessManage/CenterResourceEdit.aspx?crid=" + Server.UrlEncode(lbtnEdit.CommandArgument);//中心中心资源编辑页面
                Response.Redirect(url);
            }
            catch (System.Threading.ThreadAbortException ex1)
            { }
            catch (Exception ex)
            {
                throw (new AspNetException("查询中心资源页面lbtnEditResource_Click方法出现异常，异常原因", ex));
            }
        }
        /// <summary>
        /// 删除中心资源
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnDeleteResource_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton lbtnDelete = (sender as LinkButton);
                if (lbtnDelete == null)
                {
                    BindRepeater();
                    return;
                }
                //中心资源ID
                int id = 0;
                int.TryParse(lbtnDelete.CommandArgument, out id);
                Framework.FieldVerifyResult result = DeleteResource(id);
                switch (result)
                {
                    case Framework.FieldVerifyResult.Error:
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alert", "alert(\"发生了数据错误，无法完成请求的操作。\")", true);
                        break;
                    case Framework.FieldVerifyResult.Success:
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alert", "alert(\"删除中心资源成功。\")", true);
                        BindRepeater();
                        break;
                    default:
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alert", "alert(\"发生了数据错误，无法完成请求的操作。\")", true);
                        BindRepeater();
                        break;
                }
            }
            catch (System.Threading.ThreadAbortException ex1)
            { }
            catch (Exception ex)
            {
                throw (new AspNetException("查询中心资源页面lbtnDeleteResource_Click方法出现异常，异常原因", ex));
            }
        }

        public override void OnPageLoaded()
        {
            this.PagePermission = "OMB_CRes.View";
            this.ShortTitle = "查询中心资源";
            this.SetTitle();
        }

        #region ItemDataBound
        /// <summary>
        /// 中心中心资源单条数据绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rpCenterResourceList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LinkButton lbtnManageResourceStatus = (e.Item.FindControl("lbtnManageResourceStatus") as LinkButton);
                LinkButton lbtnDeleteResource = (e.Item.FindControl("lbtnDeleteResource") as LinkButton);
                if (lbtnManageResourceStatus != null && lbtnDeleteResource != null)
                {
                    CenterResource centerResource = (e.Item.DataItem as CenterResource);
                    if (centerResource.Status == 2)
                    {
                        lbtnManageResourceStatus.Enabled = false;
                        lbtnDeleteResource.OnClientClick = string.Empty;
                        lbtnDeleteResource.Enabled = false;
                    }
                }
            }
        }
        #endregion

        #region Method
        /// <summary>
        /// 绑定控件数据源
        /// </summary>
        private void BindDataSource()
        {
            dplResourceStatus.Items.Clear();
            dplResourceStatus.DataSource = SystemParameters.GetSystemParameters(SystemParametersType.ResourceStatus);
            dplResourceStatus.DataTextField = "key";
            dplResourceStatus.DataValueField = "value";
            dplResourceStatus.DataBind();
            dplResourceStatus.Items.Insert(0, new ListItem("请选择", ""));
            dplResourceStatus.SelectedValue = ResourceStatus;

            //开始时间
            txtBeginTime.Text = BeginTime.ToString("yyyy-MM-dd");
            //结束时间
            txtEndTime.Text = EndTime.ToString("yyyy-MM-dd");
        }
        /// <summary>
        /// 绑定数据源
        /// </summary>
        private void BindRepeater()
        {
            BindCenterResource();
        }
        /// <summary>
        /// 绑定中心中心资源
        /// </summary>
        private void BindCenterResource()
        {
            string resourceStatus = ResourceStatus;

            CenterResource centerResource = new CenterResource();
            List<CenterResource> centerResourceList = centerResource.Search(resourceStatus, BeginTime, EndTime);
            if (centerResourceList.Count > this.SiteSetting.PageSize)
                cpCenterResourcePager.Visible = true;
            cpCenterResourcePager.DataSource = centerResourceList;
            cpCenterResourcePager.PageSize = this.SiteSetting.PageSize;
            cpCenterResourcePager.BindToControl = rpCenterResourceList;
            rpCenterResourceList.DataSource = cpCenterResourcePager.DataSourcePaged;
            rpCenterResourceList.DataBind();
        }
        /// <summary>
        /// 逻辑删除中心资源信息
        /// </summary>
        /// <param name="id">中心资源ID</param>
        /// <param name="resourceType">中心资源类型</param>
        /// <returns>删除结果</returns>
        private Framework.FieldVerifyResult DeleteResource(int id)
        {
            Framework.FieldVerifyResult result = 0;
            CenterResource centerResource = new CenterResource();
            centerResource.Id = id;
            centerResource = centerResource.SelectByID();
            if (centerResource != null)
            {
                centerResource.Status = 2;//删除状态
                centerResource.UpdatedTime = DateTime.Now;
                result = centerResource.Update();
            }
            return result;
        }
        #endregion

    }
}
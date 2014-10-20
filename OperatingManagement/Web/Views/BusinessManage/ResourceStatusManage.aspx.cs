#region
//------------------------------------------------------
//Assembly:OperatingManagement.Web
//FileName:ResourceStatusManage.cs
//Remark:资源状态管理类
//------------------------------------------------------
//VERSION       AUTHOR      DATE        CONTENT
//1.0           liutao      20111015    Create     
//------------------------------------------------------
#endregion
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
    public partial class ResourceStatusManage : AspNetPage
    {
        #region 属性
        /// <summary>
        /// 查询条件，资源类型
        /// 地面站资源=1、通信资源=2、中心资源=3
        /// </summary>
        protected string ResourceType
        {
            get
            {
                if (ViewState["ResourceType"] == null)
                {
                    ViewState["ResourceType"] = Request.QueryString["resourcetype"] != null ? Request.QueryString["resourcetype"] : "1";//默认为地面站资源
                }
                return ViewState["ResourceType"].ToString();
            }
            set { ViewState["ResourceType"] = value; }
        }
        /// <summary>
        ///查询条件， 资源编号
        /// </summary>
        protected string ResourceCode
        {
            get
            {
                if (ViewState["ResourceCode"] == null)
                {
                    ViewState["ResourceCode"] = Request.QueryString["resourcecode"] != null ? Request.QueryString["resourcecode"] : string.Empty;
                }
                return ViewState["ResourceCode"].ToString();
            }
            set { ViewState["ResourceCode"] = value; }
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
                DateTime endTime = DateTime.Now.AddDays(6);
                if (ViewState["EndTime"] == null)
                {
                    ViewState["EndTime"] = Request.QueryString["endtime"] != null ? Request.QueryString["endtime"] : endTime.ToString();
                }
                DateTime.TryParse(ViewState["EndTime"].ToString(), out endTime);
                return endTime;
            }
            set { ViewState["EndTime"] = value; }
        }
        /// <summary>
        /// 查询条件，资源ID
        /// </summary>
        protected int ResourceID
        {
            get
            {
                if (ViewState["ResourceID"] == null)
                {
                    ViewState["ResourceID"] = 0;
                }
                int resourceID = 0;
                int.TryParse(ViewState["ResourceID"].ToString(), out resourceID);
                return resourceID;
            }
            set { ViewState["ResourceID"] = value; }
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
                    //从资源管理页面跳转过来需要绑定健康、占用状态
                    if (!string.IsNullOrEmpty(ResourceCode))
                    {
                        btnSearch_Click(sender, e);
                    }
                }

                cpResourceHealthStatusPager.PostBackPage += new EventHandler(cpResourceHealthStatusPager_PostBackPage);
                cpResourceUseStatusPager.PostBackPage += new EventHandler(cpResourceUseStatusPager_PostBackPage);
            }
            catch (Exception ex)
            {
                throw (new AspNetException("查询资源状态页面初始化出现异常，异常原因", ex));
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cpResourceHealthStatusPager_PostBackPage(object sender, EventArgs e)
        {
            BindResourceStatusList();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cpResourceUseStatusPager_PostBackPage(object sender, EventArgs e)
        {
            BindResourceStatusList();
        }
        /// <summary>
        /// 查询资源状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string resourceType = dplResourceType.SelectedValue;
                if (string.IsNullOrEmpty(txtResourceCode.Text.Trim()))
                {
                    //资源编号不能为空
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alert", "alert(\"资源编号不能为空。\")", true);
                    return;
                }
                int resourceID = GetResourceID(resourceType, txtResourceCode.Text.Trim());
                if (resourceID < 1)
                {
                    //资源不存在
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alert", "alert(\"资源不存在，请确认输入的资源编号是否正确。\")", true);
                    return;
                }
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
                ResourceType = dplResourceType.SelectedValue;
                ResourceCode = txtResourceCode.Text.Trim();
                BeginTime = beginTime;
                EndTime = endTime;
                ResourceID = resourceID;
                cpResourceHealthStatusPager.CurrentPage = 1;
                cpResourceUseStatusPager.CurrentPage = 1;
                BindResourceStatusList();
            }
            catch (Exception ex)
            {
                throw (new AspNetException("查询资源状态页面btnSearch_Click方法出现异常，异常原因", ex));
            }
        }
        /// <summary>
        /// 查看资源状态图形
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnViewChart_Click(object sender, EventArgs e)
        {
            try
            {
                string url = @"~/Views/BusinessManage/ResourceStatusChartManage.aspx?resourcetype={0}&resourcecode={1}&begintime={2}&endtime={3}";
                url = string.Format(url, Server.UrlEncode(ResourceType), Server.UrlEncode(ResourceCode), Server.UrlEncode(BeginTime.ToString()), Server.UrlEncode(EndTime.ToString()));
                Response.Redirect(url);
            }
            catch (System.Threading.ThreadAbortException ex1)
            { 
            }
            catch (Exception ex)
            {
                throw (new AspNetException("查询资源状态页面btnViewChart_Click方法出现异常，异常原因", ex));
            }
        }
        /// <summary>
        /// 添加资源状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string url = @"~/Views/BusinessManage/ResourceStatusAdd.aspx?resourcetype={0}&resourcecode={1}";
                url = string.Format(url, Server.UrlEncode(ResourceType), Server.UrlEncode(ResourceCode));
                Response.Redirect(url);
            }
            catch (System.Threading.ThreadAbortException ex1)
            {
            }
            catch (Exception ex)
            {
                throw (new AspNetException("查询资源状态页面btnAdd_Click方法出现异常，异常原因", ex));
            }
        }
        /// <summary>
        /// 编辑资源状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnEditResourceStatus_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton lbtnEdit = (sender as LinkButton);
                if (lbtnEdit == null)
                {
                    BindResourceStatusList();
                    return;
                }
                //状态类型，健康状态=1、占用状态=2
                string statusType = lbtnEdit.CommandName;
                string statusID = lbtnEdit.CommandArgument;

                string url = @"~/Views/BusinessManage/ResourceStatusEdit.aspx?statustype={0}&statusid={1}";
                url = string.Format(url, Server.UrlEncode(statusType), Server.UrlEncode(statusID));
                Response.Redirect(url);
            }
            catch (System.Threading.ThreadAbortException ex1)
            { }
            catch (Exception ex)
            {
                throw (new AspNetException("查询资源状态页面lbtnEditResource_Click方法出现异常，异常原因", ex));
            }
        }
        public override void OnPageLoaded()
        {
            this.PagePermission = "OMB_ResStaMan.View";
            this.ShortTitle = "查询资源状态";
            this.SetTitle();
        }

        #region Method
        /// <summary>
        /// 绑定控件数据源
        /// </summary>
        private void BindDataSource()
        {
            //资源管理资源类型列表：地面站资源=1、通信资源=2、中心资源=3
            dplResourceType.Items.Clear();
            dplResourceType.DataSource = SystemParameters.GetSystemParameters(SystemParametersType.ResourceType);
            dplResourceType.DataTextField = "key";
            dplResourceType.DataValueField = "value";
            dplResourceType.DataBind();
            dplResourceType.SelectedValue = ResourceType;

            //资源编号
            txtResourceCode.Text = ResourceCode;

            //开始时间
            txtBeginTime.Text = BeginTime.ToString("yyyy-MM-dd");
            //结束时间
            txtEndTime.Text = EndTime.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// 绑定资源状态列表
        /// </summary>
        private void BindResourceStatusList()
        {
            int resourceType = 0;
            int.TryParse(ResourceType, out resourceType);
            //查询健康状态
            HealthStatus healthStatus = new HealthStatus();
            List<HealthStatus> healthStatusList = healthStatus.Search(resourceType, ResourceID, BeginTime, EndTime);
            if (healthStatusList.Count > this.SiteSetting.PageSize)
                cpResourceHealthStatusPager.Visible = true;
            cpResourceHealthStatusPager.DataSource = healthStatusList;
            cpResourceHealthStatusPager.PageSize = this.SiteSetting.PageSize;
            cpResourceHealthStatusPager.BindToControl = rpResourceHealthStatusList;
            rpResourceHealthStatusList.DataSource = cpResourceHealthStatusPager.DataSourcePaged;
            rpResourceHealthStatusList.DataBind();

            //查询占用状态
            UseStatus useStatus = new UseStatus();
            List<UseStatus> useStatusList = useStatus.Search(resourceType, ResourceID, BeginTime, EndTime);
            if (useStatusList.Count > this.SiteSetting.PageSize)
                cpResourceUseStatusPager.Visible = true;
            cpResourceUseStatusPager.DataSource = useStatusList;
            cpResourceUseStatusPager.PageSize = this.SiteSetting.PageSize;
            cpResourceUseStatusPager.BindToControl = rpResourceUseStatusList;
            rpResourceUseStatusList.DataSource = cpResourceUseStatusPager.DataSourcePaged;
            rpResourceUseStatusList.DataBind();
        }
        /// <summary>
        /// 获得资源ID
        /// </summary>
        /// <param name="resourceType">资源类型</param>
        /// <param name="resourceCode">资源编号</param>
        /// <returns>资源ID</returns>
        private int GetResourceID(string resourceType, string resourceCode)
        {
            int resourceID = 0;
            switch (resourceType)
            {
                //地面站资源
                case "1":
                    GroundResource groundResource = new GroundResource();
                    groundResource.EquipmentCode = resourceCode;
                    groundResource = groundResource.SelectByEquipmentCode();
                    if (groundResource != null && groundResource.Id > 0)
                        resourceID = groundResource.Id;
                    break;
                //通信资源
                case "2":
                    CommunicationResource communicationResource = new CommunicationResource();
                    communicationResource.RouteCode = resourceCode;
                    communicationResource = communicationResource.SelectByCode();
                    if (communicationResource != null && communicationResource.Id > 0)
                        resourceID = communicationResource.Id;
                    break;
                //中心资源
                case "3":
                    CenterResource centerResource = new CenterResource();
                    centerResource.EquipmentCode = resourceCode;
                    centerResource = centerResource.SelectByCode();
                    if (centerResource != null && centerResource.Id > 0)
                        resourceID = centerResource.Id;
                    break;
            }
            return resourceID;
        }

        #endregion
    }
}
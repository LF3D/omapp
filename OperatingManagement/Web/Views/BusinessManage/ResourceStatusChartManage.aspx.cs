#region
//------------------------------------------------------
//Assembly:OperatingManagement.Web
//FileName:ResourceStatusChartManage.cs
//Remark:资源状态管理类
//------------------------------------------------------
//VERSION       AUTHOR      DATE        CONTENT
//1.0           liutao      20120204    Create     
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
    public partial class ResourceStatusChartManage : AspNetPage
    {
        #region 属性
        /// <summary>
        /// 资源类型
        /// </summary>
        protected int ResourceType
        {
            get
            {
                //默认地面站资源
                int resourceType = 1;
                if (Request.QueryString["resourcetype"] != null)
                {
                    int.TryParse(Request.QueryString["resourcetype"], out resourceType);
                }
                return resourceType;
            }
        }
        /// <summary>
        /// 资源编号
        /// </summary>
        protected string ResourceCode
        {
            get
            {
                string resourceCode = string.Empty;
                if (Request.QueryString["resourcecode"] != null)
                {
                    resourceCode = Request.QueryString["resourcecode"];
                }
                return resourceCode;
            }
        }
        /// <summary>
        /// 开始时间
        /// </summary>
        protected DateTime BeginTime
        {
            get
            {
                DateTime beginTime = DateTime.Now;
                if (Request.QueryString["begintime"] != null)
                {
                    DateTime.TryParse(Request.QueryString["begintime"], out beginTime);
                }
                return beginTime;
            }
        }
        /// <summary>
        /// 结束时间
        /// </summary>
        protected DateTime EndTime
        {
            get
            {
                DateTime endTime = DateTime.Now.AddDays(6);
                if (Request.QueryString["endtime"] != null)
                {
                    DateTime.TryParse(Request.QueryString["endtime"], out endTime);
                }
                return endTime;
            }
        }
        /// <summary>
        /// 健康状态列表
        /// </summary>
        protected List<HealthStatus> _healthStatusList = null;
        protected List<HealthStatus> HealthStatusList
        {
            get { return _healthStatusList; }

            set { _healthStatusList = value; }
        }
        /// <summary>
        /// 占用状态列表
        /// </summary>
        protected List<UseStatus> _useStatusList = null;
        protected List<UseStatus> UseStatusList
        {
            get { return _useStatusList; }

            set { _useStatusList = value; }
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
                        divOneResourceStatus.Visible = true;
                        divAllResourceStatus.Visible = false;
                        BindOneResourceStatusChart();
                    }
                }
            }
            catch (Exception ex)
            {
                throw (new AspNetException("查询资源状态分布图页面初始化出现异常，异常原因", ex));
            }
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
                //查询所有资源
                if (string.IsNullOrEmpty(txtResourceCode.Text.Trim()))
                {
                    divOneResourceStatus.Visible = false;
                    divAllResourceStatus.Visible = true;
                    BindAllResourceStatusChart();
                }
                //查询某个资源
                else
                {
                    divOneResourceStatus.Visible = true;
                    divAllResourceStatus.Visible = false;
                    BindOneResourceStatusChart();
                }
            }
            catch (Exception ex)
            {
                throw (new AspNetException("查询资源状态分布图页面btnSearch_Click方法出现异常，异常原因", ex));
            }
        }
        /// <summary>
        /// 返回到资源状态管理页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnReturn_Click(object sender, EventArgs e)
        {
            try
            {
                string url = @"~/Views/BusinessManage/ResourceStatusManage.aspx?resourcetype={0}&resourcecode={1}&begintime={2}&endtime={3}";
                url = string.Format(url, Server.UrlEncode(dplResourceType.SelectedValue), Server.UrlEncode(txtResourceCode.Text.Trim()), Server.UrlEncode(txtBeginTime.Text), Server.UrlEncode(txtEndTime.Text));
                Response.Redirect(url);
            }
            catch (System.Threading.ThreadAbortException ex1)
            { }
            catch (Exception ex)
            {
                throw (new AspNetException("查询资源状态分布图页面btnReturn_Click方法出现异常，异常原因", ex));
            }
        }

        protected void chartOneResourceStatus_PreRender(object sender, EventArgs e)
        {
            ChartPreRender(chartOneResourceStatus);
        }

        protected void chartAllResourceStatus_PreRender(object sender, EventArgs e)
        {
            ChartPreRender(chartAllResourceStatus);
        }

        private void ChartPreRender(System.Web.UI.DataVisualization.Charting.Chart ctrl)
        {
            try
            {
                string resourceType = string.Empty;
                string resourceName = string.Empty;
                string resourceCode = string.Empty;
                string functionType = string.Empty;
                string healthStatus = string.Empty;
                string beginTime = string.Empty;
                string endTime = string.Empty;
                string usedBy = string.Empty;
                string usedCategory = string.Empty;
                string usedFor = string.Empty;
                string canBeUsed = string.Empty;
                string usedType = string.Empty;

                string healthStatusStr = @"资源类型：{0}，资源名称：{1}，资源编码：{2}，{3}健康状态：{4}，开始时间：{5}，结束时间：{6}";
                for (int i = 0; i < ctrl.Series["seriesHealthStatus"].Points.Count; i++)
                {
                    resourceType = SystemParameters.GetSystemParameterText(SystemParametersType.ResourceType, HealthStatusList[i].ResourceType.ToString());
                    resourceName = HealthStatusList[i].ResourceName;
                    resourceCode = HealthStatusList[i].ResourceCode;
                    functionType = HealthStatusList[i].ResourceType.ToString() == "1" ? ("功能类型：" + SystemParameters.GetSystemParameterText(SystemParametersType.HealthStatusFunctionType, HealthStatusList[i].FunctionType) + "，") : "";
                    healthStatus = SystemParameters.GetSystemParameterText(SystemParametersType.HealthStatus, HealthStatusList[i].Status.ToString());
                    beginTime = HealthStatusList[i].BeginTime.ToString("yyyy-MM-dd HH:mm:ss");
                    endTime = HealthStatusList[i].EndTime.ToString("yyyy-MM-dd HH:mm:ss");
                    ctrl.Series["seriesHealthStatus"].Points[i].Label = HealthStatusList[i].ResourceName;
                    ctrl.Series["seriesHealthStatus"].Points[i].ToolTip = string.Format(healthStatusStr, resourceType, resourceName, resourceCode, functionType, healthStatus, beginTime, endTime);
                }

                string useStatusStr = @"资源类型：{0}，资源名称：{1}，资源编码：{2}，占用类型：{3}，开始时间：{4}，结束时间：{5}{6}{7}{8}{9}";
                for (int i = 0; i < ctrl.Series["seriesUseStatus"].Points.Count; i++)
                {
                    resourceType = SystemParameters.GetSystemParameterText(SystemParametersType.ResourceType, UseStatusList[i].ResourceType.ToString());
                    resourceName = UseStatusList[i].ResourceName;
                    resourceCode = UseStatusList[i].ResourceCode;
                    usedType = SystemParameters.GetSystemParameterText(SystemParametersType.UseStatusUsedType, UseStatusList[i].UsedType.ToString());
                    beginTime = UseStatusList[i].BeginTime.ToString("yyyy-MM-dd HH:mm:ss");
                    endTime = UseStatusList[i].EndTime.ToString("yyyy-MM-dd HH:mm:ss");
                    usedBy = UseStatusList[i].UsedType == 1 ? "，服务对象：" + UseStatusList[i].UsedBy : "";
                    usedCategory = UseStatusList[i].UsedType == 1 ? "，服务种类：" + UseStatusList[i].UsedCategory : "";
                    usedFor = UseStatusList[i].UsedType == 3 ? "，占用原因：" + UseStatusList[i].UsedFor : "";
                    canBeUsed = UseStatusList[i].UsedType == 3 ? "，是否可执行任务：" + SystemParameters.GetSystemParameterText(SystemParametersType.UseStatusCanBeUsed, UseStatusList[i].CanBeUsed.ToString()) : "";
                    ctrl.Series["seriesUseStatus"].Points[i].Label = UseStatusList[i].ResourceName;
                    ctrl.Series["seriesUseStatus"].Points[i].ToolTip = string.Format(useStatusStr, resourceType, resourceName, resourceCode, usedType, beginTime, endTime, usedBy, usedCategory, usedFor, canBeUsed);
                }
            }
            catch (Exception ex)
            {
                throw (new AspNetException(string.Format("查询资源状态分布图页面{0}_PreRender方法出现异常", ctrl.ID), ex));
            }
        }

        public override void OnPageLoaded()
        {
            this.PagePermission = "OMB_ResStaMan.View";
            this.ShortTitle = "查询资源状态分布图";
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
            dplResourceType.SelectedValue = ResourceType.ToString();

            //资源编号
            txtResourceCode.Text = ResourceCode;

            //开始时间
            txtBeginTime.Text = BeginTime.ToString("yyyy-MM-dd");
            //结束时间
            txtEndTime.Text = EndTime.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// 绑定一个资源状态列表
        /// </summary>
        private void BindOneResourceStatusChart()
        {
            bool validateResult = true;
            int resourceType = 1;
            int.TryParse(dplResourceType.SelectedValue, out resourceType);
            if (validateResult && string.IsNullOrEmpty(txtResourceCode.Text.Trim()))
            {
                //资源编号不能为空
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alert", "alert(\"资源编号不能为空。\")", true);
                validateResult = false;
            }
            int resourceID = GetResourceID(resourceType, txtResourceCode.Text.Trim());
            if (validateResult && resourceID < 1)
            {
                //资源不存在
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alert", "alert(\"资源不存在，请确认输入的资源编号是否正确。\")", true);
                validateResult = false;
            }
            if (validateResult && string.IsNullOrEmpty(txtBeginTime.Text.Trim()))
            {
                //起始时间不能为空
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alert", "alert(\"起始时间不能为空。\")", true);
                validateResult = false;
            }
            if (validateResult && string.IsNullOrEmpty(txtEndTime.Text.Trim()))
            {
                //结束时间不能为空
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alert", "alert(\"结束时间不能为空。\")", true);
                validateResult = false;
            }
            DateTime beginTime = DateTime.Now;
            DateTime endTime = DateTime.Now.AddDays(7);
            if (validateResult && !DateTime.TryParse(txtBeginTime.Text, out beginTime))
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alert", "alert(\"起始时间格式错误，请正确输入时间（yyyy-MM-dd）。\")", true);
                validateResult = false;
            }
            if (validateResult && !DateTime.TryParse(txtEndTime.Text, out endTime))
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alert", "alert(\"结束时间格式错误，请正确输入时间（yyyy-MM-dd）。\")", true);
                validateResult = false;
            }
            endTime = endTime.AddSeconds(86399.9);//23:59:59
            if (validateResult && beginTime > endTime)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alert", "alert(\"结束时间应大于起始时间。\")", true);
                validateResult = false;
            }

            //查询健康状态
            HealthStatus healthStatus = new HealthStatus();
            HealthStatusList = validateResult ? healthStatus.Search(resourceType, resourceID, beginTime, endTime) : new List<HealthStatus>();
            //chartOneResourceStatus.Series["seriesHealthStatus"].Points.DataBindXY(HealthStatusList, "ResourceName", HealthStatusList, "BeginTime,EndTime");
            chartOneResourceStatus.Series["seriesHealthStatus"].Points.DataBindY(HealthStatusList, "BeginTime,EndTime");

            //查询占用状态
            UseStatus useStatus = new UseStatus();
            UseStatusList = validateResult ? useStatus.Search(resourceType, resourceID, beginTime, endTime) : new List<UseStatus>();
            //chartOneResourceStatus.Series["seriesUseStatus"].Points.DataBindXY(UseStatusList, "ResourceName", UseStatusList, "BeginTime,EndTime");
            chartOneResourceStatus.Series["seriesUseStatus"].Points.DataBindY(UseStatusList, "BeginTime,EndTime");
        }
        /// <summary>
        /// 绑定全部资源状态列表
        /// </summary>
        private void BindAllResourceStatusChart()
        {
            int resourceType = 1;
            int.TryParse(dplResourceType.SelectedValue, out resourceType);
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

            //查询健康状态
            HealthStatus healthStatus = new HealthStatus();
            HealthStatusList = healthStatus.Search(resourceType, 0, beginTime, endTime);
            //chartOneResourceStatus.Series["seriesHealthStatus"].Points.DataBindXY(HealthStatusList, "ResourceName", HealthStatusList, "BeginTime,EndTime");
            chartAllResourceStatus.Series["seriesHealthStatus"].Points.DataBindY(HealthStatusList, "BeginTime,EndTime");

            //查询占用状态
            UseStatus useStatus = new UseStatus();
            UseStatusList = useStatus.Search(resourceType, 0, beginTime, endTime);
            //chartOneResourceStatus.Series["seriesUseStatus"].Points.DataBindXY(UseStatusList, "ResourceName", UseStatusList, "BeginTime,EndTime");
            chartAllResourceStatus.Series["seriesUseStatus"].Points.DataBindY(UseStatusList, "BeginTime,EndTime");
        }
        /// <summary>
        /// 获得资源ID
        /// </summary>
        /// <param name="resourceType">资源类型</param>
        /// <param name="resourceCode">资源编号</param>
        /// <returns>资源ID</returns>
        private int GetResourceID(int resourceType, string resourceCode)
        {
            int resourceID = 0;
            switch (resourceType)
            {
                //地面站资源
                case 1:
                    GroundResource groundResource = new GroundResource();
                    groundResource.EquipmentCode = resourceCode;
                    groundResource = groundResource.SelectByEquipmentCode();
                    if (groundResource != null && groundResource.Id > 0)
                        resourceID = groundResource.Id;
                    break;
                //通信资源
                case 2:
                    CommunicationResource communicationResource = new CommunicationResource();
                    communicationResource.RouteCode = resourceCode;
                    communicationResource = communicationResource.SelectByCode();
                    if (communicationResource != null && communicationResource.Id > 0)
                        resourceID = communicationResource.Id;
                    break;
                //中心资源
                case 3:
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
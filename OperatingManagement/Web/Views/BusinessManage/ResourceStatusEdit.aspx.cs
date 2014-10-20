#region
//------------------------------------------------------
//Assembly:OperatingManagement.Web
//FileName:ResourceStatusEdit.cs
//Remark:资源状态编辑类
//------------------------------------------------------
//VERSION       AUTHOR      DATE        CONTENT
//1.0           liutao      20120829    Create     
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
    public partial class ResourceStatusEdit : AspNetPage
    {
        #region 属性
        /// <summary>
        ///状态类型
        ///健康状态=1、占用状态=2
        /// </summary>
        protected string StatusType
        {
            get
            {
                string statusType = string.Empty;
                if (Request.QueryString["statustype"] != null)
                {
                    statusType = Request.QueryString["statustype"];
                }
                return statusType;
            }
        }
        /// <summary>
        /// 资源状态ID
        /// StatusType=1,StatusID为资源健康状态ID
        /// StatusType=2,StatusID为资源占用状态ID
        /// </summary>
        protected int StatusID
        {
            get
            {
                int statusID = 0;
                if (Request.QueryString["statusid"] != null)
                {
                    int.TryParse(Request.QueryString["statusid"], out statusID);
                }
                return statusID;
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                trMessage.Visible = false;
                lblMessage.Text = string.Empty;
                if (!IsPostBack)
                {
                    txtBeginTime.Attributes.Add("readonly", "true");
                    txtEndTime.Attributes.Add("readonly", "true");
                    BindDataSource();
                    BindControls();
                }
            }
            catch (Exception ex)
            {
                trMessage.Visible = true;
                lblMessage.Text = "发生未知错误，操作失败。";
                throw (new AspNetException("编辑资源状态页面初始化出现异常，异常原因", ex));
            }
        }
        /// <summary>
        /// 提交添加资源状态记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton lbtn = (sender as LinkButton);
                string msg = string.Empty;
                //资源管理资源类型列表：地面站资源=1、通信资源=2、中心资源=3
                //int resourceType = 0;
                //int.TryParse(dplResourceType.SelectedValue, out resourceType);
                if (string.IsNullOrEmpty(txtResourceCode.Text.Trim()))
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "资源编号不能为空";
                    return;
                }

                if (trHealthStatusFunctionType.Visible && string.IsNullOrEmpty(dplFunctionType.SelectedValue))
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "请选择功能类型";
                    return;
                }

                if (trHealthStatus.Visible && string.IsNullOrEmpty(dplHealthStatus.SelectedValue))
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "请选择健康状态";
                    return;
                }

                if (trUseStatusUsedType.Visible && string.IsNullOrEmpty(dplUsedType.SelectedValue))
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "请选择占用类型";
                    return;
                }

                DateTime beginTime = DateTime.Now;
                DateTime endTime = DateTime.Now;
                if (!DateTime.TryParse(FormatDateTimeString(txtBeginTime.Text.Trim()), out beginTime))
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "起始时间格式错误";
                    return;
                }
                //beginTime = beginTime.AddHours(Convert.ToDouble(dplBeginTimeHour.SelectedValue));
                //beginTime = beginTime.AddMinutes(Convert.ToDouble(dplBeginTimeMinute.SelectedValue));
                if (!DateTime.TryParse(FormatDateTimeString(txtEndTime.Text.Trim()), out endTime))
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "结束时间格式错误";
                    return;
                }
                //endTime = endTime.AddHours(Convert.ToDouble(dplEndTimeHour.SelectedValue));
                //endTime = endTime.AddMinutes(Convert.ToDouble(dplEndTimeMinute.SelectedValue));
                if (beginTime > endTime)
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "起始时间应小于结束时间";
                    return;
                }

                if (trUseStatusUsedBy.Visible && string.IsNullOrEmpty(txtUsedBy.Text.Trim()))
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "服务对象不能为空";
                    return;
                }

                if (trUseStatusUsedCategory.Visible && string.IsNullOrEmpty(txtUsedCategory.Text.Trim()))
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "服务种类不能为空";
                    return;
                }

                if (trUseStatusUsedFor.Visible && string.IsNullOrEmpty(txtUsedFor.Text.Trim()))
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "占用原因不能为空";
                    return;
                }

                if (trUseStatusCanBeUsed.Visible && string.IsNullOrEmpty(dplCanBeUsed.SelectedValue))
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "请选择是否可执行任务";
                    return;
                }
                //状态类型列表：健康状态=1、占用状态=2
                if (StatusType == "1")
                {
                    Framework.FieldVerifyResult result;
                    HealthStatus healthStatus = new HealthStatus();
                    healthStatus.Id = StatusID;
                    healthStatus = healthStatus.SelectByID();
                    if (healthStatus == null)
                    {
                        trMessage.Visible = true;
                        lblMessage.Text = "编辑的资源健康状态不存在，操作失败。";
                        return;
                    }
                    string resourceCode = GetResourceCode(healthStatus.ResourceType, healthStatus.ResourceID);
                    if (string.IsNullOrEmpty(resourceCode))
                    {
                        trMessage.Visible = true;
                        lblMessage.Text = "编辑的资源不存在，操作失败。";
                        return;
                    }
                    //healthStatus.ResourceID = healthStatus.ResourceID;
                    //healthStatus.ResourceType = healthStatus.ResourceType;
                    healthStatus.FunctionType = trHealthStatusFunctionType.Visible ? dplFunctionType.SelectedValue : string.Empty;
                    healthStatus.Status = Convert.ToInt32(dplHealthStatus.SelectedValue);
                    healthStatus.BeginTime = beginTime;
                    healthStatus.EndTime = endTime;
                    //healthStatus.CreatedTime = DateTime.Now;
                    //healthStatus.CreatedUserID = LoginUserInfo.Id;
                    healthStatus.UpdatedTime = DateTime.Now;
                    healthStatus.UpdatedUserID = LoginUserInfo.Id;

                    if (lbtn == null && healthStatus.HaveEffectiveHealthStatus())
                    {
                        msg = string.Format("该资源{0}存在重叠时间段健康异常，请修改后提交。", trHealthStatusFunctionType.Visible ? "在" + dplFunctionType.SelectedItem.Text + "功能类型下" : string.Empty);
                        trMessage.Visible = true;
                        lblMessage.Text = msg;
                        //lbtnReSubmit.Visible = true;
                        return;
                    }

                    result = healthStatus.Update();

                    switch (result)
                    {
                        case Framework.FieldVerifyResult.Error:
                            msg = "发生了数据错误，无法完成请求的操作。";
                            break;
                        case Framework.FieldVerifyResult.Success:
                            msg = "编辑健康状态成功。";
                            BindControls();
                            break;
                        default:
                            msg = "发生未知错误，操作失败。";
                            break;
                    }
                }
                //状态类型列表：健康状态=1、占用状态=2
                else if (dplStatusType.SelectedValue == "2")
                {
                    Framework.FieldVerifyResult result;
                    UseStatus useStatus = new UseStatus();
                    useStatus.Id = StatusID;
                    useStatus = useStatus.SelectByID();
                    if (useStatus == null)
                    {
                        trMessage.Visible = true;
                        lblMessage.Text = "编辑的资源占用状态不存在，操作失败。";
                        return;
                    }
                    string resourceCode = GetResourceCode(useStatus.ResourceType, useStatus.ResourceID);
                    if (string.IsNullOrEmpty(resourceCode))
                    {
                        trMessage.Visible = true;
                        lblMessage.Text = "编辑的资源不存在，操作失败。";
                        return;
                    }
                    //useStatus.ResourceID = useStatus.ResourceID;
                    //useStatus.ResourceType = resourceType;
                    useStatus.UsedType = trUseStatusUsedType.Visible ? Convert.ToInt32(dplUsedType.SelectedValue) : 0;
                    useStatus.BeginTime = beginTime;
                    useStatus.EndTime = endTime;
                    useStatus.UsedBy = trUseStatusUsedBy.Visible ? txtUsedBy.Text : string.Empty;
                    useStatus.UsedCategory = trUseStatusUsedCategory.Visible ? txtUsedCategory.Text : string.Empty;
                    useStatus.UsedFor = trUseStatusUsedFor.Visible ? txtUsedFor.Text : string.Empty;
                    useStatus.CanBeUsed = trUseStatusCanBeUsed.Visible ? Convert.ToInt32(dplCanBeUsed.SelectedValue) : 0;
                    //useStatus.CreatedTime = DateTime.Now;
                    //useStatus.CreatedUserID = LoginUserInfo.Id;
                    useStatus.UpdatedTime = DateTime.Now;
                    useStatus.UpdatedUserID = LoginUserInfo.Id;

                    if (lbtn == null && useStatus.HaveEffectiveUseStatus())
                    {
                        //msg = "该资源任务占用和其他占用存在重叠时间段，是否继续提交？";
                        msg = string.Format("该资源在{0}类型下存在重叠时间段占用，请修改后提交。", trUseStatusUsedType.Visible ? dplUsedType.SelectedItem.Text : string.Empty);
                        trMessage.Visible = true;
                        lblMessage.Text = msg;
                        //lbtnReSubmit.Visible = true;
                        return;
                    }

                    result = useStatus.Update();

                    switch (result)
                    {
                        case Framework.FieldVerifyResult.Error:
                            msg = "发生了数据错误，无法完成请求的操作。";
                            break;
                        case Framework.FieldVerifyResult.Success:
                            msg = "编辑占用状态成功。";
                            BindControls();
                            break;
                        default:
                            msg = "发生未知错误，操作失败。";
                            break;
                    }
                }
                trMessage.Visible = true;
                lblMessage.Text = msg;
                lbtnReSubmit.Visible = false;
            }
            catch (Exception ex)
            {
                trMessage.Visible = true;
                lblMessage.Text = "发生未知错误，操作失败。";
                throw (new AspNetException("编辑资源状态页面btnSubmit_Click方法出现异常，异常原因", ex));
            }
        }
        /// <summary>
        /// 重置当前控件的值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                BindControls();
            }
            catch (System.Threading.ThreadAbortException ex1)
            { }
            catch (Exception ex)
            {
                trMessage.Visible = true;
                lblMessage.Text = "发生未知错误，操作失败。";
                throw (new AspNetException("编辑资源状态页面btnReset_Click方法出现异常，异常原因", ex));
            }
        }
        protected void btnReturn_Click(object sender, EventArgs e)
        {
            try
            {
                string url = @"~/Views/BusinessManage/ResourceStatusManage.aspx?resourcetype={0}&resourcecode={1}";
                url = string.Format(url, Server.UrlEncode(dplResourceType.SelectedValue), Server.UrlEncode(txtResourceCode.Text.Trim()));
                Response.Redirect(url);
            }
            catch (System.Threading.ThreadAbortException ex1)
            { }
            catch (Exception ex)
            {
                trMessage.Visible = true;
                lblMessage.Text = "发生未知错误，操作失败。";
                throw (new AspNetException("编辑资源状态页面btnReturn_Click方法出现异常，异常原因", ex));
            }
        }
        /// <summary>
        /// 当占用类型发生改变时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dplUsedType_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetControlsVisible();
        }

        public override void OnPageLoaded()
        {
            this.PagePermission = "OMB_ResStaMan.View";
            this.ShortTitle = "编辑资源状态";
            this.SetTitle();
        }
        #region Method
        /// <summary>
        /// 绑定控件数据源
        /// </summary>
        private void BindDataSource()
        {
            //状态类型列表：健康状态=1、占用状态=2
            dplStatusType.Items.Clear();
            dplStatusType.DataSource = SystemParameters.GetSystemParameters(SystemParametersType.StatusType);
            dplStatusType.DataTextField = "key";
            dplStatusType.DataValueField = "value";
            dplStatusType.DataBind();

            dplStatusType.Enabled = false;

            //资源管理资源类型列表：地面站资源=1、通信资源=2、中心资源=3
            dplResourceType.Items.Clear();
            dplResourceType.DataSource = SystemParameters.GetSystemParameters(SystemParametersType.ResourceType);
            dplResourceType.DataTextField = "key";
            dplResourceType.DataValueField = "value";
            dplResourceType.DataBind();
            dplResourceType.Enabled = false;

            txtResourceCode.Enabled = false;

            //健康状态功能类型列表：数传数据接收、遥测数据接收、遥控操作
            dplFunctionType.Items.Clear();
            dplFunctionType.DataSource = SystemParameters.GetSystemParameters(SystemParametersType.HealthStatusFunctionType);
            dplFunctionType.DataTextField = "key";
            dplFunctionType.DataValueField = "value";
            dplFunctionType.DataBind();
            //dplFunctionType.Items.Insert(0, new ListItem("请选择", ""));

            //健康状态列表：正常=1、异常=2
            dplHealthStatus.Items.Clear();
            dplHealthStatus.DataSource = SystemParameters.GetSystemParameters(SystemParametersType.HealthStatus);
            dplHealthStatus.DataTextField = "key";
            dplHealthStatus.DataValueField = "value";
            dplHealthStatus.DataBind();
            //dplHealthStatus.Items.Insert(0, new ListItem("请选择", ""));
            dplHealthStatus.SelectedValue = "2";
            dplHealthStatus.Enabled = false;

            //占用状态占用类型列表：任务占用=1、维护占用=2、其他占用=3
            dplUsedType.Items.Clear();
            dplUsedType.DataSource = SystemParameters.GetSystemParameters(SystemParametersType.UseStatusUsedType);
            dplUsedType.DataTextField = "key";
            dplUsedType.DataValueField = "value";
            dplUsedType.DataBind();
            //dplUsedType.Items.Insert(0, new ListItem("请选择", ""));

            //占用状态是否可执行任务列表：是=1、否=2
            dplCanBeUsed.Items.Clear();
            dplCanBeUsed.DataSource = SystemParameters.GetSystemParameters(SystemParametersType.UseStatusCanBeUsed);
            dplCanBeUsed.DataTextField = "key";
            dplCanBeUsed.DataValueField = "value";
            dplCanBeUsed.DataBind();
            //dplCanBeUsed.Items.Insert(0, new ListItem("请选择", ""));

            dplBeginTimeHour.Items.Clear();
            dplEndTimeHour.Items.Clear();
            for (int i = 0; i < 24; i++)
            {
                dplBeginTimeHour.Items.Add(new ListItem(i.ToString() + "时", i.ToString()));
                dplEndTimeHour.Items.Add(new ListItem(i.ToString() + "时", i.ToString()));
            }
            dplBeginTimeMinute.Items.Clear();
            dplEndTimeMinute.Items.Clear();
            for (int i = 0; i < 60; i++)
            {
                dplBeginTimeMinute.Items.Add(new ListItem(i.ToString() + "分", i.ToString()));
                dplEndTimeMinute.Items.Add(new ListItem(i.ToString() + "分", i.ToString()));
            }
        }
        /// <summary>
        /// 为控件绑定值
        /// </summary>
        private void BindControls()
        {
            dplStatusType.SelectedValue = StatusType;
            if (StatusType == "1")
            {
                HealthStatus healthStatus = new HealthStatus();
                healthStatus.Id = StatusID;
                healthStatus = healthStatus.SelectByID();
                if (healthStatus == null)
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "编辑的资源健康状态不存在，操作失败。";
                    return;
                }
                string resourceCode = GetResourceCode(healthStatus.ResourceType, healthStatus.ResourceID);
                if (string.IsNullOrEmpty(resourceCode))
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "编辑的资源不存在，操作失败。";
                    return;
                }
                dplResourceType.SelectedValue = healthStatus.ResourceType.ToString();
                txtResourceCode.Text = resourceCode;
                dplFunctionType.SelectedValue = healthStatus.FunctionType;
                txtBeginTime.Text = healthStatus.BeginTime.ToString("yyyyMMddHHmmss");
                txtEndTime.Text = healthStatus.EndTime.ToString("yyyyMMddHHmmss");
                lblCreatedTime.Text = healthStatus.CreatedTime.ToString("yyyy-MM-dd HH:mm:ss");
                lblUpdatedTime.Text = healthStatus.UpdatedTime == DateTime.MinValue ? healthStatus.CreatedTime.ToString("yyyy-MM-dd HH:mm:ss") : healthStatus.UpdatedTime.ToString("yyyy-MM-dd HH:mm:ss");
                
            }
            else if (StatusType == "2")
            {
                UseStatus useStatus = new UseStatus();
                useStatus.Id = StatusID;
                useStatus = useStatus.SelectByID();
                if (useStatus == null)
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "编辑的资源占用状态不存在，操作失败。";
                    return;
                }
                string resourceCode = GetResourceCode(useStatus.ResourceType, useStatus.ResourceID);
                if (string.IsNullOrEmpty(resourceCode))
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "编辑的资源不存在，操作失败。";
                    return;
                }
                dplResourceType.SelectedValue = useStatus.ResourceType.ToString();
                txtResourceCode.Text = resourceCode;
                dplUsedType.SelectedValue = useStatus.UsedType.ToString();
                txtBeginTime.Text = useStatus.BeginTime.ToString("yyyyMMddHHmmss");
                txtEndTime.Text = useStatus.EndTime.ToString("yyyyMMddHHmmss");
                txtUsedBy.Text = useStatus.UsedBy;
                txtUsedCategory.Text = useStatus.UsedCategory;
                txtUsedFor.Text = useStatus.UsedFor;
                if (dplCanBeUsed.Items.FindByValue(useStatus.CanBeUsed.ToString()) != null)
                    dplCanBeUsed.SelectedValue = useStatus.CanBeUsed.ToString();
                lblCreatedTime.Text = useStatus.CreatedTime.ToString("yyyy-MM-dd HH:mm:ss");
                lblUpdatedTime.Text = useStatus.UpdatedTime == DateTime.MinValue ? useStatus.CreatedTime.ToString("yyyy-MM-dd HH:mm:ss") : useStatus.UpdatedTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
            SetControlsVisible();
        }
        /// <summary>
        /// 设置控件是否可见
        /// </summary>
        private void SetControlsVisible()
        {
            //状态类型列表：健康状态=1、占用状态=2
            if (dplStatusType.SelectedValue == "1")
            {
                //与健康状态相关控件可见
                trHealthStatusFunctionType.Visible = true;
                trHealthStatus.Visible = true;
                //与占用状态相关控件不可见
                trUseStatusUsedType.Visible = false;
                trUseStatusUsedBy.Visible = false;
                trUseStatusUsedCategory.Visible = false;
                trUseStatusUsedFor.Visible = false;
                trUseStatusCanBeUsed.Visible = false;

                //资源管理资源类型列表：地面站资源=1、通信资源=2、中心资源=3
                if (dplResourceType.SelectedValue != "1")
                {
                    //只有地面站资源有健康状态功能类型列表
                    trHealthStatusFunctionType.Visible = false;
                }
            }
            else if (dplStatusType.SelectedValue == "2")
            {
                //与健康状态相关控件不可见
                trHealthStatusFunctionType.Visible = false;
                trHealthStatus.Visible = false;
                //与占用状态相关控件可见
                trUseStatusUsedType.Visible = true;
                trUseStatusUsedBy.Visible = true;
                trUseStatusUsedCategory.Visible = true;
                trUseStatusUsedFor.Visible = true;
                trUseStatusCanBeUsed.Visible = true;

                //占用状态占用类型列表：任务占用=1、维护占用=2、其他占用=3
                if (dplUsedType.SelectedValue == "1")
                {
                    trUseStatusUsedFor.Visible = false;
                    trUseStatusCanBeUsed.Visible = false;
                }
                else if (dplUsedType.SelectedValue == "2")
                {
                    trUseStatusUsedBy.Visible = false;
                    trUseStatusUsedCategory.Visible = false;
                    trUseStatusUsedFor.Visible = false;
                    trUseStatusCanBeUsed.Visible = false;
                }
                else if (dplUsedType.SelectedValue == "3")
                {
                    trUseStatusUsedBy.Visible = false;
                    trUseStatusUsedCategory.Visible = false;
                }
            }
        }
        /// <summary>
        /// 获得资源编号
        /// </summary>
        /// <param name="resourceType">资源类型</param>
        /// <param name="resourceID">资源ID</param>
        /// <returns>资源编号</returns>
        private string GetResourceCode(int resourceType, int resourceID)
        {
            string resourceCode = string.Empty;
            switch (resourceType)
            {
                //地面站资源
                case 1:
                    GroundResource groundResource = new GroundResource();
                    groundResource.Id = resourceID;
                    groundResource = groundResource.SelectByID();
                    if (groundResource != null && groundResource.Id > 0)
                        resourceCode = groundResource.EquipmentCode;
                    break;
                //通信资源
                case 2:
                    CommunicationResource communicationResource = new CommunicationResource();
                    communicationResource.Id = resourceID;
                    communicationResource = communicationResource.SelectByID();
                    if (communicationResource != null && communicationResource.Id > 0)
                        resourceCode = communicationResource.RouteCode;
                    break;
                //中心资源
                case 3:
                    CenterResource centerResource = new CenterResource();
                    centerResource.Id = resourceID;
                    centerResource = centerResource.SelectByID();
                    if (centerResource != null && centerResource.Id > 0)
                        resourceCode = centerResource.EquipmentCode;
                    break;
            }
            return resourceCode;
        }
        /// <summary>
        /// 将yyyyMMddHHmmss格式字符串转换成yyyy-MM-dd HH:mm:ss
        /// </summary>
        /// <param name="dateTimeString"></param>
        /// <returns></returns>
        protected string FormatDateTimeString(string dateTimeString)
        {
            string result = string.Empty;
            if (!string.IsNullOrEmpty(dateTimeString) && dateTimeString.Length == 14)
            {
                result += dateTimeString.Substring(0, 4) + "-";
                result += dateTimeString.Substring(4, 2) + "-";
                result += dateTimeString.Substring(6, 2) + " ";
                result += dateTimeString.Substring(8, 2) + ":";
                result += dateTimeString.Substring(10, 2) + ":";
                result += dateTimeString.Substring(12, 2);
            }
            return result;
        }
        #endregion
    }
}
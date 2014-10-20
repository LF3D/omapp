#region
//------------------------------------------------------
//Assembly:OperatingManagement.Web
//FileName:ResourceRequirementAdd.cs
//Remark:资源需求添加类
//------------------------------------------------------
//VERSION       AUTHOR      DATE        CONTENT
//1.0           liutao      20120211    Create     
//------------------------------------------------------
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Xml;

using OperatingManagement.Framework.Core;
using OperatingManagement.WebKernel.Route;
using OperatingManagement.DataAccessLayer.BusinessManage;
using OperatingManagement.WebKernel.Basic;
using ServicesKernel;

namespace OperatingManagement.Web.Views.BusinessManage
{
    public partial class ResourceRequirementAdd : AspNetPage, IRouteContext
    {
        #region 属性
        /// <summary>
        /// 资源需求列表缓存Guid，暂时未使用
        /// </summary>
        protected Guid CacheID
        {
            get
            {
                if (ViewState["cacheid"] == null)
                {
                    Guid cacheID = Guid.NewGuid();
                    if (Request.QueryString["cacheid"] != null)
                    {
                        Guid.TryParse(Request.QueryString["cacheid"], out cacheID);
                    }
                    ViewState["cacheid"] = cacheID;
                }
                return new Guid(ViewState["cacheid"].ToString());
            }
        }
        /// <summary>
        /// 资源需求
        /// </summary>
        protected List<ResourceRequirement> ResourceRequirementList
        {
            get
            {
                if (ViewState["ResourceRequirement"] == null)
                {
                    return new List<ResourceRequirement>();
                }
                else
                {
                    return (ViewState["ResourceRequirement"] as List<ResourceRequirement>);
                }
            }
        }
        /// <summary>
        /// 不可用设备
        /// </summary>
        protected List<UnusedEquipment> UnusedEquipmentList
        {
            get
            {
                if (ViewState["UnusedEquipment"] == null)
                {
                    return new List<UnusedEquipment>();
                }
                else
                {
                    return (ViewState["UnusedEquipment"] as List<UnusedEquipment>);
                }
            }
        }
        /// <summary>
        /// 支持时段
        /// </summary>
        protected List<PeriodOfTime> PeriodOfTimeList
        {
            get
            {
                if (ViewState["PeriodOfTime"] == null)
                {
                    return new List<PeriodOfTime>();
                }
                else
                {
                    return (ViewState["PeriodOfTime"] as List<PeriodOfTime>);
                }
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
                    txtTimeBenchmark.Attributes.Add("readonly", "true");
                    BindSatNameDataSource();
                    BindDataSource();
                    BindUnusedEquipmentList();
                    BindPeriodOfTimeList();
                    BindResourceRequirementList();
                    hidWXBMIndex.Value = GetWXBMIndex().ToString();
                    lblRequirementName.Text = GetRequirementName();
                }

                cpUnusedEquipmentPager.PostBackPage += new EventHandler(cpUnusedEquipmentPager_PostBackPage);
                cpPeriodOfTimePager.PostBackPage += new EventHandler(cpPeriodOfTimePager_PostBackPage);
                cpResourceRequirementPager.PostBackPage += new EventHandler(cpResourceRequirementPager_PostBackPage);
            }
            catch (Exception ex)
            {
                trMessage.Visible = true;
                lblMessage.Text = "发生未知错误，操作失败。";
                throw (new AspNetException("新增资源调度计算页面初始化出现异常，异常原因", ex));
            }
        }
        /// <summary>
        /// 提交保存资源需求记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string msg = string.Empty;
                if (string.IsNullOrEmpty(lblRequirementName.Text.Trim()))
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "需求名称不能为空";
                    return;
                }
                if (string.IsNullOrEmpty(txtPriority.Text.Trim()))
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "优先级不能为空";
                    return;
                }
                int priority = 0;
                if (!int.TryParse(txtPriority.Text.Trim(), out priority))
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "优先级格式错误";
                    return;
                }
                if (priority < 1 || priority > 100)
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "优先级应为1-100的整数";
                    return;
                }
                if (string.IsNullOrEmpty(dplSatName.SelectedValue.Trim()))
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "卫星编码不能为空";
                    return;
                }
                if (string.IsNullOrEmpty(dplFunctionType.SelectedValue.Trim()))
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "功能类型不能为空";
                    return;
                }
                if (string.IsNullOrEmpty(txtPersistenceTime.Text.Trim()))
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "持续时长不能为空";
                    return;
                }
                int persistenceTime = 0;
                if (!int.TryParse(txtPersistenceTime.Text.Trim(), out persistenceTime))
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "持续时长格式错误";
                    return;
                }
                if (persistenceTime < 1)
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "持续时长应为大于0的整数";
                    return;
                }
                if (UnusedEquipmentList.Count < 1)
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "不可用设备不能为空";
                    return;
                }
                if (PeriodOfTimeList.Count < 1)
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "支持时段不能为空";
                    return;
                }

                ResourceRequirement resourceRequirement = new ResourceRequirement();
                resourceRequirement.RequirementName = lblRequirementName.Text.Trim();
                resourceRequirement.TimeBenchmark = string.Empty;//txtTimeBenchmark.Text.Trim();
                resourceRequirement.Priority = priority;
                resourceRequirement.WXBM = dplSatName.SelectedValue;
                resourceRequirement.FunctionType = dplFunctionType.SelectedValue;
                resourceRequirement.PersistenceTime = persistenceTime;
                resourceRequirement.UnusedEquipmentList = UnusedEquipmentList;
                resourceRequirement.PeriodOfTimeList = PeriodOfTimeList;
                resourceRequirement.WXBMIndex = Convert.ToInt32(hidWXBMIndex.Value);

                int resourceRequirementIndex = ResourceRequirementList.FindIndex(a => a.RequirementName.ToLower() == lblRequirementName.Text.Trim().ToLower());
                //新增
                if (resourceRequirementIndex < 0)
                {
                    List<ResourceRequirement> resourceRequirementList = ResourceRequirementList;
                    resourceRequirementList.Add(resourceRequirement);
                    ViewState["ResourceRequirement"] = resourceRequirementList;
                    lblMessage.Text = "添加资源需求成功。";
                }
                //修改
                else
                {
                    List<ResourceRequirement> resourceRequirementList = ResourceRequirementList;
                    resourceRequirementList.RemoveAt(resourceRequirementIndex);
                    resourceRequirementList.Insert(resourceRequirementIndex, resourceRequirement);
                    ViewState["ResourceRequirement"] = resourceRequirementList;
                    lblMessage.Text = "编辑资源需求成功。";
                }
                BindResourceRequirementList();
                ResetControls();
                trMessage.Visible = true;
            }
            catch (System.Threading.ThreadAbortException ex1)
            { }
            catch (Exception ex)
            {
                trMessage.Visible = true;
                lblMessage.Text = "发生未知错误，操作失败。";
                throw (new AspNetException("新增资源调度计算页面btnSave_Click方法出现异常，异常原因", ex));
            }
        }

        /// <summary>
        /// 重置控件状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                ViewState["ResourceRequirement"] = null;
                BindResourceRequirementList();
                txtTimeBenchmark.Text = string.Empty;
                dplTimeBenchmarkHour.SelectedIndex = 0;
                dplTimeBenchmarkMinute.SelectedIndex = 0;
                dplTimeBenchmarkSecond.SelectedIndex = 0;
                ResetControls();
            }
            catch (System.Threading.ThreadAbortException ex1)
            { }
            catch (Exception ex)
            {
                trMessage.Visible = true;
                lblMessage.Text = "发生未知错误，操作失败。";
                throw (new AspNetException("新增资源调度计算页面btnReset_Click方法出现异常，异常原因", ex));
            }
        }

        /// <summary>
        /// 提交计算资源需求
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCalculate_Click(object sender, EventArgs e)
        {
            string xmlStr = string.Empty;
            #region 校验输入
            try
            {
                //校验
                if (ResourceRequirementList == null || ResourceRequirementList.Count < 1)
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alert", "alert(\"请添加资源需求，计算失败。\")", true);
                    return;
                }

                if (string.IsNullOrEmpty(txtTimeBenchmark.Text.Trim()))
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alert", "alert(\"时间基准不能为空，计算失败。\")", true);
                    return;
                }
                DateTime timeBenchmark = DateTime.Now;
                if (!DateTime.TryParse(FormatDateTimeString(txtTimeBenchmark.Text.Trim()), out timeBenchmark))
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alert", "alert(\"时间基准格式错误，计算失败。\")", true);
                    return;
                }

                xmlStr = ResourceRequirement.GenerateResourceCalculateXML(timeBenchmark, ResourceRequirementList);
                if (string.IsNullOrEmpty(xmlStr))
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alert", "alert(\"请添加资源需求，计算失败。\")", true);
                    return;
                }
            }
            catch (Exception ex)
            {
                throw (new AspNetException("新增资源调度计算页面 校验数据出现异常，异常原因", ex));
            }
            #endregion

            #region 调用资源调度计算
            DateTime createdTime = DateTime.Now;
            string rfDirectory = string.Empty;
            string rfName = string.Empty;
            string rfDisplayName = string.Empty;
            string resultFileDirectory = string.Empty;
            string resultFileName = "out.xml";
            string strResult = string.Empty;
            try
            {
                rfDirectory = SystemParameters.GetSystemParameterValue(SystemParametersType.ResourceCalculate, "RequirementFileDirectory").TrimEnd(new char[] { '\\' }) + "\\";
                rfName = Guid.NewGuid().ToString() + ".xml";
                rfDisplayName = "资源需求文件" + createdTime.ToString("yyyyMMddHHmmss") + ".xml";
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(xmlStr);
                if (!Directory.Exists(rfDirectory))
                    Directory.CreateDirectory(rfDirectory);
                xmlDocument.Save(Path.Combine(rfDirectory, rfName));

                //string resultFileDisplayName = string.Empty;
                ZYDDCaculator oCaculator = new ZYDDCaculator();
                strResult = oCaculator.DoCaculate(Path.Combine(rfDirectory, rfName), out resultFileDirectory);
                if (!string.IsNullOrEmpty(strResult))
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alert", string.Format("alert(\"{0}。\")", strResult), true);
            }
            catch (Exception ex)
            {
                throw (new AspNetException("新增资源调度计算页面 调度计算出现异常，异常原因", ex));
            }
            #endregion

            #region 写入计算结果数据
            try
            {
                ResourceCalculate resourceCalculate = new ResourceCalculate();
                resourceCalculate.RequirementFileDirectory = rfDirectory;
                resourceCalculate.RequirementFileName = rfName;
                resourceCalculate.RequirementFileDisplayName = rfDisplayName;
                resourceCalculate.ResultFileDirectory = resultFileDirectory;
                resourceCalculate.ResultFileName = resultFileName;
                //resourceCalculate.ResultFileDisplayName = resultFileDisplayName;
                resourceCalculate.ResultFileSource = 1;
                if (!string.IsNullOrEmpty(strResult))
                    resourceCalculate.CalculateResult = 2;
                else
                    resourceCalculate.CalculateResult = 1;
                resourceCalculate.Status = 1;
                resourceCalculate.CreatedTime = createdTime;
                resourceCalculate.CreatedUserID = LoginUserInfo.Id;
                Framework.FieldVerifyResult result = resourceCalculate.Add();

                switch (result)
                {
                    case Framework.FieldVerifyResult.Error:
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alert", "alert(\"发生了数据错误，无法完成请求的操作。\")", true);
                        break;
                    case Framework.FieldVerifyResult.Success:
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alert", "alert(\"提交计算成功。\");window.location.href='/Views/BusinessManage/ResourceCalculateManage.aspx'", true);
                        ViewState["ResourceRequirement"] = null;
                        BindResourceRequirementList();
                        ResetControls();
                        break;
                    default:
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alert", "alert(\"发生未知错误，提交计算失败。\")", true);
                        break;
                }   
            }
            catch (System.Threading.ThreadAbortException ex1)
            { }
            catch(Exception ex)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alert", "alert(\"系统异常，提交计算失败。\")", true);
                throw (new AspNetException("新增资源调度计算页面 写入计算结果数据出现异常，异常原因", ex));
            }
            #endregion
        }
        /// <summary>
        /// 编辑资源需求(功能暂时隐藏)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnEditResourceRequirement_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton lbtnEditResourceRequirement = (sender as LinkButton);
                string requirementName = lbtnEditResourceRequirement.CommandArgument;
                ResourceRequirement resourceRequirement = ResourceRequirementList.Find(a => a.RequirementName.ToLower() == requirementName.ToLower());
                if (resourceRequirement == null)
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alert", "alert(\"发生了数据错误，无法完成请求的操作。\")", true);
                    BindResourceRequirementList();
                    return;
                }

                lblRequirementName.Text = resourceRequirement.RequirementName;
                hidWXBMIndex.Value = resourceRequirement.WXBMIndex.ToString();
                //txtTimeBenchmark.Text = Convert.ToDateTime(resourceRequirement.TimeBenchmark).ToString("yyyy-MM-dd");
                //dplTimeBenchmarkHour.SelectedValue = Convert.ToDateTime(resourceRequirement.TimeBenchmark).Hour.ToString();
                //dplTimeBenchmarkMinute.SelectedValue = Convert.ToDateTime(resourceRequirement.TimeBenchmark).Minute.ToString();
                //dplTimeBenchmarkSecond.SelectedValue = Convert.ToDateTime(resourceRequirement.TimeBenchmark).Second.ToString();
                txtPriority.Text = resourceRequirement.Priority.ToString();
                dplSatName.SelectedValue = resourceRequirement.WXBM;
                dplFunctionType.SelectedValue = resourceRequirement.FunctionType;
                txtPersistenceTime.Text = resourceRequirement.PersistenceTime.ToString();
                ViewState["UnusedEquipment"] = resourceRequirement.UnusedEquipmentList;
                ViewState["PeriodOfTime"] = resourceRequirement.PeriodOfTimeList;
                BindUnusedEquipmentList();
                BindPeriodOfTimeList();
            }
            catch (System.Threading.ThreadAbortException ex1)
            { }
            catch (Exception ex)
            {
                throw (new AspNetException("新增资源调度计算页面lbtnEditResourceRequirement_Click方法出现异常，异常原因", ex));
            }
        }
        /// <summary>
        /// 删除资源需求(功能暂时隐藏)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnDeleteResourceRequirement_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton lbtnDeleteResourceRequirement = (sender as LinkButton);
                string requirementName = lbtnDeleteResourceRequirement.CommandArgument;
                ResourceRequirement resourceRequirement = ResourceRequirementList.Find(a => a.RequirementName.ToLower() == requirementName.ToLower());
                if (resourceRequirement == null)
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alert", "alert(\"发生了数据错误，无法完成请求的操作。\")", true);
                    BindResourceRequirementList();
                    return;
                }

                List<ResourceRequirement> resourceRequirementList = ResourceRequirementList;
                resourceRequirementList.Remove(resourceRequirement);
                ViewState["ResourceRequirement"] = resourceRequirementList;
                BindResourceRequirementList();
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alert", "alert(\"删除资源需求成功。\")", true);
            }
            catch (System.Threading.ThreadAbortException ex1)
            { }
            catch (Exception ex)
            {
                throw (new AspNetException("新增资源调度计算页面lbtnDeleteResourceRequirement_Click方法出现异常，异常原因", ex));
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cpResourceRequirementPager_PostBackPage(object sender, EventArgs e)
        {
            try
            {
                BindResourceRequirementList();
            }
            catch (Exception ex)
            {
                throw (new AspNetException("新增资源调度计算页面cpResourceRequirementPager_PostBackPage方法出现异常，异常原因", ex));
            }
        }
        /// <summary>
        /// 添加不可用设备
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddUnusedEquipment_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtGRCode.Text.Trim()))
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "地面站编码不能为空";
                    return;
                }
                if (string.IsNullOrEmpty(txtGREquipmentCode.Text.Trim()))
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "地面站设备编码不能为空";
                    return;
                }

                UnusedEquipment unusedEquipment = new UnusedEquipment() { GRCode = txtGRCode.Text.Trim(), EquipmentCode = txtGREquipmentCode.Text.Trim() };
                List<UnusedEquipment> unusedEquipmentList = UnusedEquipmentList;
                unusedEquipmentList.Add(unusedEquipment);
                ViewState["UnusedEquipment"] = unusedEquipmentList;
                BindUnusedEquipmentList();
                ResetUnusedEquipmentControls();
            }
            catch (System.Threading.ThreadAbortException ex1)
            { }
            catch (Exception ex)
            {
                throw (new AspNetException("新增资源调度计算页面btnAddUnusedEquipment_Click方法出现异常，异常原因", ex));
            }
        }
        /// <summary>
        /// 删除不可用设备
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnDeleteUnusedEquipment_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton lbtnDeleteUnusedEquipment = (sender as LinkButton);
                string grCode = lbtnDeleteUnusedEquipment.CommandArgument.Split('$')[0];
                string equipmentCode = lbtnDeleteUnusedEquipment.CommandArgument.Split('$')[1];
                List<UnusedEquipment> unusedEquipmentList = UnusedEquipmentList;
                int index = unusedEquipmentList.FindIndex(a => a.GRCode.ToLower() == grCode.ToLower() && a.EquipmentCode.ToLower() == equipmentCode.ToLower());
                if (index >= 0)
                    unusedEquipmentList.RemoveAt(index);

                ViewState["UnusedEquipment"] = unusedEquipmentList;
                BindUnusedEquipmentList();
                ResetUnusedEquipmentControls();
            }
            catch (System.Threading.ThreadAbortException ex1)
            { }
            catch (Exception ex)
            {
                throw (new AspNetException("新增资源调度计算页面lbtnDeleteUnusedEquipment_Click方法出现异常，异常原因", ex));
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cpUnusedEquipmentPager_PostBackPage(object sender, EventArgs e)
        {
            try
            {
                BindUnusedEquipmentList();
            }
            catch (Exception ex)
            {
                throw (new AspNetException("新增资源调度计算页面cpUnusedEquipmentPager_PostBackPage方法出现异常，异常原因", ex));
            }
        }
        /// <summary>
        /// 添加支持时段
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddPeriodOfTime_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtBeginTime.Text.Trim()))
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "开始时间不能为空";
                    return;
                }
                if (string.IsNullOrEmpty(txtEndTime.Text.Trim()))
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "结束时间不能为空";
                    return;
                }

                DateTime beginTime = DateTime.Now;
                DateTime endTime = DateTime.Now;
                if (!DateTime.TryParse(FormatDateTimeString(txtBeginTime.Text.Trim()), out beginTime))
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "开始时间格式错误";
                    return;
                }
                //beginTime = beginTime.AddHours(Convert.ToDouble(dplBeginTimeHour.SelectedValue));
                //beginTime = beginTime.AddMinutes(Convert.ToDouble(dplBeginTimeMinute.SelectedValue));
                //beginTime = beginTime.AddSeconds(Convert.ToDouble(dplBeginTimeSecond.SelectedValue));
                if (!DateTime.TryParse(FormatDateTimeString(txtEndTime.Text.Trim()), out endTime))
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "结束时间格式错误";
                    return;
                }
                //endTime = endTime.AddHours(Convert.ToDouble(dplEndTimeHour.SelectedValue));
                //endTime = endTime.AddMinutes(Convert.ToDouble(dplEndTimeMinute.SelectedValue));
                //endTime = endTime.AddSeconds(Convert.ToDouble(dplEndTimeSecond.SelectedValue));
                if (beginTime > endTime)
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "开始时间应小于结束时间";
                    return;
                }

                PeriodOfTime periodOfTime = new PeriodOfTime() {  BeginTime = beginTime, EndTime = endTime };
                List<PeriodOfTime> periodOfTimeList = PeriodOfTimeList;
                periodOfTimeList.Add(periodOfTime);
                ViewState["PeriodOfTime"] = periodOfTimeList;
                BindPeriodOfTimeList();
                ResetPeriodOfTimeControls();

            }
            catch (System.Threading.ThreadAbortException ex1)
            { }
            catch (Exception ex)
            {
                throw (new AspNetException("新增资源调度计算页面btnAddPeriodOfTime_Click方法出现异常，异常原因", ex));
            }
        }
        /// <summary>
        /// 删除支持时段
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnDeletePeriodOfTime_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton lbtnDeletePeriodOfTime = (sender as LinkButton);
                DateTime beginTime = DateTime.Now;
                DateTime endTime = DateTime.Now;
                if (DateTime.TryParse(lbtnDeletePeriodOfTime.CommandArgument.Split('$')[0], out beginTime) && DateTime.TryParse(lbtnDeletePeriodOfTime.CommandArgument.Split('$')[1], out endTime))
                {
                    List<PeriodOfTime> periodOfTimeList = PeriodOfTimeList;
                    int index = periodOfTimeList.FindIndex(a => a.BeginTime == beginTime && a.EndTime == endTime);
                    if (index >= 0)
                        periodOfTimeList.RemoveAt(index);

                    ViewState["PeriodOfTime"] = periodOfTimeList;
                }

                BindPeriodOfTimeList();
                ResetPeriodOfTimeControls();
            }
            catch (System.Threading.ThreadAbortException ex1)
            { }
            catch (Exception ex)
            {
                throw (new AspNetException("新增资源调度计算页面lbtnDeletePeriodOfTime_Click方法出现异常，异常原因", ex));
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cpPeriodOfTimePager_PostBackPage(object sender, EventArgs e)
        {
            try
            {
                BindPeriodOfTimeList();
            }
            catch (Exception ex)
            {
                throw (new AspNetException("新增资源调度计算页面cpPeriodOfTimePager_PostBackPage方法出现异常，异常原因", ex));
            }
        }
        /// <summary>
        /// 当卫星编码发生变化时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void dplSatName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                hidWXBMIndex.Value = GetWXBMIndex().ToString();
                lblRequirementName.Text = GetRequirementName();
            }
            catch (System.Threading.ThreadAbortException ex1)
            { }
            catch (Exception ex)
            {
                throw (new AspNetException("新增资源调度计算页面dplSatName_SelectedIndexChanged方法出现异常，异常原因", ex));
            }
        }

        protected void rpResourceRequirementList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    Label lblUnusedEquipment = (e.Item.FindControl("lblUnusedEquipment") as Label);
                    Label lblPeriodOfTime = (e.Item.FindControl("lblPeriodOfTime") as Label);
                    ResourceRequirement resourceRequirement = (e.Item.DataItem as ResourceRequirement);
                    if (lblUnusedEquipment != null && lblPeriodOfTime != null && resourceRequirement != null)
                    {
                        string unusedEquipmentStr = "{0}.地面站编码：{1},地面站设备编码：{2};";
                        for (int i = 0; i < resourceRequirement.UnusedEquipmentList.Count; i++)
                        {
                            lblUnusedEquipment.ToolTip += string.Format(unusedEquipmentStr, i + 1, resourceRequirement.UnusedEquipmentList[i].GRCode, resourceRequirement.UnusedEquipmentList[i].EquipmentCode);
                        }
                        lblUnusedEquipment.Text = "共" + resourceRequirement.UnusedEquipmentList.Count.ToString() + "条记录";

                        string periodOfTimeStr = "{0}.开始时间：{1},结束时间：{2};";
                        for (int i = 0; i < resourceRequirement.PeriodOfTimeList.Count; i++)
                        {
                            lblPeriodOfTime.ToolTip += string.Format(periodOfTimeStr, i + 1, resourceRequirement.PeriodOfTimeList[i].BeginTime.ToString("yyyyMMddHHmmss"), resourceRequirement.PeriodOfTimeList[i].EndTime.ToString("yyyyMMddHHmmss"));
                        }
                        lblPeriodOfTime.Text = "共" + resourceRequirement.PeriodOfTimeList.Count.ToString() + "条记录";
                    }
                }
            }
            catch (System.Threading.ThreadAbortException ex1)
            { }
            catch (Exception ex)
            {
                throw (new AspNetException("新增资源调度计算页面rpResourceRequirementList_ItemDataBound方法出现异常，异常原因", ex));
            }
        }

        public override void OnPageLoaded()
        {
            this.PagePermission = "OMB_ResCac.Caculate";
            this.ShortTitle = "新增资源调度计算";
            this.SetTitle();
        }

        #region Method
        /// <summary>
        /// 绑定卫星数据源
        /// </summary>
        private void BindSatNameDataSource()
        {
            dplSatName.Items.Clear();
            Satellite satellite = new Satellite();
            dplSatName.DataSource = satellite.Cache;
            dplSatName.DataTextField = "WXMC";
            dplSatName.DataValueField = "Id";
            dplSatName.DataBind();
            //dplSatName.Items.Insert(0, new ListItem("请选择", ""));
        }
        /// <summary>
        /// 绑定控件数据源,暂时这样做,没有提供表结构
        /// </summary>
        private void BindDataSource()
        {
            dplFunctionType.Items.Clear();
            dplFunctionType.DataSource = SystemParameters.GetSystemParameters(SystemParametersType.GroundResourceFunctionType);
            dplFunctionType.DataTextField = "key";
            dplFunctionType.DataValueField = "value";
            dplFunctionType.DataBind();

            dplBeginTimeHour.Items.Clear();
            dplEndTimeHour.Items.Clear();
            dplTimeBenchmarkHour.Items.Clear();
            for (int i = 0; i < 24; i++)
            {
                dplBeginTimeHour.Items.Add(new ListItem(i.ToString() + "时", i.ToString()));
                dplEndTimeHour.Items.Add(new ListItem(i.ToString() + "时", i.ToString()));
                dplTimeBenchmarkHour.Items.Add(new ListItem(i.ToString() + "时", i.ToString()));
            }
            dplBeginTimeMinute.Items.Clear();
            dplEndTimeMinute.Items.Clear();
            dplTimeBenchmarkMinute.Items.Clear();
            dplBeginTimeSecond.Items.Clear();
            dplEndTimeSecond.Items.Clear();
            dplTimeBenchmarkSecond.Items.Clear();
            for (int i = 0; i < 60; i++)
            {
                dplBeginTimeMinute.Items.Add(new ListItem(i.ToString() + "分", i.ToString()));
                dplEndTimeMinute.Items.Add(new ListItem(i.ToString() + "分", i.ToString()));
                dplTimeBenchmarkMinute.Items.Add(new ListItem(i.ToString() + "分", i.ToString()));

                dplBeginTimeSecond.Items.Add(new ListItem(i.ToString() + "秒", i.ToString()));
                dplEndTimeSecond.Items.Add(new ListItem(i.ToString() + "秒", i.ToString()));
                dplTimeBenchmarkSecond.Items.Add(new ListItem(i.ToString() + "秒", i.ToString()));
            }       
        }
        /// <summary>
        /// 根据卫星编码生成卫星序号
        /// </summary>
        private int GetWXBMIndex()
        {
            int wxbmIndex = 0;
            //当没有选择卫星时
            if (string.IsNullOrEmpty(dplSatName.SelectedValue))
                return wxbmIndex;

            //当缓存中为空时
            if (ResourceRequirementList == null || ResourceRequirementList.Count < 1)
            {
                wxbmIndex = 1;
                return wxbmIndex;
            }

            //其他情况
            List<ResourceRequirement> resourceRequirementList = ResourceRequirementList;
            var query = resourceRequirementList.Where(a => a.WXBM.ToLower() == dplSatName.SelectedValue.ToLower()).OrderBy(a => a.WXBMIndex);
            resourceRequirementList = query.ToList();
            wxbmIndex = resourceRequirementList.Count() + 1;

            for (int i = 0; i < resourceRequirementList.Count(); i++)
            {
                if (resourceRequirementList[i].WXBMIndex != i + 1)
                {
                    wxbmIndex = i + 1;
                    break;
                }
            }
           
            return wxbmIndex;
        }
        /// <summary>
        /// 根据卫星编码生成需求名称
        /// </summary>
        /// <returns></returns>
        public string GetRequirementName()
        {
            string requirementName = string.Empty;
            //当没有选择卫星时
            if (string.IsNullOrEmpty(dplSatName.SelectedValue) || hidWXBMIndex.Value == "0")
                return requirementName;

            switch (hidWXBMIndex.Value.Length)
            {
                case 1:
                    requirementName = "00" + hidWXBMIndex.Value;
                    break;
                case 2:
                    requirementName = "0" + hidWXBMIndex.Value;
                    break;
                case 3:
                    requirementName = hidWXBMIndex.Value;
                    break;
                default:
                    //添加单个卫星个数大于999
                    requirementName = hidWXBMIndex.Value;
                    break;
            }
            return dplSatName.SelectedValue + requirementName;
        }
        /// <summary>
        /// 绑定不可用设备列表
        /// </summary>
        private void BindUnusedEquipmentList()
        {
            if (UnusedEquipmentList.Count > this.SiteSetting.PageSize)
                cpUnusedEquipmentPager.Visible = true;
            cpUnusedEquipmentPager.DataSource = UnusedEquipmentList;
            cpUnusedEquipmentPager.PageSize = this.SiteSetting.PageSize;
            cpUnusedEquipmentPager.BindToControl = rpUnusedEquipmentList;
            rpUnusedEquipmentList.DataSource = cpUnusedEquipmentPager.DataSourcePaged;
            rpUnusedEquipmentList.DataBind();
        }
        /// <summary>
        /// 重置不可用设备控件
        /// </summary>
        private void ResetUnusedEquipmentControls()
        {
            txtGRCode.Text = string.Empty;
            txtGREquipmentCode.Text = string.Empty;
            trMessage.Visible = false;
            lblMessage.Text = string.Empty;
        }
        /// <summary>
        /// 绑定支持时段列表
        /// </summary>
        private void BindPeriodOfTimeList()
        {
            if (PeriodOfTimeList.Count > this.SiteSetting.PageSize)
                cpPeriodOfTimePager.Visible = true;
            cpPeriodOfTimePager.DataSource = PeriodOfTimeList;
            cpPeriodOfTimePager.PageSize = this.SiteSetting.PageSize;
            cpPeriodOfTimePager.BindToControl = rpPeriodOfTimeList;
            rpPeriodOfTimeList.DataSource = cpPeriodOfTimePager.DataSourcePaged;
            rpPeriodOfTimeList.DataBind();
        }
        /// <summary>
        /// 重置支持时段控件
        /// </summary>
        private void ResetPeriodOfTimeControls()
        {
            txtBeginTime.Text = string.Empty;
            txtEndTime.Text = string.Empty;
            dplBeginTimeHour.SelectedIndex = 0;
            dplBeginTimeMinute.SelectedIndex = 0;
            dplBeginTimeSecond.SelectedIndex = 0;
            dplEndTimeHour.SelectedIndex = 0;
            dplEndTimeMinute.SelectedIndex = 0;
            dplEndTimeSecond.SelectedIndex = 0;
            trMessage.Visible = false;
            lblMessage.Text = string.Empty;
        }
        /// <summary>
        /// 绑定资源需求列表
        /// </summary>
        private void BindResourceRequirementList()
        {
            if (ResourceRequirementList.Count > this.SiteSetting.PageSize)
                cpResourceRequirementPager.Visible = true;
            cpResourceRequirementPager.DataSource = ResourceRequirementList;
            cpResourceRequirementPager.PageSize = this.SiteSetting.PageSize;
            cpResourceRequirementPager.BindToControl = rpResourceRequirementList;
            rpResourceRequirementList.DataSource = cpResourceRequirementPager.DataSourcePaged;
            rpResourceRequirementList.DataBind();
        }
        /// <summary>
        /// 重置控件
        /// </summary>
        private void ResetControls()
        {
            hidWXBMIndex.Value = GetWXBMIndex().ToString();
            lblRequirementName.Text = GetRequirementName();
            //txtTimeBenchmark.Text = string.Empty;
            txtPriority.Text = string.Empty;
            dplSatName.SelectedIndex = 0;
            dplFunctionType.SelectedIndex = 0;
            txtPersistenceTime.Text = string.Empty;

            txtGRCode.Text = string.Empty;
            txtGREquipmentCode.Text = string.Empty;
            ViewState["UnusedEquipment"] = null;
            BindUnusedEquipmentList();

            txtBeginTime.Text = string.Empty;
            txtEndTime.Text = string.Empty;
            dplBeginTimeHour.SelectedIndex = 0;
            dplBeginTimeMinute.SelectedIndex = 0;
            dplBeginTimeSecond.SelectedIndex = 0;
            dplEndTimeHour.SelectedIndex = 0;
            dplEndTimeMinute.SelectedIndex = 0;
            dplEndTimeSecond.SelectedIndex = 0;
            ViewState["PeriodOfTime"] = null;
            BindPeriodOfTimeList();
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
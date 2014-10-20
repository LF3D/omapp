#region
//------------------------------------------------------
//Assembly:OperatingManagement.Web
//FileName:ResourceCalculateManage.cs
//Remark:资源计算管理类
//------------------------------------------------------
//VERSION       AUTHOR      DATE        CONTENT
//1.0           liutao      20120229    Create     
//------------------------------------------------------
#endregion
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using OperatingManagement.Framework.Core;
using OperatingManagement.DataAccessLayer.BusinessManage;
using OperatingManagement.WebKernel.Basic;

namespace OperatingManagement.Web.Views.BusinessManage
{
    public partial class ResourceCalculateManage : AspNetPage
    {
        #region 属性
        /// <summary>
        /// 查询条件，计算状态
        /// </summary>
        protected int Status
        {
            get
            {       
                if (ViewState["Status"] == null)
                {
                    ViewState["Status"] = dplStatus.SelectedValue;
                }
                int status = 0;
                int.TryParse(ViewState["Status"].ToString(), out status);
                return status;
            }
            set { ViewState["Status"] = value; }
        }
        /// <summary>
        /// 查询条件，计算结果文件来源
        /// </summary>
        protected int ResultFileSource
        {
            get
            {              
                if (ViewState["ResultFileSource"] == null)
                {
                    ViewState["ResultFileSource"] = dplResultFileSource.SelectedValue;
                }
                int resultFileSource = 0;
                int.TryParse(ViewState["ResultFileSource"].ToString(), out resultFileSource);
                return resultFileSource;
            }
            set { ViewState["ResultFileSource"] = value; }
        }
        /// <summary>
        /// 查询条件，起始时间
        /// </summary>
        protected DateTime BeginTime
        {
            get
            {
                if (ViewState["BeginTime"] == null)
                {
                    ViewState["BeginTime"] = txtBeginTime.Text.Trim();
                }
                DateTime beginTime = DateTime.MinValue;
                DateTime.TryParse(ViewState["BeginTime"].ToString(), out beginTime);
                return beginTime;
            }
            set { ViewState["BeginTime"] = value; }
        }
        /// <summary>
        /// 查询条件，结束时间
        /// </summary>
        protected DateTime EndTime
        {
            get
            {
                if (ViewState["EndTime"] == null)
                {
                    ViewState["EndTime"] = txtEndTime.Text.Trim();    
                }
                DateTime endTime = DateTime.MinValue;
                DateTime.TryParse(ViewState["EndTime"].ToString(), out endTime);
                return endTime.AddSeconds(86399.9);//23:59:59
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
                    BindResourceCalculateList();
                }

                cpResourceCalculatePager.PostBackPage += new EventHandler(cpResourceCalculatePager_PostBackPage);
            }
            catch (Exception ex)
            {
                throw (new AspNetException("查询资源调度计算页面初始化出现异常，异常原因", ex));
            }
        }
        /// <summary>
        /// 查询资源调度计算结果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtBeginTime.Text.Trim()))
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alert", "alert(\"起始时间不能为空。\")", true);
                    return;
                }

                if (string.IsNullOrEmpty(txtEndTime.Text.Trim()))
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alert", "alert(\"结束时间不能为空。\")", true);
                    return;
                }
                DateTime beginTime = DateTime.MinValue;
                DateTime endTime = DateTime.MinValue;
                if (!DateTime.TryParse(txtBeginTime.Text.Trim(), out beginTime))
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alert", "alert(\"起始时间格式错误。\")", true);
                    return;
                }

                if (!DateTime.TryParse(txtEndTime.Text.Trim(), out endTime))
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alert", "alert(\"结束时间格式错误。\")", true);
                    return;
                }

                if (beginTime > endTime)
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alert", "alert(\"起始时间应小于结束时间。\")", true);
                    return;
                }
                Status = Convert.ToInt32(dplStatus.SelectedValue);
                ResultFileSource = Convert.ToInt32(dplResultFileSource.SelectedValue);
                BeginTime = beginTime;
                EndTime = endTime;
                cpResourceCalculatePager.CurrentPage = 1;
                BindResourceCalculateList();
            }
            catch (Exception ex)
            {
                throw (new AspNetException("查询资源调度计算页面btnSearch_Click方法出现异常，异常原因", ex));
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cpResourceCalculatePager_PostBackPage(object sender, EventArgs e)
        {
            BindResourceCalculateList();
        }
        /// <summary>
        /// 添加资源调度计算
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string url = @"~/Views/BusinessManage/ResourceRequirementAdd.aspx";
                Response.Redirect(url);
            }
            catch (System.Threading.ThreadAbortException ex1)
            { }
            catch (Exception ex)
            {
                throw (new AspNetException("查询资源调度计算页面btnAdd_Click方法出现异常，异常原因", ex));
            }
        }
        /// <summary>
        /// 查看资源调度计算结果
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnCalculateResultView_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton lbtnCalculateResultView = (sender as LinkButton);
                if (lbtnCalculateResultView == null)
                {
                    BindResourceCalculateList();
                    return;
                }
                string url = @"~/Views/BusinessManage/ResourceCalculateResultView.aspx?rcid=" + Server.UrlEncode(lbtnCalculateResultView.CommandArgument);
                Response.Redirect(url);
            }
            catch (System.Threading.ThreadAbortException ex1)
            { }
            catch (Exception ex)
            {
                throw (new AspNetException("查询资源调度计算页面lbtnCalculateResultView_Click方法出现异常，异常原因", ex));
            }
        }

        /// <summary>
        /// 下载资源需求文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnRequirementFileDownload_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton lbtnRequirementFileDownload = (sender as LinkButton);
                if (lbtnRequirementFileDownload == null)
                {
                    BindResourceCalculateList();
                    return;
                }
                int rcID = 0;
                if (!int.TryParse(lbtnRequirementFileDownload.CommandArgument, out rcID))
                {
                    BindResourceCalculateList();
                    return;
                }

                ResourceCalculate resourceCalculate = new ResourceCalculate();
                resourceCalculate.Id = rcID;
                resourceCalculate = resourceCalculate.SelectByID();
                if (resourceCalculate == null)
                {
                    BindResourceCalculateList();
                    return;
                }
                string fileName = resourceCalculate.RequirementFileDirectory.TrimEnd('\\') + '\\' + resourceCalculate.RequirementFileName;
                System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
                xmlDocument.Load(fileName);
           
                Response.Clear();
                Response.Buffer = false;
                Response.ContentType = "application/octet-stream";
                Response.AppendHeader("content-disposition", "attachment;filename=" + resourceCalculate.RequirementFileDisplayName + ";");
                Response.Write(xmlDocument.OuterXml);
                Response.Flush();
                Response.End();
            }
            catch (System.Threading.ThreadAbortException ex1)
            { }
            catch(Exception ex)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alert", "alert(\"下载资源需求文件失败。\")", true);
                throw (new AspNetException("查询资源调度计算页面lbtnRequirementFileDownload_Click方法出现异常，异常原因", ex));
            }
        }

        /// <summary>
        /// 下载计算结果文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnResultFileDownload_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton lbtnResultFileDownload = (sender as LinkButton);
                if (lbtnResultFileDownload == null)
                {
                    BindResourceCalculateList();
                    return;
                }
                int rcID = 0;
                if (!int.TryParse(lbtnResultFileDownload.CommandArgument, out rcID))
                {
                    BindResourceCalculateList();
                    return;
                }

                ResourceCalculate resourceCalculate = new ResourceCalculate();
                resourceCalculate.Id = rcID;
                resourceCalculate = resourceCalculate.SelectByID();
                if (resourceCalculate == null)
                {
                    BindResourceCalculateList();
                    return;
                }
                string fileName = resourceCalculate.ResultFileDirectory.TrimEnd('\\') + '\\' + resourceCalculate.ResultFileName;
                System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
                xmlDocument.Load(fileName);

                Response.Clear();
                Response.Buffer = false;
                Response.ContentType = "application/octet-stream";
                Response.AppendHeader("content-disposition", "attachment;filename=" + resourceCalculate.ResultFileDisplayName + ";");
                Response.Write(xmlDocument.OuterXml);
                Response.Flush();
                Response.End();
            }
            catch (System.Threading.ThreadAbortException ex1)
            { }
            catch(Exception ex)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alert", "alert(\"下载计算结果文件失败。\")", true);
                throw (new AspNetException("查询资源调度计算页面lbtnResultFileDownload_Click方法出现异常，异常原因", ex));
            }
        }

        /// <summary>
        /// 上传计算结果文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                if (!fileUploadResultFile.HasFile)
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alert", "alert(\"请选择要上传的计算结果文件。\")", true);
                    return;
                }
                if (fileUploadResultFile.PostedFile.ContentLength == 0)
                {
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alert", "alert(\"上传计算结果文件不能为空。\")", true);
                    return;
                }
                int fileSize = fileUploadResultFile.PostedFile.ContentLength;
                string fileName = fileUploadResultFile.PostedFile.FileName.Substring(fileUploadResultFile.PostedFile.FileName.LastIndexOf('\\') + 1);
                string fileExtension = fileName.Substring(fileName.LastIndexOf('.') + 1);
                int resultFileMaxSize = int.Parse(SystemParameters.GetSystemParameterValue(SystemParametersType.ResourceCalculate, "ResultFileMaxSize"));
                string resultFileExtension = SystemParameters.GetSystemParameterValue(SystemParametersType.ResourceCalculate, "ResultFileExtension").Trim(new char[] { ',', '，', ';', '；' });
                string[] extensionArray = resultFileExtension.Split(new char[] { ',', '，', ';', '；' }, StringSplitOptions.RemoveEmptyEntries);
                bool allowExtension = false;
                if (extensionArray != null && extensionArray.Length > 0)
                {
                    foreach (string extension in extensionArray)
                    {
                        if (extension.ToLower() == fileExtension.ToLower())
                        {
                            allowExtension = true;
                            break;
                        }
                    }
                }
                if (!allowExtension)
                {
                    string message = string.Format("上传计算结果文件格式不正确，应为：{0}。", resultFileExtension);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alert", "alert(\"" + message + "\")", true);
                    return;
                }
                if (fileSize > resultFileMaxSize)
                {
                    string message = string.Format("上传计算结果文件不能超过{0}字节", resultFileMaxSize.ToString());
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alert", "alert(\"" + message + "\")", true);
                    return;
                }
                string resultFileDirectory = SystemParameters.GetSystemParameterValue(SystemParametersType.ResourceCalculate, "ResultFileDirectory").TrimEnd(new char[] { '\\' }) + "\\";
                string resultFileName = Guid.NewGuid().ToString() + "." + fileExtension;
                fileUploadResultFile.PostedFile.SaveAs(resultFileDirectory + resultFileName);

                DateTime createdTime = DateTime.Now;
                ResourceCalculate resourceCalculate = new ResourceCalculate();
                //resourceCalculate.RequirementFileDirectory = requirementFileDirectory;
                //resourceCalculate.RequirementFileName = requirementFileName;
                //resourceCalculate.RequirementFileDisplayName = requirementFileDisplayName;
                resourceCalculate.ResultFileDirectory = resultFileDirectory;
                resourceCalculate.ResultFileName = resultFileName;
                resourceCalculate.ResultFileDisplayName = fileName;
                resourceCalculate.ResultFileSource = 2;
                //resourceCalculate.CalculateResult = 1;
                resourceCalculate.Status = 2;
                resourceCalculate.CreatedTime = createdTime;
                resourceCalculate.UpdatedTime = createdTime;
                Framework.FieldVerifyResult result = resourceCalculate.Add();

                switch (result)
                {
                    case Framework.FieldVerifyResult.Error:
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alert", "alert(\"发生了数据错误，无法完成请求的操作。\")", true);
                        break;
                    case Framework.FieldVerifyResult.Success:
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alert", "alert(\"上传计算结果文件成功。\")", true);
                        BindResourceCalculateList();
                        break;
                    default:
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alert", "alert(\"发生未知错误，上传计算结果文件失败。\")", true);
                        break;
                }
            }
            catch (System.Threading.ThreadAbortException ex1)
            { }
            catch (Exception ex)
            {
                throw (new AspNetException("查询资源调度计算页面btnUpload_Click方法出现异常，异常原因", ex));
            }
        }

        public override void OnPageLoaded()
        {
            this.PagePermission = "OMB_ResCac.Caculate";
            this.ShortTitle = "查询资源调度计算";
            this.SetTitle();
        }

        #region ItemDataBound
        /// <summary>
        /// 资源计算结果单条数据绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rpResourceCalculateList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    LinkButton lbtnRequirementFileDownload = (e.Item.FindControl("lbtnRequirementFileDownload") as LinkButton);
                    LinkButton lbtnResultFileDownload = (e.Item.FindControl("lbtnResultFileDownload") as LinkButton);
                    LinkButton lbtnCalculateResultView = (e.Item.FindControl("lbtnCalculateResultView") as LinkButton);
                    if (lbtnRequirementFileDownload != null && lbtnResultFileDownload != null && lbtnCalculateResultView != null)
                    {
                        ResourceCalculate resourceCalculate = (e.Item.DataItem as ResourceCalculate);
                        lbtnRequirementFileDownload.Enabled = (resourceCalculate.ResultFileSource == 1);
                        lbtnResultFileDownload.Enabled = (resourceCalculate.ResultFileSource == 2 || resourceCalculate.Status == 2);
                        lbtnCalculateResultView.Enabled = (resourceCalculate.ResultFileSource == 2 || resourceCalculate.CalculateResult == 1);
                    }
                }
            }
            catch (Exception ex)
            {
                throw (new AspNetException("查询资源调度计算页面rpResourceCalculateList_ItemDataBound方法出现异常，异常原因", ex));
            }
        }

        #endregion

        #region Method
        /// <summary>
        /// 绑定计算状态、计算结果文件来源数据源
        /// </summary>
        private void BindDataSource()
        {
            dplStatus.Items.Clear();
            dplStatus.DataSource = SystemParameters.GetSystemParameters(SystemParametersType.ResourceCalculateStatus);
            dplStatus.DataTextField = "key";
            dplStatus.DataValueField = "value";
            dplStatus.DataBind();
            dplStatus.Items.Insert(0, new ListItem("请选择", "0"));

            dplResultFileSource.Items.Clear();
            dplResultFileSource.DataSource = SystemParameters.GetSystemParameters(SystemParametersType.ResourceCalculateResultFileSource);
            dplResultFileSource.DataTextField = "key";
            dplResultFileSource.DataValueField = "value";
            dplResultFileSource.DataBind();
            dplResultFileSource.Items.Insert(0, new ListItem("请选择", "0"));

            txtBeginTime.Text = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
            txtEndTime.Text = DateTime.Now.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// 绑定资源调度计算结果信息
        /// </summary>
        private void BindResourceCalculateList()
        {
            ResourceCalculate resourceCalculate = new ResourceCalculate();
            int status = Status;
            int resultFileSource = ResultFileSource;
            DateTime beginTime = BeginTime;
            DateTime endTime = EndTime;

            List<ResourceCalculate> resourceCalculateList = resourceCalculate.Search(resultFileSource, status, beginTime, endTime);
            if (resourceCalculateList.Count > this.SiteSetting.PageSize)
                cpResourceCalculatePager.Visible = true;
            cpResourceCalculatePager.DataSource = resourceCalculateList;
            cpResourceCalculatePager.PageSize = this.SiteSetting.PageSize;
            cpResourceCalculatePager.BindToControl = rpResourceCalculateList;
            rpResourceCalculateList.DataSource = cpResourceCalculatePager.DataSourcePaged;
            rpResourceCalculateList.DataBind();
        }

        #endregion
    }
}
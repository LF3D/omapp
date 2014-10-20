#region
//------------------------------------------------------
//Assembly:OperatingManagement.Web
//FileName:CenterResourceEdit.cs
//Remark:中心资源编辑类
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
    public partial class CenterResourceEdit : AspNetPage
    {
        #region 属性
        /// <summary>
        /// 中心资源ID
        /// </summary>
        protected int CRID
        {
            get
            {
                int crID = 0;
                if (Request.QueryString["crid"] != null)
                {
                    int.TryParse(Request.QueryString["crid"], out crID);
                }
                return crID;
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
                    BindDataSource();
                    BindControls();
                }
            }
            catch (Exception ex)
            {
                trMessage.Visible = true;
                lblMessage.Text = "发生未知错误，操作失败。";
                throw (new AspNetException("编辑中心资源页面初始化出现异常，异常原因", ex));
            }
        }
        /// <summary>
        /// 提交更新中心资源记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                string msg = string.Empty;
                if (string.IsNullOrEmpty(txtEquipmentCode.Text.Trim()))
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "设备编号不能为空";
                    return;
                }

                if (string.IsNullOrEmpty(dplEquipmentType.SelectedValue))
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "设备类型不能为空";
                    return;
                }

                if (string.IsNullOrEmpty(txtSupportTask.Text.Trim()))
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "支持的任务不能为空";
                    return;
                }

                if (string.IsNullOrEmpty(txtDataProcess.Text.Trim()))
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "最大数据处理量不能为空";
                    return;
                }

                double dataProcess = 0.0;
                if (!double.TryParse(txtDataProcess.Text.Trim(), out dataProcess))
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "最大数据处理量格式错误";
                    return;
                }

                Framework.FieldVerifyResult result;
                CenterResource centerResource = new CenterResource();
                centerResource.Id = CRID;
                centerResource = centerResource.SelectByID();
                if (centerResource == null)
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "修改的中心资源不存在";
                    return;
                }
                centerResource.EquipmentCode = txtEquipmentCode.Text.Trim();
                centerResource.EquipmentType = dplEquipmentType.SelectedValue;
                centerResource.SupportTask = txtSupportTask.Text.Trim();
                centerResource.DataProcess = dataProcess;
                //centerResource.Status = 1;//正常，状态不更新
                //centerResource.CreatedTime = DateTime.Now;
                centerResource.UpdatedTime = DateTime.Now;
                centerResource.UpdatedUserID = LoginUserInfo.Id;

                if (centerResource.HaveActiveEquipmentCode())
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "设备编号已经存在";
                    return;
                }

                result = centerResource.Update();
                switch (result)
                {
                    case Framework.FieldVerifyResult.Error:
                        msg = "发生了数据错误，无法完成请求的操作。";
                        break;
                    case Framework.FieldVerifyResult.Success:
                        msg = "修改中心资源成功。";
                        break;
                    default:
                        msg = "发生未知错误，操作失败。";
                        break;
                }
                trMessage.Visible = true;
                lblMessage.Text = msg;
            }
            catch (Exception ex)
            {
                trMessage.Visible = true;
                lblMessage.Text = "发生未知错误，操作失败。";
                throw (new AspNetException("编辑中心资源页面btnSubmit_Click方法出现异常，异常原因", ex));
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
                throw (new AspNetException("编辑中心资源页面btnReset_Click方法出现异常，异常原因", ex));
            }
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            try
            {
                string url = @"~/Views/BusinessManage/CResourceMan.aspx";
                Response.Redirect(url);
            }
            catch (System.Threading.ThreadAbortException ex1)
            { }
            catch (Exception ex)
            {
                trMessage.Visible = true;
                lblMessage.Text = "发生未知错误，操作失败。";
                throw (new AspNetException("编辑中心资源页面btnReturn_Click方法出现异常，异常原因", ex));
            }
        }

        public override void OnPageLoaded()
        {
            this.PagePermission = "OMB_CRes.Edit";
            this.ShortTitle = "编辑中心资源";
            this.SetTitle();
        }

        #region Method
        /// <summary>
        /// 绑定控件数据源
        /// </summary>
        private void BindDataSource()
        {
            dplEquipmentType.Items.Clear();
            dplEquipmentType.DataSource = SystemParameters.GetSystemParameters(SystemParametersType.CenterResourceEquipmentType);
            dplEquipmentType.DataTextField = "key";
            dplEquipmentType.DataValueField = "value";
            dplEquipmentType.DataBind();
            //dplEquipmentType.Items.Insert(0, new ListItem("请选择", ""));
        }
        /// <summary>
        /// 绑定控件值
        /// </summary>
        private void BindControls()
        {
            CenterResource centerResource = new CenterResource();
            centerResource.Id = CRID;
            centerResource = centerResource.SelectByID();
            if (centerResource != null)
            {
                txtEquipmentCode.Text = centerResource.EquipmentCode;
                dplEquipmentType.SelectedValue = centerResource.EquipmentType;
                txtSupportTask.Text = centerResource.SupportTask;
                txtDataProcess.Text = centerResource.DataProcess.ToString();
                lblCreatedTime.Text = centerResource.CreatedTime.ToString("yyyy-MM-dd HH:mm:ss");
                lblUpdatedTime.Text = centerResource.UpdatedTime == DateTime.MinValue ? centerResource.CreatedTime.ToString("yyyy-MM-dd HH:mm:ss") : centerResource.UpdatedTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }

        #endregion
    }
}
#region
//------------------------------------------------------
//Assembly:OperatingManagement.Web
//FileName:CenterOutputPolicyAdd.cs
//Remark:中心输出策略添加类
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
    public partial class CenterOutputPolicyAdd : AspNetPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                trMessage.Visible = false;
                lblMessage.Text = string.Empty;
                if (!IsPostBack)
                {
                    txtEffectTime.Attributes.Add("readonly", "true");
                    txtDefectTime.Attributes.Add("readonly", "true");
                    InitialPageData();
                }
            }
            catch(Exception ex)
            {
                trMessage.Visible = true;
                lblMessage.Text = "发生未知错误，操作失败。";
                throw (new AspNetException("新增中心输出策略页面初始化出现异常，异常原因", ex));
            }
        }
        /// <summary>
        /// 提交添加中心输出策略记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                string msg = string.Empty;
                if (string.IsNullOrEmpty(dplTask.SelectedValue))
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "请选择任务代号";
                    return;
                }

                if (string.IsNullOrEmpty(dplSatellite.SelectedValue))
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "请选择卫星名称";
                    return;
                }

                if (string.IsNullOrEmpty(dplSource.SelectedValue))
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "请选择信源";
                    return;
                }

                if (string.IsNullOrEmpty(dplInfoType.SelectedValue))
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "请选择信息类别";
                    return;
                }

                if (string.IsNullOrEmpty(dplDdestination.SelectedValue))
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "请选择信宿";
                    return;
                }

                if (dplSource.SelectedValue == dplDdestination.SelectedValue)
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "信源与信宿不能相同";
                    return;
                }

                if (string.IsNullOrEmpty(txtEffectTime.Text.Trim()))
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "生效时间不能为空";
                    return;
                }

                if (string.IsNullOrEmpty(txtDefectTime.Text.Trim()))
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "失效时间不能为空";
                    return;
                }

                DateTime effectTime = DateTime.Now;
                DateTime defectTime = DateTime.Now;

                if (!DateTime.TryParse(txtEffectTime.Text.Trim(), out effectTime))
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "生效时间格式错误";
                    return;
                }

                if (!DateTime.TryParse(txtDefectTime.Text.Trim(), out defectTime))
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "失效时间格式错误";
                    return;
                }
                //23:59:59
                defectTime = defectTime.AddSeconds(86399.9);
                if (effectTime > defectTime)
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "失效时间应大于生效时间";
                    return;
                }
                
                Framework.FieldVerifyResult result;
                CenterOutputPolicy centerOutputPolicy = new CenterOutputPolicy();
                centerOutputPolicy.TaskID = dplTask.SelectedValue;
                centerOutputPolicy.SatName = dplSatellite.SelectedValue;
                centerOutputPolicy.InfoSource = Convert.ToInt32(dplSource.SelectedValue);
                centerOutputPolicy.InfoType = Convert.ToInt32(dplInfoType.SelectedValue);
                centerOutputPolicy.Destination = Convert.ToInt32(dplDdestination.SelectedValue);
                centerOutputPolicy.EffectTime = effectTime;
                centerOutputPolicy.DefectTime = defectTime;
                centerOutputPolicy.Note = txtNote.Text.Trim();
                centerOutputPolicy.CreatedTime = DateTime.Now;
                centerOutputPolicy.CreatedUserID = LoginUserInfo.Id;
                if (centerOutputPolicy.HaveEffectivePolicy())
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "任务" + dplTask.SelectedItem.Text + "在该时间范围已经存在中心输出策略";
                    return;
                }

                result = centerOutputPolicy.Add();
                switch (result)
                {
                    case Framework.FieldVerifyResult.Error:
                        msg = "发生了数据错误，无法完成请求的操作。";
                        break;
                    case Framework.FieldVerifyResult.Success:
                        msg = "添加中心输出策略成功。";
                        ResetControls();
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
                throw (new AspNetException("新增中心输出策略页面btnSubmit_Click方法出现异常，异常原因", ex));
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                ResetControls();
            }
            catch (System.Threading.ThreadAbortException ex1)
            { }
            catch (Exception ex)
            {
                trMessage.Visible = true;
                lblMessage.Text = "发生未知错误，操作失败。";
                throw (new AspNetException("新增中心输出策略页面btnReset_Click方法出现异常，异常原因", ex));
            }
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            try
            {
                string url = @"~/Views/BusinessManage/CenterOutputPolicyManage.aspx";
                Response.Redirect(url);
            }
            catch (System.Threading.ThreadAbortException ex1)
            { }
            catch (Exception ex)
            {
                trMessage.Visible = true;
                lblMessage.Text = "发生未知错误，操作失败。";
                throw (new AspNetException("新增中心输出策略页面btnReturn_Click方法出现异常，异常原因", ex));
            }
        }
        
        public override void OnPageLoaded()
        {
            this.ShortTitle = "新增中心输出策略";
            this.SetTitle();
        }

        #region Method
        /// <summary>
        /// 绑定控件数据源
        /// </summary>
        private void InitialPageData()
        {
            dplTask.AllowBlankItem = false;
            dplSatellite.AllowBlankItem = false;
            dplSource.AllowBlankItem = false;
            dplInfoType.AllowBlankItem = false;
            dplDdestination.AllowBlankItem = false;
        }
        /// <summary>
        /// 添加完成后将控件设置为初始状态
        /// </summary>
        private void ResetControls()
        {
            dplTask.SelectedIndex = 0;
            dplSatellite.SelectedIndex = 0;
            dplSource.SelectedIndex = 0;
            dplInfoType.SelectedIndex = 0;
            dplDdestination.SelectedIndex = 0;
            txtEffectTime.Text = string.Empty;
            txtDefectTime.Text = string.Empty;
            txtNote.Text = string.Empty;
        }

        #endregion
    }
}
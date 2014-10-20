#region
//------------------------------------------------------
//Assembly:OperatingManagement.Web
//FileName:CommunicationResourceAdd.cs
//Remark:通信资源添加类
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
    public partial class CommunicationResourceAdd : AspNetPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                trMessage.Visible = false;
                lblMessage.Text = string.Empty;
                if (!IsPostBack)
                {
                    BindDataSource();
                }
            }
            catch (Exception ex)
            {
                trMessage.Visible = true;
                lblMessage.Text = "发生未知错误，操作失败。";
                throw (new AspNetException("新增通信资源页面初始化出现异常，异常原因", ex));
            }
        }
        /// <summary>
        /// 提交添加通信资源记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                string msg = string.Empty;
                if (string.IsNullOrEmpty(txtRouteName.Text.Trim()))
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "线路名称不能为空";
                    return;
                }

                if (string.IsNullOrEmpty(txtRouteCode.Text.Trim()))
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "线路编码不能为空";
                    return;
                }

                if (string.IsNullOrEmpty(dplDirection.SelectedValue))
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "方向不能为空";
                    return;
                }

                if (string.IsNullOrEmpty(txtBandWidth.Text.Trim()))
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "带宽不能为空";
                    return;
                }

                double bandWidth = 0.0;
                if (!double.TryParse(txtBandWidth.Text.Trim(), out bandWidth))
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "带宽格式错误";
                    return;
                }

                Framework.FieldVerifyResult result;
                CommunicationResource communicationResource = new CommunicationResource();
                communicationResource.RouteName = txtRouteName.Text.Trim();
                communicationResource.RouteCode = txtRouteCode.Text.Trim();
                communicationResource.Direction = dplDirection.SelectedValue;
                communicationResource.BandWidth = bandWidth;
                communicationResource.Status = 1;//正常
                communicationResource.CreatedTime = DateTime.Now;
                communicationResource.CreatedUserID = LoginUserInfo.Id;

                if (communicationResource.HaveActiveRouteCode())
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "线路编码已经存在";
                    return;
                }

                result = communicationResource.Add();
                switch (result)
                {
                    case Framework.FieldVerifyResult.Error:
                        msg = "发生了数据错误，无法完成请求的操作。";
                        break;
                    case Framework.FieldVerifyResult.Success:
                        msg = "添加通信资源成功。";
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
                throw (new AspNetException("新增通信资源页面btnSubmit_Click方法出现异常，异常原因", ex));
            }
        }
        /// <summary>
        /// 清除当前控件的值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                throw (new AspNetException("新增通信资源页面btnReset_Click方法出现异常，异常原因", ex));
            }
        }
        /// <summary>
        /// 返回
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnReturn_Click(object sender, EventArgs e)
        {
            try
            {
                string url = @"~/Views/BusinessManage/ComResourceMan.aspx";
                Response.Redirect(url);
            }
            catch (System.Threading.ThreadAbortException ex1)
            { }
            catch (Exception ex)
            {
                trMessage.Visible = true;
                lblMessage.Text = "发生未知错误，操作失败。";
                throw (new AspNetException("新增通信资源页面btnReturn_Click方法出现异常，异常原因", ex));
            }
        }

        public override void OnPageLoaded()
        {
            this.PagePermission = "OMB_ComRes.Add";
            this.ShortTitle = "新增通信资源";
            this.SetTitle();
        }

        #region Method
        /// <summary>
        /// 绑定控件数据源
        /// </summary>
        private void BindDataSource()
        {
            dplDirection.Items.Clear();
            dplDirection.DataSource = SystemParameters.GetSystemParameters(SystemParametersType.CommunicationResourceDirection);
            dplDirection.DataTextField = "key";
            dplDirection.DataValueField = "value";
            dplDirection.DataBind();
            //dplDirection.Items.Insert(0, new ListItem("请选择", ""));
        }
        /// <summary>
        /// 添加完成后将控件设置为初始状态
        /// </summary>
        private void ResetControls()
        {
            dplDirection.SelectedIndex = 0;

            txtRouteName.Text = string.Empty;
            txtRouteCode.Text = string.Empty;
            txtBandWidth.Text = string.Empty;
        }

        #endregion
    }
}
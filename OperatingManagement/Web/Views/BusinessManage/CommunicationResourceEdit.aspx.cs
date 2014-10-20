#region
//------------------------------------------------------
//Assembly:OperatingManagement.Web
//FileName:CommunicationResourceEdit.cs
//Remark:通信资源编辑类
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
    public partial class CommunicationResourceEdit : AspNetPage
    {
        #region 属性
        /// <summary>
        /// 通信资源ID
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
                throw (new AspNetException("编辑通信资源页面初始化出现异常，异常原因", ex));
            }
        }
        /// <summary>
        /// 提交更新通信资源记录
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
                communicationResource.Id = CRID;
                communicationResource = communicationResource.SelectByID();
                if (communicationResource == null)
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "修改的通信资源不存在";
                    return;
                }

                communicationResource.RouteName = txtRouteName.Text.Trim();
                communicationResource.RouteCode = txtRouteCode.Text.Trim();
                communicationResource.Direction = dplDirection.SelectedValue;
                communicationResource.BandWidth = bandWidth;
                //communicationResource.Status = 1;//正常，状态不更新
                //communicationResource.CreatedTime = DateTime.Now;//创建时间不更新
                communicationResource.UpdatedTime = DateTime.Now;
                communicationResource.UpdatedUserID = LoginUserInfo.Id;

                if (communicationResource.HaveActiveRouteCode())
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "线路编码已经存在";
                    return;
                }

                result = communicationResource.Update();
                switch (result)
                {
                    case Framework.FieldVerifyResult.Error:
                        msg = "发生了数据错误，无法完成请求的操作。";
                        break;
                    case Framework.FieldVerifyResult.Success:
                        msg = "修改通信资源成功。";
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
                throw (new AspNetException("编辑通信资源页面btnSubmit_Click方法出现异常，异常原因", ex));
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
                throw (new AspNetException("编辑通信资源页面btnReset_Click方法出现异常，异常原因", ex));
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
                throw (new AspNetException("编辑通信资源页面btnReturn_Click方法出现异常，异常原因", ex));
            }
        }

        public override void OnPageLoaded()
        {
            this.PagePermission = "OMB_ComRes.Edit";
            this.ShortTitle = "编辑通信资源";
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
        /// 绑定控件值
        /// </summary>
        private void BindControls()
        {
            CommunicationResource communicationResource = new CommunicationResource();
            communicationResource.Id = CRID;
            communicationResource = communicationResource.SelectByID();
            if (communicationResource != null)
            {
                txtRouteName.Text = communicationResource.RouteName;
                txtRouteCode.Text = communicationResource.RouteCode;
                dplDirection.SelectedValue = communicationResource.Direction;
                txtBandWidth.Text = communicationResource.BandWidth.ToString();
                lblCreatedTime.Text = communicationResource.CreatedTime.ToString("yyyy-MM-dd HH:mm:ss");
                lblUpdatedTime.Text = communicationResource.UpdatedTime == DateTime.MinValue ? communicationResource.CreatedTime.ToString("yyyy-MM-dd HH:mm:ss") : communicationResource.UpdatedTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }

        #endregion
    }
}
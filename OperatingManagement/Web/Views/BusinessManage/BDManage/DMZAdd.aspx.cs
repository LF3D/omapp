using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using OperatingManagement.WebKernel.Basic;
using OperatingManagement.Framework.Core;
using OperatingManagement.DataAccessLayer.BusinessManage;

namespace OperatingManagement.Web.Views.BusinessManage.BDManage
{
    public partial class DMZAdd : AspNetPage
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
                throw (new AspNetException("新增为卫星页面初始化出现异常，异常原因", ex));
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                string msg = string.Empty;
                #region Check Input box
                if (string.IsNullOrEmpty(txtName.Text.Trim()))
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "名称不能为空";
                    return;
                }

                if (string.IsNullOrEmpty(txtCode.Text.Trim()))
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "编码不能为空";
                    return;
                }

                if (string.IsNullOrEmpty(txtDWCode.Text.Trim()))
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "单位编码不能为空";
                    return;
                }
                #endregion

                Framework.FieldVerifyResult result;
                DMZ oDMZ = new DMZ();
                oDMZ.DMZName = txtName.Text.Trim();
                oDMZ.DMZCode = txtCode.Text.Trim();
                oDMZ.DWCode = txtDWCode.Text.Trim();
                oDMZ.Owner = Convert.ToInt32(rblOwner.SelectedValue);

                result = oDMZ.Add();
                switch (result)
                {
                    case Framework.FieldVerifyResult.Error:
                        msg = "发生了数据错误，无法完成请求的操作。";
                        break;
                    case Framework.FieldVerifyResult.NameDuplicated:
                        msg = "名称已经存在。";
                        break;
                    case Framework.FieldVerifyResult.NameDuplicated2:
                        msg = "编码已经存在。";
                        break;
                    case Framework.FieldVerifyResult.NameDuplicated3:
                        msg = "单位编码已经存在。";
                        break;
                    case Framework.FieldVerifyResult.Success:
                        msg = "添加卫星成功。";
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
                throw (new AspNetException("新增地面站页面btnSubmit_Click方法出现异常，异常原因", ex));
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
                throw (new AspNetException("新增地面站页面btnReset_Click方法出现异常，异常原因", ex));
            }
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            try
            {
                string url = @"~/Views/BusinessManage/BDManage/DMZManage.aspx?millisecond=" + Server.UrlEncode(DateTime.Now.Millisecond.ToString());
                Response.Redirect(url);
            }
            catch (System.Threading.ThreadAbortException ex1)
            { }
            catch (Exception ex)
            {
                trMessage.Visible = true;
                lblMessage.Text = "发生未知错误，操作失败。";
                throw (new AspNetException("新增地面站页面btnReturn_Click方法出现异常，异常原因", ex));
            }
        }

        public override void OnPageLoaded()
        {
            this.PagePermission = "OMB_GSMan.Add";
            this.ShortTitle = "新增地面站";
            this.SetTitle();
        }

        #region Method
        /// <summary>
        /// 绑定控件数据源
        /// </summary>
        private void BindDataSource()
        {
            rblOwner.Items.Clear();
            rblOwner.DataSource = SystemParameters.GetSystemParameters(SystemParametersType.DMZOwner);
            rblOwner.DataTextField = "key";
            rblOwner.DataValueField = "value";
            rblOwner.DataBind();
            rblOwner.SelectedIndex = 0;
        }

        /// <summary>
        /// 添加完成后将控件设置为初始状态
        /// </summary>
        private void ResetControls()
        {
            rblOwner.SelectedIndex = 0;
            txtCode.Text = string.Empty;
            txtDWCode.Text = string.Empty;
            txtName.Text = string.Empty;
        }
        #endregion
    }
}
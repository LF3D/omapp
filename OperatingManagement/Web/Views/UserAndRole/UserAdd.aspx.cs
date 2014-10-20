using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OperatingManagement.WebKernel.Basic;
using OperatingManagement.Framework.Core;
using System.Drawing;

namespace OperatingManagement.Web.Views.UserAndRole
{
    public partial class UserAdd : AspNetPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) {
                RenderRoleNav();
            }
        }

        void RenderRoleNav() {
            if (string.IsNullOrEmpty(hfUserId.Value))
                ltHref.Text = "<span title=\"创建用户完成后，可点击此链接为其分配角色。\">[分配角色]</span>";
            else
                ltHref.Text = "<a href=\"userroleedit.aspx?id=" + hfUserId.Value + "\">[分配角色]</a>";
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            DataAccessLayer.System.User u = new DataAccessLayer.System.User()
            {
                DisplayName = txtDisplayName.Text.Trim(),
                LoginName = txtLoginName.Text.Trim(),
                Mobile = txtMobile.Text.Trim(),
                Note = txtNote.Text.Trim(),
                Password = txtPassword.Text.Trim(),
                Status = (Framework.FieldStatus)Convert.ToInt32(rdlStatus.SelectedValue),
                UserType = (Framework.UserType)Convert.ToInt32(rdlTypes.SelectedValue),
                UserCatalog = (Framework.UserCatalog)Convert.ToInt32(rdlUserCat.SelectedValue)
            };
            var result = Framework.FieldVerifyResult.Error;
            try
            {
                result = u.Add();
            }
            catch (Exception ex)
            {
                throw (new AspNetException("新增用户页面保存用户出现异常，异常原因", ex));
            }
            finally { }
            string msg = string.Empty;
            switch (result)
            {
                case Framework.FieldVerifyResult.NameDuplicated:
                    msg = "已存在相同名称，请输入其他“登录名”。";
                    break;
                case Framework.FieldVerifyResult.Error:
                    msg = "发生了数据错误，无法完成请求的操作。";
                    break;
                case Framework.FieldVerifyResult.Success:
                    msg = "新增用户已成功，可点击“分配角色”按钮为其设置所属角色。";
                    hfUserId.Value = u.Id.ToString();
                    txtLoginName.Text = txtDisplayName.Text = txtMobile.Text = txtNote.Text = string.Empty;
                    rdlStatus.SelectedIndex = rdlTypes.SelectedIndex = 0;
                    break;
                case Framework.FieldVerifyResult.NameDuplicated2:
                    msg = "已存在相同名称，请输入其他“显示名称”。";
                    break;
            }
            ltMessage.Text = msg;
            RenderRoleNav();
        }

        public override void OnPageLoaded()
        {
            this.PagePermission = "OMUserManage.Add";
            this.ShortTitle = "新增用户";
            this.SetTitle();
        }

        protected void btnEmpty_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect(Request.CurrentExecutionFilePath);
        }

    }
}
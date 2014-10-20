using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OperatingManagement.WebKernel.Basic;
using OperatingManagement.Framework.Core;

namespace OperatingManagement.Web.Views.UserAndRole
{
    public partial class UserEdit : AspNetPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) {
                BindUser();
            }
        }
        void BindUser()
        {
            double userId = Convert.ToDouble(Request.QueryString["Id"]);
            DataAccessLayer.System.User u = new DataAccessLayer.System.User() { Id = userId };
            if (u.UserType == Framework.UserType.Admin && Profile.UserName != u.LoginName)
            {
                throw new Exception("You cant edit administrator's profile.");
            }
            var user = new DataAccessLayer.System.User();
            try
            {
                user = u.SelectById();
            }
            catch (Exception ex)
            {
                throw (new AspNetException("修改用户页面读取用户数据出现异常，异常原因", ex));
            }
            finally { }

            ltLoginName.Text = user.LoginName;
            txtDisplayName.Text = user.DisplayName;
            txtMobile.Text = user.Mobile;
            txtNote.Text = user.Note;
            rdlStatus.SelectedValue = ((int)user.Status).ToString();
            rdlTypes.SelectedValue = ((int)user.UserType).ToString();
            rdlUserCat.SelectedValue = ((int)user.UserCatalog).ToString();

            ltHref.Text = "<a href=\"userroleedit.aspx?id=" + userId.ToString() + "\">[分配角色]</a>";
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            DataAccessLayer.System.User u = new DataAccessLayer.System.User()
            {
                Id=Convert.ToDouble(Request.QueryString["Id"]),
                DisplayName = txtDisplayName.Text.Trim(),
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
                result = u.Update();
            }
            catch (Exception ex)
            {
                throw (new AspNetException("修改用户页面保存用户数据出现异常，异常原因", ex));
            }
            finally { }
            
            string msg = string.Empty;
            switch (result)
            {
                case Framework.FieldVerifyResult.Error:
                    msg = "发生了数据错误，无法完成请求的操作。";
                    break;
                case Framework.FieldVerifyResult.Success:
                    msg = "编辑用户已成功，可点击“分配角色”按钮为其设置所属角色。";
                    break;
                case Framework.FieldVerifyResult.NameDuplicated2:
                    msg = "已存在相同名称，请输入其他“显示名称”。";
                    break;
            }
            ltMessage.Text = msg;
        }
        public override void OnPageLoaded()
        {
            this.PagePermission = "OMUserManage.Edit";
            this.ShortTitle = "编辑用户";
            this.SetTitle();
            //this.AddJavaScriptInclude("scripts/pages/usernrole/useredit.aspx.js");
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect("users.aspx");
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect(Request.CurrentExecutionFilePath + "?id=" + Request.QueryString["Id"]);
        }

    }
}
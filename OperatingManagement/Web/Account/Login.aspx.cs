using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OperatingManagement.WebKernel.Basic;
using OperatingManagement.WebKernel.Route;
using OperatingManagement.Framework.Core;
using OperatingManagement.DataAccessLayer;
using OperatingManagement.Framework;
using System.Web.Security;

namespace OperatingManagement.Web.Account
{
    public partial class Login : AspNetPage,IRouteContext
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public override void OnPageLoaded()
        {
            this.ShortTitle = "用户登录";
            base.OnPageLoaded();
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            DataAccessLayer.System.User u = new DataAccessLayer.System.User()
            {
                LoginName = txtLoginName.Text.Trim().ToLower(),
                Password = txtPassword.Text.Trim()
            };
            if (!u.IsValid) {
                lblMessage.Text = u.FirstValidationMessage;
                lblMessage.Visible = true;
                return;
            }
            FieldVerifyResult result = u.Verify();
            string outMsg = string.Empty;
            switch (result)
            {
                case FieldVerifyResult.NotExist:
                    outMsg = "不存在此用户。";
                    break;
                case FieldVerifyResult.PasswordIncorrect:
                    outMsg = "用户名和密码不匹配。";
                    break;
                case FieldVerifyResult.Inactive:
                    outMsg = "用户状态不正常，无法登陆。";
                    break;
                case FieldVerifyResult.Error:
                    outMsg = "内部错误，无法登录。";
                    break;
                case FieldVerifyResult.Success:
                    HttpCookie cookie = FormsAuthentication.GetAuthCookie(u.LoginName, true);
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                    FormsAuthenticationTicket newticket = new FormsAuthenticationTicket(
                                                                                    ticket.Version,
                                                                                    ticket.Name,
                                                                                    ticket.IssueDate,
                                                                                    ticket.Expiration,
                                                                                    ticket.IsPersistent,
                                                                                    DateTime.Now.ToShortDateString());
                    cookie.Value = FormsAuthentication.Encrypt(newticket);
                    AspNetCookie.AddCookie(cookie);
                    string url = FormsAuthentication.GetRedirectUrl(u.LoginName, true);
                    
                    Response.Redirect("~/index.aspx");
                    
                    //Response.Redirect(url);
                    return;
            }
            lblMessage.Text = outMsg;
            lblMessage.Visible = true;
        }

    }
}
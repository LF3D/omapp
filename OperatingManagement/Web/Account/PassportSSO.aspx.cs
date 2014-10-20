using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OperatingManagement.WebKernel.Basic;
using OperatingManagement.Framework;
using OperatingManagement.Framework.Core;

namespace OperatingManagement.Web.Account
{
    public partial class PassportSSO : AspNetPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string callback = Request.QueryString["callback"];
            if (string.IsNullOrEmpty(callback)) {
                throw new Exception("Url必须提供callback参数。");
            }
        }
        public override void OnPageLoaded()
        {
            this.ShortTitle = "用户统一认证";
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
                    string split = "$";
                    string token = u.LoginName + split +
                        GlobalSettings.EncryptPassword(u.Password) + split + 
                        DateTime.Now.Ticks.ToString();
                    string encryptedToken = GlobalSettings.Encrypt(token);
                    string callback = Request.QueryString["callback"];
                    string retUrl = callback;
                    if (callback.IndexOf('?') > 0)
                    {
                        if (callback.LastIndexOf('&') != callback.Length - 1)
                            retUrl += "&";
                    }
                    else
                        retUrl += "?";

                    retUrl += "token=" + encryptedToken;
                    Response.Redirect(retUrl);
                    return;
            }
            lblMessage.Text = outMsg;
            lblMessage.Visible = true;
        }

    }
}
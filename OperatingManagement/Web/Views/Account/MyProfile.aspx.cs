using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OperatingManagement.WebKernel.Basic;

namespace OperatingManagement.Web.Views.Account
{
    public partial class MyProfile : AspNetPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindUser();
            }
        }
        void BindUser()
        {
            ltLoginName.Text = Profile.Account.LoginName;
            txtDisplayName.Text = Profile.Account.DisplayName;
            txtMobile.Text = Profile.Account.Mobile;
            txtNote.Text = Profile.Account.Note;
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            DataAccessLayer.System.User u = new DataAccessLayer.System.User()
            {
                Id = Profile.Account.Id,
                DisplayName = txtDisplayName.Text.Trim(),
                Mobile = txtMobile.Text.Trim(),
                Note = txtNote.Text.Trim(),
                Password = txtPassword.Text.Trim(),
                Status = Profile.Account.Status,
                UserCatalog = Profile.Account.UserCatalog,
                UserType = Profile.Account.UserType
            };
            var result = u.Update();
            string msg = string.Empty;
            switch (result)
            {
                case Framework.FieldVerifyResult.Error:
                    msg = "发生了数据错误，无法完成请求的操作。";
                    break;
                case Framework.FieldVerifyResult.Success:
                    msg = "已成功修改用户信息。";
                    break;
                case Framework.FieldVerifyResult.NameDuplicated2:
                    msg = "已存在相同名称，请输入其他“显示名称”。";
                    break;
            }
            ltMessage.Text = msg;
        }
        public override void OnPageLoaded()
        {
            this.ShortTitle = "修改信息";
            this.SetTitle();
        }

    }
}
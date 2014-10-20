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
    public partial class UserRoleEdit : AspNetPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindRole();
                BindUser();
            }
        }

        void BindUser() {
            hfUserID.Value = Request.QueryString["Id"];
            DataAccessLayer.System.User u = new DataAccessLayer.System.User()
            {
                Id = Convert.ToDouble(Request.QueryString["Id"])
            };
            var user = new DataAccessLayer.System.User();
            try
            {
                user = u.SelectById();
            }
            catch (Exception ex)
            {
                throw (new AspNetException("为用户指定角色页面获取用户信息出现异常，异常原因", ex));
            }
            finally { }
            
            ltUserName.Text = user.DisplayName;
            var roles = new List<DataAccessLayer.System.Role>();
            try
            {
                roles = u.SelectRolesById();
            }
            catch (Exception ex)
            {
                throw (new AspNetException("为用户指定角色页面获取用户角色信息出现异常，异常原因", ex));
            }
            finally { }

            string strRoles = string.Empty;
            if (roles.Count > 0)
            {
                foreach (var r in roles)
                {
                    strRoles += "[" + r.Id.ToString() + "]";
                }
            }
            hfRoles.Value = strRoles;
        }

        void BindRole()
        {
            DataAccessLayer.System.Role r = new DataAccessLayer.System.Role();
            var roles = new List<DataAccessLayer.System.Role>();
            try
            {
                roles = r.SelectAll();
            }
            catch (Exception ex)
            {
                throw (new AspNetException("为用户指定角色页面获取所有角色信息出现异常，异常原因", ex));
            }
            finally { }
            rpRoles.DataSource = roles;
            rpRoles.DataBind();
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            DataAccessLayer.System.User u = new DataAccessLayer.System.User()
            {
                Id = Convert.ToDouble(Request.QueryString["Id"])
            };
            string[] roles = hfRoles.Value.Split(new string[] { "][" }, StringSplitOptions.RemoveEmptyEntries);
            string strRoles = string.Empty;
            string msg = string.Empty;
            if (roles.Length > 0)
            {
                foreach (var s in roles)
                {
                    strRoles += s.TrimStart('[').TrimEnd(']') + ",";
                }
                if (strRoles != string.Empty)
                {
                    strRoles = strRoles.Substring(0, strRoles.Length - 1);
                }
                bool result ;
                try
                {
                    result = u.AddToRoles(strRoles);
                }
                catch (Exception ex)
                {
                    throw (new AspNetException("为用户指定角色出现异常，异常原因", ex));
                }
                finally { }

                if (result)
                {
                    msg = "已成功为指定用户分配角色。";
                }
                else
                {
                    msg = "发生了数据错误，无法完成请求的操作。";
                }
            }
            else
                msg = "没有为用户指定任何角色。";
            ltMessage.Text = msg;
        }
        public override void OnPageLoaded()
        {
            this.PagePermission = "OMUserManage.Add,OMUserManage.Edit";
            this.ShortTitle = "分配角色";
            this.SetTitle();
            this.AddJavaScriptInclude("scripts/pages/usernrole/userroleedit.aspx.js");
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect("UserEdit.aspx?Id="+hfUserID.Value);
        }

    }
}
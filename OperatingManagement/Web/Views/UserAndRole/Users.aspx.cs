using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OperatingManagement.WebKernel.Basic;
using OperatingManagement.Framework.Core;
using OperatingManagement.Security;

namespace OperatingManagement.Web.Views.UserAndRole
{
    public partial class Users : AspNetPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                InitialPageData();
            cpPager.PostBackPage += new EventHandler(cpPager_PostBackPage);
        }

        private void InitialPageData()
        {
            try
            {
                DataAccessLayer.System.User u = new DataAccessLayer.System.User();
                List<DataAccessLayer.System.User> users = u.SelectAll();
                BindUsers(users);
            }
            catch (Exception ex)
            {
                throw (new AspNetException("用户列表页面初始化出现异常，异常原因", ex));
            }
            finally { }
        }

        void BindUsers(List<DataAccessLayer.System.User> users)
        {
            cpPager.DataSource = users;
            cpPager.PageSize = this.SiteSetting.PageSize;
            if (users.Count > this.SiteSetting.PageSize)
                cpPager.Visible = true;
            cpPager.BindToControl = rpUsers;
            rpUsers.DataSource = cpPager.DataSourcePaged;
            rpUsers.DataBind();
        }

        /// <summary>
        /// 检查是否当前用户或者用户列表用户是否为管理员，又增加检查是否有删除权限
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="userType"></param>
        /// <returns></returns>
        protected bool IsAdminOrCurrent(object loginName, object userType)
        {
            bool isMyself = loginName.ToString().Equals(Profile.UserName, StringComparison.InvariantCultureIgnoreCase);
            
            bool isAdmin = userType.ToString().Equals(Framework.UserType.Admin.ToString(), StringComparison.InvariantCultureIgnoreCase);
            if (isMyself)
                return isMyself;

            if (HasDeletePermission())
                return isAdmin;
            else
                return true;
        }

        /// <summary>
        /// 判断列表中的用户是否为管理员
        /// </summary>
        /// <param name="loginName"></param>
        /// <param name="userType"></param>
        /// <returns></returns>
        protected bool IsAdmin(object loginName, object userType)
        {
            bool isUserAdmin = userType.ToString().Equals(Framework.UserType.Admin.ToString(), StringComparison.InvariantCultureIgnoreCase);
            return isUserAdmin;
        }

        protected bool HasDeletePermission()
        {
            AspNetPrincipal principal = (AspNetPrincipal)HttpContext.Current.User;
            if (Profile.Account.UserType == Framework.UserType.Admin)//当前用户是管理员
                return true;
            bool blResult = principal.Permissions.Any(o => o.Module.ModuleName == "OMUserManage" && o.Task.TaskName == "Delete");
            return blResult;
        }

        public override void OnPageLoaded()
        {
            this.PagePermission = "OMUserManage.View";
            this.ShortTitle = "查看用户";
            this.SetTitle();
            this.AddJavaScriptInclude("scripts/pages/usernrole/users.aspx.js");
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            SaveCondition();
            SearchUsers(true);
        }

        protected void cpPager_PostBackPage(object sender, EventArgs e)
        {
            SearchUsers(false);
        }

        private void SearchUsers(bool fromSearch)
        {
            DataAccessLayer.System.User u = new DataAccessLayer.System.User();
            List<DataAccessLayer.System.User> users = null;
            try
            {
                if (fromSearch)
                {
                    if (txtKeyword.Text.Trim() != string.Empty)
                        users = u.Search(txtKeyword.Text.Trim().ToLower());
                    else
                        users = u.SelectAll();
                    cpPager.CurrentPage = 1;
                }
                else
                {
                    if (ViewState["_KeyWord"] == null)
                        users = u.SelectAll();
                    else
                        users = u.Search(ViewState["_KeyWord"].ToString());
                }
                BindUsers(users);
            }
            catch (Exception ex)
            {
                throw (new AspNetException("用户列表页面搜索出现异常，异常原因", ex));
            }
            finally { }
        }

        private void SaveCondition()
        {
            ViewState["_KeyWord"] = txtKeyword.Text.Trim().ToLower();
        }
    }
}
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
    public partial class Roles : AspNetPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
                BindRolers();
            cpPager.PostBackPage += new EventHandler(cpPager_PostBackPage);
        }
        void BindRolers()
        {
            DataAccessLayer.System.Role r = new DataAccessLayer.System.Role();
            List<DataAccessLayer.System.Role> roles;
            try
            {
                roles = r.SelectAll();
            }
            catch (Exception ex)
            {
                throw (new AspNetException("角色列表页面初始化出现异常，异常原因", ex));
            }
            finally { }
            cpPager.DataSource = roles;
            cpPager.PageSize = this.SiteSetting.PageSize;
            cpPager.BindToControl = rpRoles;
            rpRoles.DataSource = cpPager.DataSourcePaged;
            rpRoles.DataBind();
        }
        public override void OnPageLoaded()
        {
            this.PagePermission = "OMRoleManage.View";
            this.ShortTitle = "查看角色";
            this.SetTitle();
            this.AddJavaScriptInclude("scripts/pages/usernrole/roles.aspx.js");
        }

        protected bool HasDeletePermission()
        {
            AspNetPrincipal principal = (AspNetPrincipal)HttpContext.Current.User;
            if (Profile.Account.UserType == Framework.UserType.Admin)//当前用户是管理员
                return false;
            string[] ps = "OMRoleManage.Delete".Split(new char[] { '.' });
            bool blResult = principal.Permissions.Any(o => o.Module.ModuleName == ps[0] && o.Task.TaskName == ps[1]);
            return !blResult;
        }

        protected void cpPager_PostBackPage(object sender, EventArgs e)
        {
            BindRolers();
        }
    }
}
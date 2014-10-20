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
    public partial class Permissions : AspNetPage
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
                DataAccessLayer.System.Module r = new DataAccessLayer.System.Module();
                List<DataAccessLayer.System.Module> modules = r.SelectAll();
                BindDataSource(modules);
            }
            catch (Exception ex)
            {
                throw (new AspNetException("权限列表页面初始化出现异常，异常原因", ex));
            }
            finally { }
        }

        private void BindDataSource(List<DataAccessLayer.System.Module> modules)
        {
            cpPager.DataSource = modules;
            cpPager.PageSize = this.SiteSetting.PageSize;
            cpPager.BindToControl = rpRermissions;
            if (modules.Count > this.SiteSetting.PageSize)
                cpPager.Visible = true;
            rpRermissions.DataSource = cpPager.DataSourcePaged;
            rpRermissions.DataBind();
        }
        
        public override void OnPageLoaded()
        {
            this.PagePermission = "OMPermissionManage.View";
            this.ShortTitle = "查看权限";
            this.SetTitle();
            this.AddJavaScriptInclude("scripts/pages/usernrole/permissions.aspx.js");
        }

        protected bool HasDeletePermission()
        {
            AspNetPrincipal principal = (AspNetPrincipal)HttpContext.Current.User;
            if (Profile.Account.UserType == Framework.UserType.Admin)//当前用户是管理员
                return false;
            string[] ps = "OMPermissionManage.Delete".Split(new char[] { '.' });
            bool blResult = principal.Permissions.Any(o => o.Module.ModuleName == ps[0] && o.Task.TaskName == ps[1]);
            return !blResult;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            SaveCondition();
            Search(true);
        }

        protected void cpPager_PostBackPage(object sender, EventArgs e)
        {
            Search(false);
        }

        private void Search(bool fromSearch)
        {
            DataAccessLayer.System.Module r = new DataAccessLayer.System.Module();
            List<DataAccessLayer.System.Module> modules = null;
            try
            {
                if (fromSearch)
                {
                    if (txtKeyword.Text != string.Empty)
                        modules = r.Search(txtKeyword.Text.Trim().ToLower());
                    else
                        modules = r.SelectAll();
                    cpPager.CurrentPage = 1;
                }
                else
                {
                    if (ViewState["_KeyWord"] == null)
                        modules = r.SelectAll();
                    else
                        modules = r.Search(ViewState["_KeyWord"].ToString());
                }
                BindDataSource(modules);
            }
            catch (Exception ex)
            {
                throw (new AspNetException("权限搜索过程中出现异常，异常原因", ex));
            }
            finally { }
        }

        private void SaveCondition()
        {
            ViewState["_KeyWord"] = txtKeyword.Text.Trim().ToLower();
        }
    }
}
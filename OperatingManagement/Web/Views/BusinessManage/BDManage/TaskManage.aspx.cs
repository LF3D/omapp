using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data;

using OperatingManagement.WebKernel.Basic;
using OperatingManagement.WebKernel.Route;
using OperatingManagement.Framework.Core;
using OperatingManagement.DataAccessLayer;
using OperatingManagement.Framework;
using OperatingManagement.DataAccessLayer.BusinessManage;
using ServicesKernel.File;

namespace OperatingManagement.Web.Views.BusinessManage.BDManage
{
    public partial class TaskManage : AspNetPage, IRouteContext
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
                DataAccessLayer.BusinessManage.Task t = new DataAccessLayer.BusinessManage.Task();
                List<DataAccessLayer.BusinessManage.Task> tasks = t.SelectAll();
                BindTasks(tasks);
            }
            catch (Exception ex)
            {
                throw (new AspNetException("任务列表页面初始化出现异常，异常原因", ex));
            }
            finally { }
        }

        void BindTasks(List<DataAccessLayer.BusinessManage.Task> tasks)
        {
            cpPager.DataSource = tasks;
            cpPager.PageSize = this.SiteSetting.PageSize;
            if (tasks.Count > this.SiteSetting.PageSize)
                cpPager.Visible = true;
            cpPager.BindToControl = rpTasks;
            rpTasks.DataSource = cpPager.DataSourcePaged;
            rpTasks.DataBind();
        }

        public override void OnPageLoaded()
        {
            this.PagePermission = "OMB_TaskMan.View";
            this.ShortTitle = "查看任务";
            this.SetTitle();
        }

        protected void cpPager_PostBackPage(object sender, EventArgs e)
        {
            SearchTasks(false);
        }

        private void SearchTasks(bool fromSearch)
        {
            DataAccessLayer.BusinessManage.Task t = new DataAccessLayer.BusinessManage.Task();
            List<DataAccessLayer.BusinessManage.Task> tasks = null;
            try
            {
                tasks = t.SelectAll();
                BindTasks(tasks);
            }
            catch (Exception ex)
            {
                throw (new AspNetException("任务列表页面翻页出现异常，异常原因", ex));
            }
            finally { }
        }
    }
}
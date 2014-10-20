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
    public partial class ZYGNManage : AspNetPage, IRouteContext
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
                DataAccessLayer.BusinessManage.ZYGN t = new DataAccessLayer.BusinessManage.ZYGN();
                List<DataAccessLayer.BusinessManage.ZYGN> zys = t.SelectAll();
                BindZYGNs(zys);
            }
            catch (Exception ex)
            {
                throw (new AspNetException("资源功能列表页面初始化出现异常，异常原因", ex));
            }
            finally { }
        }

        void BindZYGNs(List<DataAccessLayer.BusinessManage.ZYGN> zy)
        {
            cpPager.DataSource = zy;
            cpPager.PageSize = this.SiteSetting.PageSize;
            if (zy.Count > this.SiteSetting.PageSize)
                cpPager.Visible = true;
            cpPager.BindToControl = rpZY;
            rpZY.DataSource = cpPager.DataSourcePaged;
            rpZY.DataBind();
        }

        public override void OnPageLoaded()
        {
            this.PagePermission = "OMB_ZYGNMan.View";
            this.ShortTitle = "查看资源功能";
            this.SetTitle();
        }

        protected void cpPager_PostBackPage(object sender, EventArgs e)
        {
            SearchTasks(false);
        }

        private void SearchTasks(bool fromSearch)
        {
            DataAccessLayer.BusinessManage.ZYGN t = new DataAccessLayer.BusinessManage.ZYGN();
            List<DataAccessLayer.BusinessManage.ZYGN> zys = null;
            try
            {
                zys = t.SelectAll();
                BindZYGNs(zys);
            }
            catch (Exception ex)
            {
                throw (new AspNetException("资源功能列表页面翻页出现异常，异常原因", ex));
            }
            finally { }
        }
    }
}
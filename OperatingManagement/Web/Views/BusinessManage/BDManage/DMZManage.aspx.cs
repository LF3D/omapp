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
using OperatingManagement.DataAccessLayer.BusinessManage;
using ServicesKernel.File;

namespace OperatingManagement.Web.Views.BusinessManage.BDManage
{
    public partial class DMZManage : AspNetPage, IRouteContext
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
                DataAccessLayer.BusinessManage.DMZ t = new DataAccessLayer.BusinessManage.DMZ();
                List<DataAccessLayer.BusinessManage.DMZ> dmzs = t.SelectAll();
                BindDatas(dmzs);
            }
            catch (Exception ex)
            {
                throw (new AspNetException("地面站列表页面初始化出现异常，异常原因", ex));
            }
            finally { }
        }

        void BindDatas(List<DataAccessLayer.BusinessManage.DMZ> datas)
        {
            cpPager.DataSource = datas;
            cpPager.PageSize = this.SiteSetting.PageSize;
            if (datas.Count > this.SiteSetting.PageSize)
                cpPager.Visible = true;
            cpPager.BindToControl = rpTasks;
            rpTasks.DataSource = cpPager.DataSourcePaged;
            rpTasks.DataBind();
        }

        public override void OnPageLoaded()
        {
            this.PagePermission = "OMB_GSMan.View";
            this.ShortTitle = "查看地面站";
            this.SetTitle();
        }

        protected void cpPager_PostBackPage(object sender, EventArgs e)
        {
            SearchDatas(false);
        }

        private void SearchDatas(bool fromSearch)
        {
            DataAccessLayer.BusinessManage.DMZ t = new DataAccessLayer.BusinessManage.DMZ();
            List<DataAccessLayer.BusinessManage.DMZ> dmzs = t.SelectAll();
            try
            {
                dmzs = t.SelectAll();
                BindDatas(dmzs);
            }
            catch (Exception ex)
            {
                throw (new AspNetException("地面站列表页面翻页出现异常，异常原因", ex));
            }
            finally { }
        }
    }
}
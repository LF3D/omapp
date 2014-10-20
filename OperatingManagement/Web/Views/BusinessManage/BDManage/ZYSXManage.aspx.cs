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
    public partial class ZYSXManage : AspNetPage, IRouteContext
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
                DataAccessLayer.BusinessManage.ZYSX t = new DataAccessLayer.BusinessManage.ZYSX();
                List<DataAccessLayer.BusinessManage.ZYSX> zys = t.SelectAll();
                BindZYSXs(zys);
            }
            catch (Exception ex)
            {
                throw (new AspNetException("资源属性列表页面初始化出现异常，异常原因", ex));
            }
            finally { }
        }

        void BindZYSXs(List<DataAccessLayer.BusinessManage.ZYSX> zy)
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
            this.PagePermission = "OMB_ZYSXMan.View";
            this.ShortTitle = "查看资源属性";
            this.SetTitle();
        }

        protected void cpPager_PostBackPage(object sender, EventArgs e)
        {
            SearchTasks(false);
        }

        private void SearchTasks(bool fromSearch)
        {
            DataAccessLayer.BusinessManage.ZYSX t = new DataAccessLayer.BusinessManage.ZYSX();
            List<DataAccessLayer.BusinessManage.ZYSX> zys = null;
            try
            {
                zys = t.SelectAll();
                BindZYSXs(zys);
            }
            catch (Exception ex)
            {
                throw (new AspNetException("资源属性列表页面翻页出现异常，异常原因", ex));
            }
            finally { }
        }

        protected string GetTypeName(string type)
        {
            string result = "";
            switch (type)
            { 
                case "1":
                    result = "int";
                    break;
                case "2":
                    result = "double";
                    break;
                case "3":
                    result = "string";
                    break;
                case "4":
                    result = "bool";
                    break;
                case "5":
                    result = "enum";
                    break;
            }
            return result;
        }

        protected string GetOwnName(string type)
        {//0(卫星);1(地面站资源);2(卫星和地面站资源);3(都不归属)
            string result = "";
            switch (type)
            {
                case "0":
                    result = "卫星";
                    break;
                case "1":
                    result = "地面站资源";
                    break;
                case "2":
                    result = "卫星和地面站资源";
                    break;
                case "3":
                    result = "都不归属";
                    break;
            }
            return result;
        }
    }
}
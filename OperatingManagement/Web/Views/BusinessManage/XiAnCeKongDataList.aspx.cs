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
using System.Web.Security;
using System.Data;

namespace OperatingManagement.Web.Views.BusinessManage
{
    public partial class XiAnCeKongDataList : AspNetPage, IRouteContext
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public override void OnPageLoaded()
        {
            this.PagePermission = "XiAnCeKongData.List";
            this.ShortTitle = "查看西安卫星测控中心数据";
            this.SetTitle();
            this.AddJavaScriptInclude("scripts/pages/XiAnCeKongDataList.aspx.js");
        }


        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string dataType = ddlType.SelectedValue;
            BindGridView(dataType);
        }

        //绑定列表
        void BindGridView(string dtype)
        {
            DateTime startDate = new DateTime();
            DateTime endDate = new DateTime();
            if (!string.IsNullOrEmpty(txtStartDate.Text))
            {
                startDate = Convert.ToDateTime(txtStartDate.Text);
            }
            if (!string.IsNullOrEmpty(txtEndDate.Text))
            {
                endDate = Convert.ToDateTime(txtEndDate);
            }

            switch (dtype)
            {
                case "tb_gdxa":
                    List<GD> listDatasGDXA = (new GD()).GetListByDate(startDate, endDate);
                    cpPager.DataSource = listDatasGDXA;
                    break;
                case "tb_gdsh":
                    //List<GDSH> listDatasGDSH = (new GDSH()).GetListByDate(startDate, endDate);
                    //cpPager.DataSource = listDatasGDSH;
                    break;
                case "tb_xdsc":
                    List<XDSC> listDatasXDSC = (new XDSC()).GetListByDate(startDate, endDate);
                    cpPager.DataSource = listDatasXDSC;
                    break;
                case "tb_T0":
                    List<T0> listDatasT0 = (new T0()).GetListByDate(startDate, endDate);
                    cpPager.DataSource = listDatasT0;
                    break;
            }

            //cpPager.DataSource = listDatas;
            cpPager.PageSize = this.SiteSetting.PageSize;
            cpPager.BindToControl = rpDatas;
            rpDatas.DataSource = cpPager.DataSourcePaged;
            rpDatas.DataBind();

        }

        //

    }
}
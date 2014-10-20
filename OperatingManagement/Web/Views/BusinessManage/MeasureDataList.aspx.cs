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
    public partial class MeasureDataList : AspNetPage, IRouteContext
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public override void OnPageLoaded()
        {
            this.PagePermission = "MeasureData.List";
            this.ShortTitle = "查看测角测速测距数据";
            this.SetTitle();
            this.AddJavaScriptInclude("scripts/pages/MeasureDataList.aspx.js");
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
                case "tb_ae":
                    List<AE> listDatasAE = (new AE()).GetListByDate(startDate, endDate);
                    cpPager.DataSource = listDatasAE;
                    break;
                case "tb_rr":
                    List<RR> listDatasRR = (new RR()).GetListByDate(startDate, endDate);
                    cpPager.DataSource = listDatasRR;
                    break;
                case "tb_r":
                    List<R> listDatasR = (new R()).GetListByDate(startDate, endDate);
                    cpPager.DataSource = listDatasR;
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
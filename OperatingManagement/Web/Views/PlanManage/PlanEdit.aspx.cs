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
using OperatingManagement.DataAccessLayer.PlanManage;
using OperatingManagement.Framework;
using System.Web.Security;
using System.Xml;

namespace OperatingManagement.Web.Views.PlanManage
{
    public partial class PlanEdit : AspNetPage, IRouteContext
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["planid"]) && !string.IsNullOrEmpty(Request.QueryString["infotype"]))
                {
                    hfinfotype.Value = Request.QueryString["infotype"].ToUpper();
                    HfID.Value = Request.QueryString["planid"];
                    string sID = Request.QueryString["planid"];
                    switch (hfinfotype.Value)
                    {
                        case "YJJH":
                            Response.Redirect("YJJHEdit.aspx?id=" + sID);
                            break;
                        case "XXXQ":
                            Response.Redirect("XXXQEdit.aspx?id=" + sID);
                            break;
                        case "MBXQ":
                            Response.Redirect("MBXQEdit.aspx?id=" + sID);
                            break;
                        case "HJXQ":
                            Response.Redirect("HJXQEdit.aspx?id=" + sID);
                            break;
                        case "DMJH":
                            Response.Redirect("DMJHEdit.aspx?id=" + sID);
                            break;
                        case "ZXJH":
                            Response.Redirect("ZXJHEdit.aspx?id=" + sID);
                            break;
                        case "TYSJ":
                            Response.Redirect("TYSJEdit.aspx?id=" + sID);
                            break;
                    }
                }
            }
        }
        public override void OnPageLoaded()
        {
            this.PagePermission = "OMPLAN_Plan.Edit";
            this.ShortTitle = "编辑计划";
            base.OnPageLoaded();
            this.AddJavaScriptInclude("scripts/pages/PlanManage/PlanEdit.aspx.js");
        }

    }
}
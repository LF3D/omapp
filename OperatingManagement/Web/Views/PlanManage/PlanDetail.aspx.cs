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
using System.Web.Security;
using System.Xml;
//using ServicesKernel.File;

namespace OperatingManagement.Web.Views.PlanManage
{
    public partial class PlanDetail : AspNetPage, IRouteContext
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (this.QueryStringObserver("planid"))
                {
                    //string sID = this.DecryptString(Request.QueryString["planid"]);
                    string sID = Request.QueryString["planid"];
                    int id = 0;
                    Int32.TryParse(sID, out id);
                    BindFileInfo();
                }
            }
        }

        void BindFileInfo()
        {
            //XmlDocument xml = new XmlDocument();
            //xml.Load(HttpContext.Current.Server.MapPath("~/file/test.xml"));
            //txtContent.Text = xml.InnerText;
        }

        public override void OnPageLoaded()
        {
            this.PagePermission = "OMPLAN_Plan.View";
            this.ShortTitle = "计划明细";
            base.OnPageLoaded();
            //this.AddJavaScriptInclude("scripts/pages/");
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            //CreatePlanFile pfile = new CreatePlanFile
            //{
            //    FilePath = @"D:\YJJH_201106031040_YKZX_FZTY.PLA",
            //    CTime = DateTime.Now,
            //    Source = "运控评估中心YKZX(02 04 00 00)",
            //    Destination = "仿真推演分系统FZTY(02 E7 00 00)",
            //    TaskID = "700任务(0500)",
            //    InfoType = "应用研究计划(00 70 06 00)",
            //    LineCount = 3,
            //    Format1 = "XXFL  JXH  SysName  StartTime  EndTime  Task",
            //    DataSection = "RJ  0012  仿真推演分系统  20110608000000  20110609000000  完成TS-4卫星释放抓捕目标试验过程推演"
            //};
            //pfile.NewFile();
        }
    }
}
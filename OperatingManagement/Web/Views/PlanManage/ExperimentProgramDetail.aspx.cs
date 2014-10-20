using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Web;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

using OperatingManagement.WebKernel.Basic;
using OperatingManagement.WebKernel.Route;
using OperatingManagement.Framework.Core;
using OperatingManagement.DataAccessLayer;
using OperatingManagement.Framework;
using System.Web.Security;
using System.Xml;

namespace OperatingManagement.Web.Views.PlanManage
{
    public partial class ExperimentProgramDetail : AspNetPage, IRouteContext
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.QueryStringObserver("id"))
            {
                string sID = Request.QueryString["id"];
                int id = 0;
                Int32.TryParse(sID, out id);
                BindFileInfo(id);
            }
        }

        void BindFileInfo(int id)
        {
            DataAccessLayer.PlanManage.SYCX cx = new DataAccessLayer.PlanManage.SYCX { Id = id };
            DataAccessLayer.PlanManage.SYCX obj = cx.SelectById();
            string fileFullName = System.Configuration.ConfigurationManager.AppSettings["SYCXPath"] + obj.FileIndex.Substring(obj.FileIndex.LastIndexOf(@"\") + 1);
            if (File.Exists(fileFullName))
            {
                XmlDocument xml = new XmlDocument();
                XDocument doc = XDocument.Load(fileFullName);
                //xml.Load(obj.FileIndex);
                txtContent.Text = doc.ToString();
            }
            else
                txtContent.Text = string.Format("文件{0}不存在", fileFullName);
        }

        public override void OnPageLoaded()
        {
            this.PagePermission = "OMPLAN_ExProgram.View";
            this.ShortTitle = "试验程序明细";
            base.OnPageLoaded();
        }

    }
}
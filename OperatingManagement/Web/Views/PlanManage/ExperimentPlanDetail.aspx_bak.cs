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
using System.IO;
using OperatingManagement.DataAccessLayer.PlanManage;
using System.Collections;
using ServicesKernel.File;

namespace OperatingManagement.Web.Views.PlanManage
{
    public partial class ExperimentPlanDetail : AspNetPage, IRouteContext
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    string sID = Request.QueryString["id"];
                    HfID.Value = sID;
                    List<JH> jh = (new JH(false)).SelectByIDS(sID);
                    HfFileIndex.Value = jh[0].FileIndex;
                    BindXML();
                }
            }
        }

        private void BindXML()
        {
            List<SYJH_SY> listTask = new List<SYJH_SY>();
            SYJH_SY task;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(HfFileIndex.Value);
            XmlNode root = xmlDoc.SelectSingleNode("试验计划/时间");
            txtSYTime.Text = root.InnerText;
            root = xmlDoc.SelectSingleNode("试验计划/试验个数");
            txtSYCount.Text = root.InnerText;

            root = xmlDoc.SelectSingleNode("试验计划");
            foreach (XmlNode n in root.ChildNodes)
            {
                if (n.Name == "试验")
                {
                    task = new SYJH_SY();
                    task.SYSatName = n["卫星名称"].InnerText;
                    task.SYType = n["试验类别"].InnerText;
                    task.SYItem = n["试验项目"].InnerText;
                    task.SYStartTime = n["开始时间"].InnerText;
                    task.SYEndTime = n["结束时间"].InnerText;
                    task.SYSysName = n["系统名称"].InnerText;
                    task.SYSysTask = n["系统任务"].InnerText;
                    listTask.Add(task);
                }
            }

            Repeater1.DataSource = listTask;
            Repeater1.DataBind();
        }

        public override void OnPageLoaded()
        {
            this.PagePermission = "OMPLAN_ExPlan.View";
            this.ShortTitle = "实验计划明细";
            base.OnPageLoaded();
        }

    }
}
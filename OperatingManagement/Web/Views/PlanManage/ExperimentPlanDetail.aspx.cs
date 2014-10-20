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
                    HfFileIndex.Value = System.Configuration.ConfigurationManager.AppSettings["SYJHPath"]  + jh[0].FileIndex.Substring(jh[0].FileIndex.LastIndexOf(@"\") + 1) ;
                    BindXML();
                }
            }
        }

        private void BindXML()
        {
            List<SYJH_SY> listTask = new List<SYJH_SY>();
            SYJH_SY task;
            if (!File.Exists(HfFileIndex.Value))
            {
                txtJHID.Text = string.Format("文件{0}不存在", HfFileIndex.Value);
                return;
            }
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(HfFileIndex.Value);
            XmlNode root = xmlDoc.SelectSingleNode("试验计划/编号");
            txtJHID.Text = root.InnerText;
            root = xmlDoc.SelectSingleNode("试验计划/时间");
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
            this.ShortTitle = "试验计划明细";
            base.OnPageLoaded();
        }

        protected void btnCreateFile_Click(object sender, EventArgs e)
        {
            string SendingFilePaths = string.Empty;
            PlanFileCreator creater = new PlanFileCreator();
            HideMsg();
            divFiles.Visible = false;
            try
            {
                SendingFilePaths = creater.CreateSendingSYJHFile(HfID.Value
                    , System.Configuration.ConfigurationManager.AppSettings["ZXBM"]
                    , "OP");
                lblFilePath.Text = SendingFilePaths;
                lbtFilePath_Click(null, null);
                //ShowMsg("文件生成成功。");
                //divFiles.Visible = true;
            }
            catch (Exception ex)
            {
                throw new AspNetException("试验计划-生成文件出现异常，异常原因", ex);
            }
        }

        protected void lbtFilePath_Click(object sender, EventArgs e)
        {
            try
            {
                string strFilePath = lblFilePath.Text.Trim();
                if (string.IsNullOrEmpty(strFilePath) || !File.Exists(strFilePath))
                {
                    ShowMsg("文件不存在。");
                    return;
                }

                Response.Clear();
                Response.Buffer = false;
                Response.ContentType = "application/octet-stream";
                Response.AppendHeader("content-disposition", "attachment;filename=" + Path.GetFileName(strFilePath) + ";");
                Response.Write(File.ReadAllText(strFilePath));
                Response.Flush();
                Response.End();
            }
            catch (System.Threading.ThreadAbortException ex1)
            { }
            catch (Exception ex)
            {
                throw (new AspNetException("试验计划-生成文件出现异常，异常原因", ex));
            }
        }

        private void HideMsg()
        {
            trMsg.Visible = false;
            lblMessage.Text = "";
        }

        private void ShowMsg(string msg)
        {
            trMsg.Visible = true;
            lblMessage.Text = msg;
        }
    }
}
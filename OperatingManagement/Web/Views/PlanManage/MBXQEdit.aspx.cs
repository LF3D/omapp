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
using OperatingManagement.Framework.Storage;
using System.Web.Security;
using System.Xml;
using ServicesKernel.File;

namespace OperatingManagement.Web.Views.PlanManage
{
    public partial class MBXQEdit : AspNetPage, IRouteContext
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    string sID = Request.QueryString["id"];
                    HfID.Value = sID;
                    BindJhTable(sID);
                    BindXML();

                    if ("detail" == Request.QueryString["op"])
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "hide", "<script type='text/javascript'>hideAllButton();</script>");
                    }
                }
            }
        }
        private void BindJhTable(string sID)
        {
            List<JH> jh = (new JH()).SelectByIDS(sID);
            txtPlanStartTime.Text = jh[0].StartTime.ToString("yyyy-MM-dd HH:mm");
            txtPlanEndTime.Text = jh[0].EndTime.ToString("yyyy-MM-dd HH:mm");
            HfFileIndex.Value = jh[0].FileIndex;
            hfTaskID.Value = jh[0].TaskID.ToString();
            string[] strTemp = jh[0].FileIndex.Split('_');
            if (strTemp.Length >= 2)
            {
                hfSatID.Value = strTemp[strTemp.Length - 2];
            }
            if (DateTime.Now > jh[0].StartTime)
            {
                btnSubmit.Visible = false;
                hfOverDate.Value = "true";
            }
        }
        private void BindXML()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(HfFileIndex.Value);
            XmlNode root = xmlDoc.SelectSingleNode("空间目标信息需求/User");
            txtMBUser.Text = root.InnerText;
            root = xmlDoc.SelectSingleNode("空间目标信息需求/Time");
            txtMBTime.Text = root.InnerText;
            root = xmlDoc.SelectSingleNode("空间目标信息需求/TargetInfo");
            txtMBTargetInfo.Text = root.InnerText;
            root = xmlDoc.SelectSingleNode("空间目标信息需求/TimeSection1");
            txtMBTimeSection1.Text = root.InnerText;
            root = xmlDoc.SelectSingleNode("空间目标信息需求/TimeSection2");
            txtMBTimeSection2.Text = root.InnerText;
            root = xmlDoc.SelectSingleNode("空间目标信息需求/Sum");
            txtMBSum.Text = root.InnerText;

            root = xmlDoc.SelectSingleNode("空间目标信息需求");
            List<MBXQSatInfo> list = new List<MBXQSatInfo>();
            MBXQSatInfo sat;
            foreach (XmlNode n in root.ChildNodes)
            {
                if (n.Name == "卫星")
                {
                    sat = new MBXQSatInfo();
                    sat.SatName = n["SatName"].InnerText;
                    sat.InfoName = n["InfoName"].InnerText;
                    sat.InfoTime = n["InfoTime"].InnerText;
                    list.Add(sat);
                }
            }
            rpMB.DataSource = list;
            rpMB.DataBind();

        }
        public override void OnPageLoaded()
        {
            this.PagePermission = "Plan.Edit";
            this.ShortTitle = "编辑计划";
            base.OnPageLoaded();
            this.AddJavaScriptInclude("scripts/pages/PlanManage/MBXQEdit.aspx.js");
        }

        protected void rpMB_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Add")
            {
                List<MBXQSatInfo> list2 = new List<MBXQSatInfo>();
                MBXQSatInfo dm;
                Repeater rp = (Repeater)source;
                foreach (RepeaterItem it in rp.Items)
                {
                    dm = new MBXQSatInfo();
                    TextBox txtMBSatName = (TextBox)it.FindControl("txtMBSatName");
                    TextBox txtMBInfoName = (TextBox)it.FindControl("txtMBInfoName");
                    TextBox txtMBInfoTime = (TextBox)it.FindControl("txtMBInfoTime");

                    dm.SatName = txtMBSatName.Text;
                    dm.InfoName = txtMBInfoName.Text;
                    dm.InfoTime = txtMBInfoTime.Text;
                    list2.Add(dm);
                }
                dm = new MBXQSatInfo();
                dm.SatName = "";
                dm.InfoName = "";
                dm.InfoTime = "";
                list2.Add(dm);
                rp.DataSource = list2;
                rp.DataBind();

            }
            if (e.CommandName == "Del")
            {
                List<MBXQSatInfo> list2 = new List<MBXQSatInfo>();
                MBXQSatInfo dm;
                Repeater rp = (Repeater)source;
                if (rp.Items.Count <= 1)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "del", "<script type='text/javascript'>alert('最后一条，无法删除!');</script>");
                }
                else
                {
                    foreach (RepeaterItem it in rp.Items)
                    {
                        if (e.Item.ItemIndex != it.ItemIndex)
                        {
                            dm = new MBXQSatInfo();
                            TextBox txtMBSatName = (TextBox)it.FindControl("txtMBSatName");
                            TextBox txtMBInfoName = (TextBox)it.FindControl("txtMBInfoName");
                            TextBox txtMBInfoTime = (TextBox)it.FindControl("txtMBInfoTime");

                            dm.SatName = txtMBSatName.Text;
                            dm.InfoName = txtMBInfoName.Text;
                            dm.InfoTime = txtMBInfoTime.Text;
                            list2.Add(dm);
                        }
                    }
                    rp.DataSource = list2;
                    rp.DataBind();
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            MBXQ obj = new MBXQ();
            obj.User = txtMBUser.Text;
            obj.Time = txtMBTime.Text;
            obj.TargetInfo = txtMBTargetInfo.Text;
            obj.TimeSection1 = txtMBTimeSection1.Text;
            obj.TimeSection2 = txtMBTimeSection2.Text;
            //obj.Sum = txtMBSum.Text;
            obj.SatInfos = new List<MBXQSatInfo>();

            MBXQSatInfo dm;
            foreach (RepeaterItem it in rpMB.Items)
            {
                dm = new MBXQSatInfo();
                TextBox txtMBSatName = (TextBox)it.FindControl("txtMBSatName");
                TextBox txtMBInfoName = (TextBox)it.FindControl("txtMBInfoName");
                TextBox txtMBInfoTime = (TextBox)it.FindControl("txtMBInfoTime");

                dm.SatName = txtMBSatName.Text;
                dm.InfoName = txtMBInfoName.Text;
                dm.InfoTime = txtMBInfoTime.Text;

                obj.SatInfos.Add(dm);
            }
            obj.Sum = obj.SatInfos.Count.ToString(); //信息条数，自动计算得到

            PlanFileCreator creater = new PlanFileCreator();
            if (hfOverDate.Value == "true")
            {
                obj.TaskID = hfTaskID.Value;
                obj.SatID = hfSatID.Value;
                string filepath = creater.CreateMBXQFile(obj, 0);

                DataAccessLayer.PlanManage.JH jh = new DataAccessLayer.PlanManage.JH()
                {
                    TaskID = obj.TaskID,
                    PlanType = "MBXQ",
                    PlanID = 0,
                    StartTime = Convert.ToDateTime(txtPlanStartTime.Text.Trim()),
                    EndTime = Convert.ToDateTime(txtPlanEndTime.Text.Trim()),
                    SRCType = 0,
                    FileIndex = filepath,
                    SatID = obj.SatID,
                    Reserve = ""
                };
                var result = jh.Add();
            }
            else
            {
                creater.FilePath = HfFileIndex.Value;
                creater.CreateMBXQFile(obj, 1);
            }

            ClientScript.RegisterStartupScript(this.GetType(), "OK", "<script type='text/javascript'>alert('计划保存成功');</script>");
        }
    }
}
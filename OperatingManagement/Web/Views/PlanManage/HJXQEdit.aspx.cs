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
using ServicesKernel.File;

namespace OperatingManagement.Web.Views.PlanManage
{
    public partial class HJXQEdit : AspNetPage, IRouteContext
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
            XmlNode root = xmlDoc.SelectSingleNode("空间环境信息需求/User");
            txtHJUser.Text = root.InnerText;
            root = xmlDoc.SelectSingleNode("空间环境信息需求/Time");
            txtHJTime.Text = root.InnerText;
            root = xmlDoc.SelectSingleNode("空间环境信息需求/EnvironInfo");
            txtHJEnvironInfo.Text = root.InnerText;
            root = xmlDoc.SelectSingleNode("空间环境信息需求/TimeSection1");
            txtHJTimeSection1.Text = root.InnerText;
            root = xmlDoc.SelectSingleNode("空间环境信息需求/TimeSection2");
            txtHJTimeSection2.Text = root.InnerText;
            root = xmlDoc.SelectSingleNode("空间环境信息需求/Sum");
            txtHJSum.Text = root.InnerText;

            root = xmlDoc.SelectSingleNode("空间环境信息需求");
            List<HJXQSatInfo> list = new List<HJXQSatInfo>();
            HJXQSatInfo sat;
            foreach (XmlNode n in root.ChildNodes)
            {
                if (n.Name == "卫星")
                {
                    sat = new HJXQSatInfo();
                    sat.SatName = n["SatName"].InnerText;
                    sat.InfoName = n["InfoName"].InnerText;
                    sat.InfoArea = n["InfoArea"].InnerText;
                    sat.InfoTime = n["InfoTime"].InnerText;
                    list.Add(sat);
                }
            }
            rpHJ.DataSource = list;
            rpHJ.DataBind();


        }
        public override void OnPageLoaded()
        {
            this.PagePermission = "Plan.Edit";
            this.ShortTitle = "编辑计划";
            base.OnPageLoaded();
            this.AddJavaScriptInclude("scripts/pages/PlanManage/HJXQEdit.aspx.js");

        }

        protected void rpHJ_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Add")
            {
                List<HJXQSatInfo> list2 = new List<HJXQSatInfo>();
                HJXQSatInfo dm;
                Repeater rp = (Repeater)source;
                foreach (RepeaterItem it in rp.Items)
                {
                    dm = new HJXQSatInfo();
                    TextBox txtHJSatName = (TextBox)it.FindControl("txtHJSatName");
                    TextBox txtHJInfoName = (TextBox)it.FindControl("txtHJInfoName");
                    TextBox txtHJInfoArea = (TextBox)it.FindControl("txtHJInfoArea");
                    TextBox txtHJInfoTime = (TextBox)it.FindControl("txtHJInfoTime");

                    dm.SatName = txtHJSatName.Text;
                    dm.InfoName = txtHJInfoName.Text;
                    dm.InfoArea = txtHJInfoArea.Text;
                    dm.InfoTime = txtHJInfoTime.Text;
                    list2.Add(dm);
                }
                dm = new HJXQSatInfo();
                dm.SatName = "";
                dm.InfoName = "";
                dm.InfoArea = "";
                dm.InfoTime = "";
                list2.Add(dm);
                rp.DataSource = list2;
                rp.DataBind();

            }
            if (e.CommandName == "Del")
            {
                List<HJXQSatInfo> list2 = new List<HJXQSatInfo>();
                HJXQSatInfo dm;
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
                            dm = new HJXQSatInfo();
                            TextBox txtHJSatName = (TextBox)it.FindControl("txtHJSatName");
                            TextBox txtHJInfoName = (TextBox)it.FindControl("txtHJInfoName");
                            TextBox txtHJInfoArea = (TextBox)it.FindControl("txtHJInfoArea");
                            TextBox txtHJInfoTime = (TextBox)it.FindControl("txtHJInfoTime");

                            dm.SatName = txtHJSatName.Text;
                            dm.InfoName = txtHJInfoName.Text;
                            dm.InfoArea = txtHJInfoArea.Text;
                            dm.InfoTime = txtHJInfoTime.Text;
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
            HJXQ obj = new HJXQ();
            obj.User = txtHJUser.Text;
            obj.Time = txtHJTime.Text;
            obj.EnvironInfo = txtHJEnvironInfo.Text;
            obj.TimeSection1 = txtHJTimeSection1.Text;
            obj.TimeSection2 = txtHJTimeSection2.Text;
            //obj.Sum = txtHJSum.Text;

            obj.SatInfos = new List<HJXQSatInfo>();

            HJXQSatInfo dm;
            foreach (RepeaterItem it in rpHJ.Items)
            {
                dm = new HJXQSatInfo();
                TextBox txtHJSatName = (TextBox)it.FindControl("txtHJSatName");
                TextBox txtHJInfoName = (TextBox)it.FindControl("txtHJInfoName");
                TextBox txtHJInfoArea = (TextBox)it.FindControl("txtHJInfoArea");
                TextBox txtHJInfoTime = (TextBox)it.FindControl("txtHJInfoTime");

                dm.SatName = txtHJSatName.Text;
                dm.InfoName = txtHJInfoName.Text;
                dm.InfoArea = txtHJInfoArea.Text;
                dm.InfoTime = txtHJInfoTime.Text;

                obj.SatInfos.Add(dm);
            }
            obj.Sum = obj.SatInfos.Count.ToString(); //信息条数，自动计算得到

            PlanFileCreator creater = new PlanFileCreator();
            if (hfOverDate.Value == "true")
            {
                obj.TaskID = hfTaskID.Value;
                obj.SatID = hfSatID.Value;
                string filepath = creater.CreateHJXQFile(obj, 0);

                DataAccessLayer.PlanManage.JH jh = new DataAccessLayer.PlanManage.JH()
                {
                    TaskID = obj.TaskID,
                    PlanType = "HJXQ",
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
                creater.CreateHJXQFile(obj, 1);
            }

            ClientScript.RegisterStartupScript(this.GetType(), "OK", "<script type='text/javascript'>alert('计划保存成功');</script>");
        }
    }
}
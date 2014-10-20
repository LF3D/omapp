using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

using OperatingManagement.WebKernel.Basic;
using OperatingManagement.WebKernel.Route;
using OperatingManagement.Framework.Core;
using OperatingManagement.DataAccessLayer;
using OperatingManagement.DataAccessLayer.PlanManage;
using OperatingManagement.DataAccessLayer.BusinessManage;
using OperatingManagement.Framework;
using OperatingManagement.Framework.Storage;
using System.Web.Security;
using System.Xml;
using System.Collections;
using ServicesKernel.File;
using System.Data;
using OperatingManagement.ServicesKernel.File;
using OperatingManagement.Framework.Components;

namespace OperatingManagement.Web.Views.PlanManage
{
    public partial class DJZYJHDetail : AspNetPage, IRouteContext
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtPlanStartTime.Attributes.Add("readonly", "true");
                txtPlanEndTime.Attributes.Add("readonly", "true");
                txtSCID.Attributes.Add("readonly", "true");
                InitialTask();

                if (!string.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    string sID = Request.QueryString["id"];

                    HfID.Value = sID;
                    hfStatus.Value = "edit";    //编辑
                    btnWord.Visible = true;
                    BindJhTable(sID);
                    BindXML();
                    hfURL.Value = "?type=DJZYJH&startDate=" + Request.QueryString["startDate"] + "&endDate=" + Request.QueryString["endDate"]
                         + "&jhStartDate=" + Request.QueryString["jhStartDate"] + "&jhEndDate=" + Request.QueryString["jhEndDate"];
                    if ("detail" == Request.QueryString["op"])
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "hide", "<script type='text/javascript'>hideAllButton();</script>");
                    }
                }

            }
        }

        public void InitialTask()
        {
            ddlTask.Items.Clear();
            ddlTask.DataSource = new Task().Cache;
            ddlTask.DataTextField = "TaskNo";
            ddlTask.DataValueField = "SCID";
            ddlTask.DataBind();
        }

        private void BindJhTable(string sID)
        {
            try
            {
                List<JH> jh = (new JH()).SelectByIDS(sID);
                txtPlanStartTime.Text = jh[0].StartTime.ToString("yyyy-MM-dd HH:mm:ss");
                txtPlanEndTime.Text = jh[0].EndTime.ToString("yyyy-MM-dd HH:mm:ss");
                HfFileIndex.Value = jh[0].FileIndex;
                ucOutTask1.SelectedValue = new Task().GetOutTaskNo(jh[0].TaskID, jh[0].SatID);
                hfTaskID.Value = ucOutTask1.SelectedValue;

                txtNote.Text = jh[0].Reserve.ToString();

            }
            catch (Exception ex)
            {
                throw (new AspNetException("计划基本信息初始化出现异常，异常原因", ex));
            }
            finally { }
        }
        private void BindXML()
        {
            try
            {
                List<DJZYJH_Plan> listTask = new List<DJZYJH_Plan>();
                DJZYJH_Plan task;
                DJZYJH_GZDP dp;
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(HfFileIndex.Value);
                XmlNode root = xmlDoc.SelectSingleNode("测控资源使用计划/时间");
                //txtDatetime.Text = root.InnerText;
                root = xmlDoc.SelectSingleNode("测控资源使用计划/计划序列号");
                txtSequence.Text = root.InnerText;
                root = xmlDoc.SelectSingleNode("测控资源使用计划/航天器标识");
                txtSCID.Text = root.InnerText;
                root = xmlDoc.SelectSingleNode("测控资源使用计划/计划数量");
                txtTaskCount.Text = root.InnerText;

                root = xmlDoc.SelectSingleNode("测控资源使用计划/计划");
                foreach (XmlNode n in root.ChildNodes)
                {
                    if (n.Name == "计划内容")
                    {
                        task = new DJZYJH_Plan();
                        task.SXH = n["计划序号"].InnerText;
                        task.DF = n["答复标志"].InnerText;
                        task.SXZ = n["计划性质"].InnerText;
                        task.MLB = n["任务类别"].InnerText;
                        task.FS = n["工作方式"].InnerText;
                        task.GZDY = n["工作单元"].InnerText;
                        task.SBDH = n["设备代号"].InnerText;
                        task.QC = n["圈次"].InnerText;
                        task.QB = n["圈标"].InnerText;
                        task.SHJ = n["测控事件类型"].InnerText;
                        task.FNUM = n["工作点频数量"].InnerText;
                        task.DJZYJHGZDPs = new List<DJZYJH_GZDP>();
                        foreach (XmlNode nn in n["工作点频"].ChildNodes)
                        {
                            if (nn.Name == "工作点频内容")
                            {
                                dp = new DJZYJH_GZDP();
                                dp.FXH = nn["点频序号"].InnerText;
                                dp.PDXZ = nn["频段选择"].InnerText;
                                dp.DPXZ = nn["点频选择"].InnerText;
                                task.DJZYJHGZDPs.Add(dp);
                            }
                        }
                        task.TNUM = n["同时支持目标数"].InnerText;
                        task.ZHB = n["任务准备开始时间"].InnerText;
                        task.RK = n["任务开始时间"].InnerText;
                        task.GZK = n["跟踪开始时间"].InnerText;
                        task.KSHX = n["开上行载波时间"].InnerText;
                        task.GSHX = n["关上行载波时间"].InnerText;
                        task.GZJ = n["跟踪结束时间"].InnerText;
                        task.JS = n["任务结束时间"].InnerText;
                        listTask.Add(task);
                    }
                }

                Repeater1.DataSource = listTask;
                Repeater1.DataBind();
            }
            catch (Exception ex)
            {
                throw (new AspNetException("绑定地面计划信息出现异常，异常原因", ex));
            }
            finally { }
        }

        protected void RepeaterGZDP_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    //频段选择
                    TextBox txtPDXZ = (TextBox)e.Item.FindControl("txtPDXZ") as TextBox;
                    List<PlanParameter> listPara = PlanParameters.ReadParameters("DJZYSQPDXZ");

                    DJZYJH_GZDP view = (DJZYJH_GZDP)e.Item.DataItem;
                    foreach (PlanParameter p in listPara)
                    {
                        if (p.Value == view.PDXZ)
                        {
                            txtPDXZ.Text= p.Text;
                            break;
                        }
                    }
                    
                }
            }
            catch (Exception ex)
            {
                throw (new AspNetException("绑定地面计划信息出现异常，异常原因", ex));
            }
            finally { }
        }

        public override void OnPageLoaded()
        {
            this.PagePermission = "OMPLAN_Plan.View";
            this.ShortTitle = "查看计划";
            base.OnPageLoaded();
            this.AddJavaScriptInclude("scripts/pages/PlanManage/DJZYJHDetail.aspx.js");
        }

        /// <summary>
        /// 获取文件名
        /// </summary>
        /// <param name="objfilepath"></param>
        /// <returns></returns>
        public string GetFileName(object objfilepath)
        {
            try
            {
                string filename = "";
                string filepath = Convert.ToString(objfilepath);
                string savepath = System.Configuration.ConfigurationManager.AppSettings["savepath"];
                filename = filepath.Replace(savepath, "");
                return filename;
            }
            catch (Exception ex)
            {
                throw (new AspNetException("获取文件名出现异常，异常原因", ex));
            }
            finally { }
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {

                Response.Redirect("PlanList.aspx" + hfURL.Value);
        }

        /// <summary>
        /// 导出Word文档 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnWord_Click(object sender, EventArgs e)
        {
            WordHandle objWord = new WordHandle();
            string sid = HfID.Value;
            if (string.IsNullOrEmpty(sid))
            {
                ltMessage.Text = "计划尚未保存或不存在该计划,无法导出Word文档";
                return;
            }
            DJZYJH obj = new DJZYJH();
            try
            {
                string filepath = objWord.CreateDJZYJHFile(obj.GetByID(sid));
                DownLoadFile(filepath);
            }
            catch (Exception ex)
            {
                throw (new AspNetException("生成Word文档出现异常，异常原因", ex));
            }
            finally { }
        }

        public void DownLoadFile(string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            string fileName = fileInfo.Name;
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName);
            Response.AddHeader("Content-Length", fileInfo.Length.ToString());
            Response.AddHeader("Content-Transfer-Encoding", "binary");
            Response.ContentType = "application/octet-stream";
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("gb2312");
            Response.WriteFile(fileInfo.FullName);
            Response.Flush();
            //Response.End();
        }

        protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    DJZYJH_Plan plan = (DJZYJH_Plan)e.Item.DataItem;
                    Repeater rp = (Repeater)e.Item.FindControl("rpGZDP") as Repeater;

                    rp.DataSource = plan.DJZYJHGZDPs;
                    rp.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw (new AspNetException("绑定地面计划信息出现异常，异常原因", ex));
            }
            finally { }
        }
    }
}
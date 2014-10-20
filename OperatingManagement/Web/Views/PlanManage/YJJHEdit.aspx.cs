using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

using OperatingManagement.WebKernel.Basic;
using OperatingManagement.WebKernel.Route;
using OperatingManagement.Framework;
using OperatingManagement.Framework.Core;
using OperatingManagement.Framework.Components;
using OperatingManagement.Framework.Storage;
using OperatingManagement.DataAccessLayer;
using OperatingManagement.DataAccessLayer.PlanManage;
using OperatingManagement.DataAccessLayer.BusinessManage;
using System.Web.Security;
using System.Xml;
using ServicesKernel.File;

namespace OperatingManagement.Web.Views.PlanManage
{
    public partial class YJJHEdit : AspNetPage, IRouteContext
    {
        bool isTempJH = false;  //是否为临时计划,默认为正式计划

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnFormal.Visible = false;
                if (Request.QueryString["view"] == "1")
                    this.IsViewOrEdit = true; 
                //txtStartTime.Attributes.Add("readonly", "true");
                //txtEndTime.Attributes.Add("readonly", "true");
                BindSysDdlSource();
                DropDownList ddlTask = ucOutTask1.FindControl("TaskList") as DropDownList;
                ddlTask.Attributes.Add("onchange", "SetSysName(this,'" + ddlSysName.ClientID + "','" + txtSysName.ClientID + "')");
                if (!string.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    string sID = Request.QueryString["id"];
                    
                    if (!string.IsNullOrEmpty(Request.QueryString["istemp"]) && Request.QueryString["istemp"] == "true")
                    {
                        isTempJH = true;
                        ViewState["isTempJH"] = true;
                        btnFormal.Visible = true;   //只有临时计划才能转为正式计划
                        btnSurePlan.Visible = !(btnFormal.Visible);
                        btnCreateFile.Visible = !(btnFormal.Visible);
                    }

                    HfID.Value = sID;   //自增ID
                    hfStatus.Value = "edit";    //编辑
                    inital(false);
                    BindJhTable(sID);
                    BindXML();
                    hfURL.Value = "?type=YJJH&startDate=" + Request.QueryString["startDate"] + "&endDate=" + Request.QueryString["endDate"]
                         + "&jhStartDate=" + Request.QueryString["jhStartDate"] + "&jhEndDate=" + Request.QueryString["jhEndDate"];
                    if ("detail" == Request.QueryString["op"])
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "hide", "<script type='text/javascript'>hideAllButton();</script>");
                    }
                }
                else
                {
                    btnReturn.Visible = false;
                    btnSaveTo.Visible = false;
                    btnSurePlan.Visible = false;
                    btnCreateFile.Visible = false;
                    hfStatus.Value = "new"; //新建
                    inital(true);
                    //txtJXH.Text = (new Sequence()).GetYJJHSequnce().ToString("0000");   //新建时先给出计划序号
                }

                if (this.IsViewOrEdit)
                {
                    SetControlsEnabled(Page, ControlNameEnum.All);
                    btnReturn.Visible = true;
                }
            }
            
        }

        private void inital(bool isNew)
        {
            txtJXH.Attributes.Add("readonly", "true");
            if (isNew)
            {
                List<YJJH_Task> list = new List<YJJH_Task>();
                list.Add(new YJJH_Task());
                rpTasks.DataSource = list;
                rpTasks.DataBind();
            }
        }

        private void BindJhTable(string sID)
        {
            try
            {
                isTempJH = GetIsTempJHValue();

                List<JH> jh = (new JH(isTempJH)).SelectByIDS(sID);
                HfFileIndex.Value = jh[0].FileIndex;
                txtJXH.Text = jh[0].PlanID.ToString("0000");
                ucOutTask1.SelectedValue = new Task().GetOutTaskNo(jh[0].TaskID, jh[0].SatID);
                hfTaskID.Value = ucOutTask1.SelectedValue;
                txtSysName.Text = ddlSysName.Items.FindByText(ucOutTask1.SelectedValue).Value;
                //string[] strTemp = jh[0].FileIndex.Split('_');
                //if (strTemp.Length >= 2)
                //{
                //    hfSatID.Value = strTemp[strTemp.Length - 2];
                //    ucSatellite1.SelectedValue = strTemp[strTemp.Length - 2];
                //}
                txtNote.Text = jh[0].Reserve.ToString();
                //计划启动后不能修改计划
                if (DateTime.Now > jh[0].StartTime)
                {
                    //btnSubmit.Visible = false;
                    //hfOverDate.Value = "true";
                }
            }
            catch (Exception ex)
            {
                throw (new AspNetException("绑定计划基本信息出现异常，异常原因", ex));
            }
            finally { }
        }

        private void BindXML()
        {
            try
            {
                YJJH oYJ = new YJJH();
                oYJ.ReadXML(HfFileIndex.Value);
                radBtnXXFL.SelectedValue = oYJ.XXFL;
                txtJXH.Text = oYJ.JXH;
                ddlSysName.SelectedValue = oYJ.SysName;
                if (oYJ.Tasks.Count == 0)
                    oYJ.Tasks.Add(new YJJH_Task());
                rpTasks.DataSource = oYJ.Tasks;
                rpTasks.DataBind();
            }
            catch (Exception ex)
            {
                throw (new AspNetException("绑定应用研究工作计划出现异常，异常原因", ex));
            }
            finally { }
        }
        public override void OnPageLoaded()
        {
            this.PagePermission = "OMPLAN_Plan.Edit";
            this.ShortTitle = "编辑计划";
            base.OnPageLoaded();
           //this.SetTitle();
            this.AddJavaScriptInclude("scripts/pages/PlanManage/YJJHEdit.aspx.js");

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                HideMsg();
                string taskID = string.Empty;
                string satID = string.Empty;
                new Task().GetTaskNoSatID(ucOutTask1.SelectedValue, out taskID, out satID);
                isTempJH = GetIsTempJHValue();

                YJJH obj = new YJJH();
                obj.XXFL = radBtnXXFL.SelectedValue;
                obj.JXH = txtJXH.Text;
                obj.SysName = ddlSysName.SelectedValue;
                txtSysName.Text = ddlSysName.SelectedValue;
                obj.Tasks = GetRPContents("保存", rpTasks, -1);
                obj.TaskID = taskID;
                obj.SatID = satID;
                CultureInfo provider = CultureInfo.InvariantCulture;

                PlanFileCreator creater = new PlanFileCreator(isTempJH);
                DateTime startTime;
                DateTime endTime;
                GetTimes(obj.Tasks, out startTime, out endTime);
                if (hfStatus.Value == "new")
                {
                    //保存时才生成计划序号
                    obj.JXH = (new Sequence()).GetYJJHSequnce().ToString("0000");
                    txtJXH.Text = obj.JXH;
                    string filepath = creater.CreateYJJHFile(obj, 0);
                    DataAccessLayer.PlanManage.JH jh = new DataAccessLayer.PlanManage.JH(isTempJH)
                    {
                        TaskID = obj.TaskID,
                        PlanType = "YJJH",
                        PlanID = Convert.ToInt32(obj.JXH),
                        StartTime = startTime,
                        EndTime = endTime,
                        SRCType = 0,
                        FileIndex = filepath,
                        SatID = obj.SatID,
                        Reserve = txtNote.Text
                    };
                    var result = jh.Add();
                    ShowMsg(result == FieldVerifyResult.Success);
                }
                else
                {
                    //当任务和卫星更新时，需要更新文件名称
                    if (hfTaskID.Value != ucOutTask1.SelectedValue)
                    {
                        string filepath = creater.CreateYJJHFile(obj, 0);
                        DataAccessLayer.PlanManage.JH jh = new DataAccessLayer.PlanManage.JH(isTempJH)
                        {
                            Id = Convert.ToInt32(HfID.Value),
                            TaskID = obj.TaskID,
                            StartTime = startTime,
                            EndTime = endTime,
                            FileIndex = filepath,
                            SatID = obj.SatID,
                            Reserve = txtNote.Text
                        };
                        var result = jh.Update();
                        ShowMsg(result == FieldVerifyResult.Success);
                        //更新隐藏域的任务ID和卫星ID
                        hfTaskID.Value = jh.TaskID;
                        hfSatID.Value = jh.SatID;
                    }
                    else
                    {
                        creater.FilePath = HfFileIndex.Value;
                        creater.CreateYJJHFile(obj, 1);
                        DataAccessLayer.PlanManage.JH jh = new DataAccessLayer.PlanManage.JH(isTempJH)
                        {
                            Id = Convert.ToInt32(HfID.Value),
                            SENDSTATUS = 0,
                            USESTATUS = 0
                        };
                        var result = jh.UpdateStatus();
                        ShowMsg(result == FieldVerifyResult.Success);
                    }
                }
                //ClientScript.RegisterStartupScript(this.GetType(), "OK", "<script type='text/javascript'>showMsg('计划保存成功');</script>");
            }
            catch (Exception ex)
            {
                throw (new AspNetException("保存计划信息出现异常，异常原因", ex));
            }
            finally { }
        }

        protected void btnSaveTo_Click(object sender, EventArgs e)
        {
            try
            {
                string taskID = string.Empty;
                string satID = string.Empty;
                new Task().GetTaskNoSatID(ucOutTask1.SelectedValue, out taskID, out satID);
                isTempJH = GetIsTempJHValue();

                YJJH obj = new YJJH();
                obj.XXFL = radBtnXXFL.SelectedValue;
                //obj.JXH = txtJXH.Text;
                obj.SysName = ddlSysName.SelectedValue;
                obj.Tasks = GetRPContents("另存", rpTasks, -1);
                CultureInfo provider = CultureInfo.InvariantCulture;

                PlanFileCreator creater = new PlanFileCreator(isTempJH);

                obj.TaskID = taskID;
                obj.SatID = satID;
                obj.JXH = (new Sequence()).GetYJJHSequnce().ToString("0000");

                if (creater.TestYJJHFileName(obj))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "File", "<script type='text/javascript'>showMsg('存在同名文件，请一分钟后重试');</script>");
                    return;
                }

                string filepath = creater.CreateYJJHFile(obj, 0);
                DateTime startTime;
                DateTime endTime;
                GetTimes(obj.Tasks, out startTime, out endTime);

                DataAccessLayer.PlanManage.JH jh = new DataAccessLayer.PlanManage.JH(isTempJH)
                {
                    TaskID = obj.TaskID,
                    PlanType = "YJJH",
                    PlanID = Convert.ToInt32(obj.JXH),
                    StartTime = startTime,
                    EndTime = endTime,
                    SRCType = 0,
                    FileIndex = filepath,
                    SatID = obj.SatID,
                    Reserve = txtNote.Text
                };
                var result = jh.Add();

                txtJXH.Text = obj.JXH;  //另存后显示新的序号
                trMessage.Visible = true;
                ltMessage.Text = "计划另存成功";
               // ClientScript.RegisterStartupScript(this.GetType(), "OK", "<script type='text/javascript'>showMsg('计划保存成功');</script>");
            }
            catch (Exception ex)
            {
                throw (new AspNetException("另存计划信息出现异常，异常原因", ex));
            }
            finally { }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(HfID.Value))
                {
                    Page.Response.Redirect(Request.CurrentExecutionFilePath,false);
                }
                else
                {
                    string sID = HfID.Value;
                    HfID.Value = sID;
                    hfStatus.Value = "edit";    //编辑
                    BindJhTable(sID);
                    BindXML();
                }
            }
            catch (Exception ex)
            {
                throw (new AspNetException("重置页面出现异常，异常原因", ex));
            }
            finally { }
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            isTempJH = GetIsTempJHValue();

            if (isTempJH)
            {
                Response.Redirect("PlanTempList.aspx" + hfURL.Value);
            }
            else
            {
                Response.Redirect("PlanList.aspx" + hfURL.Value);
            }
        }

        protected bool GetIsTempJHValue()
        {
            bool returnvalue = false;
            if (null != ViewState["isTempJH"])
            {
                returnvalue = Convert.ToBoolean(ViewState["isTempJH"]);
            }
            return returnvalue;
        }

        protected void btnFormal_Click(object sender, EventArgs e)
        {
            try
            {
                string taskID = string.Empty;
                string satID = string.Empty;
                new Task().GetTaskNoSatID(ucOutTask1.SelectedValue, out taskID, out satID);
                YJJH obj = new YJJH();
                obj.XXFL = radBtnXXFL.SelectedValue;
                //obj.JXH = txtJXH.Text;
                obj.SysName = ddlSysName.SelectedValue;
                obj.Tasks = GetRPContents("准存正式计划", rpTasks, -1);
                CultureInfo provider = CultureInfo.InvariantCulture;

                PlanFileCreator creater = new PlanFileCreator();

                obj.TaskID = taskID;
                obj.SatID = satID;
                obj.JXH = (new Sequence()).GetYJJHSequnce().ToString("0000");

                if (creater.TestYJJHFileName(obj))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "File", "<script type='text/javascript'>showMsg('存在同名文件，请一分钟后重试');</script>");
                    return;
                }

                string filepath = creater.CreateYJJHFile(obj, 0);
                DateTime startTime;
                DateTime endTime;
                GetTimes(obj.Tasks, out startTime, out endTime);

                DataAccessLayer.PlanManage.JH jh = new DataAccessLayer.PlanManage.JH()
                {
                    TaskID = obj.TaskID,
                    PlanType = "YJJH",
                    PlanID = Convert.ToInt32(obj.JXH),
                    StartTime = startTime,
                    EndTime = endTime,
                    SRCType = 0,
                    FileIndex = filepath,
                    SatID = obj.SatID,
                    Reserve = txtNote.Text
                };
                var result = jh.Add();

                //删除当前临时计划
                DataAccessLayer.PlanManage.JH jhtemp = new DataAccessLayer.PlanManage.JH(true)
                {
                    Id = Convert.ToInt32( HfID.Value ),
                };
                var resulttemp = jhtemp.DeleteTempJH();

                #region 转成正式计划之后，禁用除“返回”之外的所有按钮
                btnSubmit.Visible = false;
                btnSaveTo.Visible = false;
                btnReset.Visible = false;
                btnFormal.Visible = false;

                #endregion

                txtJXH.Text = obj.JXH;  //另存后显示新的序号
                trMessage.Visible = true;
                ltMessage.Text = "计划保存成功";



            }
            catch (Exception ex)
            {
                throw (new AspNetException("另存计划信息出现异常，异常原因", ex));
            }
            finally { }
        }

        protected void btnImport_Click(object sender, EventArgs e)
        {
            BindGridView();
            ClientScript.RegisterStartupScript(this.GetType(), "hide", "<script type='text/javascript'>showSYJHForm();</script>");
        }


        //绑定试验计划列表
        void BindGridView()
        {

            DateTime startDate = new DateTime();
            DateTime endDate = new DateTime();
            startDate = new DateTime(1900, 1, 1);
            endDate = DateTime.Now.AddDays(1);

            //List<JH> listDatas = (new JH()).GetSYJHList(startDate, endDate);
            //cpPager.DataSource = listDatas;
            //cpPager.PageSize = 5;
            //cpPager.BindToControl = rpDatas;
            //rpDatas.DataSource = cpPager.DataSourcePaged;
            //rpDatas.DataBind();
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

        /// <summary>
        /// 读取试验计划中的试验
        /// </summary>
        /// <param name="filefullpath"></param>
        /// <returns></returns>
        private List<SYJH_SY> ReadSYJHXML(string filefullpath)
        {
            List<SYJH_SY> list = new List<SYJH_SY>();
            SYJH_SY sy;
            try
            {
                CultureInfo provider = CultureInfo.InvariantCulture;
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(filefullpath);
                XmlNode root = xmlDoc.SelectSingleNode("试验计划");
                foreach (XmlNode n in root.ChildNodes)
                {
                    if (n.Name == "试验")
                    {
                        sy = new SYJH_SY();
                        sy.SYSatName = n["卫星名称"].InnerText;
                        sy.SYType = n["试验类别"].InnerText;
                        sy.SYItem = n["试验项目"].InnerText;
                        sy.SYStartTime = n["开始时间"].InnerText;
                        sy.SYEndTime = n["结束时间"].InnerText;
                        sy.SYSysName = n["系统名称"].InnerText;
                        sy.SYSysTask = n["系统任务"].InnerText;

                        list.Add(sy);
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                throw (new AspNetException("读取试验计划出现异常，异常原因", ex));
            }
            finally { }
        }

        protected void rpDatas_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    JH jh = (JH)e.Item.DataItem;
                    Repeater rp = e.Item.FindControl("rpSY") as Repeater;
                    int row = e.Item.ItemIndex;
                    List<SYJH_SY> list = new List<SYJH_SY>();
                    list = ReadSYJHXML(jh.FileIndex);
                    rp.DataSource = list;
                    rp.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw (new AspNetException("绑定地面计划信息出现异常，异常原因", ex));
            }
            finally { }
        }

        protected void rpTasks_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            Repeater rp = (Repeater)source;
            List<YJJH_Task> list;
            try
            {
                if (e.CommandName == "Add")
                {
                    list = GetRPContents("添加", rp, -1);
                    YJJH_Task co;
                    ViewState["op"] = "Add";
                    #region new a GZJH_Content
                    co = new YJJH_Task()
                    {
                        StartTime = "",
                        EndTime = "",
                        Task = ""
                    };
                    #endregion
                    list.Add(co);
                    rp.DataSource = list;
                    rp.DataBind();
                }
                if (e.CommandName == "Del")
                {
                    list = new List<YJJH_Task>();
                    ViewState["op"] = "Del";
                    if (rp.Items.Count <= 1)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "del", "<script type='text/javascript'>showMsg('最后一条，无法删除!');</script>");
                    }
                    else
                    {
                        list = GetRPContents("删除", rp, e.Item.ItemIndex);
                        rp.DataSource = list;
                        rp.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                throw (new AspNetException("绑定应用研究计划信息出现异常，异常原因", ex));
            }
            finally { }
        }

        private List<YJJH_Task> GetRPContents(string from, Repeater rp, int idx)
        {
            List<YJJH_Task> list = new List<YJJH_Task>();
            YJJH_Task co;
            try
            {
                foreach (RepeaterItem it in rp.Items)
                {
                    if (idx != it.ItemIndex)
                    {
                        #region 赋值
                        co = new YJJH_Task();
                        TextBox txtStartTime = (TextBox)it.FindControl("txtStartTime");
                        TextBox txtEndTime = (TextBox)it.FindControl("txtEndTime");
                        TextBox txtTask = (TextBox)it.FindControl("txtTask");
                        co.StartTime = txtStartTime.Text;
                        co.EndTime = txtEndTime.Text;
                        co.Task = txtTask.Text;
                        #endregion
                        list.Add(co);
                    }
                }
            }
            catch (Exception ex)
            {
                throw (new AspNetException(from + "计划，获取应用研究计划信息出现异常，异常原因", ex));
            }
            return list;
        }

        private void GetTimes(List<YJJH_Task> tasks, out DateTime startTime, out DateTime endTime)
        {
            startTime = DateTime.MaxValue;
            endTime = DateTime.MinValue;
            DateTime dt;
            if (tasks != null && tasks.Count > 0)
            {
                foreach (YJJH_Task task in tasks)
                {
                    dt = DateTime.ParseExact(task.StartTime, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
                    if (startTime > dt)
                        startTime = dt;
                    dt = DateTime.ParseExact(task.EndTime, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
                    if (endTime < dt)
                        endTime = dt;
                }
            }
        }

        private void ShowMsg(bool sucess)
        {
            if (sucess)
                ShowMsg(sucess, "计划保存成功");
            else
                ShowMsg(sucess, "计划保存失败");
        }

        private void ShowMsg(bool sucess, string msg)
        {
            trMessage.Visible = true;
            ltMessage.Text = msg;
            hfTaskID.Value = ucOutTask1.SelectedValue;
        }

        private void HideMsg()
        {
            trMessage.Visible = false;
            ltMessage.Text = "";
        }

        /// <summary>
        /// 确认计划
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSurePlan_Click(object sender, EventArgs e)
        {
            if (hfStatus.Value != "new")
            {
                DataAccessLayer.PlanManage.JH jh = new DataAccessLayer.PlanManage.JH(isTempJH)
                {
                    Id = Convert.ToInt32(HfID.Value),
                    SENDSTATUS = 0,
                    USESTATUS = 1
                };
                var result = jh.UpdateStatus();
                bool success = result == FieldVerifyResult.Success;
                ShowMsg(success, (success ? "计划确认成功" : "计划确认失败"));
            }
        }

        /// <summary>
        /// 绑定隐藏下拉框的数据源
        /// </summary>
        private void BindSysDdlSource()
        {
            List<Task> taskList = new List<Task>();
            taskList = new Task().SelectAll();
            List<PlanParameter> targetList = PlanParameters.ReadParameters("YJJHSendTargetMapping");
            XYXSInfo oInfo = new XYXSInfo();
            foreach (Task oTask in taskList)
            {
                for (int i=0;i<targetList.Count;i++)
                {
                    if (oTask.SatID.Substring(0, 3) == targetList[i].Text)
                    {
                        oTask.TaskName = oInfo.GetByAddrMark(targetList[i].Value).ADDRName;
                        break;
                    }
                    else
                        oTask.TaskName = "";
                }
            }
            ddlSysName.DataSource = taskList;
            ddlSysName.DataTextField = "OutTaskNo";
            ddlSysName.DataValueField = "TaskName";
            ddlSysName.DataBind();
            txtSysName.Text = ddlSysName.Items.FindByText(ucOutTask1.SelectedValue).Value;
        }

        protected void btnCreateFile_Click(object sender, EventArgs e)
        {
            string SendingFilePaths = string.Empty;
            PlanFileCreator creater = new PlanFileCreator();
            string targets = string.Empty;
            HideMsg();
            divFiles.Visible = false;
            try
            {
                SendingFilePaths = creater.CreateSendingYJJHFile(HfID.Value, out targets, true);
                lblFilePath.Text = SendingFilePaths;
                lbtFilePath_Click(null, null);
                //ShowMsg(true, "文件生成成功。");
                //divFiles.Visible = true;
            }
            catch (Exception ex)
            {
                throw new AspNetException("应用研究计划-生成文件出现异常，异常原因", ex);
            }
        }

        protected void lbtFilePath_Click(object sender, EventArgs e)
        {
            try
            {
                string strFilePath = lblFilePath.Text.Trim();
                if (string.IsNullOrEmpty(strFilePath) || !System.IO.File.Exists(strFilePath))
                {
                    ShowMsg(true, "文件不存在。");
                    return;
                }

                Response.Clear();
                Response.Buffer = false;
                Response.ContentType = "application/octet-stream";
                Response.AppendHeader("content-disposition", "attachment;filename=" + System.IO.Path.GetFileName(strFilePath) + ";");
                Response.Write(System.IO.File.ReadAllText(strFilePath));
                Response.Flush();
                Response.End();
            }
            catch (System.Threading.ThreadAbortException ex1)
            { }
            catch (Exception ex)
            {
                throw (new AspNetException("应用研究计划-生成文件出现异常，异常原因", ex));
            }
        }

    }
}
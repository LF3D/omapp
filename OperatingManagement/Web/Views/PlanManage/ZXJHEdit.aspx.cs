using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using OperatingManagement.WebKernel.Basic;
using OperatingManagement.WebKernel.Route;
using OperatingManagement.Framework.Core;
using OperatingManagement.DataAccessLayer;
using OperatingManagement.DataAccessLayer.BusinessManage;
using OperatingManagement.DataAccessLayer.PlanManage;
using OperatingManagement.Framework;
using System.Web.Security;
using System.Xml;
using System.Collections;
using ServicesKernel.File;

namespace OperatingManagement.Web.Views.PlanManage
{
    public partial class ZXJHEdit : AspNetPage, IRouteContext
    {
        bool isTempJH = false;  //是否为临时计划,默认为正式计划

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["view"] == "1")
                    this.IsViewOrEdit = true; 
                btnFormal.Visible = false; 
                txtPlanStartTime.Attributes.Add("readonly", "true");
                txtPlanEndTime.Attributes.Add("readonly", "true");
                initial();
                if (!string.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    string sID = Request.QueryString["id"];
                    if (!string.IsNullOrEmpty(Request.QueryString["istemp"]) && Request.QueryString["istemp"] == "true")
                    {
                        isTempJH = true;
                        ViewState["isTempJH"] = true;
                        btnFormal.Visible = true;   //只有临时计划才能转为正式计划
                        btnSurePlan.Visible = !(btnFormal.Visible);
                    }

                    HfID.Value = sID;
                    hfStatus.Value = "edit";    //编辑
                    BindJhTable(sID);
                    BindXML();
                    hfURL.Value = "?type=ZXJH&startDate=" + Request.QueryString["startDate"] + "&endDate=" + Request.QueryString["endDate"]
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
                    hfStatus.Value = "new"; //新建
                }
                if (this.IsViewOrEdit)
                {
                    SetControlsEnabled(Page, ControlNameEnum.All);
                    btnReturn.Visible = true;
                    btnReturn.Enabled = true;
                }
            }
        }

        private void initial()
        {
            pnlMain.Visible = true;
            pnlStation.Visible = false;

            List<ZXJH_SYContent> listSY = new List<ZXJH_SYContent>();
            List<ZXJH_SYContent_SC> listSYSC = new List<ZXJH_SYContent_SC>();
            List<ZXJH_SYContent_CK> listSYCK = new List<ZXJH_SYContent_CK>();
            List<ZXJH_SYContent_ZS> listSYZS = new List<ZXJH_SYContent_ZS>();
            List<ZXJH_WorkContent> listWC = new List<ZXJH_WorkContent>();
            List<ZXJH_CommandMake> listCM = new List<ZXJH_CommandMake>();
            List<ZXJH_SYDataHandle> listDH = new List<ZXJH_SYDataHandle>();
            List<ZXJH_DirectAndMonitor> listDM = new List<ZXJH_DirectAndMonitor>();
            List<ZXJH_RealTimeControl> listRC = new List<ZXJH_RealTimeControl>();
            List<ZXJH_SYEstimate> listE = new List<ZXJH_SYEstimate>();

            listSYSC.Add(new ZXJH_SYContent_SC());
            listSYCK.Add(new ZXJH_SYContent_CK());
            listSYZS.Add(new ZXJH_SYContent_ZS());
            ZXJH_SYContent sy = new ZXJH_SYContent() { 
                SCList = listSYSC,
                CKList = listSYCK,
                ZSList = listSYZS
            };
            listSY.Add(sy);
            listWC.Add(new ZXJH_WorkContent());
            listCM.Add(new ZXJH_CommandMake());
            listDH.Add(new ZXJH_SYDataHandle());
            listDM.Add(new ZXJH_DirectAndMonitor());
            listRC.Add(new ZXJH_RealTimeControl());
            listE.Add(new ZXJH_SYEstimate());

            rpSYContent.DataSource = listSY;
            rpSYContent.DataBind();
            rpWork.DataSource = listWC;
            rpWork.DataBind();
            rpCommandMake.DataSource = listCM;
            rpCommandMake.DataBind();
            rpSYDataHandle.DataSource = listDH;
            rpSYDataHandle.DataBind();
            rpDirectAndMonitor.DataSource = listDM;
            rpDirectAndMonitor.DataBind();
            rpRealTimeControl.DataSource = listRC;
            rpRealTimeControl.DataBind();
            rpSYEstimate.DataSource = listE;
            rpSYEstimate.DataBind();

        }
        private void BindJhTable(string sID)
        {
            isTempJH = GetIsTempJHValue();
            List<JH> jh = (new JH(isTempJH)).SelectByIDS(sID);
            txtPlanStartTime.Text = jh[0].StartTime.ToString("yyyy-MM-dd HH:mm:ss");
            txtPlanEndTime.Text = jh[0].EndTime.ToString("yyyy-MM-dd HH:mm:ss");
            HfFileIndex.Value = jh[0].FileIndex;
            string outTaskNo = new Task().GetOutTaskNo(jh[0].TaskID, jh[0].SatID);
            ucOutTask1.SelectedValue = outTaskNo;
            hfTaskID.Value = ucOutTask1.SelectedValue;
            txtNote.Text = jh[0].Reserve.ToString();
            //计划启动后不能修改计划
            //if (DateTime.Now > jh[0].StartTime)
            //{
            //    btnSubmit.Visible = false;
            //    hfOverDate.Value = "true";
            //}
        }
        private void BindXML()
        {
            List<ZXJH_SYContent> listSY = new List<ZXJH_SYContent>();
            List<ZXJH_WorkContent> listWC = new List<ZXJH_WorkContent>();
            List<ZXJH_CommandMake> listCM = new List<ZXJH_CommandMake>();
            List<ZXJH_SYDataHandle> listDH = new List<ZXJH_SYDataHandle>();
            List<ZXJH_DirectAndMonitor> listDM = new List<ZXJH_DirectAndMonitor>();
            List<ZXJH_RealTimeControl> listRC = new List<ZXJH_RealTimeControl>();
            List<ZXJH_SYEstimate> listE = new List<ZXJH_SYEstimate>();

            ZXJH_SYContent sy;
            ZXJH_SYContent_SC sy_sc;
            ZXJH_SYContent_CK sy_ck;
            ZXJH_SYContent_ZS sy_zs;
            ZXJH_WorkContent wc;
            ZXJH_CommandMake cm;
            ZXJH_SYDataHandle dh;
            ZXJH_DirectAndMonitor dam;
            ZXJH_RealTimeControl rc;
            ZXJH_SYEstimate sye;

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(HfFileIndex.Value);
            #region 试验计划
            XmlNode root = xmlDoc.SelectSingleNode("中心运行计划/日期");
            txtDate.Text = root.InnerText;
            root = xmlDoc.SelectSingleNode("中心运行计划/试验计划/对应日期的试验个数");
            txtSYCount.Text = root.InnerText;

            #endregion
            #region Repeater
            #region 试验计划
            root = xmlDoc.SelectSingleNode("中心运行计划/试验计划");
            foreach (XmlNode no in root.ChildNodes)
            {
                if (no.Name == "试验内容")
                {
                    sy = new ZXJH_SYContent();
                    foreach (XmlNode n in no.ChildNodes)
                    {
                        switch (n.Name)
                        {
                            case "卫星代号":
                                sy.SatID = n.InnerText;
                                break;
                            case "试验":
                                sy.SYID = n["在当日计划中的ID"].InnerText;
                                sy.SYName = n["试验项目名称"].InnerText;
                                sy.SYStartTime = n["试验开始时间"].InnerText;
                                sy.SYEndTime = n["试验结束时间"].InnerText;
                                sy.SYDays = n["试验运行的天数"].InnerText;
                                sy.SYNote = n["说明"].InnerText;
                                break;
                            case "数传":
                                sy_sc = new ZXJH_SYContent_SC();
                                sy_sc.SY_SCStationNO = n["站编号"].InnerText;
                                sy_sc.SY_SCEquipmentNO = n["设备编号"].InnerText;
                                sy_sc.SY_SCFrequencyBand = n["频段"].InnerText;
                                sy_sc.SY_SCLaps = n["圈次"].InnerText;
                                sy_sc.SY_SCStartTime = n["开始时间"].InnerText;
                                sy_sc.SY_SCEndTime = n["结束时间"].InnerText;
                                if (sy.SCList == null)
                                    sy.SCList = new List<ZXJH_SYContent_SC>();
                                sy.SCList.Add(sy_sc);
                                break;
                            case "测控":
                                sy_ck = new ZXJH_SYContent_CK();
                                sy_ck.SY_CKStationNO = n["站编号"].InnerText;
                                sy_ck.SY_CKEquipmentNO = n["设备编号"].InnerText;
                                sy_ck.SY_CKLaps = n["圈次"].InnerText;
                                sy_ck.SY_CKStartTime = n["开始时间"].InnerText;
                                sy_ck.SY_CKEndTime = n["结束时间"].InnerText;
                                if (sy.CKList == null)
                                    sy.CKList = new List<ZXJH_SYContent_CK>();
                                sy.CKList.Add(sy_ck);
                                break;
                            case "注数":
                                sy_zs = new ZXJH_SYContent_ZS();
                                sy_zs.SY_ZSFirst = n["最早时间要求"].InnerText;
                                sy_zs.SY_ZSLast = n["最晚时间要求"].InnerText;
                                sy_zs.SY_ZSContent = n["主要内容"].InnerText;
                                if (sy.ZSList == null)
                                    sy.ZSList = new List<ZXJH_SYContent_ZS>();
                                sy.ZSList.Add(sy_zs);
                                break;
                        }
                    }
                    if (sy.SCList == null)
                    {
                        sy.SCList = new List<ZXJH_SYContent_SC>();
                        sy.SCList.Add(new ZXJH_SYContent_SC());
                    }
                    if (sy.CKList == null)
                    {
                        sy.CKList = new List<ZXJH_SYContent_CK>();
                        sy.CKList.Add(new ZXJH_SYContent_CK());
                    }
                    if (sy.ZSList == null)
                    {
                        sy.ZSList = new List<ZXJH_SYContent_ZS>();
                        sy.ZSList.Add(new ZXJH_SYContent_ZS());
                    }
                    listSY.Add(sy);
                }

                //listSY.Add(sy);
            }
            rpSYContent.DataSource = listSY;
            rpSYContent.DataBind();
            #endregion 试验计划

            #region  任务管理
            root = xmlDoc.SelectSingleNode("中心运行计划/工作计划/任务管理");
            if (root.ChildNodes.Count > 0)
            {
                foreach (XmlNode n in root.ChildNodes)
                {
                    wc = new ZXJH_WorkContent();
                    wc.Work = n["工作"].InnerText;
                    wc.SYID = n["对应试验ID"].InnerText;
                    wc.StartTime = n["开始时间"].InnerText;
                    wc.MinTime = n["最短持续时间"].InnerText;
                    wc.MaxTime = n["最长持续时间"].InnerText;
                    listWC.Add(wc);
                }
            }
            else
            {
                wc = new ZXJH_WorkContent();
                listWC.Add(wc);
            }
            rpWork.DataSource = listWC;
            rpWork.DataBind();
            #endregion

            #region  指令制作
            root = xmlDoc.SelectSingleNode("中心运行计划/工作计划/指令制作");
            if (root.ChildNodes.Count == 0)
            {
                cm = new ZXJH_CommandMake();
                listCM.Add(cm);
            }
            foreach (XmlNode n in root.ChildNodes)
            {
                cm = new ZXJH_CommandMake();
                cm.Work_Command_SatID = n["卫星代号"].InnerText;
                cm.Work_Command_SYID = n["对应试验ID"].InnerText;
                cm.Work_Command_Programe = n["对应控制程序"].InnerText;
                cm.Work_Command_FinishTime = n["完成时间"].InnerText;
                cm.Work_Command_UpWay = n["上注方式"].InnerText;
                cm.Work_Command_UpTime = n["上注时间"].InnerText;
                cm.Work_Command_Note = n["说明"].InnerText;
                listCM.Add(cm);
            }
            rpCommandMake.DataSource = listCM;
            rpCommandMake.DataBind();
            #endregion

            #region 实时试验数据处理
            root = xmlDoc.SelectSingleNode("中心运行计划/工作计划/实时试验数据处理");
            if (root.ChildNodes.Count == 0)
            {
                dh = new ZXJH_SYDataHandle();
                listDH.Add(dh);
            }
            foreach (XmlNode n in root.ChildNodes)
            {
                dh = new ZXJH_SYDataHandle();
                dh.SYID = n["对应试验ID"].InnerText;
                dh.SatID = n["卫星代号"].InnerText;
                dh.Laps = n["圈次"].InnerText;
                dh.MainStation = n["主站"].InnerText;
                dh.MainStationEquipment = n["主站设备"].InnerText;
                dh.BakStation = n["备站"].InnerText;
                dh.BakStationEquipment = n["备站设备"].InnerText;
                dh.Content = n["工作内容"].InnerText;
                dh.StartTime = n["实时开始处理时间"].InnerText;
                dh.EndTime = n["实时结束处理时间"].InnerText;
                listDH.Add(dh);
            }
            rpSYDataHandle.DataSource = listDH;
            rpSYDataHandle.DataBind();
            #endregion

            #region 指挥与监视
            root = xmlDoc.SelectSingleNode("中心运行计划/工作计划/指挥与监视");
            if (root.ChildNodes.Count == 0)
            {
                dam = new ZXJH_DirectAndMonitor();
                listDM.Add(dam);
            }
            foreach (XmlNode n in root.ChildNodes)
            {
                dam = new ZXJH_DirectAndMonitor();
                dam.SYID = n["对应试验ID"].InnerText;
                dam.StartTime = n["开始时间"].InnerText;
                dam.EndTime = n["结束时间"].InnerText;
                dam.RealTimeDemoTask = n["实时演示任务"].InnerText;
                listDM.Add(dam);
            }
            rpDirectAndMonitor.DataSource = listDM;
            rpDirectAndMonitor.DataBind();
            #endregion

            #region 实时控制
            root = xmlDoc.SelectSingleNode("中心运行计划/工作计划/实时控制");
            if (root.ChildNodes.Count == 0)
            {
                rc = new ZXJH_RealTimeControl();
                listRC.Add(rc);
            }
            foreach (XmlNode n in root.ChildNodes)
            {
                rc = new ZXJH_RealTimeControl();
                rc.Work = n["工作"].InnerText;
                rc.SYID = n["对应试验ID"].InnerText;
                rc.StartTime = n["开始时间"].InnerText;
                rc.EndTime = n["结束时间"].InnerText;
                listRC.Add(rc);
            }
            rpRealTimeControl.DataSource = listRC;
            rpRealTimeControl.DataBind();
            #endregion

            #region 处理评估
            root = xmlDoc.SelectSingleNode("中心运行计划/工作计划/处理评估");
            if (root.ChildNodes.Count == 0)
            {
                sye = new ZXJH_SYEstimate();
                listE.Add(sye);
            }
            foreach (XmlNode n in root.ChildNodes)
            {
                sye = new ZXJH_SYEstimate();
                sye.SYID = n["对应试验ID"].InnerText;
                sye.StartTime = n["开始时间"].InnerText;
                sye.EndTime = n["结束时间"].InnerText;
                listE.Add(sye);
            }
            rpSYEstimate.DataSource = listE;
            rpSYEstimate.DataBind();
            #endregion

            #endregion
        }
        public override void OnPageLoaded()
        {
            this.PagePermission = "OMPLAN_Plan.Edit";
            this.ShortTitle = "编辑计划";
            base.OnPageLoaded();
            this.AddJavaScriptInclude("scripts/pages/PlanManage/ZXJHEdit.aspx.js");
        }

        protected void rpWork_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            List<ZXJH_WorkContent> list2;
            Repeater rp = (Repeater)source;
            ZXJH_WorkContent wc;
            if (e.CommandName == "Add")
            {
                list2 = GetAllWorkInfo(rp, -1);
                wc = new ZXJH_WorkContent();
                wc.Work = "";
                wc.SYID = "";
                wc.StartTime = "";
                wc.MinTime = "";
                wc.MaxTime = "";
                list2.Add(wc);
                rp.DataSource = list2;
                rp.DataBind();

            }
            if (e.CommandName == "Del")
            {
                if (rp.Items.Count <= 1)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "del", "<script type='text/javascript'>showMsg('最后一条，无法删除!');</script>");
                }
                else
                {
                    list2 = GetAllWorkInfo(rp, e.Item.ItemIndex);
                    rp.DataSource = list2;
                    rp.DataBind();
                }
            }
        }

        private List<ZXJH_WorkContent> GetAllWorkInfo(Repeater rp, int idx)
        {
            List<ZXJH_WorkContent> list2 = new List<ZXJH_WorkContent>();
            ZXJH_WorkContent wc;
            foreach (RepeaterItem it in rp.Items)
            {
                if (idx != it.ItemIndex)
                {
                    wc = new ZXJH_WorkContent();
                    DropDownList ddlWC_Work = (DropDownList)it.FindControl("ddlWC_Work");
                    TextBox txtWC_SYID = (TextBox)it.FindControl("txtWC_SYID");
                    TextBox txtWC_StartTime = (TextBox)it.FindControl("txtWC_StartTime");
                    TextBox txtWC_MinTime = (TextBox)it.FindControl("txtWC_MinTime");
                    TextBox txtWC_MaxTime = (TextBox)it.FindControl("txtWC_MaxTime");

                    wc.Work = ddlWC_Work.SelectedValue;
                    if (CheckCtrlExits(txtWC_SYID, "试验ID"))
                        wc.SYID = txtWC_SYID.Text;
                    else
                        return null;

                    if (CheckCtrlExits(txtWC_StartTime, "开始时间"))
                        wc.StartTime = txtWC_StartTime.Text;
                    else
                        return null;

                    if (CheckCtrlExits(txtWC_MinTime, "最短持续时间"))
                        wc.MinTime = txtWC_MinTime.Text;
                    else
                        return null;

                    if (CheckCtrlExits(txtWC_MaxTime, "最长持续时间"))
                        wc.MaxTime = txtWC_MaxTime.Text;
                    else
                        return null;
                    list2.Add(wc);
                }
            }
            return list2;
        }

        protected void rpSYDataHandle_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            Repeater rp = (Repeater)source;
            ZXJH_SYDataHandle dh;
            List<ZXJH_SYDataHandle> list2;
            if (e.CommandName == "Add")
            {
                list2 = GetAllSYDataHandleInfos(rp, -1);
                dh = new ZXJH_SYDataHandle();
                list2.Add(dh);
                rp.DataSource = list2;
                rp.DataBind();

            }
            if (e.CommandName == "Del")
            {
                if (rp.Items.Count <= 1)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "del", "<script type='text/javascript'>showMsg('最后一条，无法删除!');</script>");
                }
                else
                {
                    list2 = GetAllSYDataHandleInfos(rp, e.Item.ItemIndex);
                    rp.DataSource = list2;
                    rp.DataBind();
                }
            }
        }

        private List<ZXJH_SYDataHandle> GetAllSYDataHandleInfos(Repeater rp, int idx)
        {
            List<ZXJH_SYDataHandle> list2 = new List<ZXJH_SYDataHandle>();
            ZXJH_SYDataHandle dh;
            foreach (RepeaterItem it in rp.Items)
            {
                if (idx != it.ItemIndex)
                {
                    dh = new ZXJH_SYDataHandle();
                    TextBox txtSHSYID = (TextBox)it.FindControl("txtSHSYID");
                    ucs.ucSatellite ddlSatID = (ucs.ucSatellite)it.FindControl("ddlSYDataHandle_SatID");
                    TextBox txtSHLaps = (TextBox)it.FindControl("txtSHLaps");
                    DropDownList ddlMainDW = (DropDownList)it.FindControl("ddlMainDW");
                    DropDownList ddlMainSB = (DropDownList)it.FindControl("ddlMainSB");
                    DropDownList ddlBakDW = (DropDownList)it.FindControl("ddlBakDW");
                    DropDownList ddlBakSB = (DropDownList)it.FindControl("ddlBakSB");
                    TextBox txtSHContent = (TextBox)it.FindControl("txtSHContent");
                    TextBox txtSHStartTime = (TextBox)it.FindControl("txtSHStartTime");
                    TextBox txtSHEndTime = (TextBox)it.FindControl("txtSHEndTime");

                    if (CheckCtrlExits(txtSHSYID, "试验ID"))
                        dh.SYID = txtSHSYID.Text;
                    else
                        return null;

                    if (CheckCtrlExits(ddlSatID, "卫星代号"))
                        dh.SatID = ddlSatID.SelectedValue;
                    else
                        return null;

                    if (CheckCtrlExits(txtSHLaps, "圈次"))
                        dh.Laps = txtSHLaps.Text;
                    else
                        return null;

                    if (CheckCtrlExits(txtSHContent, "工作内容"))
                        dh.Content = txtSHContent.Text;
                    else
                        return null;

                    if (CheckCtrlExits(ddlMainDW, "主站"))
                        dh.MainStation = ddlMainDW.SelectedValue;
                    else
                        return null;

                    if (CheckCtrlExits(ddlMainSB, "主站设备"))
                        dh.MainStationEquipment = ddlMainSB.SelectedValue;
                    else
                        return null;

                    if (CheckCtrlExits(ddlBakDW, "备站"))
                        dh.BakStation = ddlBakDW.SelectedValue;
                    else
                        return null;

                    if (CheckCtrlExits(ddlBakSB, "备站设备"))
                        dh.BakStationEquipment = ddlBakSB.SelectedValue;
                    else
                        return null;

                    if (CheckCtrlExits(txtSHStartTime, "实时开始处理时间"))
                        dh.StartTime = txtSHStartTime.Text;
                    else
                        return null;

                    if (CheckCtrlExits(txtSHEndTime, "实时结束处理时间"))
                        dh.EndTime = txtSHEndTime.Text;
                    else
                        return null;
                    list2.Add(dh);
                }
            }
            return list2;

        }

        protected void rpDirectAndMonitor_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            List<ZXJH_DirectAndMonitor> list2;
            ZXJH_DirectAndMonitor dm;
            Repeater rp = (Repeater)source;
            if (e.CommandName == "Add")
            {
                list2 = GetAllDirectAndMonitorInfos(rp, -1);
                dm = new ZXJH_DirectAndMonitor();
                list2.Add(dm);
                rp.DataSource = list2;
                rp.DataBind();

            }
            if (e.CommandName == "Del")
            {
                if (rp.Items.Count <= 1)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "del", "<script type='text/javascript'>showMsg('最后一条，无法删除!');</script>");
                }
                else
                {
                    list2 = GetAllDirectAndMonitorInfos(rp, e.Item.ItemIndex);
                    rp.DataSource = list2;
                    rp.DataBind();
                }
            }
        }

        private List<ZXJH_DirectAndMonitor> GetAllDirectAndMonitorInfos(Repeater rp, int idx)
        {
            List<ZXJH_DirectAndMonitor> list2 = new List<ZXJH_DirectAndMonitor>();
            ZXJH_DirectAndMonitor dm;
            foreach (RepeaterItem it in rp.Items)
            {
                if (idx != it.ItemIndex)
                {
                    dm = new ZXJH_DirectAndMonitor();
                    TextBox txtDMSYID = (TextBox)it.FindControl("txtDMSYID");
                    TextBox txtDMStartTime = (TextBox)it.FindControl("txtDMStartTime");
                    TextBox txtDMEndTime = (TextBox)it.FindControl("txtDMEndTime");
                    DropDownList ddlDMRTTask = (DropDownList)it.FindControl("ddlDMRTTask");

                    if (CheckCtrlExits(txtDMSYID, "试验ID"))
                        dm.SYID = txtDMSYID.Text;
                    else
                        return null;

                    if (CheckCtrlExits(txtDMStartTime, "开始时间"))
                        dm.StartTime = txtDMStartTime.Text;
                    else
                        return null;

                    if (CheckCtrlExits(txtDMEndTime, "结束时间"))
                        dm.EndTime = txtDMEndTime.Text;
                    else
                        return null;

                    dm.RealTimeDemoTask = ddlDMRTTask.SelectedItem.Text;
                    list2.Add(dm);
                }
            }
            return list2;
        }

        protected void rpRealTimeControl_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            List<ZXJH_RealTimeControl> list2 = new List<ZXJH_RealTimeControl>();
            ZXJH_RealTimeControl rc;
            Repeater rp = (Repeater)source;
            if (e.CommandName == "Add")
            {
                list2 = GetAllRealTimeControlInfos(rp, -1);
                rc = new ZXJH_RealTimeControl();
                rc.Work = "";
                rc.SYID = "";
                rc.StartTime = "";
                rc.EndTime = "";
                list2.Add(rc);
                rp.DataSource = list2;
                rp.DataBind();

            }
            if (e.CommandName == "Del")
            {
                if (rp.Items.Count <= 1)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "del", "<script type='text/javascript'>showMsg('最后一条，无法删除!');</script>");
                }
                else
                {
                    list2 = GetAllRealTimeControlInfos(rp, e.Item.ItemIndex);
                    rp.DataSource = list2;
                    rp.DataBind();
                }
            }
        }

        private List<ZXJH_RealTimeControl> GetAllRealTimeControlInfos(Repeater rp, int idx)
        {
            List<ZXJH_RealTimeControl> list2 = new List<ZXJH_RealTimeControl>();
            ZXJH_RealTimeControl rc;
            foreach (RepeaterItem it in rp.Items)
            {
                if (idx != it.ItemIndex)
                {
                    rc = new ZXJH_RealTimeControl();
                    TextBox txtRCWork = (TextBox)it.FindControl("txtRCWork");
                    TextBox txtRCSYID = (TextBox)it.FindControl("txtRCSYID");
                    TextBox txtRCStartTime = (TextBox)it.FindControl("txtRCStartTime");
                    TextBox txtRCEndTime = (TextBox)it.FindControl("txtRCEndTime");

                    if (CheckCtrlExits(txtRCWork, "工作"))
                        rc.Work = txtRCWork.Text;
                    else
                        return null;
                    if (CheckCtrlExits(txtRCSYID, "试验ID"))
                        rc.SYID = txtRCSYID.Text;
                    else
                        return null;
                    if (CheckCtrlExits(txtRCStartTime, "开始时间"))
                        rc.StartTime = txtRCStartTime.Text;
                    else
                        return null;
                    if (CheckCtrlExits(txtRCEndTime, "结束时间"))
                        rc.EndTime = txtRCEndTime.Text;
                    else
                        return null;
                    list2.Add(rc);
                }
            }
            return list2;
        }

        protected void SYEstimate_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            List<ZXJH_SYEstimate> list2;
            ZXJH_SYEstimate sye;
            Repeater rp = (Repeater)source;
            if (e.CommandName == "Add")
            {
                list2 = GetAllSYEstimateInfos(rp, -1);
                sye = new ZXJH_SYEstimate();
                sye.SYID = "";
                sye.StartTime = "";
                sye.EndTime = "";
                list2.Add(sye);
                rp.DataSource = list2;
                rp.DataBind();

            }
            if (e.CommandName == "Del")
            {
                if (rp.Items.Count <= 1)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "del", "<script type='text/javascript'>showMsg('最后一条，无法删除!');</script>");
                }
                else
                {
                    list2 = GetAllSYEstimateInfos(rp, e.Item.ItemIndex);
                    rp.DataSource = list2;
                    rp.DataBind();
                }
            }
        }

        private List<ZXJH_SYEstimate> GetAllSYEstimateInfos(Repeater rp, int idx)
        {
            List<ZXJH_SYEstimate> list2 = new List<ZXJH_SYEstimate>();
            ZXJH_SYEstimate sye;
            foreach (RepeaterItem it in rp.Items)
            {
                if (it.ItemIndex != idx)
                {
                    sye = new ZXJH_SYEstimate();
                    TextBox txtESYID = (TextBox)it.FindControl("txtESYID");
                    TextBox txtEStartTime = (TextBox)it.FindControl("txtEStartTime");
                    TextBox txtEEndTime = (TextBox)it.FindControl("txtEEndTime");

                    if (CheckCtrlExits(txtESYID, "试验ID"))
                        sye.SYID = txtESYID.Text;
                    else
                        return null;
                    if (CheckCtrlExits(txtEStartTime, "开始时间"))
                        sye.StartTime = txtEStartTime.Text;
                    else
                        return null;
                    if (CheckCtrlExits(txtEEndTime, "完成时间"))
                        sye.EndTime = txtEEndTime.Text;
                    else
                        return null;
                    list2.Add(sye);
                }
            }
            return list2;
        }

        protected void rpSYContentSC_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            List<ZXJH_SYContent_SC> list2;
            ZXJH_SYContent_SC sy;
            Repeater rp = (Repeater)source;
            if (e.CommandName == "Add")
            {
                list2 = GetAllSYContentSCInfos(rp, -1);
                sy = new ZXJH_SYContent_SC();
                list2.Add(sy);
                rp.DataSource = list2;
                rp.DataBind();

            }
            if (e.CommandName == "Del")
            {
                if (rp.Items.Count <= 1)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "del", "<script type='text/javascript'>showMsg('最后一条，无法删除!');</script>");
                }
                else
                {
                    list2 = GetAllSYContentSCInfos(rp, e.Item.ItemIndex);
                    rp.DataSource = list2;
                    rp.DataBind();
                }
            }
        }

        private List<ZXJH_SYContent_SC> GetAllSYContentSCInfos(Repeater rp, int idx)
        {
            List<ZXJH_SYContent_SC> list2 = new List<ZXJH_SYContent_SC>();
            ZXJH_SYContent_SC sy;
            foreach (RepeaterItem it in rp.Items)
            {
                if (it.ItemIndex != idx)
                {
                    sy = new ZXJH_SYContent_SC();
                    #region 数传
                    DropDownList ddlDW = (DropDownList)it.FindControl("ddlDW");
                    DropDownList ddlSB = (DropDownList)it.FindControl("ddlSB");
                    DropDownList ddlSCFrequencyBand = (DropDownList)it.FindControl("ddlSCFrequencyBand");
                    TextBox txtSCLaps = (TextBox)it.FindControl("txtSCLaps");
                    TextBox txtSCStartTime = (TextBox)it.FindControl("txtSCStartTime");
                    TextBox txtSCEndTime = (TextBox)it.FindControl("txtSCEndTime");

                    sy.SY_SCStationNO = ddlDW.SelectedValue;
                    sy.SY_SCEquipmentNO = ddlSB.SelectedValue;
                    sy.SY_SCFrequencyBand = ddlSCFrequencyBand.SelectedValue;
                    sy.SY_SCLaps = txtSCLaps.Text;
                    sy.SY_SCStartTime = txtSCStartTime.Text;
                    sy.SY_SCEndTime = txtSCEndTime.Text;
                    #endregion
                    list2.Add(sy);
                }
            }
            return list2;
        }

        protected void rpSYContentCK_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            List<ZXJH_SYContent_CK> list2 = new List<ZXJH_SYContent_CK>();
            ZXJH_SYContent_CK sy;
            Repeater rp = (Repeater)source;
            if (e.CommandName == "Add")
            {
                list2 = GetAllSYContentCKInfos(rp, -1);
                sy = new ZXJH_SYContent_CK();
                list2.Add(sy);
                rp.DataSource = list2;
                rp.DataBind();
            }
            if (e.CommandName == "Del")
            {
                if (rp.Items.Count <= 1)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "del", "<script type='text/javascript'>showMsg('最后一条，无法删除!');</script>");
                }
                else
                {
                    list2 = GetAllSYContentCKInfos(rp, e.Item.ItemIndex);
                    rp.DataSource = list2;
                    rp.DataBind();
                }
            }
        }

        private List<ZXJH_SYContent_CK> GetAllSYContentCKInfos(Repeater rp, int idx)
        {
            List<ZXJH_SYContent_CK> list2 = new List<ZXJH_SYContent_CK>();
            ZXJH_SYContent_CK sy;
            foreach (RepeaterItem it in rp.Items)
            {
                if (it.ItemIndex != idx)
                {
                    sy = new ZXJH_SYContent_CK();
                    #region 测控
                    DropDownList ddlDW = (DropDownList)it.FindControl("ddlDW");
                    DropDownList ddlSB = (DropDownList)it.FindControl("ddlSB");
                    TextBox txtCKStartTime = (TextBox)it.FindControl("txtCKStartTime");
                    TextBox txtCKEndTime = (TextBox)it.FindControl("txtCKEndTime");
                    TextBox txtCKLaps = (TextBox)it.FindControl("txtCKLaps");

                    sy.SY_CKStationNO = ddlDW.SelectedValue;
                    sy.SY_CKEquipmentNO = ddlSB.SelectedValue;
                    sy.SY_CKStartTime = txtCKStartTime.Text;
                    sy.SY_CKEndTime = txtCKEndTime.Text;
                    sy.SY_CKLaps = txtCKLaps.Text;
                    #endregion
                    list2.Add(sy);
                }
            }
            return list2;
        }

        protected void rpSYContentZS_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            List<ZXJH_SYContent_ZS> list2;
            ZXJH_SYContent_ZS sy;
            Repeater rp = (Repeater)source;
            if (e.CommandName == "Add")
            {
                list2 = GetAllSYContentZSInfos(rp, -1);
                sy = new ZXJH_SYContent_ZS();
                list2.Add(sy);
                rp.DataSource = list2;
                rp.DataBind();

            }
            if (e.CommandName == "Del")
            {
                if (rp.Items.Count <= 1)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "del", "<script type='text/javascript'>showMsg('最后一条，无法删除!');</script>");
                }
                else
                {
                    list2 = GetAllSYContentZSInfos(rp, e.Item.ItemIndex);
                    rp.DataSource = list2;
                    rp.DataBind();
                }
            }
        }

        private List<ZXJH_SYContent_ZS> GetAllSYContentZSInfos(Repeater rp, int idx)
        {
            List<ZXJH_SYContent_ZS> list2 = new List<ZXJH_SYContent_ZS>();
            ZXJH_SYContent_ZS sy;
            foreach (RepeaterItem it in rp.Items)
            {
                if (it.ItemIndex != idx)
                {
                    sy = new ZXJH_SYContent_ZS();
                    #region 注数
                    TextBox txtZSFirst = (TextBox)it.FindControl("txtZSFirst");
                    TextBox txtZSLast = (TextBox)it.FindControl("txtZSLast");
                    TextBox txtZSContent = (TextBox)it.FindControl("txtZSContent");

                    sy.SY_ZSFirst = txtZSFirst.Text;
                    sy.SY_ZSLast = txtZSLast.Text;
                    sy.SY_ZSContent = txtZSContent.Text;

                    #endregion
                    list2.Add(sy);
                }
            }
            return list2;
        }

        /// <summary>
        /// 试验内容
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        protected void rpSYContent_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            List<ZXJH_SYContent> list2;
            ZXJH_SYContent sy;
            Repeater rp = (Repeater)source;
            if (e.CommandName == "Add")
            {
                list2 = GetAllSYContentInfos(rp, -1);
                sy = new ZXJH_SYContent();
                sy.ZSList = new List<ZXJH_SYContent_ZS>();
                sy.ZSList.Add(new ZXJH_SYContent_ZS());
                sy.SCList = new List<ZXJH_SYContent_SC>();
                sy.SCList.Add(new ZXJH_SYContent_SC());
                sy.CKList = new List<ZXJH_SYContent_CK>();
                sy.CKList.Add(new ZXJH_SYContent_CK());
                list2.Add(sy);
                rp.DataSource = list2;
                rp.DataBind();

            }
            if (e.CommandName == "Del")
            {
                if (rp.Items.Count <= 1)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "del", "<script type='text/javascript'>showMsg('最后一条，无法删除!');</script>");
                }
                else
                {
                    list2 = GetAllSYContentInfos(rp, e.Item.ItemIndex);
                    rp.DataSource = list2;
                    rp.DataBind();
                }
            }
        }

        private List<ZXJH_SYContent> GetAllSYContentInfos(Repeater rp, int idx)
        {
            List<ZXJH_SYContent> list2 = new List<ZXJH_SYContent>();
            ZXJH_SYContent sy;
            foreach (RepeaterItem it in rp.Items)
            {
                if (it.ItemIndex != idx)
                {
                    sy = new ZXJH_SYContent();
                    ucs.ucSatellite ddlSYSatID = (ucs.ucSatellite)it.FindControl("ddlSYSatID");
                    sy.SatID = ddlSYSatID.SelectedValue;
                    #region 试验
                    TextBox txtSYID = (TextBox)it.FindControl("txtSYID");
                    TextBox txtSYName = (TextBox)it.FindControl("txtSYName");
                    TextBox txtSYStartDateTime = (TextBox)it.FindControl("txtSYStartDateTime");
                    TextBox txtSYEndDateTime = (TextBox)it.FindControl("txtSYEndDateTime");
                    TextBox txtSYDays = (TextBox)it.FindControl("txtSYDays");
                    TextBox txtSYNote = (TextBox)it.FindControl("txtSYNote");

                    sy.SYID = txtSYID.Text;
                    sy.SYName = txtSYName.Text;
                    sy.SYStartTime = txtSYStartDateTime.Text;
                    sy.SYEndTime = txtSYEndDateTime.Text;
                    sy.SYDays = txtSYDays.Text;
                    sy.SYNote = txtSYNote.Text;
                    #endregion

                    //数传
                    sy.SCList = GetAllSYContentSCInfos((Repeater)it.FindControl("rpSYContentSC"), -1);
                    sy.CKList = GetAllSYContentCKInfos((Repeater)it.FindControl("rpSYContentCK"), -1);
                    sy.ZSList = GetAllSYContentZSInfos((Repeater)it.FindControl("rpSYContentZS"), -1);
                    list2.Add(sy);
                }
            }
            return list2;
        }

        protected void rpCommandMake_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Add")
            {
                List<ZXJH_CommandMake> list2 = new List<ZXJH_CommandMake>();
                ZXJH_CommandMake obj;
                Repeater rp = (Repeater)source;
                foreach (RepeaterItem it in rp.Items)
                {
                    obj = new ZXJH_CommandMake();
                    ucs.ucSatellite txtWork_Command_SatID = (ucs.ucSatellite)it.FindControl("txtWork_Command_SatID");
                    TextBox txtWork_Command_SYID = (TextBox)it.FindControl("txtWork_Command_SYID");
                    TextBox txtWork_Command_Programe = (TextBox)it.FindControl("txtWork_Command_Programe");
                    TextBox txtWork_Command_FinishTime = (TextBox)it.FindControl("txtWork_Command_FinishTime");
                    TextBox txtWork_Command_UpWay = (TextBox)it.FindControl("txtWork_Command_UpWay");
                    TextBox txtWork_Command_UpTime = (TextBox)it.FindControl("txtWork_Command_UpTime");
                    TextBox txtWork_Command_Note = (TextBox)it.FindControl("txtWork_Command_Note");

                    obj.Work_Command_SatID = txtWork_Command_SatID.SelectedValue;
                    obj.Work_Command_SYID = txtWork_Command_SYID.Text;
                    obj.Work_Command_Programe = txtWork_Command_Programe.Text;
                    obj.Work_Command_FinishTime = txtWork_Command_FinishTime.Text;
                    obj.Work_Command_UpWay = txtWork_Command_UpWay.Text;
                    obj.Work_Command_UpTime = txtWork_Command_UpTime.Text;
                    obj.Work_Command_Note = txtWork_Command_Note.Text;

                    list2.Add(obj);
                }
                obj = new ZXJH_CommandMake();
                list2.Add(obj);
                rp.DataSource = list2;
                rp.DataBind();

            }
            if (e.CommandName == "Del")
            {
                Repeater rp = (Repeater)source;
                if (rp.Items.Count <= 1)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "del", "<script type='text/javascript'>showMsg('最后一条，无法删除!');</script>");
                }
                else
                {
                    List<ZXJH_CommandMake> list2 = new List<ZXJH_CommandMake>();
                    ZXJH_CommandMake obj;

                    foreach (RepeaterItem it in rp.Items)
                    {
                        if (e.Item.ItemIndex != it.ItemIndex)
                        {
                            obj = new ZXJH_CommandMake();
                            ucs.ucSatellite txtWork_Command_SatID = (ucs.ucSatellite)it.FindControl("txtWork_Command_SatID");
                            TextBox txtWork_Command_SYID = (TextBox)it.FindControl("txtWork_Command_SYID");
                            TextBox txtWork_Command_Programe = (TextBox)it.FindControl("txtWork_Command_Programe");
                            TextBox txtWork_Command_FinishTime = (TextBox)it.FindControl("txtWork_Command_FinishTime");
                            TextBox txtWork_Command_UpWay = (TextBox)it.FindControl("txtWork_Command_UpWay");
                            TextBox txtWork_Command_UpTime = (TextBox)it.FindControl("txtWork_Command_UpTime");
                            TextBox txtWork_Command_Note = (TextBox)it.FindControl("txtWork_Command_Note");

                            obj.Work_Command_SatID = txtWork_Command_SatID.SelectedValue;
                            obj.Work_Command_SYID = txtWork_Command_SYID.Text;
                            obj.Work_Command_Programe = txtWork_Command_Programe.Text;
                            obj.Work_Command_FinishTime = txtWork_Command_FinishTime.Text;
                            obj.Work_Command_UpWay = txtWork_Command_UpWay.Text;
                            obj.Work_Command_UpTime = txtWork_Command_UpTime.Text;
                            obj.Work_Command_Note = txtWork_Command_Note.Text;

                            list2.Add(obj);
                        }
                    }
                    rp.DataSource = list2;
                    rp.DataBind();
                }
            }
        }

        private List<ZXJH_CommandMake> GetAllCommandMakeInfos(Repeater rp, int idx)
        {
            List<ZXJH_CommandMake> list2 = new List<ZXJH_CommandMake>();
            ZXJH_CommandMake obj;
            foreach (RepeaterItem it in rp.Items)
            {
                if (it.ItemIndex != idx)
                {
                    obj = new ZXJH_CommandMake();
                    ucs.ucSatellite txtWork_Command_SatID = (ucs.ucSatellite)it.FindControl("txtWork_Command_SatID");
                    TextBox txtWork_Command_SYID = (TextBox)it.FindControl("txtWork_Command_SYID");
                    TextBox txtWork_Command_Programe = (TextBox)it.FindControl("txtWork_Command_Programe");
                    TextBox txtWork_Command_FinishTime = (TextBox)it.FindControl("txtWork_Command_FinishTime");
                    TextBox txtWork_Command_UpWay = (TextBox)it.FindControl("txtWork_Command_UpWay");
                    TextBox txtWork_Command_UpTime = (TextBox)it.FindControl("txtWork_Command_UpTime");
                    TextBox txtWork_Command_Note = (TextBox)it.FindControl("txtWork_Command_Note");

                    obj.Work_Command_SatID = txtWork_Command_SatID.SelectedValue;
                    obj.Work_Command_SYID = txtWork_Command_SYID.Text;
                    obj.Work_Command_Programe = txtWork_Command_Programe.Text;
                    obj.Work_Command_FinishTime = txtWork_Command_FinishTime.Text;
                    obj.Work_Command_UpWay = txtWork_Command_UpWay.Text;
                    obj.Work_Command_UpTime = txtWork_Command_UpTime.Text;
                    obj.Work_Command_Note = txtWork_Command_Note.Text;

                    list2.Add(obj);
                }
            }
            return list2;
        }

        protected void rpSYContent_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    ZXJH_SYContent sy = (ZXJH_SYContent)e.Item.DataItem;
                    #region 正常
                    Repeater rpSC = e.Item.FindControl("rpSYContentSC") as Repeater;
                    Repeater rpCK = e.Item.FindControl("rpSYContentCK") as Repeater;
                    Repeater rpZS = e.Item.FindControl("rpSYContentZS") as Repeater;
                    ucs.ucSatellite ddlSYSatID = e.Item.FindControl("ddlSYSatID") as ucs.ucSatellite;

                    ddlSYSatID.SelectedValue = sy.SatID;
                    rpSC.DataSource = sy.SCList;
                    rpSC.DataBind();
                    rpCK.DataSource = sy.CKList;
                    rpCK.DataBind();
                    rpZS.DataSource = sy.ZSList;
                    rpZS.DataBind();

                    #endregion
                }
            }
            catch (Exception ex)
            {
                throw (new AspNetException("绑定中心计划信息-试验内容出现异常，异常原因", ex));
            }
            finally { }
        }

        protected void rpSYContentSC_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    ZXJH_SYContent_SC sy = (ZXJH_SYContent_SC)e.Item.DataItem;
                    InitialDWSB("数传", "ddlDW", "ddlSB", e, sy.SY_SCStationNO, sy.SY_SCEquipmentNO);
                    DropDownList ddlSCFrequencyBand = (DropDownList)e.Item.FindControl("ddlSCFrequencyBand") as DropDownList;
                    ddlSCFrequencyBand.SelectedValue = sy.SY_SCFrequencyBand;
                }
            }
            catch (Exception ex)
            {
                throw (new AspNetException("绑定中心计划信息-试验内容之数传出现异常，异常原因", ex));
            }
            finally { }
        }

        private void InitialDWSB(string source, string ctrlDWName, string ctrlSBName, RepeaterItemEventArgs e
            , string dwValue, string sbValue)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    DMZ oDMZ = new DMZ();
                    //工作单位
                    DropDownList ddlDW = (DropDownList)e.Item.FindControl(ctrlDWName) as DropDownList;
                    ddlDW.DataSource = oDMZ.Cache;
                    ddlDW.DataTextField = "DMZName";
                    ddlDW.DataValueField = "DMZCode";
                    ddlDW.DataBind();
                    ddlDW.SelectedValue = dwValue;

                    //设备代号
                    DropDownList ddlSB = (DropDownList)e.Item.FindControl(ctrlSBName) as DropDownList;
                    GroundResource oGR = new GroundResource();
                    oGR.DMZCode = ddlDW.SelectedValue;
                    ddlSB.DataSource = oGR.SelectByDMZCode();
                    ddlSB.DataTextField = "EQUIPMENTNAME";
                    ddlSB.DataValueField = "EQUIPMENTCODE";
                    ddlSB.DataBind();
                    ddlSB.SelectedValue = sbValue;
                }
            }
            catch (Exception ex)
            {
                throw (new AspNetException(string.Format("绑定中心计划-{0}站设备信息出现异常，异常原因", source), ex));
            }
        }

        protected void rpSYContentCK_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    ZXJH_SYContent_CK sy = (ZXJH_SYContent_CK)e.Item.DataItem;
                    InitialDWSB("测控", "ddlDW", "ddlSB", e, sy.SY_CKStationNO, sy.SY_CKEquipmentNO);
                }
            }
            catch (Exception ex)
            {
                throw (new AspNetException("绑定中心计划信息-试验内容之测控信息出现异常，异常原因", ex));
            }
            finally { }
        }

        protected void rpCommandMake_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    ZXJH_CommandMake wc = (ZXJH_CommandMake)e.Item.DataItem;
                    ucs.ucSatellite txtWork_Command_SatID = (ucs.ucSatellite)e.Item.FindControl("txtWork_Command_SatID") as ucs.ucSatellite;

                    txtWork_Command_SatID.SelectedValue = wc.Work_Command_SatID;
                }
            }
            catch (Exception ex)
            {
                throw (new AspNetException("绑定中心计划信息-指令制作出现异常，异常原因", ex));
            }
            finally { }
        }

        protected void rpDirectAndMonitor_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    ZXJH_DirectAndMonitor dm = (ZXJH_DirectAndMonitor)e.Item.DataItem;
                    DropDownList ddlDMRTTask = (DropDownList)e.Item.FindControl("ddlDMRTTask") as DropDownList;
                    ddlDMRTTask.SelectedValue = dm.RealTimeDemoTask;
                }
            }
            catch (Exception ex)
            {
                throw (new AspNetException("绑定中心计划信息-指挥与监视出现异常，异常原因", ex));
            }
            finally { }
        }

        protected void rpWork_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    ZXJH_WorkContent wc = (ZXJH_WorkContent)e.Item.DataItem;
                    DropDownList ddlWC_Work = (DropDownList)e.Item.FindControl("ddlWC_Work") as DropDownList;
                    ddlWC_Work.SelectedValue = wc.Work;
                }
            }
            catch (Exception ex)
            {
                throw (new AspNetException("绑定中心计划信息-任务管理出现异常，异常原因", ex));
            }
            finally { }
        }

        protected void rpSYDataHandle_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    ZXJH_SYDataHandle sy = (ZXJH_SYDataHandle)e.Item.DataItem;
                    InitialDWSB("实时试验数据处理主站", "ddlMainDW", "ddlMainSB", e, sy.MainStation, sy.MainStationEquipment);
                    InitialDWSB("实时试验数据处理备站", "ddlBakDW", "ddlBakSB", e, sy.BakStation, sy.BakStationEquipment);
                    ucs.ucSatellite ddlSYDataHandle_SatID = (ucs.ucSatellite)e.Item.FindControl("ddlSYDataHandle_SatID") as ucs.ucSatellite;
                    ddlSYDataHandle_SatID.SelectedValue = sy.SatID;
                }
            }
            catch (Exception ex)
            {
                throw (new AspNetException("绑定中心计划信息-实时试验数据处理出现异常，异常原因", ex));
            }
            finally { }
        }

        /// <summary>
        /// 保存计划
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                isTempJH = GetIsTempJHValue();
                string taskID = string.Empty;
                string satID = string.Empty;
                new Task().GetTaskNoSatID(ucOutTask1.SelectedValue, out taskID, out satID);

                #region basic
                ZXJH obj = new ZXJH();
                obj.Date = txtDate.Text;
                obj.SYContents = GetAllSYContentInfos(rpSYContent, -1);
                obj.WorkContents = GetAllWorkInfo(rpWork, -1);
                obj.CommandMakes = GetAllCommandMakeInfos(rpCommandMake, -1);
                obj.SYDataHandles = GetAllSYDataHandleInfos(rpSYDataHandle, -1);
                obj.DirectAndMonitors = GetAllDirectAndMonitorInfos(rpDirectAndMonitor, -1);
                obj.RealTimeControls = GetAllRealTimeControlInfos(rpRealTimeControl, -1);
                obj.SYEstimates = GetAllSYEstimateInfos(rpSYEstimate, -1);
                obj.SYCount = obj.SYContents.Count.ToString();  //对应日期的试验个数=试验内容的个数
                #endregion

                obj.TaskID = taskID;    //任务ID
                obj.SatID = satID;    //卫星ID

                PlanFileCreator creater = new PlanFileCreator(isTempJH);
                if (hfStatus.Value == "new")
                {
                    string filepath = creater.CreateZXJHFile(obj, 0);

                    DataAccessLayer.PlanManage.JH jh = new DataAccessLayer.PlanManage.JH(isTempJH)
                    {
                        TaskID = obj.TaskID,
                        PlanType = "ZXJH",
                        PlanID = (new Sequence()).GetZXJHSequnce(),
                        StartTime = Convert.ToDateTime(txtPlanStartTime.Text.Trim()),
                        EndTime = Convert.ToDateTime(txtPlanEndTime.Text.Trim()),
                        SRCType = 0,
                        FileIndex = filepath,
                        SatID = obj.SatID,
                        Reserve = txtNote.Text
                    };
                    var result = jh.Add();
                    ShowMsg(result == FieldVerifyResult.Success, obj);
                    HfID.Value = jh.ID.ToString();
                }
                else
                {
                    //当任务和卫星更新时，需要更新文件名称
                    if (hfTaskID.Value != ucOutTask1.SelectedValue)
                    {
                        string filepath = creater.CreateZXJHFile(obj, 0);

                        DataAccessLayer.PlanManage.JH jh = new DataAccessLayer.PlanManage.JH(isTempJH)
                        {
                            Id = Convert.ToInt32(HfID.Value),
                            TaskID = obj.TaskID,
                            StartTime = Convert.ToDateTime(txtPlanStartTime.Text.Trim()),
                            EndTime = Convert.ToDateTime(txtPlanEndTime.Text.Trim()),
                            FileIndex = filepath,
                            SatID = obj.SatID,
                            Reserve = txtNote.Text
                        };
                        var result = jh.Update();
                        ShowMsg(result == FieldVerifyResult.Success, obj);
                        HfID.Value = jh.ID.ToString();
                    }
                    else
                    {
                        creater.FilePath = HfFileIndex.Value;
                        creater.CreateZXJHFile(obj, 1);
                        ShowMsg(true, obj);
                        if (!isTempJH)
                        {
                            DataAccessLayer.PlanManage.JH jh = new DataAccessLayer.PlanManage.JH(isTempJH)
                            {
                                Id = Convert.ToInt32(HfID.Value),
                                SENDSTATUS = 0,
                                USESTATUS = 0
                            };
                            var result = jh.UpdateStatus();
                            ShowMsg(result == FieldVerifyResult.Success, obj);
                        }
                    }
                }
                //更新隐藏域的任务ID
                hfTaskID.Value = ucOutTask1.SelectedValue;
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
                isTempJH = GetIsTempJHValue();
                string taskID = string.Empty;
                string satID = string.Empty;
                new Task().GetTaskNoSatID(ucOutTask1.SelectedValue, out taskID, out satID);

                #region basic
                ZXJH obj = new ZXJH();
                obj.Date = txtDate.Text;
                obj.SYContents = GetAllSYContentInfos(rpSYContent, -1);
                obj.WorkContents = GetAllWorkInfo(rpWork, -1);
                obj.CommandMakes = GetAllCommandMakeInfos(rpCommandMake, -1);
                obj.SYDataHandles = GetAllSYDataHandleInfos(rpSYDataHandle, -1);
                obj.DirectAndMonitors = GetAllDirectAndMonitorInfos(rpDirectAndMonitor, -1);
                obj.RealTimeControls = GetAllRealTimeControlInfos(rpRealTimeControl, -1);
                obj.SYEstimates = GetAllSYEstimateInfos(rpSYEstimate, -1);
                obj.SYCount = obj.SYContents.Count.ToString();  //对应日期的试验个数=试验内容的个数
                #endregion

                obj.TaskID = taskID;
                obj.SatID = satID;
                //检查文件是否已经存在
                PlanFileCreator creater = new PlanFileCreator(isTempJH);
                if (creater.TestZXJHFileName(obj))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "File", "<script type='text/javascript'>showMsg('存在同名文件，请一分钟后重试');</script>");
                    return;
                }
                string filepath = creater.CreateZXJHFile(obj, 0);

                DataAccessLayer.PlanManage.JH jh = new DataAccessLayer.PlanManage.JH(isTempJH)
                {
                    TaskID = obj.TaskID,
                    PlanType = "ZXJH",
                    PlanID = (new Sequence()).GetZXJHSequnce(),
                    StartTime = Convert.ToDateTime(txtPlanStartTime.Text.Trim()),
                    EndTime = Convert.ToDateTime(txtPlanEndTime.Text.Trim()),
                    SRCType = 0,
                    FileIndex = filepath,
                    SatID = obj.SatID,
                    Reserve = txtNote.Text
                };
                var result = jh.Add();
                ShowMsg(result == FieldVerifyResult.Success, obj);
                HfID.Value = jh.ID.ToString();
            }
            catch (Exception ex)
            {
                throw (new AspNetException("保存计划信息出现异常，异常原因", ex));
            }
            finally { }
        }

        protected void btnFormal_Click(object sender, EventArgs e)
        {
            try
            {
                string taskID = string.Empty;
                string satID = string.Empty;
                new Task().GetTaskNoSatID(ucOutTask1.SelectedValue, out taskID, out satID);

                #region basic
                ZXJH obj = new ZXJH();
                obj.Date = txtDate.Text;
                obj.SYContents = GetAllSYContentInfos(rpSYContent, -1);
                obj.WorkContents = GetAllWorkInfo(rpWork, -1);
                obj.CommandMakes = GetAllCommandMakeInfos(rpCommandMake, -1);
                obj.SYDataHandles = GetAllSYDataHandleInfos(rpSYDataHandle, -1);
                obj.DirectAndMonitors = GetAllDirectAndMonitorInfos(rpDirectAndMonitor, -1);
                obj.RealTimeControls = GetAllRealTimeControlInfos(rpRealTimeControl, -1);
                obj.SYEstimates = GetAllSYEstimateInfos(rpSYEstimate, -1);
                obj.SYCount = obj.SYContents.Count.ToString();  //对应日期的试验个数=试验内容的个数
                #endregion

                PlanFileCreator creater = new PlanFileCreator();
                obj.TaskID = taskID;
                obj.SatID = satID;
                //检查文件是否已经存在
                if (creater.TestZXJHFileName(obj))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "File", "<script type='text/javascript'>showMsg('存在同名文件，请一分钟后重试');</script>");
                    return;
                }
                string filepath = creater.CreateZXJHFile(obj, 0);

                DataAccessLayer.PlanManage.JH jh = new DataAccessLayer.PlanManage.JH()
                {
                    TaskID = obj.TaskID,
                    PlanType = "ZXJH",
                    PlanID = (new Sequence()).GetZXJHSequnce(),
                    StartTime = Convert.ToDateTime(txtPlanStartTime.Text.Trim()),
                    EndTime = Convert.ToDateTime(txtPlanEndTime.Text.Trim()),
                    SRCType = 0,
                    FileIndex = filepath,
                    SatID = obj.SatID,
                    Reserve = txtNote.Text
                };
                var result = jh.Add();
                ShowMsg(result == FieldVerifyResult.Success, obj);
                HfID.Value = jh.ID.ToString();

                //删除当前临时计划
                DataAccessLayer.PlanManage.JH jhtemp = new DataAccessLayer.PlanManage.JH(true)
                {
                    Id = Convert.ToInt32(HfID.Value),
                };
                var resulttemp = jhtemp.DeleteTempJH();
                txtSYCount.Text = obj.SYCount;

                #region 转成正式计划之后，禁用一些按钮
                btnSubmit.Visible = true;
                btnSaveTo.Visible = true;
                btnReset.Visible = false;
                btnFormal.Visible = false;
                btnSurePlan.Visible = !(btnFormal.Visible);
                #endregion
            }
            catch (Exception ex)
            {
                throw (new AspNetException("保存计划信息出现异常，异常原因", ex));
            }
            finally { }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(HfID.Value))
                {
                    Page.Response.Redirect(Request.CurrentExecutionFilePath, false);
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

        /// <summary>
        /// 上传用户文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(FileUpload1.FileName))
            {
                return;
            }

            string filename = FileUpload1.FileName.Substring(FileUpload1.FileName.LastIndexOf('\\') + 1);
            string filepath = GetFullFilePath(filename);
            hfStationFile.Value = filepath;

            FileUpload1.SaveAs(filepath);
            lblUpload.Visible = true;

            #region 读取文件内容
            StationInOutFileReader reader = new StationInOutFileReader();
            List<StationInOut> list;
            list = reader.Read(filename);

            rpDatas.DataSource = list;
            rpDatas.DataBind();
            #endregion

            ClientScript.RegisterStartupScript(this.GetType(), "File", "<script type='text/javascript'>showFileContentForm();</script>");
        }

        /// <summary>
        /// 获得完整路径
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        private string GetFullFilePath(string filename)
        {
            string path = System.Configuration.ConfigurationManager.AppSettings["StationInOutFilePath"];
            if (path != string.Empty)
            {
                if (path[path.Length - 1] != '\\')
                    path = path + @"\";
            }
            else
            {
                path = AppDomain.CurrentDomain.BaseDirectory + @"TempJHSavePath\";
            }
            path += filename;
            return path;
        }

        protected void btnGetStationData_Click(object sender, EventArgs e)
        {
            string filepath = hfStationFile.Value;  //文件路径
            string ids = txtIds.Text;   //行号

            System.IO.File.Delete(filepath);    //删除临时文件
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

        private bool CheckCtrlExits(Control ctrl, string msg)
        {
            if (ctrl != null)
                return true;
            else
            {
                ltMessage.Text = string.Format("{0}的内容不能为空", msg);
                return false;
            }
        }

        private void ShowMsg(bool sucess, ZXJH obj)
        {
            if (sucess)
                ltMessage.Text = "计划保存成功";
            else
                ltMessage.Text = "计划保存失败";
            txtSYCount.Text = obj.SYCount;
            hfTaskID.Value = ucOutTask1.SelectedValue;
        }

        /// <summary>
        /// 单位改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlDW_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlDW = sender as DropDownList;
            RepeaterItem rpi = (RepeaterItem)ddlDW.Parent;
            SetDdlSB(ddlDW, rpi, "ddlSB");
        }

        /// <summary>
        /// 主站单位改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlMainDW_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlDW = sender as DropDownList;
            RepeaterItem rpi = (RepeaterItem)ddlDW.Parent;
            SetDdlSB(ddlDW, rpi, "ddlMainSB");
        }

        /// <summary>
        /// 备站单位改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlBakDW_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlDW = sender as DropDownList;
            RepeaterItem rpi = (RepeaterItem)ddlDW.Parent;
            SetDdlSB(ddlDW, rpi, "ddlBakSB");
        }

        private void SetDdlSB(DropDownList ddlDW, RepeaterItem rpi, string ctrlSBName)
        {
            DropDownList ddlSB = rpi.FindControl(ctrlSBName) as DropDownList;
            GroundResource oGR = new GroundResource();
            oGR.DMZCode = ddlDW.SelectedValue;
            ddlSB.DataSource = oGR.SelectByDMZCode();
            ddlSB.DataTextField = "EQUIPMENTNAME";
            ddlSB.DataValueField = "EQUIPMENTCODE";
            ddlSB.DataBind();
        }

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
                if (success)
                    ltMessage.Text = "计划确认成功";
                else
                    ltMessage.Text = "计划确认失败";
                hfTaskID.Value = ucOutTask1.SelectedValue;
            }
        }

        protected void btnCreateFile_Click(object sender, EventArgs e)
        {
            lbtFilePath_Click(null, e);
        }

        protected void lbtFilePath_Click(object sender, EventArgs e)
        {
            try
            {
                string strFilePath = HfFileIndex.Value;
                if (string.IsNullOrEmpty(strFilePath) || !System.IO.File.Exists(strFilePath))
                {
                    ltMessage.Text = "文件不存在。";
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
                throw (new AspNetException("中心运行计划-生成文件出现异常，异常原因", ex));
            }
        }
    }
}
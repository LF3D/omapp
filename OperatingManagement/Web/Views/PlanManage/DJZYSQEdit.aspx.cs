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

namespace OperatingManagement.Web.Views.PlanManage
{
    public partial class DJZYSQEdit : AspNetPage, IRouteContext
    {
        bool isTempJH = false;  //是否为临时计划,默认为正式计划

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["view"] == "1")
                    this.IsViewOrEdit = true;
                btnWord.Visible = false;
                btnFormal.Visible = false; 
                txtPlanStartTime.Attributes.Add("readonly", "true");
                txtPlanEndTime.Attributes.Add("readonly", "true");
                txtSCID.Attributes.Add("readonly", "true");
                InitialTask();

                //txtSequence.Text = long.MaxValue.ToString();
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
                    btnWord.Visible = true;
                    hfSBJHID.Value = "-1";
                    BindJhTable(sID);
                    BindXML();
                    hfURL.Value = "?type=DJZYSQ&startDate=" + Request.QueryString["startDate"] + "&endDate=" + Request.QueryString["endDate"]
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
                    initial();
                }
                if (this.IsViewOrEdit)
                {
                    SetControlsEnabled(Page, ControlNameEnum.All);
                    btnReturn.Visible = true;
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
        public void initial()
        {
            try
            {
                List<DJZYSQ_Task> listTask = new List<DJZYSQ_Task>();
                List<DJZYSQ_Task_AfterFeedBack> listaf = new List<DJZYSQ_Task_AfterFeedBack>();
                List<DJZYSQ_Task_ReakTimeTransfor> listrt = new List<DJZYSQ_Task_ReakTimeTransfor>();
                listaf.Add(new DJZYSQ_Task_AfterFeedBack());
                listrt.Add(new DJZYSQ_Task_ReakTimeTransfor());
                DJZYSQ_Task dm = new DJZYSQ_Task
                {
                    AfterFeedBacks = listaf,
                    ReakTimeTransfors = listrt
                };
                listTask.Add(dm);
                Repeater1.DataSource = listTask;
                Repeater1.DataBind();
            }
            catch (Exception ex)
            {
                throw (new AspNetException("初始化页面出现异常，异常原因", ex));
            }
            finally { }
        }
        private void BindJhTable(string sID)
        {
            try
            {
                string stmp = string.Empty;
                isTempJH = GetIsTempJHValue();
                List<JH> jh = (new JH(isTempJH)).SelectByIDS(sID);
                txtPlanStartTime.Text = jh[0].StartTime.ToString("yyyy-MM-dd HH:mm:ss");
                txtPlanEndTime.Text = jh[0].EndTime.ToString("yyyy-MM-dd HH:mm:ss");
                HfFileIndex.Value = jh[0].FileIndex;
                stmp = new Task().GetOutTaskNo(jh[0].TaskID, jh[0].SatID);
                hfTaskID.Value = stmp;
                ucOutTask1.SelectedValue = stmp;
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
                throw (new AspNetException("计划基本信息初始化出现异常，异常原因", ex));
            }
            finally { }
        }
        private void BindXML()
        {
            try
            {
                List<DJZYSQ_Task> listTask = new List<DJZYSQ_Task>();
                DJZYSQ_Task task;
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(HfFileIndex.Value);
                XmlNode root = xmlDoc.SelectSingleNode("测控资源使用申请/时间");
                //txtDatetime.Text = root.InnerText;
                root = xmlDoc.SelectSingleNode("测控资源使用申请/申请序列号");
                txtSequence.Text = root.InnerText;
                root = xmlDoc.SelectSingleNode("测控资源使用申请/航天器标识");
                txtSCID.Text = root.InnerText;
                root = xmlDoc.SelectSingleNode("测控资源使用申请/申请数量");
                txtTaskCount.Text = root.InnerText;

                root = xmlDoc.SelectSingleNode("测控资源使用申请/申请");
                foreach (XmlNode n in root.ChildNodes)
                {
                    if (n.Name == "申请内容")
                    {
                        task = new DJZYSQ_Task();
                        task.SXH = n["申请序号"].InnerText;
                        task.SXZ = n["申请性质"].InnerText;
                        task.MLB = n["任务类别"].InnerText;
                        task.FS = n["工作方式"].InnerText;
                        task.GZDY = n["工作单元"].InnerText;
                        task.SBDH = n["设备代号"].InnerText;
                        task.QC = n["圈次"].InnerText;
                        task.QB = n["圈标"].InnerText;
                        task.SHJ = n["测控事件类型"].InnerText;
                        task.FNUM = n["工作点频数量"].InnerText;
                        //task.FXH = n["工作点频/工作点频内容/点频序号"].InnerText;
                        //task.PDXZ = n["工作点频/工作点频内容/频段选择"].InnerText;
                        //task.DPXZ = n["工作点频/工作点频内容/点频选择"].InnerText;
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

        protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    #region 初始化各个下拉列表的值
                    //申请性质
                    DropDownList ddlSXZ = (DropDownList)e.Item.FindControl("ddlSXZ") as DropDownList;
                    ddlSXZ.DataSource = PlanParameters.ReadParameters("DJZYSQSXZ");
                    ddlSXZ.DataTextField = "Text";
                    ddlSXZ.DataValueField = "Value";
                    ddlSXZ.DataBind();
                    //工作方式
                    DropDownList ddlFS = (DropDownList)e.Item.FindControl("ddlFS") as DropDownList;
                    ddlFS.DataSource = PlanParameters.ReadParameters("DJZYSQFS");
                    ddlFS.DataTextField = "Text";
                    ddlFS.DataValueField = "Value";
                    ddlFS.DataBind();
                    //工作单元
                    DropDownList ddlGZDY = (DropDownList)e.Item.FindControl("ddlGZDY") as DropDownList;
                    ddlGZDY.DataSource = PlanParameters.ReadParameters("DJZYSQGZDY");
                    ddlGZDY.DataTextField = "Text";
                    ddlGZDY.DataValueField = "Value";
                    ddlGZDY.DataBind();
                    //设备代号
                    DropDownList ddlSBDH = (DropDownList)e.Item.FindControl("ddlSBDH") as DropDownList;
                    ddlSBDH.DataSource = PlanParameters.ReadParameters("DJZYSQSBDH");
                    ddlSBDH.DataTextField = "Text";
                    ddlSBDH.DataValueField = "Value";
                    ddlSBDH.DataBind();
                    //测控事件类型
                    DropDownList ddlSHJ = (DropDownList)e.Item.FindControl("ddlSHJ") as DropDownList;
                    ddlSHJ.DataSource = PlanParameters.ReadParameters("DJZYSQSHJ");
                    ddlSHJ.DataTextField = "Text";
                    ddlSHJ.DataValueField = "Value";
                    ddlSHJ.DataBind();

                    DJZYSQ_Task view = (DJZYSQ_Task)e.Item.DataItem;
                    ddlSXZ.SelectedValue = view.SXZ;
                    ddlFS.SelectedValue = view.FS;
                    ddlGZDY.SelectedValue = view.GZDY;
                    ddlSBDH.SelectedValue = view.SBDH;
                    ddlSHJ.SelectedValue = view.SHJ;

                    #endregion
                    #region 注册脚本事件
                    //工作单元与设备代号联动
                    ddlSBDH.Enabled = false;
                    TextBox txtSBDH = (TextBox)e.Item.FindControl("txtSBDH");
                    txtSBDH.Attributes.Add("readonly", "true");
                    txtSBDH.Text = ddlSBDH.Items.FindByText(ddlGZDY.SelectedItem.Text).Value;
                    ddlGZDY.Attributes.Add("onchange", "SetSBDH(this,'" + ddlSBDH.ClientID + "','" + txtSBDH.ClientID + "')");

                    //任务准备开始时间输入后，跟踪开始时间、任务开始时间、任务结束时间、跟踪结束时间
                    //默认分别依次累加30分钟、30秒、10分钟、30秒。
                    TextBox txtPreStartTime = (TextBox)e.Item.FindControl("txtPreStartTime");
                    TextBox txtStartTime = (TextBox)e.Item.FindControl("txtStartTime");
                    TextBox txtTrackStartTime = (TextBox)e.Item.FindControl("txtTrackStartTime");
                    TextBox txtWaveOnStartTime = (TextBox)e.Item.FindControl("txtWaveOnStartTime");
                    TextBox txtWaveOffStartTime = (TextBox)e.Item.FindControl("txtWaveOffStartTime");
                    TextBox txtTrackEndTime = (TextBox)e.Item.FindControl("txtTrackEndTime");
                    TextBox txtEndTime = (TextBox)e.Item.FindControl("txtEndTime");
                    txtPreStartTime.Attributes.Add("onblur", "SetDateTime(this,'" + txtStartTime.ClientID + "','"
                        + txtTrackStartTime.ClientID + "','" + txtWaveOnStartTime.ClientID + "','" + txtWaveOffStartTime.ClientID + "','"
                        + txtTrackEndTime.ClientID + "','" + txtEndTime.ClientID + "')");
                    txtTrackEndTime.Attributes.Add("onblur", "return ComparePreTimeAndTrackEndTime(this,'" + txtPreStartTime.ClientID + "')");
                    //任务类别
                    TextBox txtMLB = (TextBox)e.Item.FindControl("txtMLB");
                    txtMLB.Text = PlanParameters.ReadDJZYSQMLB();
                    txtMLB.Attributes.Add("readonly", "true");

                    //申请序号
                    TextBox txtSXH = (TextBox)e.Item.FindControl("txtSXH");
                    txtSXH.Text = (e.Item.ItemIndex + 1).ToString();
                    txtSXH.Attributes.Add("readonly", "true");

                    //圈标
                    TextBox txtQB = (TextBox)e.Item.FindControl("txtQB");
                    txtQB.Text = PlanParameters.ReadDJZYSQQB();
                    txtQB.Attributes.Add("readonly", "true");

                    #endregion
                    #region 添加,删除任务时，从ViewState里获取页面 “实时传输” 和“事后回放”的值
                    if (ViewState["arrG"] != null && ViewState["arrR"] != null && ViewState["arrA"] != null && ViewState["op"] != null)
                    {
                        ArrayList arrG = (ArrayList)ViewState["arrG"];
                        ArrayList arrR = (ArrayList)ViewState["arrR"];
                        ArrayList arrA = (ArrayList)ViewState["arrA"];
                        Repeater rpg = e.Item.FindControl("rpGZDP") as Repeater;
                        Repeater rpr = e.Item.FindControl("rpReakTimeTransfor") as Repeater;
                        Repeater rpa = e.Item.FindControl("rpAfterFeedBack") as Repeater;

                        List<DJZYSQ_Task_GZDP> listg = new List<DJZYSQ_Task_GZDP>();
                        List<DJZYSQ_Task_ReakTimeTransfor> listr = new List<DJZYSQ_Task_ReakTimeTransfor>();
                        List<DJZYSQ_Task_AfterFeedBack> lista = new List<DJZYSQ_Task_AfterFeedBack>();
                        DJZYSQ_Task_GZDP dp;
                        DJZYSQ_Task_ReakTimeTransfor rtt;
                        DJZYSQ_Task_AfterFeedBack afb;
                        string op = (string)ViewState["op"];
                        if (op == "Add")
                        {
                            #region DZDP
                            if (e.Item.ItemIndex <= arrG.Count - 1)
                            {
                                listg = (List<DJZYSQ_Task_GZDP>)arrG[e.Item.ItemIndex];
                                rpg.DataSource = listg;
                                rpg.DataBind();
                            }
                            else
                            {
                                dp = new DJZYSQ_Task_GZDP();
                                dp.FXH = "";
                                dp.PDXZ = "";
                                dp.DPXZ = "";
                                listg.Add(dp);
                                rpg.DataSource = listg;
                                rpg.DataBind();
                            }
                            #endregion
                            #region ReakTimeTransfor
                            if (e.Item.ItemIndex <= arrR.Count - 1)
                            {
                                listr = (List<DJZYSQ_Task_ReakTimeTransfor>)arrR[e.Item.ItemIndex];
                                rpr.DataSource = listr;
                                rpr.DataBind();
                            }
                            else
                            {
                                rtt = new DJZYSQ_Task_ReakTimeTransfor();
                                rtt.GBZ = "";
                                rtt.XBZ = "";
                                rtt.RTs = "";
                                rtt.RTe = "";
                                rtt.SL = "";
                                listr.Add(rtt);
                                rpr.DataSource = listr;
                                rpr.DataBind();
                            }
                            #endregion
                            #region AfterFeedBack
                            if (e.Item.ItemIndex <= arrA.Count - 1)
                            {
                                lista = (List<DJZYSQ_Task_AfterFeedBack>)arrA[e.Item.ItemIndex];
                                rpa.DataSource = lista;
                                rpa.DataBind();
                            }
                            else
                            {
                                afb = new DJZYSQ_Task_AfterFeedBack();
                                afb.GBZ = "";
                                afb.XBZ = "";
                                afb.Ts = "";
                                afb.Te = "";
                                afb.RTs = "";
                                afb.SL = "";
                                lista.Add(afb);
                                rpa.DataSource = lista;
                                rpa.DataBind();
                            }
                            #endregion
                        }
                        if (op == "Del")
                        {
                            #region DZDP
                            if (e.Item.ItemIndex <= arrG.Count - 1)
                            {
                                listg = (List<DJZYSQ_Task_GZDP>)arrG[e.Item.ItemIndex];
                                rpg.DataSource = listg;
                                rpg.DataBind();
                            }
                            else
                            {
                                dp = new DJZYSQ_Task_GZDP();
                                dp.FXH = "";
                                dp.PDXZ = "";
                                dp.DPXZ = "";
                                listg.Add(dp);
                                rpg.DataSource = listg;
                                rpg.DataBind();
                            }
                            #endregion
                            #region ReakTimeTransfor
                            if (e.Item.ItemIndex <= arrR.Count - 1)
                            {
                                listr = (List<DJZYSQ_Task_ReakTimeTransfor>)arrR[e.Item.ItemIndex];
                                rpr.DataSource = listr;
                                rpr.DataBind();
                            }
                            #endregion
                            #region AfterFeedBack
                            if (e.Item.ItemIndex <= arrA.Count - 1)
                            {
                                lista = (List<DJZYSQ_Task_AfterFeedBack>)arrA[e.Item.ItemIndex];
                                rpa.DataSource = lista;
                                rpa.DataBind();
                            }
                            #endregion
                        }
                    }
                    else
                    {
                        #region 正常
                        Repeater rpG = e.Item.FindControl("rpGZDP") as Repeater;
                        Repeater rpR = e.Item.FindControl("rpReakTimeTransfor") as Repeater;
                        Repeater rpA = e.Item.FindControl("rpAfterFeedBack") as Repeater;
                        int row = e.Item.ItemIndex;
                        List<DJZYSQ_Task_GZDP> list0 = new List<DJZYSQ_Task_GZDP>();
                        List<DJZYSQ_Task_ReakTimeTransfor> list1 = new List<DJZYSQ_Task_ReakTimeTransfor>();
                        List<DJZYSQ_Task_AfterFeedBack> list2 = new List<DJZYSQ_Task_AfterFeedBack>();
                        DJZYSQ_Task_GZDP dp;
                        DJZYSQ_Task_ReakTimeTransfor rt;
                        DJZYSQ_Task_AfterFeedBack afb;
                        bool hasDP = false;
                        bool hasSC = false;
                        bool hasHF = false;
                        if (hfStatus.Value == "new")
                        {
                            list0.Add(new DJZYSQ_Task_GZDP());
                            list1.Add(new DJZYSQ_Task_ReakTimeTransfor());
                            list2.Add(new DJZYSQ_Task_AfterFeedBack());
                        }
                        else
                        {
                            XmlDocument xmlDoc = new XmlDocument();
                            xmlDoc.Load(HfFileIndex.Value);
                            XmlNode root = xmlDoc.SelectSingleNode("测控资源使用申请/申请");
                            int i = 0;

                            foreach (XmlNode n in root.ChildNodes)
                            {
                                #region 申请
                                if (n.Name == "申请内容")
                                {
                                    foreach (XmlNode nd in n.ChildNodes)
                                    {
                                        if (row == i)
                                        {
                                            if (nd.Name == "工作点频")
                                            {
                                                foreach (XmlNode ndd in nd.ChildNodes)
                                                {
                                                    dp = new DJZYSQ_Task_GZDP();
                                                    dp.FXH = ndd["点频序号"].InnerText;
                                                    dp.PDXZ = ndd["频段选择"].InnerText;
                                                    dp.DPXZ = ndd["点频选择"].InnerText;
                                                    list0.Add(dp);
                                                    hasDP = true;
                                                }
                                            }
                                            if (nd.Name == "实时传输")
                                            {
                                                rt = new DJZYSQ_Task_ReakTimeTransfor();
                                                rt.GBZ = nd["格式标志"].InnerText;
                                                rt.XBZ = nd["信息流标志"].InnerText;
                                                rt.RTs = nd["数据传输开始时间"].InnerText;
                                                rt.RTe = nd["数据传输结束时间"].InnerText;
                                                rt.SL = nd["数据传输速率"].InnerText;
                                                list1.Add(rt);
                                                hasSC = true;
                                            }
                                            if (nd.Name == "事后回放")
                                            {
                                                afb = new DJZYSQ_Task_AfterFeedBack();
                                                afb.GBZ = nd["格式标志"].InnerText;
                                                afb.XBZ = nd["信息流标志"].InnerText;
                                                afb.Ts = nd["数据起始时间"].InnerText;
                                                afb.Te = nd["数据结束时间"].InnerText;
                                                afb.RTs = nd["数据传输开始时间"].InnerText;
                                                afb.SL = nd["数据传输速率"].InnerText;
                                                list2.Add(afb);
                                                hasHF = true;
                                            }
                                        }
                                    }
                                    i = i + 1;//记录是第几个任务
                                }
                                #endregion

                                if (!hasDP)
                                {
                                    dp = new DJZYSQ_Task_GZDP();
                                    list0.Add(dp);
                                }
                                if (!hasSC)
                                {
                                    rt = new DJZYSQ_Task_ReakTimeTransfor();
                                    list1.Add(rt);
                                }
                                if (!hasHF)
                                {
                                    afb = new DJZYSQ_Task_AfterFeedBack();
                                    list2.Add(afb);
                                }
                            }
                        }
                        rpG.DataSource = list0;
                        rpG.DataBind();
                        rpR.DataSource = list1;
                        rpR.DataBind();
                        rpA.DataSource = list2;
                        rpA.DataBind();

                        #endregion
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                throw (new AspNetException("绑定地面计划信息出现异常，异常原因", ex));
            }
            finally { }
        }

        protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            try
            {
                List<DJZYSQ_Task> list2 = new List<DJZYSQ_Task>();
                DJZYSQ_Task rt;
                List<DJZYSQ_Task_GZDP> listg = new List<DJZYSQ_Task_GZDP>();
                List<DJZYSQ_Task_ReakTimeTransfor> listr = new List<DJZYSQ_Task_ReakTimeTransfor>();
                List<DJZYSQ_Task_AfterFeedBack> lista = new List<DJZYSQ_Task_AfterFeedBack>();
                DJZYSQ_Task_GZDP dp;
                DJZYSQ_Task_ReakTimeTransfor rtt;
                DJZYSQ_Task_AfterFeedBack afb;
                Repeater rp = (Repeater)source;
                ArrayList arrG = new ArrayList();
                ArrayList arrR = new ArrayList();
                ArrayList arrA = new ArrayList();
                if (e.CommandName == "Add")
                {
                    ViewState["op"] = "Add";
                    foreach (RepeaterItem it in rp.Items)
                    {
                        #region GZDP
                        Repeater rpg = it.FindControl("rpGZDP") as Repeater;
                        foreach (RepeaterItem itg in rpg.Items)
                        {
                            dp = new DJZYSQ_Task_GZDP();
                            TextBox txtFXH = (TextBox)itg.FindControl("txtFXH");
                            DropDownList ddlPDXZ = (DropDownList)itg.FindControl("ddlPDXZ");
                            TextBox txtDPXZ = (TextBox)itg.FindControl("txtDPXZ");
                            if (txtFXH == null)
                                return;

                            dp.FXH = txtFXH.Text;
                            dp.PDXZ = ddlPDXZ.Text;
                            dp.DPXZ = txtDPXZ.Text;
                            listg.Add(dp);
                        }
                        arrG.Add(listg);
                        listg = new List<DJZYSQ_Task_GZDP>();
                        #endregion
                        #region ReakTimeTransfor
                        Repeater rps = it.FindControl("rpReakTimeTransfor") as Repeater;
                        foreach (RepeaterItem its in rps.Items)
                        {

                            rtt = new DJZYSQ_Task_ReakTimeTransfor();
                            TextBox txtFormatFlag = (TextBox)its.FindControl("txtFormatFlag");
                            TextBox txtInfoFlowFlag = (TextBox)its.FindControl("txtInfoFlowFlag");
                            TextBox txtTransStartTime = (TextBox)its.FindControl("txtTransStartTime");
                            TextBox txtTransEndTime = (TextBox)its.FindControl("txtTransEndTime");
                            TextBox txtTransSpeedRate = (TextBox)its.FindControl("txtTransSpeedRate");

                            rtt.GBZ = txtFormatFlag.Text;
                            rtt.XBZ = txtInfoFlowFlag.Text;
                            rtt.RTs = txtTransStartTime.Text;
                            rtt.RTe = txtTransEndTime.Text;
                            rtt.SL = txtTransSpeedRate.Text;
                            listr.Add(rtt);

                        }
                        arrR.Add(listr);
                        listr = new List<DJZYSQ_Task_ReakTimeTransfor>();
                        #endregion
                        #region AfterFeedBack
                        Repeater rpa = it.FindControl("rpAfterFeedBack") as Repeater;
                        foreach (RepeaterItem its in rpa.Items)
                        {

                            afb = new DJZYSQ_Task_AfterFeedBack();
                            TextBox txtFormatFlag = (TextBox)its.FindControl("FormatFlag");
                            TextBox txtInfoFlowFlag = (TextBox)its.FindControl("InfoFlowFlag");
                            TextBox txtDataStartTime = (TextBox)its.FindControl("DataStartTime");
                            TextBox txtDataEndTime = (TextBox)its.FindControl("DataEndTime");
                            TextBox txtTransStartTime = (TextBox)its.FindControl("TransStartTime");
                            TextBox txtTransSpeedRate = (TextBox)its.FindControl("TransSpeedRate");

                            afb.GBZ = txtFormatFlag.Text;
                            afb.XBZ = txtInfoFlowFlag.Text;
                            afb.Ts = txtDataStartTime.Text;
                            afb.Te = txtDataEndTime.Text;
                            afb.RTs = txtTransStartTime.Text;
                            afb.SL = txtTransSpeedRate.Text;
                            lista.Add(afb);

                        }
                        arrA.Add(lista);
                        lista = new List<DJZYSQ_Task_AfterFeedBack>();
                        #endregion
                        #region Task
                        rt = new DJZYSQ_Task();
                        TextBox txtSXH = (TextBox)it.FindControl("txtSXH");
                        DropDownList ddlSXZ = (DropDownList)it.FindControl("ddlSXZ");
                        TextBox txtMLB = (TextBox)it.FindControl("txtMLB");
                        DropDownList ddlFS = (DropDownList)it.FindControl("ddlFS");
                        DropDownList ddlGZDY = (DropDownList)it.FindControl("ddlGZDY");
                        DropDownList ddlSBDH = (DropDownList)it.FindControl("ddlSBDH");
                        TextBox txtQC = (TextBox)it.FindControl("txtQC");
                        TextBox txtQB = (TextBox)it.FindControl("txtQB");
                        DropDownList ddlSHJ = (DropDownList)it.FindControl("ddlSHJ");
                        //TextBox txtFNUM = (TextBox)it.FindControl("txtFNUM");
                        //TextBox txtFXH = (TextBox)it.FindControl("txtFXH");
                        //DropDownList ddlPDXZ = (DropDownList)it.FindControl("ddlPDXZ");
                        //TextBox txtDPXZ = (TextBox)it.FindControl("txtDPXZ");
                        TextBox txtTNUM = (TextBox)it.FindControl("txtTNUM");
                        TextBox txtPreStartTime = (TextBox)it.FindControl("txtPreStartTime");
                        TextBox txtStartTime = (TextBox)it.FindControl("txtStartTime");
                        TextBox txtTrackStartTime = (TextBox)it.FindControl("txtTrackStartTime");
                        TextBox txtWaveOnStartTime = (TextBox)it.FindControl("txtWaveOnStartTime");
                        TextBox txtWaveOffStartTime = (TextBox)it.FindControl("txtWaveOffStartTime");
                        TextBox txtTrackEndTime = (TextBox)it.FindControl("txtTrackEndTime");
                        TextBox txtEndTime = (TextBox)it.FindControl("txtEndTime");

                        rt.SXH = (list2.Count() + 1).ToString();//txtSXH.Text;
                        rt.SXZ = ddlSXZ.SelectedValue;
                        rt.MLB = txtMLB.Text;
                        rt.FS = ddlFS.SelectedValue;
                        rt.GZDY = ddlGZDY.SelectedValue;
                        rt.SBDH = ddlSBDH.SelectedValue;
                        rt.QC = txtQC.Text;
                        rt.QB = txtQB.Text;
                        rt.SHJ = ddlSHJ.SelectedValue;
                        //rt.FNUM = txtFNUM.Text;
                        //rt.FXH = txtFXH.Text;
                        //rt.PDXZ = ddlPDXZ.SelectedValue;
                        //rt.DPXZ = txtDPXZ.Text;
                        rt.TNUM = txtTNUM.Text;
                        rt.ZHB = txtPreStartTime.Text;
                        rt.RK = txtStartTime.Text;
                        rt.GZK = txtTrackStartTime.Text;
                        rt.KSHX = txtWaveOnStartTime.Text;
                        rt.GSHX = txtWaveOffStartTime.Text;
                        rt.GZJ = txtTrackEndTime.Text;
                        rt.JS = txtEndTime.Text;
                        list2.Add(rt);

                        #endregion
                    }
                    #region new a DJZYSQ_Task object
                    rt = new DJZYSQ_Task();
                    rt.SXH = "";
                    rt.SXZ = "";
                    rt.MLB = "";
                    rt.FS = "";
                    rt.GZDY = "";
                    rt.SBDH = "";
                    rt.QC = "";
                    rt.QB = "";
                    rt.SHJ = "";
                    rt.FNUM = "";
                    //rt.FXH = "";
                    //rt.PDXZ = "";
                    //rt.DPXZ = "";
                    rt.TNUM = "";
                    rt.ZHB = "";
                    rt.RK = "";
                    rt.GZK = "";
                    rt.KSHX = "";
                    rt.GSHX = "";
                    rt.GZJ = "";
                    rt.JS = "";
                    #endregion
                    list2.Add(rt);
                    ViewState["arrG"] = arrG;
                    ViewState["arrR"] = arrR;
                    ViewState["arrA"] = arrA;
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
                        ViewState["op"] = "Del";
                        foreach (RepeaterItem it in rp.Items)
                        {
                            if (e.Item.ItemIndex != it.ItemIndex)
                            {
                                #region GZDP
                                Repeater rpg = it.FindControl("rpGZDP") as Repeater;
                                foreach (RepeaterItem itg in rpg.Items)
                                {
                                    dp = new DJZYSQ_Task_GZDP();
                                    TextBox txtFXH = (TextBox)itg.FindControl("txtFXH");
                                    DropDownList ddlPDXZ = (DropDownList)itg.FindControl("ddlPDXZ");
                                    TextBox txtDPXZ = (TextBox)itg.FindControl("txtDPXZ");

                                    dp.FXH = txtFXH.Text;
                                    dp.PDXZ = ddlPDXZ.Text;
                                    dp.DPXZ = txtDPXZ.Text;
                                    listg.Add(dp);
                                }
                                arrG.Add(listg);
                                listg = new List<DJZYSQ_Task_GZDP>();
                                #endregion
                                #region ReakTimeTransfor
                                Repeater rps = it.FindControl("rpReakTimeTransfor") as Repeater;
                                foreach (RepeaterItem its in rps.Items)
                                {

                                    rtt = new DJZYSQ_Task_ReakTimeTransfor();
                                    TextBox txtFormatFlag = (TextBox)its.FindControl("txtFormatFlag");
                                    TextBox txtInfoFlowFlag = (TextBox)its.FindControl("txtInfoFlowFlag");
                                    TextBox txtTransStartTime = (TextBox)its.FindControl("txtTransStartTime");
                                    TextBox txtTransEndTime = (TextBox)its.FindControl("txtTransEndTime");
                                    TextBox txtTransSpeedRate = (TextBox)its.FindControl("txtTransSpeedRate");

                                    rtt.GBZ = txtFormatFlag.Text;
                                    rtt.XBZ = txtInfoFlowFlag.Text;
                                    rtt.RTs = txtTransStartTime.Text;
                                    rtt.RTe = txtTransEndTime.Text;
                                    rtt.SL = txtTransSpeedRate.Text;
                                    listr.Add(rtt);

                                }
                                arrR.Add(listr);
                                listr = new List<DJZYSQ_Task_ReakTimeTransfor>();
                                #endregion
                                #region AfterFeedBack
                                Repeater rpa = it.FindControl("rpAfterFeedBack") as Repeater;
                                foreach (RepeaterItem its in rpa.Items)
                                {

                                    afb = new DJZYSQ_Task_AfterFeedBack();
                                    TextBox txtFormatFlag = (TextBox)its.FindControl("FormatFlag");
                                    TextBox txtInfoFlowFlag = (TextBox)its.FindControl("InfoFlowFlag");
                                    TextBox txtDataStartTime = (TextBox)its.FindControl("DataStartTime");
                                    TextBox txtDataEndTime = (TextBox)its.FindControl("DataEndTime");
                                    TextBox txtTransStartTime = (TextBox)its.FindControl("TransStartTime");
                                    TextBox txtTransSpeedRate = (TextBox)its.FindControl("TransSpeedRate");

                                    afb.GBZ = txtFormatFlag.Text;
                                    afb.XBZ = txtInfoFlowFlag.Text;
                                    afb.Ts = txtDataStartTime.Text;
                                    afb.Te = txtDataEndTime.Text;
                                    afb.RTs = txtTransStartTime.Text;
                                    afb.SL = txtTransSpeedRate.Text;
                                    lista.Add(afb);

                                }
                                arrA.Add(lista);
                                lista = new List<DJZYSQ_Task_AfterFeedBack>();
                                #endregion
                                #region Task
                                rt = new DJZYSQ_Task();
                                TextBox txtSXH = (TextBox)it.FindControl("txtSXH");
                                DropDownList ddlSXZ = (DropDownList)it.FindControl("ddlSXZ");
                                TextBox txtMLB = (TextBox)it.FindControl("txtMLB");
                                DropDownList ddlFS = (DropDownList)it.FindControl("ddlFS");
                                DropDownList ddlGZDY = (DropDownList)it.FindControl("ddlGZDY");
                                DropDownList ddlSBDH = (DropDownList)it.FindControl("ddlSBDH");
                                TextBox txtQC = (TextBox)it.FindControl("txtQC");
                                TextBox txtQB = (TextBox)it.FindControl("txtQB");
                                DropDownList ddlSHJ = (DropDownList)it.FindControl("ddlSHJ");
                                //TextBox txtFNUM = (TextBox)it.FindControl("txtFNUM");
                                //TextBox txtFXH = (TextBox)it.FindControl("txtFXH");
                                //DropDownList ddlPDXZ = (DropDownList)it.FindControl("ddlPDXZ");
                                //TextBox txtDPXZ = (TextBox)it.FindControl("txtDPXZ");
                                TextBox txtTNUM = (TextBox)it.FindControl("txtTNUM");
                                TextBox txtPreStartTime = (TextBox)it.FindControl("txtPreStartTime");
                                TextBox txtStartTime = (TextBox)it.FindControl("txtStartTime");
                                TextBox txtTrackStartTime = (TextBox)it.FindControl("txtTrackStartTime");
                                TextBox txtWaveOnStartTime = (TextBox)it.FindControl("txtWaveOnStartTime");
                                TextBox txtWaveOffStartTime = (TextBox)it.FindControl("txtWaveOffStartTime");
                                TextBox txtTrackEndTime = (TextBox)it.FindControl("txtTrackEndTime");
                                TextBox txtEndTime = (TextBox)it.FindControl("txtEndTime");

                                rt.SXH = (list2.Count() + 1).ToString();//txtSXH.Text;
                                rt.SXZ = ddlSXZ.SelectedValue;
                                rt.MLB = txtMLB.Text;
                                rt.FS = ddlFS.SelectedValue;
                                rt.GZDY = ddlGZDY.SelectedValue;
                                rt.SBDH = ddlSBDH.SelectedValue;
                                rt.QC = txtQC.Text;
                                rt.QB = txtQB.Text;
                                rt.SHJ = ddlSHJ.SelectedValue;
                                //rt.FNUM = txtFNUM.Text;
                                //rt.FXH = txtFXH.Text;
                                //rt.PDXZ = ddlPDXZ.SelectedValue;
                                //rt.DPXZ = txtDPXZ.Text;
                                rt.TNUM = txtTNUM.Text;
                                rt.ZHB = txtPreStartTime.Text;
                                rt.RK = txtStartTime.Text;
                                rt.GZK = txtTrackStartTime.Text;
                                rt.KSHX = txtWaveOnStartTime.Text;
                                rt.GSHX = txtWaveOffStartTime.Text;
                                rt.GZJ = txtTrackEndTime.Text;
                                rt.JS = txtEndTime.Text;
                                list2.Add(rt);

                                #endregion
                            }
                        }
                        ViewState["arrG"] = arrG;
                        ViewState["arrR"] = arrR;
                        ViewState["arrA"] = arrA;
                        rp.DataSource = list2;
                        rp.DataBind();
                    }
                }//
            }
            catch (Exception ex)
            {
                throw (new AspNetException("绑定地面计划信息出现异常，异常原因", ex));
            }
            finally { }
        }

        protected void Repeater2_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    TextBox txtTransStartTime = (TextBox)e.Item.FindControl("txtTransStartTime");
                    TextBox txtTransEndTime = (TextBox)e.Item.FindControl("txtTransEndTime");
                    TextBox txtStartTime = (TextBox)((RepeaterItem)(e.Item.NamingContainer.NamingContainer)).FindControl("txtStartTime");
                    TextBox txtEndTime = (TextBox)((RepeaterItem)(e.Item.NamingContainer.NamingContainer)).FindControl("txtEndTime");
                    txtTransStartTime.Attributes.Add("onblur", "return CheckTransTimeRang(this,'"
                        + txtStartTime.ClientID + "','" + txtEndTime.ClientID + "')");
                    txtTransEndTime.Attributes.Add("onblur", "return CheckTransTimeRang(this,'"
                        + txtStartTime.ClientID + "','" + txtEndTime.ClientID + "')");
                }
            }
            catch (Exception ex)
            {
                throw (new AspNetException("绑定地面计划信息出现异常，异常原因", ex));
            }
            finally { }
        }

        protected void Repeater2_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Add")
                {
                    List<DJZYSQ_Task_ReakTimeTransfor> list2 = new List<DJZYSQ_Task_ReakTimeTransfor>();
                    DJZYSQ_Task_ReakTimeTransfor rt;
                    Repeater rp = (Repeater)source;
                    foreach (RepeaterItem it in rp.Items)
                    {
                        rt = new DJZYSQ_Task_ReakTimeTransfor();
                        TextBox txtFormatFlag = (TextBox)it.FindControl("txtFormatFlag");
                        TextBox txtInfoFlowFlag = (TextBox)it.FindControl("txtInfoFlowFlag");
                        TextBox txtTransStartTime = (TextBox)it.FindControl("txtTransStartTime");
                        TextBox txtTransEndTime = (TextBox)it.FindControl("txtTransEndTime");
                        TextBox txtTransSpeedRate = (TextBox)it.FindControl("txtTransSpeedRate");

                        rt.GBZ = txtFormatFlag.Text;
                        rt.XBZ = txtInfoFlowFlag.Text;
                        rt.RTs = txtTransStartTime.Text;
                        rt.RTe = txtTransEndTime.Text;
                        rt.SL = txtTransSpeedRate.Text;
                        list2.Add(rt);
                    }
                    rt = new DJZYSQ_Task_ReakTimeTransfor();
                    rt.GBZ = "";
                    rt.XBZ = "";
                    rt.RTs = "";
                    rt.RTe = "";
                    rt.SL = "";
                    list2.Add(rt);
                    rp.DataSource = list2;
                    rp.DataBind();
                }
                if (e.CommandName == "Del")
                {
                    List<DJZYSQ_Task_ReakTimeTransfor> list2 = new List<DJZYSQ_Task_ReakTimeTransfor>();
                    DJZYSQ_Task_ReakTimeTransfor rt;
                    Repeater rp = (Repeater)source;
                    if (rp.Items.Count <= 1)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "del", "<script type='text/javascript'>showMsg('最后一条，无法删除!');</script>");
                    }
                    else
                    {
                        foreach (RepeaterItem it in rp.Items)
                        {
                            if (e.Item.ItemIndex != it.ItemIndex)
                            {
                                rt = new DJZYSQ_Task_ReakTimeTransfor();
                                TextBox txtFormatFlag = (TextBox)it.FindControl("txtFormatFlag");
                                TextBox txtInfoFlowFlag = (TextBox)it.FindControl("txtInfoFlowFlag");
                                TextBox txtTransStartTime = (TextBox)it.FindControl("txtTransStartTime");
                                TextBox txtTransEndTime = (TextBox)it.FindControl("txtTransEndTime");
                                TextBox txtTransSpeedRate = (TextBox)it.FindControl("txtTransSpeedRate");

                                rt.GBZ = txtFormatFlag.Text;
                                rt.XBZ = txtInfoFlowFlag.Text;
                                rt.RTs = txtTransStartTime.Text;
                                rt.RTe = txtTransEndTime.Text;
                                rt.SL = txtTransSpeedRate.Text;
                                list2.Add(rt);
                            }
                        }
                        rp.DataSource = list2;
                        rp.DataBind();
                    }
                }//
            }
            catch (Exception ex)
            {
                throw (new AspNetException("绑定地面计划信息出现异常，异常原因", ex));
            }
            finally { }
        }

        protected void Repeater3_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Add")
                {
                    List<DJZYSQ_Task_AfterFeedBack> list2 = new List<DJZYSQ_Task_AfterFeedBack>();
                    DJZYSQ_Task_AfterFeedBack rt;
                    Repeater rp = (Repeater)source;
                    foreach (RepeaterItem it in rp.Items)
                    {
                        rt = new DJZYSQ_Task_AfterFeedBack();
                        TextBox txtFormatFlag = (TextBox)it.FindControl("FormatFlag");
                        TextBox txtInfoFlowFlag = (TextBox)it.FindControl("InfoFlowFlag");
                        TextBox txtDataStartTime = (TextBox)it.FindControl("DataStartTime");
                        TextBox txtDataEndTime = (TextBox)it.FindControl("DataEndTime");
                        TextBox txtTransStartTime = (TextBox)it.FindControl("TransStartTime");
                        TextBox txtTransSpeedRate = (TextBox)it.FindControl("TransSpeedRate");

                        rt.GBZ = txtFormatFlag.Text;
                        rt.XBZ = txtInfoFlowFlag.Text;
                        rt.Ts = txtDataStartTime.Text;
                        rt.Te = txtDataEndTime.Text;
                        rt.RTs = txtTransStartTime.Text;
                        rt.SL = txtTransSpeedRate.Text;
                        list2.Add(rt);
                    }
                    rt = new DJZYSQ_Task_AfterFeedBack();
                    rt.GBZ = "";
                    rt.XBZ = "";
                    rt.Ts = "";
                    rt.Te = "";
                    rt.RTs = "";
                    rt.SL = "";
                    list2.Add(rt);
                    rp.DataSource = list2;
                    rp.DataBind();
                }
                if (e.CommandName == "Del")
                {
                    List<DJZYSQ_Task_AfterFeedBack> list2 = new List<DJZYSQ_Task_AfterFeedBack>();
                    DJZYSQ_Task_AfterFeedBack rt;
                    Repeater rp = (Repeater)source;
                    if (rp.Items.Count <= 1)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "del", "<script type='text/javascript'>showMsg('最后一条，无法删除!');</script>");
                    }
                    else
                    {
                        foreach (RepeaterItem it in rp.Items)
                        {
                            if (e.Item.ItemIndex != it.ItemIndex)
                            {
                                rt = new DJZYSQ_Task_AfterFeedBack();
                                TextBox txtFormatFlag = (TextBox)it.FindControl("FormatFlag");
                                TextBox txtInfoFlowFlag = (TextBox)it.FindControl("InfoFlowFlag");
                                TextBox txtDataStartTime = (TextBox)it.FindControl("DataStartTime");
                                TextBox txtDataEndTime = (TextBox)it.FindControl("DataEndTime");
                                TextBox txtTransStartTime = (TextBox)it.FindControl("TransStartTime");
                                TextBox txtTransSpeedRate = (TextBox)it.FindControl("TransSpeedRate");

                                rt.GBZ = txtFormatFlag.Text;
                                rt.XBZ = txtInfoFlowFlag.Text;
                                rt.Ts = txtDataStartTime.Text;
                                rt.Te = txtDataEndTime.Text;
                                rt.RTs = txtTransStartTime.Text;
                                rt.SL = txtTransSpeedRate.Text;
                                list2.Add(rt);
                            }
                        }
                        rp.DataSource = list2;
                        rp.DataBind();
                    }
                }//
            }
            catch (Exception ex)
            {
                throw (new AspNetException("绑定地面计划信息出现异常，异常原因", ex));
            }
            finally { }
        }

        protected void RepeaterGZDP_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Add")
                {
                    List<DJZYSQ_Task_GZDP> list2 = new List<DJZYSQ_Task_GZDP>();
                    DJZYSQ_Task_GZDP rt;
                    Repeater rp = (Repeater)source;
                    foreach (RepeaterItem it in rp.Items)
                    {
                        rt = new DJZYSQ_Task_GZDP();
                        TextBox txtFXH = (TextBox)it.FindControl("txtFXH");
                        DropDownList ddlPDXZ = (DropDownList)it.FindControl("ddlPDXZ");
                        TextBox txtDPXZ = (TextBox)it.FindControl("txtDPXZ");

                        rt.FXH = txtFXH.Text;
                        rt.PDXZ = ddlPDXZ.Text;
                        rt.DPXZ = txtDPXZ.Text;
                        list2.Add(rt);
                    }
                    rt = new DJZYSQ_Task_GZDP();
                    rt.FXH = "";
                    rt.PDXZ = "";
                    rt.DPXZ = "";
                    list2.Add(rt);
                    rp.DataSource = list2;
                    rp.DataBind();
                }
                if (e.CommandName == "Del")
                {
                    List<DJZYSQ_Task_GZDP> list2 = new List<DJZYSQ_Task_GZDP>();
                    DJZYSQ_Task_GZDP rt;
                    Repeater rp = (Repeater)source;
                    if (rp.Items.Count <= 1)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "del", "<script type='text/javascript'>showMsg('最后一条，无法删除!');</script>");
                    }
                    else
                    {
                        foreach (RepeaterItem it in rp.Items)
                        {
                            if (e.Item.ItemIndex != it.ItemIndex)
                            {
                                rt = new DJZYSQ_Task_GZDP();
                                TextBox txtFXH = (TextBox)it.FindControl("txtFXH");
                                DropDownList ddlPDXZ = (DropDownList)it.FindControl("ddlPDXZ");
                                TextBox txtDPXZ = (TextBox)it.FindControl("txtDPXZ");

                                rt.FXH = txtFXH.Text;
                                rt.PDXZ = ddlPDXZ.Text;
                                rt.DPXZ = txtDPXZ.Text;
                                list2.Add(rt);
                            }
                        }
                        rp.DataSource = list2;
                        rp.DataBind();
                    }
                }//
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
                    DropDownList ddlPDXZ = (DropDownList)e.Item.FindControl("ddlPDXZ") as DropDownList;
                    ddlPDXZ.DataSource = PlanParameters.ReadParameters("DJZYSQPDXZ");
                    ddlPDXZ.DataTextField = "Text";
                    ddlPDXZ.DataValueField = "Value";
                    ddlPDXZ.DataBind();

                    DJZYSQ_Task_GZDP view = (DJZYSQ_Task_GZDP)e.Item.DataItem;
                    ddlPDXZ.SelectedValue = view.PDXZ;

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
            this.PagePermission = "OMPLAN_Plan.Edit";
            this.ShortTitle = "编辑计划";
            base.OnPageLoaded();
            this.AddJavaScriptInclude("scripts/pages/PlanManage/DMJHEdit.aspx.js");
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                Task oTask = new Task();
                string taskNO = string.Empty;
                string satID = string.Empty;
                isTempJH = GetIsTempJHValue();
                DJZYSQ obj = new DJZYSQ();

                txtSCID.Text = oTask.GetSCID(ucOutTask1.SelectedValue);
                oTask.GetTaskNoSatID(ucOutTask1.SelectedValue, out taskNO, out satID);
                if (txtSequence.Text != "")
                    obj.SNO = txtSequence.Text;
                else
                    obj.SNO = (new Sequence()).GetDJZYSQSequnce().ToString();
                obj.SJ = DateTime.Now.ToString("yyyyMMddHHmmss");
                obj.SCID = txtSCID.Text;
                // obj.TaskCount = txtTaskCount.Text;
                obj.DMJHTasks = new List<DJZYSQ_Task>();

                DJZYSQ_Task rt;
                DJZYSQ_Task_GZDP dp;
                DJZYSQ_Task_ReakTimeTransfor rtt;
                DJZYSQ_Task_AfterFeedBack afb;

                foreach (RepeaterItem it in Repeater1.Items)
                {
                    #region task
                    rt = new DJZYSQ_Task();
                    TextBox txtSXH = (TextBox)it.FindControl("txtSXH");
                    DropDownList ddlSXZ = (DropDownList)it.FindControl("ddlSXZ");
                    TextBox txtMLB = (TextBox)it.FindControl("txtMLB");
                    DropDownList ddlFS = (DropDownList)it.FindControl("ddlFS");
                    DropDownList ddlGZDY = (DropDownList)it.FindControl("ddlGZDY");
                    DropDownList ddlSBDH = (DropDownList)it.FindControl("ddlSBDH");
                    TextBox txtSBDH = (TextBox)it.FindControl("txtSBDH");
                    TextBox txtQC = (TextBox)it.FindControl("txtQC");
                    TextBox txtQB = (TextBox)it.FindControl("txtQB");
                    DropDownList ddlSHJ = (DropDownList)it.FindControl("ddlSHJ");
                    //TextBox txtFNUM = (TextBox)it.FindControl("txtFNUM");
                    //TextBox txtFXH = (TextBox)it.FindControl("txtFXH");
                    //DropDownList ddlPDXZ = (DropDownList)it.FindControl("ddlPDXZ");
                    //TextBox txtDPXZ = (TextBox)it.FindControl("txtDPXZ");
                    TextBox txtTNUM = (TextBox)it.FindControl("txtTNUM");
                    TextBox txtPreStartTime = (TextBox)it.FindControl("txtPreStartTime");
                    TextBox txtStartTime = (TextBox)it.FindControl("txtStartTime");
                    TextBox txtTrackStartTime = (TextBox)it.FindControl("txtTrackStartTime");
                    TextBox txtWaveOnStartTime = (TextBox)it.FindControl("txtWaveOnStartTime");
                    TextBox txtWaveOffStartTime = (TextBox)it.FindControl("txtWaveOffStartTime");
                    TextBox txtTrackEndTime = (TextBox)it.FindControl("txtTrackEndTime");
                    TextBox txtEndTime = (TextBox)it.FindControl("txtEndTime");

                    rt.SXH = (obj.DMJHTasks.Count() + 1).ToString();//txtSXH.Text;
                    rt.SXZ = ddlSXZ.SelectedValue;
                    rt.MLB = txtMLB.Text;
                    rt.FS = ddlFS.SelectedValue;
                    rt.GZDY = ddlGZDY.SelectedValue;
                    //rt.SBDH = ddlSBDH.SelectedValue;
                    rt.SBDH = txtSBDH.Text;
                    rt.QC = txtQC.Text;
                    rt.QB = txtQB.Text;
                    rt.SHJ = ddlSHJ.SelectedValue;
                    //rt.FNUM = txtFNUM.Text;
                    //rt.FXH = txtFXH.Text;
                    //rt.PDXZ = ddlPDXZ.SelectedValue;
                    //rt.DPXZ = txtDPXZ.Text;
                    rt.TNUM = txtTNUM.Text;
                    rt.ZHB = txtPreStartTime.Text;
                    rt.RK = txtStartTime.Text;
                    if (rt.RK.Substring(0, 1).ToUpper() == "F")
                    {
                        ltMessage.Text = "开始时间不合法";
                        return;
                    }
                    rt.GZK = txtTrackStartTime.Text;
                    if (rt.GZK.Substring(0, 1).ToUpper() == "F")
                    {
                        ltMessage.Text = "跟踪开始时间不合法";
                        return;
                    }
                    rt.KSHX = txtWaveOnStartTime.Text;
                    rt.GSHX = txtWaveOffStartTime.Text;
                    rt.GZJ = txtTrackEndTime.Text;
                    if (rt.GZJ.Substring(0, 1).ToUpper() == "F")
                    {
                        ltMessage.Text = "跟踪结束时间不合法";
                        return;
                    }
                    rt.JS = txtEndTime.Text;
                    if (rt.JS.Substring(0, 1).ToUpper() == "F")
                    {
                        ltMessage.Text = "结束时间不合法";
                        return;
                    }
                    rt.GZDPs = new List<DJZYSQ_Task_GZDP>();
                    rt.ReakTimeTransfors = new List<DJZYSQ_Task_ReakTimeTransfor>();
                    rt.AfterFeedBacks = new List<DJZYSQ_Task_AfterFeedBack>();
                    //obj.DMJHTasks.Add(rt);
                    #endregion
                    #region GZDP
                    Repeater rpg = it.FindControl("rpGZDP") as Repeater;
                    foreach (RepeaterItem its in rpg.Items)
                    {

                        dp = new DJZYSQ_Task_GZDP();
                        TextBox txtFXH = (TextBox)its.FindControl("txtFXH");
                        DropDownList ddlPDXZ = (DropDownList)its.FindControl("ddlPDXZ");
                        TextBox txtDPXZ = (TextBox)its.FindControl("txtDPXZ");

                        dp.FXH = txtFXH.Text;
                        dp.PDXZ = ddlPDXZ.SelectedValue;
                        dp.DPXZ = txtDPXZ.Text;
                        rt.GZDPs.Add(dp);

                    }
                    #endregion
                    #region ReakTimeTransfor
                    Repeater rps = it.FindControl("rpReakTimeTransfor") as Repeater;
                    foreach (RepeaterItem its in rps.Items)
                    {

                        rtt = new DJZYSQ_Task_ReakTimeTransfor();
                        TextBox txtFormatFlag = (TextBox)its.FindControl("txtFormatFlag");
                        TextBox txtInfoFlowFlag = (TextBox)its.FindControl("txtInfoFlowFlag");
                        TextBox txtTransStartTime = (TextBox)its.FindControl("txtTransStartTime");
                        TextBox txtTransEndTime = (TextBox)its.FindControl("txtTransEndTime");
                        TextBox txtTransSpeedRate = (TextBox)its.FindControl("txtTransSpeedRate");

                        rtt.GBZ = txtFormatFlag.Text;
                        rtt.XBZ = txtInfoFlowFlag.Text;
                        rtt.RTs = txtTransStartTime.Text;
                        rtt.RTe = txtTransEndTime.Text;
                        rtt.SL = txtTransSpeedRate.Text;
                        rt.ReakTimeTransfors.Add(rtt);

                    }

                    #endregion
                    #region AfterFeedBack
                    Repeater rpa = it.FindControl("rpAfterFeedBack") as Repeater;
                    foreach (RepeaterItem its in rpa.Items)
                    {

                        afb = new DJZYSQ_Task_AfterFeedBack();
                        TextBox txtFormatFlag = (TextBox)its.FindControl("FormatFlag");
                        TextBox txtInfoFlowFlag = (TextBox)its.FindControl("InfoFlowFlag");
                        TextBox txtDataStartTime = (TextBox)its.FindControl("DataStartTime");
                        TextBox txtDataEndTime = (TextBox)its.FindControl("DataEndTime");
                        TextBox txtTransStartTime = (TextBox)its.FindControl("TransStartTime");
                        TextBox txtTransSpeedRate = (TextBox)its.FindControl("TransSpeedRate");

                        afb.GBZ = txtFormatFlag.Text;
                        afb.XBZ = txtInfoFlowFlag.Text;
                        afb.Ts = txtDataStartTime.Text;
                        afb.Te = txtDataEndTime.Text;
                        afb.RTs = txtTransStartTime.Text;
                        afb.SL = txtTransSpeedRate.Text;
                        rt.AfterFeedBacks.Add(afb);

                    }
                    #endregion

                    rt.FNUM = rt.GZDPs.Count.ToString();
                    obj.DMJHTasks.Add(rt);
                }
                obj.SNUM = obj.DMJHTasks.Count.ToString();  //申请数量
                obj.TaskID = taskNO;    //任务ID
                obj.SatID = satID;    //卫星ID

                PlanFileCreator creater = new PlanFileCreator(isTempJH);
                if (hfStatus.Value == "new")
                {
                    string filepath = creater.CreateDMJHFile(obj, 0);

                    DataAccessLayer.PlanManage.JH jh = new DataAccessLayer.PlanManage.JH(isTempJH)
                    {
                        TaskID = obj.TaskID,
                        PlanType = "DJZYSQ",
                        PlanID = Convert.ToInt32(obj.SNO),
                        StartTime = Convert.ToDateTime(txtPlanStartTime.Text.Trim()),
                        EndTime = Convert.ToDateTime(txtPlanEndTime.Text.Trim()),
                        SRCType = 0,
                        FileIndex = filepath,
                        SatID = obj.SatID,
                        Reserve = txtNote.Text
                    };
                    var result = jh.Add();
                }
                else
                {
                    creater.FilePath = obj.FileIndex;
                    //当任务和卫星更新时，需要更新文件名称
                    if (hfTaskID.Value != ucOutTask1.SelectedValue)
                    {
                        string filepath = creater.CreateDMJHFile(obj, 1);

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
                        //更新隐藏域的任务ID和卫星ID
                        hfTaskID.Value = jh.TaskID;
                        hfSatID.Value = jh.SatID;
                    }
                    else
                    {
                        creater.FilePath = HfFileIndex.Value;
                        creater.CreateDMJHFile(obj, 1);
                        if (!isTempJH)
                        {
                            DataAccessLayer.PlanManage.JH jh = new DataAccessLayer.PlanManage.JH(isTempJH)
                            {
                                Id = Convert.ToInt32(HfID.Value),
                                SENDSTATUS = 0,
                                USESTATUS = 0
                            };
                            var result = jh.UpdateStatus();
                        }
                    }
                }

                ltMessage.Text = "计划保存成功";

                txtSequence.Text = obj.SNO;
                //txtDatetime.Text = obj.SJ;
                txtTaskCount.Text = obj.SNUM;
                //ClientScript.RegisterStartupScript(this.GetType(), "OK", "<script type='text/javascript'>showMsg('计划保存成功');</script>");
                //
            }
            catch (Exception ex)
            {
                throw (new AspNetException("保存地面计划信息出现异常，异常原因", ex));
            }
            finally { }
        }

        protected void btnSaveTo_Click(object sender, EventArgs e)
        {
            try
            {
                Task oTask = new Task();
                string taskNO = string.Empty;
                string satID = string.Empty;

                txtSCID.Text = oTask.GetSCID(ucOutTask1.SelectedValue);
                oTask.GetTaskNoSatID(ucOutTask1.SelectedValue, out taskNO, out satID);

                isTempJH = GetIsTempJHValue();
                DJZYSQ obj = new DJZYSQ();
                //obj.SNO = txtSequence.Text;
                obj.SNO = (new Sequence()).GetDJZYSQSequnce().ToString();
                obj.SJ = DateTime.Now.ToString("yyyyMMddHHmmss");
                obj.SCID = txtSCID.Text;
                // obj.TaskCount = txtTaskCount.Text;
                obj.DMJHTasks = new List<DJZYSQ_Task>();

                DJZYSQ_Task rt;
                DJZYSQ_Task_GZDP dp;
                DJZYSQ_Task_ReakTimeTransfor rtt;
                DJZYSQ_Task_AfterFeedBack afb;

                foreach (RepeaterItem it in Repeater1.Items)
                {
                    #region task
                    rt = new DJZYSQ_Task();
                    TextBox txtSXH = (TextBox)it.FindControl("txtSXH");
                    DropDownList ddlSXZ = (DropDownList)it.FindControl("ddlSXZ");
                    TextBox txtMLB = (TextBox)it.FindControl("txtMLB");
                    DropDownList ddlFS = (DropDownList)it.FindControl("ddlFS");
                    DropDownList ddlGZDY = (DropDownList)it.FindControl("ddlGZDY");
                    DropDownList ddlSBDH = (DropDownList)it.FindControl("ddlSBDH");
                    TextBox txtSBDH = (TextBox)it.FindControl("txtSBDH");
                    TextBox txtQC = (TextBox)it.FindControl("txtQC");
                    TextBox txtQB = (TextBox)it.FindControl("txtQB");
                    DropDownList ddlSHJ = (DropDownList)it.FindControl("ddlSHJ");
                    //TextBox txtFNUM = (TextBox)it.FindControl("txtFNUM");
                    //TextBox txtFXH = (TextBox)it.FindControl("txtFXH");
                    //DropDownList ddlPDXZ = (DropDownList)it.FindControl("ddlPDXZ");
                    //TextBox txtDPXZ = (TextBox)it.FindControl("txtDPXZ");
                    TextBox txtTNUM = (TextBox)it.FindControl("txtTNUM");
                    TextBox txtPreStartTime = (TextBox)it.FindControl("txtPreStartTime");
                    TextBox txtStartTime = (TextBox)it.FindControl("txtStartTime");
                    TextBox txtTrackStartTime = (TextBox)it.FindControl("txtTrackStartTime");
                    TextBox txtWaveOnStartTime = (TextBox)it.FindControl("txtWaveOnStartTime");
                    TextBox txtWaveOffStartTime = (TextBox)it.FindControl("txtWaveOffStartTime");
                    TextBox txtTrackEndTime = (TextBox)it.FindControl("txtTrackEndTime");
                    TextBox txtEndTime = (TextBox)it.FindControl("txtEndTime");

                    rt.SXH = (obj.DMJHTasks.Count() + 1).ToString();//txtSXH.Text;
                    rt.SXZ = ddlSXZ.SelectedValue;
                    rt.MLB = txtMLB.Text;
                    rt.FS = ddlFS.SelectedValue;
                    rt.GZDY = ddlGZDY.SelectedValue;
                    //rt.SBDH = ddlSBDH.SelectedValue;
                    rt.SBDH = txtSBDH.Text;
                    rt.QC = txtQC.Text;
                    rt.QB = txtQB.Text;
                    rt.SHJ = ddlSHJ.SelectedValue;
                    //rt.FNUM = txtFNUM.Text;
                    //rt.FXH = txtFXH.Text;
                    //rt.PDXZ = ddlPDXZ.SelectedValue;
                    //rt.DPXZ = txtDPXZ.Text;
                    rt.TNUM = txtTNUM.Text;
                    rt.ZHB = txtPreStartTime.Text;
                    rt.RK = txtStartTime.Text;
                    if (rt.RK.Substring(0, 1).ToUpper() == "F")
                    {
                        ltMessage.Text = "开始时间不合法";
                        return;
                    }
                    rt.GZK = txtTrackStartTime.Text;
                    if (rt.GZK.Substring(0, 1).ToUpper() == "F")
                    {
                        ltMessage.Text = "跟踪开始时间不合法";
                        return;
                    }
                    rt.KSHX = txtWaveOnStartTime.Text;
                    rt.GSHX = txtWaveOffStartTime.Text;
                    rt.GZJ = txtTrackEndTime.Text;
                    if (rt.GZJ.Substring(0, 1).ToUpper() == "F")
                    {
                        ltMessage.Text = "跟踪结束时间不合法";
                        return;
                    }
                    rt.JS = txtEndTime.Text;
                    if (rt.JS.Substring(0, 1).ToUpper() == "F")
                    {
                        ltMessage.Text = "结束时间不合法";
                        return;
                    }
                    rt.ReakTimeTransfors = new List<DJZYSQ_Task_ReakTimeTransfor>();
                    rt.AfterFeedBacks = new List<DJZYSQ_Task_AfterFeedBack>();
                    //obj.DMJHTasks.Add(rt);
                    #endregion
                    #region GZDP
                    Repeater rpg = it.FindControl("rpGZDP") as Repeater;
                    if (rt.GZDPs == null)
                        rt.GZDPs = new List<DJZYSQ_Task_GZDP>();
                    foreach (RepeaterItem its in rpg.Items)
                    {

                        dp = new DJZYSQ_Task_GZDP();
                        TextBox txtFXH = (TextBox)its.FindControl("txtFXH");
                        DropDownList ddlPDXZ = (DropDownList)its.FindControl("ddlPDXZ");
                        TextBox txtDPXZ = (TextBox)its.FindControl("txtDPXZ");

                        dp.FXH = txtFXH.Text;
                        dp.PDXZ = ddlPDXZ.SelectedValue;
                        dp.DPXZ = txtDPXZ.Text;
                        rt.GZDPs.Add(dp);

                    }
                    #endregion
                    #region ReakTimeTransfor
                    Repeater rps = it.FindControl("rpReakTimeTransfor") as Repeater;
                    if (rt.ReakTimeTransfors == null)
                        rt.ReakTimeTransfors = new List<DJZYSQ_Task_ReakTimeTransfor>();
                    foreach (RepeaterItem its in rps.Items)
                    {

                        rtt = new DJZYSQ_Task_ReakTimeTransfor();
                        TextBox txtFormatFlag = (TextBox)its.FindControl("txtFormatFlag");
                        TextBox txtInfoFlowFlag = (TextBox)its.FindControl("txtInfoFlowFlag");
                        TextBox txtTransStartTime = (TextBox)its.FindControl("txtTransStartTime");
                        TextBox txtTransEndTime = (TextBox)its.FindControl("txtTransEndTime");
                        TextBox txtTransSpeedRate = (TextBox)its.FindControl("txtTransSpeedRate");

                        rtt.GBZ = txtFormatFlag.Text;
                        rtt.XBZ = txtInfoFlowFlag.Text;
                        rtt.RTs = txtTransStartTime.Text;
                        rtt.RTe = txtTransEndTime.Text;
                        rtt.SL = txtTransSpeedRate.Text;
                        rt.ReakTimeTransfors.Add(rtt);

                    }

                    #endregion
                    #region AfterFeedBack
                    Repeater rpa = it.FindControl("rpAfterFeedBack") as Repeater;
                    if (rt.AfterFeedBacks == null)
                        rt.AfterFeedBacks = new List<DJZYSQ_Task_AfterFeedBack>();
                    foreach (RepeaterItem its in rpa.Items)
                    {

                        afb = new DJZYSQ_Task_AfterFeedBack();
                        TextBox txtFormatFlag = (TextBox)its.FindControl("FormatFlag");
                        TextBox txtInfoFlowFlag = (TextBox)its.FindControl("InfoFlowFlag");
                        TextBox txtDataStartTime = (TextBox)its.FindControl("DataStartTime");
                        TextBox txtDataEndTime = (TextBox)its.FindControl("DataEndTime");
                        TextBox txtTransStartTime = (TextBox)its.FindControl("TransStartTime");
                        TextBox txtTransSpeedRate = (TextBox)its.FindControl("TransSpeedRate");

                        afb.GBZ = txtFormatFlag.Text;
                        afb.XBZ = txtInfoFlowFlag.Text;
                        afb.Ts = txtDataStartTime.Text;
                        afb.Te = txtDataEndTime.Text;
                        afb.RTs = txtTransStartTime.Text;
                        afb.SL = txtTransSpeedRate.Text;
                        rt.AfterFeedBacks.Add(afb);

                    }
                    #endregion

                    rt.FNUM = rt.GZDPs.Count.ToString();
                    obj.DMJHTasks.Add(rt);
                }
                obj.SNUM = obj.DMJHTasks.Count.ToString();  //申请数量


                PlanFileCreator creater = new PlanFileCreator(isTempJH);

                obj.TaskID = taskNO;    //任务ID
                obj.SatID = satID;    //卫星ID

                //检查文件是否已经存在
                if (creater.TestDMJHFileName(obj))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "File", "<script type='text/javascript'>showMsg('存在同名文件，请一分钟后重试');</script>");
                    return;
                }
                string filepath = creater.CreateDMJHFile(obj, 0);

                DataAccessLayer.PlanManage.JH jh = new DataAccessLayer.PlanManage.JH(isTempJH)
                {
                    TaskID = obj.TaskID,
                    PlanType = "DJZYSQ",
                    PlanID = Convert.ToInt32(obj.SNO),
                    StartTime = Convert.ToDateTime(txtPlanStartTime.Text.Trim()),
                    EndTime = Convert.ToDateTime(txtPlanEndTime.Text.Trim()),
                    SRCType = 0,
                    FileIndex = filepath,
                    SatID = obj.SatID,
                    Reserve = txtNote.Text
                };
                var result = jh.Add();

                ltMessage.Text = "计划保存成功";

                txtSequence.Text = obj.SNO;
                //txtDatetime.Text = obj.SJ;
                txtTaskCount.Text = obj.SNUM;
                //ClientScript.RegisterStartupScript(this.GetType(), "OK", "<script type='text/javascript'>showMsg('计划保存成功');</script>");
                //
            }
            catch (Exception ex)
            {
                throw (new AspNetException("另存地面计划信息出现异常，异常原因", ex));
            }
            finally { }
        }

        protected void txtGetPlanInfo_Click(object sender, EventArgs e)
        {
            BindGridView();
            ClientScript.RegisterStartupScript(this.GetType(), "hide", "<script type='text/javascript'>showSBJHForm();</script>");
        }

        //绑定列表
        void BindGridView()
        {

            DateTime startDate = new DateTime();
            DateTime endDate = new DateTime();
            startDate = new DateTime(1900, 1, 1);
            endDate = DateTime.Now.AddDays(1);
            string planType = "DJZYJH";

            List<JH> listDatas = (new JH()).GetJHList(planType, startDate, endDate, DateTime.MinValue, DateTime.MinValue);
            cpPager.DataSource = listDatas;
            cpPager.PageSize = 7;
            cpPager.BindToControl = rpDatas;
            rpDatas.DataSource = cpPager.DataSourcePaged;
            rpDatas.DataBind();
        }

        protected void btnSBJH_Click(object sender, EventArgs e)
        {
            hfSBJHID.Value = "-1";
            btnSBJH.Text = "";
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

        protected void btnHidden_Click(object sender, EventArgs e)
        {
            try
            {
                #region 从测控资源使用计划获取信息
                DJZYSQ objDMJH = new DJZYSQ
                {
                    DMJHTasks = new List<DJZYSQ_Task> 
                        {
                            new DJZYSQ_Task
                            {
                                ReakTimeTransfors = new List<DJZYSQ_Task_ReakTimeTransfor>{new DJZYSQ_Task_ReakTimeTransfor()},
                                AfterFeedBacks = new List<DJZYSQ_Task_AfterFeedBack>{new DJZYSQ_Task_AfterFeedBack()}
                            }
                        }
                };
                List<JH> jh = (new JH()).SelectByIDS(hfSBJHID.Value);
                objDMJH.TaskID = jh[0].TaskID;
                string[] strTemp = jh[0].FileIndex.Split('_');
                if (strTemp.Length >= 2)
                {
                    objDMJH.SatID = strTemp[strTemp.Length - 2];
                }
                objDMJH.DMJHTasks.Clear();

                DJZYSQ_Task task;
                DJZYSQ_Task_GZDP gzdp;
                DJZYSQ_Task_ReakTimeTransfor rt = new DJZYSQ_Task_ReakTimeTransfor();
                DJZYSQ_Task_AfterFeedBack afb = new DJZYSQ_Task_AfterFeedBack();
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(jh[0].FileIndex);
                XmlNode root = xmlDoc.SelectSingleNode("测控资源使用计划/编号");
                objDMJH.SNO = root.InnerText;
                root = xmlDoc.SelectSingleNode("测控资源使用计划/时间");
                objDMJH.SJ = root.InnerText;

                root = xmlDoc.SelectSingleNode("测控资源使用计划/任务个数");
                objDMJH.SNUM = root.InnerText;

                root = xmlDoc.SelectSingleNode("测控资源使用计划/计划");
                foreach (XmlNode n in root.ChildNodes)
                {
                    if (n.Name == "计划内容")
                    {
                        task = new DJZYSQ_Task();
                        task.SXH = n["计划序号"].InnerText;
                        task.SXZ = n["计划性质"].InnerText;
                        task.MLB = n["任务类别"].InnerText;
                        task.FS = n["工作方式"].InnerText;
                        task.GZDY = n["工作单元"].InnerText;
                        task.SBDH = n["设备代号"].InnerText;
                        task.QC = n["圈次"].InnerText;
                        task.QB = n["圈标"].InnerText;
                        task.SHJ = n["测控事件类型"].InnerText;
                        task.FNUM = n["工作点频数量"].InnerText;
                        task.TNUM = n["同时支持目标数"].InnerText;
                        task.ZHB = n["任务准备开始时间"].InnerText;
                        task.RK = n["任务开始时间"].InnerText;
                        task.GZK = n["跟踪开始时间"].InnerText;
                        task.KSHX = n["开上行载波时间"].InnerText;
                        task.GSHX = n["关上行载波时间"].InnerText;
                        task.GZJ = n["跟踪结束时间"].InnerText;
                        task.JS = n["任务结束时间"].InnerText;

                        task.GZDPs = new List<DJZYSQ_Task_GZDP>();
                        task.ReakTimeTransfors = new List<DJZYSQ_Task_ReakTimeTransfor>();
                        task.AfterFeedBacks = new List<DJZYSQ_Task_AfterFeedBack>();
                        task.ReakTimeTransfors.Add(rt);
                        task.AfterFeedBacks.Add(afb);

                        foreach (XmlNode nd in n["工作点频"])
                        {
                            if (nd.Name == "工作点频内容")
                            {
                                gzdp = new DJZYSQ_Task_GZDP();
                                gzdp.FXH = nd["点频序号"].InnerText;
                                gzdp.PDXZ = nd["频段选择"].InnerText;
                                gzdp.DPXZ = nd["点频选择"].InnerText;
                                task.GZDPs.Add(gzdp);
                            }
                        }
                        objDMJH.DMJHTasks.Add(task);
                    }
                }
                #endregion

                #region BindtoPage
                txtSequence.Text = objDMJH.SNO;
                //txtDatetime.Text = objDMJH.SJ;
                txtSCID.Text = objDMJH.SCID;
                txtTaskCount.Text = objDMJH.SNUM;

                Repeater1.DataSource = objDMJH.DMJHTasks;
                Repeater1.DataBind();
                #endregion
            }
            catch (Exception ex)
            {
                throw (new AspNetException("从测控资源使用计划获取信息出现异常，异常原因", ex));
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
                    hfSBJHID.Value = "-1";
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
                Task oTask = new Task();
                string taskNO = string.Empty;
                string satID = string.Empty;

                txtSCID.Text = oTask.GetSCID(ucOutTask1.SelectedValue);
                oTask.GetTaskNoSatID(ucOutTask1.SelectedValue, out taskNO, out satID);

                DJZYSQ obj = new DJZYSQ();
                //obj.SNO = txtSequence.Text;
                obj.SNO = (new Sequence()).GetDJZYSQSequnce().ToString();
                obj.SJ = DateTime.Now.ToString("yyyyMMddHHmmss");
                obj.SCID = txtSCID.Text;
                // obj.TaskCount = txtTaskCount.Text;
                obj.DMJHTasks = new List<DJZYSQ_Task>();

                DJZYSQ_Task rt;
                DJZYSQ_Task_GZDP dp;
                DJZYSQ_Task_ReakTimeTransfor rtt;
                DJZYSQ_Task_AfterFeedBack afb;

                foreach (RepeaterItem it in Repeater1.Items)
                {
                    #region task
                    rt = new DJZYSQ_Task();
                    TextBox txtSXH = (TextBox)it.FindControl("txtSXH");
                    DropDownList ddlSXZ = (DropDownList)it.FindControl("ddlSXZ");
                    TextBox txtMLB = (TextBox)it.FindControl("txtMLB");
                    DropDownList ddlFS = (DropDownList)it.FindControl("ddlFS");
                    DropDownList ddlGZDY = (DropDownList)it.FindControl("ddlGZDY");
                    DropDownList ddlSBDH = (DropDownList)it.FindControl("ddlSBDH");
                    TextBox txtSBDH = (TextBox)it.FindControl("txtSBDH");
                    TextBox txtQC = (TextBox)it.FindControl("txtQC");
                    TextBox txtQB = (TextBox)it.FindControl("txtQB");
                    DropDownList ddlSHJ = (DropDownList)it.FindControl("ddlSHJ");
                    //TextBox txtFNUM = (TextBox)it.FindControl("txtFNUM");
                    //TextBox txtFXH = (TextBox)it.FindControl("txtFXH");
                    //DropDownList ddlPDXZ = (DropDownList)it.FindControl("ddlPDXZ");
                    //TextBox txtDPXZ = (TextBox)it.FindControl("txtDPXZ");
                    TextBox txtTNUM = (TextBox)it.FindControl("txtTNUM");
                    TextBox txtPreStartTime = (TextBox)it.FindControl("txtPreStartTime");
                    TextBox txtStartTime = (TextBox)it.FindControl("txtStartTime");
                    TextBox txtTrackStartTime = (TextBox)it.FindControl("txtTrackStartTime");
                    TextBox txtWaveOnStartTime = (TextBox)it.FindControl("txtWaveOnStartTime");
                    TextBox txtWaveOffStartTime = (TextBox)it.FindControl("txtWaveOffStartTime");
                    TextBox txtTrackEndTime = (TextBox)it.FindControl("txtTrackEndTime");
                    TextBox txtEndTime = (TextBox)it.FindControl("txtEndTime");

                    rt.SXH = (obj.DMJHTasks.Count() + 1).ToString();//txtSXH.Text;
                    rt.SXZ = ddlSXZ.SelectedValue;
                    rt.MLB = txtMLB.Text;
                    rt.FS = ddlFS.SelectedValue;
                    rt.GZDY = ddlGZDY.SelectedValue;
                    //rt.SBDH = ddlSBDH.SelectedValue;
                    rt.SBDH = txtSBDH.Text;
                    rt.QC = txtQC.Text;
                    rt.QB = txtQB.Text;
                    rt.SHJ = ddlSHJ.SelectedValue;
                    //rt.FNUM = txtFNUM.Text;
                    //rt.FXH = txtFXH.Text;
                    //rt.PDXZ = ddlPDXZ.SelectedValue;
                    //rt.DPXZ = txtDPXZ.Text;
                    rt.TNUM = txtTNUM.Text;
                    rt.ZHB = txtPreStartTime.Text;
                    rt.RK = txtStartTime.Text;
                    rt.GZK = txtTrackStartTime.Text;
                    rt.KSHX = txtWaveOnStartTime.Text;
                    rt.GSHX = txtWaveOffStartTime.Text;
                    rt.GZJ = txtTrackEndTime.Text;
                    rt.JS = txtEndTime.Text;
                    rt.ReakTimeTransfors = new List<DJZYSQ_Task_ReakTimeTransfor>();
                    rt.AfterFeedBacks = new List<DJZYSQ_Task_AfterFeedBack>();
                    //obj.DMJHTasks.Add(rt);
                    #endregion
                    #region GZDP
                    Repeater rpg = it.FindControl("rpGZDP") as Repeater;
                    rt.GZDPs = new List<DJZYSQ_Task_GZDP>();
                    foreach (RepeaterItem its in rpg.Items)
                    {

                        dp = new DJZYSQ_Task_GZDP();
                        TextBox txtFXH = (TextBox)its.FindControl("txtFXH");
                        DropDownList ddlPDXZ = (DropDownList)its.FindControl("ddlPDXZ");
                        TextBox txtDPXZ = (TextBox)its.FindControl("txtDPXZ");

                        dp.FXH = txtFXH.Text;
                        dp.PDXZ = ddlPDXZ.SelectedValue;
                        dp.DPXZ = txtDPXZ.Text;
                        rt.GZDPs.Add(dp);

                    }
                    #endregion
                    #region ReakTimeTransfor
                    Repeater rps = it.FindControl("rpReakTimeTransfor") as Repeater;
                    rt.ReakTimeTransfors = new List<DJZYSQ_Task_ReakTimeTransfor>();
                    foreach (RepeaterItem its in rps.Items)
                    {

                        rtt = new DJZYSQ_Task_ReakTimeTransfor();
                        TextBox txtFormatFlag = (TextBox)its.FindControl("txtFormatFlag");
                        TextBox txtInfoFlowFlag = (TextBox)its.FindControl("txtInfoFlowFlag");
                        TextBox txtTransStartTime = (TextBox)its.FindControl("txtTransStartTime");
                        TextBox txtTransEndTime = (TextBox)its.FindControl("txtTransEndTime");
                        TextBox txtTransSpeedRate = (TextBox)its.FindControl("txtTransSpeedRate");

                        rtt.GBZ = txtFormatFlag.Text;
                        rtt.XBZ = txtInfoFlowFlag.Text;
                        rtt.RTs = txtTransStartTime.Text;
                        rtt.RTe = txtTransEndTime.Text;
                        rtt.SL = txtTransSpeedRate.Text;
                        rt.ReakTimeTransfors.Add(rtt);

                    }

                    #endregion
                    #region AfterFeedBack
                    Repeater rpa = it.FindControl("rpAfterFeedBack") as Repeater;
                    rt.AfterFeedBacks = new List<DJZYSQ_Task_AfterFeedBack>();
                    foreach (RepeaterItem its in rpa.Items)
                    {

                        afb = new DJZYSQ_Task_AfterFeedBack();
                        TextBox txtFormatFlag = (TextBox)its.FindControl("FormatFlag");
                        TextBox txtInfoFlowFlag = (TextBox)its.FindControl("InfoFlowFlag");
                        TextBox txtDataStartTime = (TextBox)its.FindControl("DataStartTime");
                        TextBox txtDataEndTime = (TextBox)its.FindControl("DataEndTime");
                        TextBox txtTransStartTime = (TextBox)its.FindControl("TransStartTime");
                        TextBox txtTransSpeedRate = (TextBox)its.FindControl("TransSpeedRate");

                        afb.GBZ = txtFormatFlag.Text;
                        afb.XBZ = txtInfoFlowFlag.Text;
                        afb.Ts = txtDataStartTime.Text;
                        afb.Te = txtDataEndTime.Text;
                        afb.RTs = txtTransStartTime.Text;
                        afb.SL = txtTransSpeedRate.Text;
                        rt.AfterFeedBacks.Add(afb);

                    }
                    #endregion

                    rt.FNUM = rt.GZDPs.Count.ToString();
                    obj.DMJHTasks.Add(rt);
                }
                obj.SNUM = obj.DMJHTasks.Count.ToString();  //申请数量


                PlanFileCreator creater = new PlanFileCreator();

                obj.TaskID = taskNO;    //任务ID
                obj.SatID = satID;    //卫星ID

                //检查文件是否已经存在
                if (creater.TestDMJHFileName(obj))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "File", "<script type='text/javascript'>showMsg('存在同名文件，请一分钟后重试');</script>");
                    return;
                }
                string filepath = creater.CreateDMJHFile(obj, 0);

                DataAccessLayer.PlanManage.JH jh = new DataAccessLayer.PlanManage.JH()
                {
                    TaskID = obj.TaskID,
                    PlanType = "DJZYSQ",
                    PlanID = Convert.ToInt32(obj.SNO),
                    StartTime = Convert.ToDateTime(txtPlanStartTime.Text.Trim()),
                    EndTime = Convert.ToDateTime(txtPlanEndTime.Text.Trim()),
                    SRCType = 0,
                    FileIndex = filepath,
                    SatID = obj.SatID,
                    Reserve = txtNote.Text
                };
                var result = jh.Add();

                //删除当前临时计划
                DataAccessLayer.PlanManage.JH jhtemp = new DataAccessLayer.PlanManage.JH(true)
                {
                    Id = Convert.ToInt32(HfID.Value),
                };
                var resulttemp = jhtemp.DeleteTempJH();

                #region 转成正式计划之后，禁用除“返回”之外的所有按钮
                btnSubmit.Visible = false;
                btnSaveTo.Visible = false;
                btnReset.Visible = false;
                btnFormal.Visible = false;
                btnSurePlan.Visible = !(btnFormal.Visible);
                #endregion

                ltMessage.Text = "计划保存成功";

                txtSequence.Text = obj.SNO;
                //txtDatetime.Text = obj.SJ;
                txtTaskCount.Text = obj.SNUM;
                //
            }
            catch (Exception ex)
            {
                throw (new AspNetException("另存地面计划信息出现异常，异常原因", ex));
            }
            finally { }
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

            rpStation.DataSource = list;
            rpStation.DataBind();
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
            DJZYSQ ojh=null;
            string filepath = hfStationFile.Value;  //文件路径
            string ids = txtIds.Text;   //行号

            PlanProcessor pp = new PlanProcessor();
            pp.AddSIOtoCKZYSQ(ref ojh, filepath, ids);
            Repeater1.DataSource = ojh.DMJHTasks;
            Repeater1.DataBind();

            System.IO.File.Delete(filepath);    //删除临时文件
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
            DJZYSQ obj = new DJZYSQ();
            try
            {
                string filepath = objWord.CreateDJZYSQFile(obj.GetByID(sid));
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
                try
                {
                    var result = jh.UpdateStatus();
                    if (result == FieldVerifyResult.Success)
                        ltMessage.Text = "计划确认成功";
                    else
                        ltMessage.Text = "计划确认失败";
                }
                catch (Exception ex)
                {
                    throw (new AspNetException("确认测控资源使用申请出现异常，异常原因", ex));
                }
            }
        }
    }
}
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
using System.Collections;
using ServicesKernel.File;
using System.Data;

namespace OperatingManagement.Web.Views.PlanManage
{
    public partial class ZZGZJHEdit : AspNetPage, IRouteContext
    {
        bool isTempJH = false;  //是否为临时计划,默认为正式计划

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnFormal.Visible = false; 
                txtPlanStartTime.Attributes.Add("readonly", "true");
                txtPlanEndTime.Attributes.Add("readonly", "true");
                if (!string.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    string sID = Request.QueryString["id"];
                    if (!string.IsNullOrEmpty(Request.QueryString["istemp"]) && Request.QueryString["istemp"] == "true")
                    {
                        isTempJH = true;
                        ViewState["isTempJH"] = true;
                        btnFormal.Visible = true;   //只有临时计划才能转为正式计划
                    }
                    HfID.Value = sID;
                    hfStatus.Value = "edit";    //编辑
                    hfSBJHID.Value = "-1";
                    BindJhTable(sID);
                    BindXML();
                    hfURL.Value = "?type=ZZGZJH&startDate=" + Request.QueryString["startDate"] + "&endDate=" + Request.QueryString["endDate"];
                    if ("detail" == Request.QueryString["op"])
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "hide", "<script type='text/javascript'>hideAllButton();</script>");
                    }  
                }
                else
                {
                    btnReturn.Visible = false;
                    hfStatus.Value = "new"; //新建
                    btnSaveTo.Visible = false;
                    initial();
                }

            }
        }

        public void initial()
        {
            try
            {
                List<DMJH_Task> listTask = new List<DMJH_Task>();
                List<DMJH_Task_AfterFeedBack> listaf = new List<DMJH_Task_AfterFeedBack>();
                List<DMJH_Task_ReakTimeTransfor> listrt = new List<DMJH_Task_ReakTimeTransfor>();
                listaf.Add(new DMJH_Task_AfterFeedBack());
                listrt.Add(new DMJH_Task_ReakTimeTransfor());
                DMJH_Task dm = new DMJH_Task
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
                isTempJH = GetIsTempJHValue();
                List<JH> jh = (new JH(isTempJH)).SelectByIDS(sID);
                txtPlanStartTime.Text = jh[0].StartTime.ToString("yyyy-MM-dd HH:mm:ss");
                txtPlanEndTime.Text = jh[0].EndTime.ToString("yyyy-MM-dd HH:mm:ss");
                HfFileIndex.Value = jh[0].FileIndex;
                hfTaskID.Value = jh[0].TaskID.ToString();
                ucTask1.SelectedValue = jh[0].TaskID.ToString();
                string[] strTemp = jh[0].FileIndex.Split('_');
                if (strTemp.Length >= 2)
                {
                    hfSatID.Value = strTemp[strTemp.Length - 2];
                    ucSatellite1.SelectedValue = strTemp[strTemp.Length - 2];
                }
                txtNote.Text = jh[0].Reserve.ToString();
                if (DateTime.Now > jh[0].StartTime)
                {
                    btnSubmit.Visible = false;
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
                List<DMJH_Task> listTask = new List<DMJH_Task>();
                DMJH_Task task;
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(HfFileIndex.Value);
                XmlNode root = xmlDoc.SelectSingleNode("总装地面站工作计划/编号");
                txtSequence.Text = root.InnerText;
                root = xmlDoc.SelectSingleNode("总装地面站工作计划/时间");
                txtDatetime.Text = root.InnerText;
                root = xmlDoc.SelectSingleNode("总装地面站工作计划/工作单位");
                txtStationName.Text = root.InnerText;
                root = xmlDoc.SelectSingleNode("总装地面站工作计划/设备代号");
                txtEquipmentID.Text = root.InnerText;
                root = xmlDoc.SelectSingleNode("总装地面站工作计划/任务个数");
                txtTaskCount.Text = root.InnerText;

                root = xmlDoc.SelectSingleNode("总装地面站工作计划");
                foreach (XmlNode n in root.ChildNodes)
                {
                    if (n.Name == "任务")
                    {
                        task = new DMJH_Task();
                        task.TaskFlag = n["任务标志"].InnerText;
                        task.WorkWay = n["工作方式"].InnerText;
                        task.PlanPropertiy = n["计划性质"].InnerText;
                        task.WorkMode = n["工作模式"].InnerText;
                        task.PreStartTime = n["任务准备开始时间"].InnerText;
                        task.StartTime = n["任务开始时间"].InnerText;
                        task.TrackStartTime = n["跟踪开始时间"].InnerText;
                        task.WaveOnStartTime = n["开上行载波时间"].InnerText;
                        task.WaveOffStartTime = n["关上行载波时间"].InnerText;
                        task.TrackEndTime = n["跟踪结束时间"].InnerText;
                        task.EndTime = n["任务结束时间"].InnerText;
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
                    #region 初始化 工作方式，计划性质，工作模式 三个下拉列表的值
                    DropDownList ddlRFS = (DropDownList)e.Item.FindControl("ddlFS") as DropDownList;
                    ddlRFS.DataSource = PlanParameters.ReadParameters("DJZYSQFS");
                    ddlRFS.DataTextField = "Text";
                    ddlRFS.DataValueField = "Value";
                    ddlRFS.DataBind();

                    DropDownList ddlRJXZ = (DropDownList)e.Item.FindControl("ddlJXZ") as DropDownList;
                    ddlRJXZ.DataSource = PlanParameters.ReadParameters("DJZYSQSXZ");
                    ddlRJXZ.DataTextField = "Text";
                    ddlRJXZ.DataValueField = "Value";
                    ddlRJXZ.DataBind();

                    DropDownList ddlRMS = (DropDownList)e.Item.FindControl("ddlMS") as DropDownList;
                    ddlRMS.DataSource = PlanParameters.ReadParameters("ZZGZJHMS");
                    ddlRMS.DataTextField = "Text";
                    ddlRMS.DataValueField = "Value";
                    ddlRMS.DataBind();

                    DMJH_Task view = (DMJH_Task)e.Item.DataItem;
                    ddlRFS.SelectedValue = view.WorkWay;
                    ddlRJXZ.SelectedValue = view.PlanPropertiy;
                    ddlRMS.SelectedValue = view.WorkMode;

                    #endregion

                    #region 添加,删除任务时，从ViewState里获取页面 “实时传输” 和“事后回放”的值
                    if (ViewState["arrR"] != null && ViewState["arrA"] != null && ViewState["op"] != null)
                    {
                        ArrayList arrR = (ArrayList)ViewState["arrR"];
                        ArrayList arrA = (ArrayList)ViewState["arrA"];
                        Repeater rpr = e.Item.FindControl("rpReakTimeTransfor") as Repeater;
                        Repeater rpa = e.Item.FindControl("rpAfterFeedBack") as Repeater;

                        List<DMJH_Task_ReakTimeTransfor> listr = new List<DMJH_Task_ReakTimeTransfor>();
                        List<DMJH_Task_AfterFeedBack> lista = new List<DMJH_Task_AfterFeedBack>();
                        DMJH_Task_ReakTimeTransfor rtt;
                        DMJH_Task_AfterFeedBack afb;
                        string op = (string)ViewState["op"];
                        if (op == "Add")
                        {
                            #region ReakTimeTransfor
                            if (e.Item.ItemIndex <= arrR.Count - 1)
                            {
                                listr = (List<DMJH_Task_ReakTimeTransfor>)arrR[e.Item.ItemIndex];
                                rpr.DataSource = listr;
                                rpr.DataBind();
                            }
                            else
                            {
                                rtt = new DMJH_Task_ReakTimeTransfor();
                                rtt.FormatFlag = "";
                                rtt.InfoFlowFlag = "";
                                rtt.TransStartTime = "";
                                rtt.TransEndTime = "";
                                rtt.TransSpeedRate = "";
                                listr.Add(rtt);
                                rpr.DataSource = listr;
                                rpr.DataBind();
                            }
                            #endregion
                            #region AfterFeedBack
                            if (e.Item.ItemIndex <= arrA.Count - 1)
                            {
                                lista = (List<DMJH_Task_AfterFeedBack>)arrA[e.Item.ItemIndex];
                                rpa.DataSource = lista;
                                rpa.DataBind();
                            }
                            else
                            {
                                afb = new DMJH_Task_AfterFeedBack();
                                afb.FormatFlag = "";
                                afb.InfoFlowFlag = "";
                                afb.DataStartTime = "";
                                afb.DataEndTime = "";
                                afb.TransStartTime = "";
                                afb.TransSpeedRate = "";
                                lista.Add(afb);
                                rpa.DataSource = lista;
                                rpa.DataBind();
                            }
                            #endregion
                        }
                        if (op == "Del")
                        {
                            #region ReakTimeTransfor
                            if (e.Item.ItemIndex <= arrR.Count - 1)
                            {
                                listr = (List<DMJH_Task_ReakTimeTransfor>)arrR[e.Item.ItemIndex];
                                rpr.DataSource = listr;
                                rpr.DataBind();
                            }
                            #endregion
                            #region AfterFeedBack
                            if (e.Item.ItemIndex <= arrA.Count - 1)
                            {
                                lista = (List<DMJH_Task_AfterFeedBack>)arrA[e.Item.ItemIndex];
                                rpa.DataSource = lista;
                                rpa.DataBind();
                            }
                            #endregion
                        }
                    }
                    else
                    {
                        #region 正常
                        Repeater rpR = e.Item.FindControl("rpReakTimeTransfor") as Repeater;
                        Repeater rpA = e.Item.FindControl("rpAfterFeedBack") as Repeater;
                        int row = e.Item.ItemIndex;
                        List<DMJH_Task_ReakTimeTransfor> list1 = new List<DMJH_Task_ReakTimeTransfor>();
                        List<DMJH_Task_AfterFeedBack> list2 = new List<DMJH_Task_AfterFeedBack>();
                        DMJH_Task_ReakTimeTransfor rt;
                        DMJH_Task_AfterFeedBack afb;
                        if (hfStatus.Value == "new")
                        {
                            list1.Add(new DMJH_Task_ReakTimeTransfor());
                            list2.Add(new DMJH_Task_AfterFeedBack());
                        }
                        else
                        {
                            XmlDocument xmlDoc = new XmlDocument();
                            xmlDoc.Load(HfFileIndex.Value);
                            XmlNode root = xmlDoc.SelectSingleNode("总装地面站工作计划");
                            int i = 0;

                            foreach (XmlNode n in root.ChildNodes)
                            {
                                #region 任务
                                if (n.Name == "任务")
                                {
                                    foreach (XmlNode nd in n.ChildNodes)
                                    {
                                        if (row == i)
                                        {
                                            if (nd.Name == "实时传输")
                                            {
                                                rt = new DMJH_Task_ReakTimeTransfor();
                                                rt.FormatFlag = nd["格式标志"].InnerText;
                                                rt.InfoFlowFlag = nd["信息流标志"].InnerText;
                                                rt.TransStartTime = nd["数据传输开始时间"].InnerText;
                                                rt.TransEndTime = nd["数据传输结束时间"].InnerText;
                                                rt.TransSpeedRate = nd["数据传输速率"].InnerText;
                                                list1.Add(rt);
                                            }
                                            if (nd.Name == "事后回放")
                                            {
                                                afb = new DMJH_Task_AfterFeedBack();
                                                afb.FormatFlag = nd["格式标志"].InnerText;
                                                afb.InfoFlowFlag = nd["信息流标志"].InnerText;
                                                afb.DataStartTime = nd["数据起始时间"].InnerText;
                                                afb.DataEndTime = nd["数据结束时间"].InnerText;
                                                afb.TransStartTime = nd["数据传输开始时间"].InnerText;
                                                afb.TransSpeedRate = nd["数据传输速率"].InnerText;
                                                list2.Add(afb);
                                            }
                                        }
                                    }
                                    i = i + 1;//记录是第几个任务
                                }
                                #endregion

                            }
                        }
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
                List<DMJH_Task> list2 = new List<DMJH_Task>();
                DMJH_Task rt;
                List<DMJH_Task_ReakTimeTransfor> listr = new List<DMJH_Task_ReakTimeTransfor>();
                List<DMJH_Task_AfterFeedBack> lista = new List<DMJH_Task_AfterFeedBack>();
                DMJH_Task_ReakTimeTransfor rtt;
                DMJH_Task_AfterFeedBack afb;
                Repeater rp = (Repeater)source;
                ArrayList arrR = new ArrayList();
                ArrayList arrA = new ArrayList();
                if (e.CommandName == "Add")
                {
                    ViewState["op"] = "Add";
                    foreach (RepeaterItem it in rp.Items)
                    {
                        #region ReakTimeTransfor
                        Repeater rps = it.FindControl("rpReakTimeTransfor") as Repeater;
                        foreach (RepeaterItem its in rps.Items)
                        {

                            rtt = new DMJH_Task_ReakTimeTransfor();
                            TextBox txtFormatFlag = (TextBox)its.FindControl("txtFormatFlag");
                            TextBox txtInfoFlowFlag = (TextBox)its.FindControl("txtInfoFlowFlag");
                            TextBox txtTransStartTime = (TextBox)its.FindControl("txtTransStartTime");
                            TextBox txtTransEndTime = (TextBox)its.FindControl("txtTransEndTime");
                            TextBox txtTransSpeedRate = (TextBox)its.FindControl("txtTransSpeedRate");

                            rtt.FormatFlag = txtFormatFlag.Text;
                            rtt.InfoFlowFlag = txtInfoFlowFlag.Text;
                            rtt.TransStartTime = txtTransStartTime.Text;
                            rtt.TransEndTime = txtTransEndTime.Text;
                            rtt.TransSpeedRate = txtTransSpeedRate.Text;
                            listr.Add(rtt);

                        }
                        arrR.Add(listr);
                        listr = new List<DMJH_Task_ReakTimeTransfor>();
                        #endregion
                        #region AfterFeedBack
                        Repeater rpa = it.FindControl("rpAfterFeedBack") as Repeater;
                        foreach (RepeaterItem its in rpa.Items)
                        {

                            afb = new DMJH_Task_AfterFeedBack();
                            TextBox txtFormatFlag = (TextBox)its.FindControl("FormatFlag");
                            TextBox txtInfoFlowFlag = (TextBox)its.FindControl("InfoFlowFlag");
                            TextBox txtDataStartTime = (TextBox)its.FindControl("DataStartTime");
                            TextBox txtDataEndTime = (TextBox)its.FindControl("DataEndTime");
                            TextBox txtTransStartTime = (TextBox)its.FindControl("TransStartTime");
                            TextBox txtTransSpeedRate = (TextBox)its.FindControl("TransSpeedRate");

                            afb.FormatFlag = txtFormatFlag.Text;
                            afb.InfoFlowFlag = txtInfoFlowFlag.Text;
                            afb.DataStartTime = txtDataStartTime.Text;
                            afb.DataEndTime = txtDataEndTime.Text;
                            afb.TransStartTime = txtTransStartTime.Text;
                            afb.TransSpeedRate = txtTransSpeedRate.Text;
                            lista.Add(afb);

                        }
                        arrA.Add(lista);
                        lista = new List<DMJH_Task_AfterFeedBack>();
                        #endregion
                        #region Task
                        rt = new DMJH_Task();
                        TextBox txtTaskFlag = (TextBox)it.FindControl("txtTaskFlag");
                        DropDownList txtWorkWay = (DropDownList)it.FindControl("ddlFS");
                        DropDownList txtPlanPropertiy = (DropDownList)it.FindControl("ddlJXZ");
                        DropDownList txtWorkMode = (DropDownList)it.FindControl("ddlMS");
                        TextBox txtPreStartTime = (TextBox)it.FindControl("txtPreStartTime");
                        TextBox txtStartTime = (TextBox)it.FindControl("txtStartTime");
                        TextBox txtTrackStartTime = (TextBox)it.FindControl("txtTrackStartTime");
                        TextBox txtWaveOnStartTime = (TextBox)it.FindControl("txtWaveOnStartTime");
                        TextBox txtWaveOffStartTime = (TextBox)it.FindControl("txtWaveOffStartTime");
                        TextBox txtTrackEndTime = (TextBox)it.FindControl("txtTrackEndTime");
                        TextBox txtEndTime = (TextBox)it.FindControl("txtEndTime");

                        rt.TaskFlag = txtTaskFlag.Text;
                        rt.WorkWay = txtWorkWay.SelectedValue;
                        rt.PlanPropertiy = txtPlanPropertiy.SelectedValue;
                        rt.WorkMode = txtWorkMode.SelectedValue;
                        rt.PreStartTime = txtPreStartTime.Text;
                        rt.StartTime = txtStartTime.Text;
                        rt.TrackStartTime = txtTrackStartTime.Text;
                        rt.WaveOnStartTime = txtWaveOnStartTime.Text;
                        rt.WaveOffStartTime = txtWaveOffStartTime.Text;
                        rt.TrackEndTime = txtTrackEndTime.Text;
                        rt.EndTime = txtEndTime.Text;
                        list2.Add(rt);

                        #endregion
                    }
                    rt = new DMJH_Task();
                    rt.TaskFlag = "";
                    //rt.WorkWay = "";
                    //rt.PlanPropertiy = "";
                    //rt.WorkMode = "";
                    rt.PreStartTime = "";
                    rt.StartTime = "";
                    rt.TrackStartTime = "";
                    rt.WaveOnStartTime = "";
                    rt.WaveOffStartTime = "";
                    rt.TrackEndTime = "";
                    rt.EndTime = "";
                    list2.Add(rt);
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
                                #region ReakTimeTransfor
                                Repeater rps = it.FindControl("rpReakTimeTransfor") as Repeater;
                                foreach (RepeaterItem its in rps.Items)
                                {

                                    rtt = new DMJH_Task_ReakTimeTransfor();
                                    TextBox txtFormatFlag = (TextBox)its.FindControl("txtFormatFlag");
                                    TextBox txtInfoFlowFlag = (TextBox)its.FindControl("txtInfoFlowFlag");
                                    TextBox txtTransStartTime = (TextBox)its.FindControl("txtTransStartTime");
                                    TextBox txtTransEndTime = (TextBox)its.FindControl("txtTransEndTime");
                                    TextBox txtTransSpeedRate = (TextBox)its.FindControl("txtTransSpeedRate");

                                    rtt.FormatFlag = txtFormatFlag.Text;
                                    rtt.InfoFlowFlag = txtInfoFlowFlag.Text;
                                    rtt.TransStartTime = txtTransStartTime.Text;
                                    rtt.TransEndTime = txtTransEndTime.Text;
                                    rtt.TransSpeedRate = txtTransSpeedRate.Text;
                                    listr.Add(rtt);

                                }
                                arrR.Add(listr);
                                listr = new List<DMJH_Task_ReakTimeTransfor>();
                                #endregion
                                #region AfterFeedBack
                                Repeater rpa = it.FindControl("rpAfterFeedBack") as Repeater;
                                foreach (RepeaterItem its in rpa.Items)
                                {

                                    afb = new DMJH_Task_AfterFeedBack();
                                    TextBox txtFormatFlag = (TextBox)its.FindControl("FormatFlag");
                                    TextBox txtInfoFlowFlag = (TextBox)its.FindControl("InfoFlowFlag");
                                    TextBox txtDataStartTime = (TextBox)its.FindControl("DataStartTime");
                                    TextBox txtDataEndTime = (TextBox)its.FindControl("DataEndTime");
                                    TextBox txtTransStartTime = (TextBox)its.FindControl("TransStartTime");
                                    TextBox txtTransSpeedRate = (TextBox)its.FindControl("TransSpeedRate");

                                    afb.FormatFlag = txtFormatFlag.Text;
                                    afb.InfoFlowFlag = txtInfoFlowFlag.Text;
                                    afb.DataStartTime = txtDataStartTime.Text;
                                    afb.DataEndTime = txtDataEndTime.Text;
                                    afb.TransStartTime = txtTransStartTime.Text;
                                    afb.TransSpeedRate = txtTransSpeedRate.Text;
                                    lista.Add(afb);

                                }
                                arrA.Add(lista);
                                lista = new List<DMJH_Task_AfterFeedBack>();
                                #endregion
                                #region Task
                                rt = new DMJH_Task();
                                TextBox txtTaskFlag = (TextBox)it.FindControl("txtTaskFlag");
                                DropDownList txtWorkWay = (DropDownList)it.FindControl("ddlFS");
                                DropDownList txtPlanPropertiy = (DropDownList)it.FindControl("ddlJXZ");
                                DropDownList txtWorkMode = (DropDownList)it.FindControl("ddlMS");
                                TextBox txtPreStartTime = (TextBox)it.FindControl("txtPreStartTime");
                                TextBox txtStartTime = (TextBox)it.FindControl("txtStartTime");
                                TextBox txtTrackStartTime = (TextBox)it.FindControl("txtTrackStartTime");
                                TextBox txtWaveOnStartTime = (TextBox)it.FindControl("txtWaveOnStartTime");
                                TextBox txtWaveOffStartTime = (TextBox)it.FindControl("txtWaveOffStartTime");
                                TextBox txtTrackEndTime = (TextBox)it.FindControl("txtTrackEndTime");
                                TextBox txtEndTime = (TextBox)it.FindControl("txtEndTime");

                                rt.TaskFlag = txtTaskFlag.Text;
                                rt.WorkWay = txtWorkWay.SelectedValue;
                                rt.PlanPropertiy = txtPlanPropertiy.SelectedValue;
                                rt.WorkMode = txtWorkMode.SelectedValue;
                                rt.PreStartTime = txtPreStartTime.Text;
                                rt.StartTime = txtStartTime.Text;
                                rt.TrackStartTime = txtTrackStartTime.Text;
                                rt.WaveOnStartTime = txtWaveOnStartTime.Text;
                                rt.WaveOffStartTime = txtWaveOffStartTime.Text;
                                rt.TrackEndTime = txtTrackEndTime.Text;
                                rt.EndTime = txtEndTime.Text;
                                list2.Add(rt);
                                #endregion
                            }
                        }
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

        protected void Repeater2_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Add")
                {
                    List<DMJH_Task_ReakTimeTransfor> list2 = new List<DMJH_Task_ReakTimeTransfor>();
                    DMJH_Task_ReakTimeTransfor rt;
                    Repeater rp = (Repeater)source;
                    foreach (RepeaterItem it in rp.Items)
                    {
                        rt = new DMJH_Task_ReakTimeTransfor();
                        TextBox txtFormatFlag = (TextBox)it.FindControl("txtFormatFlag");
                        TextBox txtInfoFlowFlag = (TextBox)it.FindControl("txtInfoFlowFlag");
                        TextBox txtTransStartTime = (TextBox)it.FindControl("txtTransStartTime");
                        TextBox txtTransEndTime = (TextBox)it.FindControl("txtTransEndTime");
                        TextBox txtTransSpeedRate = (TextBox)it.FindControl("txtTransSpeedRate");

                        rt.FormatFlag = txtFormatFlag.Text;
                        rt.InfoFlowFlag = txtInfoFlowFlag.Text;
                        rt.TransStartTime = txtTransStartTime.Text;
                        rt.TransEndTime = txtTransEndTime.Text;
                        rt.TransSpeedRate = txtTransSpeedRate.Text;
                        list2.Add(rt);
                    }
                    rt = new DMJH_Task_ReakTimeTransfor();
                    rt.FormatFlag = "";
                    rt.InfoFlowFlag = "";
                    rt.TransStartTime = "";
                    rt.TransEndTime = "";
                    rt.TransSpeedRate = "";
                    list2.Add(rt);
                    rp.DataSource = list2;
                    rp.DataBind();
                }
                if (e.CommandName == "Del")
                {
                    List<DMJH_Task_ReakTimeTransfor> list2 = new List<DMJH_Task_ReakTimeTransfor>();
                    DMJH_Task_ReakTimeTransfor rt;
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
                                rt = new DMJH_Task_ReakTimeTransfor();
                                TextBox txtFormatFlag = (TextBox)it.FindControl("txtFormatFlag");
                                TextBox txtInfoFlowFlag = (TextBox)it.FindControl("txtInfoFlowFlag");
                                TextBox txtTransStartTime = (TextBox)it.FindControl("txtTransStartTime");
                                TextBox txtTransEndTime = (TextBox)it.FindControl("txtTransEndTime");
                                TextBox txtTransSpeedRate = (TextBox)it.FindControl("txtTransSpeedRate");

                                rt.FormatFlag = txtFormatFlag.Text;
                                rt.InfoFlowFlag = txtInfoFlowFlag.Text;
                                rt.TransStartTime = txtTransStartTime.Text;
                                rt.TransEndTime = txtTransEndTime.Text;
                                rt.TransSpeedRate = txtTransSpeedRate.Text;
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
                    List<DMJH_Task_AfterFeedBack> list2 = new List<DMJH_Task_AfterFeedBack>();
                    DMJH_Task_AfterFeedBack rt;
                    Repeater rp = (Repeater)source;
                    foreach (RepeaterItem it in rp.Items)
                    {
                        rt = new DMJH_Task_AfterFeedBack();
                        TextBox txtFormatFlag = (TextBox)it.FindControl("FormatFlag");
                        TextBox txtInfoFlowFlag = (TextBox)it.FindControl("InfoFlowFlag");
                        TextBox txtDataStartTime = (TextBox)it.FindControl("DataStartTime");
                        TextBox txtDataEndTime = (TextBox)it.FindControl("DataEndTime");
                        TextBox txtTransStartTime = (TextBox)it.FindControl("TransStartTime");
                        TextBox txtTransSpeedRate = (TextBox)it.FindControl("TransSpeedRate");

                        rt.FormatFlag = txtFormatFlag.Text;
                        rt.InfoFlowFlag = txtInfoFlowFlag.Text;
                        rt.DataStartTime = txtDataStartTime.Text;
                        rt.DataEndTime = txtDataEndTime.Text;
                        rt.TransStartTime = txtTransStartTime.Text;
                        rt.TransSpeedRate = txtTransSpeedRate.Text;
                        list2.Add(rt);
                    }
                    rt = new DMJH_Task_AfterFeedBack();
                    rt.FormatFlag = "";
                    rt.InfoFlowFlag = "";
                    rt.TransStartTime = "";
                    rt.DataEndTime = "";
                    rt.DataStartTime = "";
                    rt.TransSpeedRate = "";
                    list2.Add(rt);
                    rp.DataSource = list2;
                    rp.DataBind();
                }
                if (e.CommandName == "Del")
                {
                    List<DMJH_Task_AfterFeedBack> list2 = new List<DMJH_Task_AfterFeedBack>();
                    DMJH_Task_AfterFeedBack rt;
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
                                rt = new DMJH_Task_AfterFeedBack();
                                TextBox txtFormatFlag = (TextBox)it.FindControl("FormatFlag");
                                TextBox txtInfoFlowFlag = (TextBox)it.FindControl("InfoFlowFlag");
                                TextBox txtDataStartTime = (TextBox)it.FindControl("DataStartTime");
                                TextBox txtDataEndTime = (TextBox)it.FindControl("DataEndTime");
                                TextBox txtTransStartTime = (TextBox)it.FindControl("TransStartTime");
                                TextBox txtTransSpeedRate = (TextBox)it.FindControl("TransSpeedRate");

                                rt.FormatFlag = txtFormatFlag.Text;
                                rt.InfoFlowFlag = txtInfoFlowFlag.Text;
                                rt.DataStartTime = txtDataStartTime.Text;
                                rt.DataEndTime = txtDataEndTime.Text;
                                rt.TransStartTime = txtTransStartTime.Text;
                                rt.TransSpeedRate = txtTransSpeedRate.Text;
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


        public override void OnPageLoaded()
        {
            this.PagePermission = "Plan.Edit";
            this.ShortTitle = "编辑计划";
            base.OnPageLoaded();
            this.AddJavaScriptInclude("scripts/pages/PlanManage/DMJHEdit.aspx.js");
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                isTempJH = GetIsTempJHValue();
                ZZGZJH obj = new ZZGZJH();
                //obj.Sequence = txtSequence.Text;
                obj.Sequence = (new Sequence()).GeZZGZJHSequnce().ToString();
                obj.DateTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                obj.StationName = txtStationName.Text;
                obj.EquipmentID = txtEquipmentID.Text;
                // obj.TaskCount = txtTaskCount.Text;
                obj.DMJHTasks = new List<DMJH_Task>();

                DMJH_Task rt;
                DMJH_Task_ReakTimeTransfor rtt;
                DMJH_Task_AfterFeedBack afb;

                foreach (RepeaterItem it in Repeater1.Items)
                {
                    #region task
                    rt = new DMJH_Task();
                    TextBox txtTaskFlag = (TextBox)it.FindControl("txtTaskFlag");
                    DropDownList txtWorkWay = (DropDownList)it.FindControl("ddlFS");
                    DropDownList txtPlanPropertiy = (DropDownList)it.FindControl("ddlJXZ");
                    DropDownList txtWorkMode = (DropDownList)it.FindControl("ddlMS");
                    TextBox txtPreStartTime = (TextBox)it.FindControl("txtPreStartTime");
                    TextBox txtStartTime = (TextBox)it.FindControl("txtStartTime");
                    TextBox txtTrackStartTime = (TextBox)it.FindControl("txtTrackStartTime");
                    TextBox txtWaveOnStartTime = (TextBox)it.FindControl("txtWaveOnStartTime");
                    TextBox txtWaveOffStartTime = (TextBox)it.FindControl("txtWaveOffStartTime");
                    TextBox txtTrackEndTime = (TextBox)it.FindControl("txtTrackEndTime");
                    TextBox txtEndTime = (TextBox)it.FindControl("txtEndTime");
                    rt.TaskFlag = txtTaskFlag.Text;
                    rt.WorkWay = txtWorkWay.SelectedValue;
                    rt.PlanPropertiy = txtPlanPropertiy.SelectedValue;
                    rt.WorkMode = txtWorkMode.SelectedValue;
                    rt.PreStartTime = txtPreStartTime.Text;
                    rt.StartTime = txtStartTime.Text;
                    rt.TrackStartTime = txtTrackStartTime.Text;
                    rt.WaveOnStartTime = txtWaveOnStartTime.Text;
                    rt.WaveOffStartTime = txtWaveOffStartTime.Text;
                    rt.TrackEndTime = txtTrackEndTime.Text;
                    rt.EndTime = txtEndTime.Text;
                    rt.ReakTimeTransfors = new List<DMJH_Task_ReakTimeTransfor>();
                    rt.AfterFeedBacks = new List<DMJH_Task_AfterFeedBack>();
                    //obj.DMJHTasks.Add(rt);
                    #endregion
                    #region ReakTimeTransfor
                    Repeater rps = it.FindControl("rpReakTimeTransfor") as Repeater;
                    foreach (RepeaterItem its in rps.Items)
                    {

                        rtt = new DMJH_Task_ReakTimeTransfor();
                        TextBox txtFormatFlag = (TextBox)its.FindControl("txtFormatFlag");
                        TextBox txtInfoFlowFlag = (TextBox)its.FindControl("txtInfoFlowFlag");
                        TextBox txtTransStartTime = (TextBox)its.FindControl("txtTransStartTime");
                        TextBox txtTransEndTime = (TextBox)its.FindControl("txtTransEndTime");
                        TextBox txtTransSpeedRate = (TextBox)its.FindControl("txtTransSpeedRate");

                        rtt.FormatFlag = txtFormatFlag.Text;
                        rtt.InfoFlowFlag = txtInfoFlowFlag.Text;
                        rtt.TransStartTime = txtTransStartTime.Text;
                        rtt.TransEndTime = txtTransEndTime.Text;
                        rtt.TransSpeedRate = txtTransSpeedRate.Text;
                        rt.ReakTimeTransfors.Add(rtt);

                    }

                    #endregion
                    #region AfterFeedBack
                    Repeater rpa = it.FindControl("rpAfterFeedBack") as Repeater;
                    foreach (RepeaterItem its in rpa.Items)
                    {

                        afb = new DMJH_Task_AfterFeedBack();
                        TextBox txtFormatFlag = (TextBox)its.FindControl("FormatFlag");
                        TextBox txtInfoFlowFlag = (TextBox)its.FindControl("InfoFlowFlag");
                        TextBox txtDataStartTime = (TextBox)its.FindControl("DataStartTime");
                        TextBox txtDataEndTime = (TextBox)its.FindControl("DataEndTime");
                        TextBox txtTransStartTime = (TextBox)its.FindControl("TransStartTime");
                        TextBox txtTransSpeedRate = (TextBox)its.FindControl("TransSpeedRate");

                        afb.FormatFlag = txtFormatFlag.Text;
                        afb.InfoFlowFlag = txtInfoFlowFlag.Text;
                        afb.DataStartTime = txtDataStartTime.Text;
                        afb.DataEndTime = txtDataEndTime.Text;
                        afb.TransStartTime = txtTransStartTime.Text;
                        afb.TransSpeedRate = txtTransSpeedRate.Text;
                        rt.AfterFeedBacks.Add(afb);

                    }
                    #endregion
                    obj.DMJHTasks.Add(rt);
                }
                obj.TaskCount = obj.DMJHTasks.Count.ToString();
                obj.TaskID = ucTask1.SelectedItem.Value;
                obj.SatID = ucSatellite1.SelectedItem.Value;

                PlanFileCreator creater = new PlanFileCreator(isTempJH);
                if (hfStatus.Value == "new")
                {
                    string filepath = creater.CreateZZGZJHFile(obj, 0);

                    DataAccessLayer.PlanManage.JH jh = new DataAccessLayer.PlanManage.JH()
                    {
                        TaskID = obj.TaskID,
                        PlanType = "ZZGZJH",
                        PlanID = Convert.ToInt32(obj.Sequence),
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
                    //当任务和卫星更新时，需要更新文件名称
                    if (hfSatID.Value != ucSatellite1.SelectedValue || hfTaskID.Value != ucTask1.SelectedValue)
                    {
                        string filepath = creater.CreateZZGZJHFile(obj, 0);

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
                        creater.CreateZZGZJHFile(obj, 1);
                    }
                }

                ltMessage.Text = "计划保存成功";
                txtSequence.Text = obj.Sequence;
                txtDatetime.Text = obj.DateTime;
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
                isTempJH = GetIsTempJHValue();
                ZZGZJH obj = new ZZGZJH();
                //obj.Sequence = txtSequence.Text;
                obj.Sequence = (new Sequence()).GeZZGZJHSequnce().ToString();
                obj.DateTime = txtDatetime.Text;
                obj.StationName = txtStationName.Text;
                obj.EquipmentID = txtEquipmentID.Text;
                // obj.TaskCount = txtTaskCount.Text;
                obj.DMJHTasks = new List<DMJH_Task>();

                DMJH_Task rt;
                DMJH_Task_ReakTimeTransfor rtt;
                DMJH_Task_AfterFeedBack afb;

                foreach (RepeaterItem it in Repeater1.Items)
                {
                    #region task
                    rt = new DMJH_Task();
                    TextBox txtTaskFlag = (TextBox)it.FindControl("txtTaskFlag");
                    DropDownList txtWorkWay = (DropDownList)it.FindControl("ddlFS");
                    DropDownList txtPlanPropertiy = (DropDownList)it.FindControl("ddlJXZ");
                    DropDownList txtWorkMode = (DropDownList)it.FindControl("ddlMS");
                    TextBox txtPreStartTime = (TextBox)it.FindControl("txtPreStartTime");
                    TextBox txtStartTime = (TextBox)it.FindControl("txtStartTime");
                    TextBox txtTrackStartTime = (TextBox)it.FindControl("txtTrackStartTime");
                    TextBox txtWaveOnStartTime = (TextBox)it.FindControl("txtWaveOnStartTime");
                    TextBox txtWaveOffStartTime = (TextBox)it.FindControl("txtWaveOffStartTime");
                    TextBox txtTrackEndTime = (TextBox)it.FindControl("txtTrackEndTime");
                    TextBox txtEndTime = (TextBox)it.FindControl("txtEndTime");
                    rt.TaskFlag = txtTaskFlag.Text;
                    rt.WorkWay = txtWorkWay.SelectedValue;
                    rt.PlanPropertiy = txtPlanPropertiy.SelectedValue;
                    rt.WorkMode = txtWorkMode.SelectedValue;
                    rt.PreStartTime = txtPreStartTime.Text;
                    rt.StartTime = txtStartTime.Text;
                    rt.TrackStartTime = txtTrackStartTime.Text;
                    rt.WaveOnStartTime = txtWaveOnStartTime.Text;
                    rt.WaveOffStartTime = txtWaveOffStartTime.Text;
                    rt.TrackEndTime = txtTrackEndTime.Text;
                    rt.EndTime = txtEndTime.Text;
                    rt.ReakTimeTransfors = new List<DMJH_Task_ReakTimeTransfor>();
                    rt.AfterFeedBacks = new List<DMJH_Task_AfterFeedBack>();
                    //obj.DMJHTasks.Add(rt);
                    #endregion
                    #region ReakTimeTransfor
                    Repeater rps = it.FindControl("rpReakTimeTransfor") as Repeater;
                    foreach (RepeaterItem its in rps.Items)
                    {

                        rtt = new DMJH_Task_ReakTimeTransfor();
                        TextBox txtFormatFlag = (TextBox)its.FindControl("txtFormatFlag");
                        TextBox txtInfoFlowFlag = (TextBox)its.FindControl("txtInfoFlowFlag");
                        TextBox txtTransStartTime = (TextBox)its.FindControl("txtTransStartTime");
                        TextBox txtTransEndTime = (TextBox)its.FindControl("txtTransEndTime");
                        TextBox txtTransSpeedRate = (TextBox)its.FindControl("txtTransSpeedRate");

                        rtt.FormatFlag = txtFormatFlag.Text;
                        rtt.InfoFlowFlag = txtInfoFlowFlag.Text;
                        rtt.TransStartTime = txtTransStartTime.Text;
                        rtt.TransEndTime = txtTransEndTime.Text;
                        rtt.TransSpeedRate = txtTransSpeedRate.Text;
                        rt.ReakTimeTransfors.Add(rtt);

                    }

                    #endregion
                    #region AfterFeedBack
                    Repeater rpa = it.FindControl("rpAfterFeedBack") as Repeater;
                    foreach (RepeaterItem its in rpa.Items)
                    {

                        afb = new DMJH_Task_AfterFeedBack();
                        TextBox txtFormatFlag = (TextBox)its.FindControl("FormatFlag");
                        TextBox txtInfoFlowFlag = (TextBox)its.FindControl("InfoFlowFlag");
                        TextBox txtDataStartTime = (TextBox)its.FindControl("DataStartTime");
                        TextBox txtDataEndTime = (TextBox)its.FindControl("DataEndTime");
                        TextBox txtTransStartTime = (TextBox)its.FindControl("TransStartTime");
                        TextBox txtTransSpeedRate = (TextBox)its.FindControl("TransSpeedRate");

                        afb.FormatFlag = txtFormatFlag.Text;
                        afb.InfoFlowFlag = txtInfoFlowFlag.Text;
                        afb.DataStartTime = txtDataStartTime.Text;
                        afb.DataEndTime = txtDataEndTime.Text;
                        afb.TransStartTime = txtTransStartTime.Text;
                        afb.TransSpeedRate = txtTransSpeedRate.Text;
                        rt.AfterFeedBacks.Add(afb);

                    }
                    #endregion
                    obj.DMJHTasks.Add(rt);
                }
                obj.TaskCount = obj.DMJHTasks.Count.ToString();


                PlanFileCreator creater = new PlanFileCreator(isTempJH);

                obj.TaskID = ucTask1.SelectedItem.Value;
                obj.SatID = ucSatellite1.SelectedItem.Value;

                //检查文件是否已经存在
                if (creater.TestZZGZJHFileName(obj))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "File", "<script type='text/javascript'>showMsg('存在同名文件，请一分钟后重试');</script>");
                    return;
                }
                string filepath = creater.CreateZZGZJHFile(obj, 0);

                DataAccessLayer.PlanManage.JH jh = new DataAccessLayer.PlanManage.JH(isTempJH)
                {
                    TaskID = obj.TaskID,
                    PlanType = "ZZGZJH",
                    PlanID = Convert.ToInt32(obj.Sequence),
                    StartTime = Convert.ToDateTime(txtPlanStartTime.Text.Trim()),
                    EndTime = Convert.ToDateTime(txtPlanEndTime.Text.Trim()),
                    SRCType = 0,
                    FileIndex = filepath,
                    SatID = obj.SatID,
                    Reserve = txtNote.Text
                };
                var result = jh.Add();

                ltMessage.Text = "计划保存成功";
                txtSequence.Text = obj.Sequence;
                txtDatetime.Text = obj.DateTime;
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
                ZZGZJH objDMJH = new ZZGZJH
                {
                    DMJHTasks = new List<DMJH_Task> 
                        {
                            new DMJH_Task
                            {
                                ReakTimeTransfors = new List<DMJH_Task_ReakTimeTransfor>{new DMJH_Task_ReakTimeTransfor()},
                                AfterFeedBacks = new List<DMJH_Task_AfterFeedBack>{new DMJH_Task_AfterFeedBack()}
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

                DMJH_Task task;
                DMJH_Task_ReakTimeTransfor rt;
                DMJH_Task_AfterFeedBack afb;
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(jh[0].FileIndex);
                XmlNode root = xmlDoc.SelectSingleNode("测控资源使用计划/编号");
                objDMJH.Sequence = root.InnerText;
                root = xmlDoc.SelectSingleNode("测控资源使用计划/时间");
                objDMJH.DateTime = root.InnerText;
                //root = xmlDoc.SelectSingleNode("设备工作计划/工作单位");
                //objDMJH.StationName = root.InnerText;
                //root = xmlDoc.SelectSingleNode("设备工作计划/设备代号");
                //objDMJH.EquipmentID = root.InnerText;
                root = xmlDoc.SelectSingleNode("测控资源使用计划/任务个数");
                objDMJH.TaskCount = root.InnerText;

                root = xmlDoc.SelectSingleNode("测控资源使用计划/计划");
                foreach (XmlNode n in root.ChildNodes)
                {
                    if (n.Name == "计划内容")
                    {
                        task = new DMJH_Task();
                        //task.TaskFlag = n["任务标志"].InnerText;
                        task.WorkWay = n["工作方式"].InnerText;
                        task.PlanPropertiy = n["计划性质"].InnerText;
                        //task.WorkMode = n["工作模式"].InnerText;
                        task.PreStartTime = n["任务准备开始时间"].InnerText;
                        task.StartTime = n["任务开始时间"].InnerText;
                        task.TrackStartTime = n["跟踪开始时间"].InnerText;
                        task.WaveOnStartTime = n["开上行载波时间"].InnerText;
                        task.WaveOffStartTime = n["关上行载波时间"].InnerText;
                        task.TrackEndTime = n["跟踪结束时间"].InnerText;
                        task.EndTime = n["任务结束时间"].InnerText;
                        task.ReakTimeTransfors = new List<DMJH_Task_ReakTimeTransfor>();
                        task.AfterFeedBacks = new List<DMJH_Task_AfterFeedBack>();

                        objDMJH.DMJHTasks.Add(task);
                    }
                }
                #endregion

                #region BindtoPage
                txtSequence.Text = objDMJH.Sequence;
                txtDatetime.Text = objDMJH.DateTime;
                txtStationName.Text = objDMJH.StationName;
                txtEquipmentID.Text = objDMJH.EquipmentID;
                txtTaskCount.Text = objDMJH.TaskCount;

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
                ZZGZJH obj = new ZZGZJH();
                //obj.Sequence = txtSequence.Text;
                obj.Sequence = (new Sequence()).GeZZGZJHSequnce().ToString();
                obj.DateTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                obj.StationName = txtStationName.Text;
                obj.EquipmentID = txtEquipmentID.Text;
                // obj.TaskCount = txtTaskCount.Text;
                obj.DMJHTasks = new List<DMJH_Task>();

                DMJH_Task rt;
                DMJH_Task_ReakTimeTransfor rtt;
                DMJH_Task_AfterFeedBack afb;

                foreach (RepeaterItem it in Repeater1.Items)
                {
                    #region task
                    rt = new DMJH_Task();
                    TextBox txtTaskFlag = (TextBox)it.FindControl("txtTaskFlag");
                    DropDownList txtWorkWay = (DropDownList)it.FindControl("ddlFS");
                    DropDownList txtPlanPropertiy = (DropDownList)it.FindControl("ddlJXZ");
                    DropDownList txtWorkMode = (DropDownList)it.FindControl("ddlMS");
                    TextBox txtPreStartTime = (TextBox)it.FindControl("txtPreStartTime");
                    TextBox txtStartTime = (TextBox)it.FindControl("txtStartTime");
                    TextBox txtTrackStartTime = (TextBox)it.FindControl("txtTrackStartTime");
                    TextBox txtWaveOnStartTime = (TextBox)it.FindControl("txtWaveOnStartTime");
                    TextBox txtWaveOffStartTime = (TextBox)it.FindControl("txtWaveOffStartTime");
                    TextBox txtTrackEndTime = (TextBox)it.FindControl("txtTrackEndTime");
                    TextBox txtEndTime = (TextBox)it.FindControl("txtEndTime");
                    rt.TaskFlag = txtTaskFlag.Text;
                    rt.WorkWay = txtWorkWay.SelectedValue;
                    rt.PlanPropertiy = txtPlanPropertiy.SelectedValue;
                    rt.WorkMode = txtWorkMode.SelectedValue;
                    rt.PreStartTime = txtPreStartTime.Text;
                    rt.StartTime = txtStartTime.Text;
                    rt.TrackStartTime = txtTrackStartTime.Text;
                    rt.WaveOnStartTime = txtWaveOnStartTime.Text;
                    rt.WaveOffStartTime = txtWaveOffStartTime.Text;
                    rt.TrackEndTime = txtTrackEndTime.Text;
                    rt.EndTime = txtEndTime.Text;
                    rt.ReakTimeTransfors = new List<DMJH_Task_ReakTimeTransfor>();
                    rt.AfterFeedBacks = new List<DMJH_Task_AfterFeedBack>();
                    //obj.DMJHTasks.Add(rt);
                    #endregion
                    #region ReakTimeTransfor
                    Repeater rps = it.FindControl("rpReakTimeTransfor") as Repeater;
                    foreach (RepeaterItem its in rps.Items)
                    {

                        rtt = new DMJH_Task_ReakTimeTransfor();
                        TextBox txtFormatFlag = (TextBox)its.FindControl("txtFormatFlag");
                        TextBox txtInfoFlowFlag = (TextBox)its.FindControl("txtInfoFlowFlag");
                        TextBox txtTransStartTime = (TextBox)its.FindControl("txtTransStartTime");
                        TextBox txtTransEndTime = (TextBox)its.FindControl("txtTransEndTime");
                        TextBox txtTransSpeedRate = (TextBox)its.FindControl("txtTransSpeedRate");

                        rtt.FormatFlag = txtFormatFlag.Text;
                        rtt.InfoFlowFlag = txtInfoFlowFlag.Text;
                        rtt.TransStartTime = txtTransStartTime.Text;
                        rtt.TransEndTime = txtTransEndTime.Text;
                        rtt.TransSpeedRate = txtTransSpeedRate.Text;
                        rt.ReakTimeTransfors.Add(rtt);

                    }

                    #endregion
                    #region AfterFeedBack
                    Repeater rpa = it.FindControl("rpAfterFeedBack") as Repeater;
                    foreach (RepeaterItem its in rpa.Items)
                    {

                        afb = new DMJH_Task_AfterFeedBack();
                        TextBox txtFormatFlag = (TextBox)its.FindControl("FormatFlag");
                        TextBox txtInfoFlowFlag = (TextBox)its.FindControl("InfoFlowFlag");
                        TextBox txtDataStartTime = (TextBox)its.FindControl("DataStartTime");
                        TextBox txtDataEndTime = (TextBox)its.FindControl("DataEndTime");
                        TextBox txtTransStartTime = (TextBox)its.FindControl("TransStartTime");
                        TextBox txtTransSpeedRate = (TextBox)its.FindControl("TransSpeedRate");

                        afb.FormatFlag = txtFormatFlag.Text;
                        afb.InfoFlowFlag = txtInfoFlowFlag.Text;
                        afb.DataStartTime = txtDataStartTime.Text;
                        afb.DataEndTime = txtDataEndTime.Text;
                        afb.TransStartTime = txtTransStartTime.Text;
                        afb.TransSpeedRate = txtTransSpeedRate.Text;
                        rt.AfterFeedBacks.Add(afb);

                    }
                    #endregion
                    obj.DMJHTasks.Add(rt);
                }
                obj.TaskCount = obj.DMJHTasks.Count.ToString();
                obj.TaskID = ucTask1.SelectedItem.Value;
                obj.SatID = ucSatellite1.SelectedItem.Value;

                PlanFileCreator creater = new PlanFileCreator();
                if (hfStatus.Value == "new")
                {
                    string filepath = creater.CreateZZGZJHFile(obj, 0);

                    DataAccessLayer.PlanManage.JH jh = new DataAccessLayer.PlanManage.JH()
                    {
                        TaskID = obj.TaskID,
                        PlanType = "ZZGZJH",
                        PlanID = Convert.ToInt32(obj.Sequence),
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
                    //当任务和卫星更新时，需要更新文件名称
                    if (hfSatID.Value != ucSatellite1.SelectedValue || hfTaskID.Value != ucTask1.SelectedValue)
                    {
                        string filepath = creater.CreateZZGZJHFile(obj, 0);

                        DataAccessLayer.PlanManage.JH jh = new DataAccessLayer.PlanManage.JH()
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
                        creater.CreateZZGZJHFile(obj, 1);
                    }
                }

                #region 转成正式计划之后，禁用除“返回”之外的所有按钮
                btnSubmit.Visible = false;
                btnSaveTo.Visible = false;
                btnReset.Visible = false;
                btnFormal.Visible = false;

                #endregion

                ltMessage.Text = "计划保存成功";
                txtSequence.Text = obj.Sequence;
                txtDatetime.Text = obj.DateTime;
                //ClientScript.RegisterStartupScript(this.GetType(), "OK", "<script type='text/javascript'>showMsg('计划保存成功');</script>");
                //
            }
            catch (Exception ex)
            {
                throw (new AspNetException("保存地面计划信息出现异常，异常原因", ex));
            }
            finally { }
        }

        //
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;

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
using ServicesKernel.File;
using OperatingManagement.ServicesKernel.File;

namespace OperatingManagement.Web.Views.PlanManage
{
    public partial class GZJHEdit : AspNetPage, IRouteContext
    {
        bool isTempJH = false;  //是否为临时计划,默认为正式计划

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["view"] == "1")
                    this.IsViewOrEdit = true; 
                btnFormal.Visible = false; 
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
                    inital(false);
                    BindJhTable(sID);
                    BindXML();
                    hfURL.Value = "?type=GZJH&startDate=" + Request.QueryString["startDate"] + "&endDate=" + Request.QueryString["endDate"]
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
                    inital(true);
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
            //txtQS.Attributes.Add("readonly", "true");

            //信息分类
            ddlXXFL.DataSource = PlanParameters.ReadParameters("GZJHXXFL");
            ddlXXFL.DataTextField = "Text";
            ddlXXFL.DataValueField = "Value";
            ddlXXFL.DataBind();

            ddlMutiSatTask.DataSource = new Task().Cache.Where(t => t.SatID == "AAAA").ToList();
            ddlMutiSatTask.DataTextField = "TaskName";
            ddlMutiSatTask.DataValueField = "OutTaskNo";
            ddlMutiSatTask.DataBind();

            if (isNew)
            {
                List<GZJH_Content> list = new List<GZJH_Content>();
                list.Add(new GZJH_Content());
                rpDatas.DataSource = list;
                rpDatas.DataBind();
            }
        }
        private void BindJhTable(string sID)
        {
            try
            {
                string outTaskID = string.Empty;
                isTempJH = GetIsTempJHValue();
                List<JH> jh = (new JH(isTempJH)).SelectByIDS(sID);
                outTaskID = new Task().GetOutTaskNo(jh[0].TaskID, jh[0].SatID);
                HfFileIndex.Value = jh[0].FileIndex;
                txtJXH.Text = jh[0].PlanID.ToString("0000");
                txtPlanStartTime.Text = jh[0].StartTime.ToString("yyyy-MM-dd HH:mm:ss");
                txtPlanEndTime.Text = jh[0].EndTime.ToString("yyyy-MM-dd HH:mm:ss");
                ddlMutiSatTask.SelectedValue = outTaskID;
                hfTaskID.Value = ddlMutiSatTask.SelectedValue;
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
                CultureInfo provider = CultureInfo.InvariantCulture;
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(HfFileIndex.Value);
                XmlNode root = xmlDoc.SelectSingleNode("地面站工作计划/JXH");
                txtJXH.Text = root.InnerText;
                root = xmlDoc.SelectSingleNode("地面站工作计划/XXFL");
                ddlXXFL.SelectedValue = root.InnerText;
                bool hasChilds = false;
                //root = xmlDoc.SelectSingleNode("地面站工作计划/QS");
                //txtQS.Text = root.InnerText;

                #region Content
                root = xmlDoc.SelectSingleNode("地面站工作计划");
                List<GZJH_Content> list = new List<GZJH_Content>();
                GZJH_Content c;
                foreach (XmlNode n in root.ChildNodes)
                {
                    if (n.Name == "Content")
                    {
                        hasChilds = true;
                        c = new GZJH_Content();
                        c.DW = n["DW"].InnerText;
                        c.SB = n["SB"].InnerText;
                        c.QS = n["QS"].InnerText;
                        c.QH = n["QH"].InnerText;
                        c.DH = n["DH"].InnerText;
                        c.FS = n["FS"].InnerText;
                        c.JXZ = n["JXZ"].InnerText;
                        c.MS = n["MS"].InnerText;
                        c.QB = n["QB"].InnerText;
                        c.GXZ = n["GXZ"].InnerText;
                        c.ZHB = n["ZHB"].InnerText;
                        c.GZK = n["GZK"].InnerText;
                        c.GZJ = n["GZJ"].InnerText;
                        c.KSHX = n["KSHX"].InnerText;
                        c.GSHX = n["GSHX"].InnerText;
                        c.RK = n["RK"].InnerText;
                        c.JS = n["JS"].InnerText;
                        c.BID = n["BID"].InnerText;
                        c.SBZ = n["SBZ"].InnerText;
                        c.RTs = n["RTs"].InnerText;
                        c.RTe = n["RTe"].InnerText;
                        c.SL = n["SL"].InnerText;
                        c.HBID = n["HBID"].InnerText;
                        c.HBZ = n["HBZ"].InnerText;
                        c.Ts = n["Ts"].InnerText;
                        c.Te = n["Te"].InnerText;
                        c.HRTs = n["HRTs"].InnerText;
                        c.HSL = n["HSL"].InnerText;
                        list.Add(c);
                    }
                }
                if (!hasChilds)
                {
                    c = new GZJH_Content();
                    list.Add(c);
                }
                rpDatas.DataSource = list;
                rpDatas.DataBind();
                #endregion
            }
            catch (Exception ex)
            {
                throw (new AspNetException("绑定地面站工作计划出现异常，异常原因", ex));
            }
            finally { }
        }
        public override void OnPageLoaded()
        {
            this.PagePermission = "OMPLAN_Plan.Edit";
            this.ShortTitle = "编辑计划";
            base.OnPageLoaded();
           //this.SetTitle();
            this.AddJavaScriptInclude("scripts/pages/PlanManage/GZJHEdit.aspx.js");

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            GZJH obj = GetJHFromPage("保存");
            HideMsg();
            #region 保存计划
            try
            {
                PlanFileCreator creater = new PlanFileCreator(isTempJH);

                if (hfStatus.Value == "new")
                {
                    //新建时才生成计划序号
                    //保存时才生成计划序号
                    obj.JXH = (new Sequence()).GetGZJHSequnce().ToString("0000");
                    txtJXH.Text = obj.JXH;
                    string filepath = creater.CreateGZJHFile(obj, 0);
                    DataAccessLayer.PlanManage.JH jh = new DataAccessLayer.PlanManage.JH(isTempJH)
                    {
                        TaskID = obj.TaskID,
                        PlanType = "GZJH",
                        PlanID = Convert.ToInt32(obj.JXH),
                        StartTime = Convert.ToDateTime(txtPlanStartTime.Text),
                        EndTime = Convert.ToDateTime(txtPlanEndTime.Text),
                        SRCType = 0,
                        FileIndex = filepath,
                        SatID = obj.SatID,
                        Reserve = txtNote.Text
                    };
                    var result = jh.Add();
                    ShowMsg(result == FieldVerifyResult.Success);
                    HfID.Value = jh.ID.ToString();
                }
                else
                {
                    //当任务更新时，需要更新文件名称
                    if (hfTaskID.Value != ddlMutiSatTask.SelectedValue)
                    {
                        string filepath = creater.CreateGZJHFile(obj, 0);
                        DataAccessLayer.PlanManage.JH jh = new DataAccessLayer.PlanManage.JH(isTempJH)
                        {
                            Id = Convert.ToInt32(HfID.Value),
                            TaskID = obj.TaskID,
                            StartTime = Convert.ToDateTime(txtPlanStartTime.Text),
                            EndTime = Convert.ToDateTime(txtPlanEndTime.Text),
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
                        creater.CreateGZJHFile(obj, 1);
                        ShowMsg(true);
                        if (!isTempJH)
                        {
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
                }
            }
            catch (Exception ex)
            {
                throw (new AspNetException("保存计划出现异常，异常原因", ex));
            }
            finally { }
            #endregion
        }

        protected void btnSaveTo_Click(object sender, EventArgs e)
        {
            GZJH obj = GetJHFromPage("另存");
            HideMsg();
            #region 另存计划
            try
            {
                PlanFileCreator creater = new PlanFileCreator(isTempJH);
                obj.JXH = (new Sequence()).GetGZJHSequnce().ToString("0000");
                if (creater.TestGZJHFileName(obj))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "File", "<script type='text/javascript'>showMsg('存在同名文件，请一分钟后重试');</script>");
                    return;
                }

                string filepath = creater.CreateGZJHFile(obj, 0);
                
                DataAccessLayer.PlanManage.JH jh = new DataAccessLayer.PlanManage.JH(isTempJH)
                {
                    TaskID = obj.TaskID,
                    PlanType = "GZJH",
                    PlanID = Convert.ToInt32(obj.JXH),
                    StartTime = Convert.ToDateTime(txtPlanStartTime.Text),
                    EndTime = Convert.ToDateTime(txtPlanEndTime.Text),
                    SRCType = 0,
                    FileIndex = filepath,
                    SatID = obj.SatID,
                    Reserve = txtNote.Text
                };
                var result = jh.Add();
                ShowMsg(result == FieldVerifyResult.Success);
                HfID.Value = jh.ID.ToString();
                txtJXH.Text = obj.JXH;  //另存后显示新的序号
            }
            catch (Exception ex)
            {
                throw (new AspNetException("另存计划信息出现异常，异常原因", ex));
            }
            finally { }
            #endregion
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
            GZJH obj = GetJHFromPage("转正");
            HideMsg();
            try
            {
                PlanFileCreator creater = new PlanFileCreator();
                obj.JXH = (new Sequence()).GetGZJHSequnce().ToString("0000");
                if (creater.TestGZJHFileName(obj))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "File", "<script type='text/javascript'>showMsg('存在同名文件，请一分钟后重试');</script>");
                    return;
                }

                string filepath = creater.CreateGZJHFile(obj, 0);
                DataAccessLayer.PlanManage.JH jh = new DataAccessLayer.PlanManage.JH()
                {
                    TaskID = obj.TaskID,
                    PlanType = "GZJH",
                    PlanID = Convert.ToInt32(obj.JXH),
                    StartTime = Convert.ToDateTime(txtPlanStartTime.Text),
                    EndTime = Convert.ToDateTime(txtPlanEndTime.Text),
                    SRCType = 0,
                    FileIndex = filepath,
                    SatID = obj.SatID,
                    Reserve = txtNote.Text
                };
                var result = jh.Add();
                ShowMsg(result == FieldVerifyResult.Success);
                HfID.Value = jh.ID.ToString();
                //删除当前临时计划
                DataAccessLayer.PlanManage.JH jhtemp = new DataAccessLayer.PlanManage.JH(true)
                {
                    Id = Convert.ToInt32(HfID.Value),
                };
                var resulttemp = jhtemp.DeleteTempJH();

                #region 转成正式计划之后，禁用一些按钮
                HfID.Value = jh.ID.ToString();
                btnSubmit.Visible = true;
                btnSaveTo.Visible = true;
                btnReset.Visible = false;
                btnFormal.Visible = false;
                btnSurePlan.Visible = !(btnFormal.Visible);
                #endregion

                txtJXH.Text = obj.JXH;  //另存后显示新的序号
            }
            catch (Exception ex)
            {
                throw (new AspNetException("另存计划信息出现异常，异常原因", ex));
            }
            finally { }
        }

        protected void rpDatas_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            Repeater rp = (Repeater)source;
            List<GZJH_Content> list;
            try
            {
                if (e.CommandName == "Add")
                {
                    list = GetRPContents("添加", rp, -1);
                    GZJH_Content co;
                    ViewState["op"] = "Add";
                    #region new a GZJH_Content
                    co = new GZJH_Content() { 
                        DW="",
                        SB="",
                        QS="",
                        DH="",
                        FS="",
                        JXZ = "",
                        MS = "",
                        QB = "",
                        GXZ = "",
                        ZHB = "",
                        QH = "",
                        GZK = "",
                        GZJ = "",
                        KSHX = "",
                        GSHX = "",
                        RK = "",
                        JS = "",
                        BID = "",
                        SBZ = "",
                        RTs = "",
                        RTe = "",
                        SL = "",
                        HBID="",
                        HBZ = "",
                        Ts = "",
                        Te = "",
                        HRTs="",
                        HSL=""
                    };
                    #endregion
                    list.Add(co);
                    rp.DataSource = list;
                    rp.DataBind();
                }
                if (e.CommandName == "Del")
                {
                    list = new List<GZJH_Content>();
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
                throw (new AspNetException("绑定目标信息出现异常，异常原因", ex));
            }
            finally { }
        }

        protected void rpDatas_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    DMZ oDMZ = new DMZ();
                    Task oTask = new Task();
                    GZJH_Content g = (GZJH_Content)e.Item.DataItem;
                    #region 初始化各个下拉列表的值
                    //任务代号
                    string satID = string.Empty;
                    string taskID = string.Empty;
                    oTask.GetTaskNoSatID(ddlMutiSatTask.SelectedValue, out taskID, out satID);
                    DropDownList ddlTask = (DropDownList)e.Item.FindControl("ddlTask") as DropDownList;
                    ddlTask.DataSource = new Task().Cache.Where(t => t.TaskNo == taskID).ToList();
                    ddlTask.DataTextField = "TaskName";
                    ddlTask.DataValueField = "OutTaskNo";
                    ddlTask.DataBind();
                    if (!string.IsNullOrEmpty(g.DH))
                    {
                        ddlTask.SelectedValue = g.DH;
                    }
                    //工作单位
                    DropDownList ddlDW = (DropDownList)e.Item.FindControl("ddlDW") as DropDownList;
                    ddlDW.DataSource = oDMZ.Cache.Where(t=>t.Owner != 2).ToList();
                    ddlDW.DataTextField = "DMZName";
                    ddlDW.DataValueField = "DWCode";
                    ddlDW.DataBind();
                    if (!string.IsNullOrEmpty(g.DW))
                    {
                        ddlDW.SelectedValue = g.DW;
                    }
                    //设备代号
                    List<DMZ> lstResult = oDMZ.Cache.Where(t => t.DWCode == ddlDW.SelectedValue).ToList();
                    string strTmp = string.Empty;
                    DropDownList ddlSB = (DropDownList)e.Item.FindControl("ddlSB") as DropDownList;
                    if (lstResult.Count > 0)
                    {
                        strTmp = lstResult[0].DMZCode;
                        GroundResource oGR = new GroundResource();
                        oGR.DMZCode = strTmp;
                        ddlSB.DataSource = oGR.SelectByDMZCode();
                        ddlSB.DataTextField = "EQUIPMENTNAME";
                        ddlSB.DataValueField = "EQUIPMENTCODE";
                        ddlSB.DataBind();
                    }
                    if (!string.IsNullOrEmpty(g.SB))
                    {
                        if (ddlSB.Items.Count > 0)
                            ddlSB.SelectedValue = g.SB;
                    }
                    //工作方式
                    DropDownList ddlFS = (DropDownList)e.Item.FindControl("ddlFS") as DropDownList;
                    ddlFS.DataSource = PlanParameters.ReadParameters("DJZYSQFS");
                    ddlFS.DataTextField = "Text";
                    ddlFS.DataValueField = "Value";
                    ddlFS.DataBind();
                    //计划性质
                    DropDownList ddlJXZ = (DropDownList)e.Item.FindControl("ddlJXZ") as DropDownList;
                    ddlJXZ.DataSource = PlanParameters.ReadParameters("DJZYSQSXZ");
                    ddlJXZ.DataTextField = "Text";
                    ddlJXZ.DataValueField = "Value";
                    ddlJXZ.DataBind();
                    //设备工作模式
                    DropDownList ddlMS = (DropDownList)e.Item.FindControl("ddlMS") as DropDownList;
                    ddlMS.DataSource = PlanParameters.ReadParameters("GZJHMS");
                    ddlMS.DataTextField = "Text";
                    ddlMS.DataValueField = "Value";
                    ddlMS.DataBind();
                    //圈标
                    DropDownList ddlQB = (DropDownList)e.Item.FindControl("ddlQB") as DropDownList;
                    ddlQB.DataSource = PlanParameters.ReadParameters("GZJHQB");
                    ddlQB.DataTextField = "Text";
                    ddlQB.DataValueField = "Value";
                    ddlQB.DataBind();
                    //工作性质
                    DropDownList ddlGXZ = (DropDownList)e.Item.FindControl("ddlGXZ") as DropDownList;
                    ddlGXZ.DataSource = PlanParameters.ReadParameters("GZJHGXZ");
                    ddlGXZ.DataTextField = "Text";
                    ddlGXZ.DataValueField = "Value";
                    ddlGXZ.DataBind();
                    //信息类别标志
                    DropDownList ddlBID = (DropDownList)e.Item.FindControl("ddlBID") as DropDownList;
                    ddlBID.DataSource = PlanParameters.ReadParameters("GZJHBID");
                    ddlBID.DataTextField = "Text";
                    ddlBID.DataValueField = "Value";
                    ddlBID.DataBind();
                    //信息类别标志
                    DropDownList ddlSHBID = (DropDownList)e.Item.FindControl("ddlSHBID") as DropDownList;
                    ddlSHBID.DataSource = PlanParameters.ReadParameters("GZJHBID");
                    ddlSHBID.DataTextField = "Text";
                    ddlSHBID.DataValueField = "Value";
                    ddlSHBID.DataBind();
                    #endregion
                    #region 绑定下拉列表值
                    if (!string.IsNullOrEmpty(g.FS))
                    {
                        ddlFS.SelectedValue = g.FS;
                    }
                    if (!string.IsNullOrEmpty(g.JXZ))
                    {
                        ddlJXZ.SelectedValue = g.JXZ;
                    }
                    
                    if (!string.IsNullOrEmpty(g.MS))
                    {
                        ddlMS.SelectedValue = g.MS;
                    }
                    if (!string.IsNullOrEmpty(g.QB))
                    {
                        ddlQB.SelectedValue = g.QB;
                    }
                    if (!string.IsNullOrEmpty(g.GXZ))
                    {
                        ddlGXZ.SelectedValue = g.GXZ;
                    }
                    if (!string.IsNullOrEmpty(g.BID))
                    {
                        ddlBID.SelectedValue = g.BID;
                    }
                    if (!string.IsNullOrEmpty(g.HBID))
                    {
                        ddlSHBID.SelectedValue = g.HBID;
                    }
                    #endregion
                    #region 注册脚本事件

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

                    TextBox txtTransStartTime = (TextBox)e.Item.FindControl("txtTransStartTime");
                    TextBox txtTransEndTime = (TextBox)e.Item.FindControl("txtTransEndTime");
                    //TextBox txtStartTime = (TextBox)((RepeaterItem)(e.Item.NamingContainer.NamingContainer)).FindControl("txtStartTime");
                    //TextBox txtEndTime = (TextBox)((RepeaterItem)(e.Item.NamingContainer.NamingContainer)).FindControl("txtEndTime");
                    txtTransStartTime.Attributes.Add("onblur", "return CheckTransTimeRang(this,'"
                        + txtStartTime.ClientID + "','" + txtEndTime.ClientID + "')");
                    txtTransEndTime.Attributes.Add("onblur", "return CheckTransTimeRang(this,'"
                        + txtStartTime.ClientID + "','" + txtEndTime.ClientID + "')");
                    #endregion
                }
            }
            catch (Exception ex)
            {
                throw (new AspNetException("绑定地面计划信息出现异常，异常原因", ex));
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
            //lblUpload.Visible = true;

            #region 读取文件内容
            StationInOutFileReader reader = new StationInOutFileReader();
            List<StationInOut> list;
            try
            {
                list = reader.Read(filename);
                rpStation.DataSource = list;
                rpStation.DataBind();
            }
            catch (Exception ex)
            {
                throw (new AspNetException("新建总参GZJH，载入StationInOut文件出现异常，异常原因", ex));
            }
            finally { }
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

        /// <summary>
        /// 导入航捷进出站数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnGetStationData_Click(object sender, EventArgs e)
        {
            GZJH ojh =null;
            string filepath = hfStationFile.Value;  //文件路径
            string ids = txtIds.Text;   //行号

            PlanProcessor pp = new PlanProcessor();
            try
            {
                pp.AddSIOtoZCDMZJH(ref ojh, filepath, ids);
                rpDatas.DataSource = ojh.GZJHContents;
                rpDatas.DataBind();
            }
            catch (Exception ex)
            {
                throw (new AspNetException("新建总参GZJH，加入StationInOut文件出现异常，异常原因", ex));
            }
            finally { }
            System.IO.File.Delete(filepath);    //删除临时文件
        }

        private GZJH GetJHFromPage(string from)
        {
            string taskID = string.Empty;
            string satID = string.Empty;
            GZJH obj = new GZJH();
            try
            {
                isTempJH = GetIsTempJHValue();
                new Task().GetTaskNoSatID(ddlMutiSatTask.SelectedValue, out taskID, out satID);
                obj.SatID = satID;
                obj.TaskID = taskID;
                obj.JXH = txtJXH.Text;  //修改时用，读取原来的序号
                obj.XXFL = ddlXXFL.SelectedValue;
                obj.GZJHContents = new List<GZJH_Content>();
            }
            catch (Exception ex)
            {
                throw (new AspNetException(from + "计划，获取基本信息出现异常，异常原因", ex));
            }

            obj.GZJHContents = GetRPContents(from, rpDatas, -1);
            return obj;
        }

        private List<GZJH_Content> GetRPContents(string from, Repeater rp, int idx)
        {
            List<GZJH_Content> list = new List<GZJH_Content>();
            GZJH_Content co;
            try
            {
                foreach (RepeaterItem it in rp.Items)
                {
                    if (idx != it.ItemIndex)
                    {
                        #region 赋值
                        co = new GZJH_Content();
                        DropDownList ddlTask = (DropDownList)it.FindControl("ddlTask");
                        DropDownList ddlDW = (DropDownList)it.FindControl("ddlDW");
                        DropDownList ddlSB = (DropDownList)it.FindControl("ddlSB");
                        DropDownList ddlFS = (DropDownList)it.FindControl("ddlFS");
                        DropDownList ddlJXZ = (DropDownList)it.FindControl("ddlJXZ");
                        DropDownList ddlMS = (DropDownList)it.FindControl("ddlMS");
                        DropDownList ddlQB = (DropDownList)it.FindControl("ddlQB");
                        DropDownList ddlGXZ = (DropDownList)it.FindControl("ddlGXZ");
                        TextBox txtPreStartTime = (TextBox)it.FindControl("txtPreStartTime");
                        TextBox txtQH = (TextBox)it.FindControl("txtQH");
                        TextBox txtTrackStartTime = (TextBox)it.FindControl("txtTrackStartTime");
                        TextBox txtTrackEndTime = (TextBox)it.FindControl("txtTrackEndTime");
                        TextBox txtWaveOnStartTime = (TextBox)it.FindControl("txtWaveOnStartTime");
                        TextBox txtWaveOffStartTime = (TextBox)it.FindControl("txtWaveOffStartTime");
                        TextBox txtStartTime = (TextBox)it.FindControl("txtStartTime");
                        TextBox txtEndTime = (TextBox)it.FindControl("txtEndTime");
                        DropDownList ddlBID = (DropDownList)it.FindControl("ddlBID");
                        TextBox txtJSBZ = (TextBox)it.FindControl("txtJSBZ");
                        TextBox txtTransStartTime = (TextBox)it.FindControl("txtTransStartTime");
                        TextBox txtTransEndTime = (TextBox)it.FindControl("txtTransEndTime");
                        TextBox txtTransSpeedRate = (TextBox)it.FindControl("txtTransSpeedRate");
                        TextBox txtHBZ = (TextBox)it.FindControl("txtHBZ");
                        TextBox DataStartTime = (TextBox)it.FindControl("DataStartTime");
                        TextBox DataEndTime = (TextBox)it.FindControl("DataEndTime");
                        DropDownList ddlSHBID = (DropDownList)it.FindControl("ddlSHBID");
                        TextBox txtSHTransStartTime = (TextBox)it.FindControl("txtSHTransStartTime");
                        TextBox txtHSL = (TextBox)it.FindControl("txtHSL");

                        co.DW = ddlDW.SelectedValue;
                        co.SB = ddlSB.SelectedValue;
                        co.DH = ddlTask.SelectedValue;
                        co.FS = ddlFS.SelectedItem.Value;
                        co.JXZ = ddlJXZ.SelectedItem.Value;
                        co.MS = ddlMS.SelectedItem.Value;
                        co.QB = ddlQB.SelectedItem.Value;
                        co.GXZ = ddlGXZ.SelectedItem.Value;
                        co.ZHB = txtPreStartTime.Text;
                        co.QH = txtQH.Text;
                        co.GZK = txtTrackStartTime.Text;
                        co.GZJ = txtTrackEndTime.Text;
                        co.KSHX = txtWaveOnStartTime.Text;
                        co.GSHX = txtWaveOffStartTime.Text;
                        co.RK = txtStartTime.Text;
                        co.JS = txtEndTime.Text;
                        co.BID = ddlBID.SelectedItem.Value;
                        co.SBZ = txtJSBZ.Text;
                        co.RTs = txtTransStartTime.Text;
                        co.RTe = txtTransEndTime.Text;
                        co.SL = txtTransSpeedRate.Text;
                        co.HBID = ddlSHBID.SelectedItem.Value;
                        co.HBZ = txtHBZ.Text;
                        co.Ts = DataStartTime.Text;
                        co.Te = DataEndTime.Text;
                        co.HRTs = txtSHTransStartTime.Text;
                        co.HSL = txtHSL.Text;
                        #endregion
                        list.Add(co);
                    }
                }
            }
            catch (Exception ex)
            {
                throw (new AspNetException(from + "计划，获取计划信息出现异常，异常原因", ex));
            }
            return list;
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
            DropDownList ddlSB = rpi.FindControl("ddlSB") as DropDownList;

            List<DMZ> lstResult = new DMZ().Cache.Where(t => t.DWCode == ddlDW.SelectedValue).ToList();
            string strTmp = string.Empty;
            if (lstResult.Count > 0)
            {
                strTmp = lstResult[0].DMZCode;
                GroundResource oGR = new GroundResource();
                oGR.DMZCode = strTmp;
                ddlSB.DataSource = oGR.SelectByDMZCode();
                ddlSB.DataTextField = "EQUIPMENTNAME";
                ddlSB.DataValueField = "EQUIPMENTCODE";
                ddlSB.DataBind();
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
            hfTaskID.Value = ddlMutiSatTask.SelectedValue;
        }

        private void ShowMsg(string msg)
        {
            trMessage.Visible = true;
            ltMessage.Text = msg;
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
                ShowMsg(success, (success?"计划确认成功":"计划确认失败"));
            }
        }

        protected void btnCreateFile_Click(object sender, EventArgs e)
        {
            string SendingFilePaths = string.Empty;
            PlanFileCreator creater = new PlanFileCreator();
            HideMsg();
            divFiles.Visible = false;
            try
            {
                string targets = string.Empty;
                SendingFilePaths = creater.CreateSendingGZJHFile(HfID.Value, out targets, true);
                lblFilePath.Text = SendingFilePaths;
                lbtFilePath_Click(null, null);
                //ShowMsg("文件生成成功。");
                //divFiles.Visible = true;
            }
            catch (Exception ex)
            {
                throw new AspNetException("地面站计划-生成文件出现异常，异常原因", ex);
            }
        }

        protected void lbtFilePath_Click(object sender, EventArgs e)
        {
            try
            {
                string strFilePath = lblFilePath.Text.Trim();
                if (string.IsNullOrEmpty(strFilePath) || !System.IO.File.Exists(strFilePath))
                {
                    ShowMsg("文件不存在。");
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
                throw (new AspNetException("地面站计划-生成文件出现异常，异常原因", ex));
            }
        }
    }
}
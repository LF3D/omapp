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
using OperatingManagement.DataAccessLayer.BusinessManage;
using OperatingManagement.DataAccessLayer.PlanManage;
using OperatingManagement.Framework;
using System.Web.Security;
using System.Xml;
using ServicesKernel.File;

namespace OperatingManagement.Web.Views.PlanManage
{
    public partial class TYSJEdit : AspNetPage, IRouteContext
    {
        bool isTempJH = false;  //是否为临时计划,默认为正式计划

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["view"] == "1")
                    this.IsViewOrEdit = true; 
                btnFormal.Visible = false; 
                //txtStartTime.Attributes.Add("readonly", "true");
                //txtEndTime.Attributes.Add("readonly", "true");
                //ddlSatName_SelectedIndexChanged(null, null);
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
                    hfURL.Value = "?type=TYSJ&startDate=" + Request.QueryString["startDate"] + "&endDate=" + Request.QueryString["endDate"]
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

        public void initial()
        {
            try
            {
                List<TYSJ_Content> list = new List<TYSJ_Content>();

                list.Add(new TYSJ_Content());
                rpData.DataSource = list;
                rpData.DataBind();
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
                HfFileIndex.Value = jh[0].FileIndex;
                hfTaskID.Value = jh[0].TaskID.ToString();
                string outTaskNo = new Task().GetOutTaskNo(jh[0].TaskID, jh[0].SatID);
                if (!outTaskNo.Equals(string.Empty))
                    ucTask1.SelectedValue = outTaskNo;
                txtJXH.Text = jh[0].PlanID.ToString("0000");
                hfSatID.Value = jh[0].SatID;
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
                List<TYSJ_Content> list = new List<TYSJ_Content>();
                TYSJ_Content ct;
                CultureInfo provider = CultureInfo.InvariantCulture;
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(HfFileIndex.Value);
                XmlNode root = xmlDoc.SelectSingleNode("仿真推演试验数据");
                foreach (XmlNode n in root.ChildNodes)
                {
                    if (n.Name == "Content")
                    {
                        ct = new TYSJ_Content();
                        ct.SatName = n["SatName"].InnerText;
                        ct.Type = n["Type"].InnerText;
                        ct.TestItem = n["TestItem"].InnerText;
                        ct.StartTime = n["StartTime"].InnerText;
                        ct.EndTime = n["EndTime"].InnerText;
                        ct.Condition = n["Condition"].InnerText;
                        list.Add(ct);
                    }
                }
                rpData.DataSource = list;
                rpData.DataBind();
            }
            catch (Exception ex)
            {
                throw (new AspNetException("绑定仿真推演试验计划信息出现异常，异常原因", ex));
            }
            finally { }
        }
        public override void OnPageLoaded()
        {
            this.PagePermission = "OMPLAN_Plan.Edit";
            this.ShortTitle = "编辑计划";
            base.OnPageLoaded();
            this.AddJavaScriptInclude("scripts/pages/PlanManage/TYSJEdit.aspx.js");
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                isTempJH = GetIsTempJHValue();

                TYSJ objTYSJ = new TYSJ();
                objTYSJ.SYContents = new List<TYSJ_Content>();
                TYSJ_Content sc;
                DateTime startTime = new DateTime(); //用来保存最小开始时间
                DateTime endTime = new DateTime();  //用来保存最大结束时间
                
                objTYSJ.TaskID = ucTask1.SelectedItem.Value;
                objTYSJ.SatID = System.Configuration.ConfigurationManager.AppSettings["TYSJSatID"];
                
                CultureInfo provider = CultureInfo.InvariantCulture;
                #region Content
                foreach (RepeaterItem it in rpData.Items)
                {
                    sc = new TYSJ_Content();
                    DropDownList ddlSatName = (DropDownList)it.FindControl("ddlSatName");
                    DropDownList ddlType = (DropDownList)it.FindControl("ddlType");
                    DropDownList ddlTestItem = (DropDownList)it.FindControl("ddlTestItem");
                    TextBox txtStartTime = (TextBox)it.FindControl("txtStartTime");
                    TextBox txtEndTime = (TextBox)it.FindControl("txtEndTime");
                    TextBox txtCondition = (TextBox)it.FindControl("txtCondition");

                    if (it.ItemIndex == 0)
                    {
                        startTime = DateTime.ParseExact(txtStartTime.Text, "yyyyMMddHHmmss", provider);
                        endTime = DateTime.ParseExact(txtEndTime.Text, "yyyyMMddHHmmss", provider);
                    }
                    else
                    {
                        if (startTime > DateTime.ParseExact(txtStartTime.Text, "yyyyMMddHHmmss", provider))
                        { startTime = DateTime.ParseExact(txtStartTime.Text, "yyyyMMddHHmmss", provider); }
                        if (endTime < DateTime.ParseExact(txtEndTime.Text, "yyyyMMddHHmmss", provider))
                        { endTime = DateTime.ParseExact(txtEndTime.Text, "yyyyMMddHHmmss", provider); }
                    }
                    sc.SatName = ddlSatName.SelectedItem.Text;
                    sc.Type = ddlType.SelectedItem.Text;
                    sc.TestItem = ddlTestItem.SelectedItem.Text;
                    sc.StartTime = txtStartTime.Text;
                    sc.EndTime = txtEndTime.Text;
                    sc.Condition = txtCondition.Text;
                    objTYSJ.SYContents.Add(sc);
                }
                #endregion
                PlanFileCreator creater = new PlanFileCreator(isTempJH);
                if (hfStatus.Value == "new")
                {
                    string filepath = creater.CreateTYSJFile(objTYSJ, 0);
                    DataAccessLayer.PlanManage.JH jh = new DataAccessLayer.PlanManage.JH(isTempJH)
                    {
                        TaskID = objTYSJ.TaskID,
                        PlanType = "TYSJ",
                        PlanID = (new Sequence()).GetTYSJSequnce(),
                        //StartTime = DateTime.ParseExact(objTYSJ.StartTime, "yyyyMMddHHmmss", provider),
                        //EndTime = DateTime.ParseExact(objTYSJ.EndTime, "yyyyMMddHHmmss", provider),
                        StartTime = startTime,
                        EndTime = endTime,
                        SRCType = 0,
                        FileIndex = filepath,
                        SatID = objTYSJ.SatID,
                        Reserve = txtNote.Text
                    };
                    var result = jh.Add();
                    txtJXH.Text = jh.PlanID.ToString("0000");
                }
                else
                {
                    //当任务和卫星更新时，需要更新文件名称
                    if (hfSatID.Value != objTYSJ.SatID || hfTaskID.Value != ucTask1.SelectedValue)
                        //if (hfSatID.Value != ucSatellite1.SelectedValue || hfTaskID.Value != ucTask1.SelectedValue)
                    {
                        string filepath = creater.CreateTYSJFile(objTYSJ, 0);

                        DataAccessLayer.PlanManage.JH jh = new DataAccessLayer.PlanManage.JH(isTempJH)
                        {
                            Id = Convert.ToInt32(HfID.Value),
                            TaskID = objTYSJ.TaskID,
                            StartTime = startTime,
                            EndTime = endTime,
                            FileIndex = filepath,
                            SatID = objTYSJ.SatID,
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
                        creater.CreateTYSJFile(objTYSJ, 1);
                        DataAccessLayer.PlanManage.JH jh = new DataAccessLayer.PlanManage.JH(isTempJH)
                        {
                            Id = Convert.ToInt32(HfID.Value),
                            SENDSTATUS = 0,
                            USESTATUS = 0
                        };
                        var result = jh.UpdateStatus();
                    }
                }

                trMessage.Visible = true;
                ltMessage.Text = "计划保存成功";
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
                isTempJH = GetIsTempJHValue();

                TYSJ objTYSJ = new TYSJ();
                objTYSJ.SYContents = new List<TYSJ_Content>();
                TYSJ_Content sc;
                DateTime startTime = new DateTime(); //用来保存最小开始时间
                DateTime endTime = new DateTime();  //用来保存最大结束时间

                objTYSJ.TaskID = ucTask1.SelectedItem.Value;
                objTYSJ.SatID = System.Configuration.ConfigurationManager.AppSettings["TYSJSatID"];

                CultureInfo provider = CultureInfo.InvariantCulture;
                #region SYContent
                foreach (RepeaterItem it in rpData.Items)
                {
                    sc = new TYSJ_Content();
                    DropDownList ddlSatName = (DropDownList)it.FindControl("ddlSatName");
                    DropDownList ddlType = (DropDownList)it.FindControl("ddlType");
                    DropDownList ddlTestItem = (DropDownList)it.FindControl("ddlTestItem");
                    TextBox txtWC_SYID = (TextBox)it.FindControl("txtWC_SYID");
                    TextBox txtStartTime = (TextBox)it.FindControl("txtStartTime");
                    TextBox txtEndTime = (TextBox)it.FindControl("txtEndTime");
                    TextBox txtCondition = (TextBox)it.FindControl("txtCondition");

                    if (it.ItemIndex == 0)
                    {
                        startTime = DateTime.ParseExact(txtStartTime.Text, "yyyyMMddHHmmss", provider);
                        endTime = DateTime.ParseExact(txtEndTime.Text, "yyyyMMddHHmmss", provider);
                    }
                    else
                    {
                        if (startTime > DateTime.ParseExact(txtStartTime.Text, "yyyyMMddHHmmss", provider))
                        { startTime = DateTime.ParseExact(txtStartTime.Text, "yyyyMMddHHmmss", provider); }
                        if (endTime < DateTime.ParseExact(txtEndTime.Text, "yyyyMMddHHmmss", provider))
                        { endTime = DateTime.ParseExact(txtEndTime.Text, "yyyyMMddHHmmss", provider); }
                    }
                    sc.SatName = ddlSatName.SelectedItem.Text;
                    sc.Type = ddlType.SelectedItem.Text;
                    sc.TestItem = ddlTestItem.SelectedItem.Text;
                    sc.StartTime = txtStartTime.Text;
                    sc.EndTime = txtEndTime.Text;
                    sc.Condition = txtCondition.Text;
                    objTYSJ.SYContents.Add(sc);
                }
                #endregion
                PlanFileCreator creater = new PlanFileCreator(isTempJH);
                int planid = (new Sequence()).GetTYSJSequnce();

                //检查文件是否已经存在
                if (creater.TestTYSJFileName(objTYSJ))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "File", "<script type='text/javascript'>showMsg('存在同名文件，请一分钟后重试');</script>");
                    return;
                }
                string filepath = creater.CreateTYSJFile(objTYSJ, 0);

                DataAccessLayer.PlanManage.JH jh = new DataAccessLayer.PlanManage.JH(isTempJH)
                {
                    TaskID = objTYSJ.TaskID,
                    PlanType = "TYSJ",
                    PlanID = planid,
                    StartTime = startTime,
                    EndTime = endTime,
                    SRCType = 0,
                    FileIndex = filepath,
                    SatID = objTYSJ.SatID,
                    Reserve = txtNote.Text
                };
                var result = jh.Add();

                txtJXH.Text = planid.ToString("0000");
                trMessage.Visible = true;
                ltMessage.Text = "计划保存成功";
                //ClientScript.RegisterStartupScript(this.GetType(), "OK", "<script type='text/javascript'>showMsg('计划保存成功');</script>");
            }
            catch (Exception ex)
            {
                throw (new AspNetException("另存计划信息出现异常，异常原因", ex));
            }
            finally { }
        }

        /// <summary>
        /// 卫星名称改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlSatName_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlSatName = sender as DropDownList;
            //Repeater rp = ddlSatName.Parent.Parent as Repeater;
            //int n = ((RepeaterItem)ddlSatName.Parent).ItemIndex;
            //DropDownList ddlType = rp.Items[n].FindControl("ddlType") as DropDownList;
            RepeaterItem rpi = (RepeaterItem)ddlSatName.Parent;
            DropDownList ddlType = rpi.FindControl("ddlType") as DropDownList;

            switch (ddlSatName.SelectedItem.Text)
            {
                case "探索三号卫星":
                    ddlType.Items.Clear();
                    ddlType.Items.Add(new ListItem("GEO目标观测试验"));
                    ddlType.Items.Add(new ListItem("LEO目标成像试验"));
                    ddlType_SelectedIndexChanged(ddlType, null);
                    break;
                case "探索四号卫星":
                    ddlType.Items.Clear();
                    ddlType.Items.Add(new ListItem("释放抓捕目标试验"));
                    ddlType.Items.Add(new ListItem("逼近停靠试验"));
                    ddlType.Items.Add(new ListItem("遥操作试验"));
                    ddlType_SelectedIndexChanged(ddlType, null);
                    break;
                case "探索五号卫星":
                    ddlType.Items.Clear();
                    ddlType.Items.Add(new ListItem("远程自主快速机动试验"));
                    ddlType.Items.Add(new ListItem("TS-5-B卫星在轨施放试验"));
                    ddlType_SelectedIndexChanged(ddlType, null);
                    break;
            }
        }
        /// <summary>
        /// 试验类别改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddlType = sender as DropDownList;
            //Repeater rp = ddlType.Parent.Parent as Repeater;
            //int n = ((RepeaterItem)ddlType.Parent).ItemIndex;
            //DropDownList ddlTestItem = rp.Items[n].FindControl("ddlTestItem") as DropDownList;
            RepeaterItem rpi = (RepeaterItem)ddlType.Parent;
            DropDownList ddlTestItem = rpi.FindControl("ddlTestItem") as DropDownList;

            switch (ddlType.SelectedItem.Text)
            {
                case "GEO目标观测试验":
                    ddlTestItem.Items.Clear();
                    ddlTestItem.Items.Add(new ListItem("自然交会观测试验"));
                    ddlTestItem.Items.Add(new ListItem("同步带凝视观测试验"));
                    ddlTestItem.Items.Add(new ListItem("天地基联合观测试验"));
                    ddlTestItem.Items.Add(new ListItem("其它扩展试验项目"));
                    break;
                case "LEO目标成像试验":
                    ddlTestItem.Items.Clear();
                    ddlTestItem.Items.Add(new ListItem("等待成像试验"));
                    ddlTestItem.Items.Add(new ListItem("自动跟踪成像试验"));
                    ddlTestItem.Items.Add(new ListItem("引导跟踪成像试验"));
                    ddlTestItem.Items.Add(new ListItem("其它扩展试验项目"));
                    break;
                case "释放抓捕目标试验":
                    ddlTestItem.Items.Clear();
                    ddlTestItem.Items.Add(new ListItem("空间机械手性能试验"));
                    ddlTestItem.Items.Add(new ListItem("空间机械手协调控制试验"));
                    ddlTestItem.Items.Add(new ListItem("空间机械手辅助分离试验"));
                    ddlTestItem.Items.Add(new ListItem("释放抓捕试验"));
                    ddlTestItem.Items.Add(new ListItem("其它扩展试验项目"));
                    break;
                case "逼近停靠试验":
                    ddlTestItem.Items.Clear();
                    ddlTestItem.Items.Add(new ListItem("合作目标逼近停靠试验"));
                    ddlTestItem.Items.Add(new ListItem("非合作目标跟踪接近试验"));
                    ddlTestItem.Items.Add(new ListItem("其它扩展试验项目"));
                    break;
                case "遥操作试验":
                    ddlTestItem.Items.Clear();
                    ddlTestItem.Items.Add(new ListItem("遥编程遥操作试验"));
                    ddlTestItem.Items.Add(new ListItem("主从遥操作试验"));
                    ddlTestItem.Items.Add(new ListItem("ORU更换模拟试验"));
                    ddlTestItem.Items.Add(new ListItem("其它扩展试验项目"));
                    break;
                case "远程自主快速机动试验":
                    ddlTestItem.Items.Clear();
                    ddlTestItem.Items.Add(new ListItem("自主调相轨控试验"));
                    ddlTestItem.Items.Add(new ListItem("闭环反馈轨控试验"));
                    ddlTestItem.Items.Add(new ListItem("远近程交班试验"));
                    ddlTestItem.Items.Add(new ListItem("近程接近伴飞试验"));
                    ddlTestItem.Items.Add(new ListItem("其它扩展试验项目"));
                    break;
                case "TS-5-B卫星在轨施放试验":
                    ddlTestItem.Items.Clear();
                    ddlTestItem.Items.Add(new ListItem("动基座惯性基准传递试验"));
                    ddlTestItem.Items.Add(new ListItem("在轨施放TS-5-B卫星试验"));
                    ddlTestItem.Items.Add(new ListItem("TS-5-B卫星捕获飞行试验"));
                    ddlTestItem.Items.Add(new ListItem("TS-5-B卫星离轨控制试验"));
                    ddlTestItem.Items.Add(new ListItem("其它扩展试验项目"));
                    break;
            }
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
                TYSJ objTYSJ = new TYSJ();
                objTYSJ.SYContents = new List<TYSJ_Content>();
                TYSJ_Content sc;
                DateTime startTime = new DateTime(); //用来保存最小开始时间
                DateTime endTime = new DateTime();  //用来保存最大结束时间

                objTYSJ.TaskID = ucTask1.SelectedItem.Value;
                objTYSJ.SatID = System.Configuration.ConfigurationManager.AppSettings["TYSJSatID"];

                CultureInfo provider = CultureInfo.InvariantCulture;
                #region Content
                foreach (RepeaterItem it in rpData.Items)
                {
                    sc = new TYSJ_Content();
                    DropDownList ddlSatName = (DropDownList)it.FindControl("ddlSatName");
                    DropDownList ddlType = (DropDownList)it.FindControl("ddlType");
                    DropDownList ddlTestItem = (DropDownList)it.FindControl("ddlTestItem");
                    TextBox txtStartTime = (TextBox)it.FindControl("txtStartTime");
                    TextBox txtEndTime = (TextBox)it.FindControl("txtEndTime");
                    TextBox txtCondition = (TextBox)it.FindControl("txtCondition");

                    if (it.ItemIndex == 0)
                    {
                        startTime = DateTime.ParseExact(txtStartTime.Text, "yyyyMMddHHmmss", provider);
                        endTime = DateTime.ParseExact(txtEndTime.Text, "yyyyMMddHHmmss", provider);
                    }
                    else
                    {
                        if (startTime > DateTime.ParseExact(txtStartTime.Text, "yyyyMMddHHmmss", provider))
                        { startTime = DateTime.ParseExact(txtStartTime.Text, "yyyyMMddHHmmss", provider); }
                        if (endTime < DateTime.ParseExact(txtEndTime.Text, "yyyyMMddHHmmss", provider))
                        { endTime = DateTime.ParseExact(txtEndTime.Text, "yyyyMMddHHmmss", provider); }
                    }
                    sc.SatName = ddlSatName.SelectedItem.Text;
                    sc.Type = ddlType.SelectedItem.Text;
                    sc.TestItem = ddlTestItem.SelectedItem.Text;
                    sc.StartTime = txtStartTime.Text;
                    sc.EndTime = txtEndTime.Text;
                    sc.Condition = txtCondition.Text;
                    objTYSJ.SYContents.Add(sc);
                }
                #endregion
                PlanFileCreator creater = new PlanFileCreator();

                int planid = (new Sequence()).GetTYSJSequnce();

                //检查文件是否已经存在
                if (creater.TestTYSJFileName(objTYSJ))
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "File", "<script type='text/javascript'>showMsg('存在同名文件，请一分钟后重试');</script>");
                    return;
                }
                string filepath = creater.CreateTYSJFile(objTYSJ, 0);

                DataAccessLayer.PlanManage.JH jh = new DataAccessLayer.PlanManage.JH()
                {
                    TaskID = objTYSJ.TaskID,
                    PlanType = "TYSJ",
                    PlanID = planid,
                    StartTime = startTime,
                    EndTime = endTime,
                    SRCType = 0,
                    FileIndex = filepath,
                    SatID = objTYSJ.SatID,
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

                #endregion

                txtJXH.Text = planid.ToString("0000");
                trMessage.Visible = true;
                ltMessage.Text = "计划保存成功";
               
            }
            catch (Exception ex)
            {
                throw (new AspNetException("另存计划信息出现异常，异常原因", ex));
            }
            finally { }
        }

        protected void rpData_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Add")
            {
                List<TYSJ_Content> list2 = new List<TYSJ_Content>();
                TYSJ_Content sc;
                Repeater rp = (Repeater)source;
                foreach (RepeaterItem it in rp.Items)
                {
                    sc = new TYSJ_Content();
                    DropDownList ddlSatName = (DropDownList)it.FindControl("ddlSatName");
                    DropDownList ddlType = (DropDownList)it.FindControl("ddlType");
                    DropDownList ddlTestItem = (DropDownList)it.FindControl("ddlTestItem");
                    TextBox txtStartTime = (TextBox)it.FindControl("txtStartTime");
                    TextBox txtEndTime = (TextBox)it.FindControl("txtEndTime");
                    TextBox txtCondition = (TextBox)it.FindControl("txtCondition");
                    if (CheckCtrlExits(ddlSatName, "卫星名称"))
                        sc.SatName = ddlSatName.SelectedItem.Text;
                    else
                        return;
                    if (CheckCtrlExits(ddlType, "试验类别"))
                        sc.Type = ddlType.SelectedItem.Text;
                    else
                        return;
                    if (CheckCtrlExits(ddlTestItem, "试验项目"))
                        sc.TestItem = ddlTestItem.SelectedItem.Text;
                    else
                        return;

                    if (CheckCtrlExits(txtStartTime, "试验开始时间"))
                        sc.StartTime = txtStartTime.Text;
                    else
                        return;

                    if (CheckCtrlExits(txtEndTime, "试验结束时间"))
                        sc.EndTime = txtEndTime.Text;
                    else
                        return;

                    if (CheckCtrlExits(txtCondition, "试验条件"))
                        sc.Condition = txtCondition.Text;
                    else
                        return;

                    list2.Add(sc);
                }
                sc = new TYSJ_Content();
                list2.Add(sc);
                rp.DataSource = list2;
                rp.DataBind();

            }
            if (e.CommandName == "Del")
            {
                List<TYSJ_Content> list2 = new List<TYSJ_Content>();
                TYSJ_Content sc;
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
                            sc = new TYSJ_Content();
                            DropDownList ddlSatName = (DropDownList)it.FindControl("ddlSatName");
                            DropDownList ddlType = (DropDownList)it.FindControl("ddlType");
                            DropDownList ddlTestItem = (DropDownList)it.FindControl("ddlTestItem");
                            TextBox txtStartTime = (TextBox)it.FindControl("txtStartTime");
                            TextBox txtEndTime = (TextBox)it.FindControl("txtEndTime");
                            TextBox txtCondition = (TextBox)it.FindControl("txtCondition");

                            sc.SatName = ddlSatName.SelectedItem.Text;
                            sc.Type = ddlType.SelectedItem.Text;
                            sc.TestItem = ddlTestItem.SelectedItem.Text;
                            sc.StartTime = txtStartTime.Text;
                            sc.EndTime = txtEndTime.Text;
                            sc.Condition = txtCondition.Text;
                            
                            list2.Add(sc);
                        }
                    }
                    rp.DataSource = list2;
                    rp.DataBind();
                }
            }
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

        protected void rpData_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                TYSJ_Content sc = (TYSJ_Content)e.Item.DataItem;
                DropDownList ddlSatName = (DropDownList)e.Item.FindControl("ddlSatName");
                if (null != sc && !string.IsNullOrEmpty(sc.SatName))
                {
                    ddlSatName.SelectedIndex = ddlSatName.Items.IndexOf(ddlSatName.Items.FindByText(sc.SatName));
                }
                ddlSatName_SelectedIndexChanged(ddlSatName, null);
                DropDownList ddlType = (DropDownList)e.Item.FindControl("ddlType");
                if (null != sc && !string.IsNullOrEmpty(sc.Type))
                {
                    ddlType.SelectedIndex = ddlType.Items.IndexOf(ddlType.Items.FindByText(sc.Type));
                }
                ddlType_SelectedIndexChanged(ddlType, null);
                DropDownList ddlTestItem = (DropDownList)e.Item.FindControl("ddlTestItem");
                if (null != sc && !string.IsNullOrEmpty(sc.TestItem))
                {
                    ddlTestItem.SelectedIndex = ddlTestItem.Items.IndexOf(ddlTestItem.Items.FindByText(sc.TestItem));
                }
            }
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
                trMessage.Visible = true;
                if (result == FieldVerifyResult.Success)
                    ltMessage.Text = "计划确认成功";
                else
                    ltMessage.Text = "计划确认失败";
            }
        }
    }
}
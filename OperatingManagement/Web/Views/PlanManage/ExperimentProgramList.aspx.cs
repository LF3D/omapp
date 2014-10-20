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
using OperatingManagement.Framework.Storage;
using OperatingManagement.DataAccessLayer.PlanManage;
using System.Web.Security;
using System.Data;
using ServicesKernel.File;
using OperatingManagement.ServicesKernel.File;


namespace OperatingManagement.Web.Views.PlanManage
{
    public partial class ExperimentProgramList : AspNetPage, IRouteContext
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtCStartDate.Attributes.Add("readonly", "true");
                txtCEndDate.Attributes.Add("readonly", "true");
                txtJHStartDate.Attributes.Add("readonly", "true");
                txtJHEndDate.Attributes.Add("readonly", "true");
                pnlAll2.Visible = false;
                DefaultSearch();
                HideMessage();
            }
            cpPager.PostBackPage += new EventHandler(cpPager_PostBackPage);
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                SaveCondition();
                cpPager.CurrentPage = 1;
                BindGridView(true);
                HideMessage();
            }
            catch (Exception ex)
            {
                throw (new AspNetException("实验程序列表页面搜索出现异常，异常原因", ex));
            }
            finally { }
        }

        private void SaveCondition()
        {
            if (string.IsNullOrEmpty(txtCStartDate.Text))
                ViewState["_StartDate"] = null;
            else
                ViewState["_StartDate"] = txtCStartDate.Text.Trim();
            if (string.IsNullOrEmpty(txtCEndDate.Text))
                ViewState["_EndDate"] = null;
            else
                ViewState["_EndDate"] = Convert.ToDateTime(txtCEndDate.Text.Trim()).AddDays(1).AddMilliseconds(-1);


            if (string.IsNullOrEmpty(txtJHStartDate.Text))
                ViewState["_JHStartDate"] = null;
            else
                ViewState["_JHStartDate"] = txtJHStartDate.Text.Trim();
            if (string.IsNullOrEmpty(txtJHEndDate.Text))
                ViewState["_JHEndDate"] = null;
            else
                ViewState["_JHEndDate"] = Convert.ToDateTime(txtJHEndDate.Text.Trim()).AddDays(1).AddMilliseconds(-1);
        }

        /// <summary>
        /// 默认查询两周内的数据
        /// </summary>
        private void DefaultSearch()
        {
            txtCStartDate.Text = DateTime.Now.AddDays(-6).ToString("yyyy-MM-dd");
            txtCEndDate.Text = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
        }

        //绑定列表
        void BindGridView(bool fromSearch)
        {
            DateTime startDate = new DateTime();
            DateTime endDate = new DateTime();
            DateTime jhStartDate = new DateTime();
            DateTime jhEndDate = new DateTime();
            if (fromSearch)
            {
                if (!string.IsNullOrEmpty(txtCStartDate.Text))
                    startDate = Convert.ToDateTime(txtCStartDate.Text);
                if (!string.IsNullOrEmpty(txtCEndDate.Text))
                    endDate = Convert.ToDateTime(txtCEndDate.Text).AddDays(1).AddMilliseconds(-1); //查询时可查当天

                if (!string.IsNullOrEmpty(txtJHStartDate.Text))
                    jhStartDate = Convert.ToDateTime(txtJHStartDate.Text);
                if (!string.IsNullOrEmpty(txtJHEndDate.Text))
                    jhEndDate = Convert.ToDateTime(txtJHEndDate.Text).AddDays(1).AddMilliseconds(-1);
            }
            else
            {
                if (ViewState["_StartDate"] != null)
                    startDate = Convert.ToDateTime(ViewState["_StartDate"].ToString());
                if (ViewState["_EndDate"] != null)
                    endDate = Convert.ToDateTime(ViewState["_EndDate"].ToString());

                if (ViewState["_JHStartDate"] != null)
                    jhStartDate = Convert.ToDateTime(ViewState["_JHStartDate"].ToString());
                if (ViewState["_JHEndDate"] != null)
                    jhEndDate = Convert.ToDateTime(ViewState["_JHEndDate"].ToString());
            }

            List<SYCX> listDatas = (new SYCX()).GetListByDate(startDate, endDate, jhStartDate, jhEndDate);
            cpPager.DataSource = listDatas;
            cpPager.PageSize = this.SiteSetting.PageSize;
            if (listDatas.Count > this.SiteSetting.PageSize)
                cpPager.Visible = true;
            cpPager.BindToControl = rpDatas;
            rpDatas.DataSource = cpPager.DataSourcePaged;
            rpDatas.DataBind();

            if (listDatas.Count > 0)
                pnlAll2.Visible = true;
            else
                pnlAll2.Visible = false;
        }

        protected void cpPager_PostBackPage(object sender, EventArgs e)
        {
            BindGridView(false);
            HideMessage();
        }

        /// <summary>
        /// 生成计划
        /// </summary>
        /// <param name="proid"></param>
        void CreatePlans(int proid)
        {
            try
            {
                HideMessage();
                string ConfigSYCXPath = System.Configuration.ConfigurationManager.AppSettings["SYCXPath"];
                string fileIndex = "";  //文件路径
                PlanFileCreator creater = new PlanFileCreator(false);
                SYCX objSYCX = new SYCX() { Id=proid };
                objSYCX = objSYCX.SelectById();
                fileIndex = ConfigSYCXPath +objSYCX.FileIndex.Substring(objSYCX.FileIndex.LastIndexOf("\\")+1);
                string savefilepath = "";

                string result="";
                PlanProcessor pp = new PlanProcessor();
                string xxfl = radBtnXXFL.SelectedValue;
                DateTime beginTime = Convert.ToDateTime(txtStartTime.Text);
                DateTime endTime = Convert.ToDateTime(txtEndTime.Text);
                FieldVerifyResult oResult;

                JH objJH = new JH(true);
                objJH.SRCType = 1; //试验程序
                objJH.SRCID = proid;
                objJH.StartTime = beginTime;
                objJH.EndTime = endTime;

                #region  地面站工作计划
                List<GZJH> gjhs= pp.SYCXFile2ZCDMZGZJHs(fileIndex, xxfl, beginTime, endTime, out result);
                if (!result.Equals(string.Empty))
                {
                    ShowMessage(string.Format("生成总参地面站工作计划出错，原因：<br>{0}", result), false);
                    //return;
                }
                if (gjhs.Count > 0)
                {
                    objJH.PlanType = "GZJH";
                    for (int i = 0; i < gjhs.Count(); i++)
                    {
                        objJH.PlanID = (new Sequence()).GetGZJHSequnce();
                        gjhs[i].JXH = objJH.PlanID.ToString();

                        savefilepath = creater.CreateGZJHFile(gjhs[i], 0);

                        objJH.TaskID = gjhs[i].TaskID;
                        objJH.FileIndex = savefilepath;
                        objJH.SatID = gjhs[i].SatID;
                        oResult = objJH.Add();
                        if (oResult != FieldVerifyResult.Success)
                            ShowMessage(string.Format("生成的地面站工作计划保存失败，原因：{0}", oResult.ToString()), true);
                    }
                }
                else
                {
                    ShowMessage("未能生成地面站工作计划<br>", true);
                    //return;
                }
                #endregion

                #region  中心运行计划
                ZXJH zjh = pp.SYCXFile2ZXJH(fileIndex, xxfl, beginTime, endTime, out result);
                if (!result.Equals(string.Empty))
                {
                    ShowMessage(string.Format("生成中心运行计划出错，原因：<br>{0}", result), true);
                    //return;
                }

                if (null != zjh && zjh.SYContents != null)
                {
                    savefilepath = creater.CreateZXJHFile(zjh, 0);

                    objJH.PlanType = "ZXJH";
                    objJH.PlanID = (new Sequence()).GetZXJHSequnce();
                    objJH.TaskID = zjh.TaskID;
                    objJH.FileIndex = savefilepath;
                    objJH.SatID = zjh.SatID;
                    oResult = objJH.Add();
                    if (oResult != FieldVerifyResult.Success)
                        ShowMessage(string.Format("生成的中心工作计划保存失败，原因：{0}", oResult.ToString()), true);
                }
                else
                {
                    ShowMessage("未能生成中心运行计划<br>", true);
                    //return;
                }
                #endregion

                #region 测控资源申请
                List<DJZYSQ> djhs = pp.SYCXFile2CKZYSYSQ(fileIndex, xxfl, beginTime, endTime, out result);
                if (!result.Equals(string.Empty))
                {
                    ShowMessage(string.Format("生成测控资源申请出错，原因：<br>{0}", result), true);
                    //return;
                }
                if (djhs.Count > 0)
                {
                    objJH.PlanType = "DJZYSQ";
                    for (int i = 0; i < djhs.Count(); i++)
                    {
                        objJH.PlanID = (new Sequence()).GetDJZYSQSequnce();
                        djhs[i].SNO = objJH.PlanID.ToString();
                        savefilepath = creater.CreateDMJHFile(djhs[i], 0);

                        objJH.TaskID = djhs[i].TaskID;
                        objJH.FileIndex = savefilepath;
                        objJH.SatID = djhs[i].SatID;
                        oResult = objJH.Add();
                        if (oResult != FieldVerifyResult.Success)
                            ShowMessage(string.Format("生成的测控资源使用申请保存失败，原因：{0}", oResult.ToString()), true);
                    }
                }
                else
                {
                    ShowMessage("未能生成测控资源使用申请", true);
                    return;
                }

                #endregion

                #region 应用研究计划
                List<YJJH> yjhs = pp.SYCXFile2YJJH(fileIndex, xxfl, beginTime, endTime, out result);
                if (!result.Equals(string.Empty))
                {
                    ShowMessage(string.Format("生成应用研究计划出错，原因：<br>{0}", result), true);
                }
                if (yjhs.Count > 0)
                {
                    objJH.PlanType = "YJJH";
                    for (int i = 0; i < yjhs.Count(); i++)
                    {
                        objJH.PlanID = (new Sequence()).GetYJJHSequnce();
                        yjhs[i].JXH = objJH.PlanID.ToString();
                        savefilepath = creater.CreateYJJHFile(yjhs[i], 0);
                        yjhs[i].SatID = "AAAA";
                        objJH.TaskID = yjhs[i].TaskID;
                        objJH.FileIndex = savefilepath;
                        objJH.SatID = yjhs[i].SatID;
                        oResult = objJH.Add();
                        if (oResult != FieldVerifyResult.Success)
                            ShowMessage(string.Format("生成的应用研究计划保存失败，原因：{0}", oResult.ToString()), true);
                    }
                }
                else
                {
                    ShowMessage("未能生成应用研究计划", true);
                    return;
                }

                #endregion
                ShowMessage("计划生成成功。", false);
            }
            catch (Exception ex)
            {
                throw (new AspNetException("生成计划出现异常，异常原因", ex));
            }
            finally { }
        }

        public override void OnPageLoaded()
        {
            this.PagePermission = "OMPLAN_ExProgram.View";
            this.ShortTitle = "生成初步计划";
            this.SetTitle();
            this.AddJavaScriptInclude("scripts/pages/PlanManage/ExperimentProgramList.aspx.js");
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect(Request.CurrentExecutionFilePath);
            HideMessage();
        }

        protected void btnCreatePlan_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txtID.Text);
            CreatePlans(id);
            BindGridView(true);
        }

        private void ShowMessage(string msg, bool join)
        {
            trMessage.Visible = true;
            if (join)
                ltMessage.Text += msg;
            else
                ltMessage.Text = msg;
        }

        private void HideMessage()
        {
            trMessage.Visible = false;
            ltMessage.Text = "";
        }
    }
}
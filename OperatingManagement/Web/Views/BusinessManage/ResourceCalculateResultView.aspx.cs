#region
//------------------------------------------------------
//Assembly:OperatingManagement.Web
//FileName:ResourceCalculateResultView.cs
//Remark:资源计算管理类
//------------------------------------------------------
//VERSION       AUTHOR      DATE        CONTENT
//1.0           liutao      20120229    Create     
//------------------------------------------------------
#endregion
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using OperatingManagement.Framework.Core;
using OperatingManagement.DataAccessLayer.BusinessManage;
using OperatingManagement.WebKernel.Basic;

namespace OperatingManagement.Web.Views.BusinessManage
{
    public partial class ResourceCalculateResultView : AspNetPage
    {
        #region 属性
        /// <summary>
        /// ID
        /// </summary>
        protected int RCID
        {
            get
            {
                int rcID = 0;
                if (Request.QueryString["rcid"] != null)
                {
                    int.TryParse(Request.QueryString["rcid"], out rcID);
                }
                return rcID;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    BindControls();
                }

                cpResourceCalculateResultPager.PostBackPage += new EventHandler(cpResourceCalculateResultPager_PostBackPage);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alert", "alert(\"发生未知错误。\");window.location.href='./ResourceCalculateManage.aspx';", true);
                throw (new AspNetException("查看资源调度计算结果页面初始化出现异常，异常原因", ex));
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cpResourceCalculateResultPager_PostBackPage(object sender, EventArgs e)
        {
            try
            {
                BindControls();
            }
            catch (Exception ex)
            {
                throw (new AspNetException("查看资源调度计算结果页面cpResourceCalculateResultPager_PostBackPage方法出现异常，异常原因", ex));
            }
        }
        /// <summary>
        /// 返回资源计算管理页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnReturn_Click(object sender, EventArgs e)
        {
            try
            {
                string url = @"~/Views/BusinessManage/ResourceCalculateManage.aspx";
                Response.Redirect(url);
            }
            catch (System.Threading.ThreadAbortException ex1)
            { }
            catch (Exception ex)
            {
                throw (new AspNetException("查看资源调度计算结果页面btnReturn_Click方法出现异常，异常原因", ex));
            }
        }

        public override void OnPageLoaded()
        {
            this.PagePermission = "OMB_ResCacResult.View";
            this.ShortTitle = "查看资源调度计算结果";
            this.SetTitle();
        }

        #region ItemDataBound
        /// <summary>
        /// 资源计算结果查看单条数据绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rpResourceCalculateResultList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    Repeater rpSatelliteGroundPhaseInfoList = (e.Item.FindControl("rpSatelliteGroundPhaseInfoList") as Repeater);
                    Requirement requirement = (e.Item.DataItem as Requirement);
                    //int itemIndex = e.Item.ItemIndex;

                    if (rpSatelliteGroundPhaseInfoList != null && requirement != null)
                    {
                        rpSatelliteGroundPhaseInfoList.DataSource = requirement.SatelliteGroundPhaseInfoList;
                        rpSatelliteGroundPhaseInfoList.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                throw (new AspNetException("查看资源调度计算结果页面rpResourceCalculateResultList_ItemDataBound方法出现异常，异常原因", ex));
            }
        }

        #endregion

        #region Method
        /// <summary>
        /// 为控件绑定值
        /// </summary>
        private void BindControls()
        {
            ResourceCalculate resourceCalculate = new ResourceCalculate();
            resourceCalculate.Id = RCID;
            resourceCalculate = resourceCalculate.SelectByID();
            if (resourceCalculate == null)
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alert", "alert(\"发生未知错误。\");window.location.href='./ResourceCalculateManage.aspx';", true);
                return;
            }
            string fileName = resourceCalculate.ResultFileDirectory.TrimEnd('\\') + "\\" + resourceCalculate.ResultFileName;
            System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
            xmlDocument.Load(fileName);

            string message = string.Empty;
            ResourceCalculateResult resourceCalculateResult = ResourceCalculateResult.GenerateResourceCalculateResultList(xmlDocument, out message);
            if (resourceCalculateResult == null || !string.IsNullOrEmpty(message))
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alert", "alert(\"" + message + "\");window.location.href='./ResourceCalculateManage.aspx';", true);
                return;
            }

            lblRequirementNumber.Text = resourceCalculateResult.RequirementNumber.ToString();
            lblCompleteRequirementNumber.Text = resourceCalculateResult.CompleteRequirementNumber.ToString();
            lblTotalScore.Text = resourceCalculateResult.TotalScore.ToString();
            lblPriorityScore.Text = resourceCalculateResult.PriorityScore.ToString();
            lblEfficiencyScore.Text = resourceCalculateResult.EfficiencyScore.ToString();
            lblFocusScore.Text = resourceCalculateResult.FocusScore.ToString();
            lblGroundStationProportionScore.Text = resourceCalculateResult.GroundStationProportionScore.ToString();
            lblSatelliteProportionScore.Text = resourceCalculateResult.SatelliteProportionScore.ToString();

            if (resourceCalculateResult.RequirementList.Count > this.SiteSetting.PageSize)
                cpResourceCalculateResultPager.Visible = true;
            cpResourceCalculateResultPager.DataSource = resourceCalculateResult.RequirementList;
            cpResourceCalculateResultPager.PageSize = this.SiteSetting.PageSize;
            cpResourceCalculateResultPager.BindToControl = rpResourceCalculateResultList;
            rpResourceCalculateResultList.DataSource = cpResourceCalculateResultPager.DataSourcePaged;
            rpResourceCalculateResultList.DataBind();
        }

  

        #endregion
    }
}
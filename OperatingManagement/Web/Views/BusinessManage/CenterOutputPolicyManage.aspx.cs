#region
//------------------------------------------------------
//Assembly:OperatingManagement.Web
//FileName:CenterOutputPolicyManage.cs
//Remark:中心输出策略管理类
//------------------------------------------------------
//VERSION       AUTHOR      DATE        CONTENT
//1.0           liutao      20111015    Create     
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
    public partial class CenterOutputPolicyManage : AspNetPage
    {
        #region 属性
        /// <summary>
        /// 查询条件，任务ID
        /// </summary>
        protected string TaskID
        {
            get 
            {
                if (ViewState["TaskID"] == null)
                {
                    ViewState["TaskID"] = dplTask.SelectedValue;
                }
                return ViewState["TaskID"].ToString();
            }
            set { ViewState["TaskID"] = value; }
        }
        /// <summary>
        /// 查询条件，卫星编码
        /// </summary>
        protected string SatName
        {
            get
            {
                if (ViewState["SatName"] == null)
                {
                    ViewState["SatName"] = dplSatellite.SelectedValue;
                }
                return ViewState["SatName"].ToString();
            }
            set { ViewState["SatName"] = value; }
        }
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    BindDataSource();
                    BindSatelliteDataSource();
                    BindCOPList();
                }

                cpPager.PostBackPage += new EventHandler(cpPager_PostBackPage);
            }
            catch(Exception ex)
            {
                throw (new AspNetException("查询中心输出策略页面初始化出现异常，异常原因", ex));
            }
        }
        protected void cpPager_PostBackPage(object sender, EventArgs e)
        {
            try
            {
                BindCOPList();
            }
            catch (Exception ex)
            {
                throw (new AspNetException("查询中心输出策略页面cpPager_PostBackPage方法出现异常，异常原因", ex));
            }
        }
        /// <summary>
        /// 查询中心输出策略
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                TaskID = dplTask.SelectedValue;
                SatName = dplSatellite.SelectedValue;
                cpPager.CurrentPage = 1;
                BindCOPList();
            }
            catch (Exception ex)
            {
                throw (new AspNetException("查询中心输出策略页面btnSearch_Click方法出现异常，异常原因", ex));
            }
        }
        /// <summary>
        /// 添加中心输出策略
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string url = @"~/Views/BusinessManage/CenterOutputPolicyAdd.aspx";
                Response.Redirect(url);
            }
            catch (System.Threading.ThreadAbortException ex1)
            { }
            catch (Exception ex)
            {
                throw (new AspNetException("查询中心输出策略页面btnAdd_Click方法出现异常，异常原因", ex));
            }
        }
        /// <summary>
        /// 编辑中心输出策略
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton lbtnEdit = (sender as LinkButton);
                if (lbtnEdit == null)
                {
                    BindCOPList();
                    return;
                }
                string url = @"~/Views/BusinessManage/CenterOutputPolicyEdit.aspx?copid=" + Server.UrlEncode(lbtnEdit.CommandArgument);
                Response.Redirect(url);
            }
            catch (System.Threading.ThreadAbortException ex1)
            { }
            catch (Exception ex)
            {
                throw (new AspNetException("查询中心输出策略页面lbtnEdit_Click方法出现异常，异常原因", ex));
            }
        }
        /// <summary>
        /// 下载中心输出策略记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnDownload_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton lbtnDownload = (sender as LinkButton);
                if (lbtnDownload == null)
                {
                    BindCOPList();
                    return;
                }
                int copID = 0;
                if (!int.TryParse(lbtnDownload.CommandArgument, out copID))
                {
                    BindCOPList();
                    return;
                }

                CenterOutputPolicy centerOutputPolicy = new CenterOutputPolicy();
                centerOutputPolicy.Id = copID;
                centerOutputPolicy = centerOutputPolicy.SelectByID();
                if (centerOutputPolicy == null)
                {
                    BindCOPList();
                    return;
                }
                StringBuilder strBuilder = new StringBuilder("ConfigurationInfo\r\n");
                strBuilder.AppendFormat("TaskID={0}\r\n", centerOutputPolicy.TaskID);
                strBuilder.AppendFormat("SatName={0}\r\n", GetSatelliteWXMC(centerOutputPolicy.SatName));
                strBuilder.AppendFormat("Source={0}\r\n", GetXYXSADDRName(centerOutputPolicy.InfoSource));
                strBuilder.AppendFormat("InfoType={0}\r\n", GetXXTypeDATANAME(centerOutputPolicy.InfoType));
                strBuilder.AppendFormat("Ddestination={0}\r\n", GetXYXSADDRName(centerOutputPolicy.Destination));
                strBuilder.AppendFormat("EffectTime={0}\r\n", centerOutputPolicy.EffectTime.ToString("yyyy-MM-dd HH:mm:ss"));
                strBuilder.AppendFormat("DefectTime={0}\r\n", centerOutputPolicy.DefectTime.ToString("yyyy-MM-dd HH:mm:ss"));
                Response.Clear();
                Response.Buffer = false;
                Response.ContentType = "application/octet-stream";
                Response.AppendHeader("content-disposition", "attachment;filename=" + centerOutputPolicy.TaskID + ".dat;");
                Response.Write(strBuilder.ToString());
                Response.Flush();
                Response.End();
            }
            catch (System.Threading.ThreadAbortException ex1)
            { }
            catch (Exception ex)
            {
                throw (new AspNetException("查询中心输出策略页面lbtnDownload_Click方法出现异常，异常原因", ex));
            }
        }

        public override void OnPageLoaded()
        {
            this.ShortTitle = "查询中心输出策略";
            this.SetTitle();
        }

        #region Method
        /// <summary>
        /// 绑定任务数据源
        /// </summary>
        private void BindDataSource()
        {
            //dplTask.Items.Clear();
            //dplTask.DataSource = SystemParameters.GetSystemParameters(SystemParametersType.CenterOutputPolicyTaskList);
            //dplTask.DataTextField = "key";
            //dplTask.DataValueField = "value";
            //dplTask.DataBind();
            //dplTask.Items.Insert(0, new ListItem("全部", ""));
            dplTask.AllowBlankItem = true;
            dplTask.BlankItemText = "请选择";
            dplTask.BlankItemValue = "";
        }

        /// <summary>
        /// 绑定卫星数据源
        /// </summary>
        private void BindSatelliteDataSource()
        {
            dplSatellite.AllowBlankItem = true;
            dplSatellite.BlankItemText = "请选择";
            dplSatellite.BlankItemValue = "";
        }

        /// <summary>
        /// 绑定中心输出策略信息
        /// </summary>
        private void BindCOPList()
        {
            CenterOutputPolicy centerOutputPolicy = new CenterOutputPolicy();
            centerOutputPolicy.TaskID = TaskID;
            centerOutputPolicy.SatName = SatName;
            List<CenterOutputPolicy> centerOutputPolicyList = centerOutputPolicy.SelectByParameters();
            if (centerOutputPolicyList.Count > this.SiteSetting.PageSize)
                cpPager.Visible = true;
            cpPager.DataSource = centerOutputPolicyList;
            cpPager.PageSize = this.SiteSetting.PageSize;
            cpPager.BindToControl = rpCOPList;
            rpCOPList.DataSource = cpPager.DataSourcePaged;
            rpCOPList.DataBind();
        }

        /// <summary>
        /// 根据rid获得信源信宿的ADDRName
        /// </summary>
        /// <param name="rid">编号</param>
        /// <returns>信源信宿的地址</returns>
        protected string GetXYXSADDRName(int rid)
        {
            XYXSInfo xyxs = new XYXSInfo();
            return xyxs.GetName(rid);
        }

        /// <summary>
        /// 根据rid获得信息类型的DATANAME
        /// </summary>
        /// <param name="rid">编号</param>
        /// <returns>信息类型DATANAME</returns>
        protected string GetXXTypeDATANAME(int rid)
        {
            InfoType xxType = new InfoType();
            return xxType.GetName(rid);
        }

        /// <summary>
        /// 根据WXBM获得卫星名称
        /// </summary>
        /// <param name="wxbm">卫星编码</param>
        /// <returns>卫星名称</returns>
        protected string GetSatelliteWXMC(string wxbm)
        {
            Satellite satellite = new Satellite();
            return satellite.GetName(wxbm);
        }
        /// <summary>
        /// 通过任务代号获取任务名称
        /// </summary>
        /// <param name="taskNO">任务代号</param>
        /// <returns>任务名称</returns>
        protected string GetTaskName(string taskNO, string satID)
        {
            Task task = new Task();
            return task.GetTaskName(taskNO, satID);
        }
        #endregion
    }
}
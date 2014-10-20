#region
//------------------------------------------------------
//Assembly:OperatingManagement.Web
//FileName:GroundStationManage.cs
//Remark:地面站管理类
//------------------------------------------------------
//VERSION       AUTHOR      DATE        CONTENT
//1.0           liutao     20120823     Create     
//------------------------------------------------------
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using OperatingManagement.Framework.Core;
using OperatingManagement.DataAccessLayer.BusinessManage;
using OperatingManagement.WebKernel.Basic;

namespace OperatingManagement.Web.Views.BusinessManage
{
    public partial class GroundStationManage : AspNetPage
    {
        #region 属性
       
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    BindDataSource();
                    BindGroundStationList();
                }

                cpGroundStationPager.PostBackPage += new EventHandler(rpGroundStationList_PostBackPage);
            }
            catch (Exception ex)
            {
                throw (new AspNetException("查询地面站页面初始化出现异常，异常原因", ex));
            }
        }

        protected void rpGroundStationList_PostBackPage(object sender, EventArgs e)
        {
            try
            {
                BindGroundStationList();
            }
            catch (Exception ex)
            {
                throw (new AspNetException("查询地面站页面rpGroundStationList_PostBackPage方法出现异常，异常原因", ex));
            }
        }
        /// <summary>
        /// 查询地面站
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                cpGroundStationPager.CurrentPage = 1;
                BindGroundStationList();
            }
            catch (Exception ex)
            {
                throw (new AspNetException("查询地面站页面btnSearch_Click方法出现异常，异常原因", ex));
            }
        }
        /// <summary>
        /// 添加地面站
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string url = @"~/Views/BusinessManage/GroundStationAdd.aspx";
                Response.Redirect(url);
            }
            catch (System.Threading.ThreadAbortException ex1)
            { }
            catch (Exception ex)
            {
                throw (new AspNetException("查询地面站页面btnAdd_Click方法出现异常，异常原因", ex));
            }
        }
        /// <summary>
        /// 编辑地面站
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnEditGroundStation_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton lbtnEdit = (sender as LinkButton);
                if (lbtnEdit == null)
                {
                    BindGroundStationList();
                    return;
                }

                string url = @"~/Views/BusinessManage/GroundStationEdit.aspx?rid=" + Server.UrlEncode(lbtnEdit.CommandArgument);//地面站编辑页
                Response.Redirect(url);
            }
            catch (System.Threading.ThreadAbortException ex1)
            { }
            catch (Exception ex)
            {
                throw (new AspNetException("查询地面站页面lbtnEditGroundStation_Click方法出现异常，异常原因", ex));
            }
        }
        /// <summary>
        /// 删除地面站
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnDeleteGroundStation_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton lbtnDelete = (sender as LinkButton);
                if (lbtnDelete == null)
                {
                    BindGroundStationList();
                    return;
                }
                //地面站ID
                int id = 0;
                int.TryParse(lbtnDelete.CommandArgument, out id);
                XYXSInfo xyxsInfo = new XYXSInfo();
                xyxsInfo.Id = id;
                xyxsInfo = xyxsInfo.SelectByID();
                if (xyxsInfo == null)
                {
                    BindGroundStationList();
                    return;
                }
                xyxsInfo.Status = 2;//删除状态
                xyxsInfo.UpdatedTime = DateTime.Now;
                Framework.FieldVerifyResult result = xyxsInfo.Update();
                switch (result)
                {
                    case Framework.FieldVerifyResult.Error:
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alert", "alert(\"发生了数据错误，无法完成请求的操作。\")", true);
                        break;
                    case Framework.FieldVerifyResult.Success:
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alert", "alert(\"删除地面站成功。\")", true);
                        BindGroundStationList();
                        break;
                    default:
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alert", "alert(\"发生了数据错误，无法完成请求的操作。\")", true);
                        BindGroundStationList();
                        break;
                }
            }
            catch (System.Threading.ThreadAbortException ex1)
            { }
            catch (Exception ex)
            {
                throw (new AspNetException("查询地面站页面lbtnDeleteGroundStation_Click方法出现异常，异常原因", ex));
            }
        }

        public override void OnPageLoaded()
        {
            this.PagePermission = "OMB_GSMan.View";
            this.ShortTitle = "查询地面站";
            this.SetTitle();
        }

        #region ItemDataBound
        /// <summary>
        /// 地面站单条数据绑定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void rpGroundStationList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LinkButton lbtnEditGroundStation = (e.Item.FindControl("lbtnEditGroundStation") as LinkButton);
                LinkButton lbtnDeleteGroundStation = (e.Item.FindControl("lbtnDeleteGroundStation") as LinkButton);
                if (lbtnEditGroundStation != null && lbtnDeleteGroundStation != null)
                {
                    XYXSInfo xyxsInfo = (e.Item.DataItem as XYXSInfo);
                    //如果该地面站是删除状态则不能进行编辑、删除操作
                    if (xyxsInfo.Status == 2)
                    {
                        lbtnEditGroundStation.Enabled = false;
                        lbtnDeleteGroundStation.OnClientClick = string.Empty;
                        lbtnDeleteGroundStation.Enabled = false;
                    }
                }
            }
        }
        #endregion

        #region Method
        /// <summary>
        /// 绑定控件数据源
        /// </summary>
        private void BindDataSource()
        {
            dplOwn.Items.Clear();
            dplOwn.DataSource = SystemParameters.GetSystemParameters(SystemParametersType.XYXSInfoOwn);
            dplOwn.DataTextField = "key";
            dplOwn.DataValueField = "value";
            dplOwn.DataBind();
            dplOwn.Items.Insert(0, new ListItem("请选择", ""));

            dplStatus.Items.Clear();
            dplStatus.DataSource = SystemParameters.GetSystemParameters(SystemParametersType.XYXSInfoStatus);
            dplStatus.DataTextField = "key";
            dplStatus.DataValueField = "value";
            dplStatus.DataBind();
            dplStatus.Items.Insert(0, new ListItem("请选择", "-1"));     
        }
        /// <summary>
        /// 绑定地面站列表
        /// </summary>
        private void BindGroundStationList()
        {
            XYXSInfo xyxsInfo = new XYXSInfo();
            int status = -1;
            int.TryParse(dplStatus.SelectedValue, out status);
            List<XYXSInfo> xyxsInfoList = xyxsInfo.Search(txtAddrName.Text.Trim(), txtAddrMark.Text.Trim(), dplOwn.SelectedValue, 0, status);
            if (xyxsInfoList.Count > this.SiteSetting.PageSize)
                cpGroundStationPager.Visible = true;
            cpGroundStationPager.DataSource = xyxsInfoList;
            cpGroundStationPager.PageSize = this.SiteSetting.PageSize;
            cpGroundStationPager.BindToControl = rpGroundStationList;
            rpGroundStationList.DataSource = cpGroundStationPager.DataSourcePaged;
            rpGroundStationList.DataBind();
        }

        #endregion

    }
}
#region
//------------------------------------------------------
//Assembly:OperatingManagement.Web
//FileName:SatManage.cs
//Remark:卫星管理类
//------------------------------------------------------
//VERSION       AUTHOR      DATE        CONTENT
//1.1           liutao     20120915     Update     
//------------------------------------------------------
#endregion
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
using OperatingManagement.DataAccessLayer.BusinessManage;
using ServicesKernel.File;


namespace OperatingManagement.Web.Views.BusinessManage.BDManage
{
    public partial class SatManage : AspNetPage, IRouteContext
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
                    BindSatelliteList();
                }
                cpSatellitePager.PostBackPage += new EventHandler(cpSatellitePager_PostBackPage);
            }
            catch (Exception ex)
            {
                throw (new AspNetException("查询卫星页面初始化出现异常，异常原因", ex));
            }
        }
        protected void cpSatellitePager_PostBackPage(object sender, EventArgs e)
        {
            try
            {
                BindSatelliteList();
            }
            catch (Exception ex)
            {
                throw (new AspNetException("查询卫星页面cpSatellitePager_PostBackPage方法出现异常，异常原因", ex));
            }
        }
        /// <summary>
        /// 查询卫星
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                cpSatellitePager.CurrentPage = 1;
                BindSatelliteList();
            }
            catch (Exception ex)
            {
                throw (new AspNetException("查询卫星页面btnSearch_Click方法出现异常，异常原因", ex));
            }
        }
        /// <summary>
        /// 添加卫星
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string url = @"~/Views/BusinessManage/BDManage/SatAdd.aspx";
                Response.Redirect(url);
            }
            catch (System.Threading.ThreadAbortException ex1)
            { }
            catch (Exception ex)
            {
                throw (new AspNetException("查询卫星页面btnAdd_Click方法出现异常，异常原因", ex));
            }
        }
        /// <summary>
        /// 编辑卫星
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnEditSatellite_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton lbtnEdit = (sender as LinkButton);
                if (lbtnEdit == null)
                {
                    BindSatelliteList();
                    return;
                }

                string url = @"~/Views/BusinessManage/BDManage/SatEdit.aspx?wxbm=" + Server.UrlEncode(lbtnEdit.CommandArgument);//卫星编辑页
                Response.Redirect(url);
            }
            catch (System.Threading.ThreadAbortException ex1)
            { }
            catch (Exception ex)
            {
                throw (new AspNetException("查询卫星页面lbtnEditSatellite_Click方法出现异常，异常原因", ex));
            }
        }

        public override void OnPageLoaded()
        {
            this.PagePermission = "OMB_SatMan.View";
            this.ShortTitle = "查询卫星";
            this.SetTitle();
        }

        #region Method
        /// <summary>
        /// 绑定控件数据源
        /// </summary>
        private void BindDataSource()
        {
            dplState.Items.Clear();
            dplState.DataSource = SystemParameters.GetSystemParameters(SystemParametersType.SatelliteState);
            dplState.DataTextField = "key";
            dplState.DataValueField = "value";
            dplState.DataBind();
            dplState.Items.Insert(0, new ListItem("请选择", "-1"));
        }
        /// <summary>
        /// 绑定卫星列表
        /// </summary>
        private void BindSatelliteList()
        {
            Satellite satellite = new Satellite();
            int state = -1;
            int.TryParse(dplState.SelectedValue, out state);
            List<Satellite> satelliteList = satellite.Search(txtWXMC.Text.Trim(), txtWXBM.Text.Trim(), txtWXBS.Text.Trim(), state);
            if (satelliteList.Count > this.SiteSetting.PageSize)
                cpSatellitePager.Visible = true;
            cpSatellitePager.DataSource = satelliteList;
            cpSatellitePager.PageSize = this.SiteSetting.PageSize;
            cpSatellitePager.BindToControl = rpSatelliteList;
            rpSatelliteList.DataSource = cpSatellitePager.DataSourcePaged;
            rpSatelliteList.DataBind();
        }

        /// <summary>
        /// 获取形状名称
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected string GetShapeName(string type)
        {
            if (type == "0")
                return "球体";
            else if (type == "1")
                return "圆柱体";
            else
                return type;
        }

        /// <summary>
        /// 获取表名情况名称
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected string GetRGName(string type)
        {
            if (type == "0")
                return "粗糙";
            else if (type == "1")
                return "光滑";
            else
                return type;
        }
        #endregion
    }
}
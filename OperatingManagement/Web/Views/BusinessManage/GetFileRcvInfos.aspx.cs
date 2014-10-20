using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data;

using OperatingManagement.WebKernel.Basic;
using OperatingManagement.WebKernel.Route;
using OperatingManagement.Framework.Core;
using OperatingManagement.DataAccessLayer;
using OperatingManagement.Framework;
using OperatingManagement.DataAccessLayer.BusinessManage;
using ServicesKernel.File;

namespace OperatingManagement.Web.Views.BusinessManage
{
    public partial class GetFileRcvInfos : AspNetPage, IRouteContext
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HideMessage();
            if (!IsPostBack)
            {
                txtFrom.Attributes.Add("readonly", "true");
                txtTo.Attributes.Add("readonly", "true");
                InitialPageData();
            }
            cpPager.PostBackPage += new EventHandler(cpPager_PostBackPage);
        }

        protected void cpPager_PostBackPage(object sender, EventArgs e)
        {
            Search(false);
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Search(true);
        }

        private void Search(bool fromSearchBtn)
        {
            DateTime dtFrom = DateTime.MinValue;
            DateTime dtTo = DateTime.MinValue;
            FileReceiveInfo oRecv = new FileReceiveInfo();

            if (fromSearchBtn)
            {
                #region 判断日期格式
                if (txtFrom.Text != "")
                {
                    if (!DateTime.TryParse(txtFrom.Text.Trim(), out dtFrom))
                    {
                        ShowMessage("开始日期格式非法");
                        return;
                    }
                }
                if (txtTo.Text != "")
                {
                    if (!DateTime.TryParse(txtTo.Text.Trim(), out dtTo))
                    {
                        ShowMessage("结束日期格式非法");
                        return;
                    }
                }

                if (dtFrom != DateTime.MinValue && dtTo != DateTime.MinValue && dtFrom > dtTo)
                {
                    ShowMessage("结束日期必须大于开始日期");
                    return;
                }
                if (dtFrom == dtTo && dtFrom != DateTime.MinValue)
                    dtTo = dtTo.AddDays(1).AddSeconds(-1);
                #endregion
                oRecv.InfoTypeID = Convert.ToInt32(ddlInfoType.SelectedItem.Value);
                cpPager.CurrentPage = 1;
                this.ViewState["_filter"] = dtFrom.ToLongDateString() + "," + dtTo.ToLongDateString() + "," + ddlInfoType.SelectedItem.Value;
            }
            else
            {
                string[] strFilters = new string[0];
                if (this.ViewState["_filter"] != null)
                    strFilters = this.ViewState["_filter"].ToString().Split(new char[] { ','});
                if (strFilters.Length == 3)
                {
                    try
                    {
                        DateTime.TryParse(strFilters[0], out dtFrom);
                        DateTime.TryParse(strFilters[1], out dtTo);
                        oRecv.InfoTypeID = Convert.ToInt32(strFilters[2]);
                    }
                    catch (Exception ex)
                    {
                        throw(new AspNetException("查询文件接收记录-解析查询条件异常", ex));
                    }
                    finally { }
                }
            }
            List<FileReceiveInfo> listDatas;
            try
            {
                listDatas = oRecv.Search(dtFrom, dtTo);
                if (listDatas.Count == 0)
                    ShowMessage("没有符合条件的记录");
                BindDataSource(listDatas);
            }
            catch (Exception ex)
            {
                throw (new AspNetException("查询文件接收记录-查询出现异常", ex));
            }
            finally { }
        }

        /// <summary>
        /// 初始化页面数据
        /// </summary>
        private void InitialPageData()
        {
            FileReceiveInfo oRecv = new FileReceiveInfo();
            try
            {
                List<FileReceiveInfo> listDatas = oRecv.SelectAll();
                BindDataSource(listDatas);
            }
            catch (Exception ex)
            {
                throw (new AspNetException("查询文件接收记录-初始化页面出现异常", ex));
            }
            finally { }
        }

        /// <summary>
        /// 绑定控件数据源
        /// </summary>
        /// <param name="listDatas"></param>
        private void BindDataSource(List<FileReceiveInfo> listDatas)
        {
            cpPager.DataSource = listDatas;
            cpPager.PageSize = this.SiteSetting.PageSize;
            if (listDatas.Count > this.SiteSetting.PageSize)
                cpPager.Visible = true;
            cpPager.BindToControl = rpDatas;
            rpDatas.DataSource = cpPager.DataSourcePaged;
            rpDatas.DataBind();
        }

        /// <summary>
        /// 初始化页面基本信息
        /// </summary>
        public override void OnPageLoaded()
        {
            this.PagePermission = "OMB_FRcvInfo.View";
            this.ShortTitle = "查看文件接收记录";
            this.SetTitle();
            this.AddJavaScriptInclude("scripts/pages/BusinessManage/GetFileSInfos.aspx.js");
        }

        /// <summary>
        /// 隐藏提示信息
        /// </summary>
        private void HideMessage()
        {
            trMessage.Visible = false;
            lblMessage.Text = "";
        }

        /// <summary>
        /// 显示提示信息
        /// </summary>
        /// <param name="message"></param>
        private void ShowMessage(string message)
        {
            trMessage.Visible = true;
            lblMessage.Text = message;
        }
    }
}
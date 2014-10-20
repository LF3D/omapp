using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Web.Security;
using System.Data;
using ServicesKernel.File;

using OperatingManagement.WebKernel.Basic;
using OperatingManagement.WebKernel.Route;
using OperatingManagement.Framework.Core;
using OperatingManagement.DataAccessLayer;
using OperatingManagement.Framework;
using OperatingManagement.DataAccessLayer.BusinessManage;
using OperatingManagement.RemotingObjectInterface;

namespace OperatingManagement.Web.Views.BusinessManage
{
    public partial class GetFileSInfos : AspNetPage, IRouteContext
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

        protected void btnHidRSendFile_Click(object sender, EventArgs e)
        {
            string strRID = txtRID.Text;
            string strStatus = txtStatus.Text;
            //status=0已提交发送；status=1发送中，这两种情况不能重发
            if (strStatus == SendStatuss.Submitted.ToString() || strStatus == SendStatuss.Sending.ToString())
                return;
            if (strRID.Equals(string.Empty))
                return;
            ReSendFile(Convert.ToInt32(strRID));
        }

        protected void btnResend_Click(object sender, EventArgs e)
        {
            int iRID = 0;
            Button btnResend = (Button)sender;
            if (btnResend == null)
                return;
            if (!int.TryParse(btnResend.CommandArgument, out iRID))
                return;
            FileSendInfo oSInfo = new FileSendInfo();
            oSInfo.Id = iRID;
            oSInfo = oSInfo.SelectById();
            if (oSInfo != null)
            {
                ReSendFile(iRID);
            }
            Search(false);
        }
        
        protected bool CanReSend(object status)
        {
            string strStatus = status.ToString();
            if (strStatus == SendStatuss.Submitted.ToString() || strStatus == SendStatuss.Sending.ToString())
                return false;
            else
                return true;
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
            FileSendInfo oSend = new FileSendInfo();

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
                oSend.InfoTypeID = Convert.ToInt32(ddlInfoType.SelectedItem.Value);
                cpPager.CurrentPage = 1;
                this.ViewState["_filter"] = dtFrom.ToLongDateString() + "," + dtTo.ToLongDateString() + "," + ddlInfoType.SelectedItem.Value;
            }
            else
            {
                #region 载入存取的查询条件
                string[] strFilters = new string[0];
                if (this.ViewState["_filter"] != null)
                    strFilters = this.ViewState["_filter"].ToString().Split(new char[] { ',' });
                if (strFilters.Length == 3)
                {
                    try
                    {
                        DateTime.TryParse(strFilters[0], out dtFrom);
                        DateTime.TryParse(strFilters[1], out dtTo);
                        oSend.InfoTypeID = Convert.ToInt32(strFilters[2]);
                    }
                    catch (Exception ex)
                    {
                        throw (new AspNetException("查询文件发送记录-解析查询条件异常", ex));
                    }
                    finally { }
                }
                #endregion
            }
            List<FileSendInfo> listDatas;
            try
            {
                listDatas = oSend.Search(dtFrom, dtTo);
                if (listDatas.Count == 0)
                    ShowMessage("没有符合条件的记录");
                BindDataSource(listDatas);
            }
            catch (Exception ex)
            {
                throw (new AspNetException("查询文件发送记录-查询出现异常", ex));
            }
            finally { }
        }

        /// <summary>
        /// 初始化页面数据
        /// </summary>
        private void InitialPageData()
        {
            ddlInfoType.AllowBlankItem = true;
            FileSendInfo oSend = new FileSendInfo();
            try
            {
                List<FileSendInfo> listDatas = oSend.SelectAll();
                BindDataSource(listDatas);
            }
            catch (Exception ex)
            {
                throw (new AspNetException("查询文件发送记录-初始化页面出现异常", ex));
            }
            finally { }
        }

        /// <summary>
        /// 绑定控件数据源
        /// </summary>
        /// <param name="listDatas"></param>
        private void BindDataSource(List<FileSendInfo> listDatas)
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
        /// 重新发送文件
        /// </summary>
        /// <param name="sendInfoID"></param>
        /// <returns></returns>
        private void ReSendFile(int sendInfoID)
        {
            string strResult = string.Empty;
            IFileSender oSender = FileSenderClientAgent.GetObject<IFileSender>();
            if (oSender == default(IFileSender))
            {
                ShowMessage("无法访问文件发送服务，请查看相关配置文件。");
                return;
            }
            strResult = oSender.ReSendFile(sendInfoID);
            if (strResult != string.Empty)
            {
                try
                {
                    XElement root = XElement.Parse(strResult);
                    int iResult = Convert.ToInt32(root.Element("code").Value);
                    if (iResult == 0)
                        ShowMessage(string.Format("文件发送请求提交成功，请求ID：{0}。", root.Element("fileid").Value));
                    else
                        ShowMessage(string.Format("文件发送请求提交失败，文件服务器提示：{0}。", root.Element("msg").Value));
                }
                catch (Exception ex)
                {
                    throw (new AspNetException("解析文件发送请求提交结果出现异常", ex));
                }
                finally { }
            }
        }

        /// <summary>
        /// 初始化页面基本信息
        /// </summary>
        public override void OnPageLoaded()
        {
            this.PagePermission = "OMB_FSendInfo.View";
            this.ShortTitle = "查看文件发送记录";
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

        protected void rpDatas_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            FileSendInfo oFSInfo = null;
            string strStatus = string.Empty;
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem) 
            {
                oFSInfo = (FileSendInfo)e.Item.DataItem;
                //switch (oFSInfo.SendStatus)
                //{
                //    case SendStatuss.Submitted:
                //        strStatus = "已提交";
                //        break;
                //    case SendStatuss.Sending:
                //        strStatus = "发送中";
                //        break;
                //    case SendStatuss.Failed:
                //        strStatus = "发送失败";
                //        break;
                //    case SendStatuss.Success:
                //        strStatus = "发送成功";
                //        break;
                //}
                //((Label)e.Item.FindControl("lbStatus")).Text = strStatus;
                if (oFSInfo.SendStatus != SendStatuss.Failed)
                {
                    ((Button)e.Item.FindControl("btnResend")).Enabled = false;
                    //((Button)e.Item.FindControl("btnResend")).ForeColor = System.Drawing.Color.Gray;
                }
            }
        }
    }
}
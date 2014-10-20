using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Xml.Linq;
using System.Configuration;
using System.Web.Security;
using System.Data;
using System.Web.UI.WebControls;

using OperatingManagement.WebKernel.Basic;
using OperatingManagement.WebKernel.Route;
using OperatingManagement.Framework.Core;
using OperatingManagement.DataAccessLayer;
using OperatingManagement.Framework;
using OperatingManagement.DataAccessLayer.BusinessManage;
using OperatingManagement.RemotingObjectInterface;
using OperatingManagement.RemotingClient;
using ServicesKernel.File;
using ServicesKernel;

namespace OperatingManagement.Web.Views.BusinessManage
{
    public partial class SendFile : AspNetPage, IRouteContext
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                InitialPageData();
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            #region 验证发送条件
            if (ddlSender.SelectedItem.Value == ddlReceiver.SelectedItem.Value)
            {
                ShowMessage("信源与信宿不能相同");
                return;
            }

            if (!fuToSend.HasFile)
            {
                ShowMessage("待发送文件不能为空。");
                return;
            }
            IFileSender oSender = FileSenderClientAgent.GetObject<IFileSender>();
            if (oSender == default(IFileSender))
            {
                ShowMessage("无法访问文件发送服务，请查看相关配置文件。");
                return;
            }
            #endregion

            string strResult = string.Empty;
            string fileFullName = fuToSend.FileName;
            string fileName = fileFullName.Substring(fileFullName.LastIndexOf(@"\",0)+1);
            fuToSend.SaveAs(Path.Combine(Param.OutPutPath, fileName));
            #region 发送
            try
            {
                strResult = oSender.SendFile(fileName, Param.OutPutPath, Convert.ToInt32(rblSendWay.SelectedValue), Convert.ToInt32(ddlSender.SelectedValue)
                    , Convert.ToInt32(ddlReceiver.SelectedValue), Convert.ToInt32(ddlInfoType.SelectedValue), rblAutoResend.SelectedValue == "1");
            }
            catch (Exception ex)
            {
                File.Delete(Path.Combine(Param.OutPutPath, fileName));
                throw (new AspNetException("提交文件发送请求出现异常", ex));
            }
            finally { }
            #endregion

            if (strResult != string.Empty)
            {
                #region 解析发送结果
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
                #endregion
            }
        }

        /// <summary>
        /// 初始化页面
        /// </summary>
        private void InitialPageData()
        {
            HideMessage();
            this.ddlInfoType.AllowBlankItem = false;
            this.ddlSender.AllowBlankItem = false;
            this.ddlReceiver.AllowBlankItem = false;
            this.ddlSender.SelectedValue = "3";//默认运控评估中心
        }

        /// <summary>
        /// 初始化页面基本信息
        /// </summary>
        public override void OnPageLoaded()
        {
            this.PagePermission = "OMB_FSendFile.Do";
            this.ShortTitle = "发送文件";
            this.SetTitle();
        }
        
        /// <summary>
        /// 显示提示信息
        /// </summary>
        /// <param name="message"></param>
        private void ShowMessage(string message)
        {
            lblMessage.Text = message;
            lblMessage.Visible = true;
        }

        private void HideMessage()
        {
            lblMessage.Text = "";
            lblMessage.Visible = false;
        }
    }
}
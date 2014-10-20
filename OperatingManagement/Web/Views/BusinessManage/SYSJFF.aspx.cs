using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Data;
using System.Threading;

using OperatingManagement.WebKernel.Basic;
using OperatingManagement.WebKernel.Route;
using OperatingManagement.Framework.Core;
using OperatingManagement.DataAccessLayer.PlanManage;
using OperatingManagement.Framework;
using OperatingManagement.Framework.Components;
using OperatingManagement.DataAccessLayer.BusinessManage;
using ServicesKernel.File;
using ServicesKernel;
using ServicesKernel.GDFX;

namespace OperatingManagement.Web.Views.BusinessManage
{
    public partial class SYSJFF : AspNetPage, IRouteContext
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HideMessage();
            if (!Page.IsPostBack)
            {
                txtFrom.Attributes.Add("readonly", "true");
                txtTo.Attributes.Add("readonly", "true");
                HidControl();
            }
            else
            {
                if (this.ViewState["showfile"] != null)
                    AddLinkButton();
            }
            cpFZData.PostBackPage += new EventHandler(cpFZData_PostBackPage);
            cpUFData.PostBackPage += new EventHandler(cpUFData_PostBackPage);
            cpYCData.PostBackPage += new EventHandler(cpYCData_PostBackPage);
            cpGDData.PostBackPage += new EventHandler(cpGDData_PostBackPage);
        }

        protected void cpFZData_PostBackPage(object sender, EventArgs e)
        {
            BindData(false);
        }

        protected void cpUFData_PostBackPage(object sender, EventArgs e)
        {
            BindData(false);
        }

        protected void cpYCData_PostBackPage(object sender, EventArgs e)
        {
            BindData(false);
        }

        protected void cpGDData_PostBackPage(object sender, EventArgs e)
        {
            BindData(false);
        }

        /// <summary>
        /// 绑定YC控件数据源
        /// </summary>
        /// <param name="listDatas"></param>
        private void BindYCDataSource(List<YCPG> listDatas)
        {
            vYCData.Visible = true;
            cpYCData.DataSource = listDatas;
            cpYCData.PageSize = this.SiteSetting.PageSize;
            if (listDatas.Count > this.SiteSetting.PageSize)
                cpYCData.Visible = true;
            cpYCData.BindToControl = rpYCData;
            rpYCData.DataSource = cpYCData.DataSourcePaged;
            rpYCData.DataBind();
        }

        /// <summary>
        /// 绑定UF控件数据源
        /// </summary>
        /// <param name="listDatas"></param>
        private void BindUFDataSource(List<UserFrame> listDatas)
        {
            vUFData.Visible = true;
            cpUFData.DataSource = listDatas;
            cpUFData.PageSize = this.SiteSetting.PageSize;
            if (listDatas.Count > this.SiteSetting.PageSize)
                cpUFData.Visible = true;
            cpUFData.BindToControl = rpUFData;
            rpUFData.DataSource = cpUFData.DataSourcePaged;
            rpUFData.DataBind();
        }

        /// <summary>
        /// 绑定FZ控件数据源
        /// </summary>
        /// <param name="listDatas"></param>
        private void BindFZDataSource(List<JH> listDatas)
        {
            vFZData.Visible = true;
            cpFZData.DataSource = listDatas;
            cpFZData.PageSize = this.SiteSetting.PageSize;
            if (listDatas.Count > this.SiteSetting.PageSize)
                cpFZData.Visible = true;
            cpFZData.BindToControl = rpFZData;
            rpFZData.DataSource = cpFZData.DataSourcePaged;
            rpFZData.DataBind();
        }

        /// <summary>
        /// 绑定GD控件数据源
        /// </summary>
        /// <param name="listDatas"></param>
        private void BindGDDataSource(List<GD> listDatas)
        {
            vGDData.Visible = true;
            cpGDData.DataSource = listDatas;
            cpGDData.PageSize = this.SiteSetting.PageSize;
            if (listDatas.Count > this.SiteSetting.PageSize)
                cpGDData.Visible = true;
            cpGDData.BindToControl = rpGDData;
            rpGDData.DataSource = cpGDData.DataSourcePaged;
            rpGDData.DataBind();
        }

        /// <summary>
        /// 隐藏提示信息
        /// </summary>
        private void HideMessage()
        {
            trMessage.Visible = false;
            lblMessage.Text = "";
        }

        private void HidControl()
        {
            vUFData.Visible = false;
            vYCData.Visible = false;
            vFZData.Visible = false;
            vGDData.Visible = false;
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


        /// <summary>
        /// 初始化页面基本信息
        /// </summary>
        public override void OnPageLoaded()
        {
            this.PagePermission = "OMB_SYSJFF.View";
            this.ShortTitle = "试验数据分发";
            this.SetTitle();
            this.AddJavaScriptInclude("scripts/pages/BusinessManage/SYSJFF.aspx.js");
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            ClearLinkButton();
            BindData(true);
            //string path = System.Configuration.ConfigurationManager.AppSettings["YCPGFilePath"];
            //System.IO.FileStream oFile = System.IO.File.Create(path + "1.txt");
            //oFile.Write(new byte[] { 0, 1, 0, 1 }, 0, 4);
            //oFile.Close();
        }

        public void BindData(bool fromSearchBtn)
        {
            List<YCPG> listYCData = null;
            List<UserFrame> listUFData = null;
            List<GD> listGDData = null;
            HidControl();

            #region 取查询条件
            string taskNo = string.Empty;
            string satID = string.Empty;
            string sType = string.Empty;
            DateTime dtFrom = DateTime.MinValue;
            DateTime dtTo = DateTime.MinValue;
            if (fromSearchBtn)
            {
                sType = ddlDataType.SelectedValue;
                if (ucOutTask1.SelectedIndex != 0)
                {
                    new Task().GetTaskNoSatID(ucOutTask1.SelectedValue, out taskNo, out satID);
                }
                cpFZData.CurrentPage = 1;
                cpUFData.CurrentPage = 1;
                cpYCData.CurrentPage = 1;
                cpGDData.CurrentPage = 1;
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
                this.ViewState["_filter"] = dtFrom.ToLongDateString() + "," + dtTo.ToLongDateString() + "," 
                    + sType + "," + taskNo + "," + satID;
            }
            else
            {
                string[] strFilters = new string[0];
                if (this.ViewState["_filter"] != null)
                    strFilters = this.ViewState["_filter"].ToString().Split(new char[] { ',' });
                if (strFilters.Length == 5)
                {
                    DateTime.TryParse(strFilters[0], out dtFrom);
                    DateTime.TryParse(strFilters[1], out dtTo);
                    sType = strFilters[2];
                    taskNo = strFilters[3];
                    satID = strFilters[4];
                }
            }
            #endregion

            string dataType = string.Empty;
            switch (sType)
            {
                case "0"://TJ
                    try
                    {
                        dataType = System.Configuration.ConfigurationManager.AppSettings["TJtypeInYCPG"];
                        listYCData = new YCPG().GetSYSJ(dtFrom, dtTo, dataType, taskNo, satID);
                        BindYCDataSource(listYCData);
                    }
                    catch (Exception ex)
                    {
                        throw (new AspNetException("天基目标观测试验数据-状态数据，获取出现异常", ex));
                    }
                    finally { }
                    try
                    {
                        dataType = System.Configuration.ConfigurationManager.AppSettings["TJtypeInUserFrame"];
                        listUFData = new UserFrame().GetSYSJ(dtFrom, dtTo, taskNo, satID, dataType);
                        BindUFDataSource(listUFData);
                    }
                    catch (Exception ex)
                    {
                        throw (new AspNetException("天基目标观测试验数据-图像数据，获取出现异常", ex));
                    }
                    finally { }
                    break;
                case "1"://KJ
                    try
                    {
                        dataType = System.Configuration.ConfigurationManager.AppSettings["JDtypeInYCPG"];
                        listYCData = new YCPG().GetSYSJ(dtFrom, dtTo, dataType, taskNo, satID);
                        BindYCDataSource(listYCData);
                    }
                    catch (Exception ex)
                    {
                        throw (new AspNetException("空间机动试验数据，获取出现异常", ex));
                    }
                    finally { }
                    break;
                case "2"://FZ
                    try
                    {
                        List<JH> listFZData = new JH().GetJHList("TYSJ", dtFrom, dtTo, DateTime.MinValue, DateTime.MinValue);
                        BindFZDataSource(listFZData);
                    }
                    catch (Exception ex)
                    {
                        throw (new AspNetException("仿真推演试验数据，获取出现异常", ex));
                    }
                    finally { }
                    break;
            }
            listGDData = (new GD()).GetList(dtFrom, dtTo, ucOutTask1.SelectedValue, "");
            BindGDDataSource(listGDData);
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            ClearLinkButton();
            BindData(false);

            string strYCIds = hfycids.Value;
            string strUFIds = hfufids.Value;
            string strFZIds = hffzids.Value;
            string strGDIds = hfgdids.Value;

            if (strYCIds.Equals(string.Empty) && strUFIds.Equals(string.Empty) 
                && strFZIds.Equals(string.Empty) && strGDIds.Equals(string.Empty))
            {
                ShowMessage("没有选择要发送的数据。");
                return;
            }

            #region 判断是否多选了，一种类型试验数据的子类只允许选一个（不要做限制了）
            /*
            bool blValid = true;
            string[] ycids = new string[0];
            if (!strYCIds.Equals(string.Empty))
                ycids = strYCIds.Split(new char[] { ',' });
            string[] ufids = new string[0];
            if (!strUFIds.Equals(string.Empty))
                ufids = strUFIds.Split(new char[] { ',' });
            string[] fzids = new string[0];
            if (!strFZIds.Equals(string.Empty))
                fzids = strFZIds.Split(new char[] { ',' });
            string types = string.Empty;
            string stmp = string.Empty;
            switch (ddlDataType.SelectedValue)
            {
                case "0"://TJ
                    if (ycids.Length > 1)
                    {
                        ShowMessage("只允许选择一条状态数据。");
                        return;
                    }
                    for (int i = 0; i < ufids.Length; i++)
                    {
                        stmp = ufids[i].Substring(ufids[i].IndexOf(';') + 1);
                        if (types.IndexOf(stmp) >= 0)
                        {
                            ShowMessage("一类图像数据只允许选择一条。");
                            blValid = false;
                            break;
                        }
                        else
                            types += stmp;
                    }
                    break;
                case "1"://KJ
                    for (int i = 0; i < ycids.Length; i++)
                    {
                        stmp = ycids[i].Substring(ycids[i].IndexOf(';') + 1);
                        if (types.IndexOf(stmp) >= 0)
                        {
                            ShowMessage("一类数据只允许选择一条。");
                            blValid = false;
                            break;
                        }
                        else
                            types += stmp;
                    }
                    break;
                case "2"://FZ
                    if (fzids.Length > 1)
                    {
                        ShowMessage("只允许选择一条仿真推演试验数据。");
                        return;
                    }
                    else if (fzids.Length == 1)
                        fzids[0] = fzids[0].Substring(0, fzids[0].IndexOf(';'));
                    break;
            }
            if (!blValid)
                return;
            */
            #endregion

            //Thread oThread = new Thread(new ThreadStart(Data2FileAndSend));
            //oThread.Start();
            //ShowMessage("已提交后台进行处理。");
            Data2FileAndSend();
        }

        private void Data2FileAndSend()
        {
            string[] filePaths;
            string[] gdFilePaths;
            string msg = string.Empty;
            Data2File(true, out filePaths, out gdFilePaths, out msg);
            if (!msg.Equals(string.Empty))
                ShowMessage("文件发送请求提交失败<br>失败原因：" + msg);
            else
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                if (filePaths != null)
                {
                    for (int i = 0; i < filePaths.Length; i++)
                    {
                        sb.Append(filePaths[i] + "<br>");
                    }
                }
                if (gdFilePaths != null)
                {
                    for (int i = 0; i < gdFilePaths.Length; i++)
                    {
                        sb.Append(gdFilePaths[i] + "<br>");
                    }
                }
                ShowMessage("文件发送请求已提交成功，文件路径如下：<br>" + sb.ToString());
            }
        }

        /// <summary>
        /// 把数据转成文件
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="dataType"></param>
        private void Data2File(bool send, out string[] filePaths, out string[] gdFilePaths, out string msg)
        {
            string strYCIds = hfycids.Value;
            string strUFIds = hfufids.Value;
            string strFZIds = hffzids.Value;
            string strGDIds = hfgdids.Value;
            string sendWay = hfsendway.Value;
            msg = string.Empty;
            filePaths = null;
            gdFilePaths = null;
            DateTime dt = DateTime.Now;
            string[] ycids = new string[0];
            if (!strYCIds.Equals(string.Empty))
                ycids = strYCIds.Split(new char[] { ',' });
            string[] ufids = new string[0];
            if (!strUFIds.Equals(string.Empty))
                ufids = strUFIds.Split(new char[] { ',' });
            string[] fzids = new string[0];
            if (!strFZIds.Equals(string.Empty))
                fzids = strFZIds.Split(new char[] { ',' });

            #region 隐藏域中存的是ID+type，需重置IDs
            string types = string.Empty;
            string stmp = string.Empty;
            switch (ddlDataType.SelectedValue)
            {
                case "0"://TJ
                    if (ycids.Length == 1)
                        ycids[0] = ycids[0].Substring(0, ycids[0].IndexOf(';'));
                    for (int i = 0; i < ufids.Length; i++)
                    {
                        ufids[i] = ufids[i].Substring(0, ufids[i].IndexOf(';'));
                    }
                    break;
                case "1"://KJ
                    for (int i = 0; i < ycids.Length; i++)
                    {
                        ycids[i] = ycids[i].Substring(0, ycids[i].IndexOf(';'));
                    }
                    break;
                case "2"://FZ
                    if (fzids.Length == 1)
                        fzids[0] = fzids[0].Substring(0, fzids[0].IndexOf(';'));
                    break;
            }
            #endregion
            if (sendWay.Equals(string.Empty))
                sendWay = "0";

            BizFileCreator oBFCreator;
            string taskNo = ucOutTask1.SelectedValue;
            int desID = 0;
            string dataType = string.Empty;
            switch (ddlDataType.SelectedValue)
            {
                case "0"://TJ
                    oBFCreator = new BizFileCreator();
                    dataType = "GCSJ";
                    desID = new XYXSInfo().GetByAddrMark(FileExchangeConfig.GetTgtListForSending(dataType)[0]).Id;
                    if ((ycids != null && ycids.Length > 0) || (ufids != null && ufids.Length > 0))
                        oBFCreator.CreateAndSendGCSJDataFile(dataType, ycids, ufids, taskNo
                            , (CommunicationWays)Convert.ToInt32(sendWay), send, desID, out filePaths, out msg);
                    break;
                case "1"://KJ
                    oBFCreator = new BizFileCreator();
                    dataType = "JDSJ";
                    desID = new XYXSInfo().GetByAddrMark(FileExchangeConfig.GetTgtListForSending(dataType)[0]).Id;
                    if (ycids != null && ycids.Length > 0)
                        oBFCreator.CreateAndSendJDSJDataFile(dataType, ycids, taskNo
                            , (CommunicationWays)Convert.ToInt32(sendWay), send, desID, out filePaths, out msg);
                    break;
                case "2"://FZ
                    dataType = "TYSJ";
                    PlanFileCreator oPFCreator = new PlanFileCreator();
                    XYXSInfo oXyxs =  new XYXSInfo().GetByAddrMark(FileExchangeConfig.GetTgtListForSending(dataType)[0]);
                    desID = oXyxs.Id;
                    if (strFZIds.Equals(string.Empty))
                        break;
                    //string filename = oPFCreator.CreateSendingTYSJFile(strFZIds, oXyxs.ADDRMARK, oXyxs.ADDRName + "(" + oXyxs.EXCODE + ")");
                    string filename = oPFCreator.CreateSendingTYSJFile(strFZIds, oXyxs.ADDRMARK);
                    if (!send)
                        return;//不发送

                    int sourceID = new XYXSInfo().GetByAddrMark(Param.SourceCode).Id;
                    int infoId = new InfoType().GetIDByExMark(dataType);
                    FileSender oSender = new FileSender();
                    filePaths = new string[1];
                    filePaths[0] = Param.OutPutPath + filename;
                    oSender.SendFile(filename, Param.OutPutPath, (CommunicationWays)Convert.ToInt32(sendWay), sourceID, desID, infoId, true);
                    break;
            }
            if (msg.Equals(string.Empty))//上面的执行都没有出错
                CreateAndSendGDFile(send, strGDIds, desID, out gdFilePaths);
        }

        private void CreateAndSendGDFile(bool send, string ids, int desID, out string[] filePaths)
        {
            filePaths = null;
            try
            {
                PlanFileCreator creater = new PlanFileCreator();
                string SendingFilePaths = creater.CreateSendingGDGSFile(ids, desID);
                filePaths = SendingFilePaths.Split(new char[] { ',' });
                if (send && !ids.Equals(string.Empty))
                {
                    int sourceID = new XYXSInfo().GetByAddrMark(Param.SourceCode).Id;
                    int infoId = new InfoType().GetIDByExMark("GD");
                    string sendWay = hfsendway.Value;
                    if (sendWay.Equals(string.Empty))
                        sendWay = "0";
                    FileSender oSender = new FileSender();
                    for (int i = 0; i < filePaths.Length; i++)
                    {
                        oSender.SendFile(filePaths[i], Param.OutPutPath, (CommunicationWays)Convert.ToInt32(sendWay)
                            , sourceID, desID, infoId, true);
                    }
                }
            }
            catch (Exception ex)
            {
                throw (new AspNetException("试验数据分发-生成轨道数据文件出现异常，异常原因", ex));
            }
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            string[] filePaths;
            string[] gdFilePaths;
            string msg = string.Empty;
            BindData(false);
            Data2File(false, out filePaths, out gdFilePaths, out msg);
            if (!msg.Equals(string.Empty))
                ShowMessage("文件生成失败<br>失败原因：" + msg);
            else
            {
                string paths = string.Empty;
                if (filePaths != null)
                {
                    for (int i = 0; i < filePaths.Length; i++)
                    {
                        paths += filePaths[i] + ",";
                    }
                }
                if (gdFilePaths != null)
                {
                    for (int i = 0; i < gdFilePaths.Length; i++)
                    {
                        paths += gdFilePaths[i] + ",";
                    }
                }
                if (!paths.Equals(string.Empty))
                {
                    ShowMessage("文件已生成成功");
                    this.ViewState["showfile"] = "1";
                    this.ViewState["filepath"] = paths.TrimEnd(new char[]{','}).Split(new char[]{','});
                    AddLinkButton();
                }
                else
                    ShowMessage("没有生成如何文件");
            }
        }

        private void ClearLinkButton()
        {
            this.ViewState["showfile"] = null;
            divFilePath.Visible = false;
        }

        private void AddLinkButton()
        {
            string[] filePaths = (string[])this.ViewState["filepath"];
            LinkButton lbFile;
            for (int i = 0; i < filePaths.Length; i++)
            {
                lbFile = new LinkButton();
                lbFile.Text = filePaths[i];
                lbFile.Click += new EventHandler(lbtFilePath_Click);
                divFilePath.Controls.Add(lbFile);
            }
            divFilePath.Visible = true;
        }

        protected void lbtFilePath_Click(object sender, EventArgs e)
        {
            try
            {
                string strFilePath = ((LinkButton)sender).Text.Trim();
                if (string.IsNullOrEmpty(strFilePath) || !System.IO.File.Exists(strFilePath))
                {
                    ShowMessage("文件不存在。");
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
                throw (new AspNetException("试验数据分发-生成文件出现异常，异常原因", ex));
            }
        }
    }
}
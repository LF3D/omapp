using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using ServiceBusAPI;
using OperatingManagement.WebKernel.Basic;
using OperatingManagement.Framework.Storage;
using OperatingManagement.Framework.Core;
using OperatingManagement.DataAccessLayer.BusinessManage;
using ServicesKernel.GDFX;
using ServicesKernel;

namespace OperatingManagement.Web.Views.BusinessManage
{
    public partial class GDYB : AspNetPage
    {
        public override void OnPageLoaded()
        {
            this.PagePermission = "OMB_GDYB.Caculate";
            this.ShortTitle = "轨道预报";
            this.SetTitle();
            this.AddJavaScriptInclude("scripts/pages/BusinessManage/GDYB.aspx.js");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                txtFrom.Attributes.Add("readonly", "true");
                ucSatellite1.AllowBlankItem = false;
                InitPage();
            }
        }

        private void InitPage()
        {
            // Bind cblXyxs DataSource
            cblXyxs1.Items.Clear();
            //rblDMZ.Items.Clear();
            //只选出DMZ
            cblXyxs1.DataSource = new XYXSInfo().Cache.Where(a => a.Type == 0).ToList();
            cblXyxs1.DataTextField = "ADDRName";
            cblXyxs1.DataValueField = "INCODE";
            cblXyxs1.DataBind();
            cblXyxs1.SelectedIndex = 0;
        }

        protected void btnCalculate_Click(object sender, EventArgs e)
        {
            ObsPrer oPrer = new ObsPrer();
            string strResult = string.Empty;
            string strFullName = string.Empty;
            string resultPath = string.Empty;
            DateTime dt = DateTime.Parse(txtFrom.Text);
            int preDays = int.Parse(txtDays.Text.Trim());
            int interval = int.Parse(txtTimeSpan.Text.Trim());
            bool qcy = false;
            if (rb1.Checked)
                qcy = true;
            int qc = 0;
            List<string> dmzids = new List<string>();
            #region 检查各项条件
            foreach (ListItem item in cblXyxs1.Items)
            {
                if (item.Selected)
                    dmzids.Add(item.Value);
            }
            if (dmzids.Count() == 0)
            {
                ShowMessage("请选择至少一个地面站。");
                return;
            }
            if (qcy)
            {
                if (txtQC.Text.Trim() == "")
                {
                    ShowMessage("请设置圈次。");
                    return;
                }
                try
                {
                    qc = int.Parse(txtQC.Text.Trim());
                }
                catch (Exception ex)
                {
                    ShowMessage("请设置合法的圈次（整数）。");
                    return;
                }
            }
            #endregion

            strResult = oPrer.DoCaculate(dt, preDays, interval, ucSatellite1.SelectedValue
                , dmzids.ToArray(), qcy, qc, out resultPath);
            //resultPath = @"D:\WorkingForder\htc\Dev\ref\轨道分析\Version 2.0 20120723\测试数据\ObsPredict\output\";
            if (!string.IsNullOrEmpty(strResult))
            {
                ShowMessage(strResult);
                divCalResult.Visible = false;
                return;
            }
            else
            {
                lblResultFilePath.Text = resultPath;
                ShowMessage("轨道预报成功。");
                divCalResult.Visible = true;
            }
        }

        private void ShowMessage(string msg)
        {
            ltMessage.Text = msg;
            ltMessage.Visible = true;
        }

        private void DownLoadFile(string fileType)
        {
            try
            {
                string fileFullName = GetFileFullName(fileType);
                if (string.IsNullOrEmpty(fileFullName) || !File.Exists(fileFullName))
                {
                    ShowMessage(string.Format("{0}计算结果文件不存在。", fileType));
                    return;
                }

                Response.Clear();
                Response.Buffer = false;
                Response.ContentType = "application/octet-stream";
                Response.AppendHeader("content-disposition", "attachment;filename=" + Path.GetFileName(fileFullName) + ";");
                Response.Write(File.ReadAllText(fileFullName));
                Response.Flush();
                Response.End();
            }
            catch (System.Threading.ThreadAbortException ex1)
            { }
            catch (Exception ex)
            {
                throw (new AspNetException(string.Format("轨道分析 - 轨道预报页面另存{0}计算结果出现异常，异常原因", fileType), ex));
            }
        }

        private string GetFileFullName(string fileType)
        {
            string strFilePath = lblResultFilePath.Text;
            if (strFilePath.Equals(string.Empty))
            {
                ShowMessage("文件路径不存在");
                return string.Empty;
            }

            string[] fileNames = Directory.GetFiles(strFilePath);
            string fileFullName = string.Empty;
            string strTmp = string.Empty;
            for (int i = 0; i < fileNames.Length; i++)
            {
                strTmp = fileNames[i].Substring(fileNames[i].LastIndexOf(@"\") + 1);
                if (strTmp.Length > fileType.Length)
                {
                    if (strTmp.Substring(0, fileType.Length + 1).ToUpper() == fileType.ToUpper() + "_")
                    {
                        fileFullName = fileNames[i];
                        break;
                    }
                }
            }
            return fileFullName;
        }

        protected void lbtViewCurves_Click(object sender, EventArgs e)
        {
            string resultFilePath = GetFileFullName("MapJ");
            if (resultFilePath == string.Empty)
                return;
            string fileType = "GDYB_MapJ";
            if (File.Exists(resultFilePath))
                Response.Redirect(string.Format("GDJSCurves.aspx?fp={0}&ft={1}", resultFilePath, fileType));
        }

        protected void lbtEarthShadow_Click(object sender, EventArgs e)
        {
            DownLoadFile(((LinkButton)sender).ID.Substring(3));
        }

        protected void lbtMapJ_Click(object sender, EventArgs e)
        {
            DownLoadFile(((LinkButton)sender).ID.Substring(3));
        }

        protected void lbtMapJK_Click(object sender, EventArgs e)
        {
            DownLoadFile(((LinkButton)sender).ID.Substring(3));
        }

        protected void lbtMapW_Click(object sender, EventArgs e)
        {
            DownLoadFile(((LinkButton)sender).ID.Substring(3));
        }

        protected void lbtMoonTransit_Click(object sender, EventArgs e)
        {
            DownLoadFile(((LinkButton)sender).ID.Substring(3));
        }

        protected void lbtObsGuiding_Click(object sender, EventArgs e)
        {
            DownLoadFile(((LinkButton)sender).ID.Substring(3));
        }

        protected void lbtStaObsPre_Click(object sender, EventArgs e)
        {
            DownLoadFile(((LinkButton)sender).ID.Substring(3));
        }

        protected void lbtStationInOut_Click(object sender, EventArgs e)
        {
            DownLoadFile(((LinkButton)sender).ID.Substring(3));
        }

        protected void lbtSubSatPoint_Click(object sender, EventArgs e)
        {
            DownLoadFile(((LinkButton)sender).ID.Substring(3));
        }

        protected void lbtSunAH_Click(object sender, EventArgs e)
        {
            DownLoadFile(((LinkButton)sender).ID.Substring(3));
        }

        protected void lbtSunTransit_Click(object sender, EventArgs e)
        {
            DownLoadFile(((LinkButton)sender).ID.Substring(3));
        }

        protected void lbtVS_Click(object sender, EventArgs e)
        {
            DownLoadFile("VisibleStatistics");
        }
    }
}
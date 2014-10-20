using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OperatingManagement.WebKernel.Basic;
using OperatingManagement.Framework.Storage;
using OperatingManagement.Framework.Core;
using OperatingManagement.DataAccessLayer.BusinessManage;
using ServicesKernel.GDFX;

namespace OperatingManagement.Web.Views.BusinessManage
{
    public partial class GDJHFX : AspNetPage
    {
        public override void OnPageLoaded()
        {
            this.PagePermission = "OMB_JHFX.Caculate";
            this.ShortTitle = "交会分析";
            this.SetTitle();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            HideMessage();
        }

        protected void btnCalculate_Click(object sender, EventArgs e)
        {
            string filePath = SystemParameters.GetSystemParameterValue(SystemParametersType.GDJSResult, "result_path")
                + SystemParameters.GetSystemParameterValue(SystemParametersType.GDJSResult, "cutana_path");
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);

            if (!fuSubFile.HasFile)
            {
                ShowMessage("主星文件不能为空");
                return;
            }

            if (!Path.GetExtension(fuSubFile.FileName).Trim('.').Equals("dat",
                StringComparison.InvariantCultureIgnoreCase))
            {
                ShowMessage("“主星文件”格式只能为dat。");
                return;
            }

            if (!fuTgtFile.HasFile)
            {
                ShowMessage("目标星文件不能为空");
                return;
            }

            if (!Path.GetExtension(fuTgtFile.FileName).Trim('.').Equals("dat",
                StringComparison.InvariantCultureIgnoreCase))
            {
                ShowMessage("“目标星文件”格式只能为dat。");
                return;
            }

            string subNewPath = Path.Combine(filePath, Guid.NewGuid() + ".dat");
            fuSubFile.SaveAs(subNewPath);

            string tgtNewPath = Path.Combine(filePath, Guid.NewGuid() + ".dat");
            fuTgtFile.SaveAs(tgtNewPath);

            GDFXProcessor oProc = new GDFXProcessor();
            string strResult = string.Empty;
            string resultFileName = string.Empty;
            string resultFilePath = string.Empty;

            try
            {
                strResult = oProc.CutAnalyze(subNewPath, tgtNewPath, out resultFileName);
                if (strResult.Equals(string.Empty))
                {
                    resultFilePath = filePath + @"output\" + resultFileName;
                    lblResultPath.Text = resultFilePath;
                    lblResultFilePath.Text = string.Format("{0}|{1}", resultFilePath.ToLower(),
                        resultFilePath.ToLower().Replace("unw", "stw"));
                    divCalResult.Visible = true;
                }
                else
                    ShowMessage(string.Format("交会分析计算失败，{0}", strResult));
                DeleteFile(subNewPath);
                DeleteFile(tgtNewPath);
            }
            catch (Exception ex)
            {
                throw (new AspNetException("轨道分析 - 交会分析页面计算时出现异常，异常原因", ex));
            }
        }
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="filePath"></param>
        private void DeleteFile(string filePath)
        {
            if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
            {
                try { File.Delete(filePath); }
                catch { }
            }
        }

        private void ShowMessage(string msg)
        {
            trMessage.Visible = true;
            lblMessage.Text = msg;
        }

        private void HideMessage()
        {
            trMessage.Visible = false;
            lblMessage.Text = "";
            divCalResult.Visible = false;
        }
        /// <summary>
        /// UNW 结果文件保存到本地
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtUNWFileDownload_Click(object sender, EventArgs e)
        {
            try
            {
                string resultFilePath = lblResultPath.Text.Trim();
                if (string.IsNullOrEmpty(resultFilePath) || !File.Exists(resultFilePath.Split(new char[]{'|'})[0]))
                {
                    ShowMessage("UNW计算结果文件不存在。");
                    return;
                }

                resultFilePath = resultFilePath + "_UNW.dat";
                Response.Clear();
                Response.Buffer = false;
                Response.ContentType = "application/octet-stream";
                Response.AppendHeader("content-disposition", "attachment;filename=" + Path.GetFileName(resultFilePath) + ";");
                Response.Write(File.ReadAllText(resultFilePath));
                Response.Flush();
                Response.End();
            }
            catch (System.Threading.ThreadAbortException ex1)
            { }
            catch (Exception ex)
            {
                throw (new AspNetException("轨道分析 - 交会分析页面另存UNW计算结果出现异常，异常原因", ex));
            }

        }

        /// <summary>
        /// STW结果保存到本地
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtSTWFileDownload_Click(object sender, EventArgs e)
        {
            try
            {
                string resultFilePath = lblResultPath.Text.Trim();
                if (string.IsNullOrEmpty(resultFilePath) || !File.Exists(resultFilePath.Split(new char[] { '|' })[1]))
                {
                    ShowMessage("STW计算结果文件不存在。");
                    return;
                }

                resultFilePath = resultFilePath + "_STW.dat";
                Response.Clear();
                Response.Buffer = false;
                Response.ContentType = "application/octet-stream";
                Response.AppendHeader("content-disposition", "attachment;filename=" + Path.GetFileName(resultFilePath) + ";");
                Response.Write(File.ReadAllText(resultFilePath));
                Response.Flush();
                Response.End();
            }
            catch (System.Threading.ThreadAbortException ex1)
            { }
            catch (Exception ex)
            {
                throw (new AspNetException("轨道分析 - 交会分析页面另存STW计算结果出现异常，异常原因", ex));
            }
        }

        /// <summary>
        /// 查看计算结果的曲线
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtViewCurves_Click(object sender, EventArgs e)
        {
            if (lblResultPath.Text == "")
                Response.Redirect("GDJSCurves.aspx");

            string resultFilePath = lblResultPath.Text.Trim();
            if (!string.IsNullOrEmpty(resultFilePath) && File.Exists(resultFilePath + "_UNW.dat"))
            {
                Response.Redirect(string.Format("GDJSCurves.aspx?fp={0}&ft=CutAna_UNW", resultFilePath + "_UNW.dat"));
            }
        }
    }
}
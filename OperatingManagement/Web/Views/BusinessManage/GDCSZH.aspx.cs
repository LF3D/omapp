using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OperatingManagement.WebKernel.Basic;
using OperatingManagement.Framework.Storage;
using OperatingManagement.Framework.Core;
using OperatingManagement.DataAccessLayer.BusinessManage;
using ServicesKernel.GDFX;
using System.IO;

namespace OperatingManagement.Web.Views.BusinessManage
{
    public partial class GDCSZH : AspNetPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) {
                BindParameters();
            }
        }
        void BindParameters() {
            var list = OrbitParameters.ReadParameters();
            rblOrbitParameters.DataTextField = "Value";
            rblOrbitParameters.DataValueField = "Id";
            rblOrbitParameters.DataSource = list;
            rblOrbitParameters.DataBind();
            rblOrbitParameters.SelectedIndex = 0;
        }

        protected void btnCalculate_Click(object sender, EventArgs e)
        {
            HideMessage();
            string filePath = SystemParameters.GetSystemParameterValue(SystemParametersType.GDJSResult, "result_path") 
                + SystemParameters.GetSystemParameterValue(SystemParametersType.GDJSResult, "cvt_path");
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);
            string emitFileNewPath = null;

            #region 检查页面中的计算参数
            if (rblOrbitParameters.SelectedItem.Text.IndexOf("发射坐标系") >= 0 ||
                rblOrbitParameters.SelectedItem.Text.IndexOf("发射惯性坐标系") >= 0)
            {
                emitFileNewPath = Path.Combine(filePath, Guid.NewGuid() + ".dat");
                if (!fuParaFile.HasFile)
                {
                    ShowMessage("“发射系相关文件”不能为空。");
                    return;
                }
                if (!Path.GetExtension(fuParaFile.FileName).Trim('.').Equals("dat",
                    StringComparison.InvariantCultureIgnoreCase))
                {
                    ShowMessage("“发射系相关文件”格式只能为DAT。");
                    return;
                }
                fuParaFile.SaveAs(emitFileNewPath);
            }
            if (!fuCalFile.HasFile)
            {
                ShowMessage("“待转换相关文件”不能为空。");
                DeleteFile(emitFileNewPath);
                return;
            }
            if (!Path.GetExtension(fuCalFile.FileName).Trim('.').Equals("dat",
                StringComparison.InvariantCultureIgnoreCase))
            {
                ShowMessage("“待转换相关文件”格式只能为DAT。");
                DeleteFile(emitFileNewPath);
                return;
            }
            string cvtFileNewPath = Path.Combine(filePath, Guid.NewGuid() + ".dat");
            fuCalFile.SaveAs(cvtFileNewPath);
            #endregion

            #region 准备计算参数
            //参数转换计算
            bool blDegree = true;
            if (rblAngle.SelectedValue.ToLower() == "false")
                blDegree = false;
            bool blKM = true;
            if (rblDistance.SelectedValue.ToLower() == "false")
                blKM = false;
            string resultFileName = string.Empty;
            int iTimeZone = 8;
            string strConvertType = rblOrbitParameters.SelectedValue;
            
            //转换类型不包含发射系坐标时，发射系文件不使用的情况下也不能为空
            if (emitFileNewPath == null)
                emitFileNewPath = Path.Combine(filePath, Guid.NewGuid() + ".txt");
            #endregion

            string strResult = new GDFXProcessor().ParamConvert(blDegree, blKM, iTimeZone, strConvertType
                , cvtFileNewPath, emitFileNewPath, out resultFileName);
            if (strResult.Equals(string.Empty))
            {
                //ShowMessage(string.Format("参数转换计算成功"));
                //for 下载文件使用
                lblResultFilePath.Text = resultFileName;
                divCalResult.Visible = true;
            }
            else
                ShowMessage(string.Format("参数转换计算失败，{0}", strResult));
            DeleteFile(cvtFileNewPath);
            DeleteFile(emitFileNewPath);
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
        /// 下载计算结果文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnResultFileDownload_Click(object sender, EventArgs e)
        {
            try
            {
                string resultFilePath = lblResultFilePath.Text.Trim();
                if (string.IsNullOrEmpty(resultFilePath) || !File.Exists(resultFilePath))
                {
                    ShowMessage("计算结果文件不存在。");
                    return;
                }

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
                throw (new AspNetException("轨道分析 - 差值分析页面另存计算结果出现异常，异常原因", ex));
            }
        }

        private void DeleteFile(string filePath)
        {
            if (!string.IsNullOrEmpty(filePath) && File.Exists(filePath))
            {
                try { File.Delete(filePath); }
                catch { }
            }
        }
        public override void OnPageLoaded()
        {
            this.PagePermission = "OMB_CSZH.Caculate";
            this.ShortTitle = "参数转换";
            this.SetTitle();
            this.AddJavaScriptInclude("scripts/pages/businessmanage/orbitparacalculate.aspx.js");
        }
    }
}
#region
//------------------------------------------------------
//Assembly:OperatingManagement.Web
//FileName:OrbitDifferenceAnalysis.cs
//Remark:业务管理-轨道分析-差值分析
//------------------------------------------------------
//VERSION       AUTHOR      DATE        CONTENT
//1.0           liutao      20120519     Create     
//------------------------------------------------------
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

using OperatingManagement.Framework.Core;
using OperatingManagement.WebKernel.Basic;
using OperatingManagement.DataAccessLayer.BusinessManage;

namespace OperatingManagement.Web.Views.BusinessManage
{
    public partial class OrbitDifferenceAnalysis : AspNetPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                trMessage.Visible = false;
                lblMessage.Text = string.Empty;
            }
            catch (Exception ex)
            {
                trMessage.Visible = true;
                lblMessage.Text = "发生未知错误，操作失败。";
                throw (new AspNetException("轨道分析 - 差值分析页面初始化出现异常，异常原因", ex));
            }
        }
        /// <summary>
        /// 开始计算差值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCalculate_Click(object sender, EventArgs e)
        {
            try
            {
                divCalResult.Visible = false;
                if (!fuXLDataFile.HasFile)
                {
                    //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alert", "alert(\"请选择星历数据文件。\")", true);
                    trMessage.Visible = true;
                    lblMessage.Text = "请选择星历数据文件。";
                    return;
                }
                if (fuXLDataFile.PostedFile.ContentLength == 0)
                {
                    //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alert", "alert(\"星历数据文件不能为空。\")", true);
                    trMessage.Visible = true;
                    lblMessage.Text = "星历数据文件不能为空。";
                    return;
                }
                if (!fuDifCalTimeFile.HasFile)
                {
                    //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alert", "alert(\"请选择差值计算时间文件。\")", true);
                    trMessage.Visible = true;
                    lblMessage.Text = "请选择差值计算时间文件。";
                    return;
                }
                if (fuDifCalTimeFile.PostedFile.ContentLength == 0)
                {
                    //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alert", "alert(\"差值计算时间文件不能为空。\")", true);
                    trMessage.Visible = true;
                    lblMessage.Text = "差值计算时间文件不能为空。";
                    return;
                }

                //校验星历数据文件扩展名、文件大小等合法性
                int fileSize = fuXLDataFile.PostedFile.ContentLength;
                string fileExtension = Path.GetExtension(fuXLDataFile.PostedFile.FileName);
                int fileMaxSize = int.Parse(SystemParameters.GetSystemParameterValue(SystemParametersType.OrbitDifferenceAnalysis, "XLDataFileMaxSize"));
                string allowFileExtension = SystemParameters.GetSystemParameterValue(SystemParametersType.OrbitDifferenceAnalysis, "XLDataFileExtension").Trim(new char[] { ',', '，', ';', '；' });
                string[] extensionArray = allowFileExtension.Split(new char[] { ',', '，', ';', '；' }, StringSplitOptions.RemoveEmptyEntries);
                bool allowExtension = false;
                if (extensionArray != null && extensionArray.Length > 0)
                {
                    foreach (string extension in extensionArray)
                    {
                        if (extension.Trim(new char[] { ' ', '.' }).ToLower() == fileExtension.Trim(new char[] { ' ', '.' }).ToLower())
                        {
                            allowExtension = true;
                            break;
                        }
                    }
                }
                if (!allowExtension)
                {
                    string message = string.Format("上传星历数据文件格式不正确，应为：{0}。", allowFileExtension);
                    //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alert", "alert(\"" + message + "\")", true);
                    trMessage.Visible = true;
                    lblMessage.Text = message;
                    return;
                }
                if (fileSize > fileMaxSize)
                {
                    string message = string.Format("上传星历数据文件不能超过{0}字节", fileMaxSize.ToString());
                    //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alert", "alert(\"" + message + "\")", true);
                    trMessage.Visible = true;
                    lblMessage.Text = message;
                    return;
                }

                //校验差值计算时间文件扩展名、文件大小等合法性
                fileSize = fuDifCalTimeFile.PostedFile.ContentLength;
                fileExtension = Path.GetExtension(fuDifCalTimeFile.PostedFile.FileName);
                fileMaxSize = int.Parse(SystemParameters.GetSystemParameterValue(SystemParametersType.OrbitDifferenceAnalysis, "DifCalTimeFileMaxSize"));
                allowFileExtension = SystemParameters.GetSystemParameterValue(SystemParametersType.OrbitDifferenceAnalysis, "DifCalTimeFileExtension").Trim(new char[] { ',', '，', ';', '；' });
                extensionArray = allowFileExtension.Split(new char[] { ',', '，', ';', '；' }, StringSplitOptions.RemoveEmptyEntries);
                allowExtension = false;
                if (extensionArray != null && extensionArray.Length > 0)
                {
                    foreach (string extension in extensionArray)
                    {
                        if (extension.Trim(new char[] { ' ', '.' }).ToLower() == fileExtension.Trim(new char[] { ' ', '.' }).ToLower())
                        {
                            allowExtension = true;
                            break;
                        }
                    }
                }
                if (!allowExtension)
                {
                    string message = string.Format("上传差值计算时间文件格式不正确，应为：{0}。", allowFileExtension);
                    //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alert", "alert(\"" + message + "\")", true);
                    trMessage.Visible = true;
                    lblMessage.Text = message;
                    return;
                }
                if (fileSize > fileMaxSize)
                {
                    string message = string.Format("上传差值计算时间文件不能超过{0}字节", fileMaxSize.ToString());
                    //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alert", "alert(\"" + message + "\")", true);
                    trMessage.Visible = true;
                    lblMessage.Text = message;
                    return;
                }

                string xlDataFileDirectory = SystemParameters.GetSystemParameterValue(SystemParametersType.OrbitDifferenceAnalysis, "XLDataFileDirectory");
                if (!Directory.Exists(xlDataFileDirectory))
                    Directory.CreateDirectory(xlDataFileDirectory);
                //星历数据文件服务器路径
                string xlDataFilePath = Path.Combine(xlDataFileDirectory, Guid.NewGuid().ToString() + Path.GetExtension(fuXLDataFile.PostedFile.FileName));
                fuXLDataFile.PostedFile.SaveAs(xlDataFilePath);

                string difCalTimeFileDirectory = SystemParameters.GetSystemParameterValue(SystemParametersType.OrbitDifferenceAnalysis, "DifCalTimeFileDirectory");
                if (!Directory.Exists(difCalTimeFileDirectory))
                    Directory.CreateDirectory(difCalTimeFileDirectory);
                //差值计算时间文件服务器路径
                string difCalTimeFilePath = Path.Combine(difCalTimeFileDirectory, Guid.NewGuid().ToString() + Path.GetExtension(fuDifCalTimeFile.PostedFile.FileName));
                fuDifCalTimeFile.PostedFile.SaveAs(difCalTimeFilePath);

                ltXLDataFilePath.Text = fuXLDataFile.PostedFile.FileName;
                ltDifCalTimeFilePath.Text = fuDifCalTimeFile.PostedFile.FileName;

                ltXLDataFile.Text = File.ReadAllText(xlDataFilePath, System.Text.Encoding.Default);
                ltDifCalTimeFile.Text = File.ReadAllText(difCalTimeFilePath, System.Text.Encoding.Default);

                //定义计算结果
                bool calResult = false;
                //定义结果文件路径
                string resultFilePath = string.Empty; //@"D:\ResourceCalculate\ResultFileDirectory\2f318cd1-82ba-4593-9884-263cfb2887bd.txt";

                /**
                * TODO: 在这里开始计算，将结果calResult和结果文件路径resultFilePath赋值
                * */
                //System.Threading.Thread.Sleep(10000);

                lblResultFilePath.Text = resultFilePath;
                lblCalResult.Text = calResult ? "计算成功" : "计算失败";
                if (!string.IsNullOrEmpty(resultFilePath) && File.Exists(resultFilePath))
                {
                    ltResultFile.Text = File.ReadAllText(resultFilePath, System.Text.Encoding.Default);
                }
                divCalResult.Visible = true;

                DeleteFile(xlDataFilePath);
                DeleteFile(difCalTimeFilePath);

                ClientScript.RegisterClientScriptBlock(this.GetType(),
                   "open-dialog",
                   "var _autoOpen=true;",
                   true);
            }
            catch(Exception ex)
            {
                trMessage.Visible = true;
                lblMessage.Text = "发生未知错误，操作失败。";
                throw (new AspNetException("轨道分析 - 差值分析页面btnCalculate_Click方法出现异常，异常原因", ex));
            }
        }
        /// <summary>
        /// 清除所有信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnResetAll_Click(object sender, EventArgs e)
        {
            try 
            {
                ResetResultControls();
            }
            catch (Exception ex)
            {
                trMessage.Visible = true;
                lblMessage.Text = "发生未知错误，操作失败。";
                throw (new AspNetException("轨道分析 - 差值分析页面btnResetAll_Click方法出现异常，异常原因", ex));
            }
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
                if (string.IsNullOrEmpty(resultFilePath))
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "计算结果文件不存在。";
                    return;
                }

                if (!File.Exists(resultFilePath))
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "计算结果文件不存在。";
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
            catch(Exception ex)
            {
                //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "alert", "alert(\"下载计算结果文件失败。\")", true);
                trMessage.Visible = true;
                lblMessage.Text = "下载计算结果文件失败。";
                throw (new AspNetException("轨道分析 - 差值分析页面lbtnResultFileDownload_Click方法出现异常，异常原因", ex));
            }
        }

        public override void OnPageLoaded()
        {
            this.PagePermission = "OMB_CZFX.Caculate";
            this.ShortTitle = "差值分析";
            this.SetTitle();
            this.AddJavaScriptInclude("scripts/pages/businessmanage/OrbitDifferenceAnalysis.aspx.js");
        }

        #region Method
        /// <summary>
        /// 重置计算结果相关控件
        /// </summary>
        private void ResetResultControls()
        {
            divCalResult.Visible = false;
            lblResultFilePath.Text = string.Empty;
            lblCalResult.Text = string.Empty;

            ltXLDataFilePath.Text = string.Empty;
            ltDifCalTimeFilePath.Text = string.Empty;
            ltXLDataFile.Text = string.Empty;
            ltDifCalTimeFile.Text = string.Empty;
            ltResultFile.Text = string.Empty;
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
        #endregion
    }
}
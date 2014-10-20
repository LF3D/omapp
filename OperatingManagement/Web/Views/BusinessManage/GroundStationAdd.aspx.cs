#region
//------------------------------------------------------
//Assembly:OperatingManagement.Web
//FileName:GroundStationAdd.cs
//Remark:地面站添加类
//------------------------------------------------------
//VERSION       AUTHOR      DATE        CONTENT
//1.0           liutao      20120825    Create     
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
    public partial class GroundStationAdd : AspNetPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                trMessage.Visible = false;
                lblMessage.Text = string.Empty;
                if (!IsPostBack)
                {
                    BindDataSource();
                }
            }
            catch (Exception ex)
            {
                trMessage.Visible = true;
                lblMessage.Text = "发生未知错误，操作失败。";
                throw (new AspNetException("新增地面站页面初始化出现异常，异常原因", ex));
            }
        }
        /// <summary>
        /// 提交添加地面站记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string msg = string.Empty;
            double longitudeValue = 0.0;
            double latitudeValue = 0.0;
            double gaoCheng = 0.0;
            int tcpPort = 0;
            int udpPort = 0;
            #region Check Input
            try
            {
                if (string.IsNullOrEmpty(txtAddrName.Text.Trim()))
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "地面站名称不能为空";
                    return;
                }

                if (string.IsNullOrEmpty(txtAddrMark.Text.Trim()))
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "地面站编号不能为空";
                    return;
                }

                if (string.IsNullOrEmpty(txtInCode.Text.Trim()))
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "内部编码不能为空";
                    return;
                }

                if (string.IsNullOrEmpty(txtExCode.Text.Trim()))
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "外部编码不能为空";
                    return;
                }

                if (string.IsNullOrEmpty(dplOwn.SelectedValue))
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "管理单位不能为空";
                    return;
                }

                if (string.IsNullOrEmpty(dplCoordinate.SelectedValue))
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "站址坐标不能为空";
                    return;
                }

                if (string.IsNullOrEmpty(txtLongitude.Text.Trim()))
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "经度坐标值不能为空";
                    return;
                }
                if (!double.TryParse(txtLongitude.Text.Trim(), out longitudeValue))
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "经度坐标值格式错误";
                    return;
                }
                if (string.IsNullOrEmpty(txtLatitude.Text.Trim()))
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "纬度坐标值不能为空";
                    return;
                }
                if (!double.TryParse(txtLatitude.Text.Trim(), out latitudeValue))
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "纬度坐标值格式错误";
                    return;
                }

                if (string.IsNullOrEmpty(txtGaoCheng.Text.Trim()))
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "高程坐标值不能为空";
                    return;
                }
                if (!double.TryParse(txtGaoCheng.Text.Trim(), out gaoCheng))
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "高程坐标值格式错误";
                    return;
                }

                if (!string.IsNullOrEmpty(txtTCPPort.Text.Trim()) && !int.TryParse(txtTCPPort.Text.Trim(), out tcpPort))
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "TCP端口格式错误";
                    return;
                }

                if (!string.IsNullOrEmpty(txtUDPPort.Text.Trim()) && !int.TryParse(txtUDPPort.Text.Trim(), out udpPort))
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "UDP端口格式错误";
                    return;
                }
            }
            catch (Exception ex)
            {
                trMessage.Visible = true;
                lblMessage.Text = "发生未知错误，操作失败。";
                throw (new AspNetException("新增地面站页面保存数据-校验数据出现异常，异常原因", ex));
            }
            #endregion
            //string ftpPath = string.Empty;
                //if (!string.IsNullOrEmpty(txtFTPPath.Text.Trim()) || !string.IsNullOrEmpty(txtFTPUser.Text.Trim()) || !string.IsNullOrEmpty(txtFTPPwd.Text.Trim()))
                //{
                //    ftpPath = txtFTPPath.Text.Trim() + "@" + txtFTPUser.Text.Trim() + "@" + txtFTPPwd.Text.Trim();
            //}

            #region Save Data
            try
            {
                Framework.FieldVerifyResult result;
                XYXSInfo xyxsInfo = new XYXSInfo();
                xyxsInfo.ADDRName = txtAddrName.Text.Trim();
                xyxsInfo.ADDRMARK = txtAddrMark.Text.Trim();
                xyxsInfo.INCODE = txtInCode.Text.Trim();
                xyxsInfo.EXCODE = txtExCode.Text.Trim();
                xyxsInfo.Own = dplOwn.SelectedValue;
                xyxsInfo.Coordinate = longitudeValue.ToString() + "," + latitudeValue.ToString() + "," + gaoCheng.ToString();
                xyxsInfo.MainIP = txtMainIP.Text.Trim();
                xyxsInfo.TCPPort = tcpPort;
                xyxsInfo.BakIP = txtBakIP.Text.Trim();
                xyxsInfo.UDPPort = udpPort;
                xyxsInfo.FTPPath = txtFTPPath.Text.Trim();
                xyxsInfo.FTPUser = txtFTPUser.Text.Trim();
                xyxsInfo.FTPPwd = txtFTPPwd.Text.Trim();
                xyxsInfo.Type = 0;
                xyxsInfo.Status = 1;//正常
                xyxsInfo.CreatedTime = DateTime.Now;
                xyxsInfo.CreatedUserID = LoginUserInfo.Id;
                xyxsInfo.DWCode = txtDWCode.Text.Trim();

                result = xyxsInfo.Add();
                switch (result)
                {
                    case Framework.FieldVerifyResult.Error:
                        msg = "发生了数据错误，无法完成请求的操作。";
                        break;
                    case Framework.FieldVerifyResult.Success:
                        msg = "添加地面站成功。";
                        ResetControls();
                        break;
                    default:
                        msg = "发生未知错误，操作失败。";
                        break;
                }
                trMessage.Visible = true;
                lblMessage.Text = msg;
            }
            catch (Exception ex)
            {
                trMessage.Visible = true;
                lblMessage.Text = "发生未知错误，操作失败。";
                throw (new AspNetException("新增地面站页面btnSubmit_Click方法出现异常，异常原因", ex));
            }
            #endregion
        }
        /// <summary>
        /// 清除当前控件的值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                ResetControls();
            }
            catch (System.Threading.ThreadAbortException ex1)
            { }
            catch (Exception ex)
            {
                trMessage.Visible = true;
                lblMessage.Text = "发生未知错误，操作失败。";
                throw (new AspNetException("新增地面站页面btnReset_Click方法出现异常，异常原因", ex));
            }
        }
        /// <summary>
        /// 返回
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnReturn_Click(object sender, EventArgs e)
        {
            try
            {
                string url = @"~/Views/BusinessManage/GroundStationManage.aspx";
                Response.Redirect(url);
            }
            catch (System.Threading.ThreadAbortException ex1)
            { }
            catch (Exception ex)
            {
                trMessage.Visible = true;
                lblMessage.Text = "发生未知错误，操作失败。";
                throw (new AspNetException("新增地面站页面btnReturn_Click方法出现异常，异常原因", ex));
            }
        }

        public override void OnPageLoaded()
        {
            this.PagePermission = "OMB_GSMan.Add";
            this.ShortTitle = "新增地面站";
            this.SetTitle();
        }

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
            //dplOwner.Items.Insert(0, new ListItem("请选择", ""));

            dplCoordinate.Items.Clear();
            dplCoordinate.DataSource = SystemParameters.GetSystemParameters(SystemParametersType.GroundResourceCoordinate);
            dplCoordinate.DataTextField = "key";
            dplCoordinate.DataValueField = "value";
            dplCoordinate.DataBind();
            //dplCoordinate.Items.Insert(0, new ListItem("请选择", ""));
        }
        /// <summary>
        /// 添加完成后将控件设置为初始状态
        /// </summary>
        private void ResetControls()
        {
            dplOwn.SelectedIndex = 0;
            dplCoordinate.SelectedIndex = 0;

            txtAddrName.Text = string.Empty;
            txtAddrMark.Text = string.Empty;
            txtInCode.Text = string.Empty;
            txtExCode.Text = string.Empty;
            txtLongitude.Text = string.Empty;
            txtLatitude.Text = string.Empty;
            txtGaoCheng.Text = string.Empty;
            txtMainIP.Text = string.Empty;
            txtTCPPort.Text = string.Empty;
            txtBakIP.Text = string.Empty;
            txtUDPPort.Text = string.Empty;
            txtFTPPath.Text = string.Empty;
            txtFTPUser.Text = string.Empty;
            txtFTPPwd.Text = string.Empty;
            txtDWCode.Text = string.Empty;
        }

        #endregion
    }
}
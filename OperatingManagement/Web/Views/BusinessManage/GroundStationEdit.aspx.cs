#region
//------------------------------------------------------
//Assembly:OperatingManagement.Web
//FileName:GroundStationEdit.cs
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
    public partial class GroundStationEdit : AspNetPage
    {
        #region 属性
        /// <summary>
        /// 地面站ID
        /// </summary>
        protected int RID
        {
            get
            {
                int rID = 0;
                if (Request.QueryString["rid"] != null)
                {
                    int.TryParse(Request.QueryString["rid"], out rID);
                }
                return rID;
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                trMessage.Visible = false;
                lblMessage.Text = string.Empty;
                if (!IsPostBack)
                {
                    BindDataSource();
                    BindControls();
                }
            }
            catch (Exception ex)
            {
                trMessage.Visible = true;
                lblMessage.Text = "发生未知错误，操作失败。";
                throw (new AspNetException("编辑地面站页面初始化出现异常，异常原因", ex));
            }
        }
        /// <summary>
        /// 提交更新地面站记录
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
                throw (new AspNetException("编辑地面站页面保存数据-校验数据出现异常，异常原因", ex));
            }
            #endregion

            #region Save Data
            try
            {
                Framework.FieldVerifyResult result;
                XYXSInfo xyxsInfo = new XYXSInfo();
                xyxsInfo.Id = RID;
                xyxsInfo = xyxsInfo.SelectByID();
                if (xyxsInfo == null)
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "修改的地面站不存在";
                    return;
                }
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
                //xyxsInfo.Status = 1;//正常
                //xyxsInfo.CreatedTime = DateTime.Now;
                //xyxsInfo.CreatedUserID = LoginUserInfo.Id;
                xyxsInfo.UpdatedTime = DateTime.Now;
                xyxsInfo.UpdatedUserID = LoginUserInfo.Id;
                xyxsInfo.DWCode = txtDWCode.Text.Trim();

                result = xyxsInfo.Update();
                switch (result)
                {
                    case Framework.FieldVerifyResult.Error:
                        msg = "发生了数据错误，无法完成请求的操作。";
                        break;
                    case Framework.FieldVerifyResult.Success:
                        msg = "修改地面站成功。";
                        BindControls();
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
                throw (new AspNetException("编辑地面站页面保存数据时出现异常，异常原因", ex));
            }
            #endregion
        }
        /// <summary>
        /// 重置当前控件的值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                BindControls();
            }
            catch (System.Threading.ThreadAbortException ex1)
            { }
            catch (Exception ex)
            {
                trMessage.Visible = true;
                lblMessage.Text = "发生未知错误，操作失败。";
                throw (new AspNetException("编辑地面站页面btnReset_Click方法出现异常，异常原因", ex));
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
                throw (new AspNetException("编辑地面站页面btnReturn_Click方法出现异常，异常原因", ex));
            }
        }

        public override void OnPageLoaded()
        {
            this.PagePermission = "OMB_GSMan.Edit";
            this.ShortTitle = "编辑地面站";
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

            dplCoordinate.Items.Clear();
            dplCoordinate.DataSource = SystemParameters.GetSystemParameters(SystemParametersType.GroundResourceCoordinate);
            dplCoordinate.DataTextField = "key";
            dplCoordinate.DataValueField = "value";
            dplCoordinate.DataBind();
        }
        /// <summary>
        /// 绑定控件值
        /// </summary>
        private void BindControls()
        {
            XYXSInfo xyxsInfo = new XYXSInfo();
            xyxsInfo.Id = RID;
            xyxsInfo = xyxsInfo.SelectByID();
            if (xyxsInfo != null)
            {
                txtAddrName.Text = xyxsInfo.ADDRName;
                txtAddrMark.Text = xyxsInfo.ADDRMARK;
                txtInCode.Text = xyxsInfo.INCODE;
                txtExCode.Text = xyxsInfo.EXCODE;
                dplOwn.SelectedValue = xyxsInfo.Own;
                txtMainIP.Text = xyxsInfo.MainIP;
                txtTCPPort.Text = xyxsInfo.TCPPort.ToString();
                txtBakIP.Text = xyxsInfo.BakIP;
                txtUDPPort.Text = xyxsInfo.UDPPort.ToString();
                txtFTPPath.Text = xyxsInfo.FTPPath;
                txtFTPUser.Text = xyxsInfo.FTPUser;
                txtFTPPwd.Text = xyxsInfo.FTPPwd;
                lblCreatedTime.Text = xyxsInfo.CreatedTime.ToString("yyyy-MM-dd HH:mm:ss");
                lblUpdatedTime.Text = xyxsInfo.UpdatedTime == DateTime.MinValue ? xyxsInfo.CreatedTime.ToString("yyyy-MM-dd HH:mm:ss") : xyxsInfo.UpdatedTime.ToString("yyyy-MM-dd HH:mm:ss");
                txtDWCode.Text = xyxsInfo.DWCode;

                string[] coordinateInfo = xyxsInfo.Coordinate.Split(new char[] { ',', '，', ':', '：' }, StringSplitOptions.RemoveEmptyEntries);
                if (coordinateInfo != null && coordinateInfo.Length == 3)
                {
                    txtLongitude.Text = coordinateInfo[0];
                    txtLatitude.Text = coordinateInfo[1];
                    txtGaoCheng.Text = coordinateInfo[2];
                }

                //string[] ftpInfo = xyxsInfo.FTPPath.Split(new char[] { '@' });
                //if (ftpInfo != null && ftpInfo.Length == 3)
                //{
                //    txtFTPPath.Text = ftpInfo[0];
                //    txtFTPUser.Text = ftpInfo[1];
                //    txtFTPPwd.Text = ftpInfo[2];
                //}
            }
        }

        #endregion
    }
}
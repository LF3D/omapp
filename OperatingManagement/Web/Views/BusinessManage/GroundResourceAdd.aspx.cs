#region
//------------------------------------------------------
//Assembly:OperatingManagement.Web
//FileName:GroundResourceAdd.cs
//Remark:地面站资源添加类
//------------------------------------------------------
//VERSION       AUTHOR      DATE        CONTENT
//1.0           liutao      20111015    Create     
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
    public partial class GroundResourceAdd : AspNetPage
    {
        #region 属性
        /// <summary>
        /// 资源属性键值对列表
        /// </summary>
        protected Dictionary<int, string> ZYSXIDPValueDic
        {
            get
            {
                if (ViewState["ZYSXIDPValueDic"] == null)
                {
                    return new Dictionary<int, string>();
                }
                else
                {
                    return (ViewState["ZYSXIDPValueDic"] as Dictionary<int, string>);
                }
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
                    BindZYSXList();
                }
                BindRepeaterItems();
                cpZYSXPager.PostBackPage += new EventHandler(cpZYSXPager_PostBackPage);
            }
            catch (Exception ex)
            {
                trMessage.Visible = true;
                lblMessage.Text = "发生未知错误，操作失败。";
                throw (new AspNetException("新增地面站资源页面初始化出现异常，异常原因", ex));
            }
        }
        /// <summary>
        /// 提交添加地面站资源记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                string msg = string.Empty;
                if (string.IsNullOrEmpty(dplGroundStation.SelectedValue))
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "地面站不能为空";
                    return;
                }

                int rid = 0;
                //if (!int.TryParse(dplGroundStation.SelectedValue, out rid))
                //{
                //    trMessage.Visible = true;
                //    lblMessage.Text = "地面站序号格式错误";
                //    return;
                //}

                if (string.IsNullOrEmpty(txtEquipmentName.Text.Trim()))
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "设备名称不能为空";
                    return;
                }

                if (string.IsNullOrEmpty(txtEquipmentCode.Text.Trim()))
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "设备编号不能为空";
                    return;
                }

                if (string.IsNullOrEmpty(dplOpticalEquipment.SelectedValue.Trim()))
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "是否光学设备不能为空";
                    return;
                }

                int opticalEquipment = 0;
                if (!int.TryParse(dplOpticalEquipment.SelectedValue.Trim(), out opticalEquipment))
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "是否光学设备格式错误";
                    return;
                }

                if (cblFunctionType.SelectedItem == null)
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "请选择功能类型";
                    return;
                }
                string functionType = string.Empty;
                foreach (ListItem item in cblFunctionType.Items)
                {
                    if (item.Selected)
                    {
                        functionType += string.IsNullOrEmpty(functionType) ? item.Value : ";" + item.Value;
                    }
                }

                if (!LoopRepeaterItems())
                {
                    return;
                }

                Framework.FieldVerifyResult result;
                GroundResource groundResource = new GroundResource();
                groundResource.DMZCode = dplGroundStation.SelectedValue;
                groundResource.EquipmentName = txtEquipmentName.Text.Trim();
                groundResource.EquipmentCode = txtEquipmentCode.Text.Trim();
                groundResource.OpticalEquipment = opticalEquipment;
                groundResource.FunctionType = functionType;
                groundResource.Status = 1;//正常
                groundResource.XYXSID = Convert.ToInt32(dplXyxs.SelectedValue);
                groundResource.Coordinate = txtLongitude.Text.ToString() + "," + txtLatitude.Text.ToString() + "," + txtGaoCheng.Text.ToString();
                groundResource.CreatedTime = DateTime.Now;
                groundResource.CreatedUserID = LoginUserInfo.Id;

                //Dictionary<int,string>不支持序列化，采用其他方法
                //if (ZYSXIDPValueDic != null && ZYSXIDPValueDic.Count > 0)
                //{
                //    string extProperties = string.Empty;
                //    System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(Dictionary<int,string>));
                //    using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                //    {
                //        xmlSerializer.Serialize(ms, ZYSXIDPValueDic);
                //        extProperties = System.Text.Encoding.UTF8.GetString(ms.ToArray());
                //    }
                //    groundResource.ExtProperties = extProperties;
                //}

                if (ZYSXIDPValueDic != null && ZYSXIDPValueDic.Count > 0)
                {
                    string extProperties = string.Empty;
                    foreach (int key in ZYSXIDPValueDic.Keys)
                    {
                        if(!string.IsNullOrEmpty(extProperties))
                            extProperties += "|$|";
 
                        extProperties += key + "|#|" + ZYSXIDPValueDic[key];
                    }

                    groundResource.ExtProperties = extProperties;
                }

                if (groundResource.HaveActiveEquipmentCode())
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "设备编号已经存在";
                    return;
                }

                result = groundResource.Add();
                switch (result)
                {
                    case Framework.FieldVerifyResult.Error:
                        msg = "发生了数据错误，无法完成请求的操作。";
                        break;
                    case Framework.FieldVerifyResult.Success:
                        msg = "添加地面站资源成功。";
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
                throw (new AspNetException("新增地面站资源页面btnSubmit_Click方法出现异常，异常原因", ex));
            }
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
                throw (new AspNetException("新增地面站资源页面btnReset_Click方法出现异常，异常原因", ex));
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
                string url = @"~/Views/BusinessManage/DMZResourceMan.aspx";
                Response.Redirect(url);
            }
            catch (System.Threading.ThreadAbortException ex1)
            { }
            catch (Exception ex)
            {
                trMessage.Visible = true;
                lblMessage.Text = "发生未知错误，操作失败。";
                throw (new AspNetException("新增地面站资源页面btnReturn_Click方法出现异常，异常原因", ex));
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cpZYSXPager_PostBackPage(object sender, EventArgs e)
        {
            try
            {
                BindZYSXList();
            }
            catch (Exception ex)
            {
                throw (new AspNetException("新增地面站资源页面cpZYSXPager_PostBackPage方法出现异常，异常原因", ex));
            }
        }

        public override void OnPageLoaded()
        {
            this.PagePermission = "OMB_GRes.Add";
            this.ShortTitle = "新增地面站资源";
            this.SetTitle();
        }
        #region Method
        /// <summary>
        /// 绑定控件数据源
        /// </summary>
        private void BindDataSource()
        {
            dplGroundStation.Items.Clear();
            dplGroundStation.DataSource = new DMZ().SelectAll();
            dplGroundStation.DataTextField = "DMZName";
            dplGroundStation.DataValueField = "DMZCode";
            dplGroundStation.DataBind();

            dplOpticalEquipment.Items.Clear();
            dplOpticalEquipment.DataSource = SystemParameters.GetSystemParameters(SystemParametersType.GroundResourceOpticalEquipment);
            dplOpticalEquipment.DataTextField = "key";
            dplOpticalEquipment.DataValueField = "value";
            dplOpticalEquipment.DataBind();

            cblFunctionType.Items.Clear();
            cblFunctionType.DataSource = SystemParameters.GetSystemParameters(SystemParametersType.GroundResourceFunctionType);
            cblFunctionType.DataTextField = "key";
            cblFunctionType.DataValueField = "value";
            cblFunctionType.DataBind();

            dplXyxs.Items.Clear();
            dplXyxs.DataSource = new XYXSInfo().Cache.Where(t => t.Type == 0).ToList();
            dplXyxs.DataTextField = "AddrName";
            dplXyxs.DataValueField = "Id";
            dplXyxs.DataBind();
            dplXyxs.Items.Insert(0, new ListItem("请选择", "0"));
        }
        /// <summary>
        /// 绑定资源属性列表
        /// </summary>
        private void BindZYSXList()
        {
            List<ZYSX> zysxList = new ZYSX().GetGroundStationZYSXList();
            cpZYSXPager.Visible = false;
            cpZYSXPager.DataSource = zysxList;
            cpZYSXPager.PageSize = zysxList.Count + 1;//扩展属性不分页
            cpZYSXPager.BindToControl = rpZYSXList;
            rpZYSXList.DataSource = cpZYSXPager.DataSourcePaged;
            rpZYSXList.DataBind();
        }
        /// <summary>
        /// 添加完成后将控件设置为初始状态
        /// </summary>
        private void ResetControls()
        {
            dplGroundStation.SelectedIndex = 0;
            dplOpticalEquipment.SelectedIndex = 0;
            dplXyxs.SelectedIndex = 0;

            txtEquipmentName.Text = string.Empty;
            txtEquipmentCode.Text = string.Empty;
            txtLatitude.Text = string.Empty;
            txtLongitude.Text = string.Empty;
            txtGaoCheng.Text = string.Empty;

            foreach (ListItem item in cblFunctionType.Items)
            {
                item.Selected = false;
            }

            ViewState["ZYSXIDPValueDic"] = null;
            BindZYSXList();
            BindRepeaterItems();
        }
        /// <summary>
        /// 生成属性对应控件
        /// </summary>
        protected void BindRepeaterItems()
        {
            foreach (RepeaterItem item in rpZYSXList.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    //ZYSX zysx = (item.DataItem as ZYSX);
                    ZYSX zysx = null;
                    PlaceHolder phPValueControls = (item.FindControl("phPValueControls") as PlaceHolder);
                    HiddenField hfPID = (item.FindControl("hfPID") as HiddenField);
                    int id = 0;
                    if (hfPID != null && int.TryParse(hfPID.Value, out id))
                    {
                        zysx = new ZYSX();
                        zysx.Id = id;
                        zysx = zysx.SelectByID();
                    }
                    if (zysx != null && phPValueControls != null)
                    {
                        List<Control> controlsList = zysx.GenerateControls();
                        TextBox oTxt;
                        DropDownList ddlCtrl;
                        foreach (Control ctl in controlsList)
                        {
                            if (ctl.ClientID.Substring(0, 3) == "txt")
                            {
                                oTxt = (System.Web.UI.WebControls.TextBox)ctl;
                                oTxt.Text = "";
                                phPValueControls.Controls.Add(oTxt);
                            }
                            else
                            {
                                if (ctl.GetType() == typeof(DropDownList))
                                {
                                    ddlCtrl = (DropDownList)ctl;
                                    ddlCtrl.SelectedIndex = 0;
                                }
                                phPValueControls.Controls.Add(ctl);
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 获得并校验属性值
        /// </summary>
        private bool LoopRepeaterItems()
        {
            bool result = true;
            foreach (RepeaterItem item in rpZYSXList.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    //ZYSX zysx = (item.DataItem as ZYSX);
                    ZYSX zysx = null;
                    PlaceHolder phPValueControls = (item.FindControl("phPValueControls") as PlaceHolder);
                    HiddenField hfPID = (item.FindControl("hfPID") as HiddenField);
                    int id = 0;
                    if (hfPID != null && int.TryParse(hfPID.Value, out id))
                    {
                        zysx = new ZYSX();
                        zysx.Id = id;
                        zysx = zysx.SelectByID();
                    }
                    if (zysx != null && phPValueControls != null)
                    {
                        zysx.GetPValueFromControl(phPValueControls);
                        if (!zysx.ValidatePValue())
                        {
                            result = false;
                            trMessage.Visible = true;
                            lblMessage.Text = string.Format("属性名称为“{0}”的属性值填写错误，请修改。", zysx.PName);
                            break;
                        }
                        Dictionary<int, string> zysxIDPValueDic = ZYSXIDPValueDic;
                        if (zysxIDPValueDic.ContainsKey(zysx.Id))
                        {
                            zysxIDPValueDic[zysx.Id] = zysx.PValue;
                        }
                        else
                        {
                            zysxIDPValueDic.Add(zysx.Id, zysx.PValue);
                        }
                        ViewState["ZYSXIDPValueDic"] = zysxIDPValueDic;
                    }
                }
            }
            return result;
        }

        #endregion
    }
}
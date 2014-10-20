#region
//------------------------------------------------------
//Assembly:OperatingManagement.Web
//FileName:SatEdit.cs
//Remark:卫星编辑类
//------------------------------------------------------
//VERSION       AUTHOR      DATE        CONTENT
//1.1           liutao     20120915     Update     
//------------------------------------------------------
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using OperatingManagement.WebKernel.Basic;
using OperatingManagement.Framework.Core;
using OperatingManagement.DataAccessLayer.BusinessManage;

namespace OperatingManagement.Web.Views.BusinessManage.BDManage
{
    public partial class SatEdit : AspNetPage
    {
        #region 属性
        /// <summary>
        /// 卫星编码
        /// </summary>
        protected string WXBM
        {
            get
            {
                string wxbm = string.Empty;
                if (Request.QueryString["wxbm"] != null)
                {
                    wxbm = Request.QueryString["wxbm"];
                }
                return wxbm;
            }
        }
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
                    BindControls();
                    BindZYSXList();
                    BindZYGNList();
                }
                BindRepeaterItems();
                cpZYSXPager.PostBackPage += new EventHandler(cpZYSXPager_PostBackPage);
            }
            catch (Exception ex)
            {
                trMessage.Visible = true;
                lblMessage.Text = "发生未知错误，操作失败。";
                throw (new AspNetException("编辑卫星页面初始化出现异常，异常原因", ex));
            }
        }
        /// <summary>
        /// 提交更新卫星记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                string msg = string.Empty;
                #region Check Input Box
                if (string.IsNullOrEmpty(txtWXMC.Text.Trim()))
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "卫星名称不能为空";
                    return;
                }

                if (string.IsNullOrEmpty(txtWXBM.Text.Trim()))
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "卫星编码不能为空";
                    return;
                }

                if (string.IsNullOrEmpty(txtWXBS.Text.Trim()))
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "卫星标识不能为空";
                    return;
                }

                if (string.IsNullOrEmpty(txtMZB.Text.Trim()))
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "面质比不能为空";
                    return;
                }

                double mzb = 0;
                if (!double.TryParse(txtMZB.Text.Trim(), out mzb))
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "面质比格式错误";
                    return;
                }

                if (string.IsNullOrEmpty(txtBMFSXS.Text.Trim()))
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "表面反射系数不能为空";
                    return;
                }

                double bmfsxs = 0;
                if (!double.TryParse(txtBMFSXS.Text.Trim(), out bmfsxs))
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "表面反射系数格式错误";
                    return;
                }

                if (string.IsNullOrEmpty(txtD.Text.Trim()))
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "直径不能为空";
                    return;
                }

                double dblD = 0;
                if (!double.TryParse(txtD.Text.Trim(), out dblD))
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "直径格式错误";
                    return;
                }

                if (string.IsNullOrEmpty(txtBMFSXS.Text.Trim()))
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "长度不能为空";
                    return;
                }

                double dblL = 0;

                if (rblShape.SelectedValue == "0")
                    txtL.Text = "";
                else
                {
                    if (!double.TryParse(txtL.Text.Trim(), out dblL))
                    {
                        trMessage.Visible = true;
                        lblMessage.Text = "长度格式错误";
                        return;
                    }
                }
                #endregion

                if (!LoopRepeaterItems())
                {
                    return;
                }

                Framework.FieldVerifyResult result;
                Satellite satellite = new Satellite();
                satellite.Id = txtWXBM.Text.Trim();
                satellite = satellite.SelectByID();
                if (satellite == null)
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "修改的卫星不存在";
                    return;
                }
                satellite.WXMC = txtWXMC.Text.Trim();
                satellite.WXBM = txtWXBM.Text.Trim();
                satellite.WXBS = txtWXBS.Text.Trim();
                satellite.State = rblState.SelectedValue;
                satellite.SM = mzb;
                satellite.Ref = bmfsxs;
                satellite.Shape = int.Parse(rblShape.SelectedValue);
                satellite.D = dblD;
                satellite.L = dblL;
                satellite.RG = int.Parse(rblRG.SelectedValue);
                satellite.GN = hfWXGNs.Value;
                satellite.GNLBM = txtGNLBM.Text;
                //satellite.CreatedTime = DateTime.Now;

                if (ZYSXIDPValueDic != null && ZYSXIDPValueDic.Count > 0)
                {
                    string extProperties = string.Empty;
                    foreach (int key in ZYSXIDPValueDic.Keys)
                    {
                        if (!string.IsNullOrEmpty(extProperties))
                            extProperties += "|$|";

                        extProperties += key + "|#|" + ZYSXIDPValueDic[key];
                    }

                    satellite.SX = extProperties;
                }
                #region 注释了
                /*
                if (satellite.HaveActiveWXMC())
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "卫星名称已经存在";
                    return;
                }
                if (satellite.HaveActiveWXBM())
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "卫星编码已经存在";
                    return;
                }
                if (satellite.HaveActiveWXBS())
                {
                    trMessage.Visible = true;
                    lblMessage.Text = "卫星标识已经存在";
                    return;
                }*/
                #endregion

                result = satellite.Update();
                switch (result)
                {
                    case Framework.FieldVerifyResult.Error:
                        msg = "发生了数据错误，无法完成请求的操作。";
                        break;
                    case Framework.FieldVerifyResult.NameDuplicated:
                        msg = "卫星名称已经存在。";
                        break;
                    case Framework.FieldVerifyResult.NameDuplicated2:
                        msg = "卫星编码已经存在。";
                        break;
                    case Framework.FieldVerifyResult.NameDuplicated3:
                        msg = "卫星标识已经存在。";
                        break;
                    case Framework.FieldVerifyResult.Success:
                        msg = "编辑卫星成功。";

                        BindControls();
                        BindZYSXList();
                        BindRepeaterItems();
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
                throw (new AspNetException("编辑卫星页面btnSubmit_Click方法出现异常，异常原因", ex));
            }
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
                BindZYSXList();
                BindRepeaterItems();
            }
            catch (System.Threading.ThreadAbortException ex1)
            { }
            catch (Exception ex)
            {
                trMessage.Visible = true;
                lblMessage.Text = "发生未知错误，操作失败。";
                throw (new AspNetException("编辑卫星页面btnReset_Click方法出现异常，异常原因", ex));
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
                string url = @"~/Views/BusinessManage/BDManage/SatManage.aspx?millisecond=" + Server.UrlEncode(DateTime.Now.Millisecond.ToString());
                Response.Redirect(url);
            }
            catch (System.Threading.ThreadAbortException ex1)
            { }
            catch (Exception ex)
            {
                trMessage.Visible = true;
                lblMessage.Text = "发生未知错误，操作失败。";
                throw (new AspNetException("编辑卫星页面btnReturn_Click方法出现异常，异常原因", ex));
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
                throw (new AspNetException("编辑卫星页面cpZYSXPager_PostBackPage方法出现异常，异常原因", ex));
            }
        }
        public override void OnPageLoaded()
        {
            this.PagePermission = "OMB_SatkMan.Edit";
            this.ShortTitle = "编辑卫星";
            this.SetTitle();
            this.AddJavaScriptInclude("scripts/pages/businessManage/SatAdd.aspx.js");
        }
        #region Method
        /// <summary>
        /// 绑定控件数据源
        /// </summary>
        private void BindDataSource()
        {
            rblState.Items.Clear();
            rblState.DataSource = SystemParameters.GetSystemParameters(SystemParametersType.SatelliteState);
            rblState.DataTextField = "key";
            rblState.DataValueField = "value";
            rblState.DataBind();
            rblState.SelectedIndex = 0;

            rblShape.Items.Clear();
            rblShape.DataSource = SystemParameters.GetSystemParameters(SystemParametersType.SatelliteShape);
            rblShape.DataTextField = "key";
            rblShape.DataValueField = "value";
            rblShape.DataBind();
            rblShape.SelectedIndex = 0;

            rblRG.Items.Clear();
            rblRG.DataSource = SystemParameters.GetSystemParameters(SystemParametersType.SatelliteFace);
            rblRG.DataTextField = "key";
            rblRG.DataValueField = "value";
            rblRG.DataBind();
            rblRG.SelectedIndex = 0;
        }
        /// <summary>
        /// 绑定控件值
        /// </summary>
        private void BindControls()
        {
            Satellite satellite = new Satellite();
            satellite.Id = WXBM;
            satellite = satellite.SelectByID();
            if (satellite != null)
            {
                txtWXMC.Text = satellite.WXMC;
                txtWXBM.Text = satellite.WXBM;
                txtWXBS.Text = satellite.WXBS;
                rblState.SelectedIndex = rblState.Items.IndexOf(rblState.Items.FindByValue(satellite.State));
                txtMZB.Text = satellite.SM.ToString();
                txtBMFSXS.Text = satellite.Ref.ToString();
                txtD.Text = satellite.D.ToString();
                txtL.Text = satellite.L.ToString();
                rblShape.SelectedIndex = rblShape.Items.IndexOf(rblShape.Items.FindByValue(satellite.Shape.ToString()));
                rblRG.SelectedIndex = rblRG.Items.IndexOf(rblRG.Items.FindByValue(satellite.RG.ToString()));
                hfWXGNs.Value = satellite.GN;
                txtGNLBM.Text = satellite.GNLBM;
                lblCreatedTime.Text = satellite.CTime == DateTime.MinValue ? satellite.CTime.ToString("yyyy-MM-dd HH:mm:ss") : satellite.CTime.ToString("yyyy-MM-dd HH:mm:ss");

                if (!string.IsNullOrEmpty(satellite.SX))
                {
                    string[] keyValueArray = satellite.SX.Split(new string[] { "|$|" }, StringSplitOptions.RemoveEmptyEntries);
                    if (keyValueArray != null)
                    {
                        Dictionary<int, string> zysxIDPValueDic = new Dictionary<int, string>();
                        foreach (string keyValue in keyValueArray)
                        {
                            string[] array = keyValue.Split(new string[] { "|#|" }, StringSplitOptions.RemoveEmptyEntries);
                            int key = 0;
                            if (array != null && array.Length == 2 && int.TryParse(array[0], out key))
                            {
                                if (zysxIDPValueDic.ContainsKey(key))
                                {
                                    zysxIDPValueDic[key] = array[1];
                                }
                                else
                                {
                                    zysxIDPValueDic.Add(key, array[1]);
                                }
                            }
                        }
                        ViewState["ZYSXIDPValueDic"] = zysxIDPValueDic;
                    }
                }
            }
        }
        /// <summary>
        /// 绑定资源属性列表
        /// </summary>
        private void BindZYSXList()
        {
            List<ZYSX> zysxList = new ZYSX().GetSatelliteZYSXList();
            cpZYSXPager.Visible = false;
            cpZYSXPager.DataSource = zysxList;
            cpZYSXPager.PageSize = zysxList.Count + 1;//扩展属性不分页
            cpZYSXPager.BindToControl = rpZYSXList;
            rpZYSXList.DataSource = cpZYSXPager.DataSourcePaged;
            rpZYSXList.DataBind();
        }
        /// <summary>
        /// 绑定资源功能
        /// </summary>
        private void BindZYGNList()
        {
            List<ZYGN> lstZYGN = new List<ZYGN>();
            try
            {
                lstZYGN = new ZYGN().SelectAll();
            }
            catch (Exception ex)
            {
                throw (new AspNetException("新增卫星页面-绑定资源功能时出现异常，异常原因", ex));
            }
            rpWXGNs.DataSource = lstZYGN;
            rpWXGNs.DataBind();
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
                        if (ZYSXIDPValueDic.ContainsKey(zysx.Id))
                        {
                            zysx.PValue = ZYSXIDPValueDic[zysx.Id];
                        }
                        else
                            zysx.PValue = string.Empty;
                        List<Control> controlsList = zysx.GenerateControls();
                        foreach (Control ctl in controlsList)
                        {
                            phPValueControls.Controls.Add(ctl);
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

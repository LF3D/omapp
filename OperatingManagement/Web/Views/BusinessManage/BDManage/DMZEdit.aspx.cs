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
    public partial class DMZEdit : AspNetPage
    {
        public override void OnPageLoaded()
        {
            this.PagePermission = "OMB_GSMan.Edit";
            this.ShortTitle = "编辑地面站";
            this.SetTitle();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
            }
        }

        void BindData()
        {
            rblOwner.Items.Clear();
            rblOwner.DataSource = SystemParameters.GetSystemParameters(SystemParametersType.DMZOwner);
            rblOwner.DataTextField = "key";
            rblOwner.DataValueField = "value";
            rblOwner.DataBind();
            rblOwner.SelectedIndex = 0;

            int rid = Convert.ToInt32(Request.QueryString["rid"]);
            DataAccessLayer.BusinessManage.DMZ t = new DataAccessLayer.BusinessManage.DMZ() { Id = rid };
            var oDmz = new DataAccessLayer.BusinessManage.DMZ();
            try
            {
                oDmz = t.SelectByID();
            }
            catch (Exception ex)
            {
                throw (new AspNetException("编辑地面站页面读取数据出现异常，异常原因", ex));
            }
            finally { }

            txtName.Text = oDmz.DMZName;
            txtCode.Text = oDmz.DMZCode;
            txtDWCode.Text = oDmz.DWCode;
            rblOwner.SelectedIndex = rblOwner.Items.IndexOf(rblOwner.Items.FindByValue(oDmz.Owner.ToString()));
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
            DataAccessLayer.BusinessManage.DMZ t = new DataAccessLayer.BusinessManage.DMZ()
            {
                Id = Convert.ToInt32(Request.QueryString["rid"]),
                DMZName = txtName.Text.Trim(),
                DMZCode = txtCode.Text.Trim(),
                DWCode = txtDWCode.Text.Trim(),
                Owner = Convert.ToInt32(rblOwner.SelectedValue)
            };
            var result = Framework.FieldVerifyResult.Error;
            try
            {
                result = t.Update();
            }
            catch (Exception ex)
            {
                throw (new AspNetException("编辑地面站页面保存地面站数据出现异常，异常原因", ex));
            }
            finally { }

            string msg = string.Empty;
            switch (result)
            {
                case Framework.FieldVerifyResult.NameDuplicated:
                    msg = "已存在相同名称，请输入其他“地面站名称”。";
                    break;
                case Framework.FieldVerifyResult.Error:
                    msg = "发生了数据错误，无法完成请求的操作。";
                    break;
                case Framework.FieldVerifyResult.Success:
                    msg = "地面站已编辑成功。";
                    break;
                case Framework.FieldVerifyResult.NameDuplicated2:
                    msg = "已存在相同编码，请输入其他“地面站编码”。";
                    break;
                case Framework.FieldVerifyResult.NameDuplicated3:
                    msg = "已存在相同单位编码，请输入其他“单位编码”。";
                    break;
            }
            trMessage.Visible = true;
            lblMessage.Text = msg;
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect(Request.CurrentExecutionFilePath + "?rid=" + Request.QueryString["rid"]);
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect("dmzmanage.aspx");
        }
    }
}
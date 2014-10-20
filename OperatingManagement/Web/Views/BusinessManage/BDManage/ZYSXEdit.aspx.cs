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
    public partial class ZYSXEdit : AspNetPage
    {
        public override void OnPageLoaded()
        {
            this.PagePermission = "OMB_ZYSXMan.Edit";
            this.ShortTitle = "修改资源属性";
            this.SetTitle();
            this.AddJavaScriptInclude("scripts/pages/BusinessManage/ZYSXEdit.aspx.js");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindZYSX();
            }
        }

        void BindZYSX()
        {
            int zysxId = Convert.ToInt32(Request.QueryString["Id"]);
            hfID.Value = zysxId.ToString();
            DataAccessLayer.BusinessManage.ZYSX t = new DataAccessLayer.BusinessManage.ZYSX() { Id = zysxId };
            var zysx = new DataAccessLayer.BusinessManage.ZYSX();
            try
            {
                zysx = t.SelectByID();
            }
            catch (Exception ex)
            {
                throw (new AspNetException("修改资源属性页面读取资源属性数据出现异常，异常原因", ex));
            }
            finally { }

            txtZYSXName.Text = zysx.PName;
            txtZYSXCode.Text = zysx.PCode;
            rblType.SelectedValue = zysx.Type.ToString();
            rblOwn.SelectedValue = zysx.Own.ToString();
            txtScope.Text = zysx.Scope;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {

            System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
            DataAccessLayer.BusinessManage.ZYSX t = new DataAccessLayer.BusinessManage.ZYSX()
            {
                Id = Convert.ToInt32(hfID.Value),
                PName = txtZYSXName.Text.Trim(),
                PCode = txtZYSXCode.Text.Trim(),
                Type = Convert.ToInt32(rblType.SelectedValue),
                Scope = txtScope.Text.Trim(),
                Own = Convert.ToInt32(rblOwn.SelectedValue)
            };
            var result = Framework.FieldVerifyResult.Error;
            try
            {
                result = t.Update();
            }
            catch (Exception ex)
            {
                throw (new AspNetException("修改资源属性页面保存资源属性出现异常，异常原因", ex));
            }
            finally { }
            string msg = string.Empty;
            switch (result)
            {
                case Framework.FieldVerifyResult.NameDuplicated:
                    msg = "已存在相同名称，请输入其他“属性名称”。";
                    break;
                case Framework.FieldVerifyResult.Error:
                    msg = "发生了数据错误，无法完成请求的操作。";
                    break;
                case Framework.FieldVerifyResult.Success:
                    msg = "修改资源属性已成功。";
                    break;
                case Framework.FieldVerifyResult.NameDuplicated2:
                    msg = "已存在相同名称，请输入其他“资源属性编码”。";
                    break;
            }
            ltMessage.Text = msg;
        }

        protected void btnEmpty_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect(Request.CurrentExecutionFilePath + "?id=" + Request.QueryString["Id"]);
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect("ZYSXManage.aspx");
        }
    }
}
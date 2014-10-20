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
    public partial class ZYSXAdd : AspNetPage
    {
        public override void OnPageLoaded()
        {
            this.PagePermission = "OMB_ZYSXMan.Add";
            this.ShortTitle = "新增资源属性";
            this.SetTitle();
            this.AddJavaScriptInclude("scripts/pages/BusinessManage/ZYSXAdd.aspx.js");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {

            System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
            DataAccessLayer.BusinessManage.ZYSX t = new DataAccessLayer.BusinessManage.ZYSX()
            {
                PName = txtZYSXName.Text.Trim(),
                PCode = txtZYSXCode.Text.Trim(),
                Type = Convert.ToInt32(rblType.SelectedValue),
                Scope = txtScope.Text.Trim(),
                Own = Convert.ToInt32( rblOwn.SelectedValue)

            };
            var result = Framework.FieldVerifyResult.Error;
            try
            {
                result = t.Add();
            }
            catch (Exception ex)
            {
                throw (new AspNetException("新增资源属性页面保存任务出现异常，异常原因", ex));
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
                    msg = "新增资源属性已成功。";
                    break;
                case Framework.FieldVerifyResult.NameDuplicated2:
                    msg = "已存在相同属性编码，请输入其他“资源属性编码”。";
                    break;
                //case Framework.FieldVerifyResult.NameDuplicated3:
                //    msg = "已存在相同名称，请输入其他“任务标识”。";
                //    break;
            }
            ltMessage.Text = msg;
        }

        protected void btnEmpty_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect(Request.CurrentExecutionFilePath);
        }

    }
}
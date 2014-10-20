using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OperatingManagement.WebKernel.Basic;
using OperatingManagement.Framework.Storage;
using OperatingManagement.Framework.Core;
using System.IO;

namespace OperatingManagement.Web.Views.PlanManage
{
    public partial class OrbitParaCalculate : AspNetPage
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
            string filePath = GlobalSettings.MapPath("~/OrbitCalculate");
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);
            string paraNewPath = null;
            if (rblOrbitParameters.SelectedItem.Text.IndexOf("发射坐标系") >= 0 ||
                rblOrbitParameters.SelectedItem.Text.IndexOf("发射惯性坐标系") >= 0)
            {
                paraNewPath = Path.Combine(filePath, Guid.NewGuid() + ".txt");
                if (!fuParaFile.HasFile)
                {
                    ltMessage.Text = "“发射系相关文件”不能为空。";
                    return;
                }
                if (!Path.GetExtension(fuParaFile.FileName).Trim('.').Equals("txt",
                    StringComparison.InvariantCultureIgnoreCase))
                {
                    ltMessage.Text = "“发射系相关文件”格式只能为TXT。";
                    return;
                }
                fuParaFile.SaveAs(paraNewPath);
            }
            if (!fuCalFile.HasFile)
            {
                ltMessage.Text = "“待转换相关文件”不能为空。";
                DeleteFile(paraNewPath);
                return;
            }
            if (!Path.GetExtension(fuCalFile.FileName).Trim('.').Equals("txt",
                StringComparison.InvariantCultureIgnoreCase))
            {
                ltMessage.Text = "“待转换相关文件”格式只能为TXT。";
                DeleteFile(paraNewPath);
                return;
            }
            string calNewPath = Path.Combine(filePath, Guid.NewGuid() + ".txt");
            fuCalFile.SaveAs(calNewPath);

            ltAngle.Text = rblAngle.SelectedValue;
            ltDist.Text = rblDistance.SelectedValue;
            ltOrbit.Text = rblOrbitParameters.SelectedValue;
            if (!string.IsNullOrEmpty(paraNewPath)) {
                ltPara.Text = File.ReadAllText(paraNewPath);
            }
            ltCal.Text = File.ReadAllText(calNewPath);
            /**
             * TODO: 在这里开始计算。
             * */
            ClientScript.RegisterClientScriptBlock(this.GetType(),
                "open-dialog",
                "var _autoOpen=true;",
                true);
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
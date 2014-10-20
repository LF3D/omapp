using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OperatingManagement.WebKernel.Basic;
using OperatingManagement.Framework.Core;

namespace OperatingManagement.Web.Views.BusinessManage.BDManage
{
    public partial class TaskEdit : AspNetPage
    {
        public override void OnPageLoaded()
        {
            this.PagePermission = "OMB_TaskMan.Edit";
            this.ShortTitle = "编辑任务";
            this.SetTitle();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ucCBLSats.RepeatColumns = 2;
                BindTask();
            }
        }

        void BindTask()
        {
            int taskId = Convert.ToInt32(Request.QueryString["Id"]);
            DataAccessLayer.BusinessManage.Task t = new DataAccessLayer.BusinessManage.Task() { Id = taskId };
            var task = new DataAccessLayer.BusinessManage.Task();
            try
            {
                task = t.SelectById();
            }
            catch (Exception ex)
            {
                throw (new AspNetException("修改任务页面读取任务数据出现异常，异常原因", ex));
            }
            finally { }

            txtTaskName.Text = task.TaskName;
            txtTaskNo.Text = task.TaskNo;
            txtOutTaskNo.Text = task.OutTaskNo;
            txtObjectFlag.Text = task.ObjectFlag;
            ucCBLSats.SelectedValues = task.SatID;
            txtSCID.Text = task.SCID;
            rblIsEffective.SelectedValue = task.IsEffective;
            txtEmitTime.Text = task.EmitTime.ToString("yyyyMMddHHmmss");
            txtMiniSeconds.Text = task.EmitTime.Millisecond.ToString();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {

            System.Globalization.CultureInfo provider = System.Globalization.CultureInfo.InvariantCulture;
            if (ucCBLSats.SelectedValues.Equals(string.Empty))
            {
                ltMessage.Text = "卫星为必选项，请选择。";
                return;
            }
            DataAccessLayer.BusinessManage.Task t = new DataAccessLayer.BusinessManage.Task()
            {
                Id = Convert.ToInt32(Request.QueryString["Id"]),
                TaskName = txtTaskName.Text.Trim(),
                TaskNo = txtTaskNo.Text.Trim(),
                OutTaskNo = txtOutTaskNo.Text.Trim(),
                ObjectFlag = txtObjectFlag.Text.Trim(),
                SatID = ucCBLSats.SelectedValues.ToString(),
                SCID = txtSCID.Text.Trim(),
                IsEffective = rblIsEffective.SelectedValue.ToString(),
                EmitTime = DateTime.ParseExact(txtEmitTime.Text.Trim(), "yyyyMMddHHmmss", provider)
            };
            t.EmitTime = t.EmitTime.AddMilliseconds(int.Parse(txtMiniSeconds.Text));
            var result = Framework.FieldVerifyResult.Error;
            try
            {
                result = t.Update();
            }
            catch (Exception ex)
            {
                throw (new AspNetException("修改任务页面保存任务数据出现异常，异常原因", ex));
            }
            finally { }

            string msg = string.Empty;
            switch (result)
            {
                case Framework.FieldVerifyResult.NameDuplicated:
                    msg = "已存在相同名称，请输入其他“任务名称”。";
                    break;
                case Framework.FieldVerifyResult.Error:
                    msg = "发生了数据错误，无法完成请求的操作。";
                    break;
                case Framework.FieldVerifyResult.Success:
                    msg = "任务已编辑成功。";
                    break;
                case Framework.FieldVerifyResult.NameDuplicated2:
                    msg = "已存在相同名称，请输入其他“任务代号”。";
                    break;
                case Framework.FieldVerifyResult.NameDuplicated3:
                    msg = "已存在相同名称，请输入其他“任务标识”。";
                    break;
            }
            ltMessage.Text = msg;
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect("taskmanage.aspx");
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect(Request.CurrentExecutionFilePath + "?id=" + Request.QueryString["Id"]);
        }
    }
}
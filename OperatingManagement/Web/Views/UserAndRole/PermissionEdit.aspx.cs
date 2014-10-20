using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OperatingManagement.DataAccessLayer;
using OperatingManagement.WebKernel.Basic;
using OperatingManagement.Framework.Core;

namespace OperatingManagement.Web.Views.UserAndRole
{
    public partial class WebForm1 : AspNetPage
    {
        #region 属性
        /// <summary>
        /// ModuleID
        /// </summary>
        protected int ModuleID
        {
            get
            {
                int mID = 0;
                if (Request.QueryString["id"] != null)
                {
                    int.TryParse(Request.QueryString["id"], out mID);
                }
                return mID;
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                InitialPageData();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            SaveData();
        }

        private void SaveData()
        {
            DataAccessLayer.System.Module oModule = new DataAccessLayer.System.Module();
            if (ModuleID == 0)
                return;
            oModule.Id = ModuleID;
            oModule.ModuleName = txtName.Text;
            oModule.ModuleNote = txtNote.Text;
            string strActionIds = "";
            foreach (ListItem item in cblActions.Items)
            {
                if (item.Selected)
                    strActionIds += "," + item.Value;
            }

            if (strActionIds.Equals(string.Empty))
            {
                ShowMessage("没有为权限指定任何操作。");
                return;
            }

            oModule.ActionIds = strActionIds.Substring(1);
            try
            {
                Framework.FieldVerifyResult result = oModule.Update();
                string msg = "";
                switch (result)
                {
                    case Framework.FieldVerifyResult.Error:
                        msg = "发生了数据错误，无法完成请求的操作。";
                        break;
                    case Framework.FieldVerifyResult.Success:
                        msg = "编辑权限已成功。";
                        break;
                    case Framework.FieldVerifyResult.NameDuplicated:
                    case Framework.FieldVerifyResult.NameDuplicated2:
                        msg = "已存在相同名称，请输入其他“名称”。";
                        break;
                }
                ShowMessage(msg);
            }
            catch (Exception ex)
            {
                throw (new AspNetException("修改权限页面保存权限数据出现异常，异常原因", ex));
            }
            finally { }
        }

        /// <summary>
        /// 初始化页面，按当前ID初始化
        /// </summary>
        private void InitialPageData()
        {
            HideMessage();
            try
            {
                BindDataSource();
            }
            catch (Exception ex)
            {
                throw (new AspNetException("修改权限页面初始化出现异常，异常原因", ex));
            }
            finally
            {
            }

            DataAccessLayer.System.Module oModule = new DataAccessLayer.System.Module();
            if (ModuleID == 0)
                return;
            if (oModule != null)
            {
                oModule.Id = ModuleID;
                try
                {
                    oModule = oModule.SelectById();
                    txtName.Text = oModule.ModuleName;
                    txtNote.Text = oModule.ModuleNote;
                    string[] slModuleIds = oModule.ActionIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (ListItem item in cblActions.Items)
                    {
                        item.Selected = slModuleIds.Contains(item.Value);
                    }
                }
                catch (Exception ex)
                {
                    throw (new AspNetException("修改权限页面读取权限数据出现异常，异常原因", ex));
                }
                finally { }
            }
        }

        /// <summary>
        /// 绑定操作列表
        /// </summary>
        private void BindDataSource()
        {
            cblActions.Items.Clear();
            cblActions.DataSource = new DataAccessLayer.System.Task().SelectAll();
            cblActions.DataTextField = "TaskNote";
            cblActions.DataValueField = "Id";
            cblActions.DataBind();
        }

        /// <summary>
        /// 隐藏提示信息
        /// </summary>
        private void HideMessage()
        {
            trMessage.Visible = false;
            ltMessage.Text = "";
        }

        /// <summary>
        /// 显示提示信息
        /// </summary>
        /// <param name="message"></param>
        private void ShowMessage(string message)
        {
            trMessage.Visible = true;
            ltMessage.Text = message;
        }

        public override void OnPageLoaded()
        {
            this.PagePermission = "OMPermissionManage.Edit";
            this.ShortTitle = "编辑权限";
            this.SetTitle();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect("Permissions.aspx");
        }
    }
}
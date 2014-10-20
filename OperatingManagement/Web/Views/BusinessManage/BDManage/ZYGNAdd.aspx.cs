using System;
using System.Text;
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
    public partial class ZYGNAdd : AspNetPage
    {
        public override void OnPageLoaded()
        {
            this.PagePermission = "OMB_ZYGNMan.Add";
            this.ShortTitle = "新增资源功能";
            this.SetTitle();
            this.AddJavaScriptInclude("scripts/pages/BusinessManage/ZYGNAdd.aspx.js");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitData();
            }
        }

        protected void InitData()
        {
            List<MatchRule> lstRules = new List<MatchRule>();
            MatchRule oRule = new MatchRule();
            lstRules.Add(oRule);
            rpPPZZ.DataSource = lstRules;
            rpPPZZ.DataBind();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            StringBuilder sbRules = new StringBuilder();
            string strPCode = string.Empty;
            string strLogic = string.Empty;
            DropDownList ddlList;
            List<string> lstSXs = new List<string>();
            foreach (RepeaterItem it in rpPPZZ.Items)
            {
                ddlList = (DropDownList)it.FindControl("ddlZYSX");
                if (ddlList != null)
                    strPCode = ddlList.SelectedValue;
                if (!lstSXs.Contains(strPCode))
                    lstSXs.Add(strPCode);
                else
                {
                    ltMessage.Text = "资源属性选择有重复。";
                    return;
                }
                ddlList = (DropDownList)it.FindControl("ddlLogic");
                if (ddlList != null)
                    strLogic = ddlList.SelectedItem.Text;
                sbRules.Append("[" + strPCode + "]" + strLogic + "[" + strPCode + "],");
            }

            DataAccessLayer.BusinessManage.ZYGN t = new DataAccessLayer.BusinessManage.ZYGN()
            {
                FName = txtName.Text.Trim(),
                FCode = txtCode.Text.Trim(),
                MatchRule = sbRules.ToString().TrimEnd(new char[]{','})
            };
            var result = Framework.FieldVerifyResult.Error;
            try
            {
                result = t.Add();
            }
            catch (Exception ex)
            {
                throw (new AspNetException("新增资源功能页面保存任务出现异常，异常原因", ex));
            }
            finally { }
            string msg = string.Empty;
            switch (result)
            {
                case Framework.FieldVerifyResult.NameDuplicated:
                    msg = "已存在相同名称，请输入其他“名称”。";
                    break;
                case Framework.FieldVerifyResult.Error:
                    msg = "发生了数据错误，无法完成请求的操作。";
                    break;
                case Framework.FieldVerifyResult.Success:
                    msg = "新增资源功能已成功。";
                    break;
                case Framework.FieldVerifyResult.NameDuplicated2:
                    msg = "已存在相同编码，请输入其他“资源功能编码”。";
                    break;
            }
            ltMessage.Text = msg;
        }

        protected void btnEmpty_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect(Request.CurrentExecutionFilePath);
        }

        protected void rpPPZZ_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            List<MatchRule> lstRules = new List<MatchRule>();
            MatchRule oRule;
            Repeater rp = (Repeater)source;
            DropDownList ddlList;

            if (e.CommandName == "Add")
            {
                foreach (RepeaterItem it in rp.Items)
                {
                    oRule = new MatchRule();
                    ddlList = (DropDownList)it.FindControl("ddlZYSX");
                    if (ddlList != null)
                        oRule.PCode = ddlList.SelectedValue;
                    ddlList = (DropDownList)it.FindControl("ddlLogic");
                    if (ddlList != null)
                        oRule.LogicSymbol = (emLogicSymbol)(Enum.Parse(typeof(emLogicSymbol), ddlList.SelectedValue));
                    lstRules.Add(oRule);
                }
                oRule = new MatchRule();
                lstRules.Add(oRule);
                rp.DataSource = lstRules;
                rp.DataBind();
            }
            if (e.CommandName == "Del")
            {
                if (rp.Items.Count == 1)
                {
                    oRule = new MatchRule();
                    lstRules.Add(oRule);
                }
                else
                {
                    foreach (RepeaterItem it in rp.Items)
                    {
                        if (e.Item.ItemIndex != it.ItemIndex)
                        {
                            oRule = new MatchRule();
                            ddlList = (DropDownList)it.FindControl("ddlZYSX");
                            if (ddlList != null)
                                oRule.PCode = ddlList.SelectedValue;
                            ddlList = (DropDownList)it.FindControl("ddlLogic");
                            if (ddlList != null)
                                oRule.LogicSymbol = (emLogicSymbol)(Enum.Parse(typeof(emLogicSymbol), ddlList.SelectedValue));
                            lstRules.Add(oRule);
                        }
                    }
                }
                rp.DataSource = lstRules;
                rp.DataBind();
            }

        }

        protected void rpPPZZ_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    DropDownList ddlSY = (DropDownList)e.Item.FindControl("ddlZYSX") as DropDownList;
                    ddlSY.DataSource = new ZYSX().Cache.Where(t => t.Own == 2).ToList();
                    ddlSY.DataTextField = "PName";
                    ddlSY.DataValueField = "PCode";
                    ddlSY.DataBind();
                }
            }
            catch (Exception ex)
            {
                throw (new AspNetException("绑定匹配准则信息出现异常，异常原因", ex));
            }
            finally { }
        }
    }
}
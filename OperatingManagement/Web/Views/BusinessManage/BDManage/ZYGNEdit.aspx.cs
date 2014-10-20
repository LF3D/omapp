using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OperatingManagement.WebKernel.Basic;
using OperatingManagement.Framework.Core;
using OperatingManagement.DataAccessLayer.BusinessManage;


namespace OperatingManagement.Web.Views.BusinessManage.BDManage
{
    public partial class ZYGNEdit : AspNetPage
    {
        public override void OnPageLoaded()
        {
            this.PagePermission = "OMB_ZYGNMan.Edit";
            this.ShortTitle = "修改资源功能";
            this.SetTitle();
            this.AddJavaScriptInclude("scripts/pages/BusinessManage/ZYGNAdd.aspx.js");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindZYGN();
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

        void BindZYGN()
        {
            int zyId = Convert.ToInt32(Request.QueryString["Id"]);
            hfID.Value = zyId.ToString();
            DataAccessLayer.BusinessManage.ZYGN t = new DataAccessLayer.BusinessManage.ZYGN() { Id = zyId };
            var zy = new DataAccessLayer.BusinessManage.ZYGN();
            try
            {
                zy = t.SelectByID();
            }
            catch (Exception ex)
            {
                throw (new AspNetException("修改资源属性页面读取资源属性数据出现异常，异常原因", ex));
            }
            finally { }

            txtName.Text = zy.FName;
            txtCode.Text = zy.FCode;
            string[] strRules = new string[0];
            if (!zy.MatchRule.Equals(string.Empty))
            {
                strRules = zy.MatchRule.Split(new char[]{','});
            }
            List<MatchRule> lstRules = new List<MatchRule>();
            MatchRule oRule;
            string strTmp = string.Empty;
            for (int i = 0; i < strRules.Length; i++)
            {
                oRule = new MatchRule();
                strTmp = strRules[i].TrimStart(new char[]{'['}).TrimEnd(new char[]{']'});
                oRule.PCode = strTmp.Substring(0, strTmp.IndexOf(']'));
                strTmp = strTmp.Replace(oRule.PCode + "]", "");
                oRule.LogicSymbol = (emLogicSymbol)Enum.Parse(typeof(emLogicSymbol), GetLogicName(strTmp.Substring(0, strTmp.IndexOf('['))));
                lstRules.Add(oRule);
            }
            rpPPZZ.DataSource = lstRules;
            rpPPZZ.DataBind();
        }

        private string GetLogicName(string logic)
        {
            switch (logic)
            {
                case ">":
                    return "MoreThan";
                case ">=":
                    return "MoreThanEqual";
                case "<":
                    return "LessThan";
                case "<=":
                    return "LessThanEqual";
                case "=":
                    return "Equal";
                default :
                    return string.Empty;

            }
        }

        /// <summary>
        /// 返回匹配准则中的各部分值
        /// </summary>
        /// <param name="rule"></param>
        /// <param name="index">1:satallite; 2: 卫星属性; 3:符号； 4:DMZ；5：DMZ属性</param>
        /// <returns></returns>
        private string SpliteMatchRule(string rule,int index)
        {
            string result = "";
            int pos = -1;
            if (rule.IndexOf("<") > 1)
            {
                pos = rule.IndexOf("<");
                result = "<";
            }
            if (rule.IndexOf("<=") > 1)
            {
                pos = rule.IndexOf("<=");
                rule = rule.Replace("<=", "#"); //符号为两位，换成一位的符号，便于计算长度
                result = "<=";
            }
            if (rule.IndexOf("=") > 1)
            {
                pos = rule.IndexOf("=");
                result = "=";
            }
            if (rule.IndexOf(">=") > 1)
            {
                pos = rule.IndexOf(">=");
                rule = rule.Replace(">=", "#"); //符号为两位，换成一位的符号，便于计算长度
                result = ">=";
            }
            if (rule.IndexOf(">") > 1)
            {
                pos = rule.IndexOf(">");
                result = ">";
            }
            switch (index)
            { 
                case 1: //Satellite
                    result = rule.Substring(0, rule.IndexOf("."));
                    break;
                case 2: //Satellite 属性
                    result = rule.Substring(rule.IndexOf(".") +1, pos - rule.IndexOf(".")-1 );
                    break;
                case 3: //符号
                    break;
                case 4: //DMZ
                    result = rule.Substring(pos, rule.LastIndexOf(".") - pos);
                    break;
                case 5: //DMZ 属性
                    result = rule.Substring(rule.LastIndexOf(".") + 1);
                    break;
            }

            return result;
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
                Id = Convert.ToInt32( hfID.Value),
                FName = txtName.Text.Trim(),
                FCode = txtCode.Text.Trim(),
                MatchRule = sbRules.ToString().TrimEnd(new char[] { ',' })

            };
            var result = Framework.FieldVerifyResult.Error;
            try
            {
                result = t.Update();
            }
            catch (Exception ex)
            {
                throw (new AspNetException("修改资源功能页面保存资源功能出现异常，异常原因", ex));
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
                    msg = "修改资源功能已成功。";
                    break;
                case Framework.FieldVerifyResult.NameDuplicated2:
                    msg = "已存在相同编码，请输入其他“资源功能编码”。";
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
            Page.Response.Redirect("ZYGNManage.aspx");
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
                    DropDownList ddlSX = (DropDownList)e.Item.FindControl("ddlZYSX") as DropDownList;
                    ddlSX.DataSource = new ZYSX().Cache.Where(t => t.Own == 2).ToList();
                    ddlSX.DataTextField = "PName";
                    ddlSX.DataValueField = "PCode";
                    ddlSX.DataBind();

                    DropDownList ddlLogic = (DropDownList)e.Item.FindControl("ddlLogic") as DropDownList;
                    Repeater rp = (Repeater)sender;
                    List<MatchRule> lstRules = (List<MatchRule>)rp.DataSource;
                    ddlLogic.SelectedIndex = ddlLogic.Items.IndexOf(
                        ddlLogic.Items.FindByValue(Enum.GetName(typeof(emLogicSymbol), (lstRules[e.Item.ItemIndex].LogicSymbol))));
                    ddlSX.SelectedIndex = ddlSX.Items.IndexOf(ddlSX.Items.FindByValue(lstRules[e.Item.ItemIndex].PCode));
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
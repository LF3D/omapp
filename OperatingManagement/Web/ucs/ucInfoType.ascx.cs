using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OperatingManagement.DataAccessLayer.BusinessManage;

namespace OperatingManagement.Web.ucs
{
    public partial class ucInfoType : System.Web.UI.UserControl
    {
        private bool isAllowBlankItem = true;
        private string blankItemText = "请选择";
        private string blankItemValue = "0";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindDataSource();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        private void BindDataSource()
        {
            InfoType objInfos = new InfoType();
            InfoTypeList.Items.Clear();
            InfoTypeList.DataSource = objInfos.Cache.Where(a => a.DATATYPE == "1");//只显示文件类型数据
            InfoTypeList.DataTextField = "DATANAME";
            InfoTypeList.DataValueField = "id";
            InfoTypeList.DataBind();
            if (isAllowBlankItem)
                InfoTypeList.Items.Insert(0, new ListItem(blankItemText, blankItemValue));
        }

        public ListItem SelectedItem
        {
            get { return InfoTypeList.SelectedItem; }
        }

        public string SelectedValue
        {
            get { return InfoTypeList.SelectedValue; }
            set { InfoTypeList.SelectedValue = value; }
        }

        public bool AllowBlankItem
        {
            set { isAllowBlankItem = value; }
        }

        public string BlankItemText
        {
            get { return blankItemText; }
            set { blankItemText = value; }
        }

        public string BlankItemValue
        {
            get { return blankItemValue; }
            set { blankItemValue = value; }
        }

        public int SelectedIndex
        {
            set { InfoTypeList.SelectedIndex = value; }
            get { return InfoTypeList.SelectedIndex; }
        }
    }
}
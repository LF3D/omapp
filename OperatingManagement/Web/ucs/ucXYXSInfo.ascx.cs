using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OperatingManagement.DataAccessLayer.BusinessManage;

namespace OperatingManagement.Web.ucs
{
    public partial class ucXYXSInfo : System.Web.UI.UserControl
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
            XYXSInfo objInfos = new XYXSInfo();
            XYXSList.Items.Clear();
            XYXSList.DataSource = objInfos.Cache;
            XYXSList.DataTextField = "ADDRNAME";
            XYXSList.DataValueField = "id";
            XYXSList.DataBind();
            if (isAllowBlankItem)
                XYXSList.Items.Insert(0, new ListItem(blankItemText, blankItemValue));
        }

        public ListItem SelectedItem
        {
            get { return XYXSList.SelectedItem; }
        }

        public ListItemCollection Items
        {
            get { return XYXSList.Items; }
        }

        public string SelectedValue
        {
            get { return XYXSList.SelectedValue; }
            set { XYXSList.SelectedValue = value; }
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
            get { return XYXSList.SelectedIndex; }
            set { XYXSList.SelectedIndex = value; }
        }
    }
}
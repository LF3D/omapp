using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OperatingManagement.DataAccessLayer.BusinessManage;

namespace OperatingManagement.Web.ucs
{
    public partial class ucSatellite : System.Web.UI.UserControl
    {
        private bool isAllowBlankItem = true;
        private string blankItemText = "请选择";
        private string blankItemValue = "0";

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!Page.IsPostBack || SatelliteList.DataSource == null)
            {
                BindDataSource();
            }
        }

        private void BindDataSource()
        {
            Satellite objSatellite = new Satellite();
            SatelliteList.Items.Clear();
            SatelliteList.DataSource = objSatellite.Cache;
            SatelliteList.DataTextField = "WXMC";
            SatelliteList.DataValueField = "WXBM";
            SatelliteList.DataBind();
            if (isAllowBlankItem)
                SatelliteList.Items.Insert(0, new ListItem(blankItemText, blankItemValue));
        }

        public void ReBindData()
        {
            BindDataSource();
        }

        public ListItemCollection Items
        {
            get { return SatelliteList.Items; }
        }

        public ListItem SelectedItem
        {
            get { return SatelliteList.SelectedItem; }
        }

        public string SelectedValue
        {
            get { return SatelliteList.SelectedValue; }
            set { SatelliteList.SelectedValue = value; }
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
            get { return SatelliteList.SelectedIndex; }
            set { SatelliteList.SelectedIndex = value; }
        }

        public bool Enabled
        {
            set { SatelliteList.Enabled = value; }
            get { return SatelliteList.Enabled; }
        }
    }
}
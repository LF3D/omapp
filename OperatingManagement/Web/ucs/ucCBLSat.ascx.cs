using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OperatingManagement.DataAccessLayer.BusinessManage;

namespace OperatingManagement.Web.ucs
{
    public partial class ucCBLSat : System.Web.UI.UserControl
    {
        private string strSelectedValues = string.Empty;

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
            Satellite objSatellite = new Satellite();
            cblSatellite.Items.Clear();
            cblSatellite.DataSource = objSatellite.Cache;
            cblSatellite.DataTextField = "WXMC";
            cblSatellite.DataValueField = "WXBM";
            cblSatellite.DataBind();
            if (!strSelectedValues.Equals(string.Empty))
            {
                foreach (ListItem item in cblSatellite.Items)
                {
                    if (strSelectedValues.IndexOf(item.Value) >= 0)
                        item.Selected = true;
                }
            }
        }

        public void ReBindData()
        {
            BindDataSource();
        }

        public ListItemCollection Items
        {
            get { return cblSatellite.Items; }
        }

        public string SelectedValues
        {
            get 
            {
                StringBuilder selectedValues = new StringBuilder();
                foreach (ListItem item in cblSatellite.Items)
                {
                    if (item.Selected)
                        selectedValues.Append(item.Value + ",");
                }
                return selectedValues.ToString().TrimEnd(new char[]{','}); 
            }
            set
            {
                strSelectedValues = value;
            }
        }

        public int RepeatColumns
        {
            get { return cblSatellite.RepeatColumns; }
            set { cblSatellite.RepeatColumns = value; }
        }

        public bool Enabled
        {
            set { cblSatellite.Enabled = value; }
            get { return cblSatellite.Enabled; }
        }
    }
}
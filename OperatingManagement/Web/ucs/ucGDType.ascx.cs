using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OperatingManagement.DataAccessLayer.BusinessManage;

namespace OperatingManagement.Web.ucs
{
    public partial class ucGDType : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
        }

        protected void Page_Init(object sender, EventArgs e)
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
            GDTypeList.Items.Clear();
            GDTypeList.DataSource = objInfos.Cache.Where(a => a.EXMARK.IndexOf("GD") >= 0);//只显示轨道类型数据
            GDTypeList.DataTextField = "DATANAME";
            GDTypeList.DataValueField = "INCODE";
            GDTypeList.DataBind();
        }

        public ListItem SelectedItem
        {
            get { return GDTypeList.SelectedItem; }
        }

        public string SelectedValue
        {
            get { return GDTypeList.SelectedValue; }
            set { GDTypeList.SelectedValue = value; }
        }

        public int SelectedIndex
        {
            set { GDTypeList.SelectedIndex = value; }
            get { return GDTypeList.SelectedIndex; }
        }
    }
}
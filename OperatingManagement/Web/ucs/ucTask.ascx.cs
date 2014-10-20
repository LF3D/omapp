using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OperatingManagement.DataAccessLayer.BusinessManage;

namespace OperatingManagement.Web.ucs
{
    public partial class ucTask : System.Web.UI.UserControl
    {
        private bool isAllowBlankItem = true;
        private string blankItemText = "请选择";
        private string blankItemValue = "0";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //TaskList.Items.Clear();
                //TaskList.DataSource = new Task().Cache;
                //TaskList.DataTextField = "TaskName";
                //TaskList.DataValueField = "TaskNo";
                //TaskList.DataBind();
                //if (isAllowBlankItem)
                //    TaskList.Items.Insert(0, new ListItem(blankItemText, blankItemValue));
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!IsPostBack)
            {
                TaskList.Items.Clear();
                TaskList.DataSource = new Task().Cache;
                TaskList.DataTextField = "TaskName";
                TaskList.DataValueField = "TaskNo";
                TaskList.DataBind();
                if (isAllowBlankItem)
                    TaskList.Items.Insert(0, new ListItem(blankItemText, blankItemValue));
            }
            
        }

        public ListItem SelectedItem
        {
            get { return TaskList.SelectedItem; }
        }

        public string SelectedValue
        {
            get { return TaskList.SelectedValue; }
            set { TaskList.SelectedValue = value; }
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
            set { TaskList.SelectedIndex = value; }
            get { return TaskList.SelectedIndex; }
        }

        public bool Enabled
        {
            set { TaskList.Enabled = value; }
            get { return TaskList.Enabled; }
        }
    }
}
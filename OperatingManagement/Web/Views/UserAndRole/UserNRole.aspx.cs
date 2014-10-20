using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OperatingManagement.WebKernel.Basic;

namespace OperatingManagement.Web.Views.UserAndRole
{
    public partial class UserNRole : AspNetPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public override void OnPageLoaded()
        {
            this.ShortTitle = "用户角色";
            this.SetTitle();
        }
    }
}
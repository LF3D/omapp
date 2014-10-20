using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OperatingManagement.WebKernel.Basic;

namespace OperatingManagement.Web.Views.Exp
{
    public partial class PNF404 : AspNetPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public override void OnPageLoaded()
        {
            this.ShortTitle = "页面不存在";
            base.OnPageLoaded();
        }
    }
}

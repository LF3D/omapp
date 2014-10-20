using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OperatingManagement.WebKernel.Basic;
using OperatingManagement.Framework.Core;

namespace OperatingManagement.Web.Views.Exp
{
    public partial class Exp : AspNetPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            Exception baseEx = ex.GetBaseException();
            AspNetException exp = null;
            if (ex is AspNetException)
            {
                exp = ex as AspNetException;
            }
            else
            {
                if (baseEx is AspNetException)
                    exp = baseEx as AspNetException;
                else
                    exp = new AspNetException(ex.ToString());

            }
            if (exp == null)
                ltError.Text = "发生了未知的错误，请联系管理员。";
            else
                ltError.Text = exp.Message;
        }
        public override void OnPageLoaded()
        {
            this.ShortTitle = "异常信息";
            base.OnPageLoaded();
        }
    }
}
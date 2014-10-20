using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web;
using System.Web.Security;
using OperatingManagement.Framework.Core;
using OperatingManagement.Framework.Components;
using OperatingManagement.Framework.Storage;
using OperatingManagement.Security;
using OperatingManagement.Framework.Reflector;

namespace OperatingManagement.WebKernel.Controls
{
    public class PageNavigator:Control
    {
        public PageNavigator() { }
        /// <summary>
        /// Gets/Sets the selected identification.
        /// </summary>
        public string SelectedId { get; set; }
        /// <summary>
        /// Gets/Sets the css name which this control is using.
        /// </summary>
        public string CssName { get; set; }
        private string RenderNav()
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                FormsAuthentication.RedirectToLoginPage();
                return null;
            }
            Navigator nav = AspNetSetting.Load<Navigator>();

            //AspNetPrincipal principal = (AspNetPrincipal)HttpContext.Current.User;

            StringBuilder sbNav = new StringBuilder();
            sbNav.AppendFormat("<ul class=\"{0}\">", this.CssName);
            int i = 0;
            foreach (NavigatorItem m in nav.Items)
            {
                sbNav.AppendFormat("<li {0} ><a onfocus=\"javascript:this.blur();\" href=\"{1}\">{2}</a></li>",
                    this.SelectedId == m.Id ? "class=\"selected\"" : "",
                    (m.Href.StartsWith("javascript:") ?
                        m.Href : (GlobalSettings.RelativeWebRoot + m.Href)),
                    m.Title);
                i++;
                if (i != nav.Items.Length - 1)
                { 
                    sbNav.Append("<li class=\"split\">&nbsp;</li>");
                }
            }
            return sbNav.ToString() + "</ul>";
        }

        public override void RenderControl(HtmlTextWriter writer)
        {
            string html = this.RenderNav();
            if (!string.IsNullOrEmpty(html))
            {
                writer.Write(html);
                writer.WriteLine(Environment.NewLine);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using OperatingManagement.Framework;
using OperatingManagement.Framework.Core;

namespace OperatingManagement.Security
{
    /// <summary>
    /// Identity management. 
    /// </summary>
    public class AspNetIdentity:IIdentity
    {
        private string cookiePrefix = "O__M__Cookie_";
        private FormsAuthenticationTicket ticket = null;
        private HttpContext context = HttpContext.Current;
        Dictionary<string, string> dics = new Dictionary<string, string>();
        /// <summary>
        /// Create a new instance of <see cref="OperatingManagement.Security.AspNetIdentity"/> class.
        /// </summary>
        /// <param name="ticket"></param>
        public AspNetIdentity(FormsAuthenticationTicket ticket)
        {
            this.ticket = ticket;
            cookiePrefix = AspNetConfig.Config["cookiePrefix"].ToString();

            #region -eg.-
            //dics.Add("Email", cookiePrefix + "Email");
            #endregion
        }

        #region -IIdentity Member-
        private string authenticationType = "OMOnlineUser";
        /// <summary>
        /// Gets the authentication type.
        /// </summary>
        public string AuthenticationType
        {
            get { return authenticationType; }
        }
        /// <summary>
        /// Gets whether the user info is authenticated.
        /// </summary>
        public bool IsAuthenticated
        {
            get { return true; }
        }
        /// <summary>
        /// Gets the name of ticket(i.e.: User Name).
        /// </summary>
        public string Name
        {
            get { return this.ticket.Name; }
        }
        #endregion

        #region -Custom Fields eg.-
        /*
        public string Email
        {
            get
            {
                HttpCookie cookie = context.Request.Cookies[dics["Email"]];

                if (cookie == null || String.IsNullOrEmpty(cookie.Value))
                {
                    string value = "angelxiongjun[at]163.com";   //from database
                    SetCookie("Email", value);
                }

                return cookie.Value;
            }
        }

        private void SetCookie(string name,string value)
        {
            HttpCookie cookie = new HttpCookie(dics[name], value);
            cookie.Expires = DateTime.Now.AddDays(1);
            context.Response.Cookies.Add(cookie);
        }
         * */
        #endregion
    }
}

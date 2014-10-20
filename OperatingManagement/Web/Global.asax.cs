using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.IO;
using OperatingManagement.Framework.Core;
using System.Web.Routing;
using OperatingManagement.WebKernel.Route;
using OperatingManagement.Security;
using OperatingManagement.DataAccessLayer.System;

namespace OperatingManagement.Web
{
    public class Global : System.Web.HttpApplication
    {
        void Application_BeginRequest(object sender, EventArgs e)
        {

        }
        void Application_Start(object sender, EventArgs e)
        {
            log4net.Config.XmlConfigurator.Configure(new FileInfo(GlobalSettings.MapPath("~/Log4Net.config")));
            // Validate the certificate and return true or false as appropriate.
            // Note that it not a good practice to always return true because not
            // all certificates should be trusted.
            //System.Net.ServicePointManager.ServerCertificateValidationCallback =
            //    (obj, certificate, chain, sslPolicyErrors) => true;

            RegisterRoutes();
        }
        void RegisterRoutes()
        {
            //RouteTable.Routes.Add("Login",
            //    new Route("Login",
            //        new RouteHandler("~/Account/Login.aspx")));

            //RouteTable.Routes.Add("OM",
            //    new Route("OM/{name}",
            //        new RouteHandler("~/Default.aspx")));

            //RouteTable.Routes.Add("Error", 
            //    new Route("Error/{code}", 
            //        new ErrorRouteHandler("~/views/exp/error.aspx")));
        }
        void Application_OnPostAuthenticateRequest(object sender, EventArgs e)
        {
            System.Security.Principal.IPrincipal user = HttpContext.Current.User;
            if (user.Identity.IsAuthenticated && user.Identity.AuthenticationType == "Forms")
            {
                FormsIdentity formIdentity = user.Identity as FormsIdentity;
                AspNetIdentity identity = new AspNetIdentity(formIdentity.Ticket);
                WebProfile profile = new WebProfile(HttpContext.Current.Profile);
                List<Permission> permissions = null;
                var p = new DataAccessLayer.System.Permission();
                permissions = p.SelectByLoginName(user.Identity.Name);
                AspNetPrincipal principal = new AspNetPrincipal(identity, permissions);
                HttpContext.Current.User = principal;
                System.Threading.Thread.CurrentPrincipal = principal;
            }
        }
        void Application_End(object sender, EventArgs e)
        {

        }

        void Application_Error(object sender, EventArgs e)
        {
            HttpApplication application = (HttpApplication)sender;
            HttpContext context = application.Context;

            Exception ex = context.Server.GetLastError();
            Exception baseEx = ex.GetBaseException();
            AspNetException exp = null;
            if (ex is AspNetException)
            {
                exp = ex as AspNetException;
            }
            else if (ex is HttpException)
            {
                if ((ex as HttpException).GetHttpCode() == 404)
                {
                    Server.Transfer("~/views/exp/pnf404.aspx");
                    context.Server.ClearError();
                    return;
                }
            }

            if (exp == null)
            {
                if (ex is HttpUnhandledException && baseEx is HttpUnhandledException)
                {
                    context.Server.ClearError();
                    return;
                }
                else
                {
                    if (baseEx is AspNetException)
                        exp = baseEx as AspNetException;
                    else
                        exp = new AspNetException(ex.ToString());
                }
            }


            exp.Log();

            Server.Transfer("~/views/exp/exp.aspx");
        }

        void Session_End(object sender, EventArgs e)
        {


        }

    }
}

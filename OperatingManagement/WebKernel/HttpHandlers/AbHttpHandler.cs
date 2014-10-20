using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace OperatingManagement.WebKernel.HttpHandlers
{
    /// <summary>
    /// Declares a basic class which most http handlers will be derived.
    /// <remarks>(Implements AJAX services)</remarks>
    /// </summary>
    abstract public class AbHttpHandler:IHttpHandler
    {
        public bool IsReusable
        {
            get { return false; }
        }

        virtual public void ProcessRequest(HttpContext context)
        { }
        protected void WriteResponse(string msg, bool suc)
        {
            HttpResponse resp = HttpContext.Current.Response;
            resp.Clear();
            resp.ContentType = "text/plan";
            resp.Write("({suc:" + suc.ToString().ToLower() + ",msg:'" + msg + "'})");
        }
    }
}

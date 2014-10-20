using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using System.Web;
using System.Web.Compilation;
using System.Web.UI;

namespace OperatingManagement.WebKernel.Route
{
    /// <summary>
    /// A class to process a request for a normal route pattern.
    /// </summary>
    public class RouteHandler:IRouteHandler
    {
        /// <summary>
        /// Create a new instance of <see cref="RouteHandler"/> class.
        /// </summary>
        public RouteHandler():this(string.Empty) { }
        /// <summary>
        /// Create a new instance of <see cref="RouteHandler"/> class.
        /// </summary>
        /// <param name="virtualPath">The virtual path of current routing page</param>
        public RouteHandler(string virtualPath)
        {
            this.VirtualPath = virtualPath;
        }
        /// <summary>
        /// Gets or sets the virtual path of current routing page.
        /// </summary>
        public string VirtualPath { get; set; }
        /// <summary>
        /// Provides the object that processes the request.
        /// </summary>
        /// <param name="requestContext">An object that encapsulates information about the request.</param>
        /// <returns></returns>
        virtual public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            var context = BuildManager.CreateInstanceFromVirtualPath(
                            VirtualPath, typeof(Page)) as IRouteContext;
            return context;
        }
    }
}

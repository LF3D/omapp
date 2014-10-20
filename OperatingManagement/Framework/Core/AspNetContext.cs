using System;
using System.Collections.Specialized;
using System.IO;
using System.Threading;
using System.Web;
using System.Collections;
using System.Text;
using System.Collections.Generic;

namespace OperatingManagement.Framework.Core
{
    /// <summary>
    /// Provides the data exchange handles from context
    /// <remarks>also can deal with WEB REQUEST</remarks>
    /// </summary>
    public class AspNetContext
    {
        #region -Private Properties-
        private HybridDictionary _items = new HybridDictionary();
        private NameValueCollection _queryString = null;
        private HttpContext _httpContext = null;
        private Uri _currentUri;
        private string _rawUrl;
        #endregion

        #region -Public Properties-
        /// <summary>
        /// Gets/Sets QueryString
        /// </summary>
        public NameValueCollection QueryString
        {
            get { return _queryString; }
            set { _queryString = value; }
        }

        /// <summary>
        /// Like Context.Items(Gets)
        /// </summary>
        public IDictionary Items
        {
            get { return _items; }
        }

        /// <summary>
        /// Indexer from Items
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object this[string key]
        {
            get
            {
                return this.Items[key];
            }
            set
            {
                this.Items[key] = value;
            }
        }

        /// <summary>
        /// Gets HttpContext
        /// </summary>
        public HttpContext Context
        {
            get
            {
                return _httpContext;
            }
        }

        /// <summary>
        /// Gets/Sets Raw Url
        /// </summary>
        public string RawUrl
        {
            get
            {
                return _rawUrl;
            }
            set
            {
                _rawUrl = value;
            }
        }
        /// <summary>
        /// Gets/Sets the current Url
        /// </summary>
        public Uri CurrentUri
        {
            get
            {
                if (_currentUri == null)
                    _currentUri = new Uri("http://localhost/OM");

                return _currentUri;

            }
            set
            {
                _currentUri = value;
            }
        }

        /// <summary>
        /// Gets Whether it is the WebRequest.
        /// </summary>
        public bool IsWebRequest
        {
            get { return this.Context != null; }
        }
       
        #endregion

        #region -Construtor-
        private void Initialize(NameValueCollection qs, Uri uri, string rawUrl)
        {
            _queryString = qs;
            _currentUri = uri;
            _rawUrl = rawUrl;
        }

        /// <summary>
        /// Serialize the HttpContext as the AspNetContext
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <param name="includeQS">Whether include QueryString</param>
        private AspNetContext(HttpContext context, bool includeQS)
        {
            this._httpContext = context;
            if (includeQS)
            {
                Initialize(new NameValueCollection(context.Request.QueryString), context.Request.Url, context.Request.RawUrl);
            }
            else
            {
                Initialize(null, context.Request.Url, context.Request.RawUrl);
            }
        }

        private AspNetContext()
        {
            Initialize(new NameValueCollection(), null, string.Empty);
        }

        private static readonly string dataKey = "AspNetContextStore";

        [ThreadStatic]
        private static AspNetContext currentContext = null;

        /// <summary>
        /// Gets current AspNetContext
        /// </summary>
        public static AspNetContext Current
        {
            get
            {
                HttpContext ctx = HttpContext.Current;

                if (ctx != null)
                {
                    if (ctx.Items.Contains(dataKey))
                        return ctx.Items[dataKey] as AspNetContext;
                    else
                    {
                        AspNetContext context = new AspNetContext(ctx, true);
                        SaveContextToStore(context);
                        return context;
                    }
                }
                else if (currentContext == null)
                {
                    AspNetContext context = new AspNetContext();
                    SaveContextToStore(context);
                }
                if (currentContext == null)
                    throw new NullReferenceException("Failed to created a new IMPContext.");
                return currentContext;
            }
        }

        private static void SaveContextToStore(AspNetContext context)
        {
            if (context.IsWebRequest)
            {
                context.Context.Items[dataKey] = context;
            }
            else
            {
                currentContext = context;
            }
        }
        /// <summary>
        /// Unload and dispose the current context.
        /// </summary>
        public static void Unload()
        {
            currentContext = null;
        }
        #endregion
    }
}

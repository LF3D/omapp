using System;
using System.Web;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace OperatingManagement.Framework.Helper
{
    /// <summary>
    /// Provides the class to deal with web operations.
    /// </summary>
    public class WebHelper
    {
        #region -Members-
        public static Regex _escapePeriod = new Regex(@"(?:\.config|\.ascx|\.asax|\.cs|\.vb)$", RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex _fileComponentTextToEscape = new Regex(@"([^A-Za-z0-9 ]+|\.| )", RegexOptions.Singleline | RegexOptions.Compiled);
        public static Regex _fileComponentTextToUnescape = new Regex(@"((?:_(?:[0-9a-f][0-9a-f][0-9a-f][0-9a-f])+_)|_|\-)", RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex _pathComponentTextToEscape = new Regex(@"([^A-Za-z0-9\- ]+|\.| )", RegexOptions.Singleline | RegexOptions.Compiled);
        public static Regex _pathComponentTextToUnescape = new Regex(@"((?:_(?:[0-9a-f][0-9a-f][0-9a-f][0-9a-f])+_)|\+)", RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static Regex _strayAmpRegex = new Regex("&(?!(?:#[0-9]{2,4};|[a-z0-9]+;))", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        #endregion

        #region -AppendQuerystring-
        /// <summary>
        /// Append QueryString to the url.
        /// </summary>
        /// <param name="url">Url string.</param>
        /// <param name="querystring">QueryString.</param>
        /// <returns></returns>
        public static string AppendQuerystring(string url, string querystring)
        {
            return AppendQuerystring(url, querystring, false);
        }
        /// <summary>
        /// Append QueryString to the url.
        /// <remarks>if u set urlEncoded as true, the '&' will be instead by '&amp;'</remarks>
        /// </summary>
        /// <param name="url">Url.</param>
        /// <param name="querystring">QueryString.</param>
        /// <param name="urlEncoded">UrlEncoded</param>
        /// <returns></returns>
        public static string AppendQuerystring(string url, string querystring, bool urlEncoded)
        {
            string str = "?";
            if (url.IndexOf('?') > -1)
            {
                if (!urlEncoded) { str = "&"; }
                else { str = "&amp;"; }
            }
            return (url + str + querystring);
        }
        #endregion

        #region -EnsureHtmlEncoded-
        /// <summary>
        /// Encode the html.
        /// </summary>
        /// <param name="text">Encoding text</param>
        /// <returns></returns>
        public static string EnsureHtmlEncoded(string text)
        {
            text = _strayAmpRegex.Replace(text, "&amp;");
            text = text.Replace("\"", "&quot;");
            text = text.Replace("'", "&#39;");
            text = text.Replace("<", "&lt;");
            text = text.Replace(">", "&gt;");
            return text;
        }
        #endregion

        #region -FullPath-
        /// <summary>
        /// Gets full path with http|https://domain/...
        /// </summary>
        /// <param name="local">local path relative to web root.</param>
        /// <returns></returns>
        public static string FullPath(string local)
        {
            if (string.IsNullOrEmpty(local))
                return local;

            if (local.ToLower().StartsWith("http://") || local.ToLower().StartsWith("https://"))
                return local;

            if (HttpContext.Current == null)
                return local;

            return FullPath(HostPath(HttpContext.Current.Request.Url), local);
        }
        private static string FullPath(string hostPath, string local)
        {
            return (hostPath + local);
        }
        /// <summary>
        /// Gets Host path with schema and port
        /// <remarks>such as http://localhost:8080/ .</remarks>
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static string HostPath(Uri uri)
        {
            string str = ((uri.Port == 80) ? string.Empty : (":" + uri.Port.ToString()));
            return string.Format("{0}://{1}{2}", uri.Scheme, uri.Host, str);
        }
        #endregion

        #region -404 set-
        public static void Return404(HttpContext Context)
        {
            Context.Response.StatusCode = 0x194;
            Context.Response.SuppressContent = true;
            Context.Response.End();
        }
        #endregion
    }
}

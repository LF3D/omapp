using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace OperatingManagement.Framework.Core
{
    /// <summary>
    /// Provides a sealed class for cookie operations.
    /// </summary>
    public class AspNetCookie
    {
        #region -Get Cookie-
        /// <summary>
        /// Gets the value by name from current cookie context.
        /// </summary>
        /// <param name="cookieName"></param>
        /// <returns></returns>
        public static string GetCookieValue(string cookieName)
        {
            return GetCookieValue(cookieName, null);
        }

        /// <summary>
        /// Gets the value by key from current cookie.
        /// </summary>
        /// <param name="cookieName">Cookie Name</param>
        /// <param name="key">Cookie Key</param>
        /// <returns></returns>
        public static string GetCookieValue(string cookieName, string key)
        {
            HttpRequest request = HttpContext.Current.Request;
            if (request != null)
                return GetCookieValue(request.Cookies[cookieName], key);
            return string.Empty;
        }

        /// <summary>
        /// Gets the value by key from the cookie
        /// </summary>
        /// <param name="cookie">Cookie object.</param>
        /// <param name="key">Key</param>
        /// <returns></returns>
        public static string GetCookieValue(HttpCookie cookie, string key)
        {
            if (cookie != null)
            {
                if (!string.IsNullOrEmpty(key) && cookie.HasKeys)
                    return cookie.Values[key];
                else
                    return cookie.Value;
            }
            return string.Empty;
        }

        /// <summary>
        /// Gets cookie by name.
        /// </summary>
        /// <param name="cookieName">Cookie name</param>
        /// <returns></returns>
        public static HttpCookie GetCookie(string cookieName)
        {
            HttpRequest request = HttpContext.Current.Request;
            if (request != null)
                return request.Cookies[cookieName];
            return null;
        }

        #endregion

        #region -Delete Cookie-

        /// <summary>
        /// Remove cookie by name.
        /// </summary>
        /// <param name="cookieName">Cookie name</param>
        public static void RemoveCookie(string cookieName)
        {
            RemoveCookie(cookieName, null);
        }

        /// <summary>
        /// Remove cookie value by key
        /// </summary>
        /// <param name="cookieName">Cookie name.</param>
        /// <param name="key">Key</param>
        public static void RemoveCookie(string cookieName, string key)
        {
            HttpResponse response = HttpContext.Current.Response;
            if (response != null)
            {
                HttpCookie cookie = response.Cookies[cookieName];
                if (cookie != null)
                {
                    if (!string.IsNullOrEmpty(key) && cookie.HasKeys)
                        cookie.Values.Remove(key);
                    else
                        response.Cookies.Remove(cookieName);
                }
            }
        }

        #endregion

        #region -Update/Set Cookie-
        /// <summary>
        /// Sets the cookie value&&key pair
        /// </summary>
        /// <param name="cookieName">Cookie name.</param>
        /// <param name="key">Key</param>
        /// <param name="value">value</param>
        public static void SetCookie(string cookieName, string key, string value)
        {
            SetCookie(cookieName, key, value, null);
        }

        /// <summary>
        /// Sets the cookie value&&key pair
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        public static void SetCookie(string key, string value)
        {
            SetCookie(key, null, value, null);
        }

        /// <summary>
        /// Sets the cookie value&&key pair with the expires.
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <param name="expires">Expires(Time out date)</param>
        public static void SetCookie(string key, string value, DateTime expires)
        {
            SetCookie(key, null, value, expires);
        }

        /// <summary>
        /// Sets the cookie expires
        /// </summary>
        /// <param name="cookieName">Cookie name.</param>
        /// <param name="expires">Expires(Time out date)</param>
        public static void SetCookie(string cookieName, DateTime expires)
        {
            SetCookie(cookieName, null, null, expires);
        }

        /// <summary>
        /// Sets the cookie value&&key pair with the expires.
        /// </summary>
        /// <param name="cookieName">Cookie name</param>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <param name="expires">Expires(Time out date)</param>
        public static void SetCookie(string cookieName, string key, string value, DateTime? expires)
        {
            HttpResponse response = HttpContext.Current.Response;
            if (response != null)
            {
                HttpCookie cookie = response.Cookies[cookieName];
                if (cookie != null)
                {
                    if (!string.IsNullOrEmpty(key) && cookie.HasKeys)
                        cookie.Values.Set(key, value);
                    else
                        if (!string.IsNullOrEmpty(value))
                            cookie.Value = value;
                    if (expires != null)
                        cookie.Expires = expires.Value;
                    response.SetCookie(cookie);
                }
            }

        }

        #endregion

        #region -Add Cookie-

        /// <summary>
        /// Add Cookie to current context.
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        public static void AddCookie(string key, string value)
        {
            AddCookie(new HttpCookie(key, value));
        }

        /// <summary>
        /// Add Cookie to current context.
        /// </summary>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <param name="expires">Expires(Time out date)</param>
        public static void AddCookie(string key, string value, DateTime expires)
        {
            HttpCookie cookie = new HttpCookie(key, value);
            cookie.Expires = expires;
            AddCookie(cookie);
        }

        /// <summary>
        /// Add Cookie to current context.
        /// </summary>
        /// <param name="cookieName">Cookie name</param>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        public static void AddCookie(string cookieName, string key, string value)
        {
            HttpCookie cookie = new HttpCookie(cookieName);
            cookie.Values.Add(key, value);
            AddCookie(cookie);
        }

        /// <summary>
        /// Add Cookie to current context.
        /// </summary>
        /// <param name="cookieName">Cookie name</param>
        /// <param name="expires">Expires(Time out date)</param>
        public static void AddCookie(string cookieName, DateTime expires)
        {
            HttpCookie cookie = new HttpCookie(cookieName);
            cookie.Expires = expires;
            AddCookie(cookie);
        }

        /// <summary>
        /// Add Cookie to current context.
        /// </summary>
        /// <param name="cookieName">Cookie name</param>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <param name="expires">Expires(Time out date)</param>
        public static void AddCookie(string cookieName, string key, string value, DateTime expires)
        {
            HttpCookie cookie = new HttpCookie(cookieName);
            cookie.Expires = expires;
            cookie.Values.Add(key, value);
            AddCookie(cookie);
        }

        /// <summary>
        /// Add Cookie to current context.
        /// </summary>
        /// <param name="cookie">Cookie object</param>
        public static void AddCookie(HttpCookie cookie)
        {
            HttpResponse response = HttpContext.Current.Response;
            if (response != null)
            {
                cookie.HttpOnly = true;

                cookie.Path = "/";

                response.AppendCookie(cookie);
            }
        }

        #endregion
    }
}

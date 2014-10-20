using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web;
using System.Web.UI;
using OperatingManagement.Framework;
using OperatingManagement.Framework.Core;
using OperatingManagement.Framework.Reflector;
using OperatingManagement.Framework.Components;
using OperatingManagement.Security;

namespace OperatingManagement.WebKernel.Basic
{
    /// <summary>
    /// Basic sealed class for master pages.
    /// </summary>
    public class AspNetMasterPage : MasterPage
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (settings == null)
                settings = AspNetSetting.Load<SiteSetting>();
            OnPageLoaded();
        }

        #region -Properties-
        /// <summary>
        /// Gets profile of user.
        /// </summary>
        public WebProfile Profile
        {
            get
            {
                return new WebProfile(this.Context.Profile);
            }
        }
        private SiteSetting settings = null;
        private string _ShortTitle;
        /// <summary>
        /// Gets/Sets the shorttitle, it will be combined with SiteName as the Title of web pages
        /// </summary>
        public string ShortTitle
        {
            get { return _ShortTitle; }
            set { _ShortTitle = value; }
        }
        /// <summary>
        /// Gets the CopyRight info from site settings.
        /// </summary>
        public string CopyRight
        {
            get
            {
                if (settings == null)
                    settings = AspNetSetting.Load<SiteSetting>();
                return settings.CopyRight;
            }
        }
        #endregion

        #region -Abstract method-
        /// <summary>
        /// Add encoding meta to the head of page(utf-8 in base class).
        /// </summary>
        public virtual void AddEncodeMeta()
        {
            HtmlMeta meta = new HtmlMeta();
            meta.Attributes.Add("content", "text/html; charset=utf-8");
            meta.Attributes.Add("http-equiv", "Content-Type");
            Page.Header.Controls.Add(meta);
        }
        /// <summary>
        /// Set Page Title
        /// </summary>
        public virtual void SetTitle()
        {
            if (!string.IsNullOrEmpty(_ShortTitle))
                Page.Title = _ShortTitle + " - " + settings.SiteName;
            else
                Page.Title = settings.SiteName;
        }

        /// <summary>
        /// This method was fired when the page is loaded(it will add the basic data).
        /// <remarks>
        /// basic data:
        /// ecodeMeta
        /// icon
        /// jquery-min.js
        /// util.js
        /// setTitle()
        /// </remarks>
        /// </summary>
        public virtual void OnPageLoaded()
        {
            FixedCssLocation();
            AddEncodeMeta();
            AddGenericLink("image/x-icon", "shortcut icon", "ICON", "images/favicon.ico");
            AddJavaScriptInclude("scripts/core/jquery-1.7.1.min.js", false, false);
            AddJavaScriptInclude("scripts/core/jquery-ui-1.8.17.custom.min.js", false, false);
            AddJavaScriptInclude("scripts/core/util.js", false, false);
            //AddJavaScriptInclude("scripts/core/jquery.ui.datepicker.js", false, false);
            //AddJavaScriptInclude("scripts/core/jquery.ui.datepicker-zh-CN.js", false, false);
            //AddJavaScriptInclude("scripts/core/jquery.ui.widget.js", false, false);
            AddJavaScriptInclude("scripts/core/jquery.ui.core.js", false, false);
            SetTitle();
        }
        /// <summary>
        /// Register javascript block to client
        /// </summary>
        /// <param name="js">Javascript Text</param>
        public virtual void RegJs(string js)
        {
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "regjs", js, true);
        }
        /// <summary>
        /// Add keywords to the head of page.
        /// </summary>
        /// <param name="value">keywords value</param>
        public virtual void AddKeywords(string value)
        {
            HtmlMeta meta = new HtmlMeta();
            meta.Name = "keywords";
            meta.Content = value;
            Page.Header.Controls.Add(meta);
        }
        /// <summary>
        /// Executing javascript text
        /// </summary>
        /// <param name="script">Javascript Text</param>
        /// <param name="isInHead">Whether put it in the head of page</param>
        public virtual void ExecuteJs(string script, bool isInHead)
        {
            string str = string.Format("\n<script  type=\"text/javascript\">\n{0}\n</script>\n", script);
            if (!isInHead)
            {
                Page.ClientScript.RegisterStartupScript(Type.GetType("System.String"), Guid.NewGuid().ToString(), str);
            }
            else
            {
                HtmlGenericControl child = new HtmlGenericControl("script");
                child.Attributes.Add("type", "text/javascript");
                child.InnerHtml = "\n" + script + "\n";
                Page.Header.Controls.Add(child);
            }
        }
        /// <summary>
        /// Add keywords in the head of page.
        /// </summary>
        /// <param name="values">keywords</param>
        public virtual void AddKeywords(params string[] values)
        {
            HtmlMeta meta = null;
            foreach (string v in values)
            {
                meta = new HtmlMeta();
                meta.Name = "keywords";
                meta.Content = v;
                Page.Header.Controls.Add(meta);
            }
        }
        /// <summary>
        /// Add description in the head of page.
        /// </summary>
        /// <param name="value">Description</param>
        public virtual void AddDescription(string value)
        {
            HtmlMeta meta = new HtmlMeta();
            meta.Name = "description";
            meta.Content = value;
            Page.Header.Controls.Add(meta);
        }
        /// <summary>
        /// Add descriptions in the head of page.
        /// </summary>
        /// <param name="values">descriptions</param>
        public virtual void AddDescription(params string[] values)
        {
            HtmlMeta meta = null;
            foreach (string v in values)
            {
                meta = new HtmlMeta();
                meta.Name = "description";
                meta.Content = v;
                Page.Header.Controls.Add(meta);
            }
        }
        /// <summary>
        /// Add link meta in the head of page.
        /// </summary>
        /// <param name="relation">Relation Type</param>
        /// <param name="title">Title</param>
        /// <param name="href">Href(relative to the root, ex '/', usage as: 'css/a.css')</param>
        public virtual void AddGenericLink(string relation, string title, string href)
        {
            HtmlLink link = new HtmlLink();
            link.Attributes["rel"] = relation;
            link.Attributes["title"] = title;
            link.Attributes["href"] = GlobalSettings.RelativeWebRoot + href;
            Page.Header.Controls.Add(link);
        }
        /// <summary>
        /// Add link meta in the head of page.
        /// </summary>
        /// <param name="type">Type</param>
        /// <param name="relation">Relation Type</param>
        /// <param name="title">Title</param>
        /// <param name="href">Href(relative to the root, ex '/', usage as: 'css/a.css')</param>
        public virtual void AddGenericLink(string type, string relation, string title, string href)
        {
            HtmlLink link = new HtmlLink();
            link.Attributes["type"] = type;
            link.Attributes["rel"] = relation;
            link.Attributes["title"] = title;
            link.Attributes["href"] = GlobalSettings.RelativeWebRoot + href;
            Page.Header.Controls.Add(link);
        }
        /// <summary>
        /// Add javascript reference in the head of page.
        /// </summary>
        /// <param name="url">Url(relative to the root, ex '/', usage as: 'scripts/a.js')</param>
        /// <param name="placeInBottom">Whether put it in the bottom of page</param>
        /// <param name="addDeferAttribute">Whether defer(dont execute) after page load</param>
        public virtual void AddJavaScriptInclude(string url, bool placeInBottom, bool addDeferAttribute)
        {
            url = GlobalSettings.RelativeWebRoot + url;
            if (placeInBottom)
            {
                string script = "<script type=\"text/javascript\"" + (addDeferAttribute ? string.Empty : " defer=\"defer\"") + " src=\"" + url + "\"></script>";
                Page.ClientScript.RegisterStartupScript(GetType(), url.GetHashCode().ToString(), script);
            }
            else
            {
                HtmlGenericControl script = new HtmlGenericControl("script");
                script.Attributes["type"] = "text/javascript";
                script.Attributes["src"] = url;
                if (addDeferAttribute)
                {
                    script.Attributes["defer"] = "defer";
                }

                Page.Header.Controls.Add(script);
            }
        }
        protected virtual void FixedCssLocation()
        {
            HtmlControl hc = null;
            foreach (Control c in Page.Header.Controls)
            {
                hc = c as HtmlControl;
                if (hc != null && hc.Attributes["type"] != null &&
                    hc.Attributes["type"].Equals("text/css", StringComparison.OrdinalIgnoreCase))
                {
                    if (!hc.Attributes["href"].StartsWith("http"))
                    {
                        string path = hc.Attributes["href"];
                        if (path.StartsWith("~/"))
                        {
                            path = GlobalSettings.RelativeWebRoot + path.Substring(2);
                        }
                        hc.Attributes["href"] = path;
                        hc.EnableViewState = false;
                    }
                }
            }
        }
        #endregion
    }
}

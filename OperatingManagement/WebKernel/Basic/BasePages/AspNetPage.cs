using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web;
using System.Web.UI.WebControls;
using OperatingManagement.Security;
using OperatingManagement.Framework.Reflector;
using OperatingManagement.WebKernel;
using OperatingManagement.Framework.Components;
using OperatingManagement.WebKernel.Permission;
using OperatingManagement.Framework.Core;

namespace OperatingManagement.WebKernel.Basic
{
    /// <summary>
    /// Basic sealed class from pages.
    /// </summary>
    public class AspNetPage : Page
    {
        #region -Constructor&& PreRender-
        static AspNetPage()
        {
            _ControlPermissionChecked = new object();
            _ControlPermissionChecking = new object();
        }
        public AspNetPage()
        {
            _CheckedControl = new Dictionary<string, List<Control>>();
        }
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (_settings == null)
                _settings = AspNetSetting.Load<SiteSetting>();
            if (User.Identity.IsAuthenticated)
                _principal = User as AspNetPrincipal;
            OnPageLoaded();
            PermissionCheckingArgs pcArgs = new PermissionCheckingArgs(new Dictionary<string, List<Control>>());
            HandlePermissionChecking(pcArgs);

            bool isCheck = (!pcArgs.Cancel && this._AllowCheckPermission);
            if (isCheck)
            {
                OnPagePermissionChecking();
                HandleControlPermissionChecked(pcArgs);
            }
        }
        #endregion

        #region -Properties-
        private bool _IsPopOrIFrame = false;
        private bool _IsViewOrEdit = false;
        /// <summary>
        /// Gets/Sets whether the inherit page is pop windows or in a iframe.
        /// </summary>
        public bool IsPopOrIframe
        {
            get { return _IsPopOrIFrame; }
            set { _IsPopOrIFrame = value; }
        }
        private AspNetPrincipal _principal = null;
        /// <summary>
        /// Gets/sets the principal information.
        /// </summary>
        public AspNetPrincipal Principal
        {
            get { return _principal; }
            set { _principal = value; }
        }
        private SiteSetting _settings = null;
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
                return SiteSetting.CopyRight;
            }
        }
        /// <summary>
        /// Gets the site setting.
        /// </summary>
        public SiteSetting SiteSetting
        {
            get
            {
                if (_settings == null)
                    _settings = AspNetSetting.Load<SiteSetting>();
                return _settings;
            }
        }
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

        /// <summary>
        /// 获得当前登录用户信息
        /// </summary>
        public DataAccessLayer.System.User LoginUserInfo
        {
            get 
            {
                DataAccessLayer.System.User user = null;
                if (User.Identity.IsAuthenticated)
                {
                    user = new DataAccessLayer.System.User();
                    user.LoginName = User.Identity.Name;
                    user = user.SelectByLoginName();
                }
                return user;
            }
        }

        /// <summary>
        /// true for view, false for edit
        /// </summary>
        public bool IsViewOrEdit
        {
            get { return _IsViewOrEdit; }
            set { _IsViewOrEdit = value; }
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
                Page.Title = _ShortTitle + " - " + _settings.SiteName;
            else
                Page.Title = _settings.SiteName;
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
            SetTitle();
        }
        /// <summary>
        /// Register the js string to client
        /// </summary>
        /// <param name="js">Javascript Text</param>
        public virtual void RegJs(string js)
        {
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "regjs", js, true);
        }
        /// <summary>
        /// Add keywords to the head of page.
        /// </summary>
        /// <param name="value">keywords 值</param>
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
        public virtual void ExecuteJs(string script, bool isInHead=true)
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
        public virtual void AddJavaScriptInclude(string url, bool placeInBottom = false, bool addDeferAttribute = false)
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

        /// <summary>
        /// 检测QueryString的值是否为有效值
        /// </summary>
        /// <param name="QueryStringName"></param>
        /// <returns></returns>
        public bool QueryStringObserver(params string[] QueryStringName)
        {
            for (int i = 0; i < QueryStringName.Length; i++)
            {
                if (Request.QueryString[QueryStringName[i]] != null)
                {
                    //string s = DecryptString(Request.QueryString[QueryStringName[i]]);
                    string s = Request.QueryString[QueryStringName[i]];
                    if (s == null)
                        return false;
                    //else
                    //    Response.Write("<script>alert('发生了意外的错误,您所请求的页面即将关闭！\\n请确保您没有在地址栏手动输入无效的Url或者QueryString！');window.opener=null;window.close();</script>");
                }
            }
            return true;
        }

        public void SetControlsEnabled(Control ctrl, ControlNameEnum ctrlType)
        {
            foreach (Control item in ctrl.Controls)
            {
                //Panel
                if (item is Panel && (ctrlType == ControlNameEnum.Panel || ctrlType == ControlNameEnum.All))
                    ((Panel)item).Enabled = !IsViewOrEdit;

                //TextBox
                if (ctrlType == ControlNameEnum.Panel || ctrlType == ControlNameEnum.All)
                {
                    if (item is TextBox)
                        ((TextBox)item).Enabled = !IsViewOrEdit;
                    else if (item is HtmlInputText)
                        ((HtmlInputText)item).Disabled = IsViewOrEdit;
                    else if (item is HtmlTextArea)
                        ((HtmlTextArea)item).Disabled = IsViewOrEdit;
                }

                //Button
                if (item is Button && (ctrlType == ControlNameEnum.Button || ctrlType == ControlNameEnum.All))
                {
                    if (item is Button)
                        ((Button)item).Visible = !IsViewOrEdit;
                    else if (item is HtmlInputButton)
                        ((HtmlInputButton)item).Disabled = IsViewOrEdit;
                    else if (item is ImageButton)
                        ((ImageButton)item).Enabled = !IsViewOrEdit;
                    else if (item is LinkButton)
                        ((LinkButton)item).Enabled = !IsViewOrEdit;
                }

                //CheckBox
                if (ctrlType == ControlNameEnum.CheckBox || ctrlType == ControlNameEnum.All)
                {
                    if (item is CheckBox)
                        ((CheckBox)item).Enabled = !IsViewOrEdit;
                    else if (item is HtmlInputCheckBox)
                        ((HtmlInputCheckBox)item).Disabled = IsViewOrEdit;
                }

                //List Controls
                if (ctrlType == ControlNameEnum.ListControl || ctrlType == ControlNameEnum.All)
                {
                    if (item is DropDownList)
                        ((DropDownList)item).Enabled = !IsViewOrEdit;
                    else if (item is RadioButtonList)
                        ((RadioButtonList)item).Enabled = !IsViewOrEdit;
                    else if (item is CheckBoxList)
                        ((CheckBoxList)item).Enabled = !IsViewOrEdit;
                    else if (item is ListBox)
                        ((ListBox)item).Enabled = !IsViewOrEdit;
                    else if (item is HtmlSelect)
                        ((HtmlSelect)item).Disabled = IsViewOrEdit;
                }

                //File Upload Controls
                if (item is FileUpload && (ctrlType == ControlNameEnum.FileUpload || ctrlType == ControlNameEnum.All))
                {
                    ((FileUpload)item).Enabled = !IsViewOrEdit;
                }

                if (item.Controls.Count > 0)
                    SetControlsEnabled(item, ctrlType);
            }
        }

        public enum ControlNameEnum
        {
            Panel = 0,
            TextBox = 1,
            CheckBox = 2,
            ListControl = 3,
            Button = 4,
            FileUpload = 5,
            All = 100//
        }
        #endregion

        #region -Permission-
        private IDictionary<string, List<Control>> _CheckedControl;
        /// <summary>
        /// Gets/Sets the collection of permission check controls. pair of (RoleName, WebControl).
        /// </summary>
        public IDictionary<string, List<Control>> CheckedControl
        {
            get { return _CheckedControl; }
            set { _CheckedControl = value; }
        }
        private bool _AllowCheckPermission = true;
        /// <summary>
        /// Gets/Sets whether allow check permission[default(true)].
        /// </summary>
        public virtual bool AllowCheckPermission
        {
            get { return _AllowCheckPermission; }
            set { _AllowCheckPermission = value; }
        }
        private string _PagePermission = string.Empty;
        /// <summary>
        /// Gets/Sets the page permission.
        /// <remarks>
        /// 
        /// PagePermission is combined as 
        /// ModuleName.TaskName1,ModuleName.TaskName2..., e.g.: SystemManage.Add,SystemManage.Edit...
        /// 
        /// if the accessor has no permission to visit this page, it will throw an exception to deny accessors' operations.</remarks>
        /// </summary>
        public virtual string PagePermission
        {
            get { return _PagePermission; }
            set { _PagePermission = value; }
        }
        /// <summary>
        /// Gets all the permissions of accessor.
        /// <remarks>it was initialized in the 'Application_OnPostAuthenticateRequest' event in 'Global.asax'.</remarks>
        /// </summary>
        public List<DataAccessLayer.System.Permission> Permissions
        {
            get
            {
                if (_principal == null)
                    return null;
                else
                    return _principal.Permissions;
            }
        }
        private static object _ControlPermissionChecked;
        /// <summary>
        /// Add/Remove the 'ControlPermissionEventHandler' event.
        /// </summary>
        public event ControlPermissionEventHandler ControlPermissionChecked
        {
            add { base.Events.AddHandler(_ControlPermissionChecked, value); }
            remove { base.Events.RemoveHandler(_ControlPermissionChecked, value); }
        }

        private static object _ControlPermissionChecking;
        /// <summary>
        /// Add/Remove the 'PermissionCheckingEventHandler' event.
        /// </summary>
        public event PermissionCheckingEventHandler ControlPermissionChecking
        {
            add { base.Events.AddHandler(_ControlPermissionChecking, value); }
            remove { base.Events.RemoveHandler(_ControlPermissionChecking, value); }
        }
        private bool HandlePermissionChecking(PermissionCheckingArgs args)
        {
            OnPermissionChecking(args);
            return true;
        }
        /// <summary>
        /// This event should be override in the inherit class.
        /// <remarks>
        /// it fires up the permission checking event.
        /// All the permission checking event sequence:
        /// OnPermissionChecking, OnPagePermissionChecking, OnControlPermissionChecking, OnControlPermissionChecked
        /// </remarks>
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnPermissionChecking(PermissionCheckingArgs e)
        {
            PermissionCheckingEventHandler pcEvent = (PermissionCheckingEventHandler)base.Events[_ControlPermissionChecking];
            if (pcEvent != null)
            {
                pcEvent(this, e);
            }
        }
        /// <summary>
        /// Check the permission from PagePermission
        /// <remarks>
        /// it fires up the permission checking event.
        /// All the permission checking event sequence:
        /// OnPermissionChecking, OnPagePermissionChecking, OnControlPermissionChecking, OnControlPermissionChecked
        /// </remarks>
        /// </summary>
        protected virtual void OnPagePermissionChecking()
        {
            if (!HasPermission())
            {
                //throw new AspNetException("Accessor deny, you have no permissions!");
                throw new AspNetException("访问被拒绝，您没有访问权限!");
            }
        }
        /// <summary>
        /// Make sure that the accessors have permission to visit this page.
        /// <remarks>
        /// if the 'PagePermission' property is null or empty, will return true.
        /// </remarks>
        /// </summary>
        protected virtual bool HasPermission()
        {
            if (this.Profile.Account.UserType == Framework.UserType.Admin) { return true; }
            if (string.IsNullOrEmpty(this.PagePermission))
                return true;
            else
            {
                if (this.Permissions == null || this.Permissions.Count == 0)
                    return false;

                string[] ps = this.PagePermission.Split(',');
                bool hasPermission = false;
                foreach (string p in ps)
                {
                    hasPermission =  IsInPermission(p);//this.PagePermission);
                    if (hasPermission) return true;
                }
                return hasPermission;
            }
        }
        private bool IsInPermission(string permission)
        {
            if (string.IsNullOrEmpty(permission))
                return false;
            var ps = permission.Split('.');
            if (ps.Length != 2)
                return false;
            
            return this.Permissions.Any(o => o.Module.ModuleName == ps[0] && o.Task.TaskName == ps[1]);
        }
        private bool HandleControlPermissionChecked(PermissionCheckingArgs e)
        {
            if (e.Cancel)
                return false;
            OnControlPermissionChecking(e.CheckPermissionControls);

            ControlPermissionEventArgs args = new ControlPermissionEventArgs(_CheckedControl);
            OnControlPermissionChecked(args);
            return true;
        }
        /// <summary>
        /// Fires up the event when checking permission.
        /// <remarks>
        /// it fires up the permission checking event.
        /// All the permission checking event sequence:
        /// OnPermissionChecking, OnPagePermissionChecking, OnControlPermissionChecking, OnControlPermissionChecked
        /// </remarks>
        /// </summary>
        /// <param name="controls">Collections of controls</param>
        protected virtual void OnControlPermissionChecking(IDictionary<string, List<Control>> controls)
        {
            IEnumerator<KeyValuePair<string, List<Control>>> cs = controls.GetEnumerator();
            while (cs.MoveNext())
            {
                if (!IsInPermission(cs.Current.Key))
                {
                    foreach (Control c in cs.Current.Value)
                    {
                        c.Visible = false;
                    }
                }
            }
        }
        /// <summary>
        /// Fires up the event when the permission was checked.
        /// <remarks>
        /// it fires up the permission checking event.
        /// All the permission checking event sequence:
        /// OnPermissionChecking, OnPagePermissionChecking, OnControlPermissionChecking, OnControlPermissionChecked
        /// </remarks>
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnControlPermissionChecked(ControlPermissionEventArgs e)
        {
            ControlPermissionEventHandler cpEvent = (ControlPermissionEventHandler)base.Events[_ControlPermissionChecked];
            if (cpEvent != null)
            {
                cpEvent(this, e);
            }
        }

        protected override void OnInit(EventArgs e)
        {
            this.ControlPermissionChecking += new PermissionCheckingEventHandler(AspNetPage_ControlPermissionChecking);
            this.ControlPermissionChecked += new ControlPermissionEventHandler(AspNetPage_ControlPermissionChecked);
            base.OnInit(e);
        }
        /// <summary>
        /// The event was registered, so you can override this to change the behavior of control permission checked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void AspNetPage_ControlPermissionChecked(object sender, ControlPermissionEventArgs e) { }
        /// <summary>
        /// The event was registered, so you can override this to change the behavior of control permission checking
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void AspNetPage_ControlPermissionChecking(object sender, PermissionCheckingArgs e) { }
        #endregion
    }
}
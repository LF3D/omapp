using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OperatingManagement.Framework.Components
{
    /// <summary>
    /// Customize menu instance.
    /// </summary>
    public class CstMenu
    {
        /// <summary>
        /// Create a new instance of <see cref="CstMenu"/> class.
        /// </summary>
        public CstMenu() { }
        /// <summary>
        /// Title of menu.
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Sub menu items
        /// </summary>
        public List<CstMenuItem> MenuItems { get; set; }
    }
    /// <summary>
    /// Customize sub menu.
    /// </summary>
    public class CstMenuItem
    {
        /// <summary>
        /// Create a new instance of  <see cref="CstMenuItem"/> class.
        /// </summary>
        public CstMenuItem() { }
        /// <summary>
        /// Identification
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Title 
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// NavigateUrl(relative to web root)
        /// </summary>
        public string Href { get; set; }
        /// <summary>
        /// Sub menu item role
        /// </summary>
        public string Roles { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace OperatingManagement.WebKernel.Permission
{
    #region -ControlPermissionEventHandler-
    /// <summary>
    /// Provides the event for permission checking.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void ControlPermissionEventHandler(object sender, ControlPermissionEventArgs e);
    #endregion

    #region -ControlPermissionEventArgs-
    /// <summary>
    /// Provides arguments for <see cref="OperatingManagement.WebKernel.ControlPermissionEventHandler"/> instance.
    /// </summary>
    public class ControlPermissionEventArgs
    {
        private IDictionary<string, List<Control>> _Controls;
        /// <summary>
        /// Collection of permission required controls.
        /// </summary>
        public IDictionary<string, List<Control>> CheckedControls
        {
            get { return _Controls; }
            set { _Controls = value; }
        }

        /// <summary>
        /// Create a new instance of <see cref="OperatingManagement.WebKernel.ControlPermissionEventArgs"/> class.
        /// </summary>
        /// <param name="checkedControls">Checked Controls[have permissions]</param>
        public ControlPermissionEventArgs(IDictionary<string, List<Control>> checkedControls)
        {
            this._Controls = checkedControls;
        }
    }
    #endregion

    #region -PermissionCheckingEventHandler-
    /// <summary>
    /// This event is firing up when the permission is checking.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    public delegate void PermissionCheckingEventHandler(object sender, PermissionCheckingArgs e);
    #endregion

    #region -PermissionCheckingArgs-
    /// <summary>
    /// Provides arguments for <see cref="OperatingManagement.WebKernel.PermissionCheckingEventHandler"/> instance.
    /// </summary>
    public class PermissionCheckingArgs
    {
        private IDictionary<string, List<Control>> _CheckPermissionControls;
        /// <summary>
        /// Collection of permission required controls.
        /// </summary>
        public IDictionary<string, List<Control>> CheckPermissionControls
        {
            get
            {
                if (_CheckPermissionControls == null)
                    return new Dictionary<string, List<Control>>();

                return _CheckPermissionControls;
            }
        }
        
        /// <summary>
        /// Cancel the permission check.
        /// </summary>
        public bool Cancel { get; set; }
        /// <summary>
        /// Create a new instance of <see cref="OperatingManagement.WebKernel.PermissionCheckingArgs"/> class.
        /// </summary>
        /// <param name="controls"></param>
        public PermissionCheckingArgs(IDictionary<string, List<Control>> controls)
        {
            _CheckPermissionControls = controls;
        }
    }
    #endregion
}

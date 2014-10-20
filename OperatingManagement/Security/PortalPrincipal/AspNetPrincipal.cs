using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;
using OperatingManagement.DataAccessLayer.System;

namespace OperatingManagement.Security
{
    /// <summary>
    /// Provides principal management.
    /// </summary>
    public class AspNetPrincipal:IPrincipal
    {
        /// <summary>
        /// Create a new instance of <see cref="OperatingManagement.Security.AspNetPrincipal"/> class.
        /// </summary>
        /// <param name="identity">AspNetIdentity object.</param>
        /// <param name="permissions">Obtains permissions.</param>
        public AspNetPrincipal(AspNetIdentity identity,List<Permission> permissions)
        {
            this.Identity = identity;
            this.Permissions = permissions;
        }

        #region -IPrincipal Members-
        /// <summary>
        /// Gets/sets the basic info of specified Identity.
        /// </summary>
        public IIdentity Identity { get; set; }
        /// <summary>
        /// Gets/sets the permissions which current user have.
        /// </summary>
        public List<Permission> Permissions { get; set; }
        /// <summary>
        /// (Discard)Gets a value indicating whether the user is in a specified role,
        /// if you wanna use this method, you must combine the Module and Task like this:
        /// role => ModuleName.TaskName1,ModuleName.TaskName2... i.e.: SystemManage.Add,SystemManage.Edit...
        /// <remarks>
        /// 
        /// You should use the HasPermission method instead.
        /// 
        /// </remarks>
        /// </summary>
        /// <param name="role">The name of specified role.</param>
        /// <returns></returns>
        public bool IsInRole(string role)
        {
            if (string.IsNullOrEmpty(role))
                return false;
            var roles = role.Split(',');
            bool isInRole = false;
            foreach (string r in roles)
            {
                isInRole=HasPermission(r);
                if (isInRole) return true;
            }
            return isInRole;
        }
        /// <summary>
        /// Gets a value indicating whether the user has the specific permission.
        /// </summary>
        /// <param name="module">The module name.</param>
        /// <param name="task">The task name.</param>
        /// <returns></returns>
        public bool HasPermission(string module,string task) {
            if (this.Permissions == null || this.Permissions.Count == 0)
                return false;
            return this.Permissions.Any(o => o.Module.ModuleName == module && o.Task.TaskName == task);
        }
        /// <summary>
        /// Gets a value indicating whether the user has the specific permission.
        /// <remarks>
        /// 
        /// the permission is combined with Module and Task like this:
        /// role => ModuleName.TaskName, i.e.: SystemManage.Add.
        /// </remarks>
        /// </summary>
        /// <returns></returns>
        public bool HasPermission(string permission)
        {
            if (string.IsNullOrEmpty(permission))
                return false;
            var ps = permission.Split('.');
            if (ps.Length != 2) return false;
            return HasPermission(ps[0], ps[1]);
        }
        #endregion
    }
}

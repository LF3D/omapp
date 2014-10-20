using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OperatingManagement.RemotingHelper
{
    /// <summary>
    /// Represents a User object.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets/Sets the identification.
        /// </summary>
        public double Id { get; set; }
        /// <summary>
        /// Gets/Sets the unique login name.
        /// </summary>
        public string LoginName { get; set; }
        /// <summary>
        /// Gets/Sets the display name.
        /// </summary>
        public string DisplayName { get; set; }
        /// <summary>
        /// Gets/Sets the cellphone number.
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// Gets/Sets the user type.
        /// 
        /// <remarks>
        /// Including: 
        /// 0. Normal, 
        /// 1. Admin.
        /// </remarks>
        /// </summary>
        public int UserType { get; set; }
        /// <summary>
        /// Gets/Sets the user catalog.
        /// 
        /// <remarks>
        /// Including: 
        /// 0. Inside, 
        /// 1. Outside, 
        /// 2. Temporary.
        /// </remarks>
        /// </summary>
        public int UserCatalog { get; set; }
        /// <summary>
        /// Gets/Sets the State.
        /// 
        /// <remarks>
        /// Including: 
        /// 0. Active, 
        /// 1. Inactive, 
        /// 2. Locked, 
        /// 3. Deleted.
        /// </remarks>
        /// </summary>
        public int State { get; set; }
        /// <summary>
        /// Gets/Sets the time when this user was created.
        /// </summary>
        public DateTime CreatedTime { get; set; }

        public List<Role> Roles { get; set; }
    }
}

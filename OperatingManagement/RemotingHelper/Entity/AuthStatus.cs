using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OperatingManagement.RemotingHelper
{
    /// <summary>
    /// Represents the Authentication status of specific user.
    /// </summary>
    public class AuthStatus
    {
        /// <summary>
        /// Gets/Sets the autheticated code.
        /// 
        /// <remarks>Including: 
        /// 0. Exception was handled, 
        /// 1. User is not exist, 
        /// 2. Password is incorrect, 
        /// 3. User is not active, 
        /// 4. Data-exchange error, 
        /// 5. Success
        /// </remarks>
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// Gets/Sets the message, it performances the remoting details.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets/Sets the user object.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Gets/Sets the roles collection of current user.
        /// </summary>
        public List<Role> Roles { get; set; }
        /// <summary>
        /// Gets/Sets the permissions collection of current user.
        /// </summary>
        public List<Permission> Permissions { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OperatingManagement.RemotingHelper
{
    /// <summary>
    /// Represents a Role object.
    /// </summary>
    public class Role
    {
        /// <summary>
        /// Gets/Sets the identification.
        /// </summary>
        public double Id { get; set; }
        /// <summary>
        /// Gets./Sets the role name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets./Sets the role permissions.
        /// </summary>
        public List<Permission> Permissions { get; set; }
    }
}

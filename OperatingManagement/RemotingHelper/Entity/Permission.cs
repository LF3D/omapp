using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OperatingManagement.RemotingHelper
{
    /// <summary>
    /// Represents a Permission object, including Task and Module pairs.
    /// </summary>
    public class Permission
    {
        /// <summary>
        /// Gets/Sets the permissi
        /// </summary>
        public double Id { get; set; }
        /// <summary>
        /// Gets/Sets the Task object of current permission.
        /// </summary>
        public Task Task { get; set; }
        /// <summary>
        /// Gets/Sets the Module object of current permission.
        /// </summary>
        public Module Module { get; set; }
    }
}

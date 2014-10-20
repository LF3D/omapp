using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OperatingManagement.RemotingHelper
{
    /// <summary>
    /// Represents a Task object.
    /// </summary>
    public class Task
    {
        /// <summary>
        /// Gets/Sets the indentification.
        /// </summary>
        public double Id { get; set; }
        /// <summary>
        /// Gets/Sets the name of operation, e.g: Delete, Insert...
        /// </summary>
        public string Name { get; set; }
    }
}

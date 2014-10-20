using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OperatingManagement.Workflow
{
    /// <summary>
    /// Implements EventArgs for workflow data exchanging.
    /// </summary>
    [Serializable]
    public class MsfWorkflowEventArgs : EventArgs
    {
        /// <summary>
        /// Create a new instance of <see cref="OperatingManagement.Workflow.MsfWorkflowEventArgs"/> class.
        /// </summary>
        public MsfWorkflowEventArgs() { }
        /// <summary>
        /// Gets the instance identification.
        /// </summary>
        public Guid InstanceID { get; set; }
        /// <summary>
        /// Gets the name of EventDriven.
        /// </summary>
        public string EventName { get; set; }
        /// <summary>
        /// Gets the name of State.
        /// </summary>
        public string StateName { get; set; }
        /// <summary>
        /// Gets the delivery message.
        /// </summary>
        public string Message { get; set; }
    }
}

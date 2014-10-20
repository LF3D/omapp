using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OperatingManagement.Workflow
{
    /// <summary>
    /// Represents a State of workflow.
    /// </summary>
    [Serializable]
    public class MsfWorkflowState
    {
        /// <summary>
        /// Gets the name of State.
        /// </summary>
        public string StateName { get; set; }
        /// <summary>
        /// Gets the description of State.
        /// </summary>
        public string Description { get; set; }
    }
}

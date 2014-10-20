using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OperatingManagement.Workflow
{
    /// <summary>
    /// Represents the next sibling workflow step.
    /// </summary>
    [Serializable]
    public struct NextFlowStep
    {
        /// <summary>
        /// Name of this step.
        /// </summary>
        public string Name;
        /// <summary>
        /// Display text of this step.
        /// </summary>
        public string Text;
        /// <summary>
        /// Role of approval of this step.
        /// </summary>
        public string Role;
    }
}

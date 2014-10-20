using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OperatingManagement.Workflow
{
    /// <summary>
    /// Implements parameters for Workflow data exchanging.
    /// </summary>
	public class MsfWorkflowEventParameters
	{
        /// <summary>
        ///  Occurs when a workflow instance has completed.
        /// </summary>
        public EventHandler<MsfWorkflowEventArgs> WorkflowCompleted
        {
            get;
            set;
        }
        /// <summary>
        /// Occurs when a workflow instance is terminated.
        /// </summary>
        public EventHandler<MsfWorkflowEventArgs> WorkflowTerminated
        {
            get;
            set;
        }
	}
}

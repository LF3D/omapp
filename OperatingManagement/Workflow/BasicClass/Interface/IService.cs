using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Workflow.Activities;

namespace OperatingManagement.Workflow
{
    /// <summary>
    /// Declares a contract for all external data exchange service.
    /// </summary>
	public interface IService
	{
        /// <summary>
        /// Raises the specified event handler.
        /// </summary>
        /// <param name="eventName">name of event.</param>
        /// <param name="instanceId">instance identification of workflow.</param>
        void RaiseEvent(string eventName, Guid instanceId);
	}
}

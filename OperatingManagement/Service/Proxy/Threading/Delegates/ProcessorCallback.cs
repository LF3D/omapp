using System;
using System.Collections.Generic;
using System.Text;

namespace OperatingManagement.Service
{
    /// <summary>
    /// Represents a callback method to be executed by a thread pool thread and has a return value.
    /// </summary>
    /// <param name="state">An object containing information to be used by the callback method.</param>
    /// <returns></returns>
    public delegate object ProcessorCallback(object state);
}

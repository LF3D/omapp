using System;
using System.Collections.Generic;
using System.Text;

namespace OperatingManagement.Service
{
    /// <summary>
    /// Enumerate the state of process.
    /// </summary>
    internal enum ProcessState : byte
    {
        /// <summary>
        /// It means still waiting for processing.
        /// </summary>
        WaitForProcess = 0,
        /// <summary>
        /// It means in processing.
        /// </summary>
        Processing = 1,
        /// <summary>
        /// It means after processed.
        /// </summary>
        Processed = 2
    }
}

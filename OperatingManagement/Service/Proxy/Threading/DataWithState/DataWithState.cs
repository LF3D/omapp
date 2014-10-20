using System;
using System.Collections.Generic;
using System.Text;

namespace OperatingManagement.Service
{
    /// <summary>
    /// Represents the data with specified process state.
    /// </summary>
    internal class DataWithState
    {
        /// <summary>
        /// Currently data(readonly).
        /// </summary>
        public readonly object Data;
        /// <summary>
        /// Process state.
        /// </summary>
        public ProcessState State;

        /// <summary>
        /// Create a new instance of <see cref="OperatingManagement.Service.DataWithState"/> class.
        /// </summary>
        /// <param name="data">Currently data</param>
        /// <param name="state">Process state</param>
        public DataWithState(object data, ProcessState state)
        {
            Data = data;
            State = state;
        }
    }
}

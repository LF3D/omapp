using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace OperatingManagement.Service
{
    /// <summary>
    /// Declares a collection of <see cref="OperatingManagement.Service.DataWithState"/> object.
    /// </summary>
    internal class DataWithStateList : List<DataWithState>
    {
        /// <summary>
        /// Updates the specified ProcessState by Data.
        /// </summary>
        /// <param name="obj">Data</param>
        /// <param name="state">State</param>
        public void SetState(object obj, ProcessState state)
        {
            lock (((ICollection)this).SyncRoot)
            {
                DataWithState dws = this.Find(delegate(DataWithState i) { return Object.Equals(i.Data, obj); });

                if (dws != null)
                {
                    dws.State = state;
                }
            }
        }
        /// <summary>
        /// Gets the specified ProcessState by Data.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public ProcessState GetState(object obj)
        {
            lock (((ICollection)this).SyncRoot)
            {
                DataWithState dws = this.Find(delegate(DataWithState i) { return Object.Equals(i.Data, obj); });
                return dws.State;
            }
        }

        private int GetCount(ProcessState state)
        {
            List<DataWithState> datas = this.FindAll(delegate(DataWithState i) { return i.State == state; });
            if (datas == null) return 0;
            return datas.Count;
        }
        /// <summary>
        /// Returns the amount of data which is(are) still waiting for processing.
        /// </summary>
        public int WaitForDataCount
        {
            get
            {
                return GetCount(ProcessState.WaitForProcess);
            }
        }
        /// <summary>
        /// Returns one of the data which are still waiting for processing.
        /// </summary>
        /// <returns></returns>
        internal object GetWaitForObject()
        {
            lock (((ICollection)this).SyncRoot)
            {
                DataWithState dws = this.Find(delegate(DataWithState i) { return i.State == ProcessState.WaitForProcess; });
                if (dws == null) return null;
                dws.State = ProcessState.Processing;
                return dws.Data;
            }
        }
        /// <summary>
        /// Returns a value indicating that if the state of current specified Data is equal to the 'setState'.
        /// </summary>
        /// <param name="obj">Data</param>
        /// <param name="setState">State</param>
        /// <returns></returns>
        internal bool IsWaitForData(object obj, bool setState)
        {
            lock (((ICollection)this).SyncRoot)
            {
                //DataWithState dws = this.Find(delegate(DataWithState i) { return i.State == ProcessState.WaitForProcess; });
                DataWithState dws = this.Find(delegate(DataWithState i) { return i.Data == obj; });
                if (setState && dws != null) dws.State = ProcessState.Processing;

                return dws != null;
            }
        }
    }
}

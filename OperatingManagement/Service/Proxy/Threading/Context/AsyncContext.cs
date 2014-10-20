using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Threading;

namespace OperatingManagement.Service
{
    /// <summary>
    /// Class representing a context of asynchronization.
    /// </summary>
    internal class AsyncContext
    {
        /// <summary>
        /// Create a new instance of <see cref="OperatingManagement.Service.AsyncContext"/> class.
        /// </summary>
        /// <param name="treadCount">Amount of Threads.</param>
        /// <param name="dataWithStates">Data with ProcessState</param>
        /// <param name="needWaitAll">Whether need wait all the others</param>
        /// <param name="hasReturnValue">Whether has return value</param>
        /// <param name="processItemMethod">The callback function</param>
        /// <param name="returnValueMethod">Method to deal with the events and has return value.</param>
        /// <returns></returns>
        static public AsyncContext GetContext(int treadCount,
                                                DataWithStateList dataWithStates,
                                                bool needWaitAll,
                                                bool hasReturnValue,
                                                WaitCallback processItemMethod,
                                                ProcessorCallback returnValueMethod
            )
        {
            AsyncContext context = new AsyncContext();
            context.ThreadCount = treadCount;
            context.DataWithStates = dataWithStates;
            context.NeedWaitAll = needWaitAll;
            if (hasReturnValue)
            {
                Hashtable processResult = Hashtable.Synchronized(new Hashtable());
                context.ProcessResult = processResult;
                context.ReturnValueMethod = returnValueMethod;
            }
            else
            {
                context.VoidMethod = processItemMethod;
            }
            context.HasReturnValue = hasReturnValue;
            return context;
        }

        internal int ThreadCount;

        internal DataWithStateList DataWithStates;

        internal bool NeedWaitAll;

        internal bool HasReturnValue;

        internal WaitCallback VoidMethod;

        internal ProcessorCallback ReturnValueMethod;

        private bool _breakSignal;

        private Hashtable _processResult;
        /// <summary>
        /// Keypairs of process results.
        /// </summary>
        internal Hashtable ProcessResult
        {
            get { return _processResult; }
            set { _processResult = value; }
        }
        /// <summary>
        /// Insert specified keypair into 'ProcessResult'.
        /// </summary>
        /// <param name="obj">Key</param>
        /// <param name="result">Value</param>
        internal void SetReturnValue(object obj, object result)
        {
            lock (_processResult.SyncRoot)
            {
                _processResult[obj] = result;
            }
        }
        /// <summary>
        /// Turn on the break signal, i.e.: _breakSignal is true.
        /// </summary>
        internal void SetBreakSignal()
        {
            if (NeedWaitAll) throw new NotSupportedException("Cant set 'BreakSignal' value if you have set 'NeedWaitAll'.");

            _breakSignal = true;
        }
        /// <summary>
        /// Gets a value indicating that if the thread needs break.
        /// </summary>
        internal bool NeedBreak
        {
            get
            {
                return !NeedWaitAll && _breakSignal;
            }
        }
        /// <summary>
        /// Executes the callback function.
        /// </summary>
        /// <param name="obj"></param>
        internal void Exec(object obj)
        {
            if (HasReturnValue)
            {
                SetReturnValue(obj, ReturnValueMethod(obj));
            }
            else
            {
                VoidMethod(obj);
            }
            DataWithStates.SetState(obj, ProcessState.Processed);
        }
    }
}

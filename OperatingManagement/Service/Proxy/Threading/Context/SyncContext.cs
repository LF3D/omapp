using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Threading;
using System.Diagnostics;

namespace OperatingManagement.Service
{
    /// <summary>
    /// Class representing a context of synchronization.
    /// </summary>
    public class SyncContext
    {
        #region -DoAsync-
        /// <summary>
        /// Deals with the synchronization.
        /// </summary>
        /// <param name="threadsData">Threads' Data</param>
        /// <param name="threadCount">Amount of thread</param>
        /// <param name="callback">Callback function</param>
        public static void DoAsync<T>(IList<T> threadsData, int threadCount, WaitCallback callback)
            where T : new()
        {
            DoAsync(threadsData, threadCount, callback, true);
        }


        /// <summary>
        /// Deals with the synchronization.
        /// </summary>
        /// <param name="threadsData">Threads' Data</param>
        /// <param name="threadCount">Amount of thread</param>
        /// <param name="callback">Callback function(has return value)</param>
        /// <param name="needWaitAll">Whether need to wait for the rest</param>
        /// <param name="processResult">Output[Hashtable]</param>
        public static void DoAsync<T>(IList<T> threadsData, int threadCount, ProcessorCallback callback, bool needWaitAll, out Hashtable processResult)
             where T : new()
        {
            DoAsyncPrivate(threadsData, threadCount, null, callback, needWaitAll, true, out processResult);
        }

        /// <summary>
        /// Deal with the synchronization.
        /// </summary>
        /// <param name="threadsData">Threads' Data</param>
        /// <param name="threadCount">Amount of thread</param>
        /// <param name="callback">Callback function(has return value)</param>
        /// <param name="processResult">Output[Hashtable]</param>
        public static void DoAsync<T>(IList<T> threadsData, int threadCount, ProcessorCallback callback, out Hashtable processResult)
             where T : new()
        {
            DoAsyncPrivate(threadsData, threadCount, null, callback, true, true, out processResult);
        }

        /// <summary>
        /// Deal with the synchronization.
        /// </summary>
        /// <param name="threadsData">Threads' Data</param>
        /// <param name="threadCount">Amount of thread</param>
        /// <param name="callback">Callback function</param>
        /// <param name="needWaitAll">Whether need to wait for the rest</param>
        public static void DoAsync<T>(IList<T> threadsData, int threadCount, WaitCallback callback, bool needWaitAll) 
            where T : new()
        {
            Hashtable hash;
            DoAsyncPrivate(threadsData, threadCount, callback, null, needWaitAll, false, out hash);
        }

        private static void DoAsyncPrivate<T>(IList<T> threadsData, int threadCount, WaitCallback voidCallback, ProcessorCallback returnCallback, bool needWaitAll, bool hasReturnValue, out Hashtable processResult)
             where T : new()
        {
            if (threadsData == null) throw new ArgumentNullException("threadsData");

            if (threadCount >= 500 || threadCount < 1)
            {
                throw new ArgumentOutOfRangeException("treadCount", "The amount of thread is required to between 1 and 500.");
            }

            if (threadCount > threadsData.Count) threadCount = threadsData.Count;

            IList[] colls = new ArrayList[threadCount];

            DataWithStateList dataWithStates = new DataWithStateList();
            AutoResetEvent[] evts = new AutoResetEvent[threadCount];

            for (int i = 0; i < threadCount; i++)
            {
                colls[i] = new ArrayList();
                evts[i] = new AutoResetEvent(false);
            }

            for (int i = 0; i < threadsData.Count; i++)
            {
                object obj = threadsData[i];
                int threadIndex = i % threadCount;
                colls[threadIndex].Add(obj);
                dataWithStates.Add(new DataWithState(obj, ProcessState.WaitForProcess));
            }
            AsyncContext context = AsyncContext.GetContext(threadCount, dataWithStates, needWaitAll, hasReturnValue, voidCallback, returnCallback);

            for (int i = 0; i < threadCount; i++)
            {
                ThreadPool.QueueUserWorkItem(DoPrivate, new object[] { 
                    colls[i],context,evts[i]
                });
            }

            if (needWaitAll)
            {
                WaitHandle.WaitAll(evts);
            }
            else
            {
                WaitHandle.WaitAny(evts);
                context.SetBreakSignal();
            }
            processResult = context.ProcessResult;
        }
        #endregion

        #region -Private-
        private static int _threadNo = 0;
        private static void DoPrivate(object state)
        {
            object[] objs = state as object[];

            IList datas = objs[0] as IList;
            AsyncContext context = objs[1] as AsyncContext;
            AutoResetEvent evt = objs[2] as AutoResetEvent;

            DataWithStateList objStates = context.DataWithStates;
            string threadName = string.Empty;
            if (datas != null)
            {

                for (int i = 0; i < datas.Count; i++)
                {
                    if (context.NeedBreak)
                        break;

                    object obj = datas[i];
                    if (objStates.IsWaitForData(obj, true))
                    {
                        if (context.NeedBreak)
                            break;

                        context.Exec(obj);

                    }
                }
            }

            if (context.NeedWaitAll)
            {
                while (objStates.WaitForDataCount > _threadNo)
                {
                    if (context.NeedBreak) break;

                    object obj = objStates.GetWaitForObject();
                    if (obj != null && objStates.IsWaitForData(obj, false))
                    {
                        if (context.NeedBreak)
                            break;

                        context.Exec(obj);
                    }
                }
            }

            evt.Set();
        }
        #endregion
    }
}

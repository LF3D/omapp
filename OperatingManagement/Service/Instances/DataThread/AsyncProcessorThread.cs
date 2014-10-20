using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Data;
using System.Xml;
using System.Collections;
using System.Data.SqlClient;
using System.IO;
using System.Configuration;
using System.Diagnostics;

namespace OperatingManagement.Service
{
    /// <summary>
    /// Provides a Message Sender processor in mutipal threads.
    /// </summary>
    internal class AsyncProcessorThread
    {
        #region -Constructor-
        /// <summary>
        /// Create a new instance of <see cref="OperatingManagement.Service.AsyncProcessorThread"/> class.
        /// </summary>
        public AsyncProcessorThread()
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            //log here.
        }
        #endregion

        #region -Privacy Properties-
        private static readonly
            int threadCount = ServiceConfig.Section.Setting.ThreadCount;
        private static readonly
            int sleepTime = ServiceConfig.Section.Setting.SleepTime;
        private static readonly
           int tick = ServiceConfig.Section.Setting.Tick;
        private static readonly
            int maxRetryCount = ServiceConfig.Section.Setting.MaxRetryCount;
        private bool _enable = false;
        private bool _pause = false;
        #endregion

        #region -Public Properties/Methods-
        private Thread _thread = null;
        /// <summary>
        /// Starts asynchronous operation.
        /// </summary>
        public void Start()
        {
            _enable = true;
            _thread = new Thread(new ThreadStart(delegate
            {
                if (_enable && !_pause)
                {
                    // todo..
                }
                Thread.Sleep(TimeSpan.FromSeconds(tick));
            }));
            _thread.Start();
        }
        /// <summary>
        /// Stops asynchronous operation.
        /// </summary>
        public void Stop()
        {
            if (_thread != null)
            {
                try { _thread.Abort(); }
                catch { }
            }
            _thread = null;
            _enable = false;
        }
        /// <summary>
        /// Pauses asynchronous operation.
        /// </summary>
        public void Pause()
        {
            _pause = true;
        }
        /// <summary>
        /// Continues asynchronous operation.
        /// </summary>
        public void Continue()
        {
            _pause = false;
        }
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        public void Dispose()
        {
            Stop();
        }
        #endregion
    }
}
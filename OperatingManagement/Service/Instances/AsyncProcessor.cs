using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Data;
using System.Xml;

namespace OperatingManagement.Service
{
    /// <summary>
    /// Provides a processor to deal with the asynchronous operations.
    /// </summary>
    public class AsyncProcessor
    {
        /// <summary>
        /// Privacy constructor.
        /// </summary>
        private AsyncProcessor() { }

        private AsyncProcessorThread _processor;
        private static AsyncProcessor _instance = null;
        private static Object _AsynObject = new object();
        /// <summary>
        /// Returns a instance of <see cref="OperatingManagement.Service.AsyncProcessor"/> class.
        /// </summary>
        public static AsyncProcessor Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_AsynObject)
                    {
                        if (_instance == null) {
                            _instance = new AsyncProcessor();
                        }
                    }
                }
                return _instance;
            }
        }
        /// <summary>
        /// Starts the asynchronous threads.
        /// </summary>
        public void Start()
        {
            _processor = new AsyncProcessorThread();
            _processor.Start();
        }
        /// <summary>
        /// Stops the asynchronous threads.
        /// </summary>
        public void Stop()
        {
            if (_processor != null)
            {
                _processor.Dispose();
                _processor = null;
            }
        }
        /// <summary>
        /// Pauses the asynchronous threads.
        /// </summary>
        public void Pause()
        {
            if(_processor!=null)
            _processor.Pause();
        }
        /// <summary>
        /// Continues the asynchronous threads.
        /// </summary>
        public void Continue()
        {
            if (_processor != null)
                _processor.Continue();
        }
    }
}

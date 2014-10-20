using System;
using System.Text;


namespace OperatingManagement.Service
{
    /// <summary>
    /// Declaration of CMB Exchange service.
    /// </summary>
    internal class AsyncService : IService
    {
        /// <summary>
        /// Create a new instance of <see cref="OperatingManagement.Service.AsyncService"/> class.
        /// </summary>
        public AsyncService() { }

        #region -IService Members-

        public void Start()
        {
            AsyncProcessor.Instance.Start();
        }

        public void Stop()
        {
            AsyncProcessor.Instance.Stop();
        }

        public void Pause()
        {
            AsyncProcessor.Instance.Pause();
        }

        public void Continue()
        {
            AsyncProcessor.Instance.Continue();
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace OperatingManagement.Service
{
    /// <summary>
    /// Declares a contract for service.
    /// </summary>
    public interface IService
    {
        /// <summary>
        /// Starts Windows/Console service.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops Windows/Console service.
        /// </summary>
        void Stop();

        /// <summary>
        /// Pauses Windows/Console service.
        /// </summary>
        void Pause();

        /// <summary>
        /// Resumes Windows/Console service.
        /// </summary>
        void Continue();
    }
}

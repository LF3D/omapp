using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;

namespace OperatingManagement.Service
{
    /// <summary>
    /// Represents the service by Windows way.
    /// </summary>
    partial class WindowsService : ServiceBase
    {
        private IService _service;

        /// <summary>
        /// Create a new instance of <see cref="OperatingManagement.Service.WindowsService"/> class.
        /// </summary>
        /// <param name="service">Current Service</param>
        public WindowsService(IService service) : base()
        {
            InitializeComponent();
            this._service = service;
        }

        protected override void OnStart(string[] args)
        {
            _service.Start();
        }

        protected override void OnStop()
        {
            _service.Stop();
        }

        protected override void OnPause()
        {
            _service.Pause();
        }

        protected override void OnContinue()
        {
            _service.Continue();
        }
    }
}

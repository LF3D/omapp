using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace OperatingManagement.Service
{
    /// <summary>
    /// Represents the configured service settings from local configuration file.
    /// </summary>
    internal static class ServiceConfig
    {
        private static ServiceSection _section;

        /// <summary>
        /// Gets the configured service section.
        /// </summary>
        public static ServiceSection Section
        {
            get { return _section; }
        }

        static ServiceConfig()
        {
            _section = (ServiceSection)ConfigurationManager.GetSection("serviceProxy");
        }
    }
}

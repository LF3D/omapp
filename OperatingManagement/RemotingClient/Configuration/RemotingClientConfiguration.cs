using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace OperatingManagement.RemotingClient.Configuration
{
    /// <summary>
    /// Represents the configuration section from the specific configured file.
    /// </summary>
    internal static class RemotingClientConfiguration
    {

        static RemotingClientConfiguration()
        {
            _section = (RemotingClientSection)ConfigurationManager.GetSection("remotingObjects");
        }

        private static RemotingClientSection _section;

        /// <summary>
        /// Gets the instance of <see cref="RemotingClientSection"/> class.
        /// </summary>
        public static RemotingClientSection Section
        {
            get { return _section; }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace OperatingManagement.RemotingServer.Configuration
{
    /// <summary>
    /// Represents a section of RemotingServer setting from configured file.
    /// </summary>
    internal class RemotingServerSection : ConfigurationSection
    {
        /// <summary>
        /// Gets the port of RemotingServer.
        /// </summary>
        [ConfigurationProperty("port", IsRequired = true)]
        public int Port
        {
            get { return (int)this["port"]; }
        }

        /// <summary>
        /// Gets a value indicating that whether auto-start service.
        /// </summary>
        [ConfigurationProperty("isAuto", IsRequired = false, DefaultValue = true)]
        public bool IsAuto
        {
            get { return (bool)this["isAuto"]; }
        }

        /// <summary>
        /// Gets the collection of <see cref="RemotingServerElement"/> objects.
        /// </summary>
        [ConfigurationProperty("services")]
        public RemotingServerElementCollection Services
        {
            get { return (RemotingServerElementCollection)this["services"]; }
        }
    }
}

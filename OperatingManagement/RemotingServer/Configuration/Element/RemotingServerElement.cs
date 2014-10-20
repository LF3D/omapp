using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Runtime.Remoting;

namespace OperatingManagement.RemotingServer.Configuration
{
    /// <summary>
    /// Represents a configuration element of RemotingServer within a configuration file.
    /// </summary>
    internal class RemotingServerElement : ConfigurationElement
    {
        /// <summary>
        /// Gets the remoting uri of RemotingObject.
        /// </summary>
        [ConfigurationProperty("objectUri", IsRequired = true, IsKey = true)]
        public string ObjectUri
        {
            get { return (string)this["objectUri"]; }
        }

        /// <summary>
        /// Gets the assembly type of RemotingObject.
        /// </summary>
        [ConfigurationProperty("type", IsRequired = true)]
        public string Type
        {
            get { return (string)this["type"]; }
        }

        /// <summary>
        /// Gets the remoting mode, including: Singleton, SingleCall.
        /// </summary>
        [ConfigurationProperty("mode", IsRequired = true)]
        public string Mode
        {
            get { return (string)this["mode"]; }
        }
    }
}

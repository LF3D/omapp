using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace OperatingManagement.RemotingClient.Configuration
{
    /// <summary>
    /// Represents a configuration element of RemotingObject within a configuration file.
    /// </summary>
    internal class RemotingObjectElement : ConfigurationElement
    {
        /// <summary>
        /// Gets the assembly type of RemotingObject.
        /// </summary>
        [ConfigurationProperty("type", IsRequired = true, IsKey = true)]
        public string Type
        {
            get { return (string)this["type"]; }
        }

        /// <summary>
        /// Gets the Remoting Url of RemotingObject.
        /// </summary>
        [ConfigurationProperty("url", IsRequired = true)]
        public string Url
        {
            get { return (string)this["url"]; }
        }
    }
}

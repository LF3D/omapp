using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace OperatingManagement.RemotingClient.Configuration
{
    /// <summary>
    /// Represents a section of RemotingClient setting from configured file.
    /// </summary>
    internal class RemotingClientSection : ConfigurationSection
    {
        /// <summary>
        /// Gets the collection of <see cref="RemotingObjectElement"/> objects.
        /// </summary>
        [ConfigurationProperty("objects")]
        public RemotingObjectElementCollection Objects
        {
            get { return (RemotingObjectElementCollection)this["objects"]; }
        }
    }
}

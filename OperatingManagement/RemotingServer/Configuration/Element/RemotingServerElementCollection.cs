using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace OperatingManagement.RemotingServer.Configuration
{
    /// <summary>
    /// Represents a configuration element containing a collection of <see cref="RemotingServerElement"/> elements.
    /// </summary>
    internal class RemotingServerElementCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// Creates a new <see cref="RemotingServerElement"/>.
        /// </summary>
        /// <returns>A new instance of <see cref="RemotingServerElement"/> class.</returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new RemotingServerElement();
        }

        /// <summary>
        /// Gets the element key for a specified <see cref="RemotingServerElement"/>.
        /// </summary>
        /// <param name="element">The <see cref="RemotingServerElement"/> to return the key for.</param>
        /// <returns>An System.Object that acts as the key for the specified <see cref="RemotingServerElement"/>.</returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((RemotingServerElement)element).ObjectUri;
        }
    }
}

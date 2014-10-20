using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace OperatingManagement.RemotingClient.Configuration
{
    /// <summary>
    /// Represents a configuration element containing a collection of <see cref="RemotingObjectElement"/> elements.
    /// </summary>
    internal class RemotingObjectElementCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// Creates a new <see cref="RemotingObjectElement"/>.
        /// </summary>
        /// <returns>A new instance of <see cref="RemotingObjectElement"/> class.</returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new RemotingObjectElement();
        }

        /// <summary>
        /// Gets the element key for a specified <see cref="RemotingObjectElement"/>.
        /// </summary>
        /// <param name="element">The <see cref="RemotingObjectElement"/> to return the key for.</param>
        /// <returns>An System.Object that acts as the key for the specified <see cref="RemotingObjectElement"/>.</returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((RemotingObjectElement)element).Type;
        }
        /// <summary>
        /// Gets the <see cref="RemotingObjectElement"/> object by type.
        /// </summary>
        /// <param name="type">The specific type(String) of <see cref="RemotingObjectElement"/> object.</param>
        /// <returns>A <see cref="RemotingObjectElement"/>.</returns>
        public new RemotingObjectElement this[string type]
        {
            get { return (RemotingObjectElement)BaseGet(type); }
        }
    }
}

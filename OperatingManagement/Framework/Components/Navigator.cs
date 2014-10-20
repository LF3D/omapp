using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace OperatingManagement.Framework.Components
{
    /// <summary>
    /// Provides a sealed class for page navigator.
    /// </summary>
    [XmlRoot("navigator")]
    public class Navigator
    {
        /// <summary>
        /// Gets/Sets the navigator items.
        /// </summary>
        [XmlArrayItem("item")]
        [XmlArray("items")]
        public NavigatorItem[] Items { get; set; }
    }
    /// <summary>
    /// Provides a sealed class for page navigator items.
    /// </summary>
    [XmlRoot("item")]
    public class NavigatorItem
    {
        /// <summary>
        /// Gets/Sets the identification of item.
        /// </summary>
        [XmlAttribute("id")]
        public string Id { get; set; }
        /// <summary>
        /// Gets/Sets the title of item.
        /// </summary>
        [XmlAttribute("title")]
        public string Title { get; set; }
        /// <summary>
        /// Gets/sets the href of item.
        /// </summary>
        [XmlAttribute("href")]
        public string Href { get; set; }
    }
}

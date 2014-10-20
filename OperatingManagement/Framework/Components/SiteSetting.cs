using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace OperatingManagement.Framework.Components
{
    /// <summary>
    /// Provides a sealed class for site settings.
    /// </summary>
    [XmlRoot("siteSetting")]
    public class SiteSetting
    {
        /// <summary>
        /// Gets/Sets the default site name.
        /// </summary>
        [XmlElement("siteName")]
        public string SiteName { get; set; }
        /// <summary>
        /// Gets/Sets the size of pagination.
        /// </summary>
        [XmlElement("pageSize")]
        public int PageSize { get; set; }
        /// <summary>
        /// Gets/Sets the default Date format string.
        /// </summary>
        [XmlElement("dateFormat")]
        public string DateFormat { get; set; }
        /// <summary>
        /// Gets/Sets the default Time format string.
        /// </summary>
        [XmlElement("timeFormat")]
        public string TimeFormat { get; set; }
        /// <summary>
        /// Gets/Sets the default DateTime format string.
        /// </summary>
        [XmlElement("dateTimeFormat")]
        public string DateTimeFormat { get; set; }
        /// <summary>
        /// Gets/Sets the default copy right format string.
        /// </summary>
        [XmlElement("copyRight")]
        public string CopyRight { get; set; }
    }
}

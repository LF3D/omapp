using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace OperatingManagement.Service
{
    /// <summary>
    /// Reresents the settings of current service.
    /// </summary>
    internal class SettingElement : ConfigurationElement
    {
        /// <summary>
        /// Gets the name of service
        /// </summary>
        [ConfigurationProperty("serviceName", IsRequired = true)]
        public string ServiceName
        {
            get { return this["serviceName"].ToString(); }
        }

        /// <summary>
        /// Gets the display name of service.
        /// </summary>
        [ConfigurationProperty("displayName", IsRequired = true)]
        public string DisplayName
        {
            get { return this["displayName"].ToString(); }
        }

        /// <summary>
        /// Gets the description of service.
        /// </summary>
        [ConfigurationProperty("description", IsRequired = true)]
        public string Description
        {
            get { return this["description"].ToString(); }
        }
        /// <summary>
        /// Gets the interval before next thread starts(second).
        /// </summary>
        [ConfigurationProperty("sleepTime", IsRequired = true)]
        public int SleepTime
        {
            get { return int.Parse(this["sleepTime"].ToString()); }
        }
        /// <summary>
        /// Gets the interval of timer(second).
        /// </summary>
        [ConfigurationProperty("tick", IsRequired = true)]
        public int Tick
        {
            get { return int.Parse(this["tick"].ToString()); }
        }
        /// <summary>
        /// Gets the amount of all the thread.
        /// </summary>
        [ConfigurationProperty("threadCount", IsRequired = true)]
        public int ThreadCount
        {
            get { return int.Parse(this["threadCount"].ToString()); }
        }
        /// <summary>
        /// Gets the maximize retry count if operation was failed.
        /// </summary>
        [ConfigurationProperty("maxRetryCount", IsRequired = true)]
        public int MaxRetryCount
        {
            get { return int.Parse(this["maxRetryCount"].ToString()); }
        }
    }
}

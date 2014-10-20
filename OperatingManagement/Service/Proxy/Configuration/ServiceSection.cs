using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace OperatingManagement.Service
{

    /// <summary>
    /// Represents the configured service section from local configuration file.
    /// </summary>
    internal class ServiceSection : ConfigurationSection
    {
        /// <summary>
        /// Gets the run mode of service, i.e.: Windows or Console.
        /// </summary>
        [ConfigurationProperty("runMode", IsRequired = true)]
        public string RunMode
        {
            get { return (string)this["runMode"]; }
        }

        /// <summary>
        /// Gets the settings of current service, including ServiceName, DisplayName, Description and so on... 
        /// </summary>
        [ConfigurationProperty("setting", IsRequired = true)]
        public SettingElement Setting
        {
            get { return (SettingElement)this["setting"]; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceProcess;
using System.Reflection;
using System.Xml;
using System.Configuration;

namespace OperatingManagement.Service
{
    /// <summary>
    /// Implemention of both Windows Service and Console Service.
    /// </summary>
    public static class ServiceProxy
    {
        private const string AgrsConsole = "/c";
        /// <summary>
        /// Runs the specified service.
        /// </summary>
        /// <param name="service">Specified service</param>
        /// <param name="args">Arguments</param>
        public static void Run(IService service, string[] args)
        {
            if (service == null)
                throw new ArgumentNullException("service");


            if (args != null && args.Length > 0)
            {
                if (args.Length == 1 && args[0] == AgrsConsole)
                    ConsoleService.Run(service);
                else
                    Console.WriteLine("Bad arguments!");
                return;
            }

            string runMode = ServiceConfig.Section.RunMode;

            if (runMode.Equals("windows", StringComparison.InvariantCultureIgnoreCase))
                ServiceBase.Run(new WindowsService(service));
            else if (runMode.Equals("console", StringComparison.InvariantCultureIgnoreCase))
                ConsoleService.Run(service);
            else
                throw new ConfigurationErrorsException("Unknown configured runMode：" + runMode);
        }

        /// <summary>
        /// Initialize the installer of service.
        /// </summary>
        /// <param name="installer">Service Installer</param>
        public static void InitializeInstaller(ServiceInstaller installer)
        {
            if (installer == null)
                throw new ArgumentNullException("installer");

            /* CAUTION:
             * Load the configured infomation in a 
             * special way instead of 'ServiceConfig'.
             * */
            XmlDocument doc = new XmlDocument();
            doc.Load(Assembly.GetCallingAssembly().Location + ".config");
            XmlNode node = doc.SelectSingleNode("/configuration/serviceProxy/setting");
            installer.ServiceName = node.Attributes["serviceName"].Value;
            installer.DisplayName = node.Attributes["displayName"].Value;
            installer.Description = node.Attributes["description"].Value;
        }
    }
}

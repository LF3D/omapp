using System;
using System.Collections.Generic;
using System.Text;

namespace OperatingManagement.Service
{
    /// <summary>
    /// Represents the service by Console way.
    /// </summary>
    internal class ConsoleService
    {
        private const string _commandStop        = "#s";
        private const string _commandPause       = "#p";
        private const string _commandContinue    = "#c";

        /// <summary>
        /// Runs the service immediately.
        /// </summary>
        /// <param name="service">Current Service</param>
        public static void Run(IService service)
        {
            SettingElement setting = ServiceConfig.Section.Setting;
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine("Service Name：" + setting.ServiceName);
            Console.WriteLine("Display Name：" + setting.DisplayName);
            Console.WriteLine("Description： " + setting.Description);
            Console.WriteLine();
            Console.WriteLine("Commands：");
            Console.WriteLine("Stop：  " + _commandStop);
            Console.WriteLine("Pause： " + _commandPause);
            Console.WriteLine("Resume：" + _commandContinue);
            Console.WriteLine("--------------------------------------------");

            service.Start();
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine("The service has started.");

            Console.ForegroundColor = ConsoleColor.DarkGray;
            string command = Console.ReadLine();
            while (ProcessCommand(command, service))
            {
                command = Console.ReadLine();
            }
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
        /// <summary>
        /// Deals with the differently commands.
        /// </summary>
        /// <param name="command">Command</param>
        /// <param name="service">Current Service</param>
        /// <returns></returns>
        private static bool ProcessCommand(string command, IService service)
        {
            if (command == _commandStop)
            {
                service.Stop();
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine("The service has stopped.");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                return false;
            }
            else if (command == _commandPause)
            {
                service.Pause();
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine("The service has paused.");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                return true;
            }
            else if (command == _commandContinue)
            {
                service.Continue();
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine("The service has continued.");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                return true;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine("Bad command!");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                return true;
            }
        }
    }
}

using System.Collections.Generic;
using System.ServiceProcess;
using System.Text;

namespace OperatingManagement.Service
{
    static class Program
    {
        /// <summary>
        /// Main entry of application.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            ServiceProxy.Run(new AsyncService(), args);
        }
    }
}
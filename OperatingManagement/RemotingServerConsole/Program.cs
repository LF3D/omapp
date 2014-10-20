using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RemotingServerConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starts Remoting Server...");
            OperatingManagement.RemotingServer.RemotingServerManager.Instance.Start();
            Console.WriteLine("Remoting Server was started.");
            Console.WriteLine();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}

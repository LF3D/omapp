using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OperatingManagement.RemotingObjectInterface;
using OperatingManagement.RemotingHelper;
using System.Net;

namespace RemotingClientConsole
{
    class Program
    {
        private static string strIP = "";
        private static string strPort = "2005";

        [STAThread]
        static void Main(string[] args)
        {
            IPAddress[] addrList = Dns.GetHostEntry(Environment.MachineName).AddressList;
            for (int i = 0; i < addrList.Length; i++)
            {
                if (!addrList[i].IsIPv6LinkLocal)
                {
                    strIP = addrList[i].ToString();
                    break;
                }
            }

            if (strIP == string.Empty)
                return;
            string strResult = "";
            strResult = validateuser();
            Console.WriteLine(strResult);

            strResult = GetUsersByRoleid(10);
            Console.WriteLine(strResult);

            strResult = GetRoles();
            Console.WriteLine(strResult);

            strResult = GetAllUsers();
            Console.WriteLine(strResult);

            strResult = GetUser(10);
            Console.WriteLine(strResult);

            Console.WriteLine("Convert result ..." + strResult);
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private static string validateuser()
        {
            IAccount account = OperatingManagement.RemotingClient.RemotingActivator.GetObject<IAccount>(strIP, strPort);
            string pwd = PasswordEncryptHelper.EncryptPasswordBySalt("w12345", "MSFTJOM@web#");

            string xml = account.ValidateUser("wfuser3", pwd);
            //xml to entity
            AuthStatus status = EntityConvert.ToAuthStatus(xml);
            return xml;
        }

        private static string GetRoles()
        {
            IAccount account = OperatingManagement.RemotingClient.RemotingActivator.GetObject<IAccount>(strIP, strPort);
            string xml = account.GetAllRoles();
            //xml to entity
            UMResult oResult = EntityConvert.ToResult(xml);
            return xml;
        }

        private static string GetUsersByRoleid(int rid)
        {
            IAccount account = OperatingManagement.RemotingClient.RemotingActivator.GetObject<IAccount>(strIP, strPort);

            string xml = account.GetUsersByRoleID(rid);
            //xml to entity
            UMResult oResult = EntityConvert.ToResult(xml);
            return xml;
        }

        private static string GetAllUsers()
        {
            IAccount account = OperatingManagement.RemotingClient.RemotingActivator.GetObject<IAccount>(strIP, strPort);

            string xml = account.GetAllUsers();
            //xml to entity
            UMResult oResult = EntityConvert.ToResult(xml);
            return xml;
        }

        private static string GetUser(int id)
        {
            IAccount account = OperatingManagement.RemotingClient.RemotingActivator.GetObject<IAccount>(strIP, strPort);
            string xml = account.GetUserByID(id);
            //xml to entity
            UMResult oResult = EntityConvert.ToResult(xml);
            return xml;
        }
    }
}

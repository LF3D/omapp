using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace ServicesKernel.File
{
    public class FileSenderClientAgent
    {
        public static T GetObject<T>()
        {
            Type type = typeof(T);
            string strSenderURL = ConfigurationManager.AppSettings["FileServerPath"];
            if (!strSenderURL.Equals(string.Empty))
                return (T)Activator.GetObject(type, strSenderURL);
            else
                return default(T);
        }
    }
}

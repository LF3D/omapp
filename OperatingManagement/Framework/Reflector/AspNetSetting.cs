using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using OperatingManagement.Framework.Core;
using OperatingManagement.Framework.Cache;

namespace OperatingManagement.Framework.Reflector
{
    /// <summary>
    /// Load the settings from XML config file and insert it into cache.
    /// </summary>
    public class AspNetSetting
    {
        private AspNetSetting() { }
        /// <summary>
        /// Deserialize any Type(T) entity from custom XML file under App_Data folder.
        /// </summary>
        /// <typeparam name="T">Entity type.</typeparam>
        /// <param name="fileName">File name without extension.</param>
        /// <returns></returns>
        public static T Load<T>(string fileName)
            where T : class
        {
            if (string.IsNullOrEmpty(fileName))
                fileName = typeof(T).Name;
            string cacheKey = "Settings/" + fileName;
            if (AspNetCache.Instance.Get(cacheKey) != null)
                return AspNetCache.Instance.Get(cacheKey) as T;
            string fullFileName = GlobalSettings.MapPath(string.Format(AspNetConfig.Config["settingPattern"].ToString(), fileName));
            FileDependency fd = new FileDependency(fullFileName);

            XmlSerializer xmlSerlz = new XmlSerializer(typeof(T));
            using (FileStream fs = new FileStream(fullFileName, FileMode.Open, FileAccess.Read))
            {
                T t = (T)xmlSerlz.Deserialize(fs);
                fs.Flush();
                AspNetCache.Instance.Insert(cacheKey, t, fd);
                return t;
            }
        }
        /// <summary>
        /// Deserialize any Type(T) entity from custom XML file under App_Data folder.
        /// </summary>
        /// <typeparam name="T">Entity type.</typeparam>
        /// <returns></returns>
        public static T Load<T>()
            where T : class
        {
            return Load<T>(string.Empty);
        }
    }
}

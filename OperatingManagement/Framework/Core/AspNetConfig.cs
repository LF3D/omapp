using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Web.Configuration;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using OperatingManagement.Framework.Cache;

namespace OperatingManagement.Framework.Core
{
    /// <summary>
    ///Global configuration(load xml data from privacy.config)
    /// </summary>
    public class AspNetConfig
    {
        #region -Properties-
        private XmlDocument XmlDoc = null;
        private static object configLocker = new object();
        private static AspNetConfig _config = null;
        #endregion

        #region -Construtor-
        /// <summary>
        /// Create a new instance of <see cref="OperatingManagement.Framework.Core.AspNetConfig"/> class.
        /// </summary>
        /// <param name="doc">Xml Document</param>
        public AspNetConfig(XmlDocument doc)
        {
            XmlDoc = doc;
            LoadValuesFromConfigurationXml();
        }
        /// <summary>
        /// Create a new instance of <see cref="OperatingManagement.Framework.Core.AspNetConfig"/> class.
        /// </summary>
        /// <param name="nodePath">Path</param>
        /// <returns></returns>
        public XmlNode GetConfigSection(string nodePath)
        {
            return XmlDoc.SelectSingleNode(nodePath);
        }
        /// <summary>
        /// Gets the instance of <see cref="OperatingManagement.Framework.Core.AspNetConfig"/> class.
        /// </summary>
        public static AspNetConfig Config
        {
            get
            {
                if (_config == null)
                {
                    lock (configLocker)
                    {
                        if (_config == null)
                        {
                            string path = AspNetConfig.ConfigFilePath;
                            if (AspNetCache.Instance.Get(CachingKey) != null)
                            {
                                _config = AspNetCache.Instance.Get(CachingKey) as AspNetConfig;
                            }
                            else
                            {
                                XmlDocument doc = new XmlDocument();
                                doc.Load(path);
                                _config = new AspNetConfig(doc);
                                FileDependency fd = new FileDependency(path);
                                AspNetCache.Instance.Insert(CachingKey, _config, fd);
                            }
                        }
                    }
                }
                return _config;
            }
        }
        private static string CachingKey = "Config/Setting";
        private void LoadValuesFromConfigurationXml()
        {
            XmlNode node = GetConfigSection("privacy/core");
            XmlAttributeCollection attributeCollection = node.Attributes;
            for (int i = 0; i < attributeCollection.Count; i++)
            {
                attr.Add(attributeCollection[i].Name, attributeCollection[i].Value);
            }
        }
        #endregion

        #region -Indexer-
        private Dictionary<string, object> attr = new Dictionary<string, object>();
        /// <summary>
        /// Gets the value from congiguration settings by name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public object this[string name]
        {
            get
            {
                if (attr.ContainsKey(name))
                    return attr[name];
                else
                    return string.Empty;
            }
        }
        #endregion

        #region -Config path-
        /// <summary>
        /// Gets the file path of configuration
        /// </summary>
        public static string ConfigFilePath
        {
            get
            {
                return GlobalSettings.MapPath("~/privacy.config");
            }
        }
        #endregion
    }
}

using System;
using System.Xml;
using System.Collections.Generic;

namespace OperatingManagement.Framework.Cache
{
    /// <summary>
    /// Cache management, using XML to wrap KEYWORD and CACHING OBJECTS.
    /// </summary>
    public class AspNetCache
    {
        #region -Private Member-
        private XmlElement objectXmlMap;
        private static ICacheStrategy strategy;
        private static AspNetCache instance;
        private static object lockObject = new object();
        private XmlDocument rootXml = new XmlDocument();
        #endregion

        #region -Constructor-
        private void Initialize()
        {
            rootXml.RemoveAll();
            objectXmlMap = rootXml.CreateElement("Cache");
            rootXml.AppendChild(objectXmlMap);
        }

        private AspNetCache()
        {
            strategy = new DefaultCacheStrategy();
            Initialize();
        }
        /// <summary>
        /// Gets the instance of <see cref="OperatingManagement.Framework.Cache.AspNetCache"/> class.
        /// </summary>
        public static AspNetCache Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObject)
                    {
                        if (instance == null)
                            instance = new AspNetCache();
                    }
                }
                return instance;
            }
        }
        #endregion

        #region -CacheFactor-
        /// <summary>
        /// Resets the FACTOR for AspNetCache.
        /// <remarks>
        /// FACTOR is a value indicating the basic time factor, it is minutes, default(5minutes).
        /// </remarks>
        /// </summary>
        /// <param name="factor">default(5minutes)</param>
        public void ResetFactor(int factor)
        {
            strategy.ResetFactor(factor);
        }
        #endregion

        #region -Insert-
        /// <summary>
        /// Insert caching object to the CACHE CONTEXT.
        /// </summary>
        /// <param name="key">Caching Key</param>
        /// <param name="obj">Object</param>
        public virtual void Insert(string key, object obj)
        {
            strategy.Insert(CreateNodeMap(key), obj);
        }
        /// <summary>
        /// Insert caching object to the CACHE CONTEXT.
        /// </summary>
        /// <param name="key">Caching Key</param>
        /// <param name="obj">Object</param>
        /// <param name="multiple">Caching timespan, it is multiple*factor minutes, factor was 5 minutes as default.</param>
        public virtual void Insert(string key, object obj, double multiple)
        {
            strategy.Insert(CreateNodeMap(key), obj, multiple);
        }
        /// <summary>
        /// Insert caching object to the CACHE CONTEXT.
        /// </summary>
        /// <param name="key">Caching Key</param>
        /// <param name="obj">Object</param>
        /// <param name="deps">Cache item expiration</param>
        public virtual void Insert(string key, object obj, params ICacheItemExpiration[] deps)
        {
            strategy.Insert(CreateNodeMap(key), obj, deps);
        }
        /// <summary>
        /// Insert caching object to the CACHE CONTEXT.
        /// </summary>
        /// <param name="key">Caching Key</param>
        /// <param name="obj">Object</param>
        /// <param name="multiple">Caching timespan, it is multiple*factor minutes, factor was 5 minutes as default.</param>
        /// <param name="priority">Cache item priority</param>
        public virtual void Insert(string key, object obj, double multiple, CacheItemPriority priority)
        {
            strategy.Insert(CreateNodeMap(key), obj, multiple, priority);
        }

        /// <summary>
        /// Insert caching object to the CACHE CONTEXT.
        /// </summary>
        /// <param name="key">Caching Key</param>
        /// <param name="obj">Object</param>
        /// <param name="priority">Cache item priority</param>
        /// <param name="deps">Cache item expiration</param>
        public virtual void Insert(string key, object obj, CacheItemPriority prioprity, params ICacheItemExpiration[] deps)
        {
            strategy.Insert(CreateNodeMap(key), obj, prioprity, deps);
        }

        /// <summary>
        /// Insert caching object to the CACHE CONTEXT.
        /// <remarks>NERVER EXPIRATION</remarks>
        /// </summary>
        /// <param name="key">Caching Key</param>
        /// <param name="obj">Object</param>
        public virtual void Max(string key, object obj)
        {
            strategy.Max(CreateNodeMap(key), obj);
        }

        /// <summary>
        /// Insert caching object to the CACHE CONTEXT.
        /// <remarks>NERVER EXPIRATION</remarks>
        /// </summary>
        /// <param name="key">Caching Key</param>
        /// <param name="obj">Object</param>
        /// <param name="deps">Cache item expiration</param>
        public virtual void Max(string key, object obj, params ICacheItemExpiration[] deps)
        {
            strategy.Max(CreateNodeMap(key), obj, deps);
        }
        #endregion

        #region -Get-
        /// <summary>
        /// Gets the caching object by Caching Key.
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>Value</returns>
        public virtual object Get(string key)
        {
            object o = null;
            XmlNode node = objectXmlMap.SelectSingleNode(PrepareXpath(key));
            if (node != null)
            {
                if (node.Attributes["objectID"] != null)
                {
                    string objectId = node.Attributes["objectId"].Value;
                    o = strategy.Get(objectId);
                }
            }
            return o;
        }

        /// <summary>
        /// Gets the amount of caching objects by Caching Key.
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns></returns>
        public virtual int GetCount(string key)
        {
            XmlNode group = objectXmlMap.SelectSingleNode(PrepareXpath(key));
            if (group != null)
            {
                XmlNodeList results = group.SelectNodes(PrepareXpath(key) + "//*[@objectId]");
                return results.Count == 0 ? 1 : results.Count;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Gets the caching objects by Caching Key.
        /// </summary>
        /// <param name="key">Key</param>
        /// <returns>All objects</returns>
        public virtual object[] GetList(string key)
        {
            XmlNode group = objectXmlMap.SelectSingleNode(PrepareXpath(key));
            if (group != null)
            {
                XmlNodeList results = group.SelectNodes(PrepareXpath(key) + "/*[@objectId]");
                List<object> objects = new List<object>();
                string objectId = null;
                foreach (XmlNode result in results)
                {
                    objectId = result.Attributes["objectId"].Value;
                    objects.Add(strategy.Get(objectId));
                }
                return objects.ToArray();
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region -Remove-
        /// <summary>
        /// Removing the caching objects from CACHE CONTEXT by Caching Key.
        /// </summary>
        /// <param name="key">Key</param>
        public virtual void Remove(string key)
        {
            XmlNode result = objectXmlMap.SelectSingleNode(PrepareXpath(key));
            Remove(result);
        }

        /// <summary>
        /// Removing the caching objects from CACHE CONTEXT by Caching Key.
        /// </summary>
        /// <param name="result">Caching XmlNode</param>
        public virtual void Remove(XmlNode result)
        {
            if (result != null)
            {
                if (result.HasChildNodes)
                {
                    XmlNodeList objects = result.SelectNodes("*[@objectId]");
                    string objectId = "";
                    foreach (XmlNode node in objects)
                    {
                        objectId = node.Attributes["objectId"].Value;
                        node.ParentNode.RemoveChild(node);
                        strategy.Remove(objectId);
                    }
                }
                else
                {
                    if (result.Attributes["objectId"] != null)
                    {
                        string objectId = result.Attributes["objectId"].Value;
                        result.ParentNode.RemoveChild(result);
                        strategy.Remove(objectId);
                    }
                }
            }
        }

        /// <summary>
        /// Removing the caching objects from CACHE CONTEXT by X-Path
        /// </summary>
        /// <param name="key">X-Path key</param>
        public virtual void RemoveByPattern(string key)
        {
            string xpath = key;
            if (key.StartsWith("/"))
                xpath = "/Cache" + key;
            XmlNodeList results = objectXmlMap.SelectNodes(xpath);
            if (results != null)
            {
                foreach (XmlNode result in results)
                {
                    Remove(result);
                }
            }
        }
        #endregion

        #region -Clear-
        /// <summary>
        /// Clean up all the caching objects.
        /// </summary>
        public virtual void Clear()
        {
            Initialize();
            strategy.Clear();
        }
        #endregion

        #region -Private Method-
        private string CreateNodeMap(string key)
        {
            string newXpath = PrepareXpath(key);
            XmlNode node = objectXmlMap.SelectSingleNode(newXpath);
            if (node != null)
            {
                if (node.Attributes["objectId"] == null)
                {
                    string objectId = System.Guid.NewGuid().ToString();
                    XmlAttribute objectAttribute = objectXmlMap.OwnerDocument.CreateAttribute("objectId");
                    objectAttribute.Value = objectId;
                    node.Attributes.Append(objectAttribute);
                    return objectId;
                }
                else
                {
                    return node.Attributes["objectId"].Value;
                }
            }
            else
            {
                int separator = newXpath.LastIndexOf("/");
                string group = newXpath.Substring(0, separator);
                string element = newXpath.Substring(separator + 1);
                XmlNode groupNode = objectXmlMap.SelectSingleNode(group);
                if (groupNode == null)
                {
                    lock (lockObject)
                    {
                        groupNode = CreateNode(group);
                    }
                }
                string objectId = System.Guid.NewGuid().ToString();
                XmlElement objectElement = objectXmlMap.OwnerDocument.CreateElement(element);
                XmlAttribute objectAttribute = objectXmlMap.OwnerDocument.CreateAttribute("objectId");
                objectAttribute.Value = objectId;
                objectElement.Attributes.Append(objectAttribute);
                groupNode.AppendChild(objectElement);
                return objectId;
            }
        }

        private XmlNode CreateNode(string xpath)
        {
            string[] xpathArray = xpath.Split('/');
            string root = "";
            XmlNode parentNode = (XmlNode)objectXmlMap;
            for (int i = 1; i < xpathArray.Length; i++)
            {
                XmlNode node = objectXmlMap.SelectSingleNode(root + "/" + xpathArray[i]);
                if (node == null)
                {
                    XmlElement newElement = objectXmlMap.OwnerDocument.CreateElement(xpathArray[i]);
                    parentNode.AppendChild(newElement);
                }
                root = root + "/" + xpathArray[i];
                parentNode = objectXmlMap.SelectSingleNode(root);
            }
            return parentNode;
        }

        private string PrepareXpath(string xpath)
        {
            string[] xpathArray = xpath.Split('/');
            xpath = "/Cache";
            foreach (string s in xpathArray)
            {
                if (s != "")
                {
                    xpath = xpath + "/" + s;
                }
            }
            return xpath;
        }
        #endregion

        #region -Get DOM-
        /// <summary>
        /// Gets the caching xml tree from CACHING CONTEXT.
        /// </summary>
        /// <returns></returns>
        public System.IO.Stream GetCacheKeyXml()
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            rootXml.Save(ms);
            ms.Position = 0;
            return ms;
        }
        #endregion
    }
}

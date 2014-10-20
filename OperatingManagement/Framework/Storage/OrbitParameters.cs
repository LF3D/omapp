using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OperatingManagement.Framework.Components;
using OperatingManagement.Framework.Cache;
using System.Xml.Linq;
using OperatingManagement.Framework.Core;

namespace OperatingManagement.Framework.Storage
{
    /// <summary>
    /// Orbit parameters collections.
    /// </summary>
    public class OrbitParameters
    {
        const string _CacheKey = "Setting/OrbitParameters";
        /// <summary>
        /// Read orbit parameters data from xml file
        /// </summary>
        /// <returns></returns>
        public static List<OrbitParameter> ReadParameters()
        {
            if (AspNetCache.Instance.Get(_CacheKey) != null)
                return AspNetCache.Instance.Get(_CacheKey) as List<OrbitParameter>;
            string fullFileName = GlobalSettings.MapPath("~/app_data/orbitParameters.xml");
            FileDependency fd = new FileDependency(fullFileName);
            XElement xe = XElement.Load(fullFileName);
            try
            {
                List<OrbitParameter> items = (from q in xe.Elements("orbit")
                                              select new OrbitParameter()
                                              {
                                                  Id = q.Attribute("id").Value,
                                                  Value = q.Value
                                              }).ToList();
                AspNetCache.Instance.Insert(_CacheKey, items, fd);
                return items;
            }
            catch
            {
                return null;
            }
        }
    }
}

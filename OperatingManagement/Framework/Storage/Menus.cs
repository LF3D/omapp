using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OperatingManagement.Framework.Components;
using OperatingManagement.Framework.Cache;
using System.Xml.Linq;

namespace OperatingManagement.Framework.Storage
{
    /// <summary>
    /// Menus collections.
    /// </summary>
    public class Menus
    {
        /// <summary>
        /// Read menu data from xml file
        /// </summary>
        /// <param name="fullFileName">Full FileName</param>
        /// <returns></returns>
        public static List<CstMenu> ReadMenu(string fullFileName, string cachingKey)
        {
            if (AspNetCache.Instance.Get(cachingKey) != null)
                return AspNetCache.Instance.Get(cachingKey) as List<CstMenu>;

            FileDependency fd = new FileDependency(fullFileName);
            XElement xe = XElement.Load(fullFileName);
            try
            {
                List<CstMenu> menus = (from q in xe.Elements("menu")
                                        select new CstMenu()
                                       {
                                           Title = q.Attribute("title").Value,
                                           MenuItems = (
                                               from q1 in q.Elements("item")
                                               select new CstMenuItem()
                                               {
                                                   Id = q1.Attribute("id").Value,
                                                   Title = q1.Attribute("title").Value,
                                                   Href = q1.Attribute("href").Value,
                                                   Roles = q1.Attribute("roles").Value
                                               }).ToList()
                                       }).ToList();
                AspNetCache.Instance.Insert(cachingKey, menus, fd);
                return menus;
            }
            catch
            {
                return null;
            }
        }
    }
}

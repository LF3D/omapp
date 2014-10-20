using System;
using System.Text.RegularExpressions;
using MC = Microsoft.Practices.EnterpriseLibrary.Caching;

namespace OperatingManagement.Framework.Cache
{
    internal class DefaultCacheStrategy : CacheStrategy
    {
        private MC.ICacheManager cacheManager;

        public DefaultCacheStrategy()
        {
            cacheManager = MC.CacheFactory.GetCacheManager();
        }

        public override void Insert(string key, object obj, CacheItemPriority priority, params ICacheItemExpiration[] deps)
        {
            cacheManager.Add(key.ToLower(), obj, (MC.CacheItemPriority)priority, null, deps);
        }

        public override object Get(string key)
        {
            return cacheManager.GetData(key.ToLower());
        }

        public override void Remove(string key)
        {
            cacheManager.Remove(key.ToLower());
        }

        public override void Clear()
        {
            cacheManager.Flush();
        }
    }
}

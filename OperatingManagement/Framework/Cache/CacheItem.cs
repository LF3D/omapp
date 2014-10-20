using System;
using MC = Microsoft.Practices.EnterpriseLibrary.Caching;
using MCE = Microsoft.Practices.EnterpriseLibrary.Caching.Expirations;

namespace OperatingManagement.Framework.Cache
{
    /// <summary>
    /// Classes inherites from this interface to represent CacheItem expiration object.
    /// </summary>
    public interface ICacheItemExpiration : MC.ICacheItemExpiration
    { }

    /// <summary>
    /// Provides sealed class for expired time.
    /// </summary>
    [Serializable]
    public class AbsoluteTime : MCE.AbsoluteTime, ICacheItemExpiration
    {
        public AbsoluteTime(DateTime absoluteTime)
            : base(absoluteTime)
        {
        }

        public AbsoluteTime(TimeSpan timeFromNow)
            : base(timeFromNow)
        { }
    }

    /// <summary>
    /// Provides sealed class for extended time.
    /// </summary>
    [Serializable]
    public class ExtendedFormatTime : MCE.ExtendedFormatTime, ICacheItemExpiration
    {
        public ExtendedFormatTime(string timeFormat)
            : base(timeFormat)
        {
        }
    }

    /// <summary>
    /// Provides sealed class for file dependency object.
    /// </summary>
    [Serializable]
    public class FileDependency : MCE.FileDependency, ICacheItemExpiration
    {
        /// <summary>
        /// create a new instance of <see cref="OperatingManagement.Framework.Cache.FileDependency"/> class.
        /// </summary>
        /// <param name="fullFileName"></param>
        public FileDependency(string fullFileName)
            : base(fullFileName)
        {
        }
    }

    /// <summary>
    /// Provides sealed class for nerver expired cache.
    /// </summary>
    [Serializable]
    public class NeverExpired : MCE.NeverExpired, ICacheItemExpiration
    {
        public NeverExpired()
            : base()
        { }
    }
    /// <summary>
    /// Provides sealed class for sliding time.
    /// </summary>
    [Serializable]
    public class SlidingTime : MCE.SlidingTime, ICacheItemExpiration
    {
        public SlidingTime(TimeSpan slidingExpiration)
            : base(slidingExpiration)
        { }

        public SlidingTime(TimeSpan slidingExpiration, DateTime originalTimeStamp)
            : base(slidingExpiration, originalTimeStamp)
        {
        }
    }



    public enum CacheItemPriority
    {
        None = 0,
        Low = 1,
        Normal = 2,
        High = 3,
        NotRemovable = 4,
    }
}

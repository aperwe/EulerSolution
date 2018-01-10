using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QBits.Intuition.Collections
{
    /// <summary>
    /// Supports adding strings to colletion, limiting its maximum count of elements, and returning the cached collection.
    /// </summary>
    public class StringCache
    {
        /// <summary>
        /// Internal set of string cache.
        /// </summary>
        protected HashSet<string> Cache { get; set; }
        /// <summary>
        /// Cache capacity.
        /// </summary>
        protected int Capacity { get; set; }
        /// <summary>
        /// Default constructor with string capacity of 10.
        /// </summary>
        public StringCache()
        {
            InitializeCache(10);
        }
        /// <summary>
        /// Constructor allowing to set the capacity of the cache.
        /// </summary>
        /// <param name="capacity">Initial capacity of the cache.</param>
        public StringCache(int capacity)
        {
            InitializeCache(capacity);
        }

        /// <summary>
        /// Initializes the cache and also sets the cache capacity.
        /// </summary>
        /// <param name="capacity"></param>
        protected void InitializeCache(int capacity)
        {
            Capacity = capacity;
            Cache = new HashSet<string>();
            CacheDirty = true;
            ReturnedCache = null;
        }
        /// <summary>
        /// Copy of the cache as Enumerable.
        /// </summary>
        protected IEnumerable<string> ReturnedCache;
        /// <summary>
        /// Indicates whether cache has been modified since last reference.
        /// </summary>
        protected bool CacheDirty;
        /// <summary>
        /// Gets a copy of the strings cached in this cache.
        /// </summary>
        public virtual IEnumerable<string> CachedStrings
        {
            get
            {
                if (CacheDirty)
                {
                    ReturnedCache = Cache.ToList().AsReadOnly();
                    CacheDirty = false;
                }
                return ReturnedCache;
            }
        }
    }

    /// <summary>
    /// Supports adding strings to colletion, limiting its maximum count of elements, and returning the cached collection which is sorted by the cached string.
    /// </summary>
    public class SortedStringCache : StringCache
    {
        /// <summary>
        /// Helper method returning a read-only copy of the cache sorted ascending.
        /// </summary>
        public override IEnumerable<string> CachedStrings
        {
            get
            {
                if (CacheDirty)
                {
                    ReturnedCache = Cache.OrderBy(item => item).ToList().AsReadOnly();
                    CacheDirty = false;
                }
                return ReturnedCache;
            }
        }
    }
}

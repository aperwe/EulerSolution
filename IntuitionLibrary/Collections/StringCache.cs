using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace QBits.Intuition.Collections
{
    /// <summary>
    /// Supports adding strings to collection, limiting its maximum count of elements, and returning the cached collection.
    /// When the cache is full, <see cref="Add"/> removes the least recently used string. This class is not thread-safe.
    /// </summary>
    public class StringCache
    {
        /// <summary>
        /// Strings in the order of their last use by <see cref="Add"/>: the least recently used first.
        /// </summary>
        private readonly LinkedList<string> usageOrder = new();
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
        [MemberNotNull(nameof(Cache), nameof(ReturnedCache))]
        protected void InitializeCache(int capacity)
        {
            Capacity = capacity;
            Cache = new HashSet<string>();
            usageOrder.Clear();
            CacheDirty = true;
            ReturnedCache = Array.Empty<string>();
        }

        /// <summary>
        /// Adds a string to the cache. If the cache already holds <see cref="Capacity"/> strings, the least recently used one is removed.
        /// Adding a string that is already cached makes it the most recently used one.
        /// </summary>
        /// <param name="value">String to add. Comparison is case-sensitive.</param>
        /// <returns>True if the string was added; false if it was already cached, or if <see cref="Capacity"/> is 0 or less.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
        public bool Add(string value)
        {
            ArgumentNullException.ThrowIfNull(value);
            SynchronizeUsageOrder();

            if (Cache.Contains(value))
            {
                usageOrder.Remove(value);
                usageOrder.AddLast(value);
                return false;
            }
            if (Capacity <= 0) return false;

            while (Cache.Count >= Capacity)
            {
                Cache.Remove(usageOrder.First!.Value);
                usageOrder.RemoveFirst();
            }
            Cache.Add(value);
            usageOrder.AddLast(value);
            CacheDirty = true;
            return true;
        }

        /// <summary>
        /// A derived class can change <see cref="Cache"/> directly. Strings it removed are forgotten,
        /// and strings it added count as the least recently used ones.
        /// </summary>
        private void SynchronizeUsageOrder()
        {
            for (var node = usageOrder.First; node != null;)
            {
                var next = node.Next;
                if (!Cache.Contains(node.Value)) usageOrder.Remove(node);
                node = next;
            }
            if (usageOrder.Count == Cache.Count) return;
            foreach (var value in Cache.Where(value => !usageOrder.Contains(value)).ToList())
            {
                usageOrder.AddFirst(value);
            }
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
    /// Supports adding strings to collection, limiting its maximum count of elements, and returning the cached collection which is sorted by the cached string.
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

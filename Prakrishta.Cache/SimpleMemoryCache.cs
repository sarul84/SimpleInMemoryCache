// -------------------------------------------------------------------------------
// <copyright file="SimpleMemoryCache.cs" company="Prakrishta Technologies">
// Copyright © 2018 All Right Reserved
// </copyright>
// <Author>Arul Sengottaiyan</Author>
// <date>07/18/2018</date>
// <summary>In-Memory cache implementation class</summary>
// --------------------------------------------------------------------------------

namespace Prakrishta.Cache
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;


    /// <summary>
    /// Cache class to handle in-memory caching
    /// </summary>
    public sealed class SimpleMemoryCache : ICache
    {
        /// <summary>
        /// Holds all cached item details
        /// </summary>
        private readonly ConcurrentDictionary<CachedItemKey, object> memoryCache = new ConcurrentDictionary<CachedItemKey, object>();

        /// <summary>
        /// To detect redundant calls
        /// </summary>
        private bool disposedValue = false;

        /// <summary>
        /// Gets number of items cached
        /// </summary>
        public int Count => this.memoryCache.Count;

        /// <summary>
        /// Gets a value that indicates whether the cache is empty or not
        /// </summary>
        public bool IsEmpty => this.memoryCache.IsEmpty;

        /// <summary>
        /// Gets cached item for the key
        /// </summary>
        /// <param name="key">Cache key</param>
        /// <returns>Cached item</returns>
        public IEnumerable<object> this[string key] => this.Get(key);

        /// <summary>
        /// Gets cached item for the key and session
        /// </summary>
        /// <param name="key">Cache key</param>
        /// <param name="session">Session key</param>
        /// <returns>Cached item</returns>
        public object this[string key, string session] => this.Get(key, session);

        /// <summary>
        /// Method to add or update item to be cached into the cache
        /// </summary>
        /// <param name="key">Cache key</param>
        /// <param name="value">Value to be cached</param>
        /// <param name="session">Session key</param>
        /// <param name="cacheTime">Time after item to be removed from cache, in milliseconds</param>
        /// <returns>Returns boolean indicating if item is added or not</returns>
        public bool AddOrUpdate(string key, object value, string session, int cacheTime = Timeout.Infinite)
        {
            return this.AddOrUpdate(key, value, session, TimeSpan.FromMilliseconds((double)cacheTime));
        }

        /// <summary>
        /// Method to add or update item to be cached into the cache
        /// </summary>
        /// <param name="key">Cache key</param>
        /// <param name="value">Value to be cached</param>
        /// <param name="session">Session key</param>
        /// <param name="cacheTime">Time after item to be removed from cache</param>
        /// <returns>Returns boolean indicating if item is added or not</returns>
        public bool AddOrUpdate(string key, object value, string session, TimeSpan cacheTime)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var cachedItemKey = new CachedItemKey(key, session);

            Timer timer = new Timer(new TimerCallback(TimerProc), cachedItemKey, cacheTime, TimeSpan.FromMilliseconds(-1));

            var addOrUpdatedValue = this.memoryCache.AddOrUpdate(cachedItemKey, value, (cacheKey, oldValue) => oldValue = value);

            return addOrUpdatedValue != null;

            void TimerProc(object state)
            {
                var cacheKey = (CachedItemKey)state;
                this.Remove(cacheKey);
            }
        }

        /// <summary>
        /// Method to clear in-memory cache for the session
        /// </summary>
        /// <param name="session">session value</param>
        public void Clear(string session)
        {
            var keys = from cacheKeys in this.memoryCache
                       where cacheKeys.Key.Session.Equals(session, StringComparison.OrdinalIgnoreCase)
                       select cacheKeys.Key;

            foreach (var key in keys)
            {
                this.Remove(key);
            }
        }

        /// <summary>
        /// Clears all cached items from in-memory cache
        /// </summary>
        public void ClearAll()
        {
            this.memoryCache?.Clear();
        }

        /// <summary>
        /// Dispose of the object
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Get cached item from in-memory cache based
        /// </summary>
        /// <param name="key">cache key</param>
        /// <param name="session">session key</param>
        /// <returns>cached item</returns>
        public object Get(string key, string session)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            object value = null;

            var cacheKey = new CachedItemKey(key, session);
            if (this.memoryCache?.ContainsKey(cacheKey) == true)
            {
                this.memoryCache?.TryGetValue(cacheKey, out value);
            }

            return value;
        }

        /// <summary>
        /// Removes cached item from in-memory cache
        /// </summary>
        /// <param name="key">Cache key</param>
        /// <param name="session">Session key</param>
        /// <returns>Returns boolean indicating if cached item is removed or not</returns>
        public bool Remove(string key, string session)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            var cacheKey = new CachedItemKey(key, session);

            return this.Remove(cacheKey);
        }

        /// <summary>
        /// Removes cached item from in-memory cache
        /// </summary>
        /// <param name="key">Cache key</param>
        public void Remove(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            var keys = from cacheKeys in this.memoryCache
                       where cacheKeys.Key.Key.Equals(key, StringComparison.OrdinalIgnoreCase)
                       select cacheKeys.Key;

            foreach (var compositeKey in keys)
            {
                this.Remove(compositeKey);
            }
        }


        /// <summary>
        /// Dispose of this class
        /// </summary>
        /// <param name="disposing">True if we are disposing</param>
        private void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    this.ClearAll();
                }

                this.disposedValue = true;
            }
        }

        /// <summary>
        /// Internal method to remove cached item from in-memory cache
        /// </summary>
        /// <param name="cachedItemKey">Cache key</param>
        /// <returns>Returns boolean indicating if cached item is removed or not</returns>
        private bool Remove(CachedItemKey cachedItemKey)
        {
            object value = null;
            if (this.memoryCache?.ContainsKey(cachedItemKey) == true)
            {
                this.memoryCache?.TryRemove(cachedItemKey, out value);
            }

            return value != null;
        }

        /// <summary>
        /// Get cached items from in-memory cache based on value of key paramter of composite key
        /// </summary>
        /// <param name="key">cache key</param>
        /// <returns>cached items</returns>
        private IEnumerable<object> Get(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            IEnumerable<object> value = Enumerable.Empty<object>();

            value = from cacheKeys in this.memoryCache
                    where cacheKeys.Key.Key.Equals(key, StringComparison.OrdinalIgnoreCase)
                    select cacheKeys.Value;

            return value;
        }
    }
}

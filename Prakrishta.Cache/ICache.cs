// -------------------------------------------------------------------------------
// <copyright file="ICache.cs" company="Prakrishta Technologies">
// Copyright © 2018 All Right Reserved
// </copyright>
// <Author>Arul Sengottaiyan</Author>
// <date>07/18/2018</date>
// <summary>Contract file for cache class</summary>
// --------------------------------------------------------------------------------
namespace Prakrishta.Cache
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    /// <summary>
    /// The contract to save data to in-memory cache
    /// </summary>
    /// <typeparam name="TKey">The cache key to be used for storing data to in-memory cache</typeparam>
    /// <typeparam name="TValue">The data to be cached</typeparam>
    public interface ICache<TKey, TValue> : IDisposable
    {
        /// <summary>
        /// Gets number of items cached
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Gets a value that indicates whether the cache is empty or not
        /// </summary>
        bool IsEmpty { get; }
        
        /// <summary>
        /// Gets cached item for the key
        /// </summary>
        /// <param name="key">Cache key</param>
        /// <returns>Cached item</returns>
        IEnumerable<TValue> this[TKey key] { get; }

        /// <summary>
        /// Gets cached item for the composite key
        /// </summary>
        /// <param name="key">Cached key</param>
        /// <param name="session">Session key</param>
        /// <returns>Cached item</returns>
        TValue this[TKey key, string session] { get; }

        /// <summary>
        /// Get cached item from in-memory cache based on composite key of key & seesion value
        /// </summary>
        /// <param name="key">cache key</param>
        /// <param name="session">session key</param>
        /// <returns>cached item</returns>
        TValue Get(TKey key, string session);        

        /// <summary>
        /// Method to add or update item to be cached into the cache
        /// </summary>
        /// <param name="key">Cache key</param>
        /// <param name="value">Value to be cached</param>
        /// <param name="session">Session key</param>
        /// <param name="cacheTime">Time after item to be removed from cache, in milliseconds</param>
        /// <returns>Returns boolean indicating if item is added or not</returns>
        bool AddOrUpdate(TKey key, TValue value, string session, int cacheTime = Timeout.Infinite);

        /// <summary>
        /// Method to add or update item to be cached into the cache
        /// </summary>
        /// <param name="key">Cache key</param>
        /// <param name="value">Value to be cached</param>
        /// <param name="session">Session key</param>
        /// <param name="cacheTime">Time after item to be removed from cache</param>
        /// <returns>Returns boolean indicating if item is added or not</returns>
        bool AddOrUpdate(TKey key, TValue value, string session, TimeSpan cacheTime);

        /// <summary>
        /// Removes cached item from in-memory cache
        /// </summary>
        /// <param name="key">Cache key</param>
        void Remove(TKey key);

        /// <summary>
        /// Removes cached item from in-memory cache
        /// </summary>
        /// <param name="key">Cache key</param>
        /// <param name="session">Session key</param>
        /// <returns>Returns boolean indicating if cached item is removed or not</returns>
        bool Remove(TKey key, string session);

        /// <summary>
        /// Method to clear in-memory cache for the session
        /// </summary>
        /// <param name="session">session value</param>
        void Clear(string session);

        /// <summary>
        /// Clear all cached items from in-memory cache
        /// </summary>
        void ClearAll();
    }

    /// <summary>
    /// The contract to save data to in-memory cache
    /// </summary>
    public interface ICache : ICache<string, object>
    {
    }
}

// -------------------------------------------------------------------------------
// <copyright file="CachedItemKey.cs" company="Prakrishta Technologies">
// Copyright © 2018 All Right Reserved
// </copyright>
// <Author>Arul Sengottaiyan</Author>
// <date>07/18/2018</date>
// <summary>Cached Item key struct for cache framework</summary>
// --------------------------------------------------------------------------------
namespace Prakrishta.Cache
{
    /// <summary>
    /// Composite key class
    /// </summary>
    public struct CachedItemKey
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CachedItemKey" /> struct.
        /// </summary>
        /// <param name="key">Cache key</param>
        /// <param name="session">Session key</param>
        public CachedItemKey(string key, string session)
        {
            this.Key = key;
            this.Session = session;
        }

        /// <summary>
        /// Gets Cache key
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// Gets Seesion key
        /// </summary>
        public string Session { get; }
    }
}

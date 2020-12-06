//
//  CacheSettings.cs
//
//  Author:
//       Jarl Gullberg <jarl.gullberg@gmail.com>
//
//  Copyright (c) 2017 Jarl Gullberg
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
//
//  You should have received a copy of the GNU Lesser General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//

using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Remora.Discord.Caching.Services
{
    /// <summary>
    /// Holds various settings for individual cache objects.
    /// </summary>
    [PublicAPI]
    public class CacheSettings
    {
        /// <summary>
        /// Holds absolute cache expiration values for various types.
        /// </summary>
        private readonly IReadOnlyDictionary<Type, TimeSpan> _absoluteCacheExpirations;

        /// <summary>
        /// Holds sliding cache expiration values for various types.
        /// </summary>
        private readonly IReadOnlyDictionary<Type, TimeSpan> _slidingCacheExpirations;

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheSettings"/> class.
        /// </summary>
        /// <param name="absoluteCacheExpirations">The absolute cache expirations.</param>
        /// <param name="slidingCacheExpirations">The sliding cache expirations.</param>
        public CacheSettings
        (
            IReadOnlyDictionary<Type, TimeSpan> absoluteCacheExpirations,
            IReadOnlyDictionary<Type, TimeSpan> slidingCacheExpirations
        )
        {
            _absoluteCacheExpirations = absoluteCacheExpirations;
            _slidingCacheExpirations = slidingCacheExpirations;
        }

        /// <summary>
        /// Gets the absolute expiration time in the cache for the given type, or a default value if one does not exist.
        /// </summary>
        /// <param name="defaultExpiration">The default expiration. Defaults to 30 seconds.</param>
        /// <typeparam name="T">The cached type.</typeparam>
        /// <returns>The absolute expiration time.</returns>
        public TimeSpan GetAbsoluteExpirationOrDefault<T>(TimeSpan? defaultExpiration = null)
        {
            defaultExpiration ??= TimeSpan.FromSeconds(30);
            return GetAbsoluteExpirationOrDefault(typeof(T), defaultExpiration);
        }

        /// <summary>
        /// Gets the absolute expiration time in the cache for the given type, or a default value if one does not exist.
        /// </summary>
        /// <param name="cachedType">The cached type.</param>
        /// <param name="defaultExpiration">The default expiration. Defaults to 30 seconds.</param>
        /// <returns>The absolute expiration time.</returns>
        public TimeSpan GetAbsoluteExpirationOrDefault(Type cachedType, TimeSpan? defaultExpiration = null)
        {
            defaultExpiration ??= TimeSpan.FromSeconds(30);
            if (_absoluteCacheExpirations.TryGetValue(cachedType, out var absoluteExpiration))
            {
                return absoluteExpiration;
            }

            return defaultExpiration.Value;
        }

        /// <summary>
        /// Gets the sliding expiration time in the cache for the given type, or a default value if one does not exist.
        /// </summary>
        /// <param name="defaultExpiration">The default expiration. Defaults to 10 seconds.</param>
        /// <typeparam name="T">The cached type.</typeparam>
        /// <returns>The sliding expiration time.</returns>
        public TimeSpan GetSlidingExpirationOrDefault<T>(TimeSpan? defaultExpiration = null)
        {
            defaultExpiration ??= TimeSpan.FromSeconds(10);
            return GetSlidingExpirationOrDefault(typeof(T), defaultExpiration);
        }

        /// <summary>
        /// Gets the sliding expiration time in the cache for the given type, or a default value if one does not exist.
        /// </summary>
        /// <param name="cachedType">The cached type.</param>
        /// <param name="defaultExpiration">The default expiration. Defaults to 10 seconds.</param>
        /// <returns>The sliding expiration time.</returns>
        public TimeSpan GetSlidingExpirationOrDefault(Type cachedType, TimeSpan? defaultExpiration = null)
        {
            defaultExpiration ??= TimeSpan.FromSeconds(10);
            if (_slidingCacheExpirations.TryGetValue(cachedType, out var slidingExpiration))
            {
                return slidingExpiration;
            }

            return defaultExpiration.Value;
        }
    }
}
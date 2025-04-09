using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;

namespace mxProject.Data.Repositories
{
    /// <summary>
    /// Extension methods for IReadDataRepository and IReadDataRepositoryWithContext.
    /// </summary>
    public static class IReadDataRepositoryExtensions
    {
        #region WithCache

        /// <summary>
        /// Adds the ability to cache entities.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TKey">The key type.</typeparam>
        /// <param name="this"></param>
        /// <returns></returns>
        public static IReadDataRepository<TEntity, TKey> WithCache<TEntity, TKey>(
            this IReadDataRepository<TEntity, TKey> @this
        )
            where TEntity : IHasKey<TKey>
        {
            return @this.WithCache(CreateDefaultCache<TKey, TEntity>(), true);
        }

        /// <summary>
        /// Adds the ability to cache entities.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TKey">The key type.</typeparam>
        /// <param name="this"></param>
        /// <param name="keyGetter">The method to get the key from a spacified entiry.</param>
        /// <returns></returns>
        public static IReadDataRepository<TEntity, TKey> WithCache<TEntity, TKey>(
            this IReadDataRepository<TEntity, TKey> @this,
            Func<TEntity, TKey> keyGetter
        )
        {
            return @this.WithCache(keyGetter, CreateDefaultCache<TKey, TEntity>(), true);
        }

        /// <summary>
        /// Adds the ability to cache entities.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TKey">The key type.</typeparam>
        /// <param name="this"></param>
        /// <param name="cache">The cache.</param>
        /// <param name="disposableCache">A value indicating whether the cache can be dispose.</param>
        /// <returns></returns>
        public static IReadDataRepository<TEntity, TKey> WithCache<TEntity, TKey>(
            this IReadDataRepository<TEntity, TKey> @this,
            IEntityCache<TKey, TEntity> cache,
            bool disposableCache = false
        )
            where TEntity : IHasKey<TKey>
        {
            return new Caching.CacheReadDataRepository<TEntity, TKey>(
                @this,
                (entity) => entity.GetKey(),
                cache,
                disposableCache
                );
        }

        /// <summary>
        /// Adds the ability to cache entities.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TKey">The key type.</typeparam>
        /// <param name="this"></param>
        /// <param name="keyGetter">The method to get the key from a spacified entiry.</param>
        /// <param name="cache">The cache.</param>
        /// <param name="disposableCache">A value indicating whether the cache can be dispose.</param>
        /// <returns></returns>
        public static IReadDataRepository<TEntity, TKey> WithCache<TEntity, TKey>(
            this IReadDataRepository<TEntity, TKey> @this,
            Func<TEntity, TKey> keyGetter,
            IEntityCache<TKey, TEntity> cache,
            bool disposableCache = false
        )
        {
            return new Caching.CacheReadDataRepository<TEntity, TKey>(
                @this,
                keyGetter,
                cache,
                disposableCache
                );
        }

        /// <summary>
        /// Adds the ability to cache entities.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TKey">The key type.</typeparam>
        /// <param name="this"></param>
        /// <param name="cache">The cache.</param>
        /// <param name="disposableCache">A value indicating whether the cache can be dispose.</param>
        /// <returns></returns>
        public static IReadDataRepository<TEntity, TKey> WithCache<TEntity, TKey>(
            this IReadDataRepository<TEntity, TKey> @this,
            IDictionary<TKey, TEntity> cache,
            bool disposableCache = false
        )
            where TEntity : IHasKey<TKey>
        {
            return new Caching.CacheReadDataRepository<TEntity, TKey>(
                @this,
                (entity) => entity.GetKey(),
                CreateCache(cache),
                disposableCache
                );
        }

        /// <summary>
        /// Adds the ability to cache entities.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TKey">The key type.</typeparam>
        /// <param name="this"></param>
        /// <param name="keyGetter">The method to get the key from a spacified entiry.</param>
        /// <param name="cache">The cache.</param>
        /// <param name="disposableCache">A value indicating whether the cache can be dispose.</param>
        /// <returns></returns>
        public static IReadDataRepository<TEntity, TKey> WithCache<TEntity, TKey>(
            this IReadDataRepository<TEntity, TKey> @this,
            Func<TEntity, TKey> keyGetter,
            IDictionary<TKey, TEntity> cache,
            bool disposableCache = false
        )
        {
            return new Caching.CacheReadDataRepository<TEntity, TKey>(
                @this,
                keyGetter,
                CreateCache(cache),
                disposableCache
                );
        }

        /// <summary>
        /// Creates a default entity cache.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <returns></returns>
        private static IEntityCache<TKey, TEntity> CreateDefaultCache<TKey, TEntity>()
        {
            return new Caching.EntityCacheDictionary<TKey, TEntity>(new ConcurrentDictionary<TKey, TEntity>());
        }

        /// <summary>
        /// Creates a default entity cache.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        private static IEntityCache<TKey, TEntity> CreateCache<TKey, TEntity>(IDictionary<TKey, TEntity> dictionary)
        {
            return new Caching.EntityCacheDictionary<TKey, TEntity>(dictionary);
        }

        #endregion

        #region WithCache (WithContext)

        /// <summary>
        /// Adds the ability to cache entities.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TKey">The key type.</typeparam>
        /// <typeparam name="TContext">The context type.</typeparam>
        /// <param name="this"></param>
        /// <returns></returns>
        public static IReadDataRepositoryWithContext<TEntity, TKey, TContext> WithCache<TEntity, TKey, TContext>(
            this IReadDataRepositoryWithContext<TEntity, TKey, TContext> @this
        )
            where TEntity : IHasKey<TKey>
            where TContext : IDataRepositoryContext
        {
            return @this.WithCache(CreateDefaultCache<TKey, TEntity>(), true);
        }

        /// <summary>
        /// Adds the ability to cache entities.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TKey">The key type.</typeparam>
        /// <typeparam name="TContext">The context type.</typeparam>
        /// <param name="this"></param>
        /// <param name="keyGetter">The method to get the key from a spacified entiry.</param>
        /// <returns></returns>
        public static IReadDataRepositoryWithContext<TEntity, TKey, TContext> WithCache<TEntity, TKey, TContext>(
            this IReadDataRepositoryWithContext<TEntity, TKey, TContext> @this,
            Func<TEntity, TKey> keyGetter
        )
            where TContext : IDataRepositoryContext
        {
            return @this.WithCache(keyGetter, CreateDefaultCache<TKey, TEntity>(), true);
        }

        /// <summary>
        /// Adds the ability to cache entities.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TKey">The key type.</typeparam>
        /// <typeparam name="TContext">The context type.</typeparam>
        /// <param name="this"></param>
        /// <param name="cache">The cache.</param>
        /// <param name="disposableCache">A value indicating whether the cache can be dispose.</param>
        /// <returns></returns>
        public static IReadDataRepositoryWithContext<TEntity, TKey, TContext> WithCache<TEntity, TKey, TContext>(
            this IReadDataRepositoryWithContext<TEntity, TKey, TContext> @this,
            IEntityCache<TKey, TEntity> cache,
            bool disposableCache = false
        )
            where TEntity : IHasKey<TKey>
            where TContext : IDataRepositoryContext
        {
            return new Caching.CacheReadDataRepositoryWithContext<TEntity, TKey, TContext>(
                @this,
                (entity) => entity.GetKey(),
                cache,
                disposableCache
                );
        }

        /// <summary>
        /// Adds the ability to cache entities.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TKey">The key type.</typeparam>
        /// <typeparam name="TContext">The context type.</typeparam>
        /// <param name="this"></param>
        /// <param name="keyGetter">The method to get the key from a spacified entiry.</param>
        /// <param name="cache">The cache.</param>
        /// <param name="disposableCache">A value indicating whether the cache can be dispose.</param>
        /// <returns></returns>
        public static IReadDataRepositoryWithContext<TEntity, TKey, TContext> WithCache<TEntity, TKey, TContext>(
            this IReadDataRepositoryWithContext<TEntity, TKey, TContext> @this,
            Func<TEntity, TKey> keyGetter,
            IEntityCache<TKey, TEntity> cache,
            bool disposableCache = false
        )
            where TContext : IDataRepositoryContext
        {
            return new Caching.CacheReadDataRepositoryWithContext<TEntity, TKey, TContext>(
                @this,
                keyGetter,
                cache,
                disposableCache
                );
        }

        /// <summary>
        /// Adds the ability to cache entities.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TKey">The key type.</typeparam>
        /// <typeparam name="TContext">The context type.</typeparam>
        /// <param name="this"></param>
        /// <param name="cache">The cache.</param>
        /// <param name="disposableCache">A value indicating whether the cache can be dispose.</param>
        /// <returns></returns>
        public static IReadDataRepositoryWithContext<TEntity, TKey, TContext> WithCache<TEntity, TKey, TContext>(
            this IReadDataRepositoryWithContext<TEntity, TKey, TContext> @this,
            IDictionary<TKey, TEntity> cache,
            bool disposableCache = false
        )
            where TEntity : IHasKey<TKey>
            where TContext : IDataRepositoryContext
        {
            return new Caching.CacheReadDataRepositoryWithContext<TEntity, TKey, TContext>(
                @this,
                (entity) => entity.GetKey(),
                CreateCache(cache),
                disposableCache
                );
        }

        /// <summary>
        /// Adds the ability to cache entities.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TKey">The key type.</typeparam>
        /// <typeparam name="TContext">The context type.</typeparam>
        /// <param name="this"></param>
        /// <param name="keyGetter">The method to get the key from a spacified entiry.</param>
        /// <param name="cache">The cache.</param>
        /// <param name="disposableCache">A value indicating whether the cache can be dispose.</param>
        /// <returns></returns>
        public static IReadDataRepositoryWithContext<TEntity, TKey, TContext> WithCache<TEntity, TKey, TContext>(
            this IReadDataRepositoryWithContext<TEntity, TKey, TContext> @this,
            Func<TEntity, TKey> keyGetter,
            IDictionary<TKey, TEntity> cache,
            bool disposableCache = false
        )
            where TContext : IDataRepositoryContext
        {
            return new Caching.CacheReadDataRepositoryWithContext<TEntity, TKey, TContext>(
                @this,
                keyGetter,
                CreateCache(cache),
                disposableCache
                );
        }

        #endregion

        #region WithCache (WithUniqueKey)

        /// <summary>
        /// Adds the ability to cache entities.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TPrimaryKey">The primary key type.</typeparam>
        /// <typeparam name="TUniqueKey">The unique key type.</typeparam>
        /// <param name="this"></param>
        /// <returns></returns>
        public static IReadDataRepositoryWithUniqueKey<TEntity, TPrimaryKey, TUniqueKey> WithCache<TEntity, TPrimaryKey, TUniqueKey>(
            this IReadDataRepositoryWithUniqueKey<TEntity, TPrimaryKey, TUniqueKey> @this
        )
            where TEntity : IHasKey<TPrimaryKey>, IHasKey<TUniqueKey>
        {
            return @this.WithCache(
                CreateDefaultCache<TPrimaryKey, TEntity>(),
                CreateDefaultCache<TUniqueKey, TEntity>(),
                true
                );
        }

        /// <summary>
        /// Adds the ability to cache entities.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TPrimaryKey">The primary key type.</typeparam>
        /// <typeparam name="TUniqueKey">The unique key type.</typeparam>
        /// <param name="this"></param>
        /// <param name="primaryKeyGetter">The method to get the primary key from a spacified entiry.</param>
        /// <param name="uniqueKeyGetter">The method to get the unique key from a spacified entiry.</param>
        /// <returns></returns>
        public static IReadDataRepositoryWithUniqueKey<TEntity, TPrimaryKey, TUniqueKey> WithCache<TEntity, TPrimaryKey, TUniqueKey>(
            this IReadDataRepositoryWithUniqueKey<TEntity, TPrimaryKey, TUniqueKey> @this,
            Func<TEntity, TPrimaryKey> primaryKeyGetter,
            Func<TEntity, TUniqueKey> uniqueKeyGetter
        )
        {
            return @this.WithCache(
                primaryKeyGetter,
                CreateDefaultCache<TPrimaryKey, TEntity>(),
                uniqueKeyGetter,
                CreateDefaultCache<TUniqueKey, TEntity>(),
                true
                );
        }

        /// <summary>
        /// Adds the ability to cache entities.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TPrimaryKey">The primary key type.</typeparam>
        /// <typeparam name="TUniqueKey">The unique key type.</typeparam>
        /// <param name="this"></param>
        /// <param name="primaryKeyCache">The cache using a primary key.</param>
        /// <param name="uniqueKeyCache">The cache using a unique key.</param>
        /// <param name="disposableCache">A value indicating whether the cache can be dispose.</param>
        /// <returns></returns>
        public static IReadDataRepositoryWithUniqueKey<TEntity, TPrimaryKey, TUniqueKey> WithCache<TEntity, TPrimaryKey, TUniqueKey>(
            this IReadDataRepositoryWithUniqueKey<TEntity, TPrimaryKey, TUniqueKey> @this,
            IEntityCache<TPrimaryKey, TEntity> primaryKeyCache,
            IEntityCache<TUniqueKey, TEntity> uniqueKeyCache,
            bool disposableCache = false
        )
            where TEntity : IHasKey<TPrimaryKey>, IHasKey<TUniqueKey>
        {
            return new Caching.CacheReadDataRepositoryWithUniqueKey<TEntity, TPrimaryKey, TUniqueKey>(
                @this,
                (entity) => ((IHasKey<TPrimaryKey>)entity).GetKey(),
                primaryKeyCache,
                (entity) => ((IHasKey<TUniqueKey>)entity).GetKey(),
                uniqueKeyCache,
                disposableCache
                );
        }

        /// <summary>
        /// Adds the ability to cache entities.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TPrimaryKey">The primary key type.</typeparam>
        /// <typeparam name="TUniqueKey">The unique key type.</typeparam>
        /// <param name="this"></param>
        /// <param name="primaryKeyGetter">The method to get the primary key from a spacified entiry.</param>
        /// <param name="primaryKeyCache">The cache using a primary key.</param>
        /// <param name="uniqueKeyGetter">The method to get the unique key from a spacified entiry.</param>
        /// <param name="uniqueKeyCache">The cache using a unique key.</param>
        /// <param name="disposableCache">A value indicating whether the cache can be dispose.</param>
        /// <returns></returns>
        public static IReadDataRepositoryWithUniqueKey<TEntity, TPrimaryKey, TUniqueKey> WithCache<TEntity, TPrimaryKey, TUniqueKey>(
            this IReadDataRepositoryWithUniqueKey<TEntity, TPrimaryKey, TUniqueKey> @this,
            Func<TEntity, TPrimaryKey> primaryKeyGetter,
            IEntityCache<TPrimaryKey, TEntity> primaryKeyCache,
            Func<TEntity, TUniqueKey> uniqueKeyGetter,
            IEntityCache<TUniqueKey, TEntity> uniqueKeyCache,
            bool disposableCache = false
        )
        {
            return new Caching.CacheReadDataRepositoryWithUniqueKey<TEntity, TPrimaryKey, TUniqueKey>(
                @this,
                primaryKeyGetter,
                primaryKeyCache,
                uniqueKeyGetter,
                uniqueKeyCache,
                disposableCache
                );
        }

        /// <summary>
        /// Adds the ability to cache entities.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TPrimaryKey">The primary key type.</typeparam>
        /// <typeparam name="TUniqueKey">The unique key type.</typeparam>
        /// <param name="this"></param>
        /// <param name="primaryKeyCache">The cache using a primary key.</param>
        /// <param name="uniqueKeyCache">The cache using a unique key.</param>
        /// <param name="disposableCache">A value indicating whether the cache can be dispose.</param>
        /// <returns></returns>
        public static IReadDataRepositoryWithUniqueKey<TEntity, TPrimaryKey, TUniqueKey> WithCache<TEntity, TPrimaryKey, TUniqueKey>(
            this IReadDataRepositoryWithUniqueKey<TEntity, TPrimaryKey, TUniqueKey> @this,
            IDictionary<TPrimaryKey, TEntity> primaryKeyCache,
            IDictionary<TUniqueKey, TEntity> uniqueKeyCache,
            bool disposableCache = false
        )
            where TEntity : IHasKey<TPrimaryKey>, IHasKey<TUniqueKey>
        {
            return new Caching.CacheReadDataRepositoryWithUniqueKey<TEntity, TPrimaryKey, TUniqueKey>(
                @this,
                (entity) => ((IHasKey<TPrimaryKey>)entity).GetKey(),
                CreateCache(primaryKeyCache),
                (entity) => ((IHasKey<TUniqueKey>)entity).GetKey(),
                CreateCache(uniqueKeyCache),
                disposableCache
                );
        }

        /// <summary>
        /// Adds the ability to cache entities.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TPrimaryKey">The primary key type.</typeparam>
        /// <typeparam name="TUniqueKey">The unique key type.</typeparam>
        /// <param name="this"></param>
        /// <param name="primaryKeyGetter">The method to get the primary key from a spacified entiry.</param>
        /// <param name="primaryKeyCache">The cache using a primary key.</param>
        /// <param name="uniqueKeyGetter">The method to get the unique key from a spacified entiry.</param>
        /// <param name="uniqueKeyCache">The cache using a unique key.</param>
        /// <param name="disposableCache">A value indicating whether the cache can be dispose.</param>
        /// <returns></returns>
        public static IReadDataRepositoryWithUniqueKey<TEntity, TPrimaryKey, TUniqueKey> WithCache<TEntity, TPrimaryKey, TUniqueKey>(
            this IReadDataRepositoryWithUniqueKey<TEntity, TPrimaryKey, TUniqueKey> @this,
            Func<TEntity, TPrimaryKey> primaryKeyGetter,
            IDictionary<TPrimaryKey, TEntity> primaryKeyCache,
            Func<TEntity, TUniqueKey> uniqueKeyGetter,
            IDictionary<TUniqueKey, TEntity> uniqueKeyCache,
            bool disposableCache = false
        )
        {
            return new Caching.CacheReadDataRepositoryWithUniqueKey<TEntity, TPrimaryKey, TUniqueKey>(
                @this,
                primaryKeyGetter,
                CreateCache(primaryKeyCache),
                uniqueKeyGetter,
                CreateCache(uniqueKeyCache),
                disposableCache
                );
        }

        #endregion

        #region WithCache (WithUniqueKeyWithContextWithContext)

        /// <summary>
        /// Adds the ability to cache entities.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TPrimaryKey">The primary key type.</typeparam>
        /// <typeparam name="TUniqueKey">The unique key type.</typeparam>
        /// <typeparam name="TContext">The context type.</typeparam>
        /// <param name="this"></param>
        /// <returns></returns>
        public static IReadDataRepositoryWithUniqueKeyWithContext<TEntity, TPrimaryKey, TUniqueKey, TContext> WithCache<TEntity, TPrimaryKey, TUniqueKey, TContext>(
            this IReadDataRepositoryWithUniqueKeyWithContext<TEntity, TPrimaryKey, TUniqueKey, TContext> @this
        )
            where TEntity : IHasKey<TPrimaryKey>, IHasKey<TUniqueKey>
            where TContext : IDataRepositoryContext
        {
            return @this.WithCache(
                CreateDefaultCache<TPrimaryKey, TEntity>(),
                CreateDefaultCache<TUniqueKey, TEntity>(),
                true
                );
        }

        /// <summary>
        /// Adds the ability to cache entities.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TPrimaryKey">The primary key type.</typeparam>
        /// <typeparam name="TUniqueKey">The unique key type.</typeparam>
        /// <typeparam name="TContext">The context type.</typeparam>
        /// <param name="this"></param>
        /// <param name="primaryKeyGetter">The method to get the primary key from a spacified entiry.</param>
        /// <param name="uniqueKeyGetter">The method to get the unique key from a spacified entiry.</param>
        /// <returns></returns>
        public static IReadDataRepositoryWithUniqueKeyWithContext<TEntity, TPrimaryKey, TUniqueKey, TContext> WithCache<TEntity, TPrimaryKey, TUniqueKey, TContext>(
            this IReadDataRepositoryWithUniqueKeyWithContext<TEntity, TPrimaryKey, TUniqueKey, TContext> @this,
            Func<TEntity, TPrimaryKey> primaryKeyGetter,
            Func<TEntity, TUniqueKey> uniqueKeyGetter
        )
            where TContext : IDataRepositoryContext
        {
            return @this.WithCache(
                primaryKeyGetter,
                CreateDefaultCache<TPrimaryKey, TEntity>(),
                uniqueKeyGetter,
                CreateDefaultCache<TUniqueKey, TEntity>(),
                true
                );
        }

        /// <summary>
        /// Adds the ability to cache entities.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TPrimaryKey">The primary key type.</typeparam>
        /// <typeparam name="TUniqueKey">The unique key type.</typeparam>
        /// <typeparam name="TContext">The context type.</typeparam>
        /// <param name="this"></param>
        /// <param name="primaryKeyCache">The cache using a primary key.</param>
        /// <param name="uniqueKeyCache">The cache using a unique key.</param>
        /// <param name="disposableCache">A value indicating whether the cache can be dispose.</param>
        /// <returns></returns>
        public static IReadDataRepositoryWithUniqueKeyWithContext<TEntity, TPrimaryKey, TUniqueKey, TContext> WithCache<TEntity, TPrimaryKey, TUniqueKey, TContext>(
            this IReadDataRepositoryWithUniqueKeyWithContext<TEntity, TPrimaryKey, TUniqueKey, TContext> @this,
            IEntityCache<TPrimaryKey, TEntity> primaryKeyCache,
            IEntityCache<TUniqueKey, TEntity> uniqueKeyCache,
            bool disposableCache = false
        )
            where TEntity : IHasKey<TPrimaryKey>, IHasKey<TUniqueKey>
            where TContext : IDataRepositoryContext
        {
            return new Caching.CacheReadDataRepositoryWithUniqueKeyWithContext<TEntity, TPrimaryKey, TUniqueKey, TContext>(
                @this,
                (entity) => ((IHasKey<TPrimaryKey>)entity).GetKey(),
                primaryKeyCache,
                (entity) => ((IHasKey<TUniqueKey>)entity).GetKey(),
                uniqueKeyCache,
                disposableCache
                );
        }

        /// <summary>
        /// Adds the ability to cache entities.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TPrimaryKey">The primary key type.</typeparam>
        /// <typeparam name="TUniqueKey">The unique key type.</typeparam>
        /// <typeparam name="TContext">The context type.</typeparam>
        /// <param name="this"></param>
        /// <param name="primaryKeyGetter">The method to get the primary key from a spacified entiry.</param>
        /// <param name="primaryKeyCache">The cache using a primary key.</param>
        /// <param name="uniqueKeyGetter">The method to get the unique key from a spacified entiry.</param>
        /// <param name="uniqueKeyCache">The cache using a unique key.</param>
        /// <param name="disposableCache">A value indicating whether the cache can be dispose.</param>
        /// <returns></returns>
        public static IReadDataRepositoryWithUniqueKeyWithContext<TEntity, TPrimaryKey, TUniqueKey, TContext> WithCache<TEntity, TPrimaryKey, TUniqueKey, TContext>(
            this IReadDataRepositoryWithUniqueKeyWithContext<TEntity, TPrimaryKey, TUniqueKey, TContext> @this,
            Func<TEntity, TPrimaryKey> primaryKeyGetter,
            IEntityCache<TPrimaryKey, TEntity> primaryKeyCache,
            Func<TEntity, TUniqueKey> uniqueKeyGetter,
            IEntityCache<TUniqueKey, TEntity> uniqueKeyCache,
            bool disposableCache = false
        )
            where TContext : IDataRepositoryContext
        {
            return new Caching.CacheReadDataRepositoryWithUniqueKeyWithContext<TEntity, TPrimaryKey, TUniqueKey, TContext>(
                @this,
                primaryKeyGetter,
                primaryKeyCache,
                uniqueKeyGetter,
                uniqueKeyCache,
                disposableCache
                );
        }

        /// <summary>
        /// Adds the ability to cache entities.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TPrimaryKey">The primary key type.</typeparam>
        /// <typeparam name="TUniqueKey">The unique key type.</typeparam>
        /// <typeparam name="TContext">The context type.</typeparam>
        /// <param name="this"></param>
        /// <param name="primaryKeyCache">The cache using a primary key.</param>
        /// <param name="uniqueKeyCache">The cache using a unique key.</param>
        /// <param name="disposableCache">A value indicating whether the cache can be dispose.</param>
        /// <returns></returns>
        public static IReadDataRepositoryWithUniqueKeyWithContext<TEntity, TPrimaryKey, TUniqueKey, TContext> WithCache<TEntity, TPrimaryKey, TUniqueKey, TContext>(
            this IReadDataRepositoryWithUniqueKeyWithContext<TEntity, TPrimaryKey, TUniqueKey, TContext> @this,
            IDictionary<TPrimaryKey, TEntity> primaryKeyCache,
            IDictionary<TUniqueKey, TEntity> uniqueKeyCache,
            bool disposableCache = false
        )
            where TEntity : IHasKey<TPrimaryKey>, IHasKey<TUniqueKey>
            where TContext : IDataRepositoryContext
        {
            return new Caching.CacheReadDataRepositoryWithUniqueKeyWithContext<TEntity, TPrimaryKey, TUniqueKey, TContext>(
                @this,
                (entity) => ((IHasKey<TPrimaryKey>)entity).GetKey(),
                CreateCache(primaryKeyCache),
                (entity) => ((IHasKey<TUniqueKey>)entity).GetKey(),
                CreateCache(uniqueKeyCache),
                disposableCache
                );
        }

        /// <summary>
        /// Adds the ability to cache entities.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TPrimaryKey">The primary key type.</typeparam>
        /// <typeparam name="TUniqueKey">The unique key type.</typeparam>
        /// <typeparam name="TContext">The context type.</typeparam>
        /// <param name="this"></param>
        /// <param name="primaryKeyGetter">The method to get the primary key from a spacified entiry.</param>
        /// <param name="primaryKeyCache">The cache using a primary key.</param>
        /// <param name="uniqueKeyGetter">The method to get the unique key from a spacified entiry.</param>
        /// <param name="uniqueKeyCache">The cache using a unique key.</param>
        /// <param name="disposableCache">A value indicating whether the cache can be dispose.</param>
        /// <returns></returns>
        public static IReadDataRepositoryWithUniqueKeyWithContext<TEntity, TPrimaryKey, TUniqueKey, TContext> WithCache<TEntity, TPrimaryKey, TUniqueKey, TContext>(
            this IReadDataRepositoryWithUniqueKeyWithContext<TEntity, TPrimaryKey, TUniqueKey, TContext> @this,
            Func<TEntity, TPrimaryKey> primaryKeyGetter,
            IDictionary<TPrimaryKey, TEntity> primaryKeyCache,
            Func<TEntity, TUniqueKey> uniqueKeyGetter,
            IDictionary<TUniqueKey, TEntity> uniqueKeyCache,
            bool disposableCache = false
        )
            where TContext : IDataRepositoryContext
        {
            return new Caching.CacheReadDataRepositoryWithUniqueKeyWithContext<TEntity, TPrimaryKey, TUniqueKey, TContext>(
                @this,
                primaryKeyGetter,
                CreateCache(primaryKeyCache),
                uniqueKeyGetter,
                CreateCache(uniqueKeyCache),
                disposableCache
                );
        }

        #endregion
    }


}

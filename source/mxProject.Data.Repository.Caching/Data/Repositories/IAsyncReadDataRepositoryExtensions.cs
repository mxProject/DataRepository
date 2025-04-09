using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.Threading;

namespace mxProject.Data.Repositories
{
    /// <summary>
    /// Extension methods for IAsyncReadDataRepository and IAsyncReadDataRepositoryWithContext.
    /// </summary>
    public static class IAsyncReadDataRepositoryExtensions
    {
        #region WithCache

        /// <summary>
        /// Adds the ability to cache entities.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TKey">The key type.</typeparam>
        /// <param name="this"></param>
        /// <returns></returns>
        public static IAsyncReadDataRepository<TEntity, TKey> WithCache<TEntity, TKey>(
            this IAsyncReadDataRepository<TEntity, TKey> @this
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
        public static IAsyncReadDataRepository<TEntity, TKey> WithCache<TEntity, TKey>(
            this IAsyncReadDataRepository<TEntity, TKey> @this,
            Func<TEntity, TKey> keyGetter
        )
        {
            return @this.WithCache(CreateDefaultCache<TKey, TEntity>(), keyGetter, true);
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
        public static IAsyncReadDataRepository<TEntity, TKey> WithCache<TEntity, TKey>(
            this IAsyncReadDataRepository<TEntity, TKey> @this,
            IEntityCache<TKey, TEntity> cache,
            bool disposableCache = false
        )
            where TEntity : IHasKey<TKey>
        {
            return new Caching.CacheAsyncReadDataRepository<TEntity, TKey>(
                @this,
                cache,
                (entity) => entity.GetKey(),
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
        /// <param name="keyGetter">The method to get the key from a spacified entiry.</param>
        /// <param name="disposableCache">A value indicating whether the cache can be dispose.</param>
        /// <returns></returns>
        public static IAsyncReadDataRepository<TEntity, TKey> WithCache<TEntity, TKey>(
            this IAsyncReadDataRepository<TEntity, TKey> @this,
            IEntityCache<TKey, TEntity> cache,
            Func<TEntity, TKey> keyGetter,
            bool disposableCache = false
        )
        {
            return new Caching.CacheAsyncReadDataRepository<TEntity, TKey>(
                @this,
                cache,
                keyGetter,
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
        public static IAsyncReadDataRepository<TEntity, TKey> WithCache<TEntity, TKey>(
            this IAsyncReadDataRepository<TEntity, TKey> @this,
            ConcurrentDictionary<TKey, TEntity> cache,
            bool disposableCache = false
        )
            where TEntity : IHasKey<TKey>
        {
            return new Caching.CacheAsyncReadDataRepository<TEntity, TKey>(
                @this,
                CreateCache(cache),
                (entity) => entity.GetKey(),
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
        /// <param name="keyGetter">The method to get the key from a spacified entiry.</param>
        /// <param name="disposableCache">A value indicating whether the cache can be dispose.</param>
        /// <returns></returns>
        public static IAsyncReadDataRepository<TEntity, TKey> WithCache<TEntity, TKey>(
            this IAsyncReadDataRepository<TEntity, TKey> @this,
            ConcurrentDictionary<TKey, TEntity> cache,
            Func<TEntity, TKey> keyGetter,
            bool disposableCache = false
        )
        {
            return new Caching.CacheAsyncReadDataRepository<TEntity, TKey>(
                @this,
                CreateCache(cache),
                keyGetter,
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
        public static IAsyncReadDataRepository<TEntity, TKey> WithCache<TEntity, TKey>(
            this IAsyncReadDataRepository<TEntity, TKey> @this,
            IDictionary<TKey, TEntity> cache,
            bool disposableCache = false
        )
            where TEntity : IHasKey<TKey>
        {
            return new Caching.CacheAsyncReadDataRepository<TEntity, TKey>(
                @this,
                CreateCache(cache),
                (entity) => entity.GetKey(),
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
        /// <param name="keyGetter">The method to get the key from a spacified entiry.</param>
        /// <param name="disposableCache">A value indicating whether the cache can be dispose.</param>
        /// <returns></returns>
        public static IAsyncReadDataRepository<TEntity, TKey> WithCache<TEntity, TKey>(
            this IAsyncReadDataRepository<TEntity, TKey> @this,
            IDictionary<TKey, TEntity> cache,
            Func<TEntity, TKey> keyGetter,
            bool disposableCache = false
        )
        {
            return new Caching.CacheAsyncReadDataRepository<TEntity, TKey>(
                @this,
                CreateCache(cache),
                keyGetter,
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
            return new Caching.EntityCacheConcurrentDictionary<TKey, TEntity>(new ConcurrentDictionary<TKey, TEntity>());
        }

        /// <summary>
        /// Creates a default entity cache.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        private static IEntityCache<TKey, TEntity> CreateCache<TEntity, TKey>(IDictionary<TKey, TEntity> dictionary)
        {
            if (dictionary is ConcurrentDictionary<TKey, TEntity> concurrent)
            {
                return new Caching.EntityCacheConcurrentDictionary<TKey, TEntity>(concurrent);
            }
            else
            {
                return new Caching.EntityCacheDictionary<TKey, TEntity>(dictionary);
            }
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
        public static IAsyncReadDataRepositoryWithContext<TEntity, TKey, TContext> WithCache<TEntity, TKey, TContext>(
            this IAsyncReadDataRepositoryWithContext<TEntity, TKey, TContext> @this
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
        public static IAsyncReadDataRepositoryWithContext<TEntity, TKey, TContext> WithCache<TEntity, TKey, TContext>(
            this IAsyncReadDataRepositoryWithContext<TEntity, TKey, TContext> @this,
            Func<TEntity, TKey> keyGetter
        )
            where TContext : IDataRepositoryContext
        {
            return @this.WithCache(CreateDefaultCache<TKey, TEntity>(), keyGetter, true);
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
        public static IAsyncReadDataRepositoryWithContext<TEntity, TKey, TContext> WithCache<TEntity, TKey, TContext>(
            this IAsyncReadDataRepositoryWithContext<TEntity, TKey, TContext> @this,
            IEntityCache<TKey, TEntity> cache,
            bool disposableCache = false
        )
            where TEntity : IHasKey<TKey>
            where TContext : IDataRepositoryContext
        {
            return new Caching.CacheAsyncReadDataRepositoryWithContext<TEntity, TKey, TContext>(
                @this,
                cache,
                (entity) => entity.GetKey(),
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
        /// <param name="keyGetter">The method to get the key from a spacified entiry.</param>
        /// <param name="disposableCache">A value indicating whether the cache can be dispose.</param>
        /// <returns></returns>
        public static IAsyncReadDataRepositoryWithContext<TEntity, TKey, TContext> WithCache<TEntity, TKey, TContext>(
            this IAsyncReadDataRepositoryWithContext<TEntity, TKey, TContext> @this,
            IEntityCache<TKey, TEntity> cache,
            Func<TEntity, TKey> keyGetter,
            bool disposableCache = false
        )
            where TContext : IDataRepositoryContext
        {
            return new Caching.CacheAsyncReadDataRepositoryWithContext<TEntity, TKey, TContext>(
                @this,
                cache,
                keyGetter,
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
        public static IAsyncReadDataRepositoryWithContext<TEntity, TKey, TContext> WithCache<TEntity, TKey, TContext>(
            this IAsyncReadDataRepositoryWithContext<TEntity, TKey, TContext> @this,
            IDictionary<TKey, TEntity> cache,
            bool disposableCache = false
        )
            where TEntity : IHasKey<TKey>
            where TContext : IDataRepositoryContext
        {
            return new Caching.CacheAsyncReadDataRepositoryWithContext<TEntity, TKey, TContext>(
                @this,
                CreateCache(cache),
                (entity) => entity.GetKey(),
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
        /// <param name="keyGetter">The method to get the key from a spacified entiry.</param>
        /// <param name="disposableCache">A value indicating whether the cache can be dispose.</param>
        /// <returns></returns>
        public static IAsyncReadDataRepositoryWithContext<TEntity, TKey, TContext> WithCache<TEntity, TKey, TContext>(
            this IAsyncReadDataRepositoryWithContext<TEntity, TKey, TContext> @this,
            IDictionary<TKey, TEntity> cache,
            Func<TEntity, TKey> keyGetter,
            bool disposableCache = false
        )
            where TContext : IDataRepositoryContext
        {
            return new Caching.CacheAsyncReadDataRepositoryWithContext<TEntity, TKey, TContext>(
                @this,
                CreateCache(cache),
                keyGetter,
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
        public static IAsyncReadDataRepositoryWithUniqueKey<TEntity, TPrimaryKey, TUniqueKey> WithCache<TEntity, TPrimaryKey, TUniqueKey>(
            this IAsyncReadDataRepositoryWithUniqueKey<TEntity, TPrimaryKey, TUniqueKey> @this
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
        public static IAsyncReadDataRepositoryWithUniqueKey<TEntity, TPrimaryKey, TUniqueKey> WithCache<TEntity, TPrimaryKey, TUniqueKey>(
            this IAsyncReadDataRepositoryWithUniqueKey<TEntity, TPrimaryKey, TUniqueKey> @this,
            Func<TEntity, TPrimaryKey> primaryKeyGetter,
            Func<TEntity, TUniqueKey> uniqueKeyGetter
        )
        {
            return @this.WithCache(
                CreateDefaultCache<TPrimaryKey, TEntity>(),
                primaryKeyGetter,
                CreateDefaultCache<TUniqueKey, TEntity>(),
                uniqueKeyGetter,
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
        public static IAsyncReadDataRepositoryWithUniqueKey<TEntity, TPrimaryKey, TUniqueKey> WithCache<TEntity, TPrimaryKey, TUniqueKey>(
            this IAsyncReadDataRepositoryWithUniqueKey<TEntity, TPrimaryKey, TUniqueKey> @this,
            IEntityCache<TPrimaryKey, TEntity> primaryKeyCache,
            IEntityCache<TUniqueKey, TEntity> uniqueKeyCache,
            bool disposableCache = false
        )
            where TEntity : IHasKey<TPrimaryKey>, IHasKey<TUniqueKey>
        {
            return new Caching.CacheAsyncReadDataRepositoryWithUniqueKey<TEntity, TPrimaryKey, TUniqueKey>(
                @this,
                primaryKeyCache,
                (entity) => ((IHasKey<TPrimaryKey>)entity).GetKey(),
                uniqueKeyCache,
                (entity) => ((IHasKey<TUniqueKey>)entity).GetKey(),
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
        /// <param name="primaryKeyGetter">The method to get the primary key from a spacified entiry.</param>
        /// <param name="uniqueKeyCache">The cache using a unique key.</param>
        /// <param name="uniqueKeyGetter">The method to get the unique key from a spacified entiry.</param>
        /// <param name="disposableCache">A value indicating whether the cache can be dispose.</param>
        /// <returns></returns>
        public static IAsyncReadDataRepositoryWithUniqueKey<TEntity, TPrimaryKey, TUniqueKey> WithCache<TEntity, TPrimaryKey, TUniqueKey>(
            this IAsyncReadDataRepositoryWithUniqueKey<TEntity, TPrimaryKey, TUniqueKey> @this,
            IEntityCache<TPrimaryKey, TEntity> primaryKeyCache,
            Func<TEntity, TPrimaryKey> primaryKeyGetter,
            IEntityCache<TUniqueKey, TEntity> uniqueKeyCache,
            Func<TEntity, TUniqueKey> uniqueKeyGetter,
            bool disposableCache = false
        )
        {
            return new Caching.CacheAsyncReadDataRepositoryWithUniqueKey<TEntity, TPrimaryKey, TUniqueKey>(
                @this,
                primaryKeyCache,
                primaryKeyGetter,
                uniqueKeyCache,
                uniqueKeyGetter,
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
        public static IAsyncReadDataRepositoryWithUniqueKey<TEntity, TPrimaryKey, TUniqueKey> WithCache<TEntity, TPrimaryKey, TUniqueKey>(
            this IAsyncReadDataRepositoryWithUniqueKey<TEntity, TPrimaryKey, TUniqueKey> @this,
            IDictionary<TPrimaryKey, TEntity> primaryKeyCache,
            IDictionary<TUniqueKey, TEntity> uniqueKeyCache,
            bool disposableCache = false
        )
            where TEntity : IHasKey<TPrimaryKey>, IHasKey<TUniqueKey>
        {
            return new Caching.CacheAsyncReadDataRepositoryWithUniqueKey<TEntity, TPrimaryKey, TUniqueKey>(
                @this,
                CreateCache(primaryKeyCache),
                (entity) => ((IHasKey<TPrimaryKey>)entity).GetKey(),
                CreateCache(uniqueKeyCache),
                (entity) => ((IHasKey<TUniqueKey>)entity).GetKey(),
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
        /// <param name="primaryKeyGetter">The method to get the primary key from a spacified entiry.</param>
        /// <param name="uniqueKeyCache">The cache using a unique key.</param>
        /// <param name="uniqueKeyGetter">The method to get the unique key from a spacified entiry.</param>
        /// <param name="disposableCache">A value indicating whether the cache can be dispose.</param>
        /// <returns></returns>
        public static IAsyncReadDataRepositoryWithUniqueKey<TEntity, TPrimaryKey, TUniqueKey> WithCache<TEntity, TPrimaryKey, TUniqueKey>(
            this IAsyncReadDataRepositoryWithUniqueKey<TEntity, TPrimaryKey, TUniqueKey> @this,
            IDictionary<TPrimaryKey, TEntity> primaryKeyCache,
            Func<TEntity, TPrimaryKey> primaryKeyGetter,
            IDictionary<TUniqueKey, TEntity> uniqueKeyCache,
            Func<TEntity, TUniqueKey> uniqueKeyGetter,
            bool disposableCache = false
        )
        {
            return new Caching.CacheAsyncReadDataRepositoryWithUniqueKey<TEntity, TPrimaryKey, TUniqueKey>(
                @this,
                CreateCache(primaryKeyCache),
                primaryKeyGetter,
                CreateCache(uniqueKeyCache),
                uniqueKeyGetter,
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
        public static IAsyncReadDataRepositoryWithUniqueKeyWithContext<TEntity, TPrimaryKey, TUniqueKey, TContext> WithCache<TEntity, TPrimaryKey, TUniqueKey, TContext>(
            this IAsyncReadDataRepositoryWithUniqueKeyWithContext<TEntity, TPrimaryKey, TUniqueKey, TContext> @this
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
        public static IAsyncReadDataRepositoryWithUniqueKeyWithContext<TEntity, TPrimaryKey, TUniqueKey, TContext> WithCache<TEntity, TPrimaryKey, TUniqueKey, TContext>(
            this IAsyncReadDataRepositoryWithUniqueKeyWithContext<TEntity, TPrimaryKey, TUniqueKey, TContext> @this,
            Func<TEntity, TPrimaryKey> primaryKeyGetter,
            Func<TEntity, TUniqueKey> uniqueKeyGetter
        )
            where TContext : IDataRepositoryContext
        {
            return @this.WithCache(
                CreateDefaultCache<TPrimaryKey, TEntity>(),
                primaryKeyGetter,
                CreateDefaultCache<TUniqueKey, TEntity>(),
                uniqueKeyGetter,
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
        public static IAsyncReadDataRepositoryWithUniqueKeyWithContext<TEntity, TPrimaryKey, TUniqueKey, TContext> WithCache<TEntity, TPrimaryKey, TUniqueKey, TContext>(
            this IAsyncReadDataRepositoryWithUniqueKeyWithContext<TEntity, TPrimaryKey, TUniqueKey, TContext> @this,
            IEntityCache<TPrimaryKey, TEntity> primaryKeyCache,
            IEntityCache<TUniqueKey, TEntity> uniqueKeyCache,
            bool disposableCache = false
        )
            where TEntity : IHasKey<TPrimaryKey>, IHasKey<TUniqueKey>
            where TContext : IDataRepositoryContext
        {
            return new Caching.CacheAsyncReadDataRepositoryWithUniqueKeyWithContext<TEntity, TPrimaryKey, TUniqueKey, TContext>(
                @this,
                primaryKeyCache,
                (entity) => ((IHasKey<TPrimaryKey>)entity).GetKey(),
                uniqueKeyCache,
                (entity) => ((IHasKey<TUniqueKey>)entity).GetKey(),
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
        /// <param name="primaryKeyGetter">The method to get the primary key from a spacified entiry.</param>
        /// <param name="uniqueKeyCache">The cache using a unique key.</param>
        /// <param name="uniqueKeyGetter">The method to get the unique key from a spacified entiry.</param>
        /// <param name="disposableCache">A value indicating whether the cache can be dispose.</param>
        /// <returns></returns>
        public static IAsyncReadDataRepositoryWithUniqueKeyWithContext<TEntity, TPrimaryKey, TUniqueKey, TContext> WithCache<TEntity, TPrimaryKey, TUniqueKey, TContext>(
            this IAsyncReadDataRepositoryWithUniqueKeyWithContext<TEntity, TPrimaryKey, TUniqueKey, TContext> @this,
            IEntityCache<TPrimaryKey, TEntity> primaryKeyCache,
            Func<TEntity, TPrimaryKey> primaryKeyGetter,
            IEntityCache<TUniqueKey, TEntity> uniqueKeyCache,
            Func<TEntity, TUniqueKey> uniqueKeyGetter,
            bool disposableCache = false
        )
            where TContext : IDataRepositoryContext
        {
            return new Caching.CacheAsyncReadDataRepositoryWithUniqueKeyWithContext<TEntity, TPrimaryKey, TUniqueKey, TContext>(
                @this,
                primaryKeyCache,
                primaryKeyGetter,
                uniqueKeyCache,
                uniqueKeyGetter,
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
        public static IAsyncReadDataRepositoryWithUniqueKeyWithContext<TEntity, TPrimaryKey, TUniqueKey, TContext> WithCache<TEntity, TPrimaryKey, TUniqueKey, TContext>(
            this IAsyncReadDataRepositoryWithUniqueKeyWithContext<TEntity, TPrimaryKey, TUniqueKey, TContext> @this,
            IDictionary<TPrimaryKey, TEntity> primaryKeyCache,
            IDictionary<TUniqueKey, TEntity> uniqueKeyCache,
            bool disposableCache = false
        )
            where TEntity : IHasKey<TPrimaryKey>, IHasKey<TUniqueKey>
            where TContext : IDataRepositoryContext
        {
            return new Caching.CacheAsyncReadDataRepositoryWithUniqueKeyWithContext<TEntity, TPrimaryKey, TUniqueKey, TContext>(
                @this,
                CreateCache(primaryKeyCache),
                (entity) => ((IHasKey<TPrimaryKey>)entity).GetKey(),
                CreateCache(uniqueKeyCache),
                (entity) => ((IHasKey<TUniqueKey>)entity).GetKey(),
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
        /// <param name="primaryKeyGetter">The method to get the primary key from a spacified entiry.</param>
        /// <param name="uniqueKeyCache">The cache using a unique key.</param>
        /// <param name="uniqueKeyGetter">The method to get the unique key from a spacified entiry.</param>
        /// <param name="disposableCache">A value indicating whether the cache can be dispose.</param>
        /// <returns></returns>
        public static IAsyncReadDataRepositoryWithUniqueKeyWithContext<TEntity, TPrimaryKey, TUniqueKey, TContext> WithCache<TEntity, TPrimaryKey, TUniqueKey, TContext>(
            this IAsyncReadDataRepositoryWithUniqueKeyWithContext<TEntity, TPrimaryKey, TUniqueKey, TContext> @this,
            IDictionary<TPrimaryKey, TEntity> primaryKeyCache,
            Func<TEntity, TPrimaryKey> primaryKeyGetter,
            IDictionary<TUniqueKey, TEntity> uniqueKeyCache,
            Func<TEntity, TUniqueKey> uniqueKeyGetter,
            bool disposableCache = false
        )
            where TContext : IDataRepositoryContext
        {
            return new Caching.CacheAsyncReadDataRepositoryWithUniqueKeyWithContext<TEntity, TPrimaryKey, TUniqueKey, TContext>(
                @this,
                CreateCache(primaryKeyCache),
                primaryKeyGetter,
                CreateCache(uniqueKeyCache),
                uniqueKeyGetter,
                disposableCache
                );
        }

        #endregion
    }


}

using mxProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace mxProject.Data.Repositories.Caching
{
    /// <summary>
    /// Data repository with the functionality required for caching entities. Cache retrieved entities.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TPrimaryKey">The primary key type.</typeparam>
    /// <typeparam name="TUniqueKey">The unique key type.</typeparam>
    internal class CacheReadDataRepositoryWithUniqueKey<TEntity, TPrimaryKey, TUniqueKey> : IReadDataRepositoryWithUniqueKey<TEntity, TPrimaryKey, TUniqueKey>
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="repository">The repogitory.</param>
        /// <param name="primaryKeyGetter">The method to get the primary key from a spacified entiry.</param>
        /// <param name="primaryCache">The cache using primary keys.</param>
        /// <param name="uniqueKeyGetter">The method to get the unique key from a spacified entiry.</param>
        /// <param name="uniqueCache">The cache using unique keys.</param>
        /// <param name="disposableCache">A value indicating whether the cache can be dispose.</param>
        internal CacheReadDataRepositoryWithUniqueKey(
            IReadDataRepositoryWithUniqueKey<TEntity, TPrimaryKey, TUniqueKey> repository,
            Func<TEntity, TPrimaryKey> primaryKeyGetter,
            IEntityCache<TPrimaryKey, TEntity> primaryCache,
            Func<TEntity, TUniqueKey> uniqueKeyGetter,
            IEntityCache<TUniqueKey, TEntity> uniqueCache,
            bool disposableCache
            )
        {
            m_Repository = repository;
            m_PrimaryCache = primaryCache;
            m_PrimaryKeyGetter = primaryKeyGetter;
            m_UniqueCache = uniqueCache;
            m_UniqueKeyGetter = uniqueKeyGetter;
            m_DisposableCache = disposableCache;

            m_EntityChangedSubscriberWithPrimaryKey = DataRepositoryCachingService.CreateEntityChangedSubscriber<TPrimaryKey, TEntity>((key, entity) =>
            {
                m_PrimaryCache.Remove(key);

                var uniqueKey = m_UniqueKeyGetter(entity);

                if (uniqueKey != null)
                {
                    m_UniqueCache.Remove(uniqueKey);
                }
            });

            m_EntityChangedSubscriberWithUniqueKey = DataRepositoryCachingService.CreateEntityChangedSubscriber<TUniqueKey, TEntity>((key, entity) =>
            {
                m_UniqueCache.Remove(key);

                var primaryKey = m_PrimaryKeyGetter(entity);

                if (primaryKey != null)
                {
                    m_PrimaryCache.Remove(primaryKey);
                }
            });
        }

        private readonly IReadDataRepositoryWithUniqueKey<TEntity, TPrimaryKey, TUniqueKey> m_Repository;
        private readonly IEntityCache<TPrimaryKey, TEntity> m_PrimaryCache;
        private readonly Func<TEntity, TPrimaryKey> m_PrimaryKeyGetter;
        private readonly IEntityCache<TUniqueKey, TEntity> m_UniqueCache;
        private readonly Func<TEntity, TUniqueKey> m_UniqueKeyGetter;
        private readonly bool m_DisposableCache;

        private readonly IDisposable m_EntityChangedSubscriberWithPrimaryKey;
        private readonly IDisposable m_EntityChangedSubscriberWithUniqueKey;

        /// <inheritdoc/>
        public bool UseTransactionScope => m_Repository.UseTransactionScope;

        /// <inheritdoc/>
        public void Dispose()
        {
            m_Repository.Dispose();

            if (m_DisposableCache)
            {
                m_PrimaryCache.Dispose();
                m_UniqueCache.Dispose();
            }

            m_EntityChangedSubscriberWithPrimaryKey.Dispose();
            m_EntityChangedSubscriberWithUniqueKey.Dispose();
        }

        /// <inheritdoc/>
        public TEntity? GetByPrimaryKey(TPrimaryKey key)
        {
            if (m_PrimaryCache.TryGet(key, out var entity)) { return entity; }

            lock (m_Repository)
            {
                if (m_PrimaryCache.TryGet(key, out entity)) { return entity; }

                entity = m_Repository.GetByPrimaryKey(key);

                if (entity != null)
                {
                    StoreIntoPrimaryCache(key, entity);
                    StoreIntoUniqueCache(m_UniqueKeyGetter(entity), entity);
                }

                return entity;
            }
        }

        /// <inheritdoc/>
        public TEntity? GetByUniqueKey(TUniqueKey key)
        {
            if (m_UniqueCache.TryGet(key, out var entity)) { return entity; }

            lock (m_Repository)
            {
                if (m_UniqueCache.TryGet(key, out entity)) { return entity; }

                entity = m_Repository.GetByUniqueKey(key);

                if (entity != null)
                {
                    StoreIntoPrimaryCache(m_PrimaryKeyGetter(entity), entity);
                    StoreIntoUniqueCache(key, entity);
                }

                return entity;
            }
        }

        /// <inheritdoc/>
        public IEnumerable<TEntity> GetRangeByPrimaryKey(IEnumerable<TPrimaryKey> keys)
        {
            List<TPrimaryKey> unfoundKeys = new();

            foreach (var key in keys)
            {
                if (m_PrimaryCache.TryGet(key, out var entity))
                {
                    yield return entity;
                }
                else
                {
                    unfoundKeys.Add(key);
                }
            }

            foreach (var entity in m_Repository.GetRangeByPrimaryKey(unfoundKeys))
            {
                StoreIntoPrimaryCache(m_PrimaryKeyGetter(entity), entity);
                StoreIntoUniqueCache(m_UniqueKeyGetter(entity), entity);

                yield return entity;
            }
        }

        /// <inheritdoc/>
        public IEnumerable<TEntity> GetRangeByUniqueKey(IEnumerable<TUniqueKey> keys)
        {
            List<TUniqueKey> unfoundKeys = new();

            foreach (var key in keys)
            {
                if (m_UniqueCache.TryGet(key, out var entity))
                {
                    yield return entity;
                }
                else
                {
                    unfoundKeys.Add(key);
                }
            }

            foreach (var entity in m_Repository.GetRangeByUniqueKey(unfoundKeys))
            {
                StoreIntoPrimaryCache(m_PrimaryKeyGetter(entity), entity);
                StoreIntoUniqueCache(m_UniqueKeyGetter(entity), entity);

                yield return entity;
            }
        }

        /// <inheritdoc/>
        public IEnumerable<TEntity> GetAll()
        {
            foreach (var entity in m_Repository.GetAll())
            {
                StoreIntoPrimaryCache(m_PrimaryKeyGetter(entity), entity);
                StoreIntoUniqueCache(m_UniqueKeyGetter(entity), entity);

                yield return entity;
            }
        }

        /// <inheritdoc/>
        public IEnumerable<TPrimaryKey> GetAllPrimaryKeys()
        {
            return m_Repository.GetAllPrimaryKeys();
        }

        /// <inheritdoc/>
        public IEnumerable<TUniqueKey> GetAllUniqueKeys()
        {
            return m_Repository.GetAllUniqueKeys();
        }

        /// <summary>
        /// Stores the specified entity in the cache.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="entity"></param>
        private void StoreIntoPrimaryCache(TPrimaryKey key, TEntity entity)
        {
            if (key == null) { return; }

            m_PrimaryCache.Store(key, entity);
        }

        /// <summary>
        /// Stores the specified entity in the cache.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="entity"></param>
        private void StoreIntoUniqueCache(TUniqueKey key, TEntity entity)
        {
            if (key == null) { return; }

            m_UniqueCache.Store(key, entity);
        }
    }
}

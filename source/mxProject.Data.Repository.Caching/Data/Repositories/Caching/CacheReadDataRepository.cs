using MessagePipe;
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
    /// <typeparam name="TKey">The key type.</typeparam>
    internal class CacheReadDataRepository<TEntity, TKey> : IReadDataRepository<TEntity, TKey>
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="repository">The repogitory.</param>
        /// <param name="keyGetter">The method to get the key from a spacified entiry.</param>
        /// <param name="cache">The cache.</param>
        /// <param name="disposableCache">A value indicating whether the cache can be dispose.</param>
        internal CacheReadDataRepository(
            IReadDataRepository<TEntity, TKey> repository,
            Func<TEntity, TKey> keyGetter,
            IEntityCache<TKey, TEntity> cache,
            bool disposableCache
            )
        {
            m_Repository = repository;
            m_Cache = cache;
            m_KeyGetter = keyGetter;
            m_DisposableCache = disposableCache;

            m_EntityChangedSubscriber = DataRepositoryCachingService.CreateEntityChangedSubscriber<TKey, TEntity>((key, entity) =>
            {
                m_Cache.Remove(key);
            });
        }

        private readonly IReadDataRepository<TEntity, TKey> m_Repository;
        private readonly IEntityCache<TKey, TEntity> m_Cache;
        private readonly Func<TEntity, TKey> m_KeyGetter;
        private readonly bool m_DisposableCache;

        private readonly IDisposable m_EntityChangedSubscriber;

        /// <inheritdoc/>
        public bool UseTransactionScope => m_Repository.UseTransactionScope;

        /// <inheritdoc/>
        public void Dispose()
        {
            m_Repository.Dispose();

            if (m_DisposableCache)
            {
                m_Cache.Dispose();
            }

            m_EntityChangedSubscriber.Dispose();
        }

        /// <inheritdoc/>
        public TEntity? Get(TKey key)
        {
            if (m_Cache.TryGet(key, out var entity)) { return entity; }

            lock (m_Repository)
            {
                if (m_Cache.TryGet(key, out entity)) { return entity; }

                entity = m_Repository.Get(key);

                if (entity != null)
                {
                    StoreIntoCache(key, entity);
                }

                return entity;
            }
        }

        /// <inheritdoc/>
        public IEnumerable<TEntity> GetRange(IEnumerable<TKey> keys)
        {
            List<TKey> unfoundKeys = new();

            foreach (var key in keys)
            {
                if (m_Cache.TryGet(key, out var entity))
                {
                    yield return entity;
                }
                else
                {
                    unfoundKeys.Add(key);
                }
            }

            foreach (var entity in m_Repository.GetRange(unfoundKeys))
            {
                StoreIntoCache(m_KeyGetter(entity), entity);

                yield return entity;
            }
        }

        /// <inheritdoc/>
        public IEnumerable<TEntity> GetAll()
        {
            foreach (var entity in m_Repository.GetAll())
            {
                StoreIntoCache(m_KeyGetter(entity), entity);

                yield return entity;
            }
        }

        /// <inheritdoc/>
        public IEnumerable<TKey> GetAllKeys()
        {
            return m_Repository.GetAllKeys();
        }

        /// <summary>
        /// Stores the specified entity in the cache.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="entity"></param>
        private void StoreIntoCache(TKey key, TEntity entity)
        {
            if (key == null) { return; }

            m_Cache.Store(key, entity);
        }
    }
}

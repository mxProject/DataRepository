using mxProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using static mxProject.Data.Repositories.Caching.DataRepositoryCachingService;

namespace mxProject.Data.Repositories.Caching
{
    /// <summary>
    /// Data repository with the functionality required for caching entities. Cache retrieved entities.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TKey">The key type.</typeparam>
    /// <typeparam name="TContext">The context type.</typeparam>
    internal class CacheAsyncReadDataRepositoryWithContext<TEntity, TKey, TContext> : IAsyncReadDataRepositoryWithContext<TEntity, TKey, TContext>
        where TContext : IDataRepositoryContext
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="repository">The repogitory.</param>
        /// <param name="cache">The cache.</param>
        /// <param name="keyGetter">The method to get the key from a spacified entiry.</param>
        /// <param name="disposableCache">A value indicating whether the cache can be dispose.</param>
        internal CacheAsyncReadDataRepositoryWithContext(
            IAsyncReadDataRepositoryWithContext<TEntity, TKey, TContext> repository,
            IEntityCache<TKey, TEntity> cache,
            Func<TEntity, TKey> keyGetter,
            bool disposableCache
            )
        {
            m_Repository = repository;
            m_Cache = cache;
            m_KeyGetter = keyGetter;
            m_DisposableCache = disposableCache;

            m_EntityChangedSubscriber = CreateEntityChangedSubscriber<TKey, TEntity>((key, entity) =>
            {
                m_Cache.Remove(key);
            });
        }

        private readonly IAsyncReadDataRepositoryWithContext<TEntity, TKey, TContext> m_Repository;
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
        public async ValueTask<TEntity?> GetAsync(TKey key, TContext context)
        {
            if (m_Cache.TryGet(key, out var entity)) { return entity; }

            entity = await m_Repository.GetAsync(key, context).ConfigureAwait(false);

            if (entity != null)
            {
                StoreIntoCache(key, entity);
            }

            return entity;
        }

        /// <inheritdoc/>
        public async IAsyncEnumerable<TEntity> GetRangeAsync(IEnumerable<TKey> keys, TContext context, [EnumeratorCancellation] CancellationToken cancellationToken = default)
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

            await foreach (var entity in m_Repository.GetRangeAsync(unfoundKeys, context, cancellationToken).ConfigureAwait(false))
            {
                StoreIntoCache(m_KeyGetter(entity), entity);

                yield return entity;
            }
        }

        /// <inheritdoc/>
        public async IAsyncEnumerable<TEntity> GetRangeAsync(IAsyncEnumerable<TKey> keys, TContext context, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            List<TKey> unfoundKeys = new();

            await foreach (var key in keys.ConfigureAwait(false))
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

            await foreach (var entity in m_Repository.GetRangeAsync(unfoundKeys, context, cancellationToken).ConfigureAwait(false))
            {
                StoreIntoCache(m_KeyGetter(entity), entity);

                yield return entity;
            }
        }
        /// <inheritdoc/>
        public async IAsyncEnumerable<TEntity> GetAllAsync(TContext context, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await foreach (var entity in m_Repository.GetAllAsync(context, cancellationToken).ConfigureAwait(false))
            {
                StoreIntoCache(m_KeyGetter(entity), entity);

                yield return entity;
            }
        }

        /// <inheritdoc/>
        public IAsyncEnumerable<TKey> GetAllKeysAsync(TContext context, CancellationToken cancellationToken = default)
        {
            return m_Repository.GetAllKeysAsync(context, cancellationToken);
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

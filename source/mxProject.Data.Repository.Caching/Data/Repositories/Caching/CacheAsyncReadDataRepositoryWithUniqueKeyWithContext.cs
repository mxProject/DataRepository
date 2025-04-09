﻿using mxProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace mxProject.Data.Repositories.Caching
{
    /// <summary>
    /// Data repository with the functionality required for caching entities. Cache retrieved entities.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TPrimaryKey">The primary key type.</typeparam>
    /// <typeparam name="TUniqueKey">The unique key type.</typeparam>
    /// <typeparam name="TContext">The context type.</typeparam>
    internal class CacheAsyncReadDataRepositoryWithUniqueKeyWithContext<TEntity, TPrimaryKey, TUniqueKey, TContext> : IAsyncReadDataRepositoryWithUniqueKeyWithContext<TEntity, TPrimaryKey, TUniqueKey, TContext>
        where TContext : IDataRepositoryContext
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="repository">The repogitory.</param>
        /// <param name="primaryCache">The cache using primary keys.</param>
        /// <param name="primaryKeyGetter">The method to get the primary key from a spacified entiry.</param>
        /// <param name="uniqueCache">The cache using unique keys.</param>
        /// <param name="uniqueKeyGetter">The method to get the unique key from a spacified entiry.</param>
        /// <param name="disposableCache">A value indicating whether the cache can be dispose.</param>
        internal CacheAsyncReadDataRepositoryWithUniqueKeyWithContext(
            IAsyncReadDataRepositoryWithUniqueKeyWithContext<TEntity, TPrimaryKey, TUniqueKey, TContext> repository,
            IEntityCache<TPrimaryKey, TEntity> primaryCache,
            Func<TEntity, TPrimaryKey> primaryKeyGetter,
            IEntityCache<TUniqueKey, TEntity> uniqueCache,
            Func<TEntity, TUniqueKey> uniqueKeyGetter,
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

        private readonly IAsyncReadDataRepositoryWithUniqueKeyWithContext<TEntity, TPrimaryKey, TUniqueKey, TContext> m_Repository;
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
        public async ValueTask<TEntity?> GetByPrimaryKeyAsync(TPrimaryKey key, TContext context)
        {
            if (m_PrimaryCache.TryGet(key, out var entity)) { return entity; }

            entity = await m_Repository.GetByPrimaryKeyAsync(key, context).ConfigureAwait(false);

            if (entity != null)
            {
                StoreIntoPrimaryCache(key, entity);
                StoreIntoUniqueCache(m_UniqueKeyGetter(entity), entity);
            }

            return entity;
        }

        /// <inheritdoc/>
        public async ValueTask<TEntity?> GetByUniqueKeyAsync(TUniqueKey key, TContext context)
        {
            if (m_UniqueCache.TryGet(key, out var entity)) { return entity; }

            entity = await m_Repository.GetByUniqueKeyAsync(key, context).ConfigureAwait(false);

            if (entity != null)
            {
                StoreIntoPrimaryCache(m_PrimaryKeyGetter(entity), entity);
                StoreIntoUniqueCache(key, entity);
            }

            return entity;
        }

        /// <inheritdoc/>
        public async IAsyncEnumerable<TEntity> GetRangeByPrimaryKeyAsync(IEnumerable<TPrimaryKey> keys, TContext context, [EnumeratorCancellation] CancellationToken cancellationToken = default)
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

            await foreach (var entity in m_Repository.GetRangeByPrimaryKeyAsync(unfoundKeys, context, cancellationToken).ConfigureAwait(false))
            {
                StoreIntoPrimaryCache(m_PrimaryKeyGetter(entity), entity);
                StoreIntoUniqueCache(m_UniqueKeyGetter(entity), entity);

                yield return entity;
            }
        }

        /// <inheritdoc/>
        public async IAsyncEnumerable<TEntity> GetRangeByPrimaryKeyAsync(IAsyncEnumerable<TPrimaryKey> keys, TContext context, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            List<TPrimaryKey> unfoundKeys = new();

            await foreach (var key in keys.ConfigureAwait(false))
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

            await foreach (var entity in m_Repository.GetRangeByPrimaryKeyAsync(unfoundKeys, context, cancellationToken).ConfigureAwait(false))
            {
                StoreIntoPrimaryCache(m_PrimaryKeyGetter(entity), entity);
                StoreIntoUniqueCache(m_UniqueKeyGetter(entity), entity);

                yield return entity;
            }
        }

        /// <inheritdoc/>
        public async IAsyncEnumerable<TEntity> GetRangeByUniqueKeyAsync(IEnumerable<TUniqueKey> keys, TContext context, [EnumeratorCancellation] CancellationToken cancellationToken = default)
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

            await foreach (var entity in m_Repository.GetRangeByUniqueKeyAsync(unfoundKeys, context, cancellationToken).ConfigureAwait(false))
            {
                StoreIntoPrimaryCache(m_PrimaryKeyGetter(entity), entity);
                StoreIntoUniqueCache(m_UniqueKeyGetter(entity), entity);

                yield return entity;
            }
        }

        /// <inheritdoc/>
        public async IAsyncEnumerable<TEntity> GetRangeByUniqueKeyAsync(IAsyncEnumerable<TUniqueKey> keys, TContext context, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            List<TUniqueKey> unfoundKeys = new();

            await foreach (var key in keys.ConfigureAwait(false))
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

            await foreach (var entity in m_Repository.GetRangeByUniqueKeyAsync(unfoundKeys, context, cancellationToken).ConfigureAwait(false))
            {
                StoreIntoPrimaryCache(m_PrimaryKeyGetter(entity), entity);
                StoreIntoUniqueCache(m_UniqueKeyGetter(entity), entity);

                yield return entity;
            }
        }

        /// <inheritdoc/>
        public async IAsyncEnumerable<TEntity> GetAllAsync(TContext context, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await foreach (var entity in m_Repository.GetAllAsync(context, cancellationToken).ConfigureAwait(false))
            {
                StoreIntoPrimaryCache(m_PrimaryKeyGetter(entity), entity);
                StoreIntoUniqueCache(m_UniqueKeyGetter(entity), entity);

                yield return entity;
            }
        }

        /// <inheritdoc/>
        public IAsyncEnumerable<TPrimaryKey> GetAllPrimaryKeysAsync(TContext context, CancellationToken cancellationToken = default)
        {
            return m_Repository.GetAllPrimaryKeysAsync(context, cancellationToken);
        }

        /// <inheritdoc/>
        public IAsyncEnumerable<TUniqueKey> GetAllUniqueKeysAsync(TContext context,CancellationToken cancellationToken = default)
        {
            return m_Repository.GetAllUniqueKeysAsync(context, cancellationToken);
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

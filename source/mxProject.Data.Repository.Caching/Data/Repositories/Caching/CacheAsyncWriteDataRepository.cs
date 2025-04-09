using MessagePipe;
using mxProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace mxProject.Data.Repositories.Caching
{
    /// <summary>
    /// Data repository with the functionality required for caching entities. Notifies when an entity is changed.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TKey">The key type.</typeparam>
    internal class CacheAsyncWriteDataRepository<TEntity, TKey> : IAsyncWriteDataRepository<TEntity>
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="keyGetter">The method to get the key from a spacified entiry.</param>
        internal CacheAsyncWriteDataRepository(
            IAsyncWriteDataRepository<TEntity> repository,
            Func<TEntity, TKey> keyGetter
            )
        {
            m_Repository = repository;
            m_KeyGetter = keyGetter;

            m_EntityChangedPublisher = DataRepositoryCachingService.CreateEntityChangedPublisher<TKey, TEntity>();
        }

        private readonly IAsyncWriteDataRepository<TEntity> m_Repository;
        private readonly Func<TEntity, TKey> m_KeyGetter;

        private readonly DataRepositoryCachingService.EntityChangedPublisher<TKey, TEntity> m_EntityChangedPublisher;

        /// <inheritdoc/>
        public bool UseTransactionScope => m_Repository.UseTransactionScope;

        /// <inheritdoc/>
        public void Dispose()
        {
            m_Repository.Dispose();
        }

        /// <inheritdoc/>
        public ValueTask<int> InsertAsync(TEntity entity)
        {
            return m_Repository.InsertAsync(entity);
        }

        /// <inheritdoc/>
        public ValueTask<int> InsertRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            return m_Repository.InsertRangeAsync(entities, cancellationToken);
        }

        /// <inheritdoc/>
        public ValueTask<int> InsertRangeAsync(IAsyncEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            return m_Repository.InsertRangeAsync(entities, cancellationToken);
        }

        /// <inheritdoc/>
        public async ValueTask<int> UpdateAsync(TEntity entity)
        {
            var result = await m_Repository.UpdateAsync(entity).ConfigureAwait(false);

            NotifyUpdate(entity);

            return result;
        }

        /// <inheritdoc/>
        public async ValueTask<int> UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            var array = entities.ToArray();

            var result = await m_Repository.UpdateRangeAsync(array, cancellationToken).ConfigureAwait(false);

            NotifyUpdate(array);

            return result;
        }

        /// <inheritdoc/>
        public async ValueTask<int> UpdateRangeAsync(IAsyncEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            var list = new List<TEntity>();

            await foreach (var entity in entities.ConfigureAwait(false))
            {
                list.Add(entity);
            }

            var result = await m_Repository.UpdateRangeAsync(list, cancellationToken).ConfigureAwait(false);

            NotifyUpdate(list);

            return result;
        }

        /// <inheritdoc/>
        public async ValueTask<int> DeleteAsync(TEntity entity)
        {
            var result = await m_Repository.DeleteAsync(entity).ConfigureAwait(false);

            NotifyDelete(entity);

            return result;
        }

        /// <inheritdoc/>
        public async ValueTask<int> DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            var array = entities.ToArray();

            var result = await m_Repository.DeleteRangeAsync(array, cancellationToken).ConfigureAwait(false);

            NotifyDelete(array);

            return result;
        }

        /// <inheritdoc/>
        public async ValueTask<int> DeleteRangeAsync(IAsyncEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            var list = new List<TEntity>();

            await foreach (var entity in entities.ConfigureAwait(false))
            {
                list.Add(entity);
            }

            var result = await m_Repository.DeleteRangeAsync(list, cancellationToken).ConfigureAwait(false);

            NotifyDelete(list);

            return result;
        }

        /// <summary>
        /// Notifies that the specified entity has been updated.
        /// </summary>
        /// <param name="entity">The entity.</param>
        private void NotifyUpdate(TEntity entity)
        {
            var key = m_KeyGetter(entity);

            m_EntityChangedPublisher.Publish(key, entity);
        }

        /// <summary>
        /// Notifies that the specified entities have been updated.
        /// </summary>
        /// <param name="entities">The entities.</param>
        private void NotifyUpdate(IList<TEntity> entities)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                NotifyUpdate(entities[i]);
            }
        }

        /// <summary>
        /// Notifies that the specified entity has been deleted.
        /// </summary>
        /// <param name="entity">The entity.</param>
        private void NotifyDelete(TEntity entity)
        {
            var key = m_KeyGetter(entity);

            m_EntityChangedPublisher.Publish(key, entity);
        }

        /// <summary>
        /// Notifies that the specified entities have been deleted.
        /// </summary>
        /// <param name="entities">The entities.</param>
        private void NotifyDelete(IList<TEntity> entities)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                NotifyDelete(entities[i]);
            }
        }
    }
}

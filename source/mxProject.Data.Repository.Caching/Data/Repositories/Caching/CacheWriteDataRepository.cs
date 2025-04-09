using MessagePipe;
using mxProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace mxProject.Data.Repositories.Caching
{
    /// <summary>
    /// Data repository with the functionality required for caching entities. Notifies when an entity is changed.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TKey">The key type.</typeparam>
    internal class CacheWriteDataRepository<TEntity, TKey> : IWriteDataRepository<TEntity>
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="keyGetter">The method to get the key from a spacified entiry.</param>
        internal CacheWriteDataRepository(
            IWriteDataRepository<TEntity> repository,
            Func<TEntity, TKey> keyGetter
            )
        {
            m_Repository = repository;
            m_KeyGetter = keyGetter;

            m_EntityChangedPublisher = DataRepositoryCachingService.CreateEntityChangedPublisher<TKey, TEntity>();
        }

        private readonly IWriteDataRepository<TEntity> m_Repository;
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
        public int Insert(TEntity entity)
        {
            return m_Repository.Insert(entity);
        }

        /// <inheritdoc/>
        public int InsertRange(IEnumerable<TEntity> entities)
        {
            return m_Repository.InsertRange(entities);
        }

        /// <inheritdoc/>
        public int Update(TEntity entity)
        {
            var result = m_Repository.Update(entity);

            NotifyUpdate(entity);

            return result;
        }

        /// <inheritdoc/>
        public int UpdateRange(IEnumerable<TEntity> entities)
        {
            var array = entities.ToArray();

            var result = m_Repository.UpdateRange(array);

            NotifyUpdate(array);

            return result;
        }

        /// <inheritdoc/>
        public int Delete(TEntity entity)
        {
            var result = m_Repository.Delete(entity);

            NotifyDelete(entity);

            return result;
        }

        /// <inheritdoc/>
        public int DeleteRange(IEnumerable<TEntity> entities)
        {
            var array = entities.ToArray();

            var result = m_Repository.DeleteRange(array);

            NotifyDelete(array);

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
        private void NotifyUpdate(TEntity[] entities)
        {
            for (int i = 0; i < entities.Length; i++)
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
        private void NotifyDelete(TEntity[] entities)
        {
            for (int i = 0; i < entities.Length; i++)
            {
                NotifyDelete(entities[i]);
            }
        }
    }
}

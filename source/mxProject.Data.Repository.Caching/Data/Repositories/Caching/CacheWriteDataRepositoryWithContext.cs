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
    /// <typeparam name="TContext">The context type.</typeparam>
    internal class CacheWriteDataRepositoryWithContext<TEntity, TKey, TContext> : IWriteDataRepositoryWithContext<TEntity, TContext>
        where TContext : IDataRepositoryContext
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="keyGetter">The method to get the key from a spacified entiry.</param>
        internal CacheWriteDataRepositoryWithContext(
            IWriteDataRepositoryWithContext<TEntity, TContext> repository,
            Func<TEntity, TKey> keyGetter
            )
        {
            m_Repository = repository;
            m_KeyGetter = keyGetter;

            m_EntityChangedPublisher = DataRepositoryCachingService.CreateEntityChangedPublisher<TKey, TEntity>();
        }

        private readonly IWriteDataRepositoryWithContext<TEntity, TContext> m_Repository;
        private readonly Func<TEntity, TKey> m_KeyGetter;

        private readonly DataRepositoryCachingService.EntityChangedPublisher<TKey,TEntity> m_EntityChangedPublisher;

        /// <inheritdoc/>
        public bool UseTransactionScope => m_Repository.UseTransactionScope;

        /// <inheritdoc/>
        public void Dispose()
        {
            m_Repository.Dispose();
        }

        /// <inheritdoc/>
        public int Insert(TEntity entity, TContext context)
        {
            return m_Repository.Insert(entity, context);
        }

        /// <inheritdoc/>
        public int InsertRange(IEnumerable<TEntity> entities, TContext context)
        {
            return m_Repository.InsertRange(entities, context);
        }

        /// <inheritdoc/>
        public int Update(TEntity entity, TContext context)
        {
            var result = m_Repository.Update(entity, context);

            NotifyUpdate(entity);

            return result;
        }

        /// <inheritdoc/>
        public int UpdateRange(IEnumerable<TEntity> entities, TContext context)
        {
            var array = entities.ToArray();

            var result = m_Repository.UpdateRange(array, context);

            NotifyUpdate(array);

            return result;
        }

        /// <inheritdoc/>
        public int Delete(TEntity entity, TContext context)
        {
            var result = m_Repository.Delete(entity, context);

            NotifyDelete(entity);

            return result;
        }

        /// <inheritdoc/>
        public int DeleteRange(IEnumerable<TEntity> entities, TContext context)
        {
            var array = entities.ToArray();

            var result = m_Repository.DeleteRange(array, context);

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

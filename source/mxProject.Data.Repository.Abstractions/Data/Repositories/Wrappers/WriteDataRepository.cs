using System;
using System.Collections.Generic;
using System.Text;

namespace mxProject.Data.Repositories.Wrappers
{
    /// <summary>
    /// A data repository to write entities.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public class WriteDataRepository<TEntity> : IWriteDataRepository<TEntity>
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="useTransactionScope">A function that returns a value indicating whether to use ambient transactions using TransactionScope.</param>
        /// <param name="dispose">An action to dispose the repository.</param>
        /// <param name="insert">A function to insert an entity.</param>
        /// <param name="insertRange">A function to insert a range of entities.</param>
        /// <param name="update">A function to update an entity.</param>
        /// <param name="updateRange">A function to update a range of entities.</param>
        /// <param name="delete">A function to delete an entity.</param>
        /// <param name="deleteRange">A function to delete a range of entities.</param>
        public WriteDataRepository(
            Func<bool> useTransactionScope,
            Action dispose,
            Func<TEntity, int> insert,
            Func<IEnumerable<TEntity>, int> insertRange,
            Func<TEntity, int> update,
            Func<IEnumerable<TEntity>, int> updateRange,
            Func<TEntity, int> delete,
            Func<IEnumerable<TEntity>, int> deleteRange
            )
        {
            m_UseTransactionScope = useTransactionScope;
            m_Dispose = dispose;
            m_Insert = insert;
            m_InsertRange = insertRange;
            m_Update = update;
            m_UpdateRange = updateRange;
            m_Delete = delete;
            m_DeleteRange = deleteRange;
        }

        private readonly Func<bool> m_UseTransactionScope;
        private readonly Action m_Dispose;
        private readonly Func<TEntity, int> m_Insert;
        private readonly Func<IEnumerable<TEntity>, int> m_InsertRange;
        private readonly Func<TEntity, int> m_Update;
        private readonly Func<IEnumerable<TEntity>, int> m_UpdateRange;
        private readonly Func<TEntity, int> m_Delete;
        private readonly Func<IEnumerable<TEntity>, int> m_DeleteRange;

        /// <inheritdoc/>
        public bool UseTransactionScope => m_UseTransactionScope();

        /// <inheritdoc/>
        public void Dispose()
        {
            m_Dispose();
        }

        /// <inheritdoc/>
        public int Insert(TEntity entity)
        {
            return m_Insert(entity);
        }

        /// <inheritdoc/>
        public int InsertRange(IEnumerable<TEntity> entities)
        {
            return m_InsertRange(entities);
        }

        /// <inheritdoc/>
        public int Update(TEntity entity)
        {
            return m_Update(entity);
        }

        /// <inheritdoc/>
        public int UpdateRange(IEnumerable<TEntity> entities)
        {
            return m_UpdateRange(entities);
        }

        /// <inheritdoc/>
        public int Delete(TEntity entity)
        {
            return m_Delete(entity);
        }

        /// <inheritdoc/>
        public int DeleteRange(IEnumerable<TEntity> entities)
        {
            return m_DeleteRange(entities);
        }
    }
}

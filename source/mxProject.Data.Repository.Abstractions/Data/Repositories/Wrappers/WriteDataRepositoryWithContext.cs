using System;
using System.Collections.Generic;
using System.Text;

namespace mxProject.Data.Repositories.Wrappers
{
    /// <summary>
    /// A data repository to write entities.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TContext">The context type.</typeparam>
    public class WriteDataRepositoryWithContext<TEntity, TContext> : IWriteDataRepositoryWithContext<TEntity, TContext>
        where TContext : IDataRepositoryContext
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
        public WriteDataRepositoryWithContext(
            Func<bool> useTransactionScope,
            Action dispose,
            Func<TEntity, TContext, int> insert,
            Func<IEnumerable<TEntity>, TContext, int> insertRange,
            Func<TEntity, TContext, int> update,
            Func<IEnumerable<TEntity>, TContext, int> updateRange,
            Func<TEntity, TContext, int> delete,
            Func<IEnumerable<TEntity>, TContext, int> deleteRange
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
        private readonly Func<TEntity, TContext, int> m_Insert;
        private readonly Func<IEnumerable<TEntity>, TContext, int> m_InsertRange;
        private readonly Func<TEntity, TContext, int> m_Update;
        private readonly Func<IEnumerable<TEntity>, TContext, int> m_UpdateRange;
        private readonly Func<TEntity, TContext, int> m_Delete;
        private readonly Func<IEnumerable<TEntity>, TContext, int> m_DeleteRange;

        /// <inheritdoc/>
        public bool UseTransactionScope => m_UseTransactionScope();

        /// <inheritdoc/>
        public void Dispose()
        {
            m_Dispose();
        }

        /// <inheritdoc/>
        public int Insert(TEntity entity, TContext context)
        {
            return m_Insert(entity, context);
        }

        /// <inheritdoc/>
        public int InsertRange(IEnumerable<TEntity> entities, TContext context)
        {
            return m_InsertRange(entities, context);
        }

        /// <inheritdoc/>
        public int Update(TEntity entity, TContext context)
        {
            return m_Update(entity, context);
        }

        /// <inheritdoc/>
        public int UpdateRange(IEnumerable<TEntity> entities, TContext context)
        {
            return m_UpdateRange(entities, context);
        }

        /// <inheritdoc/>
        public int Delete(TEntity entity, TContext context)
        {
            return m_Delete(entity, context);
        }

        /// <inheritdoc/>
        public int DeleteRange(IEnumerable<TEntity> entities, TContext context)
        {
            return m_DeleteRange(entities, context);
        }
    }
}

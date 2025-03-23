using mxProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace mxProject.Data.Repositories.Wrappers
{
    /// <summary>
    /// A data repository that uses the context to retrieve entities.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TPrimaryKey">The primary key type.</typeparam>
    /// <typeparam name="TUniqueKey">The unique key type.</typeparam>
    /// <typeparam name="TContext">The context type.</typeparam>
    public class ReadDataRepositoryWithUniqueKeyWithContext<TEntity, TPrimaryKey, TUniqueKey, TContext> : IReadDataRepositoryWithUniqueKeyWithContext<TEntity, TPrimaryKey, TUniqueKey, TContext>
            where TContext : IDataRepositoryContext
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="useTransactionScope">A method to get a value that indicates whether to use an ambient transaction using TransactionScope.</param>
        /// <param name="dispose">A method to release resources.</param>
        /// <param name="getByPrimaryKey">A method to get an entity by its primary key.</param>
        /// <param name="getByUniqueKey">A method to get an entity by its unique key.</param>
        /// <param name="getRangeByPrimaryKey">A method to get entities by its primary keys.</param>
        /// <param name="getRangeByUniqueKey">A method to get entities by its unique keys.</param>
        /// <param name="getAll">A method to get all entities.</param>
        /// <param name="getAllPrimaryKeys">A method to get all primary keys.</param>
        /// <param name="getAllUniqueKeys">A method to get all unique keys.</param>
        public ReadDataRepositoryWithUniqueKeyWithContext(
            Func<bool> useTransactionScope,
            Action dispose,
            Func<TPrimaryKey, TContext, TEntity?> getByPrimaryKey,
            Func<TUniqueKey, TContext, TEntity?> getByUniqueKey,
            Func<IEnumerable<TPrimaryKey>, TContext, IEnumerable<TEntity>> getRangeByPrimaryKey,
            Func<IEnumerable<TUniqueKey>, TContext, IEnumerable<TEntity>> getRangeByUniqueKey,
            Func<TContext, IEnumerable<TEntity>> getAll,
            Func<TContext, IEnumerable<TPrimaryKey>> getAllPrimaryKeys,
            Func<TContext, IEnumerable<TUniqueKey>> getAllUniqueKeys
            )
        {
            m_UseTransactionScope = useTransactionScope;
            m_Dispose = dispose;
            m_GetByPrimaryKey = getByPrimaryKey;
            m_GetByUniqueKey = getByUniqueKey;
            m_GetRangeByPrimaryKey = getRangeByPrimaryKey;
            m_GetRangeByUniqueKey = getRangeByUniqueKey;
            m_GetAll = getAll;
            m_GetPrimaryAllKeys = getAllPrimaryKeys;
            m_GetAllUniqueKeys = getAllUniqueKeys;
        }

        private readonly Func<bool> m_UseTransactionScope;
        private readonly Action m_Dispose;
        private readonly Func<TPrimaryKey, TContext, TEntity?> m_GetByPrimaryKey;
        private readonly Func<TUniqueKey, TContext, TEntity?> m_GetByUniqueKey;
        private readonly Func<IEnumerable<TPrimaryKey>, TContext, IEnumerable<TEntity>> m_GetRangeByPrimaryKey;
        private readonly Func<IEnumerable<TUniqueKey>, TContext, IEnumerable<TEntity>> m_GetRangeByUniqueKey;
        private readonly Func<TContext, IEnumerable<TEntity>> m_GetAll;
        private readonly Func<TContext, IEnumerable<TPrimaryKey>> m_GetPrimaryAllKeys;
        private readonly Func<TContext, IEnumerable<TUniqueKey>> m_GetAllUniqueKeys;

        /// <inheritdoc/>
        public bool UseTransactionScope => m_UseTransactionScope();

        /// <inheritdoc/>
        public void Dispose()
        {
            m_Dispose();
        }

        /// <inheritdoc/>
        public TEntity? GetByPrimaryKey(TPrimaryKey key, TContext context)
        {
            return m_GetByPrimaryKey(key, context);
        }

        /// <inheritdoc/>
        public TEntity? GetByUniqueKey(TUniqueKey key, TContext context)
        {
            return m_GetByUniqueKey(key, context);
        }

        /// <inheritdoc/>
        public IEnumerable<TEntity> GetRangeByPrimaryKey(IEnumerable<TPrimaryKey> keys, TContext context)
        {
            return m_GetRangeByPrimaryKey(keys, context);
        }

        /// <inheritdoc/>
        public IEnumerable<TEntity> GetRangeByUniqueKey(IEnumerable<TUniqueKey> keys, TContext context)
        {
            return m_GetRangeByUniqueKey(keys, context);
        }

        /// <inheritdoc/>
        public IEnumerable<TEntity> GetAll(TContext context)
        {
            return m_GetAll(context);
        }

        /// <inheritdoc/>
        public IEnumerable<TPrimaryKey> GetAllPrimaryKeys(TContext context)
        {
            return m_GetPrimaryAllKeys(context);
        }

        /// <inheritdoc/>
        public IEnumerable<TUniqueKey> GetAllUniqueKeys(TContext context)
        {
            return m_GetAllUniqueKeys(context);
        }
    }
}

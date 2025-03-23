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
    /// <typeparam name="TKey">The key type.</typeparam>
    /// <typeparam name="TContext">The context type.</typeparam>
    public class ReadDataRepositoryWithContext<TEntity, TKey, TContext> : IReadDataRepositoryWithContext<TEntity, TKey, TContext>
        where TContext : IDataRepositoryContext
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="useTransactionScope">A method to get a value that indicates whether to use an ambient transaction using TransactionScope.</param>
        /// <param name="dispose">A method to release resources.</param>
        /// <param name="get">A method to get an entity by its key.</param>
        /// <param name="getRange">A method to get entities by its keys.</param>
        /// <param name="getAll">A method to get all entities.</param>
        /// <param name="getAllKeys">A method to get all keys.</param>
        public ReadDataRepositoryWithContext(
            Func<bool> useTransactionScope, 
            Action dispose,
            Func<TKey, TContext, TEntity?> get,
            Func<IEnumerable<TKey>, TContext, IEnumerable<TEntity>> getRange,
            Func<TContext, IEnumerable<TEntity>> getAll,
            Func<TContext, IEnumerable<TKey>> getAllKeys
            )
        {
            m_UseTransactionScope = useTransactionScope;
            m_Dispose = dispose;
            m_Get = get;
            m_GetRange = getRange;
            m_GetAll = getAll;
            m_GetAllKeys = getAllKeys;
        }

        private readonly Func<bool> m_UseTransactionScope;
        private readonly Action m_Dispose;
        private readonly Func<TKey, TContext, TEntity?> m_Get;
        private readonly Func<IEnumerable<TKey>, TContext, IEnumerable<TEntity>> m_GetRange;
        private readonly Func<TContext, IEnumerable<TEntity>> m_GetAll;
        private readonly Func<TContext, IEnumerable<TKey>> m_GetAllKeys;

        /// <inheritdoc/>
        public bool UseTransactionScope => m_UseTransactionScope();

        /// <inheritdoc/>
        public void Dispose()
        {
            m_Dispose();
        }

        /// <inheritdoc/>
        public TEntity? Get(TKey key, TContext context)
        {
            return m_Get(key, context);
        }

        /// <inheritdoc/>
        public IEnumerable<TEntity> GetRange(IEnumerable<TKey> keys, TContext context)
        {
            return m_GetRange(keys, context);
        }

        /// <inheritdoc/>
        public IEnumerable<TEntity> GetAll(TContext context)
        {
            return m_GetAll(context);
        }

        /// <inheritdoc/>
        public IEnumerable<TKey> GetAllKeys(TContext context)
        {
            return m_GetAllKeys(context);
        }
    }
}

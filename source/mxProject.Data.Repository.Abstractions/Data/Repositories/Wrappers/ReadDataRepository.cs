using mxProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace mxProject.Data.Repositories.Wrappers
{
    /// <summary>
    /// A data repository that retrieves entities.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TKey">The key type.</typeparam>
    public class ReadDataRepository<TEntity, TKey> : IReadDataRepository<TEntity, TKey>
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
        public ReadDataRepository(
            Func<bool> useTransactionScope, 
            Action dispose,
            Func<TKey, TEntity?> get,
            Func<IEnumerable<TKey>, IEnumerable<TEntity>> getRange,
            Func<IEnumerable<TEntity>> getAll,
            Func<IEnumerable<TKey>> getAllKeys
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
        private readonly Func<TKey, TEntity?> m_Get;
        private readonly Func<IEnumerable<TKey>, IEnumerable<TEntity>> m_GetRange;
        private readonly Func<IEnumerable<TEntity>> m_GetAll;
        private readonly Func<IEnumerable<TKey>> m_GetAllKeys;

        /// <inheritdoc/>
        public bool UseTransactionScope => m_UseTransactionScope();

        /// <inheritdoc/>
        public void Dispose()
        {
            m_Dispose();
        }

        /// <inheritdoc/>
        public TEntity? Get(TKey key)
        {
            return m_Get(key);
        }

        /// <inheritdoc/>
        public IEnumerable<TEntity> GetRange(IEnumerable<TKey> keys)
        {
            return m_GetRange(keys);
        }

        /// <inheritdoc/>
        public IEnumerable<TEntity> GetAll()
        {
            return m_GetAll();
        }

        /// <inheritdoc/>
        public IEnumerable<TKey> GetAllKeys()
        {
            return m_GetAllKeys();
        }
    }
}

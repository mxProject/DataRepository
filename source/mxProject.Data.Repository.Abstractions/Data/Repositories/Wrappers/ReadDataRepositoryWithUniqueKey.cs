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
    /// <typeparam name="TPrimaryKey">The primary key type.</typeparam>
    /// <typeparam name="TUniqueKey">The unique key type.</typeparam>
    public class ReadDataRepositoryWithUniqueKey<TEntity, TPrimaryKey, TUniqueKey> : IReadDataRepositoryWithUniqueKey<TEntity, TPrimaryKey, TUniqueKey>
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
        public ReadDataRepositoryWithUniqueKey(
            Func<bool> useTransactionScope,
            Action dispose,
            Func<TPrimaryKey, TEntity?> getByPrimaryKey,
            Func<TUniqueKey, TEntity?> getByUniqueKey,
            Func<IEnumerable<TPrimaryKey>, IEnumerable<TEntity>> getRangeByPrimaryKey,
            Func<IEnumerable<TUniqueKey>, IEnumerable<TEntity>> getRangeByUniqueKey,
            Func<IEnumerable<TEntity>> getAll,
            Func<IEnumerable<TPrimaryKey>> getAllPrimaryKeys,
            Func<IEnumerable<TUniqueKey>> getAllUniqueKeys
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
        private readonly Func<TPrimaryKey, TEntity?> m_GetByPrimaryKey;
        private readonly Func<TUniqueKey, TEntity?> m_GetByUniqueKey;
        private readonly Func<IEnumerable<TPrimaryKey>, IEnumerable<TEntity>> m_GetRangeByPrimaryKey;
        private readonly Func<IEnumerable<TUniqueKey>, IEnumerable<TEntity>> m_GetRangeByUniqueKey;
        private readonly Func<IEnumerable<TEntity>> m_GetAll;
        private readonly Func<IEnumerable<TPrimaryKey>> m_GetPrimaryAllKeys;
        private readonly Func<IEnumerable<TUniqueKey>> m_GetAllUniqueKeys;

        /// <inheritdoc/>
        public bool UseTransactionScope => m_UseTransactionScope();

        /// <inheritdoc/>
        public void Dispose()
        {
            m_Dispose();
        }

        /// <inheritdoc/>
        public TEntity? GetByPrimaryKey(TPrimaryKey key)
        {
            return m_GetByPrimaryKey(key);
        }

        /// <inheritdoc/>
        public TEntity? GetByUniqueKey(TUniqueKey key)
        {
            return m_GetByUniqueKey(key);
        }

        /// <inheritdoc/>
        public IEnumerable<TEntity> GetRangeByPrimaryKey(IEnumerable<TPrimaryKey> keys)
        {
            return m_GetRangeByPrimaryKey(keys);
        }

        /// <inheritdoc/>
        public IEnumerable<TEntity> GetRangeByUniqueKey(IEnumerable<TUniqueKey> keys)
        {
            return m_GetRangeByUniqueKey(keys);
        }

        /// <inheritdoc/>
        public IEnumerable<TEntity> GetAll()
        {
            return m_GetAll();
        }

        /// <inheritdoc/>
        public IEnumerable<TPrimaryKey> GetAllPrimaryKeys()
        {
            return m_GetPrimaryAllKeys();
        }

        /// <inheritdoc/>
        public IEnumerable<TUniqueKey> GetAllUniqueKeys()
        {
            return m_GetAllUniqueKeys();
        }
    }
}

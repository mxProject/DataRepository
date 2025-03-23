using mxProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace mxProject.Data.Repositories.Wrappers
{
    /// <summary>
    /// A data repository that retrieves entities asynchronously.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TPrimaryKey">The primary key type.</typeparam>
    /// <typeparam name="TUniqueKey">The unique key type.</typeparam>
    public class AsyncReadDataRepositoryWithUniqueKey<TEntity, TPrimaryKey, TUniqueKey> : IAsyncReadDataRepositoryWithUniqueKey<TEntity, TPrimaryKey, TUniqueKey>
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="useTransactionScope">A method to get a value that indicates whether to use an ambient transaction using TransactionScope.</param>
        /// <param name="dispose">A method to release resources.</param>
        /// <param name="getByPrimaryKey">A method to get an entity by its primary key.</param>
        /// <param name="getByUniqueKey">A method to get an entity by its unique key.</param>
        /// <param name="getRangeByPrimaryKey">A method to get entities by its primary keys.</param>
        /// <param name="getRangeByAsyncPrimaryKey">A method to get entities by its primary keys.</param>
        /// <param name="getRangeByUniqueKey">A method to get entities by its unique keys.</param>
        /// <param name="getRangeByAsyncUniqueKey">A method to get entities by its unique keys.</param>
        /// <param name="getAll">A method to get all entities.</param>
        /// <param name="getAllPrimaryKeys">A method to get all primary keys.</param>
        /// <param name="getAllUniqueKeys">A method to get all unique keys.</param>
        public AsyncReadDataRepositoryWithUniqueKey(
            Func<bool> useTransactionScope,
            Action dispose,
            Func<TPrimaryKey, ValueTask<TEntity?>> getByPrimaryKey,
            Func<TUniqueKey, ValueTask<TEntity?>> getByUniqueKey,
            Func<IEnumerable<TPrimaryKey>, CancellationToken, IAsyncEnumerable<TEntity>> getRangeByPrimaryKey,
            Func<IAsyncEnumerable<TPrimaryKey>, CancellationToken, IAsyncEnumerable<TEntity>> getRangeByAsyncPrimaryKey,
            Func<IEnumerable<TUniqueKey>, CancellationToken, IAsyncEnumerable<TEntity>> getRangeByUniqueKey,
            Func<IAsyncEnumerable<TUniqueKey>, CancellationToken, IAsyncEnumerable<TEntity>> getRangeByAsyncUniqueKey,
            Func<CancellationToken, IAsyncEnumerable<TEntity>> getAll,
            Func<CancellationToken, IAsyncEnumerable<TPrimaryKey>> getAllPrimaryKeys,
            Func<CancellationToken, IAsyncEnumerable<TUniqueKey>> getAllUniqueKeys
            )
        {
            m_UseTransactionScope = useTransactionScope;
            m_Dispose = dispose;
            m_GetByPrimaryKey = getByPrimaryKey;
            m_GetByUniqueKey = getByUniqueKey;
            m_GetRangeByPrimaryKey = getRangeByPrimaryKey;
            m_GetRangeByAsyncPrimaryKey = getRangeByAsyncPrimaryKey;
            m_GetRangeByUniqueKey = getRangeByUniqueKey;
            m_GetRangeByAsyncUniqueKey = getRangeByAsyncUniqueKey;
            m_GetAll = getAll;
            m_GetPrimaryAllKeys = getAllPrimaryKeys;
            m_GetAllUniqueKeys = getAllUniqueKeys;
        }

        private readonly Func<bool> m_UseTransactionScope;
        private readonly Action m_Dispose;
        private readonly Func<TPrimaryKey, ValueTask<TEntity?>> m_GetByPrimaryKey;
        private readonly Func<TUniqueKey, ValueTask<TEntity?>> m_GetByUniqueKey;
        private readonly Func<IEnumerable<TPrimaryKey>, CancellationToken, IAsyncEnumerable<TEntity>> m_GetRangeByPrimaryKey;
        private readonly Func<IAsyncEnumerable<TPrimaryKey>, CancellationToken, IAsyncEnumerable<TEntity>> m_GetRangeByAsyncPrimaryKey;
        private readonly Func<IEnumerable<TUniqueKey>, CancellationToken, IAsyncEnumerable<TEntity>> m_GetRangeByUniqueKey;
        private readonly Func<IAsyncEnumerable<TUniqueKey>, CancellationToken, IAsyncEnumerable<TEntity>> m_GetRangeByAsyncUniqueKey;
        private readonly Func<CancellationToken, IAsyncEnumerable<TEntity>> m_GetAll;
        private readonly Func<CancellationToken, IAsyncEnumerable<TPrimaryKey>> m_GetPrimaryAllKeys;
        private readonly Func<CancellationToken, IAsyncEnumerable<TUniqueKey>> m_GetAllUniqueKeys;

        /// <inheritdoc/>
        public bool UseTransactionScope => m_UseTransactionScope();

        /// <inheritdoc/>
        public void Dispose()
        {
            m_Dispose();
        }

        /// <inheritdoc/>
        public ValueTask<TEntity?> GetByPrimaryKeyAsync(TPrimaryKey key)
        {
            return m_GetByPrimaryKey(key);
        }

        /// <inheritdoc/>
        public ValueTask<TEntity?> GetByUniqueKeyAsync(TUniqueKey key)
        {
            return m_GetByUniqueKey(key);
        }

        /// <inheritdoc/>
        public IAsyncEnumerable<TEntity> GetRangeByPrimaryKeyAsync(IEnumerable<TPrimaryKey> keys, CancellationToken cancellationToken = default)
        {
            return m_GetRangeByPrimaryKey(keys, cancellationToken);
        }

        /// <inheritdoc/>
        public IAsyncEnumerable<TEntity> GetRangeByPrimaryKeyAsync(IAsyncEnumerable<TPrimaryKey> keys, CancellationToken cancellationToken = default)
        {
            return m_GetRangeByAsyncPrimaryKey(keys, cancellationToken);
        }

        /// <inheritdoc/>
        public IAsyncEnumerable<TEntity> GetRangeByUniqueKeyAsync(IEnumerable<TUniqueKey> keys, CancellationToken cancellationToken = default)
        {
            return m_GetRangeByUniqueKey(keys, cancellationToken);
        }

        /// <inheritdoc/>
        public IAsyncEnumerable<TEntity> GetRangeByUniqueKeyAsync(IAsyncEnumerable<TUniqueKey> keys, CancellationToken cancellationToken = default)
        {
            return m_GetRangeByAsyncUniqueKey(keys, cancellationToken);
        }

        /// <inheritdoc/>
        public IAsyncEnumerable<TEntity> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return m_GetAll(cancellationToken);
        }

        /// <inheritdoc/>
        public IAsyncEnumerable<TPrimaryKey> GetAllPrimaryKeysAsync(CancellationToken cancellationToken = default)
        {
            return m_GetPrimaryAllKeys(cancellationToken);
        }

        /// <inheritdoc/>
        public IAsyncEnumerable<TUniqueKey> GetAllUniqueKeysAsync(CancellationToken cancellationToken = default)
        {
            return m_GetAllUniqueKeys(cancellationToken);
        }
    }
}

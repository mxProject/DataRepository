using mxProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace mxProject.Data.Repositories.Wrappers
{
    /// <summary>
    /// A data repository that retrieves entities asynchronously.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TKey">The key type.</typeparam>
    public class AsyncReadDataRepository<TEntity, TKey> : IAsyncReadDataRepository<TEntity, TKey>
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="useTransactionScope">A method to get a value that indicates whether to use an ambient transaction using TransactionScope.</param>
        /// <param name="dispose">A method to release resources.</param>
        /// <param name="get">A method to get an entity by its key.</param>
        /// <param name="getRange">A method to get entities by its keys.</param>
        /// <param name="getAsyncRange">A method to get entities by its keys.</param>
        /// <param name="getAll">A method to get all entities.</param>
        /// <param name="getAllKeys">A method to get all keys.</param>
        public AsyncReadDataRepository(
            Func<bool> useTransactionScope, 
            Action dispose,
            Func<TKey, ValueTask<TEntity?>> get,
            Func<IEnumerable<TKey>, CancellationToken, IAsyncEnumerable<TEntity>> getRange,
            Func<IAsyncEnumerable<TKey>, CancellationToken, IAsyncEnumerable<TEntity>> getAsyncRange,
            Func<CancellationToken, IAsyncEnumerable<TEntity>> getAll,
            Func<CancellationToken, IAsyncEnumerable<TKey>> getAllKeys
            )
        {
            m_UseTransactionScope = useTransactionScope;
            m_Dispose = dispose;
            m_Get = get;
            m_GetRange = getRange;
            m_GetAsyncRange = getAsyncRange;
            m_GetAll = getAll;
            m_GetAllKeys = getAllKeys;
        }

        private readonly Func<bool> m_UseTransactionScope;
        private readonly Action m_Dispose;
        private readonly Func<TKey, ValueTask<TEntity?>> m_Get;
        private readonly Func<IEnumerable<TKey>, CancellationToken, IAsyncEnumerable<TEntity>> m_GetRange;
        private readonly Func<IAsyncEnumerable<TKey>, CancellationToken, IAsyncEnumerable<TEntity>> m_GetAsyncRange;
        private readonly Func<CancellationToken, IAsyncEnumerable<TEntity>> m_GetAll;
        private readonly Func<CancellationToken, IAsyncEnumerable<TKey>> m_GetAllKeys;

        /// <inheritdoc/>
        public bool UseTransactionScope => m_UseTransactionScope();

        /// <inheritdoc/>
        public void Dispose()
        {
            m_Dispose();
        }

        /// <inheritdoc/>
        public ValueTask<TEntity?> GetAsync(TKey key)
        {
            return m_Get(key);
        }

        /// <inheritdoc/>
        public IAsyncEnumerable<TEntity> GetRangeAsync(IEnumerable<TKey> keys, CancellationToken cancellationToken = default)
        {
            return m_GetRange(keys, cancellationToken);
        }

        /// <inheritdoc/>
        public IAsyncEnumerable<TEntity> GetRangeAsync(IAsyncEnumerable<TKey> keys, CancellationToken cancellationToken = default)
        {
            return m_GetAsyncRange(keys, cancellationToken);
        }

        /// <inheritdoc/>
        public IAsyncEnumerable<TEntity> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return m_GetAll(cancellationToken);
        }

        /// <inheritdoc/>
        public IAsyncEnumerable<TKey> GetAllKeysAsync(CancellationToken cancellationToken = default)
        {
            return m_GetAllKeys(cancellationToken);
        }
    }
}

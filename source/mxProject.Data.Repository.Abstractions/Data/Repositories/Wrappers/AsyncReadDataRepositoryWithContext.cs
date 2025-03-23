using mxProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace mxProject.Data.Repositories.Wrappers
{
    /// <summary>
    /// A data repository that uses the context to retrieve entities asynchronously.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TKey">The key type.</typeparam>
    /// <typeparam name="TContext">The context type.</typeparam>
    public class AsyncReadDataRepositoryWithContext<TEntity, TKey, TContext> : IAsyncReadDataRepositoryWithContext<TEntity, TKey, TContext>
        where TContext : IDataRepositoryContext
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
        public AsyncReadDataRepositoryWithContext(
            Func<bool> useTransactionScope, 
            Action dispose,
            Func<TKey, TContext, ValueTask<TEntity?>> get,
            Func<IEnumerable<TKey>, TContext, CancellationToken, IAsyncEnumerable<TEntity>> getRange,
            Func<IAsyncEnumerable<TKey>, TContext, CancellationToken, IAsyncEnumerable<TEntity>> getAsyncRange,
            Func<TContext, CancellationToken, IAsyncEnumerable<TEntity>> getAll,
            Func<TContext, CancellationToken, IAsyncEnumerable<TKey>> getAllKeys
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
        private readonly Func<TKey, TContext, ValueTask<TEntity?>> m_Get;
        private readonly Func<IEnumerable<TKey>, TContext, CancellationToken, IAsyncEnumerable<TEntity>> m_GetRange;
        private readonly Func<IAsyncEnumerable<TKey>, TContext, CancellationToken, IAsyncEnumerable<TEntity>> m_GetAsyncRange;
        private readonly Func<TContext, CancellationToken, IAsyncEnumerable<TEntity>> m_GetAll;
        private readonly Func<TContext, CancellationToken, IAsyncEnumerable<TKey>> m_GetAllKeys;

        /// <inheritdoc/>
        public bool UseTransactionScope => m_UseTransactionScope();

        /// <inheritdoc/>
        public void Dispose()
        {
            m_Dispose();
        }

        /// <inheritdoc/>
        public ValueTask<TEntity?> GetAsync(TKey key, TContext context)
        {
            return m_Get(key, context);
        }

        /// <inheritdoc/>
        public IAsyncEnumerable<TEntity> GetRangeAsync(IEnumerable<TKey> keys, TContext context, CancellationToken cancellationToken = default)
        {
            return m_GetRange(keys, context, cancellationToken);
        }

        /// <inheritdoc/>
        public IAsyncEnumerable<TEntity> GetRangeAsync(IAsyncEnumerable<TKey> keys, TContext context, CancellationToken cancellationToken = default)
        {
            return m_GetAsyncRange(keys, context, cancellationToken);
        }

        /// <inheritdoc/>
        public IAsyncEnumerable<TEntity> GetAllAsync(TContext context, CancellationToken cancellationToken = default)
        {
            return m_GetAll(context, cancellationToken);
        }

        /// <inheritdoc/>
        public IAsyncEnumerable<TKey> GetAllKeysAsync(TContext context, CancellationToken cancellationToken = default)
        {
            return m_GetAllKeys(context, cancellationToken);
        }
    }
}

using mxProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace mxProject.Data.Repositories.Wrappers
{
    /// <summary>
    /// A data repository that uses the context to retrieve entities asynchronously.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TPrimaryKey">The primary key type.</typeparam>
    /// <typeparam name="TUniqueKey">The unique key type.</typeparam>
    /// <typeparam name="TContext">The context type.</typeparam>
    public class AsyncReadDataRepositoryWithUniqueKeyWithContext<TEntity, TPrimaryKey, TUniqueKey, TContext> : IAsyncReadDataRepositoryWithUniqueKeyWithContext<TEntity, TPrimaryKey, TUniqueKey, TContext>
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
        /// <param name="getRangeByAsyncPrimaryKey">A method to get entities by its primary keys.</param>
        /// <param name="getRangeByUniqueKey">A method to get entities by its unique keys.</param>
        /// <param name="getRangeByAsyncUniqueKey">A method to get entities by its unique keys.</param>
        /// <param name="getAll">A method to get all entities.</param>
        /// <param name="getAllPrimaryKeys">A method to get all primary keys.</param>
        /// <param name="getAllUniqueKeys">A method to get all unique keys.</param>
        public AsyncReadDataRepositoryWithUniqueKeyWithContext(
            Func<bool> useTransactionScope,
            Action dispose,
            Func<TPrimaryKey, TContext, ValueTask<TEntity?>> getByPrimaryKey,
            Func<TUniqueKey, TContext, ValueTask<TEntity?>> getByUniqueKey,
            Func<IEnumerable<TPrimaryKey>, TContext, CancellationToken, IAsyncEnumerable<TEntity>> getRangeByPrimaryKey,
            Func<IAsyncEnumerable<TPrimaryKey>, TContext, CancellationToken, IAsyncEnumerable<TEntity>> getRangeByAsyncPrimaryKey,
            Func<IEnumerable<TUniqueKey>, TContext, CancellationToken, IAsyncEnumerable<TEntity>> getRangeByUniqueKey,
            Func<IAsyncEnumerable<TUniqueKey>, TContext, CancellationToken, IAsyncEnumerable<TEntity>> getRangeByAsyncUniqueKey,
            Func<TContext, CancellationToken, IAsyncEnumerable<TEntity>> getAll,
            Func<TContext, CancellationToken, IAsyncEnumerable<TPrimaryKey>> getAllPrimaryKeys,
            Func<TContext, CancellationToken, IAsyncEnumerable<TUniqueKey>> getAllUniqueKeys
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
        private readonly Func<TPrimaryKey, TContext, ValueTask<TEntity?>> m_GetByPrimaryKey;
        private readonly Func<TUniqueKey, TContext, ValueTask<TEntity?>> m_GetByUniqueKey;
        private readonly Func<IEnumerable<TPrimaryKey>, TContext, CancellationToken, IAsyncEnumerable<TEntity>> m_GetRangeByPrimaryKey;
        private readonly Func<IAsyncEnumerable<TPrimaryKey>, TContext, CancellationToken, IAsyncEnumerable<TEntity>> m_GetRangeByAsyncPrimaryKey;
        private readonly Func<IEnumerable<TUniqueKey>, TContext, CancellationToken, IAsyncEnumerable<TEntity>> m_GetRangeByUniqueKey;
        private readonly Func<IAsyncEnumerable<TUniqueKey>, TContext, CancellationToken,IAsyncEnumerable<TEntity>> m_GetRangeByAsyncUniqueKey;
        private readonly Func<TContext, CancellationToken, IAsyncEnumerable<TEntity>> m_GetAll;
        private readonly Func<TContext, CancellationToken, IAsyncEnumerable<TPrimaryKey>> m_GetPrimaryAllKeys;
        private readonly Func<TContext, CancellationToken, IAsyncEnumerable<TUniqueKey>> m_GetAllUniqueKeys;

        /// <inheritdoc/>
        public bool UseTransactionScope => m_UseTransactionScope();

        /// <inheritdoc/>
        public void Dispose()
        {
            m_Dispose();
        }

        /// <inheritdoc/>
        public ValueTask<TEntity?> GetByPrimaryKeyAsync(TPrimaryKey key, TContext context)
        {
            return m_GetByPrimaryKey(key, context);
        }

        /// <inheritdoc/>
        public ValueTask<TEntity?> GetByUniqueKeyAsync(TUniqueKey key, TContext context)
        {
            return m_GetByUniqueKey(key, context);
        }

        /// <inheritdoc/>
        public IAsyncEnumerable<TEntity> GetRangeByPrimaryKeyAsync(IEnumerable<TPrimaryKey> keys, TContext context, CancellationToken cancellationToken = default)
        {
            return m_GetRangeByPrimaryKey(keys, context, cancellationToken);
        }

        /// <inheritdoc/>
        public IAsyncEnumerable<TEntity> GetRangeByPrimaryKeyAsync(IAsyncEnumerable<TPrimaryKey> keys, TContext context, CancellationToken cancellationToken = default)
        {
            return m_GetRangeByAsyncPrimaryKey(keys, context, cancellationToken);
        }

        /// <inheritdoc/>
        public IAsyncEnumerable<TEntity> GetRangeByUniqueKeyAsync(IEnumerable<TUniqueKey> keys, TContext context, CancellationToken cancellationToken = default)
        {
            return m_GetRangeByUniqueKey(keys, context, cancellationToken);
        }

        /// <inheritdoc/>
        public IAsyncEnumerable<TEntity> GetRangeByUniqueKeyAsync(IAsyncEnumerable<TUniqueKey> keys, TContext context, CancellationToken cancellationToken = default)
        {
            return m_GetRangeByAsyncUniqueKey(keys, context, cancellationToken);
        }

        /// <inheritdoc/>
        public IAsyncEnumerable<TEntity> GetAllAsync(TContext context, CancellationToken cancellationToken = default)
        {
            return m_GetAll(context, cancellationToken);
        }

        /// <inheritdoc/>
        public IAsyncEnumerable<TPrimaryKey> GetAllPrimaryKeysAsync(TContext context, CancellationToken cancellationToken = default)
        {
            return m_GetPrimaryAllKeys(context, cancellationToken);
        }

        /// <inheritdoc/>
        public IAsyncEnumerable<TUniqueKey> GetAllUniqueKeysAsync(TContext context, CancellationToken cancellationToken = default)
        {
            return m_GetAllUniqueKeys(context, cancellationToken);
        }
    }
}

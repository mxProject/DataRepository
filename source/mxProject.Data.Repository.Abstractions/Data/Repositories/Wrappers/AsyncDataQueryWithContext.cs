﻿using mxProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace mxProject.Data.Repositories.Wrappers
{
    /// <summary>
    /// A data query that finds entities that match specified condition asynchronously.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TCondition">The query condition type.</typeparam>
    /// <typeparam name="TContext">The context type.</typeparam>
    public class AsyncDataQueryWithContext<TEntity, TCondition, TContext> : IAsyncDataQueryWithContext<TEntity, TCondition, TContext>
        where TContext : IDataRepositoryContext
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="useTransactionScope">A method to get a value that indicates whether to use an ambient transaction using TransactionScope.</param>
        /// <param name="dispose">A method to release resources.</param>
        /// <param name="getCount">A method to get the number of entities that match the specified condition.</param>
        /// <param name="query">A method to find entities that match the specified condition.</param>
        public AsyncDataQueryWithContext(
            Func<bool> useTransactionScope,
            Action dispose,
            Func<TCondition, TContext, ValueTask<int>> getCount,
            Func<TCondition, TContext, int, int?, CancellationToken, IAsyncEnumerable<TEntity>> query
            )
        {
            m_UseTransactionScope = useTransactionScope;
            m_Dispose = dispose;
            m_GetCount = getCount;
            m_Query = query;
        }

        private readonly Func<bool> m_UseTransactionScope;
        private readonly Action m_Dispose;
        private readonly Func<TCondition, TContext, ValueTask<int>> m_GetCount;
        private readonly Func<TCondition, TContext, int, int?, CancellationToken, IAsyncEnumerable<TEntity>> m_Query;

        /// <inheritdoc/>
        public bool UseTransactionScope => m_UseTransactionScope();

        /// <inheritdoc/>
        public void Dispose()
        {
            m_Dispose();
        }

        /// <inheritdoc/>
        public ValueTask<int> GetCountAsync(TCondition condition, TContext context)
        {
            return m_GetCount(condition, context);
        }

        /// <inheritdoc/>
        public IAsyncEnumerable<TEntity> QueryAsync(TCondition condition, TContext context, int skipCount = 0, int? maximumCount = null, CancellationToken cancellationToken = default)
        {
            return m_Query(condition, context, skipCount, maximumCount, cancellationToken);
        }
    }
}

using mxProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace mxProject.Data.Repositories.Wrappers
{
    /// <summary>
    /// A data query that finds entities that match specified condition.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TCondition">The query condition type.</typeparam>
    public class DataQuery<TEntity, TCondition> : IDataQuery<TEntity, TCondition>
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="useTransactionScope">A method to get a value that indicates whether to use an ambient transaction using TransactionScope.</param>
        /// <param name="dispose">A method to release resources.</param>
        /// <param name="getCount">A method to get the number of entities that match the specified condition.</param>
        /// <param name="query">A method to find entities that match the specified condition.</param>
        public DataQuery(
            Func<bool> useTransactionScope,
            Action dispose,
            Func<TCondition, int> getCount,
            Func<TCondition, int, int?, IEnumerable<TEntity>> query
            )
        {
            m_UseTransactionScope = useTransactionScope;
            m_Dispose = dispose;
            m_GetCount = getCount;
            m_Query = query;
        }

        private readonly Func<bool> m_UseTransactionScope;
        private readonly Action m_Dispose;
        private readonly Func<TCondition, int> m_GetCount;
        private readonly Func<TCondition, int, int?, IEnumerable<TEntity>> m_Query;

        /// <inheritdoc/>
        public bool UseTransactionScope => m_UseTransactionScope();

        /// <inheritdoc/>
        public void Dispose()
        {
            m_Dispose();
        }

        /// <inheritdoc/>
        public int GetCount(TCondition condition)
        {
            return m_GetCount(condition);
        }

        /// <inheritdoc/>
        public IEnumerable<TEntity> Query(TCondition condition, int skipCount = 0, int? maximumCount = null)
        {
            return m_Query(condition, skipCount, maximumCount);
        }
    }
}

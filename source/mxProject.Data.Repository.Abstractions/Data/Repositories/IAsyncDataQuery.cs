using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace mxProject.Data.Repositories
{
    /// <summary>
    /// It provides the functionality required for a query to find entities asynchronously.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TCondition">The query condition type.</typeparam>
    public interface IAsyncDataQuery<TEntity, TCondition> : IDataRepository<TEntity>
    {
        /// <summary>
        /// Finds entities that match the specified condition.
        /// </summary>
        /// <param name="condition">The conditions.</param>
        /// <param name="skipCount">The number of entities to skip</param>
        /// <param name="maximumCount">The maximum number of entities to retrieve.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entities.</returns>
        IAsyncEnumerable<TEntity> QueryAsync(TCondition condition, int skipCount = 0, int? maximumCount = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the number of entities that match the specified condition.
        /// </summary>
        /// <param name="condition">The conditions.</param>
        /// <returns>Number of entities.</returns>
        ValueTask<int> GetCountAsync(TCondition condition);
    }
}

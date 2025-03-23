using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace mxProject.Data.Repositories
{
    /// <summary>
    /// It provides the functionality required for a data repository to write entities asynchronously.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public interface IAsyncWriteDataRepository<TEntity> : IDataRepository<TEntity>
    {
        /// <summary>
        /// Inserts the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>Number of affected entities.</returns>
        ValueTask<int> InsertAsync(TEntity entity);

        /// <summary>
        /// Inserts the specified entitis.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Number of affected entities.</returns>
        ValueTask<int> InsertRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        /// <summary>
        /// Inserts the specified entitis.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Number of affected entities.</returns>
        ValueTask<int> InsertRangeAsync(IAsyncEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>Number of affected entities.</returns>
        ValueTask<int> UpdateAsync(TEntity entity);

        /// <summary>
        /// Updates the specified entitis.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Number of affected entities.</returns>
        ValueTask<int> UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates the specified entitis.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Number of affected entities.</returns>
        ValueTask<int> UpdateRangeAsync(IAsyncEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>Number of affected entities.</returns>
        ValueTask<int> DeleteAsync(TEntity entity);

        /// <summary>
        /// Deletes the specified entitis.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Number of affected entities.</returns>
        ValueTask<int> DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes the specified entitis.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Number of affected entities.</returns>
        ValueTask<int> DeleteRangeAsync(IAsyncEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
    }
}

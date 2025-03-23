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
    /// <typeparam name="TContext">The context type.</typeparam>
    public interface IAsyncWriteDataRepositoryWithContext<TEntity, TContext> : IDataRepository<TEntity>
        where TContext : IDataRepositoryContext
    {
        /// <summary>
        /// Inserts the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="context">The current context.</param>
        /// <returns>Number of affected entities.</returns>
        ValueTask<int> InsertAsync(TEntity entity, TContext context);

        /// <summary>
        /// Inserts the specified entitis.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="context">The current context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Number of affected entities.</returns>
        ValueTask<int> InsertRangeAsync(IEnumerable<TEntity> entities, TContext context, CancellationToken cancellationToken = default);

        /// <summary>
        /// Inserts the specified entitis.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="context">The current context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Number of affected entities.</returns>
        ValueTask<int> InsertRangeAsync(IAsyncEnumerable<TEntity> entities, TContext context, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="context">The current context.</param>
        /// <returns>Number of affected entities.</returns>
        ValueTask<int> UpdateAsync(TEntity entity, TContext context);

        /// <summary>
        /// Updates the specified entitis.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="context">The current context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Number of affected entities.</returns>
        ValueTask<int> UpdateRangeAsync(IEnumerable<TEntity> entities, TContext context, CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates the specified entitis.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="context">The current context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Number of affected entities.</returns>
        ValueTask<int> UpdateRangeAsync(IAsyncEnumerable<TEntity> entities, TContext context, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="context">The current context.</param>
        /// <returns>Number of affected entities.</returns>
        ValueTask<int> DeleteAsync(TEntity entity, TContext context);

        /// <summary>
        /// Deletes the specified entitis.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="context">The current context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Number of affected entities.</returns>
        ValueTask<int> DeleteRangeAsync(IEnumerable<TEntity> entities, TContext context, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes the specified entitis.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="context">The current context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Number of affected entities.</returns>
        ValueTask<int> DeleteRangeAsync(IAsyncEnumerable<TEntity> entities, TContext context, CancellationToken cancellationToken = default);
    }
}

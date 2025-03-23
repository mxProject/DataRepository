using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace mxProject.Data.Repositories
{
    /// <summary>
    /// It provides the functionality required for a data repository to read entities asynchronously.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TKey">The key type.</typeparam>
    /// <typeparam name="TContext">The context type.</typeparam>
    public interface IAsyncReadDataRepositoryWithContext<TEntity, TKey, TContext> : IDataRepository<TEntity>
        where TContext : IDataRepositoryContext
    {
        /// <summary>
        /// Gets the entity corresponding to the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="context">The current context.</param>
        /// <returns>The entity.</returns>
        ValueTask<TEntity?> GetAsync(TKey key, TContext context);

        /// <summary>
        /// Gets the entities corresponding to the specified keys.
        /// </summary>
        /// <param name="keys">The keys.</param>
        /// <param name="context">The current context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entities.</returns>
        IAsyncEnumerable<TEntity> GetRangeAsync(IEnumerable<TKey> keys, TContext context, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the entities corresponding to the specified keys.
        /// </summary>
        /// <param name="keys">The keys.</param>
        /// <param name="context">The current context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entities.</returns>
        IAsyncEnumerable<TEntity> GetRangeAsync(IAsyncEnumerable<TKey> keys, TContext context, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets all entities.
        /// </summary>
        /// <param name="context">The current context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entities.</returns>
        IAsyncEnumerable<TEntity> GetAllAsync(TContext context, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets all keys.
        /// </summary>
        /// <param name="context">The current context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The keys</returns>
        IAsyncEnumerable<TKey> GetAllKeysAsync(TContext context, CancellationToken cancellationToken = default);
    }
}

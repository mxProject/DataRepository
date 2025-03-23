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
    public interface IAsyncReadDataRepository<TEntity, TKey> : IDataRepository<TEntity>
    {
        /// <summary>
        /// Gets the entity corresponding to the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The entity.</returns>
        ValueTask<TEntity?> GetAsync(TKey key);

        /// <summary>
        /// Gets the entities corresponding to the specified keys.
        /// </summary>
        /// <param name="keys">The keys.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entities.</returns>
        IAsyncEnumerable<TEntity> GetRangeAsync(IEnumerable<TKey> keys, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the entities corresponding to the specified keys.
        /// </summary>
        /// <param name="keys">The keys.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entities.</returns>
        IAsyncEnumerable<TEntity> GetRangeAsync(IAsyncEnumerable<TKey> keys, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets all entities.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entities.</returns>
        IAsyncEnumerable<TEntity> GetAllAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets all keys.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The keys</returns>
        IAsyncEnumerable<TKey> GetAllKeysAsync(CancellationToken cancellationToken = default);
    }
}

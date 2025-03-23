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
    /// <typeparam name="TPrimaryKey">The primary key type.</typeparam>
    /// <typeparam name="TUniqueKey">The unique key type.</typeparam>
    /// <typeparam name="TContext">The context type.</typeparam>
    public interface IAsyncReadDataRepositoryWithUniqueKeyWithContext<TEntity, TPrimaryKey, TUniqueKey, TContext> : IDataRepository<TEntity>
        where TContext : IDataRepositoryContext
    {
        /// <summary>
        /// Gets the entity corresponding to the specified primary key.
        /// </summary>
        /// <param name="primaryKey">The primary key.</param>
        /// <param name="context">The current context.</param>
        /// <returns>The entity.</returns>
        ValueTask<TEntity?> GetByPrimaryKeyAsync(TPrimaryKey primaryKey, TContext context);

        /// <summary>
        /// Gets the entities corresponding to the specified primary keys.
        /// </summary>
        /// <param name="primaryKeys">The primary keys.</param>
        /// <param name="context">The current context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entities.</returns>
        IAsyncEnumerable<TEntity> GetRangeByPrimaryKeyAsync(IEnumerable<TPrimaryKey> primaryKeys, TContext context, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the entities corresponding to the specified primary keys.
        /// </summary>
        /// <param name="primaryKeys">The primary keys.</param>
        /// <param name="context">The current context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entities.</returns>
        IAsyncEnumerable<TEntity> GetRangeByPrimaryKeyAsync(IAsyncEnumerable<TPrimaryKey> primaryKeys, TContext context, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the entity corresponding to the specified unique key.
        /// </summary>
        /// <param name="uniqueKey">The unique key.</param>
        /// <param name="context">The current context.</param>
        /// <returns>The entity.</returns>
        ValueTask<TEntity?> GetByUniqueKeyAsync(TUniqueKey uniqueKey, TContext context);

        /// <summary>
        /// Gets the entities corresponding to the specified unique keys.
        /// </summary>
        /// <param name="uniqueKeys">The unique keys.</param>
        /// <param name="context">The current context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entities.</returns>
        IAsyncEnumerable<TEntity> GetRangeByUniqueKeyAsync(IEnumerable<TUniqueKey> uniqueKeys, TContext context, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the entities corresponding to the specified unique keys.
        /// </summary>
        /// <param name="uniqueKeys">The unique keys.</param>
        /// <param name="context">The current context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entities.</returns>
        IAsyncEnumerable<TEntity> GetRangeByUniqueKeyAsync(IAsyncEnumerable<TUniqueKey> uniqueKeys, TContext context, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets all entities.
        /// </summary>
        /// <param name="context">The current context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entities.</returns>
        IAsyncEnumerable<TEntity> GetAllAsync(TContext context, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets all primary keys.
        /// </summary>
        /// <param name="context">The current context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The primary keys</returns>
        IAsyncEnumerable<TPrimaryKey> GetAllPrimaryKeysAsync(TContext context, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets all unique keys.
        /// </summary>
        /// <param name="context">The current context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The unique keys</returns>
        IAsyncEnumerable<TUniqueKey> GetAllUniqueKeysAsync(TContext context, CancellationToken cancellationToken = default);
    }
}

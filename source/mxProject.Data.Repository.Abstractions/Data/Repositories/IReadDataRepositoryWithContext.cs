using System;
using System.Collections.Generic;
using System.Text;

namespace mxProject.Data.Repositories
{
    /// <summary>
    /// It provides the functionality required for a data repository to read entities.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TKey">The key type.</typeparam>
    /// <typeparam name="TContext">The context type.</typeparam>
    public interface IReadDataRepositoryWithContext<TEntity, TKey, TContext> : IDataRepository<TEntity>
        where TContext : IDataRepositoryContext
    {
        /// <summary>
        /// Gets the entity corresponding to the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="context">The current context.</param>
        /// <returns>The entity.</returns>
        TEntity? Get(TKey key, TContext context);

        /// <summary>
        /// Gets the entities corresponding to the specified keys.
        /// </summary>
        /// <param name="keys">The keys.</param>
        /// <param name="context">The current context.</param>
        /// <returns>The entities.</returns>
        IEnumerable<TEntity> GetRange(IEnumerable<TKey> keys, TContext context);

        /// <summary>
        /// Gets all entities.
        /// </summary>
        /// <param name="context">The current context.</param>
        /// <returns>The entities.</returns>
        IEnumerable<TEntity> GetAll(TContext context);

        /// <summary>
        /// Gets all keys.
        /// </summary>
        /// <param name="context">The current context.</param>
        /// <returns>The keys</returns>
        IEnumerable<TKey> GetAllKeys(TContext context);
    }
}

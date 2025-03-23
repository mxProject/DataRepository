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
    public interface IReadDataRepository<TEntity, TKey> : IDataRepository<TEntity>
    {
        /// <summary>
        /// Gets the entity corresponding to the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The entity.</returns>
        TEntity? Get(TKey key);

        /// <summary>
        /// Gets the entities corresponding to the specified keys.
        /// </summary>
        /// <param name="keys">The keys.</param>
        /// <returns>The entities.</returns>
        IEnumerable<TEntity> GetRange(IEnumerable<TKey> keys);

        /// <summary>
        /// Gets all entities.
        /// </summary>
        /// <returns>The entities.</returns>
        IEnumerable<TEntity> GetAll();

        /// <summary>
        /// Gets all keys.
        /// </summary>
        /// <returns>The keys</returns>
        IEnumerable<TKey> GetAllKeys();
    }
}

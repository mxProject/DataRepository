using System;
using System.Collections.Generic;
using System.Text;

namespace mxProject.Data.Repositories
{
    /// <summary>
    /// Provides the functionality required to cache entities.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TKey">The key type.</typeparam>
    public interface IEntityCache<TKey, TEntity> : IDisposable
    {
        /// <summary>
        /// Gets the entity corresponding to the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="entity">The entity.</param>
        /// <returns>Returns true if the entity is retrieved, otherwise false.</returns>
        bool TryGet(TKey key, out TEntity entity);

        /// <summary>
        /// Stores the specified entity.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="entity">The entity.</param>
        void Store(TKey key, TEntity entity);

        /// <summary>
        /// Removes the entity corresponding to the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>Returns true if the entity is removed, otherwise false.</returns>
        bool Remove(TKey key);

        /// <summary>
        /// Clears the stored entities.
        /// </summary>
        void Clear();
    }
}

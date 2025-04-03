using System;
using System.Collections.Generic;
using System.Text;

namespace mxProject.Data.Repositories
{
    /// <summary>
    /// Provides the functionality needed to use an entity's key as the filename.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TKey">The key type.</typeparam>
    public interface IEntityFileNamingUsingKeyAsFileName<TEntity, TKey>
    {
        /// <summary>
        /// Gets the key corresponding to the specified entity file name.
        /// </summary>
        /// <param name="entityFilePath">The entity file name.</param>
        /// <returns>The key.</returns>
        TKey? GetKeyFromFileName(string entityFilePath);

        /// <summary>
        /// Gets the entity file name corresponding to the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The entity file name.</returns>
        string GetEntityFileName(TKey key);

        /// <summary>
        /// Gets the key for the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>The key.</returns>
        TKey GetKey(TEntity entity);
    }
}

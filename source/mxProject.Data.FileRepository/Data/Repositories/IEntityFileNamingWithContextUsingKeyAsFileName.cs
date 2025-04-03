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
    /// <typeparam name="TContext">The context type.</typeparam>
    public interface IEntityFileNamingWithContextUsingKeyAsFileName<TEntity, TKey, TContext>
        where TContext : IDataRepositoryContext
    {
        /// <summary>
        /// Gets the key corresponding to the specified entity file name.
        /// </summary>
        /// <param name="entityFilePath">The entity file name.</param>
        /// <param name="context">The current context.</param>
        /// <returns>The key.</returns>
        TKey? GetKeyFromFileName(string entityFilePath, TContext context);

        /// <summary>
        /// Gets the entity file name corresponding to the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="context">The current context.</param>
        /// <returns>The entity file name.</returns>
        string GetEntityFileName(TKey key, TContext context);

        /// <summary>
        /// Gets the key for the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="context">The current context.</param>
        /// <returns>The key.</returns>
        TKey GetKey(TEntity entity, TContext context);
    }
}

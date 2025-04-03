﻿using System;
using System.Collections.Generic;
using System.Text;

namespace mxProject.Data.Repositories
{
    /// <summary>
    /// Provides the functionality required to manage entity file paths using key files.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TKey">The key type.</typeparam>
    /// <typeparam name="TContext">The context type.</typeparam>
    public interface IEntityFileNamingWithContextUsingKeyFile<TEntity, TKey, TContext>
    {
        /// <summary>
        /// Gets a value indicating whether the specified file is a key file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="context">The current context.</param>
        /// <returns>Returns true if it is a key file, otherwise false.</returns>
        bool IsKeyFile(string filePath, TContext context);

        /// <summary>
        /// Gets a value indicating whether the specified file is an entity file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="context">The current context.</param>
        /// <returns>Returns true if it is an entity file, otherwise false.</returns>
        bool IsEntityFile(string filePath, TContext context);

        /// <summary>
        /// Gets the key corresponding to the specified key file name.
        /// </summary>
        /// <param name="keyFilePath">The key file name.</param>
        /// <param name="context">The current context.</param>
        /// <returns>The key.</returns>
        TKey GetKeyFromFileName(string keyFilePath, TContext context);

        /// <summary>
        /// Gets the key file name corresponding to the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="context">The current context.</param>
        /// <returns>The key file name.</returns>
        string GetKeyFileName(TKey key, TContext context);

        /// <summary>
        /// Gets the file path where the specified entity will be saved.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="context">The current context.</param>
        /// <returns>The file path.</returns>
        string GetEntitySaveFileName(TEntity entity, TContext context);

        /// <summary>
        /// Gets the key for the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="context">The current context.</param>
        /// <returns>The key.</returns>
        TKey GetKey(TEntity entity, TContext context);
    }
}

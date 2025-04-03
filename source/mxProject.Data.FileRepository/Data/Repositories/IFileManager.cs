using System;
using System.Collections.Generic;
using System.Text;

namespace mxProject.Data.Repositories
{
    /// <summary>
    /// Provides the functionality required to read and write files.
    /// </summary>
    public interface IFileManager
    {
        /// <summary>
        /// Gets a value that indicates whether to use ambient transactions using TransactionScope.
        /// </summary>
        bool UseTransactionScope { get; }

        /// <summary>
        /// Loads the entity from the specified file.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="filePath">The file path.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>The entity.</returns>
        TEntity LoadFromFile<TEntity>(string filePath, Encoding encoding);

        /// <summary>
        /// Saves the specified entity to the specified file.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="filePath">The file path.</param>
        /// <param name="encoding">The encoding.</param>
        void SaveToFile<TEntity>(TEntity entity, string filePath, Encoding encoding);

        /// <summary>
        /// Deletes the specified file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        void DeleteFile(string filePath);

        /// <summary>
        /// Writes the specified string to the specified file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="contents">The string.</param>
        /// <param name="encoding">The encoding.</param>
        void WriteAllText(string filePath, string contents, Encoding encoding);
    }
}

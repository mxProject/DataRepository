using mxProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace mxProject.Data.Repositories
{
    /// <summary>
    /// Basic implementation of a data repository using a file system.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TKey">The key type.</typeparam>
    /// <typeparam name="TContext">The context type.</typeparam>
    public abstract class FileRepositoryWithContextBase<TEntity, TKey, TContext> : IReadDataRepositoryWithContext<TEntity, TKey, TContext>, IWriteDataRepositoryWithContext<TEntity, TContext>
        where TContext : IDataRepositoryContext
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="dataDirectoryPath">The directory in which to save data files.</param>
        protected FileRepositoryWithContextBase(string dataDirectoryPath)
        {
            DataDirectoryPath = PathEx.GetFullPath(dataDirectoryPath);
        }

        /// <summary>
        /// Gets the directory in which to save data files.
        /// </summary>
        public string DataDirectoryPath { get; }

        /// <inheritdoc/>
        public abstract bool UseTransactionScope { get; }

        /// <inheritdoc/>
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #region get

        /// <inheritdoc/>
        public TEntity? Get(TKey key, TContext context)
        {
            var filePath = GetEntityFilePath(key, context);

            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath)) { return default; }

            return LoadFromFile(filePath!, context);
        }

        /// <inheritdoc/>
        public IEnumerable<TEntity> GetRange(IEnumerable<TKey> keys, TContext context)
        {
            foreach (var key in keys)
            {
                var entity = Get(key, context);

                if (entity != null) { yield return entity; }
            }
        }

        /// <inheritdoc/>
        public IEnumerable<TEntity> GetAll(TContext context)
        {
            foreach (var filePath in Directory.GetFiles(DataDirectoryPath, "", SearchOption.AllDirectories))
            {
                if (TryGetEntity(filePath, out var entity, context)) { yield return entity; }
            }
        }

        /// <inheritdoc/>
        public IEnumerable<TKey> GetAllKeys(TContext context)
        {
            foreach (var filePath in Directory.GetFiles(DataDirectoryPath, "", SearchOption.AllDirectories))
            {
                if (TryGetKey(filePath, out var key, context)) { yield return key; }
            }
        }

        #endregion

        #region insert

        /// <inheritdoc/>
        public int Insert(TEntity entity, TContext context)
        {
            var filePath = GetEntitySaveFilePath(entity, context);

            SaveToFile(entity, filePath, context);

            return 1;
        }

        /// <inheritdoc/>
        public int InsertRange(IEnumerable<TEntity> entities, TContext context)
        {
            int count = 0;

            foreach (var entity in entities)
            {
                count += Insert(entity, context);
            }

            return count;
        }

        #endregion

        #region update

        /// <inheritdoc/>
        public int Update(TEntity entity, TContext context)
        {
            var filePath = GetEntitySaveFilePath(entity, context);

            SaveToFile(entity, filePath, context);

            return 1;
        }

        /// <inheritdoc/>
        public int UpdateRange(IEnumerable<TEntity> entities, TContext context)
        {
            int count = 0;

            foreach (var entity in entities)
            {
                count += Update(entity, context);
            }

            return count;
        }

        #endregion

        #region delete

        /// <inheritdoc/>
        public int Delete(TEntity entity, TContext context)
        {
            var filePath = GetEntitySaveFilePath(entity, context);

            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath)) { return 0; }

            DeleteFile(filePath, context);

            return 1;
        }

        /// <inheritdoc/>
        public int DeleteRange(IEnumerable<TEntity> entities, TContext context)
        {
            int count = 0;

            foreach (var entity in entities)
            {
                count += Delete(entity, context);
            }

            return count;
        }

        #endregion

        /// <summary>
        /// Gets the key that corresponds to the specified file path.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="key">The identified Key.</param>
        /// <param name="context">The current context.</param>
        /// <returns>Returns true if the key is identified, otherwise false.</returns>
        protected abstract bool TryGetKey(string filePath, out TKey key, TContext context);

        /// <summary>
        /// Gets the entity that corresponds to the specified file path.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="entity">The deserialized entity.</param>
        /// <param name="context">The current context.</param>
        /// <returns>Returns true if the entity is deserialized, otherwise false.</returns>
        protected abstract bool TryGetEntity(string filePath, out TEntity entity, TContext context);

        /// <summary>
        /// Gets the file path that corresponds to the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="context">The current context.</param>
        /// <returns>The file path.</returns>
        protected abstract string? GetEntityFilePath(TKey key, TContext context);

        /// <summary>
        /// Gets the file path where the specified entity will be saved.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="context">The current context.</param>
        /// <returns>The file path.</returns>
        protected abstract string GetEntitySaveFilePath(TEntity entity, TContext context);

        /// <summary>
        /// Loads the entity from the specified file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="context">The current context.</param>
        /// <returns>The entity.</returns>
        protected abstract TEntity LoadFromFile(string filePath, TContext context);

        /// <summary>
        /// Saves the specified entity to the specified file.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="filePath">The file path.</param>
        /// <param name="context">The current context.</param>
        protected abstract void SaveToFile(TEntity entity, string filePath, TContext context);

        /// <summary>
        /// Deletes the specified file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="context">The current context.</param>
        protected abstract void DeleteFile(string filePath, TContext context);
    }
}

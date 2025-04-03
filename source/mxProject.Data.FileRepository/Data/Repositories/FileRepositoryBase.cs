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
    public abstract class FileRepositoryBase<TEntity, TKey> : IReadDataRepository<TEntity, TKey>, IWriteDataRepository<TEntity>
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="dataDirectoryPath">The directory in which to save data files.</param>
        protected FileRepositoryBase(string dataDirectoryPath)
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
        public TEntity? Get(TKey key)
        {
            var filePath = GetEntityFilePath(key);

            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath)) { return default; }

            return LoadFromFile(filePath!);
        }

        /// <inheritdoc/>
        public IEnumerable<TEntity> GetRange(IEnumerable<TKey> keys)
        {
            foreach (var key in keys)
            {
                var entity = Get(key);

                if (entity != null) { yield return entity; }
            }
        }

        /// <inheritdoc/>
        public IEnumerable<TEntity> GetAll()
        {
            foreach (var filePath in Directory.GetFiles(DataDirectoryPath, "", SearchOption.AllDirectories))
            {
                if (TryGetEntity(filePath, out var entity)) { yield return entity; }
            }
        }

        /// <inheritdoc/>
        public IEnumerable<TKey> GetAllKeys()
        {
            foreach (var filePath in Directory.GetFiles(DataDirectoryPath, "", SearchOption.AllDirectories))
            {
                if (TryGetKey(filePath, out var key)) { yield return key; }
            }
        }

        #endregion

        #region insert

        /// <inheritdoc/>
        public int Insert(TEntity entity)
        {
            var filePath = GetEntitySaveFilePath(entity);

            SaveToFile(entity, filePath);

            return 1;
        }

        /// <inheritdoc/>
        public int InsertRange(IEnumerable<TEntity> entities)
        {
            int count = 0;

            foreach (var entity in entities)
            {
                count += Insert(entity);
            }

            return count;
        }

        #endregion

        #region update

        /// <inheritdoc/>
        public int Update(TEntity entity)
        {
            var filePath = GetEntitySaveFilePath(entity);

            SaveToFile(entity, filePath);

            return 1;
        }

        /// <inheritdoc/>
        public int UpdateRange(IEnumerable<TEntity> entities)
        {
            int count = 0;

            foreach (var entity in entities)
            {
                count += Update(entity);
            }

            return count;
        }

        #endregion

        #region delete

        /// <inheritdoc/>
        public int Delete(TEntity entity)
        {
            var filePath = GetEntitySaveFilePath(entity);

            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath)) { return 0; }

            DeleteFile(filePath);

            return 1;
        }

        /// <inheritdoc/>
        public int DeleteRange(IEnumerable<TEntity> entities)
        {
            int count = 0;

            foreach (var entity in entities)
            {
                count += Delete(entity);
            }

            return count;
        }

        #endregion

        /// <summary>
        /// Gets the key that corresponds to the specified file path.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="key">The identified Key.</param>
        /// <returns>Returns true if the key is identified, otherwise false.</returns>
        protected abstract bool TryGetKey(string filePath, out TKey key);

        /// <summary>
        /// Gets the entity that corresponds to the specified file path.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="entity">The deserialized entity.</param>
        /// <returns>Returns true if the entity is deserialized, otherwise false.</returns>
        protected abstract bool TryGetEntity(string filePath, out TEntity entity);

        /// <summary>
        /// Gets the file path that corresponds to the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The file path.</returns>
        protected abstract string? GetEntityFilePath(TKey key);

        /// <summary>
        /// Gets the file path where the specified entity will be saved.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>The file path.</returns>
        protected abstract string GetEntitySaveFilePath(TEntity entity);

        /// <summary>
        /// Loads the entity from the specified file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns>The entity.</returns>
        protected abstract TEntity LoadFromFile(string filePath);

        /// <summary>
        /// Saves the specified entity to the specified file.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="filePath">The file path.</param>
        protected abstract void SaveToFile(TEntity entity, string filePath);

        /// <summary>
        /// Deletes the specified file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        protected abstract void DeleteFile(string filePath);
    }
}

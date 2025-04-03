using mxProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace mxProject.Data.Repositories
{
    /// <summary>
    /// Data repository that uses the file system, using the entity's key as the filename.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TKey">The key type.</typeparam>
    public class FileRepositoryUsingKeyAsFileName<TEntity, TKey> : FileRepositoryBase<TEntity, TKey>
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="dataDirectoryPath">The directory in which to save data files.</param>
        /// <param name="fileNaming">The logic that determines the entity file name.</param>
        /// <param name="fileManager">The file manager.</param>
        /// <param name="encoding">The encoding.</param>
        public FileRepositoryUsingKeyAsFileName(string dataDirectoryPath, IEntityFileNamingUsingKeyAsFileName<TEntity, TKey> fileNaming, IFileManager fileManager, Encoding encoding)
            : base(dataDirectoryPath)
        {
            m_EntityFileNaming = fileNaming;
            m_FileManager = fileManager;
            m_Encoding = encoding;
        }

        private readonly IEntityFileNamingUsingKeyAsFileName<TEntity, TKey> m_EntityFileNaming;
        private readonly IFileManager m_FileManager;
        private readonly Encoding m_Encoding;

        /// <inheritdoc/>
        public override bool UseTransactionScope
        {
            get { return m_FileManager.UseTransactionScope; }
        }

        /// <inheritdoc/>
        protected override bool TryGetKey(string filePath, out TKey key)
        {
            var keyOrNull = m_EntityFileNaming.GetKeyFromFileName(PathEx.ToRalativePath(filePath, DataDirectoryPath));

            if (keyOrNull == null)
            {
                key = default!;
                return false;
            }
            else
            {
                key = keyOrNull;
                return true;
            }
        }

        /// <inheritdoc/>
        protected override bool TryGetEntity(string filePath, out TEntity entity)
        {
            if (!TryGetKey(filePath, out var key))
            {
                entity = default!;
                return false;
            }
            else
            {
                entity = Get(key)!;
                return entity != null;
            }
        }

        /// <inheritdoc/>
        protected override string? GetEntityFilePath(TKey key)
        {
            return Path.Combine(DataDirectoryPath, m_EntityFileNaming.GetEntityFileName(key));
        }

        /// <inheritdoc/>
        protected override string GetEntitySaveFilePath(TEntity entity)
        {
            return GetEntityFilePath(m_EntityFileNaming.GetKey(entity))!;
        }

        /// <inheritdoc/>
        protected override TEntity LoadFromFile(string filePath)
        {
            return m_FileManager.LoadFromFile<TEntity>(filePath, m_Encoding);
        }

        /// <inheritdoc/>
        protected override void SaveToFile(TEntity entity, string filePath)
        {
            m_FileManager.SaveToFile(entity, filePath, m_Encoding);
        }

        /// <inheritdoc/>
        protected override void DeleteFile(string filePath)
        {
            m_FileManager.DeleteFile(filePath);
        }
    }
}

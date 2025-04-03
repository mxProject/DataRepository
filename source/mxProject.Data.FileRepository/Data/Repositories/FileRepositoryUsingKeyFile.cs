using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace mxProject.Data.Repositories
{
    /// <summary>
    /// Data repository that uses the file system, using a key file to manage path to entity file.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TKey">The key type.</typeparam>
    public class FileRepositoryUsingKeyFile<TEntity, TKey> : FileRepositoryBase<TEntity, TKey>
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="dataDirectoryPath">The directory in which to save data files.</param>
        /// <param name="fileNaming">The logic that determines the entity file name.</param>
        /// <param name="fileManager">The file manager.</param>
        /// <param name="encoding">The encoding.</param>
        public FileRepositoryUsingKeyFile(string dataDirectoryPath, IEntityFileNamingUsingKeyFile<TEntity, TKey> fileNaming, IFileManager fileManager, Encoding encoding)
            : base(dataDirectoryPath)
        {
            m_EntityFileNaming = fileNaming;
            m_FileManager = fileManager;
            m_Encoding = encoding;
        }

        private readonly IEntityFileNamingUsingKeyFile<TEntity, TKey> m_EntityFileNaming;
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
            if (!m_EntityFileNaming.IsKeyFile(filePath))
            {
                key = default!;
                return false;
            }

            key = m_EntityFileNaming.GetKeyFromFileName(PathEx.ToRalativePath(filePath, DataDirectoryPath));
            return true;
        }

        /// <inheritdoc/>
        protected override bool TryGetEntity(string filePath, out TEntity entity)
        {
            if (!m_EntityFileNaming.IsEntityFile(filePath))
            {
                entity = default!;
                return false;
            }

            entity = LoadFromFile(filePath);
            return true;
        }

        /// <inheritdoc/>
        protected override string GetEntityFilePath(TKey key)
        {
            var keyFilePath = Path.Combine(DataDirectoryPath, m_EntityFileNaming.GetKeyFileName(key));

            if (!File.Exists(keyFilePath)) { return null!; }

            return PathEx.ToAbsolutePath(File.ReadAllText(keyFilePath), DataDirectoryPath);
        }

        /// <inheritdoc/>
        protected override string GetEntitySaveFilePath(TEntity entity)
        {
            return PathEx.ToAbsolutePath(m_EntityFileNaming.GetEntitySaveFileName(entity), DataDirectoryPath);
        }

        /// <inheritdoc/>
        protected override TEntity LoadFromFile(string filePath)
        {
            return m_FileManager.LoadFromFile<TEntity>(filePath, m_Encoding);
        }

        /// <inheritdoc/>
        /// <exception cref="DuplicateNameException">
        /// There is an entity file with different key.
        /// </exception>
        protected override void SaveToFile(TEntity entity, string filePath)
        {
            var key = m_EntityFileNaming.GetKey(entity);
            var keyFilePath = Path.Combine(DataDirectoryPath, m_EntityFileNaming.GetKeyFileName(key));

            var currentEntityFilePath = GetEntityFilePath(m_EntityFileNaming.GetKey(entity));

            if (currentEntityFilePath != null && currentEntityFilePath != filePath)
            {
                if (File.Exists(currentEntityFilePath))
                {
                    m_FileManager.DeleteFile(currentEntityFilePath);
                }
            }

            if (File.Exists(filePath))
            {
                var currentEntity = LoadFromFile(filePath);

                if (!Equals(m_EntityFileNaming.GetKey(currentEntity), key))
                {
                    throw new DuplicateNameException("There is an entity file with different key.");
                }
            }

            m_FileManager.WriteAllText(keyFilePath, PathEx.ToRalativePath(filePath, DataDirectoryPath), m_Encoding);

            m_FileManager.SaveToFile(entity, filePath, m_Encoding);
        }

        /// <inheritdoc/>
        protected override void DeleteFile(string filePath)
        {
            var entity = LoadFromFile(filePath);

            var keyFilePath = Path.Combine(DataDirectoryPath, m_EntityFileNaming.GetKeyFileName(m_EntityFileNaming.GetKey(entity)));

            if (File.Exists(keyFilePath))
            {
                m_FileManager.DeleteFile(keyFilePath);
            }

            m_FileManager.DeleteFile(filePath);
        }
    }
}

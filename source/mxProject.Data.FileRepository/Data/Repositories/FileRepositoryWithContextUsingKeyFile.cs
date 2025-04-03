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
    /// <typeparam name="TContext">The context type.</typeparam>
    public class FileRepositoryWithContextUsingKeyFile<TEntity, TKey, TContext> : FileRepositoryWithContextBase<TEntity, TKey, TContext>
        where TContext : IDataRepositoryContext
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="dataDirectoryPath">The directory in which to save data files.</param>
        /// <param name="fileNaming">The logic that determines the entity file name.</param>
        /// <param name="fileManager">The file manager.</param>
        /// <param name="encoding">The encoding.</param>
        public FileRepositoryWithContextUsingKeyFile(string dataDirectoryPath, IEntityFileNamingWithContextUsingKeyFile<TEntity, TKey, TContext> fileNaming, IFileManagerWithContext<TContext> fileManager, Encoding encoding)
            : base(dataDirectoryPath)
        {
            m_EntityFileNaming = fileNaming;
            m_FileManager = fileManager;
            m_Encoding = encoding;
        }

        private readonly IEntityFileNamingWithContextUsingKeyFile<TEntity, TKey, TContext> m_EntityFileNaming;
        private readonly IFileManagerWithContext<TContext> m_FileManager;
        private readonly Encoding m_Encoding;

        /// <inheritdoc/>
        public override bool UseTransactionScope
        {
            get { return m_FileManager.UseTransactionScope; }
        }

        /// <inheritdoc/>
        protected override bool TryGetKey(string filePath, out TKey key, TContext context)
        {
            if (!m_EntityFileNaming.IsKeyFile(filePath, context))
            {
                key = default!;
                return false;
            }

            key = m_EntityFileNaming.GetKeyFromFileName(PathEx.ToRalativePath(filePath, DataDirectoryPath), context);
            return true;
        }

        /// <inheritdoc/>
        protected override bool TryGetEntity(string filePath, out TEntity entity, TContext context)
        {
            if (!m_EntityFileNaming.IsEntityFile(filePath, context))
            {
                entity = default!;
                return false;
            }

            entity = LoadFromFile(filePath, context);
            return true;
        }

        /// <inheritdoc/>
        protected override string GetEntityFilePath(TKey key, TContext context)
        {
            var keyFilePath = Path.Combine(DataDirectoryPath, m_EntityFileNaming.GetKeyFileName(key, context));

            if (!File.Exists(keyFilePath)) { return null!; }

            return PathEx.ToAbsolutePath(File.ReadAllText(keyFilePath), DataDirectoryPath);
        }

        /// <inheritdoc/>
        protected override string GetEntitySaveFilePath(TEntity entity, TContext context)
        {
            return PathEx.ToAbsolutePath(m_EntityFileNaming.GetEntitySaveFileName(entity, context), DataDirectoryPath);
        }

        /// <inheritdoc/>
        protected override TEntity LoadFromFile(string filePath, TContext context)
        {
            return m_FileManager.LoadFromFile<TEntity>(filePath, m_Encoding, context);
        }

        /// <inheritdoc/>
        /// <exception cref="DuplicateNameException">
        /// There is an entity file with different key.
        /// </exception>
        protected override void SaveToFile(TEntity entity, string filePath, TContext context)
        {
            var key = m_EntityFileNaming.GetKey(entity, context);
            var keyFilePath = Path.Combine(DataDirectoryPath, m_EntityFileNaming.GetKeyFileName(key, context));

            var currentEntityFilePath = GetEntityFilePath(m_EntityFileNaming.GetKey(entity, context), context);

            if (currentEntityFilePath != null && currentEntityFilePath != filePath)
            {
                if (File.Exists(currentEntityFilePath))
                {
                    m_FileManager.DeleteFile(currentEntityFilePath, context);
                }
            }

            if (File.Exists(filePath))
            {
                var currentEntity = LoadFromFile(filePath, context);

                if (!Equals(m_EntityFileNaming.GetKey(currentEntity, context), key))
                {
                    throw new DuplicateNameException("There is an entity file with different key.");
                }
            }

            m_FileManager.WriteAllText(keyFilePath, PathEx.ToRalativePath(filePath, DataDirectoryPath), m_Encoding, context);

            m_FileManager.SaveToFile(entity, filePath, m_Encoding, context);
        }

        /// <inheritdoc/>
        protected override void DeleteFile(string filePath, TContext context)
        {
            var entity = LoadFromFile(filePath, context);

            var keyFilePath = Path.Combine(DataDirectoryPath, m_EntityFileNaming.GetKeyFileName(m_EntityFileNaming.GetKey(entity, context), context));

            if (File.Exists(keyFilePath))
            {
                m_FileManager.DeleteFile(keyFilePath, context);
            }

            m_FileManager.DeleteFile(filePath, context);
        }
    }
}

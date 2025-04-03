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
    /// <typeparam name="TContext">The context type.</typeparam>
    public class FileRepositoryWithContextUsingKeyAsFileName<TEntity, TKey, TContext> : FileRepositoryWithContextBase<TEntity, TKey, TContext>
        where TContext : IDataRepositoryContext
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="dataDirectoryPath">The directory in which to save data files.</param>
        /// <param name="fileNaming">The logic that determines the entity file name.</param>
        /// <param name="fileManager">The file manager.</param>
        /// <param name="encoding">The encoding.</param>
        public FileRepositoryWithContextUsingKeyAsFileName(string dataDirectoryPath, IEntityFileNamingWithContextUsingKeyAsFileName<TEntity, TKey, TContext> fileNaming, IFileManagerWithContext<TContext> fileManager, Encoding encoding)
            : base(dataDirectoryPath)
        {
            m_EntityFileNaming = fileNaming;
            m_FileManager = fileManager;
            m_Encoding = encoding;
        }

        private readonly IEntityFileNamingWithContextUsingKeyAsFileName<TEntity, TKey, TContext> m_EntityFileNaming;
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
            var keyOrNull = m_EntityFileNaming.GetKeyFromFileName(PathEx.ToRalativePath(filePath, DataDirectoryPath), context);

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
        protected override bool TryGetEntity(string filePath, out TEntity entity, TContext context)
        {
            if (!TryGetKey(filePath, out var key, context))
            {
                entity = default!;
                return false;
            }
            else
            {
                entity = Get(key, context)!;
                return entity != null;
            }
        }

        /// <inheritdoc/>
        protected override string? GetEntityFilePath(TKey key, TContext context)
        {
            return Path.Combine(DataDirectoryPath, m_EntityFileNaming.GetEntityFileName(key, context));
        }

        /// <inheritdoc/>
        protected override string GetEntitySaveFilePath(TEntity entity, TContext context)
        {
            return GetEntityFilePath(m_EntityFileNaming.GetKey(entity, context), context)!;
        }

        /// <inheritdoc/>
        protected override TEntity LoadFromFile(string filePath, TContext context)
        {
            return m_FileManager.LoadFromFile<TEntity>(filePath, m_Encoding, context);
        }

        /// <inheritdoc/>
        protected override void SaveToFile(TEntity entity, string filePath, TContext context)
        {
            m_FileManager.SaveToFile(entity, filePath, m_Encoding, context);
        }

        /// <inheritdoc/>
        protected override void DeleteFile(string filePath, TContext context)
        {
            m_FileManager.DeleteFile(filePath, context);
        }
    }
}

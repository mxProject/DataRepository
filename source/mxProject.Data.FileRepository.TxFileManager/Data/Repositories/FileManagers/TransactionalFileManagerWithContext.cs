using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ChinhDo.Transactions;

namespace mxProject.Data.Repositories.FileManagers
{
    /// <summary>
    /// Manages file operations with transactional support using TxFileManager.
    /// </summary>
    /// <typeparam name="TContext">The context type.</typeparam>
    public class TransactionalFileManagerWithContext<TContext> : IFileManagerWithContext<TContext>
        where TContext : IDataRepositoryContext
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="serializer">The serializer to use for file operations.</param>
        /// <param name="temporaryDirectoryPath">The path of the temporary directory.</param>
        public TransactionalFileManagerWithContext(IEntityFileSerializerWithContext<TContext> serializer, string? temporaryDirectoryPath = null)
        {
            m_Serializer = serializer;

            if (string.IsNullOrWhiteSpace(temporaryDirectoryPath))
            {
                m_FileManager = new TxFileManager();
            }
            else
            {
                m_FileManager = new TxFileManager(temporaryDirectoryPath);
            }
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="serializer">The serializer to use for file operations.</param>
        /// <param name="temporaryDirectoryPath">The path of the temporary directory.</param>
        public TransactionalFileManagerWithContext(IEntityFileSerializer serializer, string? temporaryDirectoryPath = null)
            : this(serializer.WithContext<TContext>(), temporaryDirectoryPath)
        {
        }

        private readonly IEntityFileSerializerWithContext<TContext> m_Serializer;
        private readonly TxFileManager m_FileManager;

        /// <inheritdoc/>
        public bool UseTransactionScope => true;

        /// <inheritdoc/>
        public void DeleteFile(string filePath, TContext context)
        {
            m_FileManager.Delete(filePath);
        }

        /// <inheritdoc/>
        public TEntity LoadFromFile<TEntity>(string filePath, Encoding encoding, TContext context)
        {
            using var stream = File.OpenRead(filePath);

            return m_Serializer.Deserialize<TEntity>(stream, encoding, context);
        }

        /// <inheritdoc/>
        public void SaveToFile<TEntity>(TEntity entity, string filePath, Encoding encoding, TContext context)
        {
            var directory = Path.GetDirectoryName(filePath);

            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                m_FileManager.CreateDirectory(directory);
            }

            if (File.Exists(filePath))
            {
                m_FileManager.Snapshot(filePath);
            }

            using var stream = new MemoryStream();

            m_Serializer.Serialize(entity, stream, encoding, context);

            stream.Flush();
            stream.Position = 0;

            using var reader = new StreamReader(stream, encoding);

            var json = reader.ReadToEnd();

            m_FileManager.WriteAllText(filePath, json, encoding);
        }

        /// <inheritdoc/>
        public void WriteAllText(string filePath, string contents, Encoding encoding, TContext context)
        {
            var directory = Path.GetDirectoryName(filePath);

            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                m_FileManager.CreateDirectory(directory);
            }

            if (File.Exists(filePath))
            {
                m_FileManager.Snapshot(filePath);
            }

            m_FileManager.WriteAllText(filePath, contents, encoding);
        }
    }
}

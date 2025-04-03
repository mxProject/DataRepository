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
    public class TransactionalFileManager : IFileManager
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="serializer">The serializer to use for file operations.</param>
        /// <param name="temporaryDirectoryPath">The path of the temporary directory.</param>
        public TransactionalFileManager(IEntityFileSerializer serializer, string? temporaryDirectoryPath = null)
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

        private readonly IEntityFileSerializer m_Serializer;
        private readonly TxFileManager m_FileManager;

        /// <inheritdoc/>
        public bool UseTransactionScope => true;

        /// <inheritdoc/>
        public void DeleteFile(string filePath)
        {
            m_FileManager.Delete(filePath);
        }

        /// <inheritdoc/>
        public TEntity LoadFromFile<TEntity>(string filePath, Encoding encoding)
        {
            using var stream = File.OpenRead(filePath);

            return m_Serializer.Deserialize<TEntity>(stream, encoding);
        }

        /// <inheritdoc/>
        public void SaveToFile<TEntity>(TEntity entity, string filePath, Encoding encoding)
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

            m_Serializer.Serialize(entity, stream, encoding);

            stream.Flush();
            stream.Position = 0;

            using var reader = new StreamReader(stream, encoding);

            var json = reader.ReadToEnd();

            m_FileManager.WriteAllText(filePath, json, encoding);
        }

        /// <inheritdoc/>
        public void WriteAllText(string filePath, string contents, Encoding encoding)
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

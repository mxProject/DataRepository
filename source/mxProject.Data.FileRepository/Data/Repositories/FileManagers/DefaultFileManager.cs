using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace mxProject.Data.Repositories.FileManagers
{
    /// <summary>
    /// Default implementation of the <see cref="IFileManager"/> interface.
    /// </summary>
    public class DefaultFileManager : IFileManager
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="serializer">The serializer to use for file operations.</param>
        public DefaultFileManager(IEntityFileSerializer serializer)
        {
            m_Serializer = serializer;
        }

        private readonly IEntityFileSerializer m_Serializer;

        /// <inheritdoc/>
        public bool UseTransactionScope => false;

        /// <inheritdoc/>
        public void DeleteFile(string filePath)
        {
            File.Delete(filePath);
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
                Directory.CreateDirectory(directory);
            }

            using var stream = File.Create(filePath);

            m_Serializer.Serialize(entity, stream, encoding);

            stream.Flush();
        }

        /// <inheritdoc/>
        public void WriteAllText(string filePath, string contents, Encoding encoding)
        {
            var directory = Path.GetDirectoryName(filePath);

            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllText(filePath, contents, encoding);
        }
    }
}

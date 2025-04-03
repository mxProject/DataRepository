using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace mxProject.Data.Repositories.FileManagers
{
    /// <summary>
    /// Default implementation of the <see cref="IFileManagerWithContext{TContext}"/> interface.
    /// </summary>
    /// <typeparam name="TContext">The context type.</typeparam>
    public class DefaultFileManagerWithContext<TContext> : IFileManagerWithContext<TContext>
        where TContext : IDataRepositoryContext
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="serializer">The serializer to use for file operations.</param>
        public DefaultFileManagerWithContext(IEntityFileSerializerWithContext<TContext> serializer)
        {
            m_Serializer = serializer;
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="serializer">The serializer to use for file operations.</param>
        public DefaultFileManagerWithContext(IEntityFileSerializer serializer)
        {
            m_Serializer = serializer.WithContext<TContext>();
        }

        private readonly IEntityFileSerializerWithContext<TContext> m_Serializer;

        /// <inheritdoc/>
        public bool UseTransactionScope => false;

        /// <inheritdoc/>
        public void DeleteFile(string filePath, TContext context)
        {
            File.Delete(filePath);
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
                Directory.CreateDirectory(directory);
            }

            using var stream = File.Create(filePath);

            m_Serializer.Serialize(entity, stream, encoding, context);

            stream.Flush();
        }

        /// <inheritdoc/>
        public void WriteAllText(string filePath, string contents, Encoding encoding, TContext context)
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

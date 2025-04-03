using System;
using System.Collections.Generic;
using System.Text;

namespace mxProject.Data.Repositories.FileNamings
{
    /// <summary>
    /// Converts keys to file names.
    /// </summary>
    /// <typeparam name="TKey">The key type.</typeparam>
    /// <typeparam name="TContext">The context type.</typeparam>
    internal class KeyFileNameConverterWithContext<TKey, TContext> : IKeyFileNameConverterWithContext<TKey, TContext>
        where TContext : IDataRepositoryContext
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="fileExtension">The file extension.</param>
        /// <param name="toFileName">The function to convert from key to file name.</param>
        /// <param name="fromFileName">The function to convert from file name to key.</param>
        internal KeyFileNameConverterWithContext(string fileExtension, Func<TKey, TContext, string> toFileName, Func<string, TContext, TKey> fromFileName)
        {
            m_FileExtension = fileExtension;
            m_ToFileName = toFileName;
            m_FromFileName = fromFileName;
        }

        private readonly string m_FileExtension;
        private readonly Func<TKey, TContext, string> m_ToFileName;
        private readonly Func<string, TContext, TKey> m_FromFileName;

        /// <inheritdoc/>
        public string FileExtension => m_FileExtension;

        /// <inheritdoc/>
        public TKey FromFileNameWithoutExtension(string fileName, TContext context)
        {
            return m_FromFileName(fileName, context);
        }

        /// <inheritdoc/>
        public string ToFileNameWithoutExtension(TKey key, TContext context)
        {
            return m_ToFileName(key, context);
        }
    }
}

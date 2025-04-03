using System;
using System.Collections.Generic;
using System.Text;

namespace mxProject.Data.Repositories.FileNamings
{
    /// <summary>
    /// Converts keys to file names.
    /// </summary>
    /// <typeparam name="TKey">The key type.</typeparam>
    internal class KeyFileNameConverter<TKey> : IKeyFileNameConverter<TKey>
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="fileExtension">The file extension.</param>
        /// <param name="toFileName">The function to convert from key to file name.</param>
        /// <param name="fromFileName">The function to convert from file name to key.</param>
        internal KeyFileNameConverter(string fileExtension, Func<TKey, string> toFileName, Func<string, TKey> fromFileName)
        {
            m_FileExtension = fileExtension;
            m_ToFileName = toFileName;
            m_FromFileName = fromFileName;
        }

        private readonly string m_FileExtension;
        private readonly Func<TKey, string> m_ToFileName;
        private readonly Func<string, TKey> m_FromFileName;

        /// <inheritdoc/>
        public string FileExtension => m_FileExtension;

        /// <inheritdoc/>
        public TKey FromFileNameWithoutExtension(string fileName)
        {
            return m_FromFileName(fileName);
        }

        /// <inheritdoc/>
        public string ToFileNameWithoutExtension(TKey key)
        {
            return m_ToFileName(key);
        }
    }
}

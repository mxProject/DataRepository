using mxProject.Data.Repositories.FileNamings;
using System;
using System.Collections.Generic;
using System.Text;

namespace mxProject.Data.Repositories
{
    /// <summary>
    /// Provides functionality to convert a key to a file name.
    /// </summary>
    /// <typeparam name="TKey">The key type.</typeparam>
    public interface IKeyFileNameConverter<TKey>
    {
        /// <summary>
        /// Gets the file extension.
        /// </summary>
        string FileExtension { get; }

        /// <summary>
        /// Converts the specified key to a file name without extension.
        /// </summary>
        /// <param name="key">The key to convert.</param>
        /// <returns>The file name without extension.</returns>
        string ToFileNameWithoutExtension(TKey key);

        /// <summary>
        /// Converts the specified file name without extension to a key.
        /// </summary>
        /// <param name="fileName">The file name without extension.</param>
        /// <returns>The key.</returns>
        TKey FromFileNameWithoutExtension(string fileName);
    }
}

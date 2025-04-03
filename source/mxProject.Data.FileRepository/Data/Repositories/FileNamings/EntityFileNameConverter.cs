using System;
using System.Collections.Generic;
using System.Text;

namespace mxProject.Data.Repositories.FileNamings
{
    /// <summary>
    /// Converts entities to file names.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    internal class EntityFileNameConverter<TEntity> : IEntityFileNameConverter<TEntity>
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="fileExtension">The file extension.</param>
        /// <param name="toFileName">The function to convert from entity to file name.</param>
        internal EntityFileNameConverter(string fileExtension, Func<TEntity, string> toFileName)
        {
            m_FileExtension = fileExtension;
            m_ToFileName = toFileName;
        }

        private readonly string m_FileExtension;
        private readonly Func<TEntity, string> m_ToFileName;

        /// <inheritdoc/>
        public string FileExtension => m_FileExtension;

        /// <inheritdoc/>
        public string ToFileNameWithoutExtension(TEntity key)
        {
            return m_ToFileName(key);
        }
    }
}

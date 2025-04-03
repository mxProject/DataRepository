using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace mxProject.Data.Repositories.FileNamings
{
    /// <summary>
    /// Provides the functionality needed to use an entity's key as the filename.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TKey">The key type.</typeparam>
    public class EntityFileNamingUsingKeyAsFileName<TEntity, TKey> : IEntityFileNamingUsingKeyAsFileName<TEntity, TKey>
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="keyGetter">The function to get the key from the entity.</param>
        /// <param name="fileNameConverter">The converter to convert the key to a file name.</param>
        public EntityFileNamingUsingKeyAsFileName(Func<TEntity, TKey> keyGetter, IKeyFileNameConverter<TKey> fileNameConverter)
        {
            m_KeyGetter = keyGetter;
            m_FileNameConverter = fileNameConverter;
        }

        private readonly Func<TEntity, TKey> m_KeyGetter;
        private readonly IKeyFileNameConverter<TKey> m_FileNameConverter;

        /// <inheritdoc/>
        public string GetEntityFileName(TKey key)
        {
            return $"{m_FileNameConverter.ToFileNameWithoutExtension(key)}{PathEx.NormalizeExtension(m_FileNameConverter.FileExtension)}";
        }

        /// <inheritdoc/>
        public TKey GetKey(TEntity entity)
        {
            return m_KeyGetter(entity);
        }

        /// <inheritdoc/>
        public TKey? GetKeyFromFileName(string entityFilePath)
        {
            return m_FileNameConverter.FromFileNameWithoutExtension(Path.GetFileNameWithoutExtension(entityFilePath));
        }
    }
}

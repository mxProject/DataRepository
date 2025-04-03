using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace mxProject.Data.Repositories.FileNamings
{
    /// <summary>
    /// Provides the functionality required to manage entity file paths using key files.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TKey">The key type.</typeparam>
    public class EntityFileNamingUsingKeyFile<TEntity, TKey> : IEntityFileNamingUsingKeyFile<TEntity, TKey>
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="keyGetter">The function to get the key from the entity.</param>
        /// <param name="keyFileNameConverter">The key file name converter.</param>
        /// <param name="entityFileNameConverter">The entity file name converter.</param>
        public EntityFileNamingUsingKeyFile(Func<TEntity, TKey> keyGetter, IKeyFileNameConverter<TKey> keyFileNameConverter, IEntityFileNameConverter<TEntity> entityFileNameConverter)
        {
            m_KeyGetter = keyGetter;
            m_KeyFileNameConverter = keyFileNameConverter;
            m_EntityFileNameConverter = entityFileNameConverter;
        }

        private readonly Func<TEntity, TKey> m_KeyGetter;
        private readonly IKeyFileNameConverter<TKey> m_KeyFileNameConverter;
        private readonly IEntityFileNameConverter<TEntity> m_EntityFileNameConverter;

        /// <inheritdoc/>
        public bool IsEntityFile(string filePath)
        {
            if (!File.Exists(filePath)) { return false; }
            return string.Compare(Path.GetExtension(filePath), PathEx.NormalizeExtension(m_EntityFileNameConverter.FileExtension), true) == 0;
        }

        /// <inheritdoc/>
        public bool IsKeyFile(string filePath)
        {
            if (!File.Exists(filePath)) { return false; }
            return string.Compare(Path.GetExtension(filePath), PathEx.NormalizeExtension(m_KeyFileNameConverter.FileExtension), true) == 0;
        }

        /// <inheritdoc/>
        public TKey GetKey(TEntity entity)
        {
            return m_KeyGetter(entity);
        }

        /// <inheritdoc/>
        public string GetKeyFileName(TKey key)
        {
            return @$"keys\{m_KeyFileNameConverter.ToFileNameWithoutExtension(key)}{PathEx.NormalizeExtension(m_KeyFileNameConverter.FileExtension)}";
        }

        /// <inheritdoc/>
        public string GetEntitySaveFileName(TEntity entity)
        {
            return @$"entities\{m_EntityFileNameConverter.ToFileNameWithoutExtension(entity)}{PathEx.NormalizeExtension(m_EntityFileNameConverter.FileExtension)}";
        }

        /// <inheritdoc/>
        public TKey GetKeyFromFileName(string keyFilePath)
        {
            return m_KeyFileNameConverter.FromFileNameWithoutExtension(Path.GetFileNameWithoutExtension(keyFilePath));
        }
    }
}

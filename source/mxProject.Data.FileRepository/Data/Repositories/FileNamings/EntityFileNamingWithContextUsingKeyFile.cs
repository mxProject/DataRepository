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
    /// <typeparam name="TContext">The context type.</typeparam>
    public class EntityFileNamingWithContextUsingKeyFile<TEntity, TKey, TContext> : IEntityFileNamingWithContextUsingKeyFile<TEntity, TKey, TContext>
        where TContext : IDataRepositoryContext
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="keyGetter">The function to get the key from the entity.</param>
        /// <param name="keyFileNameConverter">The key file name converter.</param>
        /// <param name="entityFileNameConverter">The entity file name converter.</param>
        public EntityFileNamingWithContextUsingKeyFile(Func<TEntity, TContext, TKey> keyGetter, IKeyFileNameConverterWithContext<TKey, TContext> keyFileNameConverter, IEntityFileNameConverterWithContext<TEntity, TContext> entityFileNameConverter)
        {
            m_KeyGetter = keyGetter;
            m_KeyFileNameConverter = keyFileNameConverter;
            m_EntityFileNameConverter = entityFileNameConverter;
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="keyGetter">The function to get the key from the entity.</param>
        /// <param name="keyFileNameConverter">The key file name converter.</param>
        /// <param name="entityFileNameConverter">The entity file name converter.</param>
        public EntityFileNamingWithContextUsingKeyFile(Func<TEntity, TContext, TKey> keyGetter, IKeyFileNameConverter<TKey> keyFileNameConverter, IEntityFileNameConverter<TEntity> entityFileNameConverter)
        {
            m_KeyGetter = keyGetter;
            m_KeyFileNameConverter = keyFileNameConverter.WithContext<TKey, TContext>();
            m_EntityFileNameConverter = entityFileNameConverter.WithContext<TEntity, TContext>();
        }

        private readonly Func<TEntity, TContext, TKey> m_KeyGetter;
        private readonly IKeyFileNameConverterWithContext<TKey, TContext> m_KeyFileNameConverter;
        private readonly IEntityFileNameConverterWithContext<TEntity, TContext> m_EntityFileNameConverter;

        /// <inheritdoc/>
        public bool IsEntityFile(string filePath, TContext context)
        {
            if (!File.Exists(filePath)) { return false; }
            return string.Compare(Path.GetExtension(filePath), PathEx.NormalizeExtension(m_EntityFileNameConverter.FileExtension), true) == 0;
        }

        /// <inheritdoc/>
        public bool IsKeyFile(string filePath, TContext context)
        {
            if (!File.Exists(filePath)) { return false; }
            return string.Compare(Path.GetExtension(filePath), PathEx.NormalizeExtension(m_KeyFileNameConverter.FileExtension), true) == 0;
        }

        /// <inheritdoc/>
        public TKey GetKey(TEntity entity, TContext context)
        {
            return m_KeyGetter(entity, context);
        }

        /// <inheritdoc/>
        public string GetKeyFileName(TKey key, TContext context)
        {
            return @$"keys\{m_KeyFileNameConverter.ToFileNameWithoutExtension(key, context)}{PathEx.NormalizeExtension(m_KeyFileNameConverter.FileExtension)}";
        }

        /// <inheritdoc/>
        public string GetEntitySaveFileName(TEntity entity, TContext context)
        {
            return @$"entities\{m_EntityFileNameConverter.ToFileNameWithoutExtension(entity, context)}{PathEx.NormalizeExtension(m_EntityFileNameConverter.FileExtension)}";
        }

        /// <inheritdoc/>
        public TKey GetKeyFromFileName(string keyFilePath, TContext context)
        {
            return m_KeyFileNameConverter.FromFileNameWithoutExtension(Path.GetFileNameWithoutExtension(keyFilePath), context);
        }
    }
}

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
    /// <typeparam name="TContext">The context type.</typeparam>
    public class EntityFileNamingWithContextUsingKeyAsFileName<TEntity, TKey, TContext> : IEntityFileNamingWithContextUsingKeyAsFileName<TEntity, TKey, TContext>
        where TContext : IDataRepositoryContext
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="keyGetter">The function to get the key from the entity.</param>
        /// <param name="fileNameConverter">The converter to convert the key to a file name.</param>
        public EntityFileNamingWithContextUsingKeyAsFileName(Func<TEntity, TContext, TKey> keyGetter, IKeyFileNameConverterWithContext<TKey, TContext> fileNameConverter)
        {
            m_KeyGetter = keyGetter;
            m_FileNameConverter = fileNameConverter;
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="keyGetter">The function to get the key from the entity.</param>
        /// <param name="fileNameConverter">The converter to convert the key to a file name.</param>
        public EntityFileNamingWithContextUsingKeyAsFileName(Func<TEntity, TContext, TKey> keyGetter, IKeyFileNameConverter<TKey> fileNameConverter)
        {
            m_KeyGetter = keyGetter;
            m_FileNameConverter = fileNameConverter.WithContext<TKey, TContext>();
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="keyGetter">The function to get the key from the entity.</param>
        /// <param name="fileNameConverter">The converter to convert the key to a file name.</param>
        public EntityFileNamingWithContextUsingKeyAsFileName(Func<TEntity, TKey> keyGetter, IKeyFileNameConverter<TKey> fileNameConverter)
        {
            m_KeyGetter = (entity, context) => keyGetter(entity);
            m_FileNameConverter = fileNameConverter.WithContext<TKey, TContext>();
        }

        private readonly Func<TEntity, TContext, TKey> m_KeyGetter;
        private readonly IKeyFileNameConverterWithContext<TKey, TContext> m_FileNameConverter;

        /// <inheritdoc/>
        public string GetEntityFileName(TKey key, TContext context)
        {
            return $"{m_FileNameConverter.ToFileNameWithoutExtension(key, context)}{PathEx.NormalizeExtension(m_FileNameConverter.FileExtension)}";
        }

        /// <inheritdoc/>
        public TKey GetKey(TEntity entity, TContext context)
        {
            return m_KeyGetter(entity, context);
        }

        /// <inheritdoc/>
        public TKey? GetKeyFromFileName(string entityFilePath, TContext context)
        {
            return m_FileNameConverter.FromFileNameWithoutExtension(Path.GetFileNameWithoutExtension(entityFilePath), context);
        }
    }
}

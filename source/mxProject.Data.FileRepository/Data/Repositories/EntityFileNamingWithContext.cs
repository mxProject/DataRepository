using System;
using System.Collections.Generic;
using System.Text;
using mxProject.Data.Repositories.FileNamings;

namespace mxProject.Data.Repositories
{
    /// <summary>
    /// Provides methods to create file naming converters and entity file naming using key files.
    /// </summary>
    public static class EntityFileNamingWithContext
    {
        #region IEntityFileNamingUsingKeyFile

        /// <summary>
        /// Creates an instance of <see cref="IEntityFileNamingUsingKeyFile{TEntity, TKey}"/>.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TKey">The key type.</typeparam>
        /// <typeparam name="TContext">The context type.</typeparam>
        /// <param name="getKey">The function to get the key from the entity.</param>
        /// <param name="keyFileNameConverter">The key file name converter.</param>
        /// <param name="entityFileNameConverter">The entity file name converter.</param>
        /// <returns>An instance of <see cref="IEntityFileNamingUsingKeyFile{TEntity, TKey}"/>.</returns>
        public static IEntityFileNamingWithContextUsingKeyFile<TEntity, TKey, TContext> CreateUsingKeyFile<TEntity, TKey, TContext>(Func<TEntity, TContext, TKey> getKey, IKeyFileNameConverterWithContext<TKey, TContext> keyFileNameConverter, IEntityFileNameConverterWithContext<TEntity, TContext> entityFileNameConverter)
            where TContext : IDataRepositoryContext
        {
            return new EntityFileNamingWithContextUsingKeyFile<TEntity, TKey, TContext>(getKey, keyFileNameConverter, entityFileNameConverter);
        }

        /// <summary>
        /// Creates an instance of <see cref="IEntityFileNamingUsingKeyFile{TEntity, TKey}"/>.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TKey">The key type.</typeparam>
        /// <typeparam name="TContext">The context type.</typeparam>
        /// <param name="keyFileNameConverter">The key file name converter.</param>
        /// <param name="entityFileNameConverter">The entity file name converter.</param>
        /// <returns>An instance of <see cref="IEntityFileNamingUsingKeyFile{TEntity, TKey}"/>.</returns>
        public static IEntityFileNamingWithContextUsingKeyFile<TEntity, TKey, TContext> CreateUsingKeyFile<TEntity, TKey, TContext>(IKeyFileNameConverterWithContext<TKey, TContext> keyFileNameConverter, IEntityFileNameConverterWithContext<TEntity, TContext> entityFileNameConverter)
            where TEntity : IHasKey<TKey>
            where TContext : IDataRepositoryContext
        {
            return new EntityFileNamingWithContextUsingKeyFile<TEntity, TKey, TContext>((x, c) => x.GetKey(), keyFileNameConverter, entityFileNameConverter);
        }

        #endregion

        #region IEntityFileNamingUsingKeyAsFileName

        /// <summary>
        /// Creates an instance of <see cref="IEntityFileNamingUsingKeyAsFileName{TEntity, TKey}"/>.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TKey">The key type.</typeparam>
        /// <typeparam name="TContext">The context type.</typeparam>
        /// <param name="getKey">The function to get the key from the entity.</param>
        /// <param name="keyFileNameConverter">The key file name converter.</param>
        /// <returns>An instance of <see cref="IEntityFileNamingUsingKeyAsFileName{TEntity, TKey}"/>.</returns>
        public static IEntityFileNamingWithContextUsingKeyAsFileName<TEntity, TKey, TContext> CreateUsingKeyAsFileName<TEntity, TKey, TContext>(Func<TEntity, TContext, TKey> getKey, IKeyFileNameConverterWithContext<TKey, TContext> keyFileNameConverter)
            where TContext : IDataRepositoryContext
        {
            return new EntityFileNamingWithContextUsingKeyAsFileName<TEntity, TKey, TContext>(getKey, keyFileNameConverter);
        }

        /// <summary>
        /// Creates an instance of <see cref="IEntityFileNamingUsingKeyAsFileName{TEntity, TKey}"/>.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TKey">The key type.</typeparam>
        /// <typeparam name="TContext">The context type.</typeparam>
        /// <param name="keyFileNameConverter">The key file name converter.</param>
        /// <returns>An instance of <see cref="IEntityFileNamingUsingKeyAsFileName{TEntity, TKey}"/>.</returns>
        public static IEntityFileNamingWithContextUsingKeyAsFileName<TEntity, TKey, TContext> CreateUsingKeyAsFileName<TEntity, TKey, TContext>(IKeyFileNameConverterWithContext<TKey, TContext> keyFileNameConverter)
            where TEntity : IHasKey<TKey>
            where TContext : IDataRepositoryContext
        {
            return new EntityFileNamingWithContextUsingKeyAsFileName<TEntity, TKey, TContext>((x, c) => x.GetKey(), keyFileNameConverter);
        }

        #endregion

        #region IKeyFileNameConverter

        /// <summary>
        /// Creates an instance of <see cref="IKeyFileNameConverter{TKey}"/>.
        /// </summary>
        /// <typeparam name="TKey">The key type.</typeparam>
        /// <typeparam name="TContext">The context type.</typeparam>
        /// <param name="fileExtension">The file extension.</param>
        /// <param name="toFileNameWithoutExtension">The function to convert the key to a file name without extension.</param>
        /// <param name="fromFileNameWithoutExtension">The function to convert a file name without extension to a key.</param>
        /// <returns>An instance of <see cref="IKeyFileNameConverter{TKey}"/>.</returns>
        public static IKeyFileNameConverterWithContext<TKey, TContext> CreateKeyFileNameConverter<TKey, TContext>(string fileExtension, Func<TKey, TContext, string> toFileNameWithoutExtension, Func<string, TContext, TKey> fromFileNameWithoutExtension)
            where TContext : IDataRepositoryContext
        {
            return new KeyFileNameConverterWithContext<TKey, TContext>(fileExtension, toFileNameWithoutExtension, fromFileNameWithoutExtension);
        }

        #endregion

        #region IEntityFileNameConverter

        /// <summary>
        /// Creates an instance of <see cref="IEntityFileNameConverter{TEntity}"/>.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TContext">The context type.</typeparam>
        /// <param name="fileExtension">The file extension.</param>
        /// <param name="toFileNameWithoutExtension">The function to convert the entity to a file name without extension.</param>
        /// <returns>An instance of <see cref="IEntityFileNameConverter{TEntity}"/>.</returns>
        public static IEntityFileNameConverterWithContext<TEntity, TContext> CreateEntityFileNameConverter<TEntity, TContext>(string fileExtension, Func<TEntity, TContext, string> toFileNameWithoutExtension)
            where TContext : IDataRepositoryContext
        {
            return new EntityFileNameConverterWithContext<TEntity, TContext>(fileExtension, toFileNameWithoutExtension);
        }

        #endregion
    }
}

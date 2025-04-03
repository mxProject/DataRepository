using System;
using System.Collections.Generic;
using System.Text;
using mxProject.Data.Repositories.FileNamings;

namespace mxProject.Data.Repositories
{
    /// <summary>
    /// Provides methods to create file naming converters and entity file naming using key files.
    /// </summary>
    public static class EntityFileNaming
    {
        #region IEntityFileNamingUsingKeyFile

        /// <summary>
        /// Creates an instance of <see cref="IEntityFileNamingUsingKeyFile{TEntity, TKey}"/>.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TKey">The key type.</typeparam>
        /// <param name="getKey">The function to get the key from the entity.</param>
        /// <param name="keyFileNameConverter">The key file name converter.</param>
        /// <param name="entityFileNameConverter">The entity file name converter.</param>
        /// <returns>An instance of <see cref="IEntityFileNamingUsingKeyFile{TEntity, TKey}"/>.</returns>
        public static IEntityFileNamingUsingKeyFile<TEntity, TKey> CreateUsingKeyFile<TEntity, TKey>(Func<TEntity, TKey> getKey, IKeyFileNameConverter<TKey> keyFileNameConverter, IEntityFileNameConverter<TEntity> entityFileNameConverter)
        {
            return new EntityFileNamingUsingKeyFile<TEntity, TKey>(getKey, keyFileNameConverter, entityFileNameConverter);
        }

        /// <summary>
        /// Creates an instance of <see cref="IEntityFileNamingUsingKeyFile{TEntity, TKey}"/>.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TKey">The key type.</typeparam>
        /// <param name="keyFileNameConverter">The key file name converter.</param>
        /// <param name="entityFileNameConverter">The entity file name converter.</param>
        /// <returns>An instance of <see cref="IEntityFileNamingUsingKeyFile{TEntity, TKey}"/>.</returns>
        public static IEntityFileNamingUsingKeyFile<TEntity, TKey> CreateUsingKeyFile<TEntity, TKey>(IKeyFileNameConverter<TKey> keyFileNameConverter, IEntityFileNameConverter<TEntity> entityFileNameConverter)
            where TEntity : IHasKey<TKey>
        {
            return new EntityFileNamingUsingKeyFile<TEntity, TKey>(x => x.GetKey(), keyFileNameConverter, entityFileNameConverter);
        }

        #endregion

        #region IEntityFileNamingUsingKeyAsFileName

        /// <summary>
        /// Creates an instance of <see cref="IEntityFileNamingUsingKeyAsFileName{TEntity, TKey}"/>.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TKey">The key type.</typeparam>
        /// <param name="getKey">The function to get the key from the entity.</param>
        /// <param name="keyFileNameConverter">The key file name converter.</param>
        /// <returns>An instance of <see cref="IEntityFileNamingUsingKeyAsFileName{TEntity, TKey}"/>.</returns>
        public static IEntityFileNamingUsingKeyAsFileName<TEntity, TKey> CreateUsingKeyAsFileName<TEntity, TKey>(Func<TEntity, TKey> getKey, IKeyFileNameConverter<TKey> keyFileNameConverter)
        {
            return new EntityFileNamingUsingKeyAsFileName<TEntity, TKey>(getKey, keyFileNameConverter);
        }

        /// <summary>
        /// Creates an instance of <see cref="IEntityFileNamingUsingKeyAsFileName{TEntity, TKey}"/>.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TKey">The key type.</typeparam>
        /// <param name="keyFileNameConverter">The key file name converter.</param>
        /// <returns>An instance of <see cref="IEntityFileNamingUsingKeyAsFileName{TEntity, TKey}"/>.</returns>
        public static IEntityFileNamingUsingKeyAsFileName<TEntity, TKey> CreateUsingKeyAsFileName<TEntity, TKey>(IKeyFileNameConverter<TKey> keyFileNameConverter)
            where TEntity : IHasKey<TKey>
        {
            return new EntityFileNamingUsingKeyAsFileName<TEntity, TKey>(x => x.GetKey(), keyFileNameConverter);
        }

        #endregion

        #region IKeyFileNameConverter

        /// <summary>
        /// Creates an instance of <see cref="IKeyFileNameConverter{TKey}"/>.
        /// </summary>
        /// <typeparam name="TKey">The key type.</typeparam>
        /// <param name="fileExtension">The file extension.</param>
        /// <param name="toFileNameWithoutExtension">The function to convert the key to a file name without extension.</param>
        /// <param name="fromFileNameWithoutExtension">The function to convert a file name without extension to a key.</param>
        /// <returns>An instance of <see cref="IKeyFileNameConverter{TKey}"/>.</returns>
        public static IKeyFileNameConverter<TKey> CreateKeyFileNameConverter<TKey>(string fileExtension, Func<TKey, string> toFileNameWithoutExtension, Func<string, TKey> fromFileNameWithoutExtension)
        {
            return new KeyFileNameConverter<TKey>(fileExtension, toFileNameWithoutExtension, fromFileNameWithoutExtension);
        }

        #endregion

        #region IEntityFileNameConverter

        /// <summary>
        /// Creates an instance of <see cref="IEntityFileNameConverter{TEntity}"/>.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="fileExtension">The file extension.</param>
        /// <param name="toFileNameWithoutExtension">The function to convert the entity to a file name without extension.</param>
        /// <returns>An instance of <see cref="IEntityFileNameConverter{TEntity}"/>.</returns>
        public static IEntityFileNameConverter<TEntity> CreateEntityFileNameConverter<TEntity>(string fileExtension, Func<TEntity, string> toFileNameWithoutExtension)
        {
            return new EntityFileNameConverter<TEntity>(fileExtension, toFileNameWithoutExtension);
        }

        #endregion
    }
}

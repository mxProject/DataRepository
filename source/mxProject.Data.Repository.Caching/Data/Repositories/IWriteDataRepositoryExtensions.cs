using System;
using System.Collections.Generic;
using System.Text;

namespace mxProject.Data.Repositories
{
    /// <summary>
    /// Extension methods for IWriteDataRepository and IWriteDataRepositoryWithContext.
    /// </summary>
    public static class IWriteDataRepositoryExtensions
    {
        #region WithCache

        /// <summary>
        /// Adds the ability to cache entities.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TKey">The key type.</typeparam>
        /// <param name="this"></param>
        /// <returns></returns>
        public static IWriteDataRepository<TEntity> WithCache<TEntity, TKey>(
            this IWriteDataRepository<TEntity> @this
            )
            where TEntity : IHasKey<TKey>
        {
            return new Caching.CacheWriteDataRepository<TEntity, TKey>(
                @this,
                entity => entity.GetKey()
                );
        }

        /// <summary>
        /// Adds the ability to cache entities.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TKey">The key type.</typeparam>
        /// <param name="this"></param>
        /// <param name="keyGetter">The method to get the key from a spacified entiry.</param>
        /// <returns></returns>
        public static IWriteDataRepository<TEntity> WithCache<TEntity, TKey>(
            this IWriteDataRepository<TEntity> @this,
            Func<TEntity, TKey> keyGetter
            )
        {
            return new Caching.CacheWriteDataRepository<TEntity, TKey>(
                @this,
                keyGetter
                );
        }

        #endregion

        #region WithCache (WithContext)

        /// <summary>
        /// Adds the ability to cache entities.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TKey">The key type.</typeparam>
        /// <typeparam name="TContext">The context type.</typeparam>
        /// <param name="this"></param>
        /// <returns></returns>
        public static IWriteDataRepositoryWithContext<TEntity, TContext> WithCache<TEntity, TKey, TContext>(
            this IWriteDataRepositoryWithContext<TEntity, TContext> @this
            )
            where TEntity : IHasKey<TKey>
            where TContext : IDataRepositoryContext
        {
            return new Caching.CacheWriteDataRepositoryWithContext<TEntity, TKey, TContext>(
                @this,
                entity => entity.GetKey()
                );
        }

        /// <summary>
        /// Adds the ability to cache entities.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TKey">The key type.</typeparam>
        /// <typeparam name="TContext">The context type.</typeparam>
        /// <param name="this"></param>
        /// <param name="keyGetter">The method to get the key from a spacified entiry.</param>
        /// <returns></returns>
        public static IWriteDataRepositoryWithContext<TEntity, TContext> WithCache<TEntity, TKey, TContext>(
            this IWriteDataRepositoryWithContext<TEntity, TContext> @this,
            Func<TEntity, TKey> keyGetter
            )
            where TContext : IDataRepositoryContext
        {
            return new Caching.CacheWriteDataRepositoryWithContext<TEntity, TKey, TContext>(
                @this,
                keyGetter
                );
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace mxProject.Data.Repositories
{
    /// <summary>
    /// Extension methods for IAsyncReadDataRepository and IAsyncReadDataRepositoryWithContext.
    /// </summary>
    public static class IAsyncReadDataRepositoryExtensions
    {
        #region AsPrimaryKeyRepository

        /// <summary>
        /// Converts to a repository for getting entities by primary key.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TPrimaryKey">The primary key type.</typeparam>
        /// <typeparam name="TUniqueKey">The unique key type.</typeparam>
        /// <param name="this"></param>
        /// <returns></returns>
        public static IAsyncReadDataRepository<TEntity, TPrimaryKey> AsPrimaryKeyRepository<TEntity, TPrimaryKey, TUniqueKey>(this IAsyncReadDataRepositoryWithUniqueKey<TEntity, TPrimaryKey, TUniqueKey> @this)
        {
            return new Wrappers.AsyncReadDataRepository<TEntity, TPrimaryKey>(
                () => @this.UseTransactionScope,
                () => @this.Dispose(),
                (key) => @this.GetByPrimaryKeyAsync(key),
                (keys, cancellationToken) => @this.GetRangeByPrimaryKeyAsync(keys, cancellationToken),
                (keys, cancellationToken) => @this.GetRangeByPrimaryKeyAsync(keys, cancellationToken),
                (cancellationToken) => @this.GetAllAsync(cancellationToken),
                (cancellationToken) => @this.GetAllPrimaryKeysAsync(cancellationToken)
                );
        }

        /// <summary>
        /// Converts to a repository for getting entities by primary key.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TPrimaryKey">The primary key type.</typeparam>
        /// <typeparam name="TUniqueKey">The unique key type.</typeparam>
        /// <typeparam name="TContext">The context type.</typeparam>
        /// <param name="this"></param>
        /// <returns></returns>
        public static IAsyncReadDataRepositoryWithContext<TEntity, TPrimaryKey, TContext> AsPrimaryKeyRepository<TEntity, TPrimaryKey, TUniqueKey, TContext>(this IAsyncReadDataRepositoryWithUniqueKeyWithContext<TEntity, TPrimaryKey, TUniqueKey, TContext> @this)
            where TContext : IDataRepositoryContext
        {
            return new Wrappers.AsyncReadDataRepositoryWithContext<TEntity, TPrimaryKey, TContext>(
                () => @this.UseTransactionScope,
                () => @this.Dispose(),
                (key, context) => @this.GetByPrimaryKeyAsync(key, context),
                (keys, context, cancellationToken) => @this.GetRangeByPrimaryKeyAsync(keys, context, cancellationToken),
                (keys, context, cancellationToken) => @this.GetRangeByPrimaryKeyAsync(keys, context, cancellationToken),
                (context, cancellationToken) => @this.GetAllAsync(context, cancellationToken),
                (context, cancellationToken) => @this.GetAllPrimaryKeysAsync(context, cancellationToken)
                );
        }

        #endregion

        #region AsUniqueKeyRepository

        /// <summary>
        /// Converts to a repository for getting entities by unique key.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TPrimaryKey">The primary key type.</typeparam>
        /// <typeparam name="TUniqueKey">The unique key type.</typeparam>
        /// <param name="this"></param>
        /// <returns></returns>
        public static IAsyncReadDataRepository<TEntity, TUniqueKey> AsUniqueKeyRepository<TEntity, TPrimaryKey, TUniqueKey>(this IAsyncReadDataRepositoryWithUniqueKey<TEntity, TPrimaryKey, TUniqueKey> @this)
        {
            return new Wrappers.AsyncReadDataRepository<TEntity, TUniqueKey>(
                () => @this.UseTransactionScope,
                () => @this.Dispose(),
                (key) => @this.GetByUniqueKeyAsync(key),
                (keys, cancellationToken) => @this.GetRangeByUniqueKeyAsync(keys, cancellationToken),
                (keys, cancellationToken) => @this.GetRangeByUniqueKeyAsync(keys, cancellationToken),
                (cancellationToken) => @this.GetAllAsync(cancellationToken),
                (cancellationToken) => @this.GetAllUniqueKeysAsync(cancellationToken)
                );
        }

        /// <summary>
        /// Converts to a repository for getting entities by unique key.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TPrimaryKey">The primary key type.</typeparam>
        /// <typeparam name="TUniqueKey">The unique key type.</typeparam>
        /// <typeparam name="TContext">The context type.</typeparam>
        /// <param name="this"></param>
        /// <returns></returns>
        public static IAsyncReadDataRepositoryWithContext<TEntity, TUniqueKey, TContext> AsUniqueKeyRepository<TEntity, TPrimaryKey, TUniqueKey, TContext>(this IAsyncReadDataRepositoryWithUniqueKeyWithContext<TEntity, TPrimaryKey, TUniqueKey, TContext> @this)
            where TContext : IDataRepositoryContext
        {
            return new Wrappers.AsyncReadDataRepositoryWithContext<TEntity, TUniqueKey, TContext>(
                () => @this.UseTransactionScope,
                () => @this.Dispose(),
                (key, context) => @this.GetByUniqueKeyAsync(key, context),
                (keys, context, cancellationToken) => @this.GetRangeByUniqueKeyAsync(keys, context, cancellationToken),
                (keys, context, cancellationToken) => @this.GetRangeByUniqueKeyAsync(keys, context, cancellationToken),
                (context, cancellationToken) => @this.GetAllAsync(context, cancellationToken),
                (context, cancellationToken) => @this.GetAllUniqueKeysAsync(context, cancellationToken)
                );
        }

        #endregion
    }
}

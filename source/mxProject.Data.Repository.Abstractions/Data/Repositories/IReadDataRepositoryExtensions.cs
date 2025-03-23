using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;

namespace mxProject.Data.Repositories
{
    /// <summary>
    /// Extension methods for IReadDataRepository and IReadDataRepositoryWithContext.
    /// </summary>
    public static class IReadDataRepositoryExtensions
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
        public static IReadDataRepository<TEntity, TPrimaryKey> AsPrimaryKeyRepository<TEntity, TPrimaryKey, TUniqueKey>(this IReadDataRepositoryWithUniqueKey<TEntity, TPrimaryKey, TUniqueKey> @this)
        {
            return new Wrappers.ReadDataRepository<TEntity, TPrimaryKey>(
                () => @this.UseTransactionScope,
                () => @this.Dispose(),
                (key) => @this.GetByPrimaryKey(key),
                (keys) => @this.GetRangeByPrimaryKey(keys),
                () => @this.GetAll(),
                () => @this.GetAllPrimaryKeys()
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
        public static IReadDataRepositoryWithContext<TEntity, TPrimaryKey, TContext> AsPrimaryKeyRepository<TEntity, TPrimaryKey, TUniqueKey, TContext>(this IReadDataRepositoryWithUniqueKeyWithContext<TEntity, TPrimaryKey, TUniqueKey, TContext> @this)
            where TContext : IDataRepositoryContext
        {
            return new Wrappers.ReadDataRepositoryWithContext<TEntity, TPrimaryKey, TContext>(
                () => @this.UseTransactionScope,
                () => @this.Dispose(),
                (key, context) => @this.GetByPrimaryKey(key, context),
                (keys, context) => @this.GetRangeByPrimaryKey(keys, context),
                (context) => @this.GetAll(context),
                (context) => @this.GetAllPrimaryKeys(context)
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
        public static IReadDataRepository<TEntity, TUniqueKey> AsUniqueKeyRepository<TEntity, TPrimaryKey, TUniqueKey>(this IReadDataRepositoryWithUniqueKey<TEntity, TPrimaryKey, TUniqueKey> @this)
        {
            return new Wrappers.ReadDataRepository<TEntity, TUniqueKey>(
                () => @this.UseTransactionScope,
                () => @this.Dispose(),
                (key) => @this.GetByUniqueKey(key),
                (keys) => @this.GetRangeByUniqueKey(keys),
                () => @this.GetAll(),
                () => @this.GetAllUniqueKeys()
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
        public static IReadDataRepositoryWithContext<TEntity, TUniqueKey, TContext> AsUniqueKeyRepository<TEntity, TPrimaryKey, TUniqueKey, TContext>(this IReadDataRepositoryWithUniqueKeyWithContext<TEntity, TPrimaryKey, TUniqueKey, TContext> @this)
            where TContext : IDataRepositoryContext
        {
            return new Wrappers.ReadDataRepositoryWithContext<TEntity, TUniqueKey, TContext>(
                () => @this.UseTransactionScope,
                () => @this.Dispose(),
                (key, context) => @this.GetByUniqueKey(key, context),
                (keys, context) => @this.GetRangeByUniqueKey(keys, context),
                (context) => @this.GetAll(context),
                (context) => @this.GetAllUniqueKeys(context)
                );
        }

        #endregion
    }


}

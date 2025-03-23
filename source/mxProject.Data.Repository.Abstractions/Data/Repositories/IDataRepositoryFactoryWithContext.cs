using System;
using System.Collections.Generic;
using System.Text;

namespace mxProject.Data.Repositories
{
    /// <summary>
    /// It provides the functionality required for a factory that gets a data repository.
    /// </summary>
    /// <typeparam name="TContext">The context type.</typeparam>
    public interface IDataRepositoryFactoryWithContext<TContext> where TContext : IDataRepositoryContext
    {
        /// <summary>
        /// Gets a data repository for reading entities of the specified type.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TKey">The key type.</typeparam>
        /// <returns>The data repository.</returns>
        IReadDataRepositoryWithContext<TEntity, TKey, TContext> GetReadDataRepository<TEntity, TKey>();

        /// <summary>
        /// Gets a data repository for reading entities of the specified type.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TPrimaryKey">The primary key type.</typeparam>
        /// <typeparam name="TUniqueKey">The unique key type.</typeparam>
        /// <returns>The data repository.</returns>
        IReadDataRepositoryWithUniqueKeyWithContext<TEntity, TPrimaryKey, TUniqueKey, TContext> GetReadDataRepository<TEntity, TPrimaryKey, TUniqueKey>();

        /// <summary>
        /// Gets a data repository for writing entities of the specified type.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <returns>The data repository.</returns>
        IWriteDataRepositoryWithContext<TEntity, TContext> GetWriteDataRepository<TEntity>();

        /// <summary>
        /// Gets a data repository for querying entities of the specified type.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TCondition">The condition type.</typeparam>
        /// <returns>The data repository.</returns>
        IDataQueryWithContext<TEntity, TCondition, TContext> GetDataQuery<TEntity, TCondition>();


        /// <summary>
        /// Gets a data repository for asynchronously reading entities of the specified type.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TKey">The key type.</typeparam>
        /// <returns>The data repository.</returns>
        IAsyncReadDataRepositoryWithContext<TEntity, TKey, TContext> GetAsyncReadDataRepository<TEntity, TKey>();

        /// <summary>
        /// Gets a data repository for asynchronously reading entities of the specified type.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TPrimaryKey">The primary key type.</typeparam>
        /// <typeparam name="TUniqueKey">The unique key type.</typeparam>
        /// <returns>The data repository.</returns>
        IAsyncReadDataRepositoryWithUniqueKeyWithContext<TEntity, TPrimaryKey, TUniqueKey, TContext> GetAsyncReadDataRepository<TEntity, TPrimaryKey, TUniqueKey>();

        /// <summary>
        /// Gets a data repository for asynchronously writing entities of the specified type.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <returns>The data repository.</returns>
        IAsyncWriteDataRepositoryWithContext<TEntity, TContext> GetAsyncWriteDataRepository<TEntity>();

        /// <summary>
        /// Gets a data repository for asynchronously querying entities of the specified type.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TCondition">The condition type.</typeparam>
        /// <returns>The data repository.</returns>
        IAsyncDataQueryWithContext<TEntity, TCondition, TContext> GetAsyncDataQuery<TEntity, TCondition>();
    }
}

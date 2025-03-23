using System;
using System.Collections.Generic;
using System.Text;

namespace mxProject.Data.Repositories
{
    /// <summary>
    /// It provides the functionality required for a factory that gets a data repository.
    /// </summary>
    public interface IDataRepositoryFactory
    {
        /// <summary>
        /// Gets a data repository for reading entities of the specified type.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TKey">The key type.</typeparam>
        /// <returns>The data repository.</returns>
        IReadDataRepository<TEntity, TKey> GetReadDataRepository<TEntity, TKey>();

        /// <summary>
        /// Gets a data repository for reading entities of the specified type.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TPrimaryKey">The primary key type.</typeparam>
        /// <typeparam name="TUniqueKey">The unique key type.</typeparam>
        /// <returns>The data repository.</returns>
        IReadDataRepositoryWithUniqueKey<TEntity, TPrimaryKey, TUniqueKey> GetReadDataRepository<TEntity, TPrimaryKey, TUniqueKey>();

        /// <summary>
        /// Gets a data repository for writing entities of the specified type.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <returns>The data repository.</returns>
        IWriteDataRepository<TEntity> GetWriteDataRepository<TEntity>();

        /// <summary>
        /// Gets a data repository for querying entities of the specified type.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TCondition">The condition type.</typeparam>
        /// <returns>The data repository.</returns>
        IDataQuery<TEntity, TCondition> GetDataQuery<TEntity, TCondition>();


        /// <summary>
        /// Gets a data repository for asynchronously reading entities of the specified type.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TKey">The key type.</typeparam>
        /// <returns>The data repository.</returns>
        IAsyncReadDataRepository<TEntity, TKey> GetAsyncReadDataRepository<TEntity, TKey>();

        /// <summary>
        /// Gets a data repository for asynchronously reading entities of the specified type.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TPrimaryKey">The primary key type.</typeparam>
        /// <typeparam name="TUniqueKey">The unique key type.</typeparam>
        /// <returns>The data repository.</returns>
        IAsyncReadDataRepositoryWithUniqueKey<TEntity, TPrimaryKey, TUniqueKey> GetAsyncReadDataRepository<TEntity, TPrimaryKey, TUniqueKey>();

        /// <summary>
        /// Gets a data repository for asynchronously writing entities of the specified type.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <returns>The data repository.</returns>
        IAsyncWriteDataRepository<TEntity> GetAsyncWriteDataRepository<TEntity>();

        /// <summary>
        /// Gets a data repository for asynchronously querying entities of the specified type.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TCondition">The condition type.</typeparam>
        /// <returns>The data repository.</returns>
        IAsyncDataQuery<TEntity, TCondition> GetAsyncDataQuery<TEntity, TCondition>();
    }
}

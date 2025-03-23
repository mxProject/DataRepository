using System;
using System.Collections.Generic;
using System.Text;

namespace mxProject.Data.Repositories
{
    /// <summary>
    /// It provides the functionality required for a data repository to read entities.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TPrimaryKey">The primary key type.</typeparam>
    /// <typeparam name="TUniqueKey">The unique ket type.</typeparam>
    public interface IReadDataRepositoryWithUniqueKey<TEntity, TPrimaryKey, TUniqueKey> : IDataRepository<TEntity>
    {
        /// <summary>
        /// Gets the entity corresponding to the specified primary key.
        /// </summary>
        /// <param name="primaryKey">The primary key.</param>
        /// <returns>The entity.</returns>
        TEntity? GetByPrimaryKey(TPrimaryKey primaryKey);

        /// <summary>
        /// Gets the entities corresponding to the specified primary keys.
        /// </summary>
        /// <param name="primaryKeys">The primary keys.</param>
        /// <returns>The entities.</returns>
        IEnumerable<TEntity> GetRangeByPrimaryKey(IEnumerable<TPrimaryKey> primaryKeys);

        /// <summary>
        /// Gets the entity corresponding to the specified unique key.
        /// </summary>
        /// <param name="uniqueKey">The unique key.</param>
        /// <returns>The entity.</returns>
        TEntity? GetByUniqueKey(TUniqueKey uniqueKey);

        /// <summary>
        /// Gets the entities corresponding to the specified unique keys.
        /// </summary>
        /// <param name="uniqueKeys">The unique keys.</param>
        /// <returns>The entities.</returns>
        IEnumerable<TEntity> GetRangeByUniqueKey(IEnumerable<TUniqueKey> uniqueKeys);

        /// <summary>
        /// Gets all entities.
        /// </summary>
        /// <returns>The entities.</returns>
        IEnumerable<TEntity> GetAll();

        /// <summary>
        /// Gets all primary keys.
        /// </summary>
        /// <returns>The primary keys</returns>
        IEnumerable<TPrimaryKey> GetAllPrimaryKeys();

        /// <summary>
        /// Gets all unique keys.
        /// </summary>
        /// <returns>The unique keys</returns>
        IEnumerable<TUniqueKey> GetAllUniqueKeys();
    }
}

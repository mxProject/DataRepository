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
    /// <typeparam name="TContext">The context type.</typeparam>
    public interface IReadDataRepositoryWithUniqueKeyWithContext<TEntity, TPrimaryKey, TUniqueKey, TContext> : IDataRepository<TEntity>
        where TContext : IDataRepositoryContext
    {
        /// <summary>
        /// Gets the entity corresponding to the specified primary key.
        /// </summary>
        /// <param name="primaryKey">The primary key.</param>
        /// <param name="context">The current context.</param>
        /// <returns>The entity.</returns>
        TEntity? GetByPrimaryKey(TPrimaryKey primaryKey, TContext context);

        /// <summary>
        /// Gets the entities corresponding to the specified primary keys.
        /// </summary>
        /// <param name="primaryKeys">The primary keys.</param>
        /// <param name="context">The current context.</param>
        /// <returns>The entities.</returns>
        IEnumerable<TEntity> GetRangeByPrimaryKey(IEnumerable<TPrimaryKey> primaryKeys, TContext context);

        /// <summary>
        /// Gets the entity corresponding to the specified unique key.
        /// </summary>
        /// <param name="uniqueKey">The unique key.</param>
        /// <param name="context">The current context.</param>
        /// <returns>The entity.</returns>
        TEntity? GetByUniqueKey(TUniqueKey uniqueKey, TContext context);

        /// <summary>
        /// Gets the entities corresponding to the specified unique keys.
        /// </summary>
        /// <param name="uniqueKeys">The unique keys.</param>
        /// <param name="context">The current context.</param>
        /// <returns>The entities.</returns>
        IEnumerable<TEntity> GetRangeByUniqueKey(IEnumerable<TUniqueKey> uniqueKeys, TContext context);

        /// <summary>
        /// Gets all entities.
        /// </summary>
        /// <param name="context">The current context.</param>
        /// <returns>The entities.</returns>
        IEnumerable<TEntity> GetAll(TContext context);

        /// <summary>
        /// Gets all primary keys.
        /// </summary>
        /// <param name="context">The current context.</param>
        /// <returns>The primary keys</returns>
        IEnumerable<TPrimaryKey> GetAllPrimaryKeys(TContext context);

        /// <summary>
        /// Gets all unique keys.
        /// </summary>
        /// <param name="context">The current context.</param>
        /// <returns>The unique keys</returns>
        IEnumerable<TUniqueKey> GetAllUniqueKeys(TContext context);
    }
}

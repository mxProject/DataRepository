using System;
using System.Collections.Generic;
using System.Text;

namespace mxProject.Data.Repositories
{
    /// <summary>
    /// It provides the functionality required for a data repository to write entities.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TContext">The context type.</typeparam>
    public interface IWriteDataRepositoryWithContext<TEntity, TContext> : IDataRepository<TEntity>
        where TContext : IDataRepositoryContext
    {
        /// <summary>
        /// Inserts the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="context">The current context.</param>
        /// <returns>Number of affected entities.</returns>
        int Insert(TEntity entity, TContext context);

        /// <summary>
        /// Inserts the specified entitis.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="context">The current context.</param>
        /// <returns>Number of affected entities.</returns>
        int InsertRange(IEnumerable<TEntity> entities, TContext context);

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="context">The current context.</param>
        /// <returns>Number of affected entities.</returns>
        int Update(TEntity entity, TContext context);

        /// <summary>
        /// Updates the specified entitis.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="context">The current context.</param>
        /// <returns>Number of affected entities.</returns>
        int UpdateRange(IEnumerable<TEntity> entities, TContext context);

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="context">The current context.</param>
        /// <returns>Number of affected entities.</returns>
        int Delete(TEntity entity, TContext context);

        /// <summary>
        /// Deletes the specified entitis.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="context">The current context.</param>
        /// <returns>Number of affected entities.</returns>
        int DeleteRange(IEnumerable<TEntity> entities, TContext context);
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace mxProject.Data.Repositories
{
    /// <summary>
    /// It provides the functionality required for a data repository to write entities.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public interface IWriteDataRepository<TEntity> : IDataRepository<TEntity>
    {
        /// <summary>
        /// Inserts the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>Number of affected entities.</returns>
        int Insert(TEntity entity);

        /// <summary>
        /// Inserts the specified entitis.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <returns>Number of affected entities.</returns>
        int InsertRange(IEnumerable<TEntity> entities);

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>Number of affected entities.</returns>
        int Update(TEntity entity);

        /// <summary>
        /// Updates the specified entitis.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <returns>Number of affected entities.</returns>
        int UpdateRange(IEnumerable<TEntity> entities);

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>Number of affected entities.</returns>
        int Delete(TEntity entity);

        /// <summary>
        /// Deletes the specified entitis.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <returns>Number of affected entities.</returns>
        int DeleteRange(IEnumerable<TEntity> entities);
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace mxProject.Data.Repositories
{
    /// <summary>
    /// It provides the functionality required for a query to find entities.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TCondition">The query condition type.</typeparam>
    /// <typeparam name="TContext">The context type.</typeparam>
    public interface IDataQueryWithContext<TEntity, TCondition, TContext> : IDataRepository<TEntity>
        where TContext : IDataRepositoryContext
    {
        /// <summary>
        /// Finds entities that match the specified condition.
        /// </summary>
        /// <param name="condition">The conditions.</param>
        /// <param name="context">The current context.</param>
        /// <param name="skipCount">The number of entities to skip</param>
        /// <param name="maximumCount">The maximum number of entities to retrieve.</param>
        /// <returns>The entities.</returns>
        IEnumerable<TEntity> Query(TCondition condition, TContext context, int skipCount = 0, int? maximumCount = null);

        /// <summary>
        /// Gets the number of entities that match the specified condition.
        /// </summary>
        /// <param name="condition">The conditions.</param>
        /// <param name="context">The current context.</param>
        /// <returns>Number of entities.</returns>
        int GetCount(TCondition condition, TContext context);
    }
}

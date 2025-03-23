using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Transactions;

namespace mxProject.Data.Repositories
{
    /// <summary>
    /// A basic implementation of a query that uses a database.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TCondition">The query condition type.</typeparam>
    public abstract class DbQueryBase<TEntity, TCondition> : DbRepositoryBase<TEntity>, IDataQuery<TEntity, TCondition>
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="connectionActivator">The method to activate a connection.</param>
        /// <param name="useTransactionScope">A value that indicates whether to use ambient transactions using TransactionScope.</param>
        protected DbQueryBase(Func<IDbConnection> connectionActivator, bool useTransactionScope)
            : base(connectionActivator, useTransactionScope)
        {
        }

        #region Query

        /// <inheritdoc/>
        public IEnumerable<TEntity> Query(TCondition condition, int skipCount = 0, int? maximumCount = null)
        {
            return Executor.ExecuteIteratorOnNewConnection(
                (this, condition, skipCount, maximumCount),
                (x, commandActivator) => x.Item1.Query(commandActivator, x.condition, x.skipCount, x.maximumCount)
                );
        }

        /// <summary>
        /// Finds entities that match the specified condition.
        /// </summary>
        /// <param name="connection">The current connection.</param>
        /// <param name="condition">The conditions.</param>
        /// <param name="skipCount">The number of entities to skip</param>
        /// <param name="maximumCount">The maximum number of entities to retrieve.</param>
        /// <returns>The entities.</returns>
        public IEnumerable<TEntity> Query(IDbConnection connection, TCondition condition, int skipCount = 0, int? maximumCount = null)
        {
            return Executor.ExecuteOnConnection(
                (this, condition, skipCount, maximumCount),
                connection,
                (x, commandActivator) => x.Item1.Query(commandActivator, x.condition, x.skipCount, x.maximumCount)
                );
        }

        /// <summary>
        /// Finds entities that match the specified condition.
        /// </summary>
        /// <param name="transaction">The current transaction.</param>
        /// <param name="condition">The conditions.</param>
        /// <param name="skipCount">The number of entities to skip</param>
        /// <param name="maximumCount">The maximum number of entities to retrieve.</param>
        /// <returns>The entities.</returns>
        public IEnumerable<TEntity> Query(IDbTransaction transaction, TCondition condition, int skipCount = 0, int? maximumCount = null)
        {
            return Executor.ExecuteOnTransaction(
                (this, condition, skipCount, maximumCount),
                transaction,
                (x, commandActivator) => x.Item1.Query(commandActivator, x.condition, x.skipCount, x.maximumCount)
                );
        }

        /// <summary>
        /// Finds entities that match the specified condition.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="condition">The conditions.</param>
        /// <param name="skipCount">The number of entities to skip</param>
        /// <param name="maximumCount">The maximum number of entities to retrieve.</param>
        /// <returns>The entities.</returns>
        public abstract IEnumerable<TEntity> Query(Func<IDbCommand> commandActivator, TCondition condition, int skipCount = 0, int? maximumCount = null);

        #endregion

        #region GetCount

        /// <inheritdoc/>
        public int GetCount(TCondition condition)
        {
            return Executor.ExecuteOnNewConnection(
                (this, condition),
                (x, commandActivator) => x.Item1.GetCount(commandActivator, x.condition)
                );
        }

        /// <summary>
        /// Gets the number of entities that match the specified condition.
        /// </summary>
        /// <param name="connection">The current connection.</param>
        /// <param name="condition">The conditions.</param>
        /// <returns>Number of entities.</returns>
        public int GetCount(IDbConnection connection, TCondition condition)
        {
            return Executor.ExecuteOnConnection(
                (this, condition),
                connection,
                (x, commandActivator) => x.Item1.GetCount(commandActivator, x.condition)
                );
        }

        /// <summary>
        /// Gets the number of entities that match the specified condition.
        /// </summary>
        /// <param name="transaction">The current transaction.</param>
        /// <param name="condition">The conditions.</param>
        /// <returns>Number of entities.</returns>
        public int GetCount(IDbTransaction transaction, TCondition condition)
        {
            return Executor.ExecuteOnTransaction(
                (this, condition),
                transaction,
                (x, commandActivator) => x.Item1.GetCount(commandActivator, x.condition)
                );
        }

        /// <summary>
        /// Gets the number of entities that match the specified condition.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="condition">The conditions.</param>
        /// <returns>Number of entities.</returns>
        public abstract int GetCount(Func<IDbCommand> commandActivator, TCondition condition);

        #endregion
    }
}

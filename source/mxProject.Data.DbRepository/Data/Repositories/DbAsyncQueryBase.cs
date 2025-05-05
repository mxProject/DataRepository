using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace mxProject.Data.Repositories
{
    /// <summary>
    /// A basic implementation of an asynchronously query that uses a database.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TCondition">The query condition type.</typeparam>
    public abstract class DbAsyncQueryBase<TEntity, TCondition> : DbQueryBase<TEntity, TCondition>, IAsyncDataQuery<TEntity, TCondition>
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="connectionActivator">The method to activate a connection.</param>
        /// <param name="useTransactionScope">A value that indicates whether to use ambient transactions using TransactionScope.</param>
        protected DbAsyncQueryBase(Func<IDbConnection> connectionActivator, bool useTransactionScope)
            : base(connectionActivator, useTransactionScope)
        {
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="connectionActivator">The method to activate a connection.</param>
        /// <param name="useTransactionScope">A value that indicates whether to use ambient transactions using TransactionScope.</param>
        /// <param name="configureCommand">A method to configure a command.</param>
        protected DbAsyncQueryBase(Func<IDbConnection> connectionActivator, bool useTransactionScope, Action<IDbCommand> configureCommand)
            : base(connectionActivator, useTransactionScope, configureCommand)
        {
        }

        #region QueryAsync

        /// <inheritdoc/>
        public IAsyncEnumerable<TEntity> QueryAsync(TCondition condition, int skipCount = 0, int? maximumCount = null, CancellationToken cancellationToken = default)
        {
            return Executor.ExecuteIteratorOnNewConnectionAsync(
                (this, condition, skipCount, maximumCount, cancellationToken),
                (x, commandActivator) => x.Item1.QueryAsync(commandActivator, x.condition, x.skipCount, x.maximumCount, x.cancellationToken)
                );
        }

        /// <summary>
        /// Finds entities that match the specified condition.
        /// </summary>
        /// <param name="connection">The current connection.</param>
        /// <param name="condition">The conditions.</param>
        /// <param name="skipCount">The number of entities to skip</param>
        /// <param name="maximumCount">The maximum number of entities to retrieve.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entities.</returns>
        public IAsyncEnumerable<TEntity> QueryAsync(IDbConnection connection, TCondition condition, int skipCount = 0, int? maximumCount = null, CancellationToken cancellationToken = default)
        {
            return Executor.ExecuteIteratorOnConnectionAsync(
                (this, condition, skipCount, maximumCount, cancellationToken),
                connection,
                (x, commandActivator) => x.Item1.QueryAsync(commandActivator, x.condition, x.skipCount, x.maximumCount, x.cancellationToken)
                );
        }

        /// <summary>
        /// Finds entities that match the specified condition.
        /// </summary>
        /// <param name="transaction">The current transaction.</param>
        /// <param name="condition">The conditions.</param>
        /// <param name="skipCount">The number of entities to skip</param>
        /// <param name="maximumCount">The maximum number of entities to retrieve.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entities.</returns>
        public IAsyncEnumerable<TEntity> QueryAsync(IDbTransaction transaction, TCondition condition, int skipCount = 0, int? maximumCount = null, CancellationToken cancellationToken = default)
        {
            return Executor.ExecuteIteratorOnTransactionAsync(
                (this, condition, skipCount, maximumCount, cancellationToken),
                transaction,
                (x, commandActivator) => x.Item1.QueryAsync(commandActivator, x.condition, x.skipCount, x.maximumCount, x.cancellationToken)
                );
        }

        /// <summary>
        /// Finds entities that match the specified condition.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="condition">The conditions.</param>
        /// <param name="skipCount">The number of entities to skip</param>
        /// <param name="maximumCount">The maximum number of entities to retrieve.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entities.</returns>
        protected abstract IAsyncEnumerable<TEntity> QueryAsync(Func<IDbCommand> commandActivator, TCondition condition, int skipCount, int? maximumCount, CancellationToken cancellationToken);

        #endregion

        #region GetCountAsync

        /// <inheritdoc/>
        public ValueTask<int> GetCountAsync(TCondition condition)
        {
            return Executor.ExecuteOnNewConnectionAsync(
                (this, condition),
                (x, commandActivator) => x.Item1.GetCountAsync(commandActivator, x.condition)
                );
        }

        /// <summary>
        /// Gets the number of entities that match the specified condition.
        /// </summary>
        /// <param name="connection">The current connection.</param>
        /// <param name="condition">The conditions.</param>
        /// <returns>Number of entities.</returns>
        public ValueTask<int> GetCountAsync(IDbConnection connection, TCondition condition)
        {
            return Executor.ExecuteOnConnectionAsync(
                (this, condition),
                connection,
                (x, commandActivator) => x.Item1.GetCountAsync(commandActivator, x.condition)
                );
        }

        /// <summary>
        /// Gets the number of entities that match the specified condition.
        /// </summary>
        /// <param name="transaction">The current transaction.</param>
        /// <param name="condition">The conditions.</param>
        /// <returns>Number of entities.</returns>
        public ValueTask<int> GetCountAsync(IDbTransaction transaction, TCondition condition)
        {
            return Executor.ExecuteOnTransactionAsync(
                (this, condition),
                transaction,
                (x, commandActivator) => x.Item1.GetCountAsync(commandActivator, x.condition)
                );
        }

        /// <summary>
        /// Gets the number of entities that match the specified condition.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="condition">The conditions.</param>
        /// <returns>Number of entities.</returns>
        protected abstract ValueTask<int> GetCountAsync(Func<IDbCommand> commandActivator, TCondition condition);

        #endregion
    }
}

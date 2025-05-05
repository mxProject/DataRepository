using System;
using System.Collections.Generic;
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
    /// <typeparam name="TContext">The context type.</typeparam>
    public abstract class DbAsyncQueryWithContextBase<TEntity, TCondition, TContext> : DbQueryWithContextBase<TEntity, TCondition, TContext>, IAsyncDataQueryWithContext<TEntity, TCondition, TContext>
        where TContext : IDataRepositoryContext
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="connectionActivator">The method to activate a connection.</param>
        /// <param name="useTransactionScope">A value that indicates whether to use ambient transactions using TransactionScope.</param>
        protected DbAsyncQueryWithContextBase(Func<IDbConnection> connectionActivator, bool useTransactionScope)
            : base(connectionActivator, useTransactionScope)
        {
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="connectionActivator">The method to activate a connection.</param>
        /// <param name="useTransactionScope">A value that indicates whether to use ambient transactions using TransactionScope.</param>
        /// <param name="configureCommand">A method to configure a command.</param>
        protected DbAsyncQueryWithContextBase(Func<IDbConnection> connectionActivator, bool useTransactionScope, Action<IDbCommand> configureCommand)
            : base(connectionActivator, useTransactionScope, configureCommand)
        {
        }

        #region QueryAsync

        /// <inheritdoc/>
        public IAsyncEnumerable<TEntity> QueryAsync(TCondition condition, TContext context, int skipCount = 0, int? maximumCount = null, CancellationToken cancellationToken = default)
        {
            if (TryGetCurrentTransaction(context, out var transaction))
            {
                return QueryAsync(transaction, condition, context, skipCount, maximumCount, cancellationToken);
            }
            else if (TryGetCurrentConnection(context, out var connection))
            {
                return QueryAsync(connection, condition, context, skipCount, maximumCount, cancellationToken);
            }
            else
            {
                return Executor.ExecuteIteratorOnNewConnectionAsync(
                    (this, condition, skipCount, maximumCount, cancellationToken),
                    (x, commandActivator, cotext) => x.Item1.QueryAsync(commandActivator, x.condition, context, x.skipCount, x.maximumCount, x.cancellationToken),
                    context
                    );
            }
        }

        /// <summary>
        /// Finds entities that match the specified condition.
        /// </summary>
        /// <param name="connection">The current connection.</param>
        /// <param name="condition">The conditions.</param>
        /// <param name="context">The current context.</param>
        /// <param name="skipCount">The number of entities to skip</param>
        /// <param name="maximumCount">The maximum number of entities to retrieve.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entities.</returns>
        public IAsyncEnumerable<TEntity> QueryAsync(IDbConnection connection, TCondition condition, TContext context, int skipCount = 0, int? maximumCount = null, CancellationToken cancellationToken = default)
        {
            return Executor.ExecuteIteratorOnConnectionAsync(
                (this, condition, skipCount, maximumCount, cancellationToken),
                connection,
                (x, commandActivator, context) => x.Item1.QueryAsync(commandActivator, x.condition, context, x.skipCount, x.maximumCount, x.cancellationToken),
                context
                );
        }

        /// <summary>
        /// Finds entities that match the specified condition.
        /// </summary>
        /// <param name="transaction">The current transaction.</param>
        /// <param name="condition">The conditions.</param>
        /// <param name="context">The current context.</param>
        /// <param name="skipCount">The number of entities to skip</param>
        /// <param name="maximumCount">The maximum number of entities to retrieve.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entities.</returns>
        public IAsyncEnumerable<TEntity> QueryAsync(IDbTransaction transaction, TCondition condition, TContext context, int skipCount = 0, int? maximumCount = null, CancellationToken cancellationToken = default)
        {
            return Executor.ExecuteIteratorOnTransactionAsync(
                (this, condition, skipCount, maximumCount, cancellationToken),
                transaction,
                (x, commandActivator, context) => x.Item1.QueryAsync(commandActivator, x.condition, context, x.skipCount, x.maximumCount, x.cancellationToken),
                context
                );
        }

        /// <summary>
        /// Finds entities that match the specified condition.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="condition">The conditions.</param>
        /// <param name="context">The current context.</param>
        /// <param name="skipCount">The number of entities to skip</param>
        /// <param name="maximumCount">The maximum number of entities to retrieve.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entities.</returns>
        public abstract IAsyncEnumerable<TEntity> QueryAsync(Func<IDbCommand> commandActivator, TCondition condition, TContext context, int skipCount, int? maximumCount, CancellationToken cancellationToken);

        #endregion

        #region GetCountAsync

        /// <inheritdoc/>
        public ValueTask<int> GetCountAsync(TCondition condition, TContext context)
        {
            if (TryGetCurrentTransaction(context, out var transaction))
            {
                return GetCountAsync(transaction, condition, context);
            }
            else if (TryGetCurrentConnection(context, out var connection))
            {
                return GetCountAsync(connection, condition, context);
            }
            else
            {
                return Executor.ExecuteOnNewConnectionAsync(
                    (this, condition),
                    (x, commandActivator, cotext) => x.Item1.GetCountAsync(commandActivator, x.condition, context),
                    context
                    );
            }
        }

        /// <summary>
        /// Gets the number of entities that match the specified condition.
        /// </summary>
        /// <param name="connection">The current connection.</param>
        /// <param name="condition">The conditions.</param>
        /// <param name="context">The current context.</param>
        /// <returns>Number of entities.</returns>
        public ValueTask<int> GetCountAsync(IDbConnection connection, TCondition condition, TContext context)
        {
            return Executor.ExecuteOnConnectionAsync(
                (this, condition),
                connection,
                (x, commandActivator, context) => x.Item1.GetCountAsync(commandActivator, x.condition, context),
                context
                );
        }

        /// <summary>
        /// Gets the number of entities that match the specified condition.
        /// </summary>
        /// <param name="transaction">The current transaction.</param>
        /// <param name="condition">The conditions.</param>
        /// <param name="context">The current context.</param>
        /// <returns>Number of entities.</returns>
        public ValueTask<int> GetCountAsync(IDbTransaction transaction, TCondition condition, TContext context)
        {
            return Executor.ExecuteOnTransactionAsync(
                (this, condition),
                transaction,
                (x, commandActivator, context) => x.Item1.GetCountAsync(commandActivator, x.condition, context),
                context
                );
        }

        /// <summary>
        /// Gets the number of entities that match the specified condition.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="condition">The conditions.</param>
        /// <param name="context">The current context.</param>
        /// <returns>Number of entities.</returns>
        public abstract ValueTask<int> GetCountAsync(Func<IDbCommand> commandActivator, TCondition condition, TContext context);

        #endregion
    }
}

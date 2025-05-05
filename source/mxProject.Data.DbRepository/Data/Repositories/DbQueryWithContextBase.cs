using System;
using System.Collections.Generic;
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
    /// <typeparam name="TContext">The context type.</typeparam>
    public abstract class DbQueryWithContextBase<TEntity, TCondition, TContext> : DbRepositoryWithContextBase<TEntity, TContext>, IDataQueryWithContext<TEntity, TCondition, TContext>
        where TContext : IDataRepositoryContext
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="connectionActivator">The method to activate a connection.</param>
        /// <param name="useTransactionScope">A value that indicates whether to use ambient transactions using TransactionScope.</param>
        protected DbQueryWithContextBase(Func<IDbConnection> connectionActivator, bool useTransactionScope)
            : base(connectionActivator, useTransactionScope)
        {
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="connectionActivator">The method to activate a connection.</param>
        /// <param name="useTransactionScope">A value that indicates whether to use ambient transactions using TransactionScope.</param>
        /// <param name="configureCommand">A method to configure a command.</param>
        protected DbQueryWithContextBase(Func<IDbConnection> connectionActivator, bool useTransactionScope, Action<IDbCommand> configureCommand)
            : base(connectionActivator, useTransactionScope, configureCommand)
        {
        }

        #region Query

        /// <inheritdoc/>
        public IEnumerable<TEntity> Query(TCondition condition, TContext context, int skipCount = 0, int? maximumCount = null)
        {
            if (TryGetCurrentTransaction(context, out var transaction))
            {
                return Query(transaction, condition, context, skipCount, maximumCount);
            }
            else if (TryGetCurrentConnection(context, out var connection))
            {
                return Query(connection, condition, context, skipCount, maximumCount);
            }
            else
            {
                return Executor.ExecuteIteratorOnNewConnection(
                    (this, condition, skipCount, maximumCount),
                    (x, commandActivator, cotext) => x.Item1.Query(commandActivator, x.condition, context, x.skipCount, x.maximumCount),
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
        /// <returns>The entities.</returns>
        public IEnumerable<TEntity> Query(IDbConnection connection, TCondition condition, TContext context, int skipCount = 0, int? maximumCount = null)
        {
            return Executor.ExecuteOnConnection(
                (this, condition, skipCount, maximumCount),
                connection,
                (x, commandActivator, context) => x.Item1.Query(commandActivator, x.condition, context, x.skipCount, x.maximumCount),
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
        /// <returns>The entities.</returns>
        public IEnumerable<TEntity> Query(IDbTransaction transaction, TCondition condition, TContext context, int skipCount = 0, int? maximumCount = null)
        {
            return Executor.ExecuteOnTransaction(
                (this, condition, skipCount, maximumCount),
                transaction,
                (x, commandActivator, context) => x.Item1.Query(commandActivator, x.condition, context, x.skipCount, x.maximumCount),
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
        /// <returns>The entities.</returns>
        protected abstract IEnumerable<TEntity> Query(Func<IDbCommand> commandActivator, TCondition condition, TContext context, int skipCount, int? maximumCount);

        #endregion

        #region GetCount

        /// <inheritdoc/>
        public int GetCount(TCondition condition, TContext context)
        {
            if (TryGetCurrentTransaction(context, out var transaction))
            {
                return GetCount(transaction, condition, context);
            }
            else if (TryGetCurrentConnection(context, out var connection))
            {
                return GetCount(connection, condition, context);
            }
            else
            {
                return Executor.ExecuteOnNewConnection(
                    (this, condition),
                    (x, commandActivator, cotext) => x.Item1.GetCount(commandActivator, x.condition, context),
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
        public int GetCount(IDbConnection connection, TCondition condition, TContext context)
        {
            return Executor.ExecuteOnConnection(
                (this, condition),
                connection,
                (x, commandActivator, context) => x.Item1.GetCount(commandActivator, x.condition, context),
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
        public int GetCount(IDbTransaction transaction, TCondition condition, TContext context)
        {
            return Executor.ExecuteOnTransaction(
                (this, condition),
                transaction,
                (x, commandActivator, context) => x.Item1.GetCount(commandActivator, x.condition, context),
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
        public abstract int GetCount(Func<IDbCommand> commandActivator, TCondition condition, TContext context);

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Text;

namespace mxProject.Data.Repositories
{
    /// <summary>
    /// A basic implementation of a repository that uses a database.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TContext">The context type.</typeparam>
    public abstract class DbRepositoryWithContextBase<TEntity, TContext> : IDataRepository<TEntity>
        where TContext : IDataRepositoryContext
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="connectionActivator">The method to activate a connection.</param>
        /// <param name="useTransactionScope">A value that indicates whether to use ambient transactions using TransactionScope.</param>
        protected DbRepositoryWithContextBase(Func<IDbConnection> connectionActivator, bool useTransactionScope)
        {
            ConnectionActivator = connectionActivator;
            Executor = new DbCommandExecutorWithContext<TContext>(connectionActivator, useTransactionScope);
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="connectionActivator">The method to activate a connection.</param>
        /// <param name="useTransactionScope">A value that indicates whether to use ambient transactions using TransactionScope.</param>
        /// <param name="configureCommand">A method to configure a command.</param>
        protected DbRepositoryWithContextBase(Func<IDbConnection> connectionActivator, bool useTransactionScope, Action<IDbCommand> configureCommand)
        {
            ConnectionActivator = connectionActivator;
            Executor = new DbCommandExecutorWithContext<TContext>(connectionActivator, useTransactionScope, configureCommand);
        }

        /// <summary>
        /// Gets the method to activate a connection.
        /// </summary>
        protected Func<IDbConnection> ConnectionActivator { get; }

        /// <summary>
        /// Gets the executor.
        /// </summary>
        protected DbCommandExecutorWithContext<TContext> Executor { get; }

        /// <summary>
        /// Gets a value that indicates whether to use ambient transactions using TransactionScope.
        /// </summary>
        public bool UseTransactionScope { get; }

        #region Dispose

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing">A value indicating whether the Dispose method has been called.</param>
        protected virtual void Dispose(bool disposing)
        {
        }

        #endregion

        #region context

        /// <summary>
        /// Gets the current connection from the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="connection">The connection that found.</param>
        /// <returns>Returns true if a connection was obtained, otherwize false.</returns>
        protected bool TryGetCurrentConnection(TContext context, out IDbConnection connection)
        {
            var dbContext = context as IDbContext;

            connection = dbContext?.GetCurrentConnection()!;

            return connection != null;
        }

        /// <summary>
        /// Gets the current transaction from the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="transaction">The transaction that found.</param>
        /// <returns>Returns true if a transaction was obtained, otherwize false.</returns>
        protected bool TryGetCurrentTransaction(TContext context, out IDbTransaction transaction)
        {
            var dbContext = context as IDbContext;

            transaction = dbContext?.GetCurrentTransaction()!;

            return transaction != null && transaction.Connection != null;
        }

        #endregion
    }
}

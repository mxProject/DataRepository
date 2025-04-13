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
    public abstract class DbRepositoryBase<TEntity> : IDataRepository<TEntity>
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="connectionActivator">A method to activate a connection.</param>
        /// <param name="useTransactionScope">A value that indicates whether to use ambient transactions using TransactionScope.</param>
        protected DbRepositoryBase(Func<IDbConnection> connectionActivator, bool useTransactionScope)
        {
            ConnectionActivator = connectionActivator;
            Executor = new DbCommandExecutor(connectionActivator, useTransactionScope);
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="connectionActivator">A method to activate a connection.</param>
        /// <param name="useTransactionScope">A value that indicates whether to use ambient transactions using TransactionScope.</param>
        /// <param name="configureCommand">A method to configure a command.</param>
        protected DbRepositoryBase(Func<IDbConnection> connectionActivator, bool useTransactionScope, Action<IDbCommand> configureCommand)
        {
            ConnectionActivator = connectionActivator;
            Executor = new DbCommandExecutor(connectionActivator, useTransactionScope, configureCommand);
        }

        /// <summary>
        /// Gets the method to activate a connection.
        /// </summary>
        protected Func<IDbConnection> ConnectionActivator { get; }

        /// <summary>
        /// Gets the executor.
        /// </summary>
        protected DbCommandExecutor Executor { get; }

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
    }
}

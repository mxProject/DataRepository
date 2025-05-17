using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace mxProject.Data.Repositories
{
    /// <summary>
    /// A basic implementation of an asynchronously repository that uses a database.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TPrimaryKey">The primary key type.</typeparam>
    /// <typeparam name="TUniqueKey">The unique key type.</typeparam>
    public abstract class DbAsyncReadRepositoryWithUniqueKeyBase<TEntity, TPrimaryKey, TUniqueKey> : DbReadRepositoryWithUniqueKeyBase<TEntity, TPrimaryKey, TUniqueKey>, IAsyncReadDataRepositoryWithUniqueKey<TEntity, TPrimaryKey, TUniqueKey>
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="connectionActivator">The method to activate a connection.</param>
        /// <param name="useTransactionScope">A value that indicates whether to use ambient transactions using TransactionScope.</param>
        protected DbAsyncReadRepositoryWithUniqueKeyBase(Func<IDbConnection> connectionActivator, bool useTransactionScope)
            : base(connectionActivator, useTransactionScope)
        {
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="connectionActivator">The method to activate a connection.</param>
        /// <param name="useTransactionScope">A value that indicates whether to use ambient transactions using TransactionScope.</param>
        /// <param name="configureCommand">A method to configure a command.</param>
        protected DbAsyncReadRepositoryWithUniqueKeyBase(Func<IDbConnection> connectionActivator, bool useTransactionScope, Action<IDbCommand> configureCommand)
            : base(connectionActivator, useTransactionScope, configureCommand)
        {
        }

        #region GetByPrimaryKeyAsync

        /// <inheritdoc/>
        public ValueTask<TEntity?> GetByPrimaryKeyAsync(TPrimaryKey primaryKey)
        {
            return Executor.ExecuteOnNewConnectionAsync(
                (this, primaryKey),
                (x, commandActivator) => x.Item1.GetByPrimaryKeyAsync(commandActivator, x.primaryKey)
                );
        }

        /// <summary>
        /// Gets the entity corresponding to the specified primary key.
        /// </summary>
        /// <param name="connection">The current connection.</param>
        /// <param name="primaryKey">The primary key.</param>
        /// <returns>The entity.</returns>
        public ValueTask<TEntity?> GetByPrimaryKeyAsync(IDbConnection connection, TPrimaryKey primaryKey)
        {
            return Executor.ExecuteOnConnectionAsync(
                (this, primaryKey),
                connection,
                (x, commandActivator) => x.Item1.GetByPrimaryKeyAsync(commandActivator, x.primaryKey)
                );
        }

        /// <summary>
        /// Gets the entity corresponding to the specified primary key.
        /// </summary>
        /// <param name="transaction">The current transaction.</param>
        /// <param name="primaryKey">The primary key.</param>
        /// <returns>The entity.</returns>
        public ValueTask<TEntity?> GetByPrimaryKeyAsync(IDbTransaction transaction, TPrimaryKey primaryKey)
        {
            return Executor.ExecuteOnTransactionAsync(
                (this, primaryKey),
                transaction,
                (x, commandActivator) => x.Item1.GetByPrimaryKeyAsync(commandActivator, x.primaryKey)
                );
        }

        /// <summary>
        /// Gets the entity corresponding to the specified primary key.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="primaryKey">The primary key.</param>
        /// <returns>The entity.</returns>
        protected abstract ValueTask<TEntity?> GetByPrimaryKeyAsync(Func<IDbCommand> commandActivator, TPrimaryKey primaryKey);

        #endregion

        #region GetByUniqueKeyAsync

        /// <inheritdoc/>
        public ValueTask<TEntity?> GetByUniqueKeyAsync(TUniqueKey uniqueKey)
        {
            return Executor.ExecuteOnNewConnectionAsync(
                (this, uniqueKey),
                (x, commandActivator) => x.Item1.GetByUniqueKeyAsync(commandActivator, x.uniqueKey)
                );
        }

        /// <summary>
        /// Gets the entity corresponding to the specified unique key.
        /// </summary>
        /// <param name="connection">The current connection.</param>
        /// <param name="uniqueKey">The unique key.</param>
        /// <returns>The entity.</returns>
        public ValueTask<TEntity?> GetByUniqueKeyAsync(IDbConnection connection, TUniqueKey uniqueKey)
        {
            return Executor.ExecuteOnConnectionAsync(
                (this, uniqueKey),
                connection,
                (x, commandActivator) => x.Item1.GetByUniqueKeyAsync(commandActivator, x.uniqueKey)
                );
        }

        /// <summary>
        /// Gets the entity corresponding to the specified unique key.
        /// </summary>
        /// <param name="transaction">The current transaction.</param>
        /// <param name="uniqueKey">The unique key.</param>
        /// <returns>The entity.</returns>
        public ValueTask<TEntity?> GetByUniqueKeyAsync(IDbTransaction transaction, TUniqueKey uniqueKey)
        {
            return Executor.ExecuteOnTransactionAsync(
                (this, uniqueKey),
                transaction,
                (x, commandActivator) => x.Item1.GetByUniqueKeyAsync(commandActivator, x.uniqueKey)
                );
        }

        /// <summary>
        /// Gets the entity corresponding to the specified unique key.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="uniqueKey">The unique key.</param>
        /// <returns>The entity.</returns>
        protected abstract ValueTask<TEntity?> GetByUniqueKeyAsync(Func<IDbCommand> commandActivator, TUniqueKey uniqueKey);

        #endregion

        #region GetRangeByPrimaryKeyAsync

        /// <inheritdoc/>
        public IAsyncEnumerable<TEntity> GetRangeByPrimaryKeyAsync(IEnumerable<TPrimaryKey> primaryKeys, CancellationToken cancellationToken = default)
        {
            return Executor.ExecuteIteratorOnNewConnectionAsync(
                (this, primaryKeys, cancellationToken),
                (x, commandActivator) => x.Item1.GetRangeByPrimaryKeyAsync(commandActivator, x.primaryKeys, x.cancellationToken)
                );
        }

        /// <summary>
        /// Gets the entity corresponding to the specified primary keys.
        /// </summary>
        /// <param name="connection">The current connection.</param>
        /// <param name="primaryKeys">The primary keys.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entities.</returns>
        public IAsyncEnumerable<TEntity> GetRangeByPrimaryKeyAsync(IDbConnection connection, IEnumerable<TPrimaryKey> primaryKeys, CancellationToken cancellationToken = default)
        {
            return Executor.ExecuteIteratorOnConnectionAsync(
                (this, primaryKeys, cancellationToken),
                connection,
                (x, commandActivator) => x.Item1.GetRangeByPrimaryKeyAsync(commandActivator, x.primaryKeys, x.cancellationToken)
                );
        }

        /// <summary>
        /// Gets the entity corresponding to the specified primary keys.
        /// </summary>
        /// <param name="transaction">The current transaction.</param>
        /// <param name="primaryKeys">The primary keys.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entities.</returns>
        public IAsyncEnumerable<TEntity> GetRangeByPrimaryKeyAsync(IDbTransaction transaction, IEnumerable<TPrimaryKey> primaryKeys, CancellationToken cancellationToken = default)
        {
            return Executor.ExecuteIteratorOnTransactionAsync(
                (this, primaryKeys, cancellationToken),
                transaction,
                (x, commandActivator) => x.Item1.GetRangeByPrimaryKeyAsync(commandActivator, x.primaryKeys, x.cancellationToken)
                );
        }

        /// <summary>
        /// Gets the entity corresponding to the specified primary keys.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="primaryKeys">The primary keys.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entities.</returns>
        protected abstract IAsyncEnumerable<TEntity> GetRangeByPrimaryKeyAsync(Func<IDbCommand> commandActivator, IEnumerable<TPrimaryKey> primaryKeys, CancellationToken cancellationToken);

        /// <inheritdoc/>
        public IAsyncEnumerable<TEntity> GetRangeByPrimaryKeyAsync(IAsyncEnumerable<TPrimaryKey> primaryKeys, CancellationToken cancellationToken = default)
        {
            return Executor.ExecuteIteratorOnNewConnectionAsync(
                (this, primaryKeys, cancellationToken),
                (x, commandActivator) => x.Item1.GetRangeByPrimaryKeyAsync(commandActivator, x.primaryKeys, x.cancellationToken)
                );
        }

        /// <summary>
        /// Gets the entity corresponding to the specified primary keys.
        /// </summary>
        /// <param name="connection">The current connection.</param>
        /// <param name="primaryKeys">The primary keys.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entities.</returns>
        public IAsyncEnumerable<TEntity> GetRangeByPrimaryKeyAsync(IDbConnection connection, IAsyncEnumerable<TPrimaryKey> primaryKeys, CancellationToken cancellationToken = default)
        {
            return Executor.ExecuteIteratorOnConnectionAsync(
                (this, primaryKeys, cancellationToken),
                connection,
                (x, commandActivator) => x.Item1.GetRangeByPrimaryKeyAsync(commandActivator, x.primaryKeys, x.cancellationToken)
                );
        }

        /// <summary>
        /// Gets the entity corresponding to the specified primary keys.
        /// </summary>
        /// <param name="transaction">The current transaction.</param>
        /// <param name="primaryKeys">The primary keys.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entities.</returns>
        public IAsyncEnumerable<TEntity> GetRangeByPrimaryKeyAsync(IDbTransaction transaction, IAsyncEnumerable<TPrimaryKey> primaryKeys, CancellationToken cancellationToken = default)
        {
            return Executor.ExecuteIteratorOnTransactionAsync(
                (this, primaryKeys, cancellationToken),
                transaction,
                (x, commandActivator) => x.Item1.GetRangeByPrimaryKeyAsync(commandActivator, x.primaryKeys, x.cancellationToken)
                );
        }

        /// <summary>
        /// Gets the entity corresponding to the specified primary keys.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="primaryKeys">The primary keys.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entities.</returns>
        protected abstract IAsyncEnumerable<TEntity> GetRangeByPrimaryKeyAsync(Func<IDbCommand> commandActivator, IAsyncEnumerable<TPrimaryKey> primaryKeys, CancellationToken cancellationToken);

        #endregion

        #region GetRangeByUniqueKeyAsync

        /// <inheritdoc/>
        public IAsyncEnumerable<TEntity> GetRangeByUniqueKeyAsync(IEnumerable<TUniqueKey> uniqueKeys, CancellationToken cancellationToken = default)
        {
            return Executor.ExecuteIteratorOnNewConnectionAsync(
                (this, uniqueKeys, cancellationToken),
                (x, commandActivator) => x.Item1.GetRangeByUniqueKeyAsync(commandActivator, x.uniqueKeys, x.cancellationToken)
                );
        }

        /// <summary>
        /// Gets the entity corresponding to the specified unique keys.
        /// </summary>
        /// <param name="connection">The current connection.</param>
        /// <param name="uniqueKeys">The unique keys.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entities.</returns>
        public IAsyncEnumerable<TEntity> GetRangeByUniqueKeyAsync(IDbConnection connection, IEnumerable<TUniqueKey> uniqueKeys, CancellationToken cancellationToken = default)
        {
            return Executor.ExecuteIteratorOnConnectionAsync(
                (this, uniqueKeys, cancellationToken),
                connection,
                (x, commandActivator) => x.Item1.GetRangeByUniqueKeyAsync(commandActivator, x.uniqueKeys, x.cancellationToken)
                );
        }

        /// <summary>
        /// Gets the entity corresponding to the specified unique keys.
        /// </summary>
        /// <param name="transaction">The current transaction.</param>
        /// <param name="uniqueKeys">The unique keys.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entities.</returns>
        public IAsyncEnumerable<TEntity> GetRangeByUniqueKeyAsync(IDbTransaction transaction, IEnumerable<TUniqueKey> uniqueKeys, CancellationToken cancellationToken = default)
        {
            return Executor.ExecuteIteratorOnTransactionAsync(
                (this, uniqueKeys, cancellationToken),
                transaction,
                (x, commandActivator) => x.Item1.GetRangeByUniqueKeyAsync(commandActivator, x.uniqueKeys, x.cancellationToken)
                );
        }

        /// <summary>
        /// Gets the entity corresponding to the specified unique keys.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="uniqueKeys">The unique keys.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entities.</returns>
        protected abstract IAsyncEnumerable<TEntity> GetRangeByUniqueKeyAsync(Func<IDbCommand> commandActivator, IEnumerable<TUniqueKey> uniqueKeys, CancellationToken cancellationToken);

        /// <inheritdoc/>
        public IAsyncEnumerable<TEntity> GetRangeByUniqueKeyAsync(IAsyncEnumerable<TUniqueKey> uniqueKeys, CancellationToken cancellationToken = default)
        {
            return Executor.ExecuteIteratorOnNewConnectionAsync(
                (this, uniqueKeys, cancellationToken),
                (x, commandActivator) => x.Item1.GetRangeByUniqueKeyAsync(commandActivator, x.uniqueKeys, x.cancellationToken)
                );
        }

        /// <summary>
        /// Gets the entity corresponding to the specified unique keys.
        /// </summary>
        /// <param name="connection">The current connection.</param>
        /// <param name="uniqueKeys">The unique keys.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entities.</returns>
        public IAsyncEnumerable<TEntity> GetRangeByUniqueKeyAsync(IDbConnection connection, IAsyncEnumerable<TUniqueKey> uniqueKeys, CancellationToken cancellationToken = default)
        {
            return Executor.ExecuteIteratorOnConnectionAsync(
                (this, uniqueKeys, cancellationToken),
                connection,
                (x, commandActivator) => x.Item1.GetRangeByUniqueKeyAsync(commandActivator, x.uniqueKeys, x.cancellationToken)
                );
        }

        /// <summary>
        /// Gets the entity corresponding to the specified unique keys.
        /// </summary>
        /// <param name="transaction">The current transaction.</param>
        /// <param name="uniqueKeys">The unique keys.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entities.</returns>
        public IAsyncEnumerable<TEntity> GetRangeByUniqueKeyAsync(IDbTransaction transaction, IAsyncEnumerable<TUniqueKey> uniqueKeys, CancellationToken cancellationToken = default)
        {
            return Executor.ExecuteIteratorOnTransactionAsync(
                (this, uniqueKeys, cancellationToken),
                transaction,
                (x, commandActivator) => x.Item1.GetRangeByUniqueKeyAsync(commandActivator, x.uniqueKeys, x.cancellationToken)
                );
        }

        /// <summary>
        /// Gets the entity corresponding to the specified unique keys.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="uniqueKeys">The unique keys.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entities.</returns>
        protected abstract IAsyncEnumerable<TEntity> GetRangeByUniqueKeyAsync(Func<IDbCommand> commandActivator, IAsyncEnumerable<TUniqueKey> uniqueKeys, CancellationToken cancellationToken);

        #endregion

        #region GetAllAsync

        /// <inheritdoc/>
        public IAsyncEnumerable<TEntity> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return Executor.ExecuteIteratorOnNewConnectionAsync(
                (this, cancellationToken),
                (x, commandActivator) => x.Item1.GetAllAsync(commandActivator, x.cancellationToken)
                );
        }

        /// <summary>
        /// Gets all entities.
        /// </summary>
        /// <param name="connection">The current connection.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entities.</returns>
        public IAsyncEnumerable<TEntity> GetAllAsync(IDbConnection connection, CancellationToken cancellationToken = default)
        {
            return Executor.ExecuteIteratorOnConnectionAsync(
                (this, cancellationToken),
                connection,
                (x, commandActivator) => x.Item1.GetAllAsync(commandActivator, x.cancellationToken)
                );
        }

        /// <summary>
        /// Gets all entities.
        /// </summary>
        /// <param name="transaction">The current transaction.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entities.</returns>
        public IAsyncEnumerable<TEntity> GetAll(IDbTransaction transaction, CancellationToken cancellationToken = default)
        {
            return Executor.ExecuteIteratorOnTransactionAsync(
                (this, cancellationToken),
                transaction,
                (x, commandActivator) => x.Item1.GetAllAsync(commandActivator, x.cancellationToken)
                );
        }

        /// <summary>
        /// Gets all entities.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entities.</returns>
        protected abstract IAsyncEnumerable<TEntity> GetAllAsync(Func<IDbCommand> commandActivator, CancellationToken cancellationToken);

        #endregion

        #region GetAllPrimaryKeysAsync

        /// <inheritdoc/>
        public IAsyncEnumerable<TPrimaryKey> GetAllPrimaryKeysAsync(CancellationToken cancellationToken = default)
        {
            return Executor.ExecuteIteratorOnNewConnectionAsync(
                (this, cancellationToken),
                (x, commandActivator) => x.Item1.GetAllPrimaryKeysAsync(commandActivator, x.cancellationToken)
                );
        }

        /// <summary>
        /// Gets all primary keys.
        /// </summary>
        /// <param name="connection">The curernt connection.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The primary keys.</returns>
        public IAsyncEnumerable<TPrimaryKey> GetAllPrimaryKeysAsync(IDbConnection connection, CancellationToken cancellationToken = default)
        {
            return Executor.ExecuteIteratorOnConnectionAsync(
                (this, cancellationToken),
                connection,
                (x, commandActivator) => x.Item1.GetAllPrimaryKeysAsync(commandActivator, x.cancellationToken)
                );
        }

        /// <summary>
        /// Gets all primary keys.
        /// </summary>
        /// <param name="transaction">The curernt transaction.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The primary keys.</returns>
        public IAsyncEnumerable<TPrimaryKey> GetAllPrimaryKeysAsync(IDbTransaction transaction, CancellationToken cancellationToken = default)
        {
            return Executor.ExecuteIteratorOnTransactionAsync(
                (this, cancellationToken),
                transaction,
                (x, commandActivator) => x.Item1.GetAllPrimaryKeysAsync(commandActivator, x.cancellationToken)
                );
        }

        /// <summary>
        /// Gets all primary keys.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The primary keys.</returns>
        protected abstract IAsyncEnumerable<TPrimaryKey> GetAllPrimaryKeysAsync(Func<IDbCommand> commandActivator, CancellationToken cancellationToken);

        #endregion

        #region GetAllUniqueKeysAsync

        /// <inheritdoc/>
        public IAsyncEnumerable<TUniqueKey> GetAllUniqueKeysAsync(CancellationToken cancellationToken = default)
        {
            return Executor.ExecuteIteratorOnNewConnectionAsync(
                (this, cancellationToken),
                (x, commandActivator) => x.Item1.GetAllUniqueKeysAsync(commandActivator, x.cancellationToken)
                );
        }

        /// <summary>
        /// Gets all unique keys.
        /// </summary>
        /// <param name="connection">The curernt connection.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The unique keys.</returns>
        public IAsyncEnumerable<TUniqueKey> GetAllUniqueKeysAsync(IDbConnection connection, CancellationToken cancellationToken = default)
        {
            return Executor.ExecuteIteratorOnConnectionAsync(
                (this, cancellationToken),
                connection,
                (x, commandActivator) => x.Item1.GetAllUniqueKeysAsync(commandActivator, x.cancellationToken)
                );
        }

        /// <summary>
        /// Gets all unique keys.
        /// </summary>
        /// <param name="transaction">The curernt transaction.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The unique keys.</returns>
        public IAsyncEnumerable<TUniqueKey> GetAllUniqueKeysAsync(IDbTransaction transaction, CancellationToken cancellationToken = default)
        {
            return Executor.ExecuteIteratorOnTransactionAsync(
                (this, cancellationToken),
                transaction,
                (x, commandActivator) => x.Item1.GetAllUniqueKeysAsync(commandActivator, x.cancellationToken)
                );
        }

        /// <summary>
        /// Gets all unique keys.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The unique keys.</returns>
        protected abstract IAsyncEnumerable<TUniqueKey> GetAllUniqueKeysAsync(Func<IDbCommand> commandActivator, CancellationToken cancellationToken);

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace mxProject.Data.Repositories
{
    /// <summary>
    /// A basic implementation of a repository that uses a database.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TPrimaryKey">The primary key type.</typeparam>
    /// <typeparam name="TUniqueKey">The unique key type.</typeparam>
    public abstract class DbReadRepositoryWithUniqueKeyBase<TEntity, TPrimaryKey, TUniqueKey> : DbRepositoryBase<TEntity>, IReadDataRepositoryWithUniqueKey<TEntity, TPrimaryKey, TUniqueKey>
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="connectionActivator">The method to activate a connection.</param>
        /// <param name="useTransactionScope">A value that indicates whether to use ambient transactions using TransactionScope.</param>
        protected DbReadRepositoryWithUniqueKeyBase(Func<IDbConnection> connectionActivator, bool useTransactionScope)
            : base(connectionActivator, useTransactionScope)
        {
        }

        #region Get

        /// <inheritdoc/>
        public TEntity? GetByPrimaryKey(TPrimaryKey primaryKey)
        {
            return Executor.ExecuteOnNewConnection(
                (this, primaryKey),
                (x, commandActivator) => x.Item1.GetByPrimaryKey(commandActivator, x.primaryKey)
                );
        }

        /// <summary>
        /// Gets the entity corresponding to the specified primary key.
        /// </summary>
        /// <param name="connection">The current connection.</param>
        /// <param name="primaryKey">The primary key.</param>
        /// <returns>The entity.</returns>
        public TEntity? GetByPrimaryKey(IDbConnection connection, TPrimaryKey primaryKey)
        {
            return Executor.ExecuteOnConnection(
                (this, primaryKey),
                connection,
                (x, commandActivator) => x.Item1.GetByPrimaryKey(commandActivator, x.primaryKey)
                );
        }

        /// <summary>
        /// Gets the entity corresponding to the specified primary key.
        /// </summary>
        /// <param name="transaction">The current transaction.</param>
        /// <param name="primaryKey">The primary key.</param>
        /// <returns>The entity.</returns>
        public TEntity? GetByPrimaryKey(IDbTransaction transaction, TPrimaryKey primaryKey)
        {
            return Executor.ExecuteOnTransaction(
                (this, primaryKey),
                transaction,
                (x, commandActivator) => x.Item1.GetByPrimaryKey(commandActivator, x.primaryKey)
                );
        }

        /// <summary>
        /// Gets the entity corresponding to the specified primary key.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="primaryKey">The primary key.</param>
        /// <returns>The entity.</returns>
        protected abstract TEntity? GetByPrimaryKey(Func<IDbCommand> commandActivator, TPrimaryKey primaryKey);

        /// <inheritdoc/>
        public TEntity? GetByUniqueKey(TUniqueKey uniqueKey)
        {
            return Executor.ExecuteOnNewConnection(
                (this, uniqueKey),
                (x, commandActivator) => x.Item1.GetByUniqueKey(commandActivator, x.uniqueKey)
                );
        }

        /// <summary>
        /// Gets the entity corresponding to the specified unique key.
        /// </summary>
        /// <param name="connection">The current connection.</param>
        /// <param name="uniqueKey">The unique key.</param>
        /// <returns>The entity.</returns>
        public TEntity? GetByUniqueKey(IDbConnection connection, TUniqueKey uniqueKey)
        {
            return Executor.ExecuteOnConnection(
                (this, uniqueKey),
                connection,
                (x, commandActivator) => x.Item1.GetByUniqueKey(commandActivator, x.uniqueKey)
                );
        }

        /// <summary>
        /// Gets the entity corresponding to the specified unique key.
        /// </summary>
        /// <param name="transaction">The current transaction.</param>
        /// <param name="uniqueKey">The unique key.</param>
        /// <returns>The entity.</returns>
        public TEntity? GetByUniqueKey(IDbTransaction transaction, TUniqueKey uniqueKey)
        {
            return Executor.ExecuteOnTransaction(
                (this, uniqueKey),
                transaction,
                (x, commandActivator) => x.Item1.GetByUniqueKey(commandActivator, x.uniqueKey)
                );
        }

        /// <summary>
        /// Gets the entity corresponding to the specified unique key.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="uniqueKey">The unique key.</param>
        /// <returns>The entity.</returns>
        protected abstract TEntity? GetByUniqueKey(Func<IDbCommand> commandActivator, TUniqueKey uniqueKey);

        #endregion

        #region GetRange

        /// <inheritdoc/>
        public IEnumerable<TEntity> GetRangeByPrimaryKey(IEnumerable<TPrimaryKey> primaryKeys)
        {
            return Executor.ExecuteOnNewConnection(
                (this, primaryKeys),
                (x, commandActivator) => x.Item1.GetRangeByPrimaryKey(commandActivator, x.primaryKeys)
                );
        }

        /// <summary>
        /// Gets the entity corresponding to the specified primary keys.
        /// </summary>
        /// <param name="connection">The current connection.</param>
        /// <param name="primaryKeys">The primary keys.</param>
        /// <returns>The entities.</returns>
        public IEnumerable<TEntity> GetRangeByPrimaryKey(IDbConnection connection, IEnumerable<TPrimaryKey> primaryKeys)
        {
            return Executor.ExecuteOnConnection(
                (this, primaryKeys),
                connection,
                (x, commandActivator) => x.Item1.GetRangeByPrimaryKey(commandActivator, x.primaryKeys)
                );
        }

        /// <summary>
        /// Gets the entity corresponding to the specified primary keys.
        /// </summary>
        /// <param name="transaction">The current transaction.</param>
        /// <param name="primaryKeys">The primary keys.</param>
        /// <returns>The entities.</returns>
        public IEnumerable<TEntity> GetRangeByPrimaryKey(IDbTransaction transaction, IEnumerable<TPrimaryKey> primaryKeys)
        {
            return Executor.ExecuteOnTransaction(
                (this, primaryKeys),
                transaction,
                (x, commandActivator) => x.Item1.GetRangeByPrimaryKey(commandActivator, x.primaryKeys)
                );
        }

        /// <summary>
        /// Gets the entity corresponding to the specified primary keys.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="primaryKeys">The primary keys.</param>
        /// <returns>The entities.</returns>
        protected abstract IEnumerable<TEntity> GetRangeByPrimaryKey(Func<IDbCommand> commandActivator, IEnumerable<TPrimaryKey> primaryKeys);

        /// <inheritdoc/>
        public IEnumerable<TEntity> GetRangeByUniqueKey(IEnumerable<TUniqueKey> uniqueKeys)
        {
            return Executor.ExecuteOnNewConnection(
                (this, uniqueKeys),
                (x, commandActivator) => x.Item1.GetRangeByUniqueKey(commandActivator, x.uniqueKeys)
                );
        }

        /// <summary>
        /// Gets the entity corresponding to the specified unique keys.
        /// </summary>
        /// <param name="connection">The current connection.</param>
        /// <param name="uniqueKeys">The unique keys.</param>
        /// <returns>The entities.</returns>
        public IEnumerable<TEntity> GetRangeByUniqueKey(IDbConnection connection, IEnumerable<TUniqueKey> uniqueKeys)
        {
            return Executor.ExecuteOnConnection(
                (this, uniqueKeys),
                connection,
                (x, commandActivator) => x.Item1.GetRangeByUniqueKey(commandActivator, x.uniqueKeys)
                );
        }

        /// <summary>
        /// Gets the entity corresponding to the specified unique keys.
        /// </summary>
        /// <param name="transaction">The current transaction.</param>
        /// <param name="uniqueKeys">The unique keys.</param>
        /// <returns>The entities.</returns>
        public IEnumerable<TEntity> GetRangeByUniqueKey(IDbTransaction transaction, IEnumerable<TUniqueKey> uniqueKeys)
        {
            return Executor.ExecuteOnTransaction(
                (this, uniqueKeys),
                transaction,
                (x, commandActivator) => x.Item1.GetRangeByUniqueKey(commandActivator, x.uniqueKeys)
                );
        }

        /// <summary>
        /// Gets the entity corresponding to the specified unique keys.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="uniqueKeys">The unique keys.</param>
        /// <returns>The entities.</returns>
        protected abstract IEnumerable<TEntity> GetRangeByUniqueKey(Func<IDbCommand> commandActivator, IEnumerable<TUniqueKey> uniqueKeys);

        #endregion

        #region GetAll

        /// <inheritdoc/>
        public IEnumerable<TEntity> GetAll()
        {
            return Executor.ExecuteOnNewConnection(
                this,
                (x, commandActivator) => x.GetAll(commandActivator)
                );
        }

        /// <summary>
        /// Gets all entities.
        /// </summary>
        /// <param name="connection">The current connection.</param>
        /// <returns>The entities.</returns>
        public IEnumerable<TEntity> GetAll(IDbConnection connection)
        {
            return Executor.ExecuteOnConnection(
                this,
                connection,
                (x, commandActivator) => x.GetAll(commandActivator)
                );
        }

        /// <summary>
        /// Gets all entities.
        /// </summary>
        /// <param name="transaction">The current transaction.</param>
        /// <returns>The entities.</returns>
        public IEnumerable<TEntity> GetAll(IDbTransaction transaction)
        {
            return Executor.ExecuteOnTransaction(
                this,
                transaction,
                (x, commandActivator) => x.GetAll(commandActivator)
                );
        }

        /// <summary>
        /// Gets all entities.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <returns>The entities.</returns>
        protected abstract IEnumerable<TEntity> GetAll(Func<IDbCommand> commandActivator);

        #endregion

        #region GetAllPrimaryKeys

        /// <inheritdoc/>
        public IEnumerable<TPrimaryKey> GetAllPrimaryKeys()
        {
            return Executor.ExecuteOnNewConnection(
                this,
                (x, commandActivator) => x.GetAllPrimaryKeys(commandActivator)
                );
        }

        /// <summary>
        /// Gets all primary keys.
        /// </summary>
        /// <param name="connection">The curernt connection.</param>
        /// <returns>The primary keys.</returns>
        public IEnumerable<TPrimaryKey> GetAllPrimaryKeys(IDbConnection connection)
        {
            return Executor.ExecuteOnConnection(
                this,
                connection,
                (x, commandActivator) => x.GetAllPrimaryKeys(commandActivator)
                );
        }

        /// <summary>
        /// Gets all primary keys.
        /// </summary>
        /// <param name="transaction">The curernt transaction.</param>
        /// <returns>The primary keys.</returns>
        public IEnumerable<TPrimaryKey> GetAllPrimaryKeys(IDbTransaction transaction)
        {
            return Executor.ExecuteOnTransaction(
                this,
                transaction,
                (x, commandActivator) => x.GetAllPrimaryKeys(commandActivator)
                );
        }

        /// <summary>
        /// Gets all primary keys.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <returns>The primary keys.</returns>
        protected abstract IEnumerable<TPrimaryKey> GetAllPrimaryKeys(Func<IDbCommand> commandActivator);

        #endregion

        #region GetAllUniqueKeys

        /// <inheritdoc/>
        public IEnumerable<TUniqueKey> GetAllUniqueKeys()
        {
            return Executor.ExecuteOnNewConnection(
                this,
                (x, commandActivator) => x.GetAllUniqueKeys(commandActivator)
                );
        }

        /// <summary>
        /// Gets all unique keys.
        /// </summary>
        /// <param name="connection">The curernt connection.</param>
        /// <returns>The unique keys.</returns>
        public IEnumerable<TUniqueKey> GetAllUniqueKeys(IDbConnection connection)
        {
            return Executor.ExecuteOnConnection(
                this,
                connection,
                (x, commandActivator) => x.GetAllUniqueKeys(commandActivator)
                );
        }

        /// <summary>
        /// Gets all unique keys.
        /// </summary>
        /// <param name="transaction">The curernt transaction.</param>
        /// <returns>The unique keys.</returns>
        public IEnumerable<TUniqueKey> GetAllUniqueKeys(IDbTransaction transaction)
        {
            return Executor.ExecuteOnTransaction(
                this,
                transaction,
                (x, commandActivator) => x.GetAllUniqueKeys(commandActivator)
                );
        }

        /// <summary>
        /// Gets all unique keys.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <returns>The unique keys.</returns>
        protected abstract IEnumerable<TUniqueKey> GetAllUniqueKeys(Func<IDbCommand> commandActivator);

        #endregion
    }
}

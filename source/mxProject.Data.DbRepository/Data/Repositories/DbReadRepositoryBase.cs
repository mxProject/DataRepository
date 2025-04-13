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
    /// <typeparam name="TKey">The key type.</typeparam>
    public abstract class DbReadRepositoryBase<TEntity, TKey> : DbRepositoryBase<TEntity>, IReadDataRepository<TEntity, TKey>
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="connectionActivator">The method to activate a connection.</param>
        /// <param name="useTransactionScope">A value that indicates whether to use ambient transactions using TransactionScope.</param>
        protected DbReadRepositoryBase(Func<IDbConnection> connectionActivator, bool useTransactionScope)
            : base(connectionActivator, useTransactionScope)
        {
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="connectionActivator">The method to activate a connection.</param>
        /// <param name="useTransactionScope">A value that indicates whether to use ambient transactions using TransactionScope.</param>
        /// <param name="configureCommand">A method to configure a command.</param>
        protected DbReadRepositoryBase(Func<IDbConnection> connectionActivator, bool useTransactionScope, Action<IDbCommand> configureCommand)
            : base(connectionActivator, useTransactionScope, configureCommand)
        {
        }

        #region Get

        /// <inheritdoc/>
        public TEntity? Get(TKey key)
        {
            return Executor.ExecuteOnNewConnection(
                (this, key),
                (x, commandActivator) => x.Item1.Get(commandActivator, x.key)
                );
        }

        /// <summary>
        /// Gets the entity corresponding to the specified key.
        /// </summary>
        /// <param name="connection">The curernt connection.</param>
        /// <param name="key">The key.</param>
        /// <returns>The entity.</returns>
        public TEntity? Get(IDbConnection connection, TKey key)
        {
            return Executor.ExecuteOnConnection(
                (this, key),
                connection,
                (x, commandActivator) => x.Item1.Get(commandActivator, x.key)
                );
        }

        /// <summary>
        /// Gets the entity corresponding to the specified key.
        /// </summary>
        /// <param name="transaction">The curernt transaction.</param>
        /// <param name="key">The key.</param>
        /// <returns>The entity.</returns>
        public TEntity? Get(IDbTransaction transaction, TKey key)
        {
            return Executor.ExecuteOnTransaction(
                (this, key),
                transaction,
                (x, commandActivator) => x.Item1.Get(commandActivator, x.key)
                );
        }

        /// <summary>
        /// Gets the entity corresponding to the specified key.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="key">The key.</param>
        /// <returns>The entity.</returns>
        protected abstract TEntity? Get(Func<IDbCommand> commandActivator, TKey key);

        #endregion

        #region GetRange

        /// <inheritdoc/>
        public IEnumerable<TEntity> GetRange(IEnumerable<TKey> keys)
        {
            return Executor.ExecuteIteratorOnNewConnection(
                (this, keys),
                (x, commandActivator) => x.Item1.GetRange(commandActivator, x.keys)
                );
        }

        /// <summary>
        /// Gets the entity corresponding to the specified keys.
        /// </summary>
        /// <param name="connection">The curernt connection.</param>
        /// <param name="keys">The keys.</param>
        /// <returns>The entity.</returns>
        public IEnumerable<TEntity> GetRange(IDbConnection connection, IEnumerable<TKey> keys)
        {
            return Executor.ExecuteOnConnection(
                (this, keys),
                connection,
                (x, commandActivator) => x.Item1.GetRange(commandActivator, x.keys)
                );
        }

        /// <summary>
        /// Gets the entity corresponding to the specified keys.
        /// </summary>
        /// <param name="transaction">The curernt transaction.</param>
        /// <param name="keys">The keys.</param>
        /// <returns>The entity.</returns>
        public IEnumerable<TEntity> GetRange(IDbTransaction transaction, IEnumerable<TKey> keys)
        {
            return Executor.ExecuteOnTransaction(
                (this, keys),
                transaction,
                (x, commandActivator) => x.Item1.GetRange(commandActivator, x.keys)
                );
        }

        /// <summary>
        /// Gets the entity corresponding to the specified keys.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="keys">The keys.</param>
        /// <returns>The entity.</returns>
        protected abstract IEnumerable<TEntity> GetRange(Func<IDbCommand> commandActivator, IEnumerable<TKey> keys);

        #endregion

        #region GetAll

        /// <inheritdoc/>
        public IEnumerable<TEntity> GetAll()
        {
            return Executor.ExecuteIteratorOnNewConnection(
                this,
                (x, commandActivator) => x.GetAll(commandActivator)
                );
        }

        /// <summary>
        /// Gets all entities.
        /// </summary>
        /// <param name="connection">The curernt connection.</param>
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
        /// <param name="transaction">The curernt transaction.</param>
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

        #region GetAllKeys

        /// <inheritdoc/>
        public IEnumerable<TKey> GetAllKeys()
        {
            return Executor.ExecuteIteratorOnNewConnection(
                this,
                (x, commandActivator) => x.GetAllKeys(commandActivator)
                );
        }

        /// <summary>
        /// Gets all keys.
        /// </summary>
        /// <param name="connection">The curernt connection.</param>
        /// <returns>The keys.</returns>
        public IEnumerable<TKey> GetAllKeys(IDbConnection connection)
        {
            return Executor.ExecuteOnConnection(
                this,
                connection,
                (x, commandActivator) => x.GetAllKeys(commandActivator)
                );
        }

        /// <summary>
        /// Gets all keys.
        /// </summary>
        /// <param name="transaction">The curernt transaction.</param>
        /// <returns>The keys.</returns>
        public IEnumerable<TKey> GetAllKeys(IDbTransaction transaction)
        {
            return Executor.ExecuteOnTransaction(
                this,
                transaction,
                (x, commandActivator) => x.GetAllKeys(commandActivator)
                );
        }

        /// <summary>
        /// Gets all keys.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <returns>The keys.</returns>
        protected abstract IEnumerable<TKey> GetAllKeys(Func<IDbCommand> commandActivator);

        #endregion
    }
}

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
    /// <typeparam name="TKey">The key type.</typeparam>
    public abstract class DbAsyncReadRepositoryBase<TEntity, TKey> : DbReadRepositoryBase<TEntity, TKey>, IAsyncReadDataRepository<TEntity, TKey>
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="connectionActivator">The method to activate a connection.</param>
        /// <param name="useTransactionScope">A value that indicates whether to use ambient transactions using TransactionScope.</param>
        protected DbAsyncReadRepositoryBase(Func<IDbConnection> connectionActivator, bool useTransactionScope)
            : base(connectionActivator, useTransactionScope)
        {
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="connectionActivator">The method to activate a connection.</param>
        /// <param name="useTransactionScope">A value that indicates whether to use ambient transactions using TransactionScope.</param>
        /// <param name="configureCommand">A method to configure a command.</param>
        protected DbAsyncReadRepositoryBase(Func<IDbConnection> connectionActivator, bool useTransactionScope, Action<IDbCommand> configureCommand)
            : base(connectionActivator, useTransactionScope, configureCommand)
        {
        }

        #region GetAsync

        /// <inheritdoc/>
        public ValueTask<TEntity?> GetAsync(TKey key)
        {
            return Executor.ExecuteOnNewConnectionAsync(
                (this, key),
                (x, commandActivator) => x.Item1.GetAsync(commandActivator, x.key)
                );
        }

        /// <summary>
        /// Gets the entity corresponding to the specified key.
        /// </summary>
        /// <param name="connection">The curernt connection.</param>
        /// <param name="key">The key.</param>
        /// <returns>The entity.</returns>
        public ValueTask<TEntity?> GetAsync(IDbConnection connection, TKey key)
        {
            return Executor.ExecuteOnConnectionAsync(
                (this, key),
                connection,
                (x, commandActivator) => x.Item1.GetAsync(commandActivator, x.key)
                );
        }

        /// <summary>
        /// Gets the entity corresponding to the specified key.
        /// </summary>
        /// <param name="transaction">The curernt transaction.</param>
        /// <param name="key">The key.</param>
        /// <returns>The entity.</returns>
        public ValueTask<TEntity?> GetAsync(IDbTransaction transaction, TKey key)
        {
            return Executor.ExecuteOnTransactionAsync(
                (this, key),
                transaction,
                (x, commandActivator) => x.Item1.GetAsync(commandActivator, x.key)
                );
        }

        /// <summary>
        /// Gets the entity corresponding to the specified key.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="key">The key.</param>
        /// <returns>The entity.</returns>
        protected abstract ValueTask<TEntity?> GetAsync(Func<IDbCommand> commandActivator, TKey key);

        #endregion

        #region GetRangeAsync

        /// <inheritdoc/>
        public IAsyncEnumerable<TEntity> GetRangeAsync(IEnumerable<TKey> keys, CancellationToken cancellationToken = default)
        {
            return Executor.ExecuteIteratorOnNewConnectionAsync(
                (this, keys, cancellationToken),
                (x, commandActivator) => x.Item1.GetRangeAsync(commandActivator, x.keys, x.cancellationToken)
                );
        }

        /// <summary>
        /// Gets the entity corresponding to the specified keys.
        /// </summary>
        /// <param name="connection">The curernt connection.</param>
        /// <param name="keys">The keys.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entity.</returns>
        public IAsyncEnumerable<TEntity> GetRangeAsync(IDbConnection connection, IEnumerable<TKey> keys, CancellationToken cancellationToken = default)
        {
            return Executor.ExecuteIteratorOnConnectionAsync(
                (this, keys, cancellationToken),
                connection,
                (x, commandActivator) => x.Item1.GetRangeAsync(commandActivator, x.keys, x.cancellationToken)
                );
        }

        /// <summary>
        /// Gets the entity corresponding to the specified keys.
        /// </summary>
        /// <param name="transaction">The curernt transaction.</param>
        /// <param name="keys">The keys.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entity.</returns>
        public IAsyncEnumerable<TEntity> GetRangeAsync(IDbTransaction transaction, IEnumerable<TKey> keys, CancellationToken cancellationToken = default)
        {
            return Executor.ExecuteIteratorOnTransactionAsync(
                (this, keys, cancellationToken),
                transaction,
                (x, commandActivator) => x.Item1.GetRangeAsync(commandActivator, x.keys, x.cancellationToken)
                );
        }

        /// <summary>
        /// Gets the entity corresponding to the specified keys.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="keys">The keys.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entity.</returns>
        protected abstract IAsyncEnumerable<TEntity> GetRangeAsync(Func<IDbCommand> commandActivator, IEnumerable<TKey> keys, CancellationToken cancellationToken);

        /// <inheritdoc/>
        public IAsyncEnumerable<TEntity> GetRangeAsync(IAsyncEnumerable<TKey> keys, CancellationToken cancellationToken = default)
        {
            return Executor.ExecuteIteratorOnNewConnectionAsync(
                (this, keys, cancellationToken),
                (x, commandActivator) => x.Item1.GetRangeAsync(commandActivator, x.keys, x.cancellationToken)
                );
        }

        /// <summary>
        /// Gets the entity corresponding to the specified keys.
        /// </summary>
        /// <param name="connection">The curernt connection.</param>
        /// <param name="keys">The keys.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entity.</returns>
        public IAsyncEnumerable<TEntity> GetRangeAsync(IDbConnection connection, IAsyncEnumerable<TKey> keys, CancellationToken cancellationToken = default)
        {
            return Executor.ExecuteIteratorOnConnectionAsync(
                (this, keys, cancellationToken),
                connection,
                (x, commandActivator) => x.Item1.GetRangeAsync(commandActivator, x.keys, x.cancellationToken)
                );
        }

        /// <summary>
        /// Gets the entity corresponding to the specified keys.
        /// </summary>
        /// <param name="transaction">The curernt transaction.</param>
        /// <param name="keys">The keys.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entity.</returns>
        public IAsyncEnumerable<TEntity> GetRangeAsync(IDbTransaction transaction, IAsyncEnumerable<TKey> keys, CancellationToken cancellationToken = default)
        {
            return Executor.ExecuteIteratorOnTransactionAsync(
                (this, keys, cancellationToken),
                transaction,
                (x, commandActivator) => x.Item1.GetRangeAsync(commandActivator, x.keys, x.cancellationToken)
                );
        }

        /// <summary>
        /// Gets the entity corresponding to the specified keys.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="keys">The keys.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entity.</returns>
        protected abstract IAsyncEnumerable<TEntity> GetRangeAsync(Func<IDbCommand> commandActivator, IAsyncEnumerable<TKey> keys, CancellationToken cancellationToken);

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
        /// <param name="connection">The curernt connection.</param>
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
        /// <param name="transaction">The curernt transaction.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entities.</returns>
        public IAsyncEnumerable<TEntity> GetAllAsync(IDbTransaction transaction, CancellationToken cancellationToken = default)
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

        #region GetAllKeysAsync

        /// <inheritdoc/>
        public IAsyncEnumerable<TKey> GetAllKeysAsync(CancellationToken cancellationToken = default)
        {
            return Executor.ExecuteIteratorOnNewConnectionAsync(
                (this, cancellationToken),
                (x, commandActivator) => x.Item1.GetAllKeysAsync(commandActivator, x.cancellationToken)
                );
        }

        /// <summary>
        /// Gets all keys.
        /// </summary>
        /// <param name="connection">The curernt connection.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The keys.</returns>
        public IAsyncEnumerable<TKey> GetAllKeysAsync(IDbConnection connection, CancellationToken cancellationToken = default)
        {
            return Executor.ExecuteIteratorOnConnectionAsync(
                (this, cancellationToken),
                connection,
                (x, commandActivator) => x.Item1.GetAllKeysAsync(commandActivator, x.cancellationToken)
                );
        }

        /// <summary>
        /// Gets all keys.
        /// </summary>
        /// <param name="transaction">The curernt transaction.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The keys.</returns>
        public IAsyncEnumerable<TKey> GetAllKeysAsync(IDbTransaction transaction, CancellationToken cancellationToken = default)
        {
            return Executor.ExecuteIteratorOnTransactionAsync(
                (this, cancellationToken),
                transaction,
                (x, commandActivator) => x.Item1.GetAllKeysAsync(commandActivator, x.cancellationToken)
                );
        }

        /// <summary>
        /// Gets all keys.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The keys.</returns>
        protected abstract IAsyncEnumerable<TKey> GetAllKeysAsync(Func<IDbCommand> commandActivator, CancellationToken cancellationToken);

        #endregion
    }
}

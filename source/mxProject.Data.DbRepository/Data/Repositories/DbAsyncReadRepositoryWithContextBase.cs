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
    /// <typeparam name="TContext">The context type.</typeparam>
    public abstract class DbAsyncReadRepositoryWithContextBase<TEntity, TKey, TContext> : DbReadRepositoryWithContextBase<TEntity, TKey, TContext>, IAsyncReadDataRepositoryWithContext<TEntity, TKey, TContext>
        where TContext : IDataRepositoryContext
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="connectionActivator">The method to activate a connection.</param>
        /// <param name="useTransactionScope">A value that indicates whether to use ambient transactions using TransactionScope.</param>
        protected DbAsyncReadRepositoryWithContextBase(Func<IDbConnection> connectionActivator, bool useTransactionScope)
            : base(connectionActivator, useTransactionScope)
        {
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="connectionActivator">The method to activate a connection.</param>
        /// <param name="useTransactionScope">A value that indicates whether to use ambient transactions using TransactionScope.</param>
        /// <param name="configureCommand">A method to configure a command.</param>
        protected DbAsyncReadRepositoryWithContextBase(Func<IDbConnection> connectionActivator, bool useTransactionScope, Action<IDbCommand> configureCommand)
            : base(connectionActivator, useTransactionScope, configureCommand)
        {
        }

        #region GetAsync

        /// <inheritdoc/>
        public ValueTask<TEntity?> GetAsync(TKey key, TContext context)
        {
            if (TryGetCurrentTransaction(context, out var transaction))
            {
                return GetAsync(transaction, key, context);
            }
            else if (TryGetCurrentConnection(context, out var connection))
            {
                return GetAsync(connection, key, context);
            }
            else
            {
                return Executor.ExecuteOnNewConnectionAsync(
                    (this, key),
                    (x, commandActivator, context) => x.Item1.GetAsync(commandActivator, x.key, context),
                    context
                    );
            }
        }

        /// <summary>
        /// Gets the entity corresponding to the specified key.
        /// </summary>
        /// <param name="connection">The curernt connection.</param>
        /// <param name="key">The key.</param>
        /// <param name="context">The current context.</param>
        /// <returns>The entity.</returns>
        public ValueTask<TEntity?> GetAsync(IDbConnection connection, TKey key, TContext context)
        {
            return Executor.ExecuteOnConnectionAsync(
                (this, key),
                connection,
                (x, commandActivator, context) => x.Item1.GetAsync(commandActivator, x.key, context),
                context
                );
        }

        /// <summary>
        /// Gets the entity corresponding to the specified key.
        /// </summary>
        /// <param name="transaction">The curernt transaction.</param>
        /// <param name="key">The key.</param>
        /// <param name="context">The current context.</param>
        /// <returns>The entity.</returns>
        public ValueTask<TEntity?> GetAsync(IDbTransaction transaction, TKey key, TContext context)
        {
            return Executor.ExecuteOnTransactionAsync(
                (this, key),
                transaction,
                (x, commandActivator, context) => x.Item1.GetAsync(commandActivator, x.key, context),
                context
                );
        }

        /// <summary>
        /// Gets the entity corresponding to the specified key.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="key">The key.</param>
        /// <param name="context">The current context.</param>
        /// <returns>The entity.</returns>
        protected abstract ValueTask<TEntity?> GetAsync(Func<IDbCommand> commandActivator, TKey key, TContext context);

        #endregion

        #region GetRangeAsync

        /// <inheritdoc/>
        public IAsyncEnumerable<TEntity> GetRangeAsync(IEnumerable<TKey> keys, TContext context, CancellationToken cancellationToken = default)
        {
            if (TryGetCurrentTransaction(context, out var transaction))
            {
                return GetRangeAsync(transaction, keys, context, cancellationToken);
            }
            else if (TryGetCurrentConnection(context, out var connection))
            {
                return GetRangeAsync(connection, keys, context, cancellationToken);
            }
            else
            {
                return Executor.ExecuteIteratorOnNewConnectionAsync(
                    (this, keys, cancellationToken),
                    (x, commandActivator, context) => x.Item1.GetRangeAsync(commandActivator, x.keys, context, x.cancellationToken),
                    context
                    );
            }
        }

        /// <summary>
        /// Gets the entity corresponding to the specified keys.
        /// </summary>
        /// <param name="connection">The curernt connection.</param>
        /// <param name="keys">The keys.</param>
        /// <param name="context">The current context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entity.</returns>
        public IAsyncEnumerable<TEntity> GetRangeAsync(IDbConnection connection, IEnumerable<TKey> keys, TContext context, CancellationToken cancellationToken = default)
        {
            return Executor.ExecuteIteratorOnConnectionAsync(
                (this, keys, cancellationToken),
                connection,
                (x, commandActivator, context) => x.Item1.GetRangeAsync(commandActivator, x.keys, context, x.cancellationToken),
                context
                );
        }

        /// <summary>
        /// Gets the entity corresponding to the specified keys.
        /// </summary>
        /// <param name="transaction">The curernt transaction.</param>
        /// <param name="keys">The keys.</param>
        /// <param name="context">The current context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entity.</returns>
        public IAsyncEnumerable<TEntity> GetRangeAsync(IDbTransaction transaction, IEnumerable<TKey> keys, TContext context, CancellationToken cancellationToken = default)
        {
            return Executor.ExecuteIteratorOnTransactionAsync(
                (this, keys, cancellationToken),
                transaction,
                (x, commandActivator, context) => x.Item1.GetRangeAsync(commandActivator, x.keys, context, x.cancellationToken),
                context
                );
        }

        /// <summary>
        /// Gets the entity corresponding to the specified keys.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="keys">The keys.</param>
        /// <param name="context">The current context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entity.</returns>
        protected abstract IAsyncEnumerable<TEntity> GetRangeAsync(Func<IDbCommand> commandActivator, IEnumerable<TKey> keys, TContext context, CancellationToken cancellationToken);

        /// <inheritdoc/>
        public IAsyncEnumerable<TEntity> GetRangeAsync(IAsyncEnumerable<TKey> keys, TContext context, CancellationToken cancellationToken = default)
        {
            if (TryGetCurrentTransaction(context, out var transaction))
            {
                return GetRangeAsync(transaction, keys, context, cancellationToken);
            }
            else if (TryGetCurrentConnection(context, out var connection))
            {
                return GetRangeAsync(connection, keys, context, cancellationToken);
            }
            else
            {
                return Executor.ExecuteIteratorOnNewConnectionAsync(
                    (this, keys, cancellationToken),
                    (x, commandActivator, context) => x.Item1.GetRangeAsync(commandActivator, x.keys, context, x.cancellationToken),
                    context
                    );
            }
        }

        /// <summary>
        /// Gets the entity corresponding to the specified keys.
        /// </summary>
        /// <param name="connection">The curernt connection.</param>
        /// <param name="keys">The keys.</param>
        /// <param name="context">The current context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entity.</returns>
        public IAsyncEnumerable<TEntity> GetRangeAsync(IDbConnection connection, IAsyncEnumerable<TKey> keys, TContext context, CancellationToken cancellationToken = default)
        {
            return Executor.ExecuteIteratorOnConnectionAsync(
                (this, keys, cancellationToken),
                connection,
                (x, commandActivator, context) => x.Item1.GetRangeAsync(commandActivator, x.keys, context, x.cancellationToken),
                context
                );
        }

        /// <summary>
        /// Gets the entity corresponding to the specified keys.
        /// </summary>
        /// <param name="transaction">The curernt transaction.</param>
        /// <param name="keys">The keys.</param>
        /// <param name="context">The current context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entity.</returns>
        public IAsyncEnumerable<TEntity> GetRangeAsync(IDbTransaction transaction, IAsyncEnumerable<TKey> keys, TContext context, CancellationToken cancellationToken = default)
        {
            return Executor.ExecuteIteratorOnTransactionAsync(
                (this, keys, cancellationToken),
                transaction,
                (x, commandActivator, context) => x.Item1.GetRangeAsync(commandActivator, x.keys, context, x.cancellationToken),
                context
                );
        }

        /// <summary>
        /// Gets the entity corresponding to the specified keys.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="keys">The keys.</param>
        /// <param name="context">The current context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entity.</returns>
        protected abstract IAsyncEnumerable<TEntity> GetRangeAsync(Func<IDbCommand> commandActivator, IAsyncEnumerable<TKey> keys, TContext context, CancellationToken cancellationToken);

        #endregion

        #region GetAllAsync

        /// <inheritdoc/>
        public IAsyncEnumerable<TEntity> GetAllAsync(TContext context, CancellationToken cancellationToken = default)
        {
            if (TryGetCurrentTransaction(context, out var transaction))
            {
                return GetAllAsync(transaction, context, cancellationToken);
            }
            else if (TryGetCurrentConnection(context, out var connection))
            {
                return GetAllAsync(connection, context, cancellationToken);
            }
            else
            {
                return Executor.ExecuteIteratorOnNewConnectionAsync(
                    (this, cancellationToken),
                    (x, commandActivator, context) => x.Item1.GetAllAsync(commandActivator, context, x.cancellationToken),
                    context
                    );
            }
        }

        /// <summary>
        /// Gets all entities.
        /// </summary>
        /// <param name="connection">The curernt connection.</param>
        /// <param name="context">The current context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entities.</returns>
        public IAsyncEnumerable<TEntity> GetAllAsync(IDbConnection connection, TContext context, CancellationToken cancellationToken = default)
        {
            return Executor.ExecuteIteratorOnConnectionAsync(
                (this, cancellationToken),
                connection,
                (x, commandActivator, context) => x.Item1.GetAllAsync(commandActivator, context, x.cancellationToken),
                context
                );
        }

        /// <summary>
        /// Gets all entities.
        /// </summary>
        /// <param name="transaction">The curernt transaction.</param>
        /// <param name="context">The current context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entities.</returns>
        public IAsyncEnumerable<TEntity> GetAllAsync(IDbTransaction transaction, TContext context, CancellationToken cancellationToken = default)
        {
            return Executor.ExecuteIteratorOnTransactionAsync(
                (this, cancellationToken),
                transaction,
                (x, commandActivator, context) => x.Item1.GetAllAsync(commandActivator, context, x.cancellationToken),
                context
                );
        }

        /// <summary>
        /// Gets all entities.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="context">The current context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entities.</returns>
        protected abstract IAsyncEnumerable<TEntity> GetAllAsync(Func<IDbCommand> commandActivator, TContext context, CancellationToken cancellationToken);

        #endregion

        #region GetAllKeysAsync

        /// <inheritdoc/>
        public IAsyncEnumerable<TKey> GetAllKeysAsync(TContext context, CancellationToken cancellationToken = default)
        {
            if (TryGetCurrentTransaction(context, out var transaction))
            {
                return GetAllKeysAsync(transaction, context, cancellationToken);
            }
            else if (TryGetCurrentConnection(context, out var connection))
            {
                return GetAllKeysAsync(connection, context, cancellationToken);
            }
            else
            {
                return Executor.ExecuteIteratorOnNewConnectionAsync(
                    (this, cancellationToken),
                    (x, commandActivator, context) => x.Item1.GetAllKeysAsync(commandActivator, context, x.cancellationToken),
                    context
                    );
            }
        }

        /// <summary>
        /// Gets all keys.
        /// </summary>
        /// <param name="connection">The curernt connection.</param>
        /// <param name="context">The current context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The keys.</returns>
        public IAsyncEnumerable<TKey> GetAllKeysAsync(IDbConnection connection, TContext context, CancellationToken cancellationToken = default)
        {
            return Executor.ExecuteIteratorOnConnectionAsync(
                (this, cancellationToken),
                connection,
                (x, commandActivator, context) => x.Item1.GetAllKeysAsync(commandActivator, context, x.cancellationToken),
                context
                );
        }

        /// <summary>
        /// Gets all keys.
        /// </summary>
        /// <param name="transaction">The curernt transaction.</param>
        /// <param name="context">The current context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The keys.</returns>
        public IAsyncEnumerable<TKey> GetAllKeysAsync(IDbTransaction transaction, TContext context, CancellationToken cancellationToken = default)
        {
            return Executor.ExecuteIteratorOnTransactionAsync(
                (this, cancellationToken),
                transaction,
                (x, commandActivator, context) => x.Item1.GetAllKeysAsync(commandActivator, context, x.cancellationToken),
                context
                );
        }

        /// <summary>
        /// Gets all keys.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="context">The current context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The keys.</returns>
        protected abstract IAsyncEnumerable<TKey> GetAllKeysAsync(Func<IDbCommand> commandActivator, TContext context, CancellationToken cancellationToken);

        #endregion
    }
}

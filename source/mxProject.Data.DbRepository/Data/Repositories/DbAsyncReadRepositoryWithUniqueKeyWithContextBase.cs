using System;
using System.Collections.Generic;
using System.Data;
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
    /// <typeparam name="TContext">The context type.</typeparam>
    public abstract class DbAsyncReadRepositoryWithUniqueKeyWithContextBase<TEntity, TPrimaryKey, TUniqueKey, TContext> : DbReadRepositoryWithUniqueKeyWithContextBase<TEntity, TPrimaryKey, TUniqueKey, TContext>, IAsyncReadDataRepositoryWithUniqueKeyWithContext<TEntity, TPrimaryKey, TUniqueKey, TContext>
        where TContext : IDataRepositoryContext
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="connectionActivator">The method to activate a connection.</param>
        /// <param name="useTransactionScope">A value that indicates whether to use ambient transactions using TransactionScope.</param>
        protected DbAsyncReadRepositoryWithUniqueKeyWithContextBase(Func<IDbConnection> connectionActivator, bool useTransactionScope)
            : base(connectionActivator, useTransactionScope)
        {
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="connectionActivator">The method to activate a connection.</param>
        /// <param name="useTransactionScope">A value that indicates whether to use ambient transactions using TransactionScope.</param>
        /// <param name="configureCommand">A method to configure a command.</param>
        protected DbAsyncReadRepositoryWithUniqueKeyWithContextBase(Func<IDbConnection> connectionActivator, bool useTransactionScope, Action<IDbCommand> configureCommand)
            : base(connectionActivator, useTransactionScope, configureCommand)
        {
        }

        #region GetByPrimaryKeyAsync

        /// <inheritdoc/>
        public ValueTask<TEntity?> GetByPrimaryKeyAsync(TPrimaryKey primaryKey, TContext context)
        {
            if (TryGetCurrentTransaction(context, out var transaction))
            {
                return GetByPrimaryKeyAsync(transaction, primaryKey, context);
            }
            else if (TryGetCurrentConnection(context, out var connection))
            {
                return GetByPrimaryKeyAsync(connection, primaryKey, context);
            }
            else
            {
                return Executor.ExecuteOnNewConnectionAsync(
                    (this, primaryKey),
                    (x, commandActivator, context) => x.Item1.GetByPrimaryKeyAsync(commandActivator, x.primaryKey, context),
                    context
                    );
            }
        }

        /// <summary>
        /// Gets the entity corresponding to the specified primary key.
        /// </summary>
        /// <param name="connection">The current connection.</param>
        /// <param name="primaryKey">The primary key.</param>
        /// <param name="context">The current context.</param>
        /// <returns>The entity.</returns>
        public ValueTask<TEntity?> GetByPrimaryKeyAsync(IDbConnection connection, TPrimaryKey primaryKey, TContext context)
        {
            return Executor.ExecuteOnConnectionAsync(
                (this, primaryKey),
                connection,
                (x, commandActivator, context) => x.Item1.GetByPrimaryKeyAsync(commandActivator, x.primaryKey, context),
                context
                );
        }

        /// <summary>
        /// Gets the entity corresponding to the specified primary key.
        /// </summary>
        /// <param name="transaction">The current transaction.</param>
        /// <param name="primaryKey">The primary key.</param>
        /// <param name="context">The current context.</param>
        /// <returns>The entity.</returns>
        public ValueTask<TEntity?> GetByPrimaryKeyAsync(IDbTransaction transaction, TPrimaryKey primaryKey, TContext context)
        {
            return Executor.ExecuteOnTransactionAsync(
                (this, primaryKey),
                transaction,
                (x, commandActivator, context) => x.Item1.GetByPrimaryKeyAsync(commandActivator, x.primaryKey, context),
                context
                );
        }

        /// <summary>
        /// Gets the entity corresponding to the specified primary key.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="primaryKey">The primary key.</param>
        /// <param name="context">The current context.</param>
        /// <returns>The entity.</returns>
        protected abstract ValueTask<TEntity?> GetByPrimaryKeyAsync(Func<IDbCommand> commandActivator, TPrimaryKey primaryKey, TContext context);

        #endregion

        #region GetByUniqueKeyAsync

        /// <inheritdoc/>
        public ValueTask<TEntity?> GetByUniqueKeyAsync(TUniqueKey uniqueKey, TContext context)
        {
            if (TryGetCurrentTransaction(context, out var transaction))
            {
                return GetByUniqueKeyAsync(transaction, uniqueKey, context);
            }
            else if (TryGetCurrentConnection(context, out var connection))
            {
                return GetByUniqueKeyAsync(connection, uniqueKey, context);
            }
            else
            {
                return Executor.ExecuteOnNewConnectionAsync(
                    (this, uniqueKey),
                    (x, commandActivator, context) => x.Item1.GetByUniqueKeyAsync(commandActivator, x.uniqueKey, context),
                    context
                    );
            }
        }

        /// <summary>
        /// Gets the entity corresponding to the specified unique key.
        /// </summary>
        /// <param name="connection">The current connection.</param>
        /// <param name="uniqueKey">The unique key.</param>
        /// <param name="context">The current context.</param>
        /// <returns>The entity.</returns>
        public ValueTask<TEntity?> GetByUniqueKeyAsync(IDbConnection connection, TUniqueKey uniqueKey, TContext context)
        {
            return Executor.ExecuteOnConnectionAsync(
                (this, uniqueKey),
                connection,
                (x, commandActivator, context) => x.Item1.GetByUniqueKeyAsync(commandActivator, x.uniqueKey, context),
                context
                );
        }

        /// <summary>
        /// Gets the entity corresponding to the specified unique key.
        /// </summary>
        /// <param name="transaction">The current transaction.</param>
        /// <param name="uniqueKey">The unique key.</param>
        /// <param name="context">The current context.</param>
        /// <returns>The entity.</returns>
        public ValueTask<TEntity?> GetByUniqueKeyAsync(IDbTransaction transaction, TUniqueKey uniqueKey, TContext context)
        {
            return Executor.ExecuteOnTransactionAsync(
                (this, uniqueKey),
                transaction,
                (x, commandActivator, context) => x.Item1.GetByUniqueKeyAsync(commandActivator, x.uniqueKey, context),
                context
                );
        }

        /// <summary>
        /// Gets the entity corresponding to the specified unique key.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="uniqueKey">The unique key.</param>
        /// <param name="context">The current context.</param>
        /// <returns>The entity.</returns>
        protected abstract ValueTask<TEntity?> GetByUniqueKeyAsync(Func<IDbCommand> commandActivator, TUniqueKey uniqueKey, TContext context);

        #endregion

        #region GetRangeByPrimaryKeyAsync

        /// <inheritdoc/>
        public IAsyncEnumerable<TEntity> GetRangeByPrimaryKeyAsync(IEnumerable<TPrimaryKey> primaryKeys, TContext context, CancellationToken cancellationToken = default)
        {
            if (TryGetCurrentTransaction(context, out var transaction))
            {
                return GetRangeByPrimaryKeyAsync(transaction, primaryKeys, context, cancellationToken);
            }
            else if (TryGetCurrentConnection(context, out var connection))
            {
                return GetRangeByPrimaryKeyAsync(connection, primaryKeys, context, cancellationToken);
            }
            else
            {
                return Executor.ExecuteIteratorOnNewConnectionAsync(
                    (this, primaryKeys, cancellationToken),
                    (x, commandActivator, context) => x.Item1.GetRangeByPrimaryKeyAsync(commandActivator, x.primaryKeys, context, x.cancellationToken),
                    context
                    );
            }
        }

        /// <summary>
        /// Gets the entity corresponding to the specified primary keys.
        /// </summary>
        /// <param name="connection">The current connection.</param>
        /// <param name="primaryKeys">The primary keys.</param>
        /// <param name="context">The current context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entities.</returns>
        public IAsyncEnumerable<TEntity> GetRangeByPrimaryKeyAsync(IDbConnection connection, IEnumerable<TPrimaryKey> primaryKeys, TContext context, CancellationToken cancellationToken = default)
        {
            return Executor.ExecuteIteratorOnConnectionAsync(
                (this, primaryKeys, cancellationToken),
                connection,
                (x, commandActivator, context) => x.Item1.GetRangeByPrimaryKeyAsync(commandActivator, x.primaryKeys, context, x.cancellationToken),
                context
                );
        }

        /// <summary>
        /// Gets the entity corresponding to the specified primary keys.
        /// </summary>
        /// <param name="transaction">The current transaction.</param>
        /// <param name="primaryKeys">The primary keys.</param>
        /// <param name="context">The current context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entities.</returns>
        public IAsyncEnumerable<TEntity> GetRangeByPrimaryKeyAsync(IDbTransaction transaction, IEnumerable<TPrimaryKey> primaryKeys, TContext context, CancellationToken cancellationToken = default)
        {
            return Executor.ExecuteIteratorOnTransactionAsync(
                (this, primaryKeys, cancellationToken),
                transaction,
                (x, commandActivator, context) => x.Item1.GetRangeByPrimaryKeyAsync(commandActivator, x.primaryKeys, context, x.cancellationToken),
                context
                );
        }

        /// <summary>
        /// Gets the entity corresponding to the specified primary keys.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="primaryKeys">The primary keys.</param>
        /// <param name="context">The current context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entities.</returns>
        protected abstract IAsyncEnumerable<TEntity> GetRangeByPrimaryKeyAsync(Func<IDbCommand> commandActivator, IEnumerable<TPrimaryKey> primaryKeys, TContext context, CancellationToken cancellationToken);

        /// <inheritdoc/>
        public IAsyncEnumerable<TEntity> GetRangeByPrimaryKeyAsync(IAsyncEnumerable<TPrimaryKey> primaryKeys, TContext context, CancellationToken cancellationToken = default)
        {
            if (TryGetCurrentTransaction(context, out var transaction))
            {
                return GetRangeByPrimaryKeyAsync(transaction, primaryKeys, context, cancellationToken);
            }
            else if (TryGetCurrentConnection(context, out var connection))
            {
                return GetRangeByPrimaryKeyAsync(connection, primaryKeys, context, cancellationToken);
            }
            else
            {
                return Executor.ExecuteIteratorOnNewConnectionAsync(
                    (this, primaryKeys, cancellationToken),
                    (x, commandActivator, context) => x.Item1.GetRangeByPrimaryKeyAsync(commandActivator, x.primaryKeys, context, x.cancellationToken),
                    context
                    );
            }
        }

        /// <summary>
        /// Gets the entity corresponding to the specified primary keys.
        /// </summary>
        /// <param name="connection">The current connection.</param>
        /// <param name="primaryKeys">The primary keys.</param>
        /// <param name="context">The current context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entities.</returns>
        public IAsyncEnumerable<TEntity> GetRangeByPrimaryKeyAsync(IDbConnection connection, IAsyncEnumerable<TPrimaryKey> primaryKeys, TContext context, CancellationToken cancellationToken = default)
        {
            return Executor.ExecuteIteratorOnConnectionAsync(
                (this, primaryKeys, cancellationToken),
                connection,
                (x, commandActivator, context) => x.Item1.GetRangeByPrimaryKeyAsync(commandActivator, x.primaryKeys, context, x.cancellationToken),
                context
                );
        }

        /// <summary>
        /// Gets the entity corresponding to the specified primary keys.
        /// </summary>
        /// <param name="transaction">The current transaction.</param>
        /// <param name="primaryKeys">The primary keys.</param>
        /// <param name="context">The current context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entities.</returns>
        public IAsyncEnumerable<TEntity> GetRangeByPrimaryKeyAsync(IDbTransaction transaction, IAsyncEnumerable<TPrimaryKey> primaryKeys, TContext context, CancellationToken cancellationToken = default)
        {
            return Executor.ExecuteIteratorOnTransactionAsync(
                (this, primaryKeys, cancellationToken),
                transaction,
                (x, commandActivator, context) => x.Item1.GetRangeByPrimaryKeyAsync(commandActivator, x.primaryKeys, context, x.cancellationToken),
                context
                );
        }

        /// <summary>
        /// Gets the entity corresponding to the specified primary keys.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="primaryKeys">The primary keys.</param>
        /// <param name="context">The current context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entities.</returns>
        protected abstract IAsyncEnumerable<TEntity> GetRangeByPrimaryKeyAsync(Func<IDbCommand> commandActivator, IAsyncEnumerable<TPrimaryKey> primaryKeys, TContext context, CancellationToken cancellationToken);

        #endregion

        #region GetRangeByUniqueKeyAsync

        /// <inheritdoc/>
        public IAsyncEnumerable<TEntity> GetRangeByUniqueKeyAsync(IEnumerable<TUniqueKey> uniqueKeys, TContext context, CancellationToken cancellationToken = default)
        {
            if (TryGetCurrentTransaction(context, out var transaction))
            {
                return GetRangeByUniqueKeyAsync(transaction, uniqueKeys, context, cancellationToken);
            }
            else if (TryGetCurrentConnection(context, out var connection))
            {
                return GetRangeByUniqueKeyAsync(connection, uniqueKeys, context, cancellationToken);
            }
            else
            {
                return Executor.ExecuteIteratorOnNewConnectionAsync(
                    (this, uniqueKeys, cancellationToken),
                    (x, commandActivator, context) => x.Item1.GetRangeByUniqueKeyAsync(commandActivator, x.uniqueKeys, context, x.cancellationToken),
                    context
                    );
            }
        }

        /// <summary>
        /// Gets the entity corresponding to the specified unique keys.
        /// </summary>
        /// <param name="connection">The current connection.</param>
        /// <param name="uniqueKeys">The unique keys.</param>
        /// <param name="context">The current context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entities.</returns>
        public IAsyncEnumerable<TEntity> GetRangeByUniqueKeyAsync(IDbConnection connection, IEnumerable<TUniqueKey> uniqueKeys, TContext context, CancellationToken cancellationToken = default)
        {
            return Executor.ExecuteIteratorOnConnectionAsync(
                (this, uniqueKeys, cancellationToken),
                connection,
                (x, commandActivator, context) => x.Item1.GetRangeByUniqueKeyAsync(commandActivator, x.uniqueKeys, context, x.cancellationToken),
                context
                );
        }

        /// <summary>
        /// Gets the entity corresponding to the specified unique keys.
        /// </summary>
        /// <param name="transaction">The current transaction.</param>
        /// <param name="uniqueKeys">The unique keys.</param>
        /// <param name="context">The current context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entities.</returns>
        public IAsyncEnumerable<TEntity> GetRangeByUniqueKeyAsync(IDbTransaction transaction, IEnumerable<TUniqueKey> uniqueKeys, TContext context, CancellationToken cancellationToken = default)
        {
            return Executor.ExecuteIteratorOnTransactionAsync(
                (this, uniqueKeys, cancellationToken),
                transaction,
                (x, commandActivator, context) => x.Item1.GetRangeByUniqueKeyAsync(commandActivator, x.uniqueKeys, context, x.cancellationToken),
                context
                );
        }

        /// <summary>
        /// Gets the entity corresponding to the specified unique keys.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="uniqueKeys">The unique keys.</param>
        /// <param name="context">The current context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entities.</returns>
        protected abstract IAsyncEnumerable<TEntity> GetRangeByUniqueKeyAsync(Func<IDbCommand> commandActivator, IEnumerable<TUniqueKey> uniqueKeys, TContext context, CancellationToken cancellationToken);

        /// <inheritdoc/>
        public IAsyncEnumerable<TEntity> GetRangeByUniqueKeyAsync(IAsyncEnumerable<TUniqueKey> uniqueKeys, TContext context, CancellationToken cancellationToken = default)
        {
            if (TryGetCurrentTransaction(context, out var transaction))
            {
                return GetRangeByUniqueKeyAsync(transaction, uniqueKeys, context, cancellationToken);
            }
            else if (TryGetCurrentConnection(context, out var connection))
            {
                return GetRangeByUniqueKeyAsync(connection, uniqueKeys, context, cancellationToken);
            }
            else
            {
                return Executor.ExecuteIteratorOnNewConnectionAsync(
                    (this, uniqueKeys, cancellationToken),
                    (x, commandActivator, context) => x.Item1.GetRangeByUniqueKeyAsync(commandActivator, x.uniqueKeys, context, x.cancellationToken),
                    context
                    );
            }
        }

        /// <summary>
        /// Gets the entity corresponding to the specified unique keys.
        /// </summary>
        /// <param name="connection">The current connection.</param>
        /// <param name="uniqueKeys">The unique keys.</param>
        /// <param name="context">The current context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entities.</returns>
        public IAsyncEnumerable<TEntity> GetRangeByUniqueKeyAsync(IDbConnection connection, IAsyncEnumerable<TUniqueKey> uniqueKeys, TContext context, CancellationToken cancellationToken = default)
        {
            return Executor.ExecuteIteratorOnConnectionAsync(
                (this, uniqueKeys, cancellationToken),
                connection,
                (x, commandActivator, context) => x.Item1.GetRangeByUniqueKeyAsync(commandActivator, x.uniqueKeys, context, x.cancellationToken),
                context
                );
        }

        /// <summary>
        /// Gets the entity corresponding to the specified unique keys.
        /// </summary>
        /// <param name="transaction">The current transaction.</param>
        /// <param name="uniqueKeys">The unique keys.</param>
        /// <param name="context">The current context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entities.</returns>
        public IAsyncEnumerable<TEntity> GetRangeByUniqueKeyAsync(IDbTransaction transaction, IAsyncEnumerable<TUniqueKey> uniqueKeys, TContext context, CancellationToken cancellationToken = default)
        {
            return Executor.ExecuteIteratorOnTransactionAsync(
                (this, uniqueKeys, cancellationToken),
                transaction,
                (x, commandActivator, context) => x.Item1.GetRangeByUniqueKeyAsync(commandActivator, x.uniqueKeys, context, x.cancellationToken),
                context
                );
        }

        /// <summary>
        /// Gets the entity corresponding to the specified unique keys.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="uniqueKeys">The unique keys.</param>
        /// <param name="context">The current context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entities.</returns>
        protected abstract IAsyncEnumerable<TEntity> GetRangeByUniqueKeyAsync(Func<IDbCommand> commandActivator, IAsyncEnumerable<TUniqueKey> uniqueKeys, TContext context, CancellationToken cancellationToken);

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
        /// <param name="connection">The current connection.</param>
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
        /// <param name="transaction">The current transaction.</param>
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

        #region GetAllPrimaryKeysAsync

        /// <inheritdoc/>
        public IAsyncEnumerable<TPrimaryKey> GetAllPrimaryKeysAsync(TContext context, CancellationToken cancellationToken = default)
        {
            if (TryGetCurrentTransaction(context, out var transaction))
            {
                return GetAllPrimaryKeysAsync(transaction, context, cancellationToken);
            }
            else if (TryGetCurrentConnection(context, out var connection))
            {
                return GetAllPrimaryKeysAsync(connection, context, cancellationToken);
            }
            else
            {
                return Executor.ExecuteIteratorOnNewConnectionAsync(
                    (this, cancellationToken),
                    (x, commandActivator, context) => x.Item1.GetAllPrimaryKeysAsync(commandActivator, context, x.cancellationToken),
                    context
                    );
            }
        }

        /// <summary>
        /// Gets all primary keys.
        /// </summary>
        /// <param name="connection">The curernt connection.</param>
        /// <param name="context">The current context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The primary keys.</returns>
        public IAsyncEnumerable<TPrimaryKey> GetAllPrimaryKeysAsync(IDbConnection connection, TContext context, CancellationToken cancellationToken = default)
        {
            return Executor.ExecuteIteratorOnConnectionAsync(
                (this, cancellationToken),
                connection,
                (x, commandActivator, context) => x.Item1.GetAllPrimaryKeysAsync(commandActivator, context, x.cancellationToken),
                context
                );
        }

        /// <summary>
        /// Gets all primary keys.
        /// </summary>
        /// <param name="transaction">The curernt transaction.</param>
        /// <param name="context">The current context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The primary keys.</returns>
        public IAsyncEnumerable<TPrimaryKey> GetAllPrimaryKeysAsync(IDbTransaction transaction, TContext context, CancellationToken cancellationToken = default)
        {
            return Executor.ExecuteIteratorOnTransactionAsync(
                (this, cancellationToken),
                transaction,
                (x, commandActivator, context) => x.Item1.GetAllPrimaryKeysAsync(commandActivator, context, x.cancellationToken),
                context
                );
        }

        /// <summary>
        /// Gets all primary keys.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="context">The current context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The primary keys.</returns>
        protected abstract IAsyncEnumerable<TPrimaryKey> GetAllPrimaryKeysAsync(Func<IDbCommand> commandActivator, TContext context, CancellationToken cancellationToken);

        #endregion

        #region GetAllUniqueKeysAsync

        /// <inheritdoc/>
        public IAsyncEnumerable<TUniqueKey> GetAllUniqueKeysAsync(TContext context, CancellationToken cancellationToken = default)
        {
            if (TryGetCurrentTransaction(context, out var transaction))
            {
                return GetAllUniqueKeysAsync(transaction, context, cancellationToken);
            }
            else if (TryGetCurrentConnection(context, out var connection))
            {
                return GetAllUniqueKeysAsync(connection, context, cancellationToken);
            }
            else
            {
                return Executor.ExecuteIteratorOnNewConnectionAsync(
                    (this, cancellationToken),
                    (x, commandActivator, context) => x.Item1.GetAllUniqueKeysAsync(commandActivator, context, x.cancellationToken),
                    context
                    );
            }
        }

        /// <summary>
        /// Gets all unique keys.
        /// </summary>
        /// <param name="connection">The curernt connection.</param>
        /// <param name="context">The current context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The unique keys.</returns>
        public IAsyncEnumerable<TUniqueKey> GetAllUniqueKeysAsync(IDbConnection connection, TContext context, CancellationToken cancellationToken = default)
        {
            return Executor.ExecuteIteratorOnConnectionAsync(
                (this, cancellationToken),
                connection,
                (x, commandActivator, context) => x.Item1.GetAllUniqueKeysAsync(commandActivator, context, x.cancellationToken),
                context
                );
        }

        /// <summary>
        /// Gets all unique keys.
        /// </summary>
        /// <param name="transaction">The curernt transaction.</param>
        /// <param name="context">The current context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The unique keys.</returns>
        public IAsyncEnumerable<TUniqueKey> GetAllUniqueKeysAsync(IDbTransaction transaction, TContext context, CancellationToken cancellationToken = default)
        {
            return Executor.ExecuteIteratorOnTransactionAsync(
                (this, cancellationToken),
                transaction,
                (x, commandActivator, context) => x.Item1.GetAllUniqueKeysAsync(commandActivator, context, x.cancellationToken),
                context
                );
        }

        /// <summary>
        /// Gets all unique keys.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="context">The current context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The unique keys.</returns>
        protected abstract IAsyncEnumerable<TUniqueKey> GetAllUniqueKeysAsync(Func<IDbCommand> commandActivator, TContext context, CancellationToken cancellationToken);

        #endregion
    }
}

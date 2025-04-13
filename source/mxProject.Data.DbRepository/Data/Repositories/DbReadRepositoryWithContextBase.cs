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
    /// <typeparam name="TContext">The context type.</typeparam>
    public abstract class DbReadRepositoryWithContextBase<TEntity, TKey, TContext> : DbRepositoryWithContextBase<TEntity, TContext>, IReadDataRepositoryWithContext<TEntity, TKey, TContext>
        where TContext : IDataRepositoryContext
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="connectionActivator">The method to activate a connection.</param>
        /// <param name="useTransactionScope">A value that indicates whether to use ambient transactions using TransactionScope.</param>
        protected DbReadRepositoryWithContextBase(Func<IDbConnection> connectionActivator, bool useTransactionScope)
            : base(connectionActivator, useTransactionScope)
        {
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="connectionActivator">The method to activate a connection.</param>
        /// <param name="useTransactionScope">A value that indicates whether to use ambient transactions using TransactionScope.</param>
        /// <param name="configureCommand">A method to configure a command.</param>
        protected DbReadRepositoryWithContextBase(Func<IDbConnection> connectionActivator, bool useTransactionScope, Action<IDbCommand> configureCommand)
            : base(connectionActivator, useTransactionScope, configureCommand)
        {
        }

        #region Get

        /// <inheritdoc/>
        public TEntity? Get(TKey key, TContext context)
        {
            if (TryGetCurrentTransaction(context, out var transaction))
            {
                return Get(transaction, key, context);
            }
            else if (TryGetCurrentConnection(context, out var connection))
            {
                return Get(connection, key, context);
            }
            else
            {
                return Executor.ExecuteOnNewConnection(
                    (this, key),
                    (x, commandActivator, context) => x.Item1.Get(commandActivator, x.key, context),
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
        public TEntity? Get(IDbConnection connection, TKey key, TContext context)
        {
            return Executor.ExecuteOnConnection(
                (this, key),
                connection,
                (x, commandActivator, context) => x.Item1.Get(commandActivator, x.key, context),
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
        public TEntity? Get(IDbTransaction transaction, TKey key, TContext context)
        {
            return Executor.ExecuteOnTransaction(
                (this, key),
                transaction,
                (x, commandActivator, context) => x.Item1.Get(commandActivator, x.key, context),
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
        protected abstract TEntity? Get(Func<IDbCommand> commandActivator, TKey key, TContext context);

        #endregion

        #region GetRange

        /// <inheritdoc/>
        public IEnumerable<TEntity> GetRange(IEnumerable<TKey> keys, TContext context)
        {
            if (TryGetCurrentTransaction(context, out var transaction))
            {
                return GetRange(transaction, keys, context);
            }
            else if (TryGetCurrentConnection(context, out var connection))
            {
                return GetRange(connection, keys, context);
            }
            else
            {
                return Executor.ExecuteIteratorOnNewConnection(
                    (this, keys),
                    (x, commandActivator, context) => x.Item1.GetRange(commandActivator, x.keys, context),
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
        /// <returns>The entity.</returns>
        public IEnumerable<TEntity> GetRange(IDbConnection connection, IEnumerable<TKey> keys, TContext context)
        {
            return Executor.ExecuteOnConnection(
                (this, keys),
                connection,
                (x, commandActivator, context) => x.Item1.GetRange(commandActivator, x.keys, context),
                context
                );
        }

        /// <summary>
        /// Gets the entity corresponding to the specified keys.
        /// </summary>
        /// <param name="transaction">The curernt transaction.</param>
        /// <param name="keys">The keys.</param>
        /// <param name="context">The current context.</param>
        /// <returns>The entity.</returns>
        public IEnumerable<TEntity> GetRange(IDbTransaction transaction, IEnumerable<TKey> keys, TContext context)
        {
            return Executor.ExecuteOnTransaction(
                (this, keys),
                transaction,
                (x, commandActivator, context) => x.Item1.GetRange(commandActivator, x.keys, context),
                context
                );
        }

        /// <summary>
        /// Gets the entity corresponding to the specified keys.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="keys">The keys.</param>
        /// <param name="context">The current context.</param>
        /// <returns>The entity.</returns>
        protected abstract IEnumerable<TEntity> GetRange(Func<IDbCommand> commandActivator, IEnumerable<TKey> keys, TContext context);

        #endregion

        #region GetAll

        /// <inheritdoc/>
        public IEnumerable<TEntity> GetAll(TContext context)
        {
            if (TryGetCurrentTransaction(context, out var transaction))
            {
                return GetAll(transaction, context);
            }
            else if (TryGetCurrentConnection(context, out var connection))
            {
                return GetAll(connection, context);
            }
            else
            {
                return Executor.ExecuteIteratorOnNewConnection(
                    this,
                    (x, commandActivator, context) => x.GetAll(commandActivator, context),
                    context
                    );
            }
        }

        /// <summary>
        /// Gets all entities.
        /// </summary>
        /// <param name="connection">The curernt connection.</param>
        /// <param name="context">The current context.</param>
        /// <returns>The entities.</returns>
        public IEnumerable<TEntity> GetAll(IDbConnection connection, TContext context)
        {
            return Executor.ExecuteOnConnection(
                this,
                connection,
                (x, commandActivator, context) => x.GetAll(commandActivator, context),
                context
                );
        }

        /// <summary>
        /// Gets all entities.
        /// </summary>
        /// <param name="transaction">The curernt transaction.</param>
        /// <param name="context">The current context.</param>
        /// <returns>The entities.</returns>
        public IEnumerable<TEntity> GetAll(IDbTransaction transaction, TContext context)
        {
            return Executor.ExecuteOnTransaction(
                this,
                transaction,
                (x, commandActivator, context) => x.GetAll(commandActivator, context),
                context
                );
        }

        /// <summary>
        /// Gets all entities.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="context">The current context.</param>
        /// <returns>The entities.</returns>
        protected abstract IEnumerable<TEntity> GetAll(Func<IDbCommand> commandActivator, TContext context);

        #endregion

        #region GetAllKeys

        /// <inheritdoc/>
        public IEnumerable<TKey> GetAllKeys(TContext context)
        {
            if (TryGetCurrentTransaction(context, out var transaction))
            {
                return GetAllKeys(transaction, context);
            }
            else if (TryGetCurrentConnection(context, out var connection))
            {
                return GetAllKeys(connection, context);
            }
            else
            {
                return Executor.ExecuteIteratorOnNewConnection(
                    this,
                    (x, commandActivator, context) => x.GetAllKeys(commandActivator, context),
                    context
                    );
            }
        }

        /// <summary>
        /// Gets all keys.
        /// </summary>
        /// <param name="connection">The curernt connection.</param>
        /// <param name="context">The current context.</param>
        /// <returns>The keys.</returns>
        public IEnumerable<TKey> GetAllKeys(IDbConnection connection, TContext context)
        {
            return Executor.ExecuteOnConnection(
                this,
                connection,
                (x, commandActivator, context) => x.GetAllKeys(commandActivator, context),
                context
                );
        }

        /// <summary>
        /// Gets all keys.
        /// </summary>
        /// <param name="transaction">The curernt transaction.</param>
        /// <param name="context">The current context.</param>
        /// <returns>The keys.</returns>
        public IEnumerable<TKey> GetAllKeys(IDbTransaction transaction, TContext context)
        {
            return Executor.ExecuteOnTransaction(
                this,
                transaction,
                (x, commandActivator, context) => x.GetAllKeys(commandActivator, context),
                context
                );
        }

        /// <summary>
        /// Gets all keys.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="context">The current context.</param>
        /// <returns>The keys.</returns>
        protected abstract IEnumerable<TKey> GetAllKeys(Func<IDbCommand> commandActivator, TContext context);

        #endregion
    }
}

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
    /// <typeparam name="TContext">The context type.</typeparam>
    public abstract class DbReadRepositoryWithUniqueKeyWithContextBase<TEntity, TPrimaryKey, TUniqueKey, TContext> : DbRepositoryWithContextBase<TEntity, TContext>, IReadDataRepositoryWithUniqueKeyWithContext<TEntity, TPrimaryKey, TUniqueKey, TContext>
        where TContext : IDataRepositoryContext
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="connectionActivator">The method to activate a connection.</param>
        /// <param name="useTransactionScope">A value that indicates whether to use ambient transactions using TransactionScope.</param>
        protected DbReadRepositoryWithUniqueKeyWithContextBase(Func<IDbConnection> connectionActivator, bool useTransactionScope)
            : base(connectionActivator, useTransactionScope)
        {
        }

        #region Get

        /// <inheritdoc/>
        public TEntity? GetByPrimaryKey(TPrimaryKey primaryKey, TContext context)
        {
            if (TryGetCurrentTransaction(context, out var transaction))
            {
                return GetByPrimaryKey(transaction, primaryKey, context);
            }
            else if (TryGetCurrentConnection(context, out var connection))
            {
                return GetByPrimaryKey(connection, primaryKey, context);
            }
            else
            {
                return Executor.ExecuteOnNewConnection(
                    (this, primaryKey),
                    (x, commandActivator, context) => x.Item1.GetByPrimaryKey(commandActivator, x.primaryKey, context),
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
        public TEntity? GetByPrimaryKey(IDbConnection connection, TPrimaryKey primaryKey, TContext context)
        {
            return Executor.ExecuteOnConnection(
                (this, primaryKey),
                connection,
                (x, commandActivator, context) => x.Item1.GetByPrimaryKey(commandActivator, x.primaryKey, context),
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
        public TEntity? GetByPrimaryKey(IDbTransaction transaction, TPrimaryKey primaryKey, TContext context)
        {
            return Executor.ExecuteOnTransaction(
                (this, primaryKey),
                transaction,
                (x, commandActivator, context) => x.Item1.GetByPrimaryKey(commandActivator, x.primaryKey, context),
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
        protected abstract TEntity? GetByPrimaryKey(Func<IDbCommand> commandActivator, TPrimaryKey primaryKey, TContext context);

        /// <inheritdoc/>
        public TEntity? GetByUniqueKey(TUniqueKey uniqueKey, TContext context)
        {
            if (TryGetCurrentTransaction(context, out var transaction))
            {
                return GetByUniqueKey(transaction, uniqueKey, context);
            }
            else if (TryGetCurrentConnection(context, out var connection))
            {
                return GetByUniqueKey(connection, uniqueKey, context);
            }
            else
            {
                return Executor.ExecuteOnNewConnection(
                    (this, uniqueKey),
                    (x, commandActivator, context) => x.Item1.GetByUniqueKey(commandActivator, x.uniqueKey, context),
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
        public TEntity? GetByUniqueKey(IDbConnection connection, TUniqueKey uniqueKey, TContext context)
        {
            return Executor.ExecuteOnConnection(
                (this, uniqueKey),
                connection,
                (x, commandActivator, context) => x.Item1.GetByUniqueKey(commandActivator, x.uniqueKey, context),
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
        public TEntity? GetByUniqueKey(IDbTransaction transaction, TUniqueKey uniqueKey, TContext context)
        {
            return Executor.ExecuteOnTransaction(
                (this, uniqueKey),
                transaction,
                (x, commandActivator, context) => x.Item1.GetByUniqueKey(commandActivator, x.uniqueKey, context),
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
        protected abstract TEntity? GetByUniqueKey(Func<IDbCommand> commandActivator, TUniqueKey uniqueKey, TContext context);

        #endregion

        #region GetRange

        /// <inheritdoc/>
        public IEnumerable<TEntity> GetRangeByPrimaryKey(IEnumerable<TPrimaryKey> primaryKeys, TContext context)
        {
            if (TryGetCurrentTransaction(context, out var transaction))
            {
                return GetRangeByPrimaryKey(transaction, primaryKeys, context);
            }
            else if (TryGetCurrentConnection(context, out var connection))
            {
                return GetRangeByPrimaryKey(connection, primaryKeys, context);
            }
            else
            {
                return Executor.ExecuteOnNewConnection(
                    (this, primaryKeys),
                    (x, commandActivator, context) => x.Item1.GetRangeByPrimaryKey(commandActivator, x.primaryKeys, context),
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
        /// <returns>The entities.</returns>
        public IEnumerable<TEntity> GetRangeByPrimaryKey(IDbConnection connection, IEnumerable<TPrimaryKey> primaryKeys, TContext context)
        {
            return Executor.ExecuteOnConnection(
                (this, primaryKeys),
                connection,
                (x, commandActivator, context) => x.Item1.GetRangeByPrimaryKey(commandActivator, x.primaryKeys, context),
                context
                );
        }

        /// <summary>
        /// Gets the entity corresponding to the specified primary keys.
        /// </summary>
        /// <param name="transaction">The current transaction.</param>
        /// <param name="primaryKeys">The primary keys.</param>
        /// <param name="context">The current context.</param>
        /// <returns>The entities.</returns>
        public IEnumerable<TEntity> GetRangeByPrimaryKey(IDbTransaction transaction, IEnumerable<TPrimaryKey> primaryKeys, TContext context)
        {
            return Executor.ExecuteOnTransaction(
                (this, primaryKeys),
                transaction,
                (x, commandActivator, context) => x.Item1.GetRangeByPrimaryKey(commandActivator, x.primaryKeys, context),
                context
                );
        }

        /// <summary>
        /// Gets the entity corresponding to the specified primary keys.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="primaryKeys">The primary keys.</param>
        /// <param name="context">The current context.</param>
        /// <returns>The entities.</returns>
        protected abstract IEnumerable<TEntity> GetRangeByPrimaryKey(Func<IDbCommand> commandActivator, IEnumerable<TPrimaryKey> primaryKeys, TContext context);

        /// <inheritdoc/>
        public IEnumerable<TEntity> GetRangeByUniqueKey(IEnumerable<TUniqueKey> uniqueKeys, TContext context)
        {
            if (TryGetCurrentTransaction(context, out var transaction))
            {
                return GetRangeByUniqueKey(transaction, uniqueKeys, context);
            }
            else if (TryGetCurrentConnection(context, out var connection))
            {
                return GetRangeByUniqueKey(connection, uniqueKeys, context);
            }
            else
            {
                return Executor.ExecuteOnNewConnection(
                    (this, uniqueKeys),
                    (x, commandActivator, context) => x.Item1.GetRangeByUniqueKey(commandActivator, x.uniqueKeys, context),
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
        /// <returns>The entities.</returns>
        public IEnumerable<TEntity> GetRangeByUniqueKey(IDbConnection connection, IEnumerable<TUniqueKey> uniqueKeys, TContext context)
        {
            return Executor.ExecuteOnConnection(
                (this, uniqueKeys),
                connection,
                (x, commandActivator, context) => x.Item1.GetRangeByUniqueKey(commandActivator, x.uniqueKeys, context),
                context
                );
        }

        /// <summary>
        /// Gets the entity corresponding to the specified unique keys.
        /// </summary>
        /// <param name="transaction">The current transaction.</param>
        /// <param name="uniqueKeys">The unique keys.</param>
        /// <param name="context">The current context.</param>
        /// <returns>The entities.</returns>
        public IEnumerable<TEntity> GetRangeByUniqueKey(IDbTransaction transaction, IEnumerable<TUniqueKey> uniqueKeys, TContext context)
        {
            return Executor.ExecuteOnTransaction(
                (this, uniqueKeys),
                transaction,
                (x, commandActivator, context) => x.Item1.GetRangeByUniqueKey(commandActivator, x.uniqueKeys, context),
                context
                );
        }

        /// <summary>
        /// Gets the entity corresponding to the specified unique keys.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="uniqueKeys">The unique keys.</param>
        /// <param name="context">The current context.</param>
        /// <returns>The entities.</returns>
        protected abstract IEnumerable<TEntity> GetRangeByUniqueKey(Func<IDbCommand> commandActivator, IEnumerable<TUniqueKey> uniqueKeys, TContext context);

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
                return Executor.ExecuteOnNewConnection(
                    this,
                    (x, commandActivator, context) => x.GetAll(commandActivator, context),
                    context
                    );
            }
        }

        /// <summary>
        /// Gets all entities.
        /// </summary>
        /// <param name="connection">The current connection.</param>
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
        /// <param name="transaction">The current transaction.</param>
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

        #region GetAllPrimaryKeys

        /// <inheritdoc/>
        public IEnumerable<TPrimaryKey> GetAllPrimaryKeys(TContext context)
        {
            if (TryGetCurrentTransaction(context, out var transaction))
            {
                return GetAllPrimaryKeys(transaction, context);
            }
            else if (TryGetCurrentConnection(context, out var connection))
            {
                return GetAllPrimaryKeys(connection, context);
            }
            else
            {
                return Executor.ExecuteOnNewConnection(
                    this,
                    (x, commandActivator, context) => x.GetAllPrimaryKeys(commandActivator, context),
                    context
                    );
            }
        }

        /// <summary>
        /// Gets all primary keys.
        /// </summary>
        /// <param name="connection">The curernt connection.</param>
        /// <param name="context">The current context.</param>
        /// <returns>The primary keys.</returns>
        public IEnumerable<TPrimaryKey> GetAllPrimaryKeys(IDbConnection connection, TContext context)
        {
            return Executor.ExecuteOnConnection(
                this,
                connection,
                (x, commandActivator, context) => x.GetAllPrimaryKeys(commandActivator, context),
                context
                );
        }

        /// <summary>
        /// Gets all primary keys.
        /// </summary>
        /// <param name="transaction">The curernt transaction.</param>
        /// <param name="context">The current context.</param>
        /// <returns>The primary keys.</returns>
        public IEnumerable<TPrimaryKey> GetAllPrimaryKeys(IDbTransaction transaction, TContext context)
        {
            return Executor.ExecuteOnTransaction(
                this,
                transaction,
                (x, commandActivator, context) => x.GetAllPrimaryKeys(commandActivator, context),
                context
                );
        }

        /// <summary>
        /// Gets all primary keys.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="context">The current context.</param>
        /// <returns>The primary keys.</returns>
        protected abstract IEnumerable<TPrimaryKey> GetAllPrimaryKeys(Func<IDbCommand> commandActivator, TContext context);

        #endregion

        #region GetAllUniqueKeys

        /// <inheritdoc/>
        public IEnumerable<TUniqueKey> GetAllUniqueKeys(TContext context)
        {
            if (TryGetCurrentTransaction(context, out var transaction))
            {
                return GetAllUniqueKeys(transaction, context);
            }
            else if (TryGetCurrentConnection(context, out var connection))
            {
                return GetAllUniqueKeys(connection, context);
            }
            else
            {
                return Executor.ExecuteOnNewConnection(
                    this,
                    (x, commandActivator, context) => x.GetAllUniqueKeys(commandActivator, context),
                    context
                    );
            }
        }

        /// <summary>
        /// Gets all unique keys.
        /// </summary>
        /// <param name="connection">The curernt connection.</param>
        /// <param name="context">The current context.</param>
        /// <returns>The unique keys.</returns>
        public IEnumerable<TUniqueKey> GetAllUniqueKeys(IDbConnection connection, TContext context)
        {
            return Executor.ExecuteOnConnection(
                this,
                connection,
                (x, commandActivator, context) => x.GetAllUniqueKeys(commandActivator, context),
                context
                );
        }

        /// <summary>
        /// Gets all unique keys.
        /// </summary>
        /// <param name="transaction">The curernt transaction.</param>
        /// <param name="context">The current context.</param>
        /// <returns>The unique keys.</returns>
        public IEnumerable<TUniqueKey> GetAllUniqueKeys(IDbTransaction transaction, TContext context)
        {
            return Executor.ExecuteOnTransaction(
                this,
                transaction,
                (x, commandActivator, context) => x.GetAllUniqueKeys(commandActivator, context),
                context
                );
        }

        /// <summary>
        /// Gets all unique keys.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="context">The current context.</param>
        /// <returns>The unique keys.</returns>
        protected abstract IEnumerable<TUniqueKey> GetAllUniqueKeys(Func<IDbCommand> commandActivator, TContext context);

        #endregion
    }
}

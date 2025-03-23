using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Transactions;

namespace mxProject.Data.Repositories
{
    /// <summary>
    /// A basic implementation of a repository that uses a database.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TKey">The key type.</typeparam>
    /// <typeparam name="TContext">The context type.</typeparam>
    public abstract class DbReadWriteRepositoryWithContextBase<TEntity, TKey, TContext> : DbReadRepositoryWithContextBase<TEntity, TKey, TContext>, IWriteDataRepositoryWithContext<TEntity, TContext>
        where TContext : IDataRepositoryContext
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="connectionActivator">The method to activate a connection.</param>
        /// <param name="useTransactionScope">A value that indicates whether to use ambient transactions using TransactionScope.</param>
        protected DbReadWriteRepositoryWithContextBase(Func<IDbConnection> connectionActivator, bool useTransactionScope)
            : base(connectionActivator, useTransactionScope)
        {
        }

        #region Insert

        /// <inheritdoc/>
        public int Insert(TEntity entity, TContext context)
        {
            if (TryGetCurrentTransaction(context, out var transaction))
            {
                return Insert(transaction, entity, context);
            }
            else if (TryGetCurrentConnection(context, out var connection))
            {
                return Insert(connection, entity, context);
            }
            else
            {
                return Executor.ExecuteOnNewConnection(
                    (this, entity),
                    (x, commandActivator, context) => x.Item1.Insert(commandActivator, x.entity, context),
                    context
                    );
            }
        }

        /// <summary>
        /// Inserts the specified entity.
        /// </summary>
        /// <param name="connection">The current connection.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="context">The current context.</param>
        /// <returns>Number of affected entities.</returns>
        public int Insert(IDbConnection connection, TEntity entity, TContext context)
        {
            return Executor.ExecuteOnConnection(
                (this, entity),
                connection,
                (x, commandActivator, context) => x.Item1.Insert(commandActivator, x.entity, context),
                context
                );
        }

        /// <summary>
        /// Inserts the specified entity.
        /// </summary>
        /// <param name="transaction">The current transaction.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="context">The current context.</param>
        /// <returns>Number of affected entities.</returns>
        public int Insert(IDbTransaction transaction, TEntity entity, TContext context)
        {
            return Executor.ExecuteOnTransaction(
                (this, entity),
                transaction,
                (x, commandActivator, context) => x.Item1.Insert(commandActivator, x.entity, context),
                context
                );
        }

        /// <summary>
        /// Inserts the specified entity.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="context">The current context.</param>
        /// <returns>Number of affected entities.</returns>
        protected abstract int Insert(Func<IDbCommand> commandActivator, TEntity entity, TContext context);

        #endregion

        #region InsertRange

        /// <inheritdoc/>
        public int InsertRange(IEnumerable<TEntity> entities, TContext context)
        {
            if (TryGetCurrentTransaction(context, out var transaction))
            {
                return InsertRange(transaction, entities, context);
            }
            else if (TryGetCurrentConnection(context, out var connection))
            {
                return InsertRange(connection, entities, context);
            }
            else
            {
                return Executor.ExecuteOnNewConnection(
                    (this, entities),
                    (x, commandActivator, context) => x.Item1.InsertRange(commandActivator, x.entities, context),
                    context
                    );
            }
        }

        /// <summary>
        /// Inserts the specified entitis.
        /// </summary>
        /// <param name="connection">The current connection.</param>
        /// <param name="entities">The entity.</param>
        /// <param name="context">The current context.</param>
        /// <returns>Number of affected entities.</returns>
        public int InsertRange(IDbConnection connection, IEnumerable<TEntity> entities, TContext context)
        {
            return Executor.ExecuteOnConnection(
                (this, entities),
                connection,
                (x, commandActivator, context) => x.Item1.InsertRange(commandActivator, x.entities, context),
                context
                );
        }

        /// <summary>
        /// Inserts the specified entitis.
        /// </summary>
        /// <param name="transaction">The current transaction.</param>
        /// <param name="entities">The entity.</param>
        /// <param name="context">The current context.</param>
        /// <returns>Number of affected entities.</returns>
        public int InsertRange(IDbTransaction transaction, IEnumerable<TEntity> entities, TContext context)
        {
            return Executor.ExecuteOnTransaction(
                (this, entities),
                transaction,
                (x, commandActivator, context) => x.Item1.InsertRange(commandActivator, x.entities, context),
                context
                );
        }

        /// <summary>
        /// Inserts the specified entitis.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="entities">The entity.</param>
        /// <param name="context">The current context.</param>
        /// <returns>Number of affected entities.</returns>
        protected abstract int InsertRange(Func<IDbCommand> commandActivator, IEnumerable<TEntity> entities, TContext context);

        #endregion

        #region Update

        /// <inheritdoc/>
        public int Update(TEntity entity, TContext context)
        {
            if (TryGetCurrentTransaction(context, out var transaction))
            {
                return Update(transaction, entity, context);
            }
            else if (TryGetCurrentConnection(context, out var connection))
            {
                return Update(connection, entity, context);
            }
            else
            {
                return Executor.ExecuteOnNewConnection(
                    (this, entity),
                    (x, commandActivator, context) => x.Item1.Update(commandActivator, x.entity, context),
                    context
                    );
            }
        }

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="connection">The current connection.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="context">The current context.</param>
        /// <returns>Number of affected entities.</returns>
        public int Update(IDbConnection connection, TEntity entity, TContext context)
        {
            return Executor.ExecuteOnConnection(
                (this, entity),
                connection,
                (x, commandActivator, context) => x.Item1.Update(commandActivator, x.entity, context),
                context
                );
        }

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="transaction">The current transaction.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="context">The current context.</param>
        /// <returns>Number of affected entities.</returns>
        public int Update(IDbTransaction transaction, TEntity entity, TContext context)
        {
            return Executor.ExecuteOnTransaction(
                (this, entity),
                transaction,
                (x, commandActivator, context) => x.Item1.Update(commandActivator, x.entity, context),
                context
                );
        }

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="context">The current context.</param>
        /// <returns>Number of affected entities.</returns>
        protected abstract int Update(Func<IDbCommand> commandActivator, TEntity entity, TContext context);

        #endregion

        #region UpdateRange

        /// <inheritdoc/>
        public int UpdateRange(IEnumerable<TEntity> entities, TContext context)
        {
            if (TryGetCurrentTransaction(context, out var transaction))
            {
                return UpdateRange(transaction, entities, context);
            }
            else if (TryGetCurrentConnection(context, out var connection))
            {
                return UpdateRange(connection, entities, context);
            }
            else
            {
                return Executor.ExecuteOnNewConnection(
                    (this, entities),
                    (x, commandActivator, context) => x.Item1.UpdateRange(commandActivator, x.entities, context),
                    context
                    );
            }
        }

        /// <summary>
        /// Updates the specified entitis.
        /// </summary>
        /// <param name="connection">The current connection.</param>
        /// <param name="entities">The entity.</param>
        /// <param name="context">The current context.</param>
        /// <returns>Number of affected entities.</returns>
        public int UpdateRange(IDbConnection connection, IEnumerable<TEntity> entities, TContext context)
        {
            return Executor.ExecuteOnConnection(
                (this, entities),
                connection,
                (x, commandActivator, context) => x.Item1.UpdateRange(commandActivator, x.entities, context),
                context
                );
        }

        /// <summary>
        /// Updates the specified entitis.
        /// </summary>
        /// <param name="transaction">The current transaction.</param>
        /// <param name="entities">The entity.</param>
        /// <param name="context">The current context.</param>
        /// <returns>Number of affected entities.</returns>
        public int UpdateRange(IDbTransaction transaction, IEnumerable<TEntity> entities, TContext context)
        {
            return Executor.ExecuteOnTransaction(
                (this, entities),
                transaction,
                (x, commandActivator, context) => x.Item1.UpdateRange(commandActivator, x.entities, context),
                context
                );
        }

        /// <summary>
        /// Updates the specified entitis.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="entities">The entity.</param>
        /// <param name="context">The current context.</param>
        /// <returns>Number of affected entities.</returns>
        protected abstract int UpdateRange(Func<IDbCommand> commandActivator, IEnumerable<TEntity> entities, TContext context);

        #endregion

        #region Delete

        /// <inheritdoc/>
        public int Delete(TEntity entity, TContext context)
        {
            if (TryGetCurrentTransaction(context, out var transaction))
            {
                return Delete(transaction, entity, context);
            }
            else if (TryGetCurrentConnection(context, out var connection))
            {
                return Delete(connection, entity, context);
            }
            else
            {
                return Executor.ExecuteOnNewConnection(
                    (this, entity),
                    (x, commandActivator, context) => x.Item1.Delete(commandActivator, x.entity, context),
                    context
                    );
            }
        }

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="connection">The current connection.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="context">The current context.</param>
        /// <returns>Number of affected entities.</returns>
        public int Delete(IDbConnection connection, TEntity entity, TContext context)
        {
            return Executor.ExecuteOnConnection(
                (this, entity),
                connection,
                (x, commandActivator, context) => x.Item1.Delete(commandActivator, x.entity, context),
                context
                );
        }

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="transaction">The current transaction.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="context">The current context.</param>
        /// <returns>Number of affected entities.</returns>
        public int Delete(IDbTransaction transaction, TEntity entity, TContext context)
        {
            return Executor.ExecuteOnTransaction(
                (this, entity),
                transaction,
                (x, commandActivator, context) => x.Item1.Delete(commandActivator, x.entity, context),
                context
                );
        }

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="context">The current context.</param>
        protected abstract int Delete(Func<IDbCommand> commandActivator, TEntity entity, TContext context);

        #endregion

        #region DeleteRange

        /// <inheritdoc/>
        public int DeleteRange(IEnumerable<TEntity> entities, TContext context)
        {
            if (TryGetCurrentTransaction(context, out var transaction))
            {
                return DeleteRange(transaction, entities, context);
            }
            else if (TryGetCurrentConnection(context, out var connection))
            {
                return DeleteRange(connection, entities, context);
            }
            else
            {
                return Executor.ExecuteOnNewConnection(
                    (this, entities),
                    (x, commandActivator, context) => x.Item1.DeleteRange(commandActivator, x.entities, context),
                    context
                    );
            }
        }

        /// <summary>
        /// Deletes the specified entitis.
        /// </summary>
        /// <param name="connection">The current connection.</param>
        /// <param name="entities">The entity.</param>
        /// <param name="context">The current context.</param>
        /// <returns>Number of affected entities.</returns>
        public int DeleteRange(IDbConnection connection, IEnumerable<TEntity> entities, TContext context)
        {
            return Executor.ExecuteOnConnection(
                (this, entities),
                connection,
                (x, commandActivator, context) => x.Item1.DeleteRange(commandActivator, x.entities, context),
                context
                );
        }

        /// <summary>
        /// Deletes the specified entitis.
        /// </summary>
        /// <param name="transaction">The current transaction.</param>
        /// <param name="entities">The entity.</param>
        /// <param name="context">The current context.</param>
        /// <returns>Number of affected entities.</returns>
        public int DeleteRange(IDbTransaction transaction, IEnumerable<TEntity> entities, TContext context)
        {
            return Executor.ExecuteOnTransaction(
                (this, entities),
                transaction,
                (x, commandActivator, context) => x.Item1.DeleteRange(commandActivator, x.entities, context),
                context
                );
        }

        /// <summary>
        /// Deletes the specified entitis.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="entities">The entity.</param>
        /// <param name="context">The current context.</param>
        /// <returns>Number of affected entities.</returns>
        protected abstract int DeleteRange(Func<IDbCommand> commandActivator, IEnumerable<TEntity> entities, TContext context);

        #endregion
    }
}

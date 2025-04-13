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
    public abstract class DbReadWriteRepositoryWithUniqueKeyBase<TEntity, TPrimaryKey, TUniqueKey> : DbReadRepositoryWithUniqueKeyBase<TEntity, TPrimaryKey, TUniqueKey>, IWriteDataRepository<TEntity>
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="connectionActivator">The method to activate a connection.</param>
        /// <param name="useTransactionScope">A value that indicates whether to use ambient transactions using TransactionScope.</param>
        protected DbReadWriteRepositoryWithUniqueKeyBase(Func<IDbConnection> connectionActivator, bool useTransactionScope)
            : base(connectionActivator, useTransactionScope)
        {
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="connectionActivator">The method to activate a connection.</param>
        /// <param name="useTransactionScope">A value that indicates whether to use ambient transactions using TransactionScope.</param>
        /// <param name="configureCommand">A method to configure a command.</param>
        protected DbReadWriteRepositoryWithUniqueKeyBase(Func<IDbConnection> connectionActivator, bool useTransactionScope, Action<IDbCommand> configureCommand)
            : base(connectionActivator, useTransactionScope, configureCommand)
        {
        }

        #region Insert

        /// <inheritdoc/>
        public int Insert(TEntity entity)
        {
            return Executor.ExecuteOnNewConnection(
                (this, entity),
                (x, commandActivator) => x.Item1.Insert(commandActivator, x.entity)
                );
        }

        /// <summary>
        /// Inserts the specified entity.
        /// </summary>
        /// <param name="connection">The current connection.</param>
        /// <param name="entity">The entity.</param>
        /// <returns>Number of affected entities.</returns>
        public int Insert(IDbConnection connection, TEntity entity)
        {
            return Executor.ExecuteOnConnection(
                (this, entity),
                connection,
                (x, commandActivator) => x.Item1.Insert(commandActivator, x.entity)
                );
        }

        /// <summary>
        /// Inserts the specified entity.
        /// </summary>
        /// <param name="transaction">The current transaction.</param>
        /// <param name="entity">The entity.</param>
        /// <returns>Number of affected entities.</returns>
        public int Insert(IDbTransaction transaction, TEntity entity)
        {
            return Executor.ExecuteOnTransaction(
                (this, entity),
                transaction,
                (x, commandActivator) => x.Item1.Insert(commandActivator, x.entity)
                );
        }

        /// <summary>
        /// Inserts the specified entity.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="entity">The entity.</param>
        /// <returns>Number of affected entities.</returns>
        protected abstract int Insert(Func<IDbCommand> commandActivator, TEntity entity);

        #endregion

        #region InsertRange

        /// <inheritdoc/>
        public int InsertRange(IEnumerable<TEntity> entities)
        {
            return Executor.ExecuteOnNewConnection(
                (this, entities),
                (x, commandActivator) => x.Item1.InsertRange(commandActivator, x.entities)
                );
        }

        /// <summary>
        /// Inserts the specified entitis.
        /// </summary>
        /// <param name="connection">The current connection.</param>
        /// <param name="entities">The entity.</param>
        /// <returns>Number of affected entities.</returns>
        public int InsertRange(IDbConnection connection, IEnumerable<TEntity> entities)
        {
            return Executor.ExecuteOnConnection(
                (this, entities),
                connection,
                (x, commandActivator) => x.Item1.InsertRange(commandActivator, x.entities)
                );
        }

        /// <summary>
        /// Inserts the specified entitis.
        /// </summary>
        /// <param name="transaction">The current transaction.</param>
        /// <param name="entities">The entity.</param>
        /// <returns>Number of affected entities.</returns>
        public int InsertRange(IDbTransaction transaction, IEnumerable<TEntity> entities)
        {
            return Executor.ExecuteOnTransaction(
                (this, entities),
                transaction,
                (x, commandActivator) => x.Item1.InsertRange(commandActivator, x.entities)
                );
        }

        /// <summary>
        /// Inserts the specified entitis.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="entities">The entity.</param>
        /// <returns>Number of affected entities.</returns>
        protected abstract int InsertRange(Func<IDbCommand> commandActivator, IEnumerable<TEntity> entities);

        #endregion

        #region Update

        /// <inheritdoc/>
        public int Update(TEntity entity)
        {
            return Executor.ExecuteOnNewConnection(
                (this, entity),
                (x, commandActivator) => x.Item1.Update(commandActivator, x.entity)
                );
        }

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="connection">The current connection.</param>
        /// <param name="entity">The entity.</param>
        /// <returns>Number of affected entities.</returns>
        public int Update(IDbConnection connection, TEntity entity)
        {
            return Executor.ExecuteOnConnection(
                (this, entity),
                connection,
                (x, commandActivator) => x.Item1.Update(commandActivator, x.entity)
                );
        }

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="transaction">The current transaction.</param>
        /// <param name="entity">The entity.</param>
        /// <returns>Number of affected entities.</returns>
        public int Update(IDbTransaction transaction, TEntity entity)
        {
            return Executor.ExecuteOnTransaction(
                (this, entity),
                transaction,
                (x, commandActivator) => x.Item1.Update(commandActivator, x.entity)
                );
        }

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="entity">The entity.</param>
        /// <returns>Number of affected entities.</returns>
        protected abstract int Update(Func<IDbCommand> commandActivator, TEntity entity);

        #endregion

        #region UpdateRange

        /// <inheritdoc/>
        public int UpdateRange(IEnumerable<TEntity> entities)
        {
            return Executor.ExecuteOnNewConnection(
                (this, entities),
                (x, commandActivator) => x.Item1.UpdateRange(commandActivator, x.entities)
                );
        }

        /// <summary>
        /// Updates the specified entitis.
        /// </summary>
        /// <param name="connection">The current connection.</param>
        /// <param name="entities">The entity.</param>
        /// <returns>Number of affected entities.</returns>
        public int UpdateRange(IDbConnection connection, IEnumerable<TEntity> entities)
        {
            return Executor.ExecuteOnConnection(
                (this, entities),
                connection,
                (x, commandActivator) => x.Item1.UpdateRange(commandActivator, x.entities)
                );
        }

        /// <summary>
        /// Updates the specified entitis.
        /// </summary>
        /// <param name="transaction">The current transaction.</param>
        /// <param name="entities">The entity.</param>
        /// <returns>Number of affected entities.</returns>
        public int UpdateRange(IDbTransaction transaction, IEnumerable<TEntity> entities)
        {
            return Executor.ExecuteOnTransaction(
                (this, entities),
                transaction,
                (x, commandActivator) => x.Item1.UpdateRange(commandActivator, x.entities)
                );
        }

        /// <summary>
        /// Updates the specified entitis.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="entities">The entity.</param>
        /// <returns>Number of affected entities.</returns>
        protected abstract int UpdateRange(Func<IDbCommand> commandActivator, IEnumerable<TEntity> entities);

        #endregion

        #region Delete

        /// <inheritdoc/>
        public int Delete(TEntity entity)
        {
            return Executor.ExecuteOnNewConnection(
                (this, entity),
                (x, commandActivator) => x.Item1.Delete(commandActivator, x.entity)
                );
        }

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="connection">The current connection.</param>
        /// <param name="entity">The entity.</param>
        /// <returns>Number of affected entities.</returns>
        public int Delete(IDbConnection connection, TEntity entity)
        {
            return Executor.ExecuteOnConnection(
                (this, entity),
                connection,
                (x, commandActivator) => x.Item1.Delete(commandActivator, x.entity)
                );
        }

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="transaction">The current transaction.</param>
        /// <param name="entity">The entity.</param>
        /// <returns>Number of affected entities.</returns>
        public int Delete(IDbTransaction transaction, TEntity entity)
        {
            return Executor.ExecuteOnTransaction(
                (this, entity),
                transaction,
                (x, commandActivator) => x.Item1.Delete(commandActivator, x.entity)
                );
        }

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="entity">The entity.</param>
        /// <returns>Number of affected entities.</returns>
        protected abstract int Delete(Func<IDbCommand> commandActivator, TEntity entity);

        #endregion

        #region DeleteRange

        /// <inheritdoc/>
        public int DeleteRange(IEnumerable<TEntity> entities)
        {
            return Executor.ExecuteOnNewConnection(
                (this, entities),
                (x, commandActivator) => x.Item1.DeleteRange(commandActivator, x.entities)
                );
        }

        /// <summary>
        /// Deletes the specified entitis.
        /// </summary>
        /// <param name="connection">The current connection.</param>
        /// <param name="entities">The entity.</param>
        /// <returns>Number of affected entities.</returns>
        public int DeleteRange(IDbConnection connection, IEnumerable<TEntity> entities)
        {
            return Executor.ExecuteOnConnection(
                (this, entities),
                connection,
                (x, commandActivator) => x.Item1.DeleteRange(commandActivator, x.entities)
                );
        }

        /// <summary>
        /// Deletes the specified entitis.
        /// </summary>
        /// <param name="transaction">The current transaction.</param>
        /// <param name="entities">The entity.</param>
        /// <returns>Number of affected entities.</returns>
        public int DeleteRange(IDbTransaction transaction, IEnumerable<TEntity> entities)
        {
            return Executor.ExecuteOnTransaction(
                (this, entities),
                transaction,
                (x, commandActivator) => x.Item1.DeleteRange(commandActivator, x.entities)
                );
        }

        /// <summary>
        /// Deletes the specified entitis.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="entities">The entity.</param>
        /// <returns>Number of affected entities.</returns>
        protected abstract int DeleteRange(Func<IDbCommand> commandActivator, IEnumerable<TEntity> entities);

        #endregion
    }
}

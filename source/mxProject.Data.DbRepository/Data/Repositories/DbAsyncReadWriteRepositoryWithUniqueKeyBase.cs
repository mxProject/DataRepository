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
    public abstract class DbAsyncReadWriteRepositoryWithUniqueKeyBase<TEntity, TPrimaryKey, TUniqueKey> : DbAsyncReadRepositoryWithUniqueKeyBase<TEntity, TPrimaryKey, TUniqueKey>, IAsyncWriteDataRepository<TEntity>
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="connectionActivator">The method to activate a connection.</param>
        /// <param name="useTransactionScope">A value that indicates whether to use ambient transactions using TransactionScope.</param>
        protected DbAsyncReadWriteRepositoryWithUniqueKeyBase(Func<IDbConnection> connectionActivator, bool useTransactionScope)
            : base(connectionActivator, useTransactionScope)
        {
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="connectionActivator">The method to activate a connection.</param>
        /// <param name="useTransactionScope">A value that indicates whether to use ambient transactions using TransactionScope.</param>
        /// <param name="configureCommand">A method to configure a command.</param>
        protected DbAsyncReadWriteRepositoryWithUniqueKeyBase(Func<IDbConnection> connectionActivator, bool useTransactionScope, Action<IDbCommand> configureCommand)
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

        #region InsertAsync

        /// <inheritdoc/>
        public ValueTask<int> InsertAsync(TEntity entity)
        {
            return Executor.ExecuteOnNewConnectionAsync(
                (this, entity),
                (x, commandActivator) => x.Item1.InsertAsync(commandActivator, x.entity)
                );
        }

        /// <summary>
        /// Inserts the specified entity.
        /// </summary>
        /// <param name="connection">The current connection.</param>
        /// <param name="entity">The entity.</param>
        /// <returns>Number of affected entities.</returns>
        public ValueTask<int> InsertAsync(IDbConnection connection, TEntity entity)
        {
            return Executor.ExecuteOnConnectionAsync(
                (this, entity),
                connection,
                (x, commandActivator) => x.Item1.InsertAsync(commandActivator, x.entity)
                );
        }

        /// <summary>
        /// Inserts the specified entity.
        /// </summary>
        /// <param name="transaction">The current transaction.</param>
        /// <param name="entity">The entity.</param>
        /// <returns>Number of affected entities.</returns>
        public ValueTask<int> InsertAsync(IDbTransaction transaction, TEntity entity)
        {
            return Executor.ExecuteOnTransactionAsync(
                (this, entity),
                transaction,
                (x, commandActivator) => x.Item1.InsertAsync(commandActivator, x.entity)
                );
        }

        /// <summary>
        /// Inserts the specified entity.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="entity">The entity.</param>
        /// <returns>Number of affected entities.</returns>
        protected abstract ValueTask<int> InsertAsync(Func<IDbCommand> commandActivator, TEntity entity);

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

        #region InsertRangeAsync

        /// <inheritdoc/>
        public ValueTask<int> InsertRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            return Executor.ExecuteOnNewConnectionAsync(
                (this, entities, cancellationToken),
                (x, commandActivator) => x.Item1.InsertRangeAsync(commandActivator, x.entities, x.cancellationToken)
                );
        }

        /// <summary>
        /// Inserts the specified entitis.
        /// </summary>
        /// <param name="connection">The current connection.</param>
        /// <param name="entities">The entity.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Number of affected entities.</returns>
        public ValueTask<int> InsertRangeAsync(IDbConnection connection, IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            return Executor.ExecuteOnConnectionAsync(
                (this, entities, cancellationToken),
                connection,
                (x, commandActivator) => x.Item1.InsertRangeAsync(commandActivator, x.entities, x.cancellationToken)
                );
        }

        /// <summary>
        /// Inserts the specified entitis.
        /// </summary>
        /// <param name="transaction">The current transaction.</param>
        /// <param name="entities">The entity.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Number of affected entities.</returns>
        public ValueTask<int> InsertRangeAsync(IDbTransaction transaction, IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            return Executor.ExecuteOnTransactionAsync(
                (this, entities, cancellationToken),
                transaction,
                (x, commandActivator) => x.Item1.InsertRangeAsync(commandActivator, x.entities, x.cancellationToken)
                );
        }

        /// <summary>
        /// Inserts the specified entitis.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="entities">The entity.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Number of affected entities.</returns>
        protected abstract ValueTask<int> InsertRangeAsync(Func<IDbCommand> commandActivator, IEnumerable<TEntity> entities, CancellationToken cancellationToken);

        /// <inheritdoc/>
        public ValueTask<int> InsertRangeAsync(IAsyncEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            return Executor.ExecuteOnNewConnectionAsync(
                (this, entities, cancellationToken),
                (x, commandActivator) => x.Item1.InsertRangeAsync(commandActivator, x.entities, x.cancellationToken)
                );
        }

        /// <summary>
        /// Inserts the specified entitis.
        /// </summary>
        /// <param name="connection">The current connection.</param>
        /// <param name="entities">The entity.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Number of affected entities.</returns>
        public ValueTask<int> InsertRangeAsync(IDbConnection connection, IAsyncEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            return Executor.ExecuteOnConnectionAsync(
                (this, entities, cancellationToken),
                connection,
                (x, commandActivator) => x.Item1.InsertRangeAsync(commandActivator, x.entities, x.cancellationToken)
                );
        }

        /// <summary>
        /// Inserts the specified entitis.
        /// </summary>
        /// <param name="transaction">The current transaction.</param>
        /// <param name="entities">The entity.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Number of affected entities.</returns>
        public ValueTask<int> InsertRangeAsync(IDbTransaction transaction, IAsyncEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            return Executor.ExecuteOnTransactionAsync(
                (this, entities, cancellationToken),
                transaction,
                (x, commandActivator) => x.Item1.InsertRangeAsync(commandActivator, x.entities, x.cancellationToken)
                );
        }

        /// <summary>
        /// Inserts the specified entitis.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="entities">The entity.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Number of affected entities.</returns>
        protected abstract ValueTask<int> InsertRangeAsync(Func<IDbCommand> commandActivator, IAsyncEnumerable<TEntity> entities, CancellationToken cancellationToken);

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

        #region UpdateAsync

        /// <inheritdoc/>
        public ValueTask<int> UpdateAsync(TEntity entity)
        {
            return Executor.ExecuteOnNewConnectionAsync(
                (this, entity),
                (x, commandActivator) => x.Item1.UpdateAsync(commandActivator, x.entity)
                );
        }

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="connection">The current connection.</param>
        /// <param name="entity">The entity.</param>
        /// <returns>Number of affected entities.</returns>
        public ValueTask<int> UpdateAsync(IDbConnection connection, TEntity entity)
        {
            return Executor.ExecuteOnConnectionAsync(
                (this, entity),
                connection,
                (x, commandActivator) => x.Item1.UpdateAsync(commandActivator, x.entity)
                );
        }

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="transaction">The current transaction.</param>
        /// <param name="entity">The entity.</param>
        /// <returns>Number of affected entities.</returns>
        public ValueTask<int> UpdateAsync(IDbTransaction transaction, TEntity entity)
        {
            return Executor.ExecuteOnTransactionAsync(
                (this, entity),
                transaction,
                (x, commandActivator) => x.Item1.UpdateAsync(commandActivator, x.entity)
                );
        }

        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="entity">The entity.</param>
        /// <returns>Number of affected entities.</returns>
        protected abstract ValueTask<int> UpdateAsync(Func<IDbCommand> commandActivator, TEntity entity);

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

        #region UpdateRangeAsync

        /// <inheritdoc/>
        public ValueTask<int> UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            return Executor.ExecuteOnNewConnectionAsync(
                (this, entities, cancellationToken),
                (x, commandActivator) => x.Item1.UpdateRangeAsync(commandActivator, x.entities, x.cancellationToken)
                );
        }

        /// <summary>
        /// Updates the specified entitis.
        /// </summary>
        /// <param name="connection">The current connection.</param>
        /// <param name="entities">The entity.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Number of affected entities.</returns>
        public ValueTask<int> UpdateRangeAsync(IDbConnection connection, IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            return Executor.ExecuteOnConnectionAsync(
                (this, entities, cancellationToken),
                connection,
                (x, commandActivator) => x.Item1.UpdateRangeAsync(commandActivator, x.entities, x.cancellationToken)
                );
        }

        /// <summary>
        /// Updates the specified entitis.
        /// </summary>
        /// <param name="transaction">The current transaction.</param>
        /// <param name="entities">The entity.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Number of affected entities.</returns>
        public ValueTask<int> UpdateRangeAsync(IDbTransaction transaction, IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            return Executor.ExecuteOnTransactionAsync(
                (this, entities, cancellationToken),
                transaction,
                (x, commandActivator) => x.Item1.UpdateRangeAsync(commandActivator, x.entities, x.cancellationToken)
                );
        }

        /// <summary>
        /// Updates the specified entitis.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="entities">The entity.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Number of affected entities.</returns>
        protected abstract ValueTask<int> UpdateRangeAsync(Func<IDbCommand> commandActivator, IEnumerable<TEntity> entities, CancellationToken cancellationToken);

        /// <inheritdoc/>
        public ValueTask<int> UpdateRangeAsync(IAsyncEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            return Executor.ExecuteOnNewConnectionAsync(
                (this, entities, cancellationToken),
                (x, commandActivator) => x.Item1.UpdateRangeAsync(commandActivator, x.entities, x.cancellationToken)
                );
        }

        /// <summary>
        /// Updates the specified entitis.
        /// </summary>
        /// <param name="connection">The current connection.</param>
        /// <param name="entities">The entity.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Number of affected entities.</returns>
        public ValueTask<int> UpdateRangeAsync(IDbConnection connection, IAsyncEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            return Executor.ExecuteOnConnectionAsync(
                (this, entities, cancellationToken),
                connection,
                (x, commandActivator) => x.Item1.UpdateRangeAsync(commandActivator, x.entities, x.cancellationToken)
                );
        }

        /// <summary>
        /// Updates the specified entitis.
        /// </summary>
        /// <param name="transaction">The current transaction.</param>
        /// <param name="entities">The entity.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Number of affected entities.</returns>
        public ValueTask<int> UpdateRangeAsync(IDbTransaction transaction, IAsyncEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            return Executor.ExecuteOnTransactionAsync(
                (this, entities, cancellationToken),
                transaction,
                (x, commandActivator) => x.Item1.UpdateRangeAsync(commandActivator, x.entities, x.cancellationToken)
                );
        }

        /// <summary>
        /// Updates the specified entitis.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="entities">The entity.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Number of affected entities.</returns>
        protected abstract ValueTask<int> UpdateRangeAsync(Func<IDbCommand> commandActivator, IAsyncEnumerable<TEntity> entities, CancellationToken cancellationToken);

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

        #region DeleteAsync

        /// <inheritdoc/>
        public ValueTask<int> DeleteAsync(TEntity entity)
        {
            return Executor.ExecuteOnNewConnectionAsync(
                (this, entity),
                (x, commandActivator) => x.Item1.DeleteAsync(commandActivator, x.entity)
                );
        }

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="connection">The current connection.</param>
        /// <param name="entity">The entity.</param>
        /// <returns>Number of affected entities.</returns>
        public ValueTask<int> DeleteAsync(IDbConnection connection, TEntity entity)
        {
            return Executor.ExecuteOnConnectionAsync(
                (this, entity),
                connection,
                (x, commandActivator) => x.Item1.DeleteAsync(commandActivator, x.entity)
                );
        }

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="transaction">The current transaction.</param>
        /// <param name="entity">The entity.</param>
        /// <returns>Number of affected entities.</returns>
        public ValueTask<int> DeleteAsync(IDbTransaction transaction, TEntity entity)
        {
            return Executor.ExecuteOnTransactionAsync(
                (this, entity),
                transaction,
                (x, commandActivator) => x.Item1.DeleteAsync(commandActivator, x.entity)
                );
        }

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="entity">The entity.</param>
        /// <returns>Number of affected entities.</returns>
        protected abstract ValueTask<int> DeleteAsync(Func<IDbCommand> commandActivator, TEntity entity);

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

        #region DeleteRangeAsync

        /// <inheritdoc/>
        public ValueTask<int> DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            return Executor.ExecuteOnNewConnectionAsync(
                (this, entities, cancellationToken),
                (x, commandActivator) => x.Item1.DeleteRangeAsync(commandActivator, x.entities, x.cancellationToken)
                );
        }

        /// <summary>
        /// Deletes the specified entitis.
        /// </summary>
        /// <param name="connection">The current connection.</param>
        /// <param name="entities">The entity.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Number of affected entities.</returns>
        public ValueTask<int> DeleteRangeAsync(IDbConnection connection, IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            return Executor.ExecuteOnConnectionAsync(
                (this, entities, cancellationToken),
                connection,
                (x, commandActivator) => x.Item1.DeleteRangeAsync(commandActivator, x.entities, x.cancellationToken)
                );
        }

        /// <summary>
        /// Deletes the specified entitis.
        /// </summary>
        /// <param name="transaction">The current transaction.</param>
        /// <param name="entities">The entity.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Number of affected entities.</returns>
        public ValueTask<int> DeleteRangeAsync(IDbTransaction transaction, IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            return Executor.ExecuteOnTransactionAsync(
                (this, entities, cancellationToken),
                transaction,
                (x, commandActivator) => x.Item1.DeleteRangeAsync(commandActivator, x.entities, x.cancellationToken)
                );
        }

        /// <summary>
        /// Deletes the specified entitis.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="entities">The entity.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Number of affected entities.</returns>
        protected abstract ValueTask<int> DeleteRangeAsync(Func<IDbCommand> commandActivator, IEnumerable<TEntity> entities, CancellationToken cancellationToken);

        /// <inheritdoc/>
        public ValueTask<int> DeleteRangeAsync(IAsyncEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            return Executor.ExecuteOnNewConnectionAsync(
                (this, entities, cancellationToken),
                (x, commandActivator) => x.Item1.DeleteRangeAsync(commandActivator, x.entities, x.cancellationToken)
                );
        }

        /// <summary>
        /// Deletes the specified entitis.
        /// </summary>
        /// <param name="connection">The current connection.</param>
        /// <param name="entities">The entity.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Number of affected entities.</returns>
        public ValueTask<int> DeleteRangeAsync(IDbConnection connection, IAsyncEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            return Executor.ExecuteOnConnectionAsync(
                (this, entities, cancellationToken),
                connection,
                (x, commandActivator) => x.Item1.DeleteRangeAsync(commandActivator, x.entities, x.cancellationToken)
                );
        }

        /// <summary>
        /// Deletes the specified entitis.
        /// </summary>
        /// <param name="transaction">The current transaction.</param>
        /// <param name="entities">The entity.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Number of affected entities.</returns>
        public ValueTask<int> DeleteRangeAsync(IDbTransaction transaction, IAsyncEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            return Executor.ExecuteOnTransactionAsync(
                (this, entities, cancellationToken),
                transaction,
                (x, commandActivator) => x.Item1.DeleteRangeAsync(commandActivator, x.entities, x.cancellationToken)
                );
        }

        /// <summary>
        /// Deletes the specified entitis.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="entities">The entity.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Number of affected entities.</returns>
        protected abstract ValueTask<int> DeleteRangeAsync(Func<IDbCommand> commandActivator, IAsyncEnumerable<TEntity> entities, CancellationToken cancellationToken);

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using mxProject.Data.Extensions;
using mxProject.Data.Repositories;

namespace Test.DbRepositories
{
    /// <summary>
    /// Repository class for sample entities.
    /// </summary>
    internal class SampleEntityAsyncRepository : DbAsyncReadWriteRepositoryBase<SampleEntity, int>
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="connectionActivator">A method to activate a connection.</param>
        /// <param name="useTransactionScope">A value indicating whether to use TransactionScope.</param>
        /// <param name="configureCommand">A method to configure a command.</param>
        public SampleEntityAsyncRepository(Func<IDbConnection> connectionActivator, bool useTransactionScope, Action<IDbCommand> configureCommand)
            : base(connectionActivator, useTransactionScope, configureCommand)
        {
        }

        #region get

        /// <summary>
        /// Gets the entity corresponding to the specified key.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="key">The key.</param>
        /// <returns>The entity.</returns>
        protected override SampleEntity? Get(Func<IDbCommand> commandActivator, int key)
        {
            return SampleEntityRepository.GetSampleEntities(commandActivator, new[] { key }).FirstOrDefault();
        }

        /// <summary>
        /// Gets the entities corresponding to the specified keys.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="keys">The keys.</param>
        /// <returns>The entities.</returns>
        protected override IEnumerable<SampleEntity> GetRange(Func<IDbCommand> commandActivator, IEnumerable<int> keys)
        {
            return SampleEntityRepository.GetSampleEntities(commandActivator, keys);
        }

        /// <summary>
        /// Gets all entities.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <returns>The entities.</returns>
        protected override IEnumerable<SampleEntity> GetAll(Func<IDbCommand> commandActivator)
        {
            return SampleEntityRepository.GetSampleEntities(commandActivator, Array.Empty<int>());
        }

        /// <summary>
        /// Gets all keys.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <returns>The keys.</returns>
        protected override IEnumerable<int> GetAllKeys(Func<IDbCommand> commandActivator)
        {
            return SampleEntityRepository.GetAllSampleEntityKeys(commandActivator);
        }

        #endregion

        #region get async

        /// <summary>
        /// Gets the entity corresponding to the specified key.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="key">The key.</param>
        /// <returns>The entity.</returns>
        protected override ValueTask<SampleEntity?> GetAsync(Func<IDbCommand> commandActivator, int key)
        {
            return new ValueTask<SampleEntity?>(SampleEntityRepository.GetSampleEntities(commandActivator, new[] { key }).FirstOrDefault());
        }

        /// <summary>
        /// Gets the entities corresponding to the specified keys.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="keys">The keys.</param>
        /// <returns>The entities.</returns>
        protected async override IAsyncEnumerable<SampleEntity> GetRangeAsync(Func<IDbCommand> commandActivator, IEnumerable<int> keys, [EnumeratorCancellation] CancellationToken cancellation = default)
        {
            await Task.Yield();

            foreach (var entity in SampleEntityRepository.GetSampleEntities(commandActivator, keys))
            {
                yield return entity;
            }
        }

        /// <summary>
        /// Gets the entities corresponding to the specified keys.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="keys">The keys.</param>
        /// <returns>The entities.</returns>
        protected async override IAsyncEnumerable<SampleEntity> GetRangeAsync(Func<IDbCommand> commandActivator, IAsyncEnumerable<int> keys, [EnumeratorCancellation] CancellationToken cancellation = default)
        {
            await Task.Yield();

            foreach (var entity in SampleEntityRepository.GetSampleEntities(commandActivator, ToSync(keys)))
            {
                yield return entity;
            }
        }

        /// <summary>
        /// Gets all entities.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <returns>The entities.</returns>
        protected async override IAsyncEnumerable<SampleEntity> GetAllAsync(Func<IDbCommand> commandActivator, [EnumeratorCancellation] CancellationToken cancellation = default)
        {
            await Task.Yield();

            foreach (var entity in SampleEntityRepository.GetSampleEntities(commandActivator, Array.Empty<int>()))
            {
                yield return entity;
            }
        }

        /// <summary>
        /// Gets all keys.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <returns>The keys.</returns>
        protected async override IAsyncEnumerable<int> GetAllKeysAsync(Func<IDbCommand> commandActivator, [EnumeratorCancellation] CancellationToken cancellation = default)
        {
            await Task.Yield();

            foreach (var key in SampleEntityRepository.GetAllSampleEntityKeys(commandActivator))
            {
                yield return key;
            }
        }

        #endregion

        #region insert

        /// <summary>
        /// Inserts the entity.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="entity">The entity.</param>
        /// <returns>Number of affected rows.</returns>
        protected override int Insert(Func<IDbCommand> commandActivator, SampleEntity entity)
        {
            return InsertRange(commandActivator, new[] { entity });
        }

        /// <summary>
        /// Inserts the entities.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="entities">The entities.</param>
        /// <returns>Number of affected rows.</returns>
        protected override int InsertRange(Func<IDbCommand> commandActivator, IEnumerable<SampleEntity> entities)
        {
            return SampleEntityRepository.InsertSampleEntities(commandActivator, entities);
        }

        #endregion

        #region insert async

        /// <summary>
        /// Inserts the entity.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="entity">The entity.</param>
        /// <returns>Number of affected rows.</returns>
        protected override ValueTask<int> InsertAsync(Func<IDbCommand> commandActivator, SampleEntity entity)
        {
            return InsertRangeAsync(commandActivator, new[] { entity }, default);
        }

        /// <summary>
        /// Inserts the entities.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="entities">The entities.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Number of affected rows.</returns>
        protected override async ValueTask<int> InsertRangeAsync(Func<IDbCommand> commandActivator, IEnumerable<SampleEntity> entities, CancellationToken cancellationToken = default)
        {
            using var command = SampleEntityRepository.CreateInsertCommand(commandActivator);

            int affectedRows = 0;

            foreach (var entity in entities)
            {
                command.SetParameterValue("@CODE", entity.Code);
                command.SetParameterValue("@NAME", entity.Name);
                command.SetParameterValue("@ID", entity.Id);

                affectedRows += await command.ExecuteNonQueryAsync(cancellationToken);
            }

            return affectedRows;
        }

        /// <summary>
        /// Inserts the entities.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="entities">The entities.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Number of affected rows.</returns>
        protected override async ValueTask<int> InsertRangeAsync(Func<IDbCommand> commandActivator, IAsyncEnumerable<SampleEntity> entities, CancellationToken cancellationToken = default)
        {
            using var command = SampleEntityRepository.CreateInsertCommand(commandActivator);

            int affectedRows = 0;

            await foreach (var entity in entities.ConfigureAwait(false))
            {
                command.SetParameterValue("@CODE", entity.Code);
                command.SetParameterValue("@NAME", entity.Name);
                command.SetParameterValue("@ID", entity.Id);

                affectedRows += await command.ExecuteNonQueryAsync(cancellationToken);
            }

            return affectedRows;
        }

        #endregion

        #region update  

        /// <summary>
        /// Updates the entity.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="entity">The entity.</param>
        /// <returns>Number of affected rows.</returns>
        protected override int Update(Func<IDbCommand> commandActivator, SampleEntity entity)
        {
            return SampleEntityRepository.UpdateSampleEntities(commandActivator, new[] { entity });
        }

        /// <summary>
        /// Updates the entities.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="entities">The entities.</param>
        /// <returns>Number of affected rows.</returns>
        protected override int UpdateRange(Func<IDbCommand> commandActivator, IEnumerable<SampleEntity> entities)
        {
            return SampleEntityRepository.UpdateSampleEntities(commandActivator, entities);
        }

        #endregion

        #region update async

        /// <summary>
        /// Updates the entity.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="entity">The entity.</param>
        /// <returns>Number of affected rows.</returns>
        protected override ValueTask<int> UpdateAsync(Func<IDbCommand> commandActivator, SampleEntity entity)
        {
            return UpdateRangeAsync(commandActivator, new[] { entity }, default);
        }

        /// <summary>
        /// Updates the entities.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="entities">The entities.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Number of affected rows.</returns>
        protected override async ValueTask<int> UpdateRangeAsync(Func<IDbCommand> commandActivator, IEnumerable<SampleEntity> entities, CancellationToken cancellationToken)
        {
            using var command = SampleEntityRepository.CreateUpdateCommand(commandActivator);

            int affectedRows = 0;

            foreach (var entity in entities)
            {
                command.SetParameterValue("@CODE", entity.Code);
                command.SetParameterValue("@NAME", entity.Name);
                command.SetParameterValue("@ID", entity.Id);

                affectedRows += await command.ExecuteNonQueryAsync(cancellationToken);
            }

            return affectedRows;
        }

        /// <summary>
        /// Updates the entities.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="entities">The entities.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Number of affected rows.</returns>
        protected override async ValueTask<int> UpdateRangeAsync(Func<IDbCommand> commandActivator, IAsyncEnumerable<SampleEntity> entities, CancellationToken cancellationToken)
        {
            using var command = SampleEntityRepository.CreateInsertCommand(commandActivator);

            int affectedRows = 0;

            await foreach (var entity in entities.ConfigureAwait(false))
            {
                command.SetParameterValue("@CODE", entity.Code);
                command.SetParameterValue("@NAME", entity.Name);
                command.SetParameterValue("@ID", entity.Id);

                affectedRows += await command.ExecuteNonQueryAsync(cancellationToken);
            }

            return affectedRows;
        }

        #endregion

        #region delete

        /// <summary>
        /// Deletes the entity.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="entity">The entity.</param>
        /// <returns>Number of affected rows.</returns>
        protected override int Delete(Func<IDbCommand> commandActivator, SampleEntity entity)
        {
            return SampleEntityRepository.DeleteSampleEntities(commandActivator, new[] { entity });
        }

        /// <summary>
        /// Deletes the entities.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="entities">The entities.</param>
        /// <returns>Number of affected rows.</returns>
        protected override int DeleteRange(Func<IDbCommand> commandActivator, IEnumerable<SampleEntity> entities)
        {
            return SampleEntityRepository.DeleteSampleEntities(commandActivator, entities);
        }

        #endregion

        #region delete async

        /// <summary>
        /// Deletes the entity.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="entity">The entity.</param>
        /// <returns>Number of affected rows.</returns>
        protected override ValueTask<int> DeleteAsync(Func<IDbCommand> commandActivator, SampleEntity entity)
        {
            return DeleteSampleEntitiesAsync(commandActivator, new[] { entity }, default);
        }

        /// <summary>
        /// Deletes the entities.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="entities">The entities.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Number of affected rows.</returns>
        protected override ValueTask<int> DeleteRangeAsync(Func<IDbCommand> commandActivator, IEnumerable<SampleEntity> entities, CancellationToken cancellationToken)
        {
            return DeleteSampleEntitiesAsync(commandActivator, entities, cancellationToken);
        }

        /// <summary>
        /// Deletes the entities.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="entities">The entities.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Number of affected rows.</returns>
        protected override ValueTask<int> DeleteRangeAsync(Func<IDbCommand> commandActivator, IAsyncEnumerable<SampleEntity> entities, CancellationToken cancellationToken)
        {
            return DeleteSampleEntitiesAsync(commandActivator, entities, cancellationToken);
        }

        /// <summary>
        /// Deletes the entities.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="entities">The entities.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Number of affected rows.</returns>
        internal static async ValueTask<int> DeleteSampleEntitiesAsync(Func<IDbCommand> commandActivator, IEnumerable<SampleEntity> entities, CancellationToken cancellationToken)
        {
            var sql = new StringBuilder();

            sql.AppendLine("delete from SAMPLE_TABLE");

            sql.AppendLine("where ID = @ID");

            using var command = commandActivator();

            Assert.Equal(SampleDatabase.DefaultCommandTimeout, command.CommandTimeout);

            command.AddParameter("@ID", 0);

            command.CommandText = sql.ToString();

            int affectedRows = 0;

            foreach (var entity in entities)
            {
                command.SetParameterValue("@ID", entity.Id);

                affectedRows += await command.ExecuteNonQueryAsync(cancellationToken);
            }

            return affectedRows;
        }

        /// <summary>
        /// Deletes the entities.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="entities">The entities.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Number of affected rows.</returns>
        internal static async ValueTask<int> DeleteSampleEntitiesAsync(Func<IDbCommand> commandActivator, IAsyncEnumerable<SampleEntity> entities, CancellationToken cancellationToken)
        {
            var sql = new StringBuilder();

            sql.AppendLine("delete from SAMPLE_TABLE");

            sql.AppendLine("where ID = @ID");

            using var command = commandActivator();

            Assert.Equal(SampleDatabase.DefaultCommandTimeout, command.CommandTimeout);

            command.AddParameter("@ID", 0);

            command.CommandText = sql.ToString();

            int affectedRows = 0;

            await foreach (var entity in entities.ConfigureAwait(false))
            {
                command.SetParameterValue("@ID", entity.Id);

                affectedRows += await command.ExecuteNonQueryAsync(cancellationToken);
            }

            return affectedRows;
        }

        #endregion

        private static List<T> ToSync<T>(IAsyncEnumerable<T> asyncEnumerable)
        {
            var list = new List<T>();
            var enumerator = asyncEnumerable.GetAsyncEnumerator();
            while (enumerator.MoveNextAsync().AsTask().Result)
            {
                list.Add(enumerator.Current);
            }
            return list;
        }
    }
}

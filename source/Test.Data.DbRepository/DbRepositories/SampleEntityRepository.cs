using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using mxProject.Data.Repositories;

namespace Test.DbRepositories
{
    /// <summary>
    /// Repository class for sample entities.
    /// </summary>
    internal class SampleEntityRepository : DbReadWriteRepositoryBase<SampleEntity, int>
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="connectionActivator">A method to activate a connection.</param>
        /// <param name="useTransactionScope">A value indicating whether to use TransactionScope.</param>
        /// <param name="configureCommand">A method to configure a command.</param>
        public SampleEntityRepository(Func<IDbConnection> connectionActivator, bool useTransactionScope, Action<IDbCommand> configureCommand)
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
            return GetSampleEntities(commandActivator, new[] { key }).FirstOrDefault();
        }

        /// <summary>
        /// Gets the entities corresponding to the specified keys.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="keys">The keys.</param>
        /// <returns>The entities.</returns>
        protected override IEnumerable<SampleEntity> GetRange(Func<IDbCommand> commandActivator, IEnumerable<int> keys)
        {
            return GetSampleEntities(commandActivator, keys);
        }

        /// <summary>
        /// Gets all entities.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <returns>The entities.</returns>
        protected override IEnumerable<SampleEntity> GetAll(Func<IDbCommand> commandActivator)
        {
            return GetSampleEntities(commandActivator, Array.Empty<int>());
        }

        /// <summary>
        /// Gets all keys.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <returns>The keys.</returns>
        protected override IEnumerable<int> GetAllKeys(Func<IDbCommand> commandActivator)
        {
            return GetAllSampleEntityKeys(commandActivator);
        }

        /// <summary>
        /// Gets the entities corresponding to the specified keys.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="keys">The keys.</param>
        /// <returns>The entities.</returns>
        internal static IEnumerable<SampleEntity> GetSampleEntities(Func<IDbCommand> commandActivator, IEnumerable<int> keys)
        {
            using var command = commandActivator();

            Assert.Equal(SampleDatabase.DefaultCommandTimeout, command.CommandTimeout);

            var sql = new StringBuilder();

            sql.AppendLine("select");

            sql.AppendLine("ID");
            sql.AppendLine(", CODE");
            sql.AppendLine(", NAME");

            sql.AppendLine("from");
            sql.AppendLine("SAMPLE_TABLE");

            sql.AppendLine("where 1=1");

            if (keys != null && keys.Any())
            {
                var array = keys.ToArray();

                if (array.Length == 1)
                {
                    sql.AppendLine("and ID = @ID");
                    command.AddParameter("@ID", array[0]);
                }
                else
                {
                    sql.Append("and ID in (");

                    for (int i = 0; i < array.Length; ++i)
                    {
                        if (i > 0) { sql.Append(", "); }
                        sql.Append($"@ID{i}");
                        command.AddParameter($"@ID{i}", array[i]);
                    }

                    sql.AppendLine(")");
                }
            }

            command.CommandText = sql.ToString();

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                yield return new SampleEntity
                {
                    Id = reader.GetInt32(reader.GetOrdinal("ID")),
                    Code = reader.GetString(reader.GetOrdinal("CODE")),
                    Name = reader.GetString(reader.GetOrdinal("NAME"))
                };
            }
        }

        internal static IEnumerable<int> GetAllSampleEntityKeys(Func<IDbCommand> commandActivator)
        {
            using var command = commandActivator();

            Assert.Equal(SampleDatabase.DefaultCommandTimeout, command.CommandTimeout);

            var sql = new StringBuilder();

            sql.AppendLine("select");

            sql.AppendLine("ID");

            sql.AppendLine("from");
            sql.AppendLine("SAMPLE_TABLE");

            command.CommandText = sql.ToString();

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                yield return reader.GetInt32(reader.GetOrdinal("ID"));
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
            return InsertSampleEntities(commandActivator, new[] { entity });
        }

        /// <summary>
        /// Inserts the entities.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="entities">The entities.</param>
        /// <returns>Number of affected rows.</returns>
        protected override int InsertRange(Func<IDbCommand> commandActivator, IEnumerable<SampleEntity> entities)
        {
            return InsertSampleEntities(commandActivator, entities);
        }

        /// <summary>
        /// Inserts the entities.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="entities">The entities.</param>
        /// <returns>Number of affected rows.</returns>
        internal static int InsertSampleEntities(Func<IDbCommand> commandActivator, IEnumerable<SampleEntity> entities)
        {
            using var command = CreateInsertCommand(commandActivator);

            int affectedRows = 0;

            foreach (var entity in entities)
            {
                command.SetParameterValue("@CODE", entity.Code);
                command.SetParameterValue("@NAME", entity.Name);
                command.SetParameterValue("@ID", entity.Id);

                affectedRows += command.ExecuteNonQuery();
            }

            return affectedRows;
        }

        /// <summary>
        /// Creates an insert command for the SAMPLE_TABLE.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <returns>The configured insert command.</returns>
        internal static IDbCommand CreateInsertCommand(Func<IDbCommand> commandActivator)
        {
            var sql = new StringBuilder();

            sql.AppendLine("insert into SAMPLE_TABLE (");

            sql.AppendLine("ID");
            sql.AppendLine(", CODE");
            sql.AppendLine(", NAME");

            sql.AppendLine(") values (");

            sql.AppendLine("@ID");
            sql.AppendLine(", @CODE");
            sql.AppendLine(", @NAME");

            sql.AppendLine(")");

            var command = commandActivator();

            Assert.Equal(SampleDatabase.DefaultCommandTimeout, command.CommandTimeout);

            command.AddParameter("@CODE", "");
            command.AddParameter("@NAME", "");
            command.AddParameter("@ID", 0);

            command.CommandText = sql.ToString();

            return command;
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
            return UpdateSampleEntities(commandActivator, new[] { entity });
        }

        /// <summary>
        /// Updates the entities.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="entities">The entities.</param>
        /// <returns>Number of affected rows.</returns>
        protected override int UpdateRange(Func<IDbCommand> commandActivator, IEnumerable<SampleEntity> entities)
        {
            return UpdateSampleEntities(commandActivator, entities);
        }

        /// <summary>
        /// Updates the entities.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="entities">The entities.</param>
        /// <returns>Number of affected rows.</returns>
        internal static int UpdateSampleEntities(Func<IDbCommand> commandActivator, IEnumerable<SampleEntity> entities)
        {
            var command = CreateUpdateCommand(commandActivator);

            int affectedRows = 0;

            foreach (var entity in entities)
            {
                command.SetParameterValue("@CODE", entity.Code);
                command.SetParameterValue("@NAME", entity.Name);
                command.SetParameterValue("@ID", entity.Id);

                affectedRows += command.ExecuteNonQuery();
            }

            return affectedRows;
        }

        /// <summary>
        /// Creates an update command for the SAMPLE_TABLE.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <returns>The configured update command.</returns>
        internal static IDbCommand CreateUpdateCommand(Func<IDbCommand> commandActivator)
        {
            var sql = new StringBuilder();

            sql.AppendLine("update SAMPLE_TABLE set");

            sql.AppendLine("CODE = @CODE");
            sql.AppendLine(", NAME = @NAME");

            sql.AppendLine("where ID = @ID");

            var command = commandActivator();

            Assert.Equal(SampleDatabase.DefaultCommandTimeout, command.CommandTimeout);

            command.AddParameter("@CODE", "");
            command.AddParameter("@NAME", "");
            command.AddParameter("@ID", 0);

            command.CommandText = sql.ToString();

            return command;
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
            return DeleteSampleEntities(commandActivator, new[] { entity });
        }

        /// <summary>
        /// Deletes the entities.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="entities">The entities.</param>
        /// <returns>Number of affected rows.</returns>
        protected override int DeleteRange(Func<IDbCommand> commandActivator, IEnumerable<SampleEntity> entities)
        {
            return DeleteSampleEntities(commandActivator, entities);
        }

        /// <summary>
        /// Deletes the entities.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="entities">The entities.</param>
        /// <returns>Number of affected rows.</returns>
        internal static int DeleteSampleEntities(Func<IDbCommand> commandActivator, IEnumerable<SampleEntity> entities)
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

                affectedRows += command.ExecuteNonQuery();
            }

            return affectedRows;
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using mxProject.Data.Repositories;
using Test.DbRepositories;

namespace Test.DbRepositories
{
    /// <summary>
    /// Represents the condition for querying SampleEntity.
    /// </summary>
    internal class SampleEntityCondition
    {
        /// <summary>
        /// Gets or sets the minimum code.
        /// </summary>
        internal string? MinimumCode { get; set; }

        /// <summary>
        /// Gets or sets the maximum code.
        /// </summary>
        internal string? MaximumCode { get; set; }

        /// <summary>
        /// Gets or sets the name pattern.
        /// </summary>
        internal string? NamePattern { get; set; }
    }

    /// <summary>
    /// Provides methods to query SampleEntity from the database.
    /// </summary>
    internal class SampleEntityQuery : DbQueryBase<SampleEntity, SampleEntityCondition>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SampleEntityQuery"/> class.
        /// </summary>
        /// <param name="connectionActivator">A method to activate a connection.</param>
        /// <param name="useTransactionScope">A value that indicates whether to use ambient transactions using TransactionScope.</param>
        /// <param name="configureCommand">A method to configure a command.</param>
        public SampleEntityQuery(Func<IDbConnection> connectionActivator, bool useTransactionScope, Action<IDbCommand> configureCommand)
            : base(connectionActivator, useTransactionScope, configureCommand)
        {
        }

        /// <summary>
        /// Gets the count of entities that match the specified condition.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="condition">The query condition.</param>
        /// <returns>The count of entities.</returns>
        public override int GetCount(Func<IDbCommand> commandActivator, SampleEntityCondition condition)
        {
            using var command = CreateCommand(true, commandActivator, condition);

            return Convert.ToInt32(command.ExecuteScalar());
        }

        /// <summary>
        /// Queries the entities that match the specified condition.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="condition">The query condition.</param>
        /// <param name="skipCount">The number of entities to skip.</param>
        /// <param name="maximumCount">The maximum number of entities to retrieve.</param>
        /// <returns>The entities that match the condition.</returns>
        public override IEnumerable<SampleEntity> Query(Func<IDbCommand> commandActivator, SampleEntityCondition condition, int skipCount = 0, int? maximumCount = null)
        {
            using var command = CreateCommand(false, commandActivator, condition, skipCount, maximumCount);

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                yield return new SampleEntity()
                {
                    Id = reader.GetInt32(reader.GetOrdinal("ID")),
                    Code = reader.GetString(reader.GetOrdinal("CODE")),
                    Name = reader.GetString(reader.GetOrdinal("NAME"))
                };
            }
        }

        /// <summary>
        /// Creates a command based on the specified condition.
        /// </summary>
        /// <param name="forGetCount">Indicates whether the command is for getting the count.</param>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="condition">The query condition.</param>
        /// <param name="skipCount">The number of entities to skip.</param>
        /// <param name="maximumCount">The maximum number of entities to retrieve.</param>
        /// <returns>The created command.</returns>
        private IDbCommand CreateCommand(bool forGetCount, Func<IDbCommand> commandActivator, SampleEntityCondition condition, int skipCount = 0, int? maximumCount = null)
        {
            var command = commandActivator();

            Assert.Equal(SampleDatabase.DefaultCommandTimeout, command.CommandTimeout);

            var sql = new StringBuilder();

            if (forGetCount)
            {
                sql.AppendLine("select count(*) from (");
            }

            sql.AppendLine("select");

            sql.AppendLine("ID");
            sql.AppendLine(", CODE");
            sql.AppendLine(", NAME");

            sql.AppendLine("from");
            sql.AppendLine("SAMPLE_TABLE");

            sql.AppendLine("where 1=1");

            if (!string.IsNullOrEmpty(condition.MinimumCode))
            {
                sql.AppendLine("and CODE >= @MinimumCode");
                command.AddParameter("@MinimumCode", condition.MinimumCode);
            }

            if (!string.IsNullOrEmpty(condition.MaximumCode))
            {
                sql.AppendLine("and CODE <= @MaximumCode");
                command.AddParameter("@MaximumCode", condition.MaximumCode);
            }

            if (!string.IsNullOrEmpty(condition.NamePattern))
            {
                sql.AppendLine("and NAME like @NamePattern");
                command.AddParameter("@NamePattern", condition.NamePattern);
            }

            if (forGetCount)
            {
                sql.AppendLine(") query");
            }
            else
            {
                sql.AppendLine("order by CODE");
            }

            command.CommandText = sql.ToString();
            return command;
        }
    }
}

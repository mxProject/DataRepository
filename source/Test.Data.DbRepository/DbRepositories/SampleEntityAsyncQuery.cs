using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using mxProject.Data.Extensions;
using mxProject.Data.Repositories;
using Test.DbRepositories;

namespace Test.DbRepositories
{
    /// <summary>
    /// Provides methods to query SampleEntity from the database.
    /// </summary>
    internal class SampleEntityAsyncQuery : DbAsyncQueryBase<SampleEntity, SampleEntityCondition>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SampleEntityQuery"/> class.
        /// </summary>
        /// <param name="connectionActivator">A method to activate a connection.</param>
        /// <param name="useTransactionScope">A value that indicates whether to use ambient transactions using TransactionScope.</param>
        /// <param name="configureCommand">A method to configure a command.</param>
        public SampleEntityAsyncQuery(Func<IDbConnection> connectionActivator, bool useTransactionScope, Action<IDbCommand> configureCommand)
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
            using var command = SampleEntityQuery.CreateSelectCommand(true, commandActivator, condition);

            return Convert.ToInt32(command.ExecuteScalar());
        }

        /// <summary>
        /// Gets the count of entities that match the specified condition.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="condition">The query condition.</param>
        /// <returns>The count of entities.</returns>
        public override async ValueTask<int> GetCountAsync(Func<IDbCommand> commandActivator, SampleEntityCondition condition)
        {
            using var command = SampleEntityQuery.CreateSelectCommand(true, commandActivator, condition);

            return Convert.ToInt32(await command.ExecuteScalarAsync());
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
            using var command = SampleEntityQuery.CreateSelectCommand(false, commandActivator, condition, skipCount, maximumCount);

            using var reader = command.ExecuteReader(CommandBehavior.Default);

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
        /// Queries the entities that match the specified condition.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="condition">The query condition.</param>
        /// <param name="skipCount">The number of entities to skip.</param>
        /// <param name="maximumCount">The maximum number of entities to retrieve.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The entities that match the condition.</returns>
        public async override IAsyncEnumerable<SampleEntity> QueryAsync(Func<IDbCommand> commandActivator, SampleEntityCondition condition, int skipCount = 0, int? maximumCount = null, [EnumeratorCancellation]CancellationToken cancellationToken = default)
        {
            using var command = SampleEntityQuery.CreateSelectCommand(false, commandActivator, condition, skipCount, maximumCount);

            using var reader = await command.ExecuteReaderAsync(CommandBehavior.Default, cancellationToken);

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
    }
}

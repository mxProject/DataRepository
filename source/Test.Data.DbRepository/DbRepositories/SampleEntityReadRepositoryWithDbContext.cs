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
    /// Provides methods to read SampleEntity from the database.
    /// </summary>
    internal class SampleEntityReadRepositoryWithDbContext : DbReadRepositoryWithContextBase<SampleEntity, int, SampleDbContext>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SampleEntityReadRepositoryWithContext"/> class.
        /// </summary>
        /// <param name="connectionActivator">The method to activate a connection.</param>
        /// <param name="useTransactionScope">A value indicating whether to use ambient transactions using TransactionScope.</param>
        public SampleEntityReadRepositoryWithDbContext(Func<IDbConnection> connectionActivator, bool useTransactionScope)
            : base(connectionActivator, useTransactionScope)
        {
        }

        /// <summary>
        /// Gets the entity corresponding to the specified key.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="key">The key.</param>
        /// <param name="context">The current context.</param>
        /// <returns>The entity.</returns>
        protected override SampleEntity? Get(Func<IDbCommand> commandActivator, int key, SampleDbContext context)
        {
            return SampleEntityRepository.GetSampleEntities(commandActivator, new[] { key }).FirstOrDefault();
        }

        /// <summary>
        /// Gets the entities corresponding to the specified keys.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="keys">The keys.</param>
        /// <param name="context">The current context.</param>
        /// <returns>The entities.</returns>
        protected override IEnumerable<SampleEntity> GetRange(Func<IDbCommand> commandActivator, IEnumerable<int> keys, SampleDbContext context)
        {
            return SampleEntityRepository.GetSampleEntities(commandActivator, keys);
        }

        /// <summary>
        /// Gets all entities.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="context">The current context.</param>
        /// <returns>The entities.</returns>
        protected override IEnumerable<SampleEntity> GetAll(Func<IDbCommand> commandActivator, SampleDbContext context)
        {
            return SampleEntityRepository.GetSampleEntities(commandActivator, Array.Empty<int>());
        }

        /// <summary>
        /// Gets all keys.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="context">The current context.</param>
        /// <returns>The keys.</returns>
        protected override IEnumerable<int> GetAllKeys(Func<IDbCommand> commandActivator, SampleDbContext context)
        {
            return SampleEntityRepository.GetAllSampleEntityKeys(commandActivator);
        }
    }
}

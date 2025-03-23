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
    /// Repository class for sample entities.
    /// </summary>
    internal class SampleEntityRepositoryWithDbContext : DbReadWriteRepositoryWithContextBase<SampleEntity, int, SampleDbContext>
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="connectionActivator">The method to activate a connection.</param>
        /// <param name="useTransactionScope">A value indicating whether to use TransactionScope.</param>
        public SampleEntityRepositoryWithDbContext(Func<IDbConnection> connectionActivator, bool useTransactionScope)
            : base(connectionActivator, useTransactionScope)
        {
        }

        #region get

        /// <summary>
        /// Gets the entity corresponding to the specified key.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="key">The key.</param>
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
        /// <returns>The entities.</returns>
        protected override IEnumerable<SampleEntity> GetRange(Func<IDbCommand> commandActivator, IEnumerable<int> keys, SampleDbContext context)
        {
            return SampleEntityRepository.GetSampleEntities(commandActivator, keys);
        }

        /// <summary>
        /// Gets all entities.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <returns>The entities.</returns>
        protected override IEnumerable<SampleEntity> GetAll(Func<IDbCommand> commandActivator, SampleDbContext context)
        {
            return SampleEntityRepository.GetSampleEntities(commandActivator, Array.Empty<int>());
        }

        /// <summary>
        /// Gets all keys.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <returns>The keys.</returns>
        protected override IEnumerable<int> GetAllKeys(Func<IDbCommand> commandActivator, SampleDbContext context)
        {
            return SampleEntityRepository.GetAllSampleEntityKeys(commandActivator);
        }

        #endregion

        #region insert

        /// <summary>
        /// Inserts the entity.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="entity">The entity.</param>
        /// <returns>Number of affected rows.</returns>
        protected override int Insert(Func<IDbCommand> commandActivator, SampleEntity entity, SampleDbContext context)
        {
            return SampleEntityRepository.InsertSampleEntities(commandActivator, new[] { entity });
        }

        /// <summary>
        /// Inserts the entities.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="entities">The entities.</param>
        /// <returns>Number of affected rows.</returns>
        protected override int InsertRange(Func<IDbCommand> commandActivator, IEnumerable<SampleEntity> entities, SampleDbContext context)
        {
            return SampleEntityRepository.InsertSampleEntities(commandActivator, entities);
        }

        #endregion

        #region update  

        /// <summary>
        /// Updates the entity.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="entity">The entity.</param>
        /// <returns>Number of affected rows.</returns>
        protected override int Update(Func<IDbCommand> commandActivator, SampleEntity entity, SampleDbContext context)
        {
            return SampleEntityRepository.UpdateSampleEntities(commandActivator, new[] { entity });
        }

        /// <summary>
        /// Updates the entities.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="entities">The entities.</param>
        /// <returns>Number of affected rows.</returns>
        protected override int UpdateRange(Func<IDbCommand> commandActivator, IEnumerable<SampleEntity> entities, SampleDbContext context)
        {
            return SampleEntityRepository.UpdateSampleEntities(commandActivator, entities);
        }

        #endregion

        #region delete

        /// <summary>
        /// Deletes the entity.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="entity">The entity.</param>
        /// <returns>Number of affected rows.</returns>
        protected override int Delete(Func<IDbCommand> commandActivator, SampleEntity entity, SampleDbContext context)
        {
            return SampleEntityRepository.DeleteSampleEntities(commandActivator, new[] { entity });
        }

        /// <summary>
        /// Deletes the entities.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="entities">The entities.</param>
        /// <returns>Number of affected rows.</returns>
        protected override int DeleteRange(Func<IDbCommand> commandActivator, IEnumerable<SampleEntity> entities, SampleDbContext context)
        {
            return SampleEntityRepository.DeleteSampleEntities(commandActivator, entities);
        }

        #endregion
    }
}

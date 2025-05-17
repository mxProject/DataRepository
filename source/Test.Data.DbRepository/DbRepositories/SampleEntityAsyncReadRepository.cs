using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using mxProject.Data.Repositories;

namespace Test.DbRepositories
{
    /// <summary>
    /// Provides methods to read SampleEntity from the database.
    /// </summary>
    internal class SampleEntityAsyncReadRepository : DbAsyncReadRepositoryBase<SampleEntity, int>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SampleEntityReadRepository"/> class.
        /// </summary>
        /// <param name="connectionActivator">A method to activate a connection.</param>
        /// <param name="useTransactionScope">A value that indicates whether to use ambient transactions using TransactionScope.</param>
        /// <param name="configureCommand">A method to configure a command.</param>
        public SampleEntityAsyncReadRepository(Func<IDbConnection> connectionActivator, bool useTransactionScope, Action<IDbCommand> configureCommand)
            : base(connectionActivator, useTransactionScope, configureCommand)
        {
        }

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
        protected override IEnumerable<SampleEntity> GetRange(Func<IDbCommand> commandActivator, IEnumerable<int> keys)
        {
            return SampleEntityRepository.GetSampleEntities(commandActivator, keys);
        }

        /// <summary>
        /// Gets the entities corresponding to the specified keys.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="keys">The keys.</param>
        /// <returns>The entities.</returns>
        protected override async IAsyncEnumerable<SampleEntity> GetRangeAsync(Func<IDbCommand> commandActivator, IEnumerable<int> keys, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await Task.Yield();

            foreach(var entity in SampleEntityRepository.GetSampleEntities(commandActivator, keys))
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
        protected override async IAsyncEnumerable<SampleEntity> GetRangeAsync(Func<IDbCommand> commandActivator, IAsyncEnumerable<int> keys, [EnumeratorCancellation] CancellationToken cancellationToken = default)
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
        protected override IEnumerable<SampleEntity> GetAll(Func<IDbCommand> commandActivator)
        {
            return SampleEntityRepository.GetSampleEntities(commandActivator, Array.Empty<int>());
        }

        /// <summary>
        /// Gets all entities.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <returns>The entities.</returns>
        protected override async IAsyncEnumerable<SampleEntity> GetAllAsync(Func<IDbCommand> commandActivator, [EnumeratorCancellation] CancellationToken cancellationToken = default)
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
        protected override IEnumerable<int> GetAllKeys(Func<IDbCommand> commandActivator)
        {
            return SampleEntityRepository.GetAllSampleEntityKeys(commandActivator);
        }

        /// <summary>
        /// Gets all keys.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <returns>The keys.</returns>
        protected override async IAsyncEnumerable<int> GetAllKeysAsync(Func<IDbCommand> commandActivator, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await Task.Yield();

            foreach (var entity in SampleEntityRepository.GetAllSampleEntityKeys(commandActivator))
            {
                yield return entity;
            }
        }

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

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
    internal class SampleEntityAsyncReadRepositoryWithContext : DbAsyncReadRepositoryWithContextBase<SampleEntity, int, SampleContext>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SampleEntityReadRepositoryWithContext"/> class.
        /// </summary>
        /// <param name="connectionActivator">A method to activate a connection.</param>
        /// <param name="useTransactionScope">A value indicating whether to use ambient transactions using TransactionScope.</param>
        /// <param name="configureCommand">A method to configure a command.</param>
        public SampleEntityAsyncReadRepositoryWithContext(Func<IDbConnection> connectionActivator, bool useTransactionScope, Action<IDbCommand> configureCommand)
            : base(connectionActivator, useTransactionScope, configureCommand)
        {
        }

        #region get

        /// <summary>
        /// Gets the entity corresponding to the specified key.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="key">The key.</param>
        /// <param name="context">The current context.</param>
        /// <returns>The entity.</returns>
        protected override SampleEntity? Get(Func<IDbCommand> commandActivator, int key, SampleContext context)
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
        protected override IEnumerable<SampleEntity> GetRange(Func<IDbCommand> commandActivator, IEnumerable<int> keys, SampleContext context)
        {
            return SampleEntityRepository.GetSampleEntities(commandActivator, keys);
        }

        /// <summary>
        /// Gets all entities.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="context">The current context.</param>
        /// <returns>The entities.</returns>
        protected override IEnumerable<SampleEntity> GetAll(Func<IDbCommand> commandActivator, SampleContext context)
        {
            return SampleEntityRepository.GetSampleEntities(commandActivator, Array.Empty<int>());
        }

        /// <summary>
        /// Gets all keys.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="context">The current context.</param>
        /// <returns>The keys.</returns>
        protected override IEnumerable<int> GetAllKeys(Func<IDbCommand> commandActivator, SampleContext context)
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
        /// <param name="context">The current context.</param>
        /// <returns>The entity.</returns>
        protected override ValueTask<SampleEntity?> GetAsync(Func<IDbCommand> commandActivator, int key, SampleContext context)
        {
            return new ValueTask<SampleEntity?>(SampleEntityRepository.GetSampleEntities(commandActivator, new[] { key }).FirstOrDefault());
        }

        /// <summary>
        /// Gets the entities corresponding to the specified keys.
        /// </summary>
        /// <param name="commandActivator">The method to activate a new command.</param>
        /// <param name="keys">The keys.</param>
        /// <param name="context">The current context.</param>
        /// <returns>The entities.</returns>
        protected override async IAsyncEnumerable<SampleEntity> GetRangeAsync(Func<IDbCommand> commandActivator, IEnumerable<int> keys, SampleContext context, [EnumeratorCancellation] CancellationToken cancellationToken)
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
        /// <param name="context">The current context.</param>
        /// <returns>The entities.</returns>
        protected override async IAsyncEnumerable<SampleEntity> GetRangeAsync(Func<IDbCommand> commandActivator, IAsyncEnumerable<int> keys, SampleContext context, [EnumeratorCancellation] CancellationToken cancellationToken)
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
        /// <param name="context">The current context.</param>
        /// <returns>The entities.</returns>
        protected override async IAsyncEnumerable<SampleEntity> GetAllAsync(Func<IDbCommand> commandActivator, SampleContext context, [EnumeratorCancellation] CancellationToken cancellationToken)
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
        /// <param name="context">The current context.</param>
        /// <returns>The keys.</returns>
        protected override async IAsyncEnumerable<int> GetAllKeysAsync(Func<IDbCommand> commandActivator, SampleContext context, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            await Task.Yield();

            foreach (var entity in SampleEntityRepository.GetAllSampleEntityKeys(commandActivator))
            {
                yield return entity;
            }
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

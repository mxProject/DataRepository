using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using mxProject.Data.Repositories;

namespace Test.Caching
{
    internal class SampleAsyncReadRepositoryWithUniqueKeyWithContext : IAsyncReadDataRepositoryWithUniqueKeyWithContext<SampleEntity, int, string, SampleContext>
    {
        internal SampleAsyncReadRepositoryWithUniqueKeyWithContext(SampleDatabaseMock database)
        {
            m_Database = database;
        }

        private readonly SampleDatabaseMock m_Database;

        public bool UseTransactionScope => false;

        public void Dispose()
        {
        }

        public ValueTask<SampleEntity?> GetByPrimaryKeyAsync(int key, SampleContext context)
        {
            return ValueTask.FromResult(m_Database.GetByPrimaryKey(key));
        }

        public async IAsyncEnumerable<SampleEntity> GetRangeByPrimaryKeyAsync(IEnumerable<int> keys, SampleContext context, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            foreach (var key in keys)
            {
                await Task.Yield();

                var entity = m_Database.GetByPrimaryKey(key);

                if (entity != null) { yield return entity; }
            }
        }

        public async IAsyncEnumerable<SampleEntity> GetRangeByPrimaryKeyAsync(IAsyncEnumerable<int> keys, SampleContext context, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await foreach (var key in keys.ConfigureAwait(false))
            {
                await Task.Yield();

                var entity = m_Database.GetByPrimaryKey(key);

                if (entity != null) { yield return entity; }
            }
        }

        public ValueTask<SampleEntity?> GetByUniqueKeyAsync(string key, SampleContext context)
        {
            return ValueTask.FromResult(m_Database.GetByUniqueKey(key));
        }

        public async IAsyncEnumerable<SampleEntity> GetRangeByUniqueKeyAsync(IEnumerable<string> keys, SampleContext context, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            foreach (var key in keys)
            {
                await Task.Yield();

                var entity = m_Database.GetByUniqueKey(key);

                if (entity != null) { yield return entity; }
            }
        }

        public async IAsyncEnumerable<SampleEntity> GetRangeByUniqueKeyAsync(IAsyncEnumerable<string> keys, SampleContext context, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await foreach (var key in keys.ConfigureAwait(false))
            {
                await Task.Yield();

                var entity = m_Database.GetByUniqueKey(key);

                if (entity != null) { yield return entity; }
            }
        }

        public async IAsyncEnumerable<SampleEntity> GetAllAsync(SampleContext context, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            foreach (var entity in m_Database.GetAll())
            {
                await Task.Yield();

                yield return entity;
            }
        }

        public async IAsyncEnumerable<int> GetAllPrimaryKeysAsync(SampleContext context, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            foreach (var key in m_Database.GetAllPrimaryKeys())
            {
                await Task.Yield();

                yield return key;
            }
        }

        public async IAsyncEnumerable<string> GetAllUniqueKeysAsync(SampleContext context, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            foreach (var key in m_Database.GetAllUniqueKeys())
            {
                await Task.Yield();

                yield return key;
            }
        }
    }
}

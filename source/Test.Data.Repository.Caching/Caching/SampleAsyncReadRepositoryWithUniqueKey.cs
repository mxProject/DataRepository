using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using mxProject.Data.Repositories;

namespace Test.Caching
{
    internal class SampleAsyncReadRepositoryWithUniqueKey : IAsyncReadDataRepositoryWithUniqueKey<SampleEntity, int, string>
    {
        internal SampleAsyncReadRepositoryWithUniqueKey(SampleDatabaseMock database)
        {
            m_Database = database;
        }

        private readonly SampleDatabaseMock m_Database;

        public bool UseTransactionScope => false;

        public void Dispose()
        {
        }

        public ValueTask<SampleEntity?> GetByPrimaryKeyAsync(int key)
        {
            return ValueTask.FromResult(m_Database.GetByPrimaryKey(key));
        }

        public async IAsyncEnumerable<SampleEntity> GetRangeByPrimaryKeyAsync(IEnumerable<int> keys, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            foreach (var key in keys)
            {
                await Task.Yield();

                var entity = m_Database.GetByPrimaryKey(key);

                if (entity != null) { yield return entity; }
            }
        }

        public async IAsyncEnumerable<SampleEntity> GetRangeByPrimaryKeyAsync(IAsyncEnumerable<int> keys, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await foreach (var key in keys.ConfigureAwait(false))
            {
                await Task.Yield();

                var entity = m_Database.GetByPrimaryKey(key);

                if (entity != null) { yield return entity; }
            }
        }

        public ValueTask<SampleEntity?> GetByUniqueKeyAsync(string key)
        {
            return ValueTask.FromResult(m_Database.GetByUniqueKey(key));
        }

        public async IAsyncEnumerable<SampleEntity> GetRangeByUniqueKeyAsync(IEnumerable<string> keys, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            foreach (var key in keys)
            {
                await Task.Yield();

                var entity = m_Database.GetByUniqueKey(key);

                if (entity != null) { yield return entity; }
            }
        }

        public async IAsyncEnumerable<SampleEntity> GetRangeByUniqueKeyAsync(IAsyncEnumerable<string> keys, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await foreach (var key in keys.ConfigureAwait(false))
            {
                await Task.Yield();

                var entity = m_Database.GetByUniqueKey(key);

                if (entity != null) { yield return entity; }
            }
        }

        public async IAsyncEnumerable<SampleEntity> GetAllAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            foreach (var entity in m_Database.GetAll())
            {
                await Task.Yield();

                yield return entity;
            }
        }

        public async IAsyncEnumerable<int> GetAllPrimaryKeysAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            foreach (var key in m_Database.GetAllPrimaryKeys())
            {
                await Task.Yield();

                yield return key;
            }
        }

        public async IAsyncEnumerable<string> GetAllUniqueKeysAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            foreach (var key in m_Database.GetAllUniqueKeys())
            {
                await Task.Yield();

                yield return key;
            }
        }
    }
}

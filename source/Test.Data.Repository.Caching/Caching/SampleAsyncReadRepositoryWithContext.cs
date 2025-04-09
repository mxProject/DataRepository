using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using mxProject.Data.Repositories;

namespace Test.Caching
{
    internal class SampleAsyncReadRepositoryWithContext : IAsyncReadDataRepositoryWithContext<SampleEntity, int, SampleContext>
    {
        internal SampleAsyncReadRepositoryWithContext(SampleDatabaseMock database)
        {
            m_Database = database;
        }

        private readonly SampleDatabaseMock m_Database;

        public bool UseTransactionScope => false;

        public void Dispose()
        {
        }

        public ValueTask<SampleEntity?> GetAsync(int key, SampleContext context)
        {
            return ValueTask.FromResult(m_Database.GetByPrimaryKey(key));
        }

        public async IAsyncEnumerable<SampleEntity> GetRangeAsync(IEnumerable<int> keys, SampleContext context, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            foreach (var key in keys)
            {
                await Task.Yield();

                var entity = m_Database.GetByPrimaryKey(key);

                if (entity != null) { yield return entity; }
            }
        }

        public async IAsyncEnumerable<SampleEntity> GetRangeAsync(IAsyncEnumerable<int> keys, SampleContext context, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await foreach (var key in keys.ConfigureAwait(false))
            {
                await Task.Yield();

                var entity = m_Database.GetByPrimaryKey(key);

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

        public async IAsyncEnumerable<int> GetAllKeysAsync(SampleContext context, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            foreach (var key in m_Database.GetAllPrimaryKeys())
            {
                await Task.Yield();
                yield return key;
            }
        }
    }
}

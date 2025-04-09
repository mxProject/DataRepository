using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using mxProject.Data.Repositories;

namespace Test.Caching
{
    internal class SampleAsyncWriteRepositoryWithContext : IAsyncWriteDataRepositoryWithContext<SampleEntity, SampleContext>
    {
        internal SampleAsyncWriteRepositoryWithContext(SampleDatabaseMock database)
        {
            m_Database = database;
        }

        private readonly SampleDatabaseMock m_Database;

        public bool UseTransactionScope => false;

        public void Dispose()
        {
        }

        public ValueTask<int> InsertAsync(SampleEntity entity, SampleContext context)
        {
            m_Database.Insert(entity);

            return ValueTask.FromResult(1);
        }

        public ValueTask<int> InsertRangeAsync(IEnumerable<SampleEntity> entities, SampleContext context, CancellationToken cancellationToken = default)
        {
            int count = 0;

            foreach (var entity in entities)
            {
                m_Database.Insert(entity);
                count++;
            }

            return ValueTask.FromResult(count);
        }

        public async ValueTask<int> InsertRangeAsync(IAsyncEnumerable<SampleEntity> entities, SampleContext context, CancellationToken cancellationToken = default)
        {
            int count = 0;

            await foreach (var entity in entities)
            {
                m_Database.Insert(entity);
                count++;
            }

            return count;
        }

        public ValueTask<int> UpdateAsync(SampleEntity entity, SampleContext context)
        {
            return ValueTask.FromResult(m_Database.Update(entity) ? 1 : 0);
        }

        public ValueTask<int> UpdateRangeAsync(IEnumerable<SampleEntity> entities, SampleContext context, CancellationToken cancellationToken = default)
        {
            int count = 0;

            foreach (var entity in entities)
            {
                if (m_Database.Update(entity))
                {
                    count++;
                }
            }

            return ValueTask.FromResult(count);
        }

        public async ValueTask<int> UpdateRangeAsync(IAsyncEnumerable<SampleEntity> entities, SampleContext context, CancellationToken cancellationToken = default)
        {
            int count = 0;

            await foreach (var entity in entities)
            {
                if (m_Database.Update(entity))
                {
                    count++;
                }
            }

            return count;
        }

        public ValueTask<int> DeleteAsync(SampleEntity entity, SampleContext context)
        {
            return ValueTask.FromResult(m_Database.Delete(entity) ? 1 : 0);
        }

        public ValueTask<int> DeleteRangeAsync(IEnumerable<SampleEntity> entities, SampleContext context, CancellationToken cancellationToken = default)
        {
            int count = 0;

            foreach (var entity in entities)
            {
                if (m_Database.Delete(entity))
                {
                    count++;
                }
            }

            return ValueTask.FromResult(count);
        }

        public async ValueTask<int> DeleteRangeAsync(IAsyncEnumerable<SampleEntity> entities, SampleContext context, CancellationToken cancellationToken = default)
        {
            int count = 0;

            await foreach (var entity in entities)
            {
                if (m_Database.Delete(entity))
                {
                    count++;
                }
            }

            return count;
        }
    }
}

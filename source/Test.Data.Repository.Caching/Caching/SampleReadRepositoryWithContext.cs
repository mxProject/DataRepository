using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mxProject.Data.Repositories;

namespace Test.Caching
{
    internal class SampleReadRepositoryWithContext : IReadDataRepositoryWithContext<SampleEntity, int, SampleContext>
    {
        internal SampleReadRepositoryWithContext(SampleDatabaseMock database)
        {
            m_Database = database;
        }

        private readonly SampleDatabaseMock m_Database;

        public bool UseTransactionScope => false;

        public void Dispose()
        {
        }

        public SampleEntity? Get(int key, SampleContext context)
        {
            return m_Database.GetByPrimaryKey(key);
        }

        public IEnumerable<SampleEntity> GetRange(IEnumerable<int> keys, SampleContext context)
        {
            foreach (var key in keys)
            {
                var entity = m_Database.GetByPrimaryKey(key);

                if (entity != null) { yield return entity; }
            }
        }

        public IEnumerable<SampleEntity> GetAll(SampleContext context)
        {
            return m_Database.GetAll();
        }

        public IEnumerable<int> GetAllKeys(SampleContext context)
        {
            return m_Database.GetAllPrimaryKeys();
        }
    }
}

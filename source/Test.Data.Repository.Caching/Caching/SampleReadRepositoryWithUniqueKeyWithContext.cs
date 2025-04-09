using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mxProject.Data.Repositories;

namespace Test.Caching
{
    internal class SampleReadRepositoryWithUniqueKeyWithContext : IReadDataRepositoryWithUniqueKeyWithContext<SampleEntity, int, string, SampleContext>
    {
        internal SampleReadRepositoryWithUniqueKeyWithContext(SampleDatabaseMock database)
        {
            m_Database = database;
        }

        private readonly SampleDatabaseMock m_Database;

        public bool UseTransactionScope => false;

        public void Dispose()
        {
        }

        public SampleEntity? GetByPrimaryKey(int key, SampleContext context)
        {
            return m_Database.GetByPrimaryKey(key);
        }

        public IEnumerable<SampleEntity> GetRangeByPrimaryKey(IEnumerable<int> keys, SampleContext context)
        {
            foreach (var key in keys)
            {
                var entity = m_Database.GetByPrimaryKey(key);

                if (entity != null) { yield return entity; }
            }
        }

        public SampleEntity? GetByUniqueKey(string key, SampleContext context)
        {
            return m_Database.GetByUniqueKey(key);
        }

        public IEnumerable<SampleEntity> GetRangeByUniqueKey(IEnumerable<string> keys, SampleContext context)
        {
            foreach (var key in keys)
            {
                var entity = m_Database.GetByUniqueKey(key);

                if (entity != null) { yield return entity; }
            }
        }

        public IEnumerable<SampleEntity> GetAll(SampleContext context)
        {
            return m_Database.GetAll();
        }

        public IEnumerable<int> GetAllPrimaryKeys(SampleContext context)
        {
            return m_Database.GetAllPrimaryKeys();
        }

        public IEnumerable<string> GetAllUniqueKeys(SampleContext context)
        {
            return m_Database.GetAllUniqueKeys();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mxProject.Data.Repositories;

namespace Test.Caching
{
    internal class SampleWriteRepository : IWriteDataRepository<SampleEntity>
    {
        internal SampleWriteRepository(SampleDatabaseMock database)
        {
            m_Database = database;
        }

        private readonly SampleDatabaseMock m_Database;

        public bool UseTransactionScope => false;

        public void Dispose()
        {
        }

        public int Insert(SampleEntity entity)
        {
            m_Database.Insert(entity);

            return 1;
        }

        public int InsertRange(IEnumerable<SampleEntity> entities)
        {
            int count = 0;

            foreach (var entity in entities)
            {
                m_Database.Insert(entity);
                count++;
            }

            return count;
        }

        public int Update(SampleEntity entity)
        {
            return m_Database.Update(entity) ? 1 : 0;
        }

        public int UpdateRange(IEnumerable<SampleEntity> entities)
        {
            int count = 0;

            foreach (var entity in entities)
            {
                if (m_Database.Update(entity))
                {
                    count++;
                }
            }

            return count;
        }

        public int Delete(SampleEntity entity)
        {
            return m_Database.Delete(entity) ? 1 : 0;
        }

        public int DeleteRange(IEnumerable<SampleEntity> entities)
        {
            int count = 0;

            foreach (var entity in entities)
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

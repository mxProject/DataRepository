using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Xunit.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace Test.Caching
{
    internal class SampleDatabaseMock
    {
        internal SampleDatabaseMock(ITestOutputHelper output)
        {
            m_Output = output;
        }

        private readonly ITestOutputHelper m_Output;

        private readonly ConcurrentDictionary<int, SampleEntity> m_Entities = new();


        internal SampleEntity? GetByPrimaryKey(int id)
        {
            m_Output.WriteLine($"{nameof(SampleDatabaseMock)}.{nameof(GetByPrimaryKey)}({id})");

            if (m_Entities.TryGetValue(id, out var entity) && entity != null)
            {
                m_Output.WriteLine($"=> returns {entity}");
                return (SampleEntity)entity.Clone();
            }
            else
            {
                m_Output.WriteLine($"=> returns null");
                return default;
            }
        }

        internal SampleEntity? GetByUniqueKey(string code)
        {
            m_Output.WriteLine($"{nameof(SampleDatabaseMock)}.{nameof(GetByUniqueKey)}({code})");

            return (SampleEntity?)(GetByUniqueKeyMain(code)?.Clone());
        }

        private SampleEntity? GetByUniqueKeyMain(string code)
        {
            var entity = m_Entities.Values.FirstOrDefault(x => x.Code == code);

            if (entity != null)
            {
                m_Output.WriteLine($"=> returns {entity}");
                return entity;
            }
            else
            {
                m_Output.WriteLine($"=> returns null");
                return default;
            }
        }

        internal IEnumerable<SampleEntity> GetAll()
        {
            m_Output.WriteLine($"{nameof(SampleDatabaseMock)}.{nameof(GetAll)}()");

            return m_Entities.Values.Select(x => (SampleEntity)x.Clone());
        }

        internal IEnumerable<int> GetAllPrimaryKeys()
        {
            m_Output.WriteLine($"{nameof(SampleDatabaseMock)}.{nameof(GetAllPrimaryKeys)}()");

            return m_Entities.Keys;
        }

        internal IEnumerable<string> GetAllUniqueKeys()
        {
            m_Output.WriteLine($"{nameof(SampleDatabaseMock)}.{nameof(GetAllUniqueKeys)}()");

            return m_Entities.Values.Where(x => x.Code != null).Select(x => x.Code!).Distinct();
        }

        internal void Insert(SampleEntity entity)
        {
            m_Output.WriteLine($"{nameof(SampleDatabaseMock)}.{nameof(Insert)}({entity})");

            lock (m_Entities)
            {
                var found = GetByUniqueKeyMain(entity.Code!);

                if (found != null)
                {
                    throw new Exception($"An entity with the same unique key already exists. key = {entity.Code}");
                }

                if (!m_Entities.TryAdd(entity.ID, entity))
                {
                    throw new Exception($"An entity with the same primary key already exists. key = {entity.ID}");
                }
            }
        }

        internal bool Update(SampleEntity entity)
        {
            m_Output.WriteLine($"{nameof(SampleDatabaseMock)}.{nameof(Update)}({entity})");

            lock (m_Entities)
            {
                var found = GetByUniqueKeyMain(entity.Code!);

                if (found != null && found.ID != entity.ID)
                {
                    throw new Exception($"An entity with the same unique key already exists. key = {entity.Code}");
                }

                if (!m_Entities.TryGetValue(entity.ID, out found)) { return false; }

                return m_Entities.TryUpdate(entity.ID, entity, found);
            }
        }

        internal bool Delete(SampleEntity entity)
        {
            m_Output.WriteLine($"{nameof(SampleDatabaseMock)}.{nameof(Delete)}({entity})");

            return m_Entities.TryRemove(entity.ID, out _);
        }
    }
}

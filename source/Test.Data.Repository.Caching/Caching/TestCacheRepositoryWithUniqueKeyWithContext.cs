using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mxProject.Data.Repositories.Caching;
using mxProject.Data.Repositories;
using Xunit.Abstractions;

namespace Test.Caching
{
    /// <summary>
    /// Testing <see cref="CacheReadDataRepositoryWithUniqueKeyWithContext{TEntity, TPrimaryKey, TUniqueKey, TContext}"/> and <see cref="CacheWriteDataRepositoryWithContext{TEntity, TKey, TContext}"/>.
    /// </summary>
    [Collection("Caching")]
    public class TestCacheRepositoryWithUniqueKeyWithContext
    {
        public TestCacheRepositoryWithUniqueKeyWithContext(ITestOutputHelper output)
        {
            m_Output = output;
        }

        private readonly ITestOutputHelper m_Output;

        [Fact]
        public void AssertCacheLifetime()
        {
            var context = new SampleContext();

            var database = new SampleDatabaseMock(m_Output);

            var writeRepo = new SampleWriteRepositoryWithContext(database).WithCache(x => x.ID);
            var readRepo = new SampleReadRepositoryWithUniqueKeyWithContext(database).WithCache(x => x.ID, x => x.Code!);

            m_Output.WriteLine("----- get entity -----");

            Assert.Null(readRepo.GetByPrimaryKey(1, context));
            Assert.Null(readRepo.GetByUniqueKey("001", context));

            m_Output.WriteLine("----- insert entity -----");

            SampleEntity entity1 = new SampleEntity() { ID = 1, Code = "001", Name = "entity1" };

            writeRepo.Insert(entity1, context);

            m_Output.WriteLine("----- insert entities -----");

            writeRepo.InsertRange(
                new[] {
                    new SampleEntity() { ID = 2, Code = "002", Name = "entity2" },
                    new SampleEntity() { ID = 3, Code = "003", Name = "entity3" }
                }
                , context);

            m_Output.WriteLine("----- get entity -----");

            var found1 = readRepo.GetByPrimaryKey(1, context);

            Assert.NotNull(found1);
            Assert.Equal(entity1.Name, found1.Name);

            m_Output.WriteLine("----- get entity -----");

            // The cached entity will be returned.
            Assert.Equal(found1, readRepo.GetByPrimaryKey(1, context));

            // The cached entity will be returned.
            Assert.Equal(found1, readRepo.GetByUniqueKey("001", context));

            m_Output.WriteLine("----- get entities -----");

            // Only entity1, the cached entity will be returned.
            var founds = readRepo.GetRangeByPrimaryKey(new[] { 1, 2, 3 }, context).ToDictionary(x => x.ID);

            Assert.Equal(3, founds.Count());
            Assert.Equal(found1, founds[1]);

            var found2 = founds[2];
            var found3 = founds[3];

            m_Output.WriteLine("----- get entities -----");

            // the cached entities will be returned.
            founds = readRepo.GetRangeByUniqueKey(new[] { "001", "002", "003" }, context).ToDictionary(x => x.ID);

            Assert.Equal(3, founds.Count());
            Assert.Equal(found1, founds[1]);
            Assert.Equal(found2, founds[2]);
            Assert.Equal(found3, founds[3]);

            m_Output.WriteLine("----- update entity -----");

            entity1 = new SampleEntity() { ID = 1, Code = "001", Name = "entity-1" };

            // The cached entity will be removed from the cache.
            writeRepo.Update(entity1, context);

            m_Output.WriteLine("----- update entities -----");

            // The cached entity will be removed from the cache.
            writeRepo.UpdateRange(new[] {
                new SampleEntity() { ID = 1, Code = "001", Name = "entity-1" },
                new SampleEntity() { ID = 2, Code = "002", Name = "entity-2" }
            }
            , context);

            m_Output.WriteLine("----- get entities -----");

            // Only entity3, the cached entity will be returned.
            founds = readRepo.GetRangeByUniqueKey(new[] { "001", "002", "003" }, context).ToDictionary(x => x.ID);

            Assert.NotEqual(found1, founds[1]);
            Assert.NotEqual(found2, founds[2]);
            Assert.Equal(found3, founds[3]);

            m_Output.WriteLine("----- get entity -----");

            // The cached entity will be returned.
            Assert.Equal(founds[1], readRepo.GetByPrimaryKey(1, context));
            Assert.Equal(founds[2], readRepo.GetByPrimaryKey(2, context));
            Assert.Equal(founds[3], readRepo.GetByPrimaryKey(3, context));

            Assert.Equal(founds[1], readRepo.GetByUniqueKey("001", context));
            Assert.Equal(founds[2], readRepo.GetByUniqueKey("002", context));
            Assert.Equal(founds[3], readRepo.GetByUniqueKey("003", context));

            m_Output.WriteLine("----- delete entity -----");

            // The cached entity will be removed from the cache.
            writeRepo.Delete(entity1, context);

            m_Output.WriteLine("----- get entity -----");

            found1 = readRepo.GetByPrimaryKey(1, context);

            Assert.Null(found1);

            found1 = readRepo.GetByUniqueKey("001", context);

            Assert.Null(found1);
        }
    }
}

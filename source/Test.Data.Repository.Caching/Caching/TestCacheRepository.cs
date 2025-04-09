﻿using System;
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
    /// Testing <see cref="CacheReadDataRepository{TEntity, TKey}"/> and <see cref="CacheWriteDataRepository{TEntity, TKey}"/>.
    /// </summary>
    [Collection("Caching")]
    public class TestCacheRepository
    {
        public TestCacheRepository(ITestOutputHelper output)
        {
            m_Output = output;
        }

        private readonly ITestOutputHelper m_Output;

        [Fact]
        public void AssertCacheLifetime()
        {
            var database = new SampleDatabaseMock(m_Output);

            using var writeRepo = new SampleWriteRepository(database).WithCache(x => x.ID);
            using var readRepo = new SampleReadRepository(database).WithCache(x => x.ID);

            m_Output.WriteLine("----- get entity -----");

            Assert.Null(readRepo.Get(1));

            m_Output.WriteLine("----- insert entity -----");

            SampleEntity entity1 = new SampleEntity() { ID = 1, Code = "001", Name = "entity1" };

            writeRepo.Insert(entity1);

            m_Output.WriteLine("----- insert entities -----");

            writeRepo.InsertRange(
                new[] {
                    new SampleEntity() { ID = 2, Code = "002", Name = "entity2" },
                    new SampleEntity() { ID = 3, Code = "003", Name = "entity3" }
                });

            m_Output.WriteLine("----- get entity -----");

            var found1 = readRepo.Get(1);

            Assert.NotNull(found1);
            Assert.Equal(entity1.Name, found1.Name);

            m_Output.WriteLine("----- get entity -----");

            // The cached entity will be returned.
            Assert.Equal(found1, readRepo.Get(1));

            m_Output.WriteLine("----- get entities -----");

            // Only entity1, the cached entity will be returned.
            var founds = readRepo.GetRange(new[] { 1, 2, 3 }).ToDictionary(x => x.ID);

            Assert.Equal(3, founds.Count());
            Assert.Equal(found1, founds[1]);

            var found2 = founds[2];
            var found3 = founds[3];

            m_Output.WriteLine("----- get entities -----");

            // the cached entities will be returned.
            founds = readRepo.GetRange(new[] { 1, 2, 3 }).ToDictionary(x => x.ID);

            Assert.Equal(3, founds.Count());
            Assert.Equal(found1, founds[1]);
            Assert.Equal(found2, founds[2]);
            Assert.Equal(found3, founds[3]);

            m_Output.WriteLine("----- update entity -----");

            entity1 = new SampleEntity() { ID = 1, Code = "001", Name = "entity-1" };

            // The cached entity will be removed from the cache.
            writeRepo.Update(entity1);

            m_Output.WriteLine("----- update entities -----");

            // The cached entity will be removed from the cache.
            writeRepo.UpdateRange(new[] {
                new SampleEntity() { ID = 1, Code = "001", Name = "entity-1" },
                new SampleEntity() { ID = 2, Code = "002", Name = "entity-2" }
            });

            m_Output.WriteLine("----- get entities -----");

            // Only entity3, the cached entity will be returned.
            founds = readRepo.GetRange(new[] { 1, 2, 3 }).ToDictionary(x => x.ID);

            Assert.NotEqual(found1, founds[1]);
            Assert.NotEqual(found2, founds[2]);
            Assert.Equal(found3, founds[3]);

            m_Output.WriteLine("----- get entity -----");

            // The cached entity will be returned.
            Assert.Equal(founds[1], readRepo.Get(1));
            Assert.Equal(founds[2], readRepo.Get(2));
            Assert.Equal(founds[3], readRepo.Get(3));

            m_Output.WriteLine("----- delete entity -----");

            // The cached entity will be removed from the cache.
            writeRepo.Delete(entity1);

            m_Output.WriteLine("----- get entity -----");

            found1 = readRepo.Get(1);

            Assert.Null(found1);
        }
    }
}

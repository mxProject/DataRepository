﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mxProject.Data;
using mxProject.Data.Repositories.Caching;
using mxProject.Data.Repositories;
using Xunit.Abstractions;

namespace Test.Caching
{
    /// <summary>
    /// Testing <see cref="CacheAsyncReadDataRepositoryWithUniqueKeyWithContext{TEntity, TPrimaryKey, TUniqueKey, TContext}"/> and <see cref="CacheAsyncWriteDataRepositoryWithContext{TEntity, TKey, TContext}"/>.
    /// </summary>
    [Collection("Caching")]
    public class TestCacheAsyncRepositoryWithUniqueKeyWithContext
    {
        public TestCacheAsyncRepositoryWithUniqueKeyWithContext(ITestOutputHelper output)
        {
            m_Output = output;
        }

        private readonly ITestOutputHelper m_Output;

        [Fact]
        public async Task AssertCacheLifetimeAsync()
        {
            var context = new SampleContext();

            var database = new SampleDatabaseMock(m_Output);

            var writeRepo = new SampleAsyncWriteRepositoryWithContext(database).WithCache(x => x.ID);
            var readRepo = new SampleAsyncReadRepositoryWithUniqueKeyWithContext(database).WithCache(x => x.ID, x => x.Code!);

            m_Output.WriteLine("----- get entity -----");

            Assert.Null(await readRepo.GetByPrimaryKeyAsync(1, context));
            Assert.Null(await readRepo.GetByUniqueKeyAsync("001", context));

            m_Output.WriteLine("----- insert entity -----");

            SampleEntity entity1 = new SampleEntity() { ID = 1, Code = "001", Name = "entity1" };

            await writeRepo.InsertAsync(entity1, context);

            m_Output.WriteLine("----- insert entities -----");

            await writeRepo.InsertRangeAsync(
                new[] {
                    new SampleEntity() { ID = 2, Code = "002", Name = "entity2" },
                    new SampleEntity() { ID = 3, Code = "003", Name = "entity3" }
                }
                , context);

            m_Output.WriteLine("----- get entity -----");

            var found1 = await readRepo.GetByPrimaryKeyAsync(1, context);

            Assert.NotNull(found1);
            Assert.Equal(entity1.Name, found1.Name);

            m_Output.WriteLine("----- get entity -----");

            // The cached entity will be returned.
            Assert.Equal(found1, await readRepo.GetByPrimaryKeyAsync(1, context));

            // The cached entity will be returned.
            Assert.Equal(found1, await readRepo.GetByUniqueKeyAsync("001", context));

            m_Output.WriteLine("----- get entities -----");

            // Only entity1, the cached entity will be returned.
            var founds = (await ToEnumerableAsync(readRepo.GetRangeByPrimaryKeyAsync(new[] { 1, 2, 3 }, context))).ToDictionary(x => x.ID);

            Assert.Equal(3, founds.Count());
            Assert.Equal(found1, founds[1]);

            var found2 = founds[2];
            var found3 = founds[3];

            m_Output.WriteLine("----- get entities -----");

            // the cached entities will be returned.
            founds = (await ToEnumerableAsync(readRepo.GetRangeByUniqueKeyAsync(new[] { "001", "002", "003" }, context))).ToDictionary(x => x.ID);

            Assert.Equal(3, founds.Count());
            Assert.Equal(found1, founds[1]);
            Assert.Equal(found2, founds[2]);
            Assert.Equal(found3, founds[3]);

            m_Output.WriteLine("----- update entity -----");

            entity1 = new SampleEntity() { ID = 1, Code = "001", Name = "entity-1" };

            // The cached entity will be removed from the cache.
            await writeRepo.UpdateAsync(entity1, context);

            m_Output.WriteLine("----- update entities -----");

            // The cached entity will be removed from the cache.
            await writeRepo.UpdateRangeAsync(new[] {
                new SampleEntity() { ID = 1, Code = "001", Name = "entity-1" },
                new SampleEntity() { ID = 2, Code = "002", Name = "entity-2" }
            }
            , context);

            m_Output.WriteLine("----- get entities -----");

            // Only entity3, the cached entity will be returned.
            founds = (await ToEnumerableAsync(readRepo.GetRangeByUniqueKeyAsync(new[] { "001", "002", "003" }, context))).ToDictionary(x => x.ID);

            Assert.NotEqual(found1, founds[1]);
            Assert.NotEqual(found2, founds[2]);
            Assert.Equal(found3, founds[3]);

            m_Output.WriteLine("----- get entity -----");

            // The cached entity will be returned.
            Assert.Equal(founds[1], await readRepo.GetByPrimaryKeyAsync(1, context));
            Assert.Equal(founds[2], await readRepo.GetByPrimaryKeyAsync(2, context));
            Assert.Equal(founds[3], await readRepo.GetByPrimaryKeyAsync(3, context));

            Assert.Equal(founds[1], await readRepo.GetByUniqueKeyAsync("001", context));
            Assert.Equal(founds[2], await readRepo.GetByUniqueKeyAsync("002", context));
            Assert.Equal(founds[3], await readRepo.GetByUniqueKeyAsync("003", context));

            m_Output.WriteLine("----- delete entity -----");

            // The cached entity will be removed from the cache.
            await writeRepo.DeleteAsync(entity1, context);

            m_Output.WriteLine("----- get entity -----");

            found1 = await readRepo.GetByPrimaryKeyAsync(1, context);

            Assert.Null(found1);

            found1 = await readRepo.GetByUniqueKeyAsync("001", context);

            Assert.Null(found1);
        }

        private async ValueTask<IEnumerable<T>> ToEnumerableAsync<T>(IAsyncEnumerable<T> collection)
        {
            List<T> list = new();

            await foreach (var obj in collection)
            {
                list.Add(obj);
            }

            return list;
        }
    }
}

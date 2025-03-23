using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Xunit.Abstractions;

namespace Test.DbRepositories
{
    /// <summary>
    /// Test class for database write operations with context.
    /// </summary>
    [Collection("DbRepositories")]
    public class TestDbWriteRepositoryWithContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestDbWriteRepositoryWithContext"/> class.
        /// </summary>
        /// <param name="outputHelper">The output helper for test output.</param>
        public TestDbWriteRepositoryWithContext(ITestOutputHelper outputHelper)
        {
            m_OutputHelper = outputHelper;
        }

        private readonly ITestOutputHelper m_OutputHelper;

        /// <summary>
        /// Creates a new transaction scope.
        /// </summary>
        /// <returns>A new <see cref="TransactionScope"/> instance.</returns>
        private TransactionScope CreateTransactionScope()
        {
            return new TransactionScope(TransactionScopeOption.Required, TransactionScopeAsyncFlowOption.Enabled);
        }

        /// <summary>
        /// Creates a new sample context.
        /// </summary>
        /// <returns>A new <see cref="SampleContext"/> instance.</returns>
        private SampleContext CreateContext()
        {
            return new SampleContext();
        }

        /// <summary>
        /// Tests the insert operation.
        /// </summary>
        [Fact]
        public void Insert()
        {
            using var repo = new SampleEntityRepositoryWithContext(SampleDatabase.CreateConnection, true);

            var context = CreateContext();

            // use ambient transaction
            using var scope = CreateTransactionScope();

            var entity = new SampleEntity()
            {
                Id = 11,
                Code = "00011",
                Name = "item11"
            };

            // insert entity
            Assert.Equal(1, repo.Insert(entity, context));

            // get entity
            entity = repo.Get(entity.Id, context);

            m_OutputHelper.WriteLine($"Id: {entity!.Id}, Code: {entity.Code}, Name: {entity.Name}");

            Assert.Equal(11, entity.Id);

            // rollback transaction
            scope.Dispose();

            // get entity
            entity = repo.Get(entity.Id, context);

            Assert.Null(entity);
        }

        /// <summary>
        /// Tests the insert range operation.
        /// </summary>
        [Fact]
        public void InsertRange()
        {
            using var repo = new SampleEntityRepositoryWithContext(SampleDatabase.CreateConnection, true);

            var context = CreateContext();

            // use ambient transaction
            using var scope = CreateTransactionScope();

            var list = new List<SampleEntity>();

            list.Add(new SampleEntity()
            {
                Id = 11,
                Code = "00011",
                Name = "item11"
            });

            list.Add(new SampleEntity()
            {
                Id = 12,
                Code = "00012",
                Name = "item12"
            });

            // insert entities
            Assert.Equal(list.Count, repo.InsertRange(list, context));

            // get entities
            var entities = repo.GetRange(list.Select(x => x.Id), context).ToDictionary(x => x.Id);

            foreach (var entity in entities.Values)
            {
                m_OutputHelper.WriteLine($"Id: {entity.Id}, Code: {entity.Code}, Name: {entity.Name}");
            }

            Assert.Equal(list.Count, entities.Count);
            Assert.Equal(11, entities[11].Id);
            Assert.Equal(12, entities[12].Id);

            // rollback transaction
            scope.Dispose();

            // get entities
            entities = repo.GetRange(list.Select(x => x.Id), context).ToDictionary(x => x.Id);

            Assert.Empty(entities);
        }

        /// <summary>
        /// Tests the update operation.
        /// </summary>
        [Fact]
        public void Update()
        {
            using var repo = new SampleEntityRepositoryWithContext(SampleDatabase.CreateConnection, true);

            var context = CreateContext();

            // use ambient transaction
            using var scope = CreateTransactionScope();

            var entity = new SampleEntity()
            {
                Id = 1,
                Code = "00001",
                Name = "ITEM1"
            };

            // update entity
            Assert.Equal(1, repo.Update(entity, context));

            // get entity
            entity = repo.Get(entity.Id, context);

            m_OutputHelper.WriteLine($"Id: {entity!.Id}, Code: {entity.Code}, Name: {entity.Name}");

            Assert.Equal("ITEM1", entity.Name);

            // rollback transaction
            scope.Dispose();

            // get entity
            entity = repo.Get(entity.Id, context);

            m_OutputHelper.WriteLine($"Id: {entity!.Id}, Code: {entity.Code}, Name: {entity.Name}");

            Assert.Equal("item1", entity.Name);
        }

        /// <summary>
        /// Tests the update range operation.
        /// </summary>
        [Fact]
        public void UpdateRange()
        {
            using var repo = new SampleEntityRepositoryWithContext(SampleDatabase.CreateConnection, true);

            var context = CreateContext();

            // use ambient transaction
            using var scope = CreateTransactionScope();

            var list = new List<SampleEntity>();

            list.Add(new SampleEntity()
            {
                Id = 1,
                Code = "00001",
                Name = "ITEM1"
            });

            list.Add(new SampleEntity()
            {
                Id = 2,
                Code = "00002",
                Name = "ITEM2"
            });

            // id = -1 is not exist.
            list.Add(new SampleEntity()
            {
                Id = -1,
                Code = "-0001",
                Name = "ITEM-1"
            });

            // update entity
            Assert.Equal(2, repo.UpdateRange(list, context));

            // get entities
            var entities = repo.GetRange(list.Select(x => x.Id), context).ToDictionary(x => x.Id);

            foreach (var entity in entities.Values)
            {
                m_OutputHelper.WriteLine($"Id: {entity.Id}, Code: {entity.Code}, Name: {entity.Name}");
            }

            Assert.Equal(2, entities.Count);
            Assert.Equal("ITEM1", entities[1].Name);
            Assert.Equal("ITEM2", entities[2].Name);

            // rollback transaction
            scope.Dispose();

            // get entities
            entities = repo.GetRange(list.Select(x => x.Id), context).ToDictionary(x => x.Id);

            foreach (var entity in entities.Values)
            {
                m_OutputHelper.WriteLine($"Id: {entity.Id}, Code: {entity.Code}, Name: {entity.Name}");
            }

            Assert.Equal(2, entities.Count);
            Assert.Equal("item1", entities[1].Name);
            Assert.Equal("item2", entities[2].Name);
        }

        /// <summary>
        /// Tests the delete operation.
        /// </summary>
        [Fact]
        public void Delete()
        {
            using var repo = new SampleEntityRepositoryWithContext(SampleDatabase.CreateConnection, true);

            var context = CreateContext();

            // use ambient transaction
            using var scope = CreateTransactionScope();

            var entity = new SampleEntity()
            {
                Id = 1,
                Code = "00001",
                Name = "item1"
            };

            // delete entity
            Assert.Equal(1, repo.Delete(entity, context));

            // get entity
            Assert.Null(repo.Get(entity.Id, context));

            // rollback transaction
            scope.Dispose();

            // get entity
            entity = repo.Get(entity.Id, context);

            m_OutputHelper.WriteLine($"Id: {entity!.Id}, Code: {entity.Code}, Name: {entity.Name}");

            Assert.Equal("item1", entity.Name);
        }

        /// <summary>
        /// Tests the delete range operation.
        /// </summary>
        [Fact]
        public void DeleteRange()
        {
            using var repo = new SampleEntityRepositoryWithContext(SampleDatabase.CreateConnection, true);

            var context = CreateContext();

            // use ambient transaction
            using var scope = CreateTransactionScope();

            var list = new List<SampleEntity>();

            list.Add(new SampleEntity()
            {
                Id = 1,
                Code = "00001",
                Name = "item1"
            });

            list.Add(new SampleEntity()
            {
                Id = 2,
                Code = "00002",
                Name = "item2"
            });

            // id = -1 is not exist.
            list.Add(new SampleEntity()
            {
                Id = -1,
                Code = "-0001",
                Name = "item-1"
            });

            // delete entities
            Assert.Equal(2, repo.DeleteRange(list, context));

            // get entity
            Assert.Null(repo.Get(1, context));
            Assert.Null(repo.Get(2, context));

            // rollback transaction
            scope.Dispose();

            // get entity
            var entity = repo.Get(1, context);

            m_OutputHelper.WriteLine($"Id: {entity!.Id}, Code: {entity.Code}, Name: {entity.Name}");

            Assert.Equal("item1", entity.Name);
        }
    }
}

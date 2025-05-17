using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.ObjectModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Xunit.Abstractions;

namespace Test.DbRepositories
{
    /// <summary>
    /// Test class for database write operations with DbContext.
    /// </summary>
    [Collection("DbRepositories")]
    public class TestDbWriteRepositoryWithDbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestDbWriteRepositoryWithDbContext"/> class.
        /// </summary>
        /// <param name="outputHelper">The output helper for test output.</param>
        public TestDbWriteRepositoryWithDbContext(ITestOutputHelper outputHelper)
        {
            m_OutputHelper = outputHelper;
        }

        private readonly ITestOutputHelper m_OutputHelper;

        /// <summary>
        /// Creates a new instance of <see cref="SampleDbContext"/> using the specified connection.
        /// </summary>
        /// <param name="connection">The database connection.</param>
        /// <returns>A new instance of <see cref="SampleDbContext"/>.</returns>
        private static SampleDbContext CreateContext(IDbConnection connection)
        {
            return new SampleDbContext(connection);
        }

        /// <summary>
        /// Creates a new instance of <see cref="SampleDbContext"/> using the specified transaction.
        /// </summary>
        /// <param name="transaction">The database transaction.</param>
        /// <returns>A new instance of <see cref="SampleDbContext"/>.</returns>
        private static SampleDbContext CreateContext(IDbTransaction transaction)
        {
            return new SampleDbContext(transaction);
        }

        /// <summary>
        /// Tests inserting an entity specifying a connection.
        /// </summary>
        [Fact]
        public void Insert_SpecifyingConnection()
        {
            using var repo = new SampleEntityRepositoryWithDbContext(SampleDatabase.CreateConnection, true, SampleDatabase.ConfigureCommand);

            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            var context = CreateContext(connection);

            var entity = new SampleEntity()
            {
                Id = 11,
                Code = "00011",
                Name = "item11"
            };

            try
            {
                // insert entity
                Assert.Equal(1, repo.Insert(entity, context));

                // get entity
                var found = repo.Get(entity.Id, context);

                m_OutputHelper.WriteLine($"Id: {found!.Id}, Code: {found.Code}, Name: {found.Name}");

                Assert.Equal(11, entity.Id);
            }
            finally
            {
                repo.Delete(entity, context);
            }
        }

        /// <summary>
        /// Tests inserting an entity specifying a transaction.
        /// </summary>
        [Fact]
        public void Insert_SpecifyingTransaction()
        {
            using var repo = new SampleEntityRepositoryWithDbContext(SampleDatabase.CreateConnection, true, SampleDatabase.ConfigureCommand);

            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            // begin transaction
            using var transaction = connection.BeginTransaction();

            var context = CreateContext(transaction);

            var entity = new SampleEntity()
            {
                Id = 11,
                Code = "00011",
                Name = "item11"
            };

            // insert entity
            Assert.Equal(1, repo.Insert(entity, context));

            // get entity
            var found = repo.Get(entity.Id, context);

            m_OutputHelper.WriteLine($"Id: {found!.Id}, Code: {found.Code}, Name: {found.Name}");

            Assert.Equal(11, entity.Id);

            // rollback transaction
            transaction.Rollback();

            // get entity
            Assert.Null(repo.Get(entity.Id, context));
        }

        /// <summary>
        /// Tests inserting a range of entities specifying a connection.
        /// </summary>
        [Fact]
        public void InsertRange_SpecifyingConnection()
        {
            using var repo = new SampleEntityRepositoryWithDbContext(SampleDatabase.CreateConnection, true, SampleDatabase.ConfigureCommand);

            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            var context = CreateContext(connection);

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

            try
            {
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
            }
            finally
            {
                repo.DeleteRange(list, context);
            }
        }

        /// <summary>
        /// Tests inserting a range of entities specifying a transaction.
        /// </summary>
        [Fact]
        public void InsertRange_SpecifyingTransaction()
        {
            using var repo = new SampleEntityRepositoryWithDbContext(SampleDatabase.CreateConnection, true, SampleDatabase.ConfigureCommand);

            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            // begin transaction
            using var transaction = connection.BeginTransaction();

            var context = CreateContext(transaction);

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
            transaction.Rollback();

            // get entity
            Assert.Null(repo.Get(11, context));
            Assert.Null(repo.Get(12, context));
        }

        /// <summary>
        /// Tests updating an entity specifying a connection.
        /// </summary>
        [Fact]
        public void Update_SpecifyingConnection()
        {
            using var repo = new SampleEntityRepositoryWithDbContext(SampleDatabase.CreateConnection, true, SampleDatabase.ConfigureCommand);

            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            var context = CreateContext(connection);

            var entity = new SampleEntity()
            {
                Id = 1,
                Code = "00001",
                Name = "ITEM1"
            };

            // update entity
            Assert.Equal(1, repo.Update(entity, context));

            try
            {
                // get entity
                var found = repo.Get(entity.Id, context);

                m_OutputHelper.WriteLine($"Id: {found!.Id}, Code: {found.Code}, Name: {found.Name}");

                Assert.Equal("ITEM1", found.Name);
            }
            finally
            {
                entity.Name = "item1";
                repo.Update(entity, context);
            }
        }

        /// <summary>
        /// Tests updating an entity specifying a transaction.
        /// </summary>
        [Fact]
        public void Update_SpecifyingTransaction()
        {
            using var repo = new SampleEntityRepositoryWithDbContext(SampleDatabase.CreateConnection, true, SampleDatabase.ConfigureCommand);

            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            // begin transaction
            using var transaction = connection.BeginTransaction();

            var context = CreateContext(transaction);

            var entity = new SampleEntity()
            {
                Id = 1,
                Code = "00001",
                Name = "ITEM1"
            };

            // update entity
            Assert.Equal(1, repo.Update(entity, context));

            // get entity
            var found = repo.Get(entity.Id, context);

            m_OutputHelper.WriteLine($"Id: {found!.Id}, Code: {found.Code}, Name: {found.Name}");

            Assert.Equal("ITEM1", found.Name);

            // rollback transaction
            transaction.Rollback();

            // get entity
            found = repo.Get(entity.Id, context);

            m_OutputHelper.WriteLine($"Id: {found!.Id}, Code: {found.Code}, Name: {found.Name}");

            Assert.Equal("item1", found.Name);
        }

        /// <summary>
        /// Tests updating a range of entities specifying a connection.
        /// </summary>
        [Fact]
        public void UpdateRange_SpecifyingConnection()
        {
            using var repo = new SampleEntityRepositoryWithDbContext(SampleDatabase.CreateConnection, true, SampleDatabase.ConfigureCommand);

            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            var context = CreateContext(connection);

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

            try
            {
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
            }
            finally
            {
                list[0].Name = "item1";
                list[1].Name = "item2";
                repo.UpdateRange(list, context);
            }
        }

        /// <summary>
        /// Tests updating a range of entities specifying a transaction.
        /// </summary>
        [Fact]
        public void UpdateRange_SpecifyingTransaction()
        {
            using var repo = new SampleEntityRepositoryWithDbContext(SampleDatabase.CreateConnection, true, SampleDatabase.ConfigureCommand);

            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            // begin transaction
            using var transaction = connection.BeginTransaction();

            var context = CreateContext(transaction);

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
            transaction.Rollback();

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
        /// Tests deleting an entity specifying a connection.
        /// </summary>
        [Fact]
        public void Delete_SpecifyingConnection()
        {
            using var repo = new SampleEntityRepositoryWithDbContext(SampleDatabase.CreateConnection, true, SampleDatabase.ConfigureCommand);

            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            var context = CreateContext(connection);

            var entity = new SampleEntity()
            {
                Id = 1,
                Code = "00001",
                Name = "item1"
            };

            try
            {
                // delete entity
                Assert.Equal(1, repo.Delete(entity, context));

                // get entity
                Assert.Null(repo.Get(entity.Id, context));
            }
            finally
            {
                repo.Insert(entity, context);
            }
        }

        /// <summary>
        /// Tests deleting an entity specifying a transaction.
        /// </summary>
        [Fact]
        public void Delete_SpecifyingTransaction()
        {
            using var repo = new SampleEntityRepositoryWithDbContext(SampleDatabase.CreateConnection, true, SampleDatabase.ConfigureCommand);

            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            // begin transaction
            using var transaction = connection.BeginTransaction();

            var context = CreateContext(transaction);

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
            transaction.Rollback();

            // get entity
            Assert.NotNull(repo.Get(entity.Id, context));
        }

        /// <summary>
        /// Tests deleting a range of entities specifying a connection.
        /// </summary>
        [Fact]
        public void DeleteRange_SpecifyingConnection()
        {
            using var repo = new SampleEntityRepositoryWithDbContext(SampleDatabase.CreateConnection, true, SampleDatabase.ConfigureCommand);

            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            var context = CreateContext(connection);

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

            try
            {
                // delete entities
                Assert.Equal(2, repo.DeleteRange(list, context));

                // get entity
                Assert.Null(repo.Get(1, context));
                Assert.Null(repo.Get(2, context));
            }
            finally
            {
                repo.InsertRange(list.Take(2), context);
            }
        }

        /// <summary>
        /// Tests deleting a range of entities specifying a transaction.
        /// </summary>
        [Fact]
        public void DeleteRange_SpecifyingTransaction()
        {
            using var repo = new SampleEntityRepositoryWithDbContext(SampleDatabase.CreateConnection, true, SampleDatabase.ConfigureCommand);

            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            // begin transaction
            using var transaction = connection.BeginTransaction();

            var context = CreateContext(transaction);

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
            transaction.Rollback();

            // get entity
            Assert.NotNull(repo.Get(1, context));
            Assert.NotNull(repo.Get(2, context));
        }
    }
}

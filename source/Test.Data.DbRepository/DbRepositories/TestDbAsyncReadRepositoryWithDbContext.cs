﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Test.DbRepositories
{
    /// <summary>
    /// Repository for reading data from the database.
    /// </summary>
    [Collection("DbRepositories")]
    public class TestDbAsyncReadRepositoryWithDbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestDbReadRepositoryWithDbContext"/> class.
        /// </summary>
        /// <param name="outputHelper">The output helper for test output.</param>
        public TestDbAsyncReadRepositoryWithDbContext(ITestOutputHelper outputHelper)
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

        #region Get

        /// <summary>
        /// Tests the Get method of the repository using a specified connection.
        /// </summary>
        [Fact]
        public void Get_SpecifyingConnection()
        {
            using var repo = new SampleEntityAsyncReadRepositoryWithDbContext(null!, true, SampleDatabase.ConfigureCommand);

            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            var context = CreateContext(connection);

            var entity = repo.Get(1, context);

            m_OutputHelper.WriteLine($"Id: {entity!.Id}, Code: {entity.Code}, Name: {entity.Name}");

            Assert.NotNull(entity);
            Assert.Equal(1, entity.Id);
            Assert.Equal("00001", entity.Code);
            Assert.Equal("item1", entity.Name);
        }

        /// <summary>
        /// Tests the Get method of the repository using a specified connection.
        /// </summary>
        [Fact]
        public async Task GetAsync_SpecifyingConnection()
        {
            using var repo = new SampleEntityAsyncReadRepositoryWithDbContext(null!, true, SampleDatabase.ConfigureCommand);

            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            var context = CreateContext(connection);

            var entity = await repo.GetAsync(1, context);

            m_OutputHelper.WriteLine($"Id: {entity!.Id}, Code: {entity.Code}, Name: {entity.Name}");

            Assert.NotNull(entity);
            Assert.Equal(1, entity.Id);
            Assert.Equal("00001", entity.Code);
            Assert.Equal("item1", entity.Name);
        }

        /// <summary>
        /// Tests the Get method of the repository using a specified transaction.
        /// </summary>
        [Fact]
        public void Get_SpecifyingTransaction()
        {
            using var repo = new SampleEntityAsyncReadRepositoryWithDbContext(null!, true, SampleDatabase.ConfigureCommand);

            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            using var transaction = connection.BeginTransaction();

            var context = CreateContext(transaction);

            var entity = repo.Get(1, context);

            m_OutputHelper.WriteLine($"Id: {entity!.Id}, Code: {entity.Code}, Name: {entity.Name}");

            Assert.NotNull(entity);
            Assert.Equal(1, entity.Id);
            Assert.Equal("00001", entity.Code);
            Assert.Equal("item1", entity.Name);
        }

        /// <summary>
        /// Tests the Get method of the repository using a specified transaction.
        /// </summary>
        [Fact]
        public async Task GetAsync_SpecifyingTransaction()
        {
            using var repo = new SampleEntityAsyncReadRepositoryWithDbContext(null!, true, SampleDatabase.ConfigureCommand);

            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            using var transaction = connection.BeginTransaction();

            var context = CreateContext(transaction);

            var entity = await repo.GetAsync(1, context);

            m_OutputHelper.WriteLine($"Id: {entity!.Id}, Code: {entity.Code}, Name: {entity.Name}");

            Assert.NotNull(entity);
            Assert.Equal(1, entity.Id);
            Assert.Equal("00001", entity.Code);
            Assert.Equal("item1", entity.Name);
        }

        #endregion

        #region GetRange

        /// <summary>
        /// Tests the GetRange method of the repository using a specified connection.
        /// </summary>
        [Fact]
        public void GetRange_SpecifyingConnection()
        {
            using var repo = new SampleEntityAsyncReadRepositoryWithDbContext(null!, true, SampleDatabase.ConfigureCommand);

            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            var context = CreateContext(connection);

            // id = -1 is not exist.
            var entities = repo.GetRange([4, 2, -1], context).ToArray();

            foreach (var entity in entities)
            {
                m_OutputHelper.WriteLine($"Id: {entity.Id}, Code: {entity.Code}, Name: {entity.Name}");
            }

            Assert.NotNull(entities);
            Assert.Equal(2, entities.Length);
        }

        /// <summary>
        /// Tests the GetRange method of the repository using a specified connection.
        /// </summary>
        [Fact]
        public async Task GetRangeAsync_SpecifyingConnection()
        {
            using var repo = new SampleEntityAsyncReadRepositoryWithDbContext(null!, true, SampleDatabase.ConfigureCommand);

            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            var context = CreateContext(connection);

            // id = -1 is not exist.
            var entities = repo.GetRangeAsync([4, 2, -1], context);

            int count = 0;

            await foreach (var entity in entities)
            {
                count++;
                m_OutputHelper.WriteLine($"Id: {entity.Id}, Code: {entity.Code}, Name: {entity.Name}");
            }

            Assert.NotNull(entities);
            Assert.Equal(2, count);
        }

        /// <summary>
        /// Tests the GetRange method of the repository using a specified transaction.
        /// </summary>
        [Fact]
        public void GetRange_SpecifyingTransaction()
        {
            using var repo = new SampleEntityAsyncReadRepositoryWithDbContext(null!, true, SampleDatabase.ConfigureCommand);

            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            using var transaction = connection.BeginTransaction();

            var context = CreateContext(transaction);

            // id = -1 is not exist.
            var entities = repo.GetRange([4, 2, -1], context).ToArray();

            foreach (var entity in entities)
            {
                m_OutputHelper.WriteLine($"Id: {entity.Id}, Code: {entity.Code}, Name: {entity.Name}");
            }

            Assert.NotNull(entities);
            Assert.Equal(2, entities.Length);
        }

        /// <summary>
        /// Tests the GetRange method of the repository using a specified transaction.
        /// </summary>
        [Fact]
        public async Task GetRangeAsync_SpecifyingTransaction()
        {
            using var repo = new SampleEntityAsyncReadRepositoryWithDbContext(null!, true, SampleDatabase.ConfigureCommand);

            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            using var transaction = connection.BeginTransaction();

            var context = CreateContext(transaction);

            // id = -1 is not exist.
            var entities = repo.GetRangeAsync([4, 2, -1], context);

            int count = 0;

            await foreach (var entity in entities)
            {
                count++;
                m_OutputHelper.WriteLine($"Id: {entity.Id}, Code: {entity.Code}, Name: {entity.Name}");
            }

            Assert.NotNull(entities);
            Assert.Equal(2, count);
        }

        #endregion

        #region GetAll

        /// <summary>
        /// Tests the GetAll method of the repository using a specified connection.
        /// </summary>
        [Fact]
        public void GetAll_SpecifyingConnection()
        {
            using var repo = new SampleEntityAsyncReadRepositoryWithDbContext(null!, true, SampleDatabase.ConfigureCommand);

            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            var context = CreateContext(connection);

            var entities = repo.GetAll(context).ToArray();

            foreach (var entity in entities)
            {
                m_OutputHelper.WriteLine($"Id: {entity.Id}, Code: {entity.Code}, Name: {entity.Name}");
            }
        }

        /// <summary>
        /// Tests the GetAll method of the repository using a specified connection.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_SpecifyingConnection()
        {
            using var repo = new SampleEntityAsyncReadRepositoryWithDbContext(null!, true, SampleDatabase.ConfigureCommand);

            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            var context = CreateContext(connection);

            var entities = repo.GetAllAsync(context);

            await foreach (var entity in entities)
            {
                m_OutputHelper.WriteLine($"Id: {entity.Id}, Code: {entity.Code}, Name: {entity.Name}");
            }
        }

        /// <summary>
        /// Tests the GetAll method of the repository using a specified transaction.
        /// </summary>
        [Fact]
        public void GetAll_SpecifyingTransaction()
        {
            using var repo = new SampleEntityAsyncReadRepositoryWithDbContext(null!, true, SampleDatabase.ConfigureCommand);

            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            using var transaction = connection.BeginTransaction();

            var context = CreateContext(transaction);

            var entities = repo.GetAll(context).ToArray();

            foreach (var entity in entities)
            {
                m_OutputHelper.WriteLine($"Id: {entity.Id}, Code: {entity.Code}, Name: {entity.Name}");
            }
        }

        /// <summary>
        /// Tests the GetAll method of the repository using a specified transaction.
        /// </summary>
        [Fact]
        public async Task GetAllAsync_SpecifyingTransaction()
        {
            using var repo = new SampleEntityAsyncReadRepositoryWithDbContext(null!, true, SampleDatabase.ConfigureCommand);

            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            using var transaction = connection.BeginTransaction();

            var context = CreateContext(transaction);

            var entities = repo.GetAllAsync(context);

            await foreach (var entity in entities)
            {
                m_OutputHelper.WriteLine($"Id: {entity.Id}, Code: {entity.Code}, Name: {entity.Name}");
            }
        }

        #endregion

        #region GetAllKeys

        /// <summary>
        /// Tests the GetAllKeys method of the repository using a specified connection.
        /// </summary>
        [Fact]
        public void GetAllKeys_SpecifyingConnection()
        {
            using var repo = new SampleEntityAsyncReadRepositoryWithDbContext(null!, true, SampleDatabase.ConfigureCommand);

            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            var context = CreateContext(connection);

            var keys = repo.GetAllKeys(context).ToArray();

            foreach (var key in keys)
            {
                m_OutputHelper.WriteLine($"Id: {key}");
            }
        }

        /// <summary>
        /// Tests the GetAllKeys method of the repository using a specified connection.
        /// </summary>
        [Fact]
        public async Task GetAllKeysAsync_SpecifyingConnection()
        {
            using var repo = new SampleEntityAsyncReadRepositoryWithDbContext(null!, true, SampleDatabase.ConfigureCommand);

            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            var context = CreateContext(connection);

            var keys = repo.GetAllKeysAsync(context);

            await foreach (var key in keys)
            {
                m_OutputHelper.WriteLine($"Id: {key}");
            }
        }

        /// <summary>
        /// Tests the GetAllKeys method of the repository using a specified transaction.
        /// </summary>
        [Fact]
        public void GetAllKeys_SpecifyingTransaction()
        {
            using var repo = new SampleEntityAsyncReadRepositoryWithDbContext(null!, true, SampleDatabase.ConfigureCommand);

            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            using var transaction = connection.BeginTransaction();

            var context = CreateContext(transaction);

            var keys = repo.GetAllKeys(context).ToArray();

            foreach (var key in keys)
            {
                m_OutputHelper.WriteLine($"Id: {key}");
            }
        }

        /// <summary>
        /// Tests the GetAllKeys method of the repository using a specified transaction.
        /// </summary>
        [Fact]
        public async Task GetAllKeysAsync_SpecifyingTransaction()
        {
            using var repo = new SampleEntityAsyncReadRepositoryWithDbContext(null!, true, SampleDatabase.ConfigureCommand);

            using var connection = SampleDatabase.CreateConnection();

            connection.Open();

            using var transaction = connection.BeginTransaction();

            var context = CreateContext(transaction);

            var keys = repo.GetAllKeysAsync(context);

            await foreach (var key in keys)
            {
                m_OutputHelper.WriteLine($"Id: {key}");
            }
        }

        #endregion
    }
}

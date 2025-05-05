using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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
    public class TestDbAsyncReadRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestDbReadRepository"/> class.
        /// </summary>
        /// <param name="outputHelper">The output helper for test output.</param>
        public TestDbAsyncReadRepository(ITestOutputHelper outputHelper)
        {
            m_OutputHelper = outputHelper;
        }

        private readonly ITestOutputHelper m_OutputHelper;

        #region Get

        /// <summary>
        /// Tests the Get method of the repository.
        /// </summary>
        [Fact]
        public void Get()
        {
            using var repo = new SampleEntityAsyncReadRepository(SampleDatabase.CreateConnection, true, SampleDatabase.ConfigureCommand);

            var entity = repo.Get(1);

            m_OutputHelper.WriteLine($"Id: {entity!.Id}, Code: {entity.Code}, Name: {entity.Name}");

            Assert.NotNull(entity);
            Assert.Equal(1, entity.Id);
            Assert.Equal("00001", entity.Code);
            Assert.Equal("item1", entity.Name);
        }

        /// <summary>
        /// Tests the Get method of the repository.
        /// </summary>
        [Fact]
        public async Task GetAsync()
        {
            using var repo = new SampleEntityAsyncReadRepository(SampleDatabase.CreateConnection, true, SampleDatabase.ConfigureCommand);

            var entity = await repo.GetAsync(1);

            m_OutputHelper.WriteLine($"Id: {entity!.Id}, Code: {entity.Code}, Name: {entity.Name}");

            Assert.NotNull(entity);
            Assert.Equal(1, entity.Id);
            Assert.Equal("00001", entity.Code);
            Assert.Equal("item1", entity.Name);
        }

        #endregion

        #region GetRange

        /// <summary>
        /// Tests the GetRange method of the repository.
        /// </summary>
        [Fact]
        public void GetRange()
        {
            using var repo = new SampleEntityAsyncReadRepository(SampleDatabase.CreateConnection, true, SampleDatabase.ConfigureCommand);

            // id = -1 is not exist.
            var entities = repo.GetRange([4, 2, -1]).ToArray();

            foreach (var entity in entities)
            {
                m_OutputHelper.WriteLine($"Id: {entity.Id}, Code: {entity.Code}, Name: {entity.Name}");
            }

            Assert.NotNull(entities);
            Assert.Equal(2, entities.Length);
        }

        /// <summary>
        /// Tests the GetRange method of the repository.
        /// </summary>
        [Fact]
        public async Task GetRangeAsync()
        {
            using var repo = new SampleEntityAsyncReadRepository(SampleDatabase.CreateConnection, true, SampleDatabase.ConfigureCommand);

            // id = -1 is not exist.
            var entities = repo.GetRangeAsync([4, 2, -1]);

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
        /// Tests the GetAll method of the repository.
        /// </summary>
        [Fact]
        public void GetAll()
        {
            using var repo = new SampleEntityAsyncReadRepository(SampleDatabase.CreateConnection, true, SampleDatabase.ConfigureCommand);

            var entities = repo.GetAll().ToArray();

            foreach (var entity in entities)
            {
                m_OutputHelper.WriteLine($"Id: {entity.Id}, Code: {entity.Code}, Name: {entity.Name}");
            }
        }

        /// <summary>
        /// Tests the GetAll method of the repository.
        /// </summary>
        [Fact]
        public async Task GetAllAsync()
        {
            using var repo = new SampleEntityAsyncReadRepository(SampleDatabase.CreateConnection, true, SampleDatabase.ConfigureCommand);

            var entities = repo.GetAllAsync();

            await foreach (var entity in entities)
            {
                m_OutputHelper.WriteLine($"Id: {entity.Id}, Code: {entity.Code}, Name: {entity.Name}");
            }
        }

        #endregion

        #region GetAllKeys

        /// <summary>
        /// Tests the GetAllKeys method of the repository.
        /// </summary>
        [Fact]
        public void GetAllKeys()
        {
            using var repo = new SampleEntityAsyncReadRepository(SampleDatabase.CreateConnection, true, SampleDatabase.ConfigureCommand);

            var keys = repo.GetAllKeys().ToArray();

            foreach (var key in keys)
            {
                m_OutputHelper.WriteLine($"Id: {key}");
            }
        }

        /// <summary>
        /// Tests the GetAllKeys method of the repository.
        /// </summary>
        [Fact]
        public async Task GetAllKeysAsync()
        {
            using var repo = new SampleEntityAsyncReadRepository(SampleDatabase.CreateConnection, true, SampleDatabase.ConfigureCommand);

            var keys = repo.GetAllKeysAsync();

            await foreach (var key in keys)
            {
                m_OutputHelper.WriteLine($"Id: {key}");
            }
        }

        #endregion
    }
}

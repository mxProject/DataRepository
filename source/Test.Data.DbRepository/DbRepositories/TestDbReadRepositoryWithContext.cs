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
    public class TestDbReadRepositoryWithContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestDbReadRepositoryWithContext"/> class.
        /// </summary>
        /// <param name="outputHelper">The output helper for test output.</param>
        public TestDbReadRepositoryWithContext(ITestOutputHelper outputHelper)
        {
            m_OutputHelper = outputHelper;
        }

        private readonly ITestOutputHelper m_OutputHelper;

        /// <summary>
        /// Creates a new instance of the <see cref="SampleContext"/> class.
        /// </summary>
        /// <returns>A new <see cref="SampleContext"/> instance.</returns>
        private SampleContext CreateContext()
        {
            return new SampleContext();
        }

        /// <summary>
        /// Tests the Get method of the repository.
        /// </summary>
        [Fact]
        public void Get()
        {
            using var repo = new SampleEntityReadRepositoryWithContext(SampleDatabase.CreateConnection, true);

            var context = CreateContext();

            var entity = repo.Get(1, context);

            m_OutputHelper.WriteLine($"Id: {entity!.Id}, Code: {entity.Code}, Name: {entity.Name}");

            Assert.NotNull(entity);
            Assert.Equal(1, entity.Id);
            Assert.Equal("00001", entity.Code);
            Assert.Equal("item1", entity.Name);
        }

        /// <summary>
        /// Tests the GetRange method of the repository.
        /// </summary>
        [Fact]
        public void GetRange()
        {
            using var repo = new SampleEntityReadRepositoryWithContext(SampleDatabase.CreateConnection, true);

            var context = CreateContext();

            // id = -1 does not exist.
            var entities = repo.GetRange(new[] { 4, 2, -1 }, context).ToArray();

            foreach (var entity in entities)
            {
                m_OutputHelper.WriteLine($"Id: {entity.Id}, Code: {entity.Code}, Name: {entity.Name}");
            }

            Assert.NotNull(entities);
            Assert.Equal(2, entities.Count());
        }

        /// <summary>
        /// Tests the GetAll method of the repository.
        /// </summary>
        [Fact]
        public void GetAll()
        {
            using var repo = new SampleEntityReadRepositoryWithContext(SampleDatabase.CreateConnection, true);

            var context = CreateContext();

            var entities = repo.GetAll(context).ToArray();

            foreach (var entity in entities)
            {
                m_OutputHelper.WriteLine($"Id: {entity.Id}, Code: {entity.Code}, Name: {entity.Name}");
            }
        }

        /// <summary>
        /// Tests the GetAllKeys method of the repository.
        /// </summary>
        [Fact]
        public void GetAllKeys()
        {
            using var repo = new SampleEntityReadRepositoryWithContext(SampleDatabase.CreateConnection, true);

            var context = CreateContext();

            var keys = repo.GetAllKeys(context).ToArray();

            foreach (var key in keys)
            {
                m_OutputHelper.WriteLine($"Id: {key}");
            }
        }
    }
}

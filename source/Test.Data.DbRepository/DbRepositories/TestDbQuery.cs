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
    public class TestDbQuery
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestDbQuery"/> class.
        /// </summary>
        /// <param name="outputHelper">The output helper for test output.</param>
        public TestDbQuery(ITestOutputHelper outputHelper)
        {
            m_OutputHelper = outputHelper;
        }

        private readonly ITestOutputHelper m_OutputHelper;

        /// <summary>
        /// Tests the GetCount method of the repository.
        /// </summary>
        [Fact]
        public void GetCount()
        {
            using var repo = new SampleEntityQuery(SampleDatabase.CreateConnection, true, SampleDatabase.ConfigureCommand);

            var condition = new SampleEntityCondition()
            {
                MinimumCode = "00002",
                MaximumCode = "00004",
            };

            var count = repo.GetCount(condition);

            Assert.Equal(3, count);
        }

        /// <summary>
        /// Tests the Query method of the repository.
        /// </summary>
        [Fact]
        public void Query()
        {
            using var repo = new SampleEntityQuery(SampleDatabase.CreateConnection, true, SampleDatabase.ConfigureCommand);

            var condition = new SampleEntityCondition()
            {
                MinimumCode = "00002",
                MaximumCode = "00004",
            };

            var entities = repo.Query(condition).ToArray();

            foreach (var entity in entities)
            {
                m_OutputHelper.WriteLine($"Id: {entity.Id}, Code: {entity.Code}, Name: {entity.Name}");
            }

            Assert.Equal(3, entities.Length);
        }
    }
}

﻿using System;
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
    public class TestDbReadRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestDbReadRepository"/> class.
        /// </summary>
        /// <param name="outputHelper">The output helper for test output.</param>
        public TestDbReadRepository(ITestOutputHelper outputHelper)
        {
            m_OutputHelper = outputHelper;
        }

        private readonly ITestOutputHelper m_OutputHelper;

        /// <summary>
        /// Tests the Get method of the repository.
        /// </summary>
        [Fact]
        public void Get()
        {
            using var repo = new SampleEntityReadRepository(SampleDatabase.CreateConnection, true, SampleDatabase.ConfigureCommand);

            var entity = repo.Get(1);

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
            using var repo = new SampleEntityReadRepository(SampleDatabase.CreateConnection, true, SampleDatabase.ConfigureCommand);

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
        /// Tests the GetAll method of the repository.
        /// </summary>
        [Fact]
        public void GetAll()
        {
            using var repo = new SampleEntityReadRepository(SampleDatabase.CreateConnection, true, SampleDatabase.ConfigureCommand);

            var entities = repo.GetAll().ToArray();

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
            using var repo = new SampleEntityReadRepository(SampleDatabase.CreateConnection, true, SampleDatabase.ConfigureCommand);

            var keys = repo.GetAllKeys().ToArray();

            foreach (var key in keys)
            {
                m_OutputHelper.WriteLine($"Id: {key}");
            }
        }
    }
}

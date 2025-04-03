using Xunit.Abstractions;
using System.Transactions;
using mxProject.Data.Repositories;
using mxProject.Data.Repositories.FileManagers;

namespace Test.FileRepositories
{
    /// <summary>
    /// Test class for FileRepositoryUseKeyFile.
    /// </summary>
    public class TestFileRepositoryUseKeyFile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestFileRepositoryUseKeyFile"/> class.
        /// </summary>
        /// <param name="output">The test output helper.</param>
        public TestFileRepositoryUseKeyFile(ITestOutputHelper output)
        {
            m_Output = output;
        }

        private readonly ITestOutputHelper m_Output;

        /// <summary>
        /// Tests the Insert and Get methods of the repository.
        /// </summary>
        [Theory]
        [MemberData(nameof(InsertAndGet_Arguments))]
        public void InsertAndGet(IFileManager fileManager)
        {
            string dataDirectory = Path.Combine(@$"TestOutput\{nameof(TestFileRepositoryUseKeyFile)}\{nameof(InsertAndGet)}_{fileManager.GetType().Name}");

            if (Directory.Exists(dataDirectory))
            {
                Directory.Delete(dataDirectory, true);
            }

            Directory.CreateDirectory(dataDirectory);

            ISampleEntityRepository repository = new SampleEntityRepositoryUsingKeyFile(dataDirectory, fileManager);

            List<SampleEntity> entities =
            [
                new SampleEntity() { ID = Guid.NewGuid(), Code = 123, Name = "abc" },
                new SampleEntity() { ID = Guid.NewGuid(), Code = 456, Name = "xyz" },
                new SampleEntity() { ID = Guid.NewGuid(), Code = 789, Name = "123" },
            ];

            repository.InsertRange(entities);

            repository.Update(entities[0]);

            repository.Delete(entities[1]);

            m_Output.WriteLine("----- Get -----");

            var entity = repository.Get(entities[0].ID!.Value);

            m_Output.WriteLine($"{entity!.ID}, {entity.Code}, {entity.Name}");

            Assert.Equal(entities[0].Name, entity.Name);

            entity = repository.Get(entities[1].ID!.Value);

            Assert.Null(entity);

            m_Output.WriteLine("----- GetAll -----");

            var allEntities = repository.GetAll().ToArray();

            foreach (var e in allEntities)
            {
                m_Output.WriteLine($"{e.ID}, {e.Code}, {e.Name}");
            }

            Assert.Equal(2, allEntities.Length);

            m_Output.WriteLine("----- GetAllKeys -----");

            var allKeys = repository.GetAllKeys().ToArray();

            foreach (var key in allKeys)
            {
                m_Output.WriteLine($"{key}");
            }

            Assert.Equal(2, allKeys.Length);
        }

        public static IEnumerable<object[]> InsertAndGet_Arguments()
        {
            var serializer = new SampleJsonSerializer();

            yield return new object[] { new DefaultFileManager(serializer) };
            yield return new object[] { new TransactionalFileManager(serializer) };
        }

        /// <summary>
        /// Tests the transaction handling of the repository.
        /// </summary>
        /// <param name="expectFailed">if set to <c>true</c> raises an exception.</param>
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Transaction(bool expectFailed)
        {
            string dataDirectory = Path.Combine(@$"TestOutput\{nameof(TestFileRepositoryUseKeyFile)}\{nameof(Transaction)}_{(expectFailed ? "failed" : "succeed")}");

            if (Directory.Exists(dataDirectory))
            {
                Directory.Delete(dataDirectory, true);
            }

            Directory.CreateDirectory(dataDirectory);

            var fileManager = new TransactionalFileManager(new SampleJsonSerializer());

            ISampleEntityRepository repository = new SampleEntityRepositoryUsingKeyFile(dataDirectory, fileManager);

            List<SampleEntity> entities =
            [
                new SampleEntity() { ID = Guid.NewGuid(), Code = 123, Name = "abc" },
                new SampleEntity() { ID = Guid.NewGuid(), Code = 456, Name = "xyz" },
            ];

            try
            {
                using var scope = new TransactionScope();

                repository.InsertRange(entities);

                if (expectFailed)
                {
                    repository.Insert(new SampleEntity() { ID = Guid.NewGuid(), Code = 123, Name = "ABC" });
                }

                scope.Complete();
            }
            catch (Exception ex)
            {
                m_Output.WriteLine(ex.Message);
            }

            if (expectFailed)
            {
                foreach (var entity in entities)
                {
                    Assert.False(File.Exists(Path.Combine(dataDirectory, "keys", $"{entity.ID}.key")));
                    Assert.False(File.Exists(Path.Combine(dataDirectory, "entities", $"{entity.Code:d5}_{entity.Name}.json")));
                }
            }
            else
            {
                foreach (var entity in entities)
                {
                    Assert.True(File.Exists(Path.Combine(dataDirectory, "keys", $"{entity.ID}.key")));
                    Assert.True(File.Exists(Path.Combine(dataDirectory, "entities", $"{entity.Code:d5}_{entity.Name}.json")));
                }
            }
        }
    }
}

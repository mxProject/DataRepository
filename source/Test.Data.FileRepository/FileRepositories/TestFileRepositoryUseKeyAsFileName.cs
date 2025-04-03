using Xunit.Abstractions;
using System.Transactions;
using mxProject.Data.Repositories;
using mxProject.Data.Repositories.FileManagers;

namespace Test.FileRepositories
{
    public class TestFileRepositoryUseKeyAsFileName
    {
        public TestFileRepositoryUseKeyAsFileName(ITestOutputHelper output)
        {
            m_Output = output;
        }

        private readonly ITestOutputHelper m_Output;

        [Theory]
        [MemberData(nameof(InsertAndGet_Arguments))]
        public void InsertAndGet(IFileManager fileManager)
        {
            string dataDirectory = Path.Combine(@$"TestOutput\{nameof(TestFileRepositoryUseKeyAsFileName)}\{nameof(InsertAndGet)}_{fileManager.GetType().Name}");

            if (Directory.Exists(dataDirectory))
            {
                Directory.Delete(dataDirectory, true);
            }

            Directory.CreateDirectory(dataDirectory);

            ISampleEntityRepository repository = new SampleEntityRepositoryUsingKeyAsFileName(dataDirectory, fileManager);

            List<SampleEntity> entities =
            [
                new SampleEntity() { ID = Guid.NewGuid(), Code = 123, Name = "abc" },
                new SampleEntity() { ID = Guid.NewGuid(), Code = 456, Name = "xyz" },
                new SampleEntity() { ID = Guid.NewGuid(), Code = 789, Name = "123" },
            ];

            repository.InsertRange(entities);

            entities[0].Name = "ABCDE";

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

            foreach(var e in allEntities)
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

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Transaction(bool expectFailed)
        {
            string dataDirectory = Path.Combine(@$"TestOutput\{nameof(TestFileRepositoryUseKeyAsFileName)}\{nameof(Transaction)}_{(expectFailed ? "failed" : "succeed")}");

            if (Directory.Exists(dataDirectory))
            {
                Directory.Delete(dataDirectory, true);
            }

            Directory.CreateDirectory(dataDirectory);

            var fileManager = new TransactionalFileManager(new SampleJsonSerializer());

            ISampleEntityRepository repository = new SampleEntityRepositoryUsingKeyAsFileName(dataDirectory, fileManager);

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
                    repository.Update(new SampleEntity() { ID = null, Name = "ABCDE" });
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
                    Assert.False(File.Exists(Path.Combine(dataDirectory, $"{entity.ID}.json")));
                }
            }
            else
            {
                foreach (var entity in entities)
                {
                    Assert.True(File.Exists(Path.Combine(dataDirectory, $"{entity.ID}.json")));
                }
            }
        }
    }
}

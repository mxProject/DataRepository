using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mxProject.Data.Repositories;
using mxProject.Data.Repositories.FileNamings;
using Test.FileRepositories;

namespace Test.FileRepositories
{
    /// <summary>
    /// Repository for SampleEntity using key file.
    /// </summary>
    internal class SampleEntityRepositoryUsingKeyFile : FileRepositoryUsingKeyFile<SampleEntity, Guid>, ISampleEntityRepository
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="dataDirectory">The data directory.</param>
        /// <param name="fileManager">The file manager.</param>
        internal SampleEntityRepositoryUsingKeyFile(string dataDirectory, IFileManager fileManager)
            : base(dataDirectory, CreateEntityFileNaming(), fileManager, Encoding.UTF8)
        {
        }

        /// <summary>
        /// Creates the entity file naming.
        /// </summary>
        /// <returns>The entity file naming.</returns>
        private static IEntityFileNamingUsingKeyFile<SampleEntity, Guid> CreateEntityFileNaming()
        {
            // ex) 350c9b44-8bd5-46af-b981-078892acfd90.key
            var keyFileNaming = EntityFileNaming.CreateKeyFileNameConverter(".key", x => x.ToString(), x => Guid.Parse(x));

            // ex) 00001_Entity1.json
            var entityFileNaming = EntityFileNaming.CreateEntityFileNameConverter<SampleEntity>(".json", x => $"{x.Code:d5}_{x.Name}");

            return EntityFileNaming.CreateUsingKeyFile<SampleEntity, Guid>(x => x.ID!.Value, keyFileNaming, entityFileNaming);
        }
    }
}
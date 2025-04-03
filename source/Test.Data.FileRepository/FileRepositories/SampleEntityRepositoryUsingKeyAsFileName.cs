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
    /// Repository for SampleEntity using the entity's key as the filename.
    /// </summary>
    internal class SampleEntityRepositoryUsingKeyAsFileName
        : FileRepositoryUsingKeyAsFileName<SampleEntity, Guid>, ISampleEntityRepository
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="dataDirectory">The directory in which to save data files.</param>
        /// <param name="fileManager">The file manager.</param>
        internal SampleEntityRepositoryUsingKeyAsFileName(string dataDirectory, IFileManager fileManager)
            : base(dataDirectory, CreateEntityFileNaming(), fileManager, Encoding.UTF8)
        {
        }

        /// <summary>
        /// Creates the entity file naming logic.
        /// </summary>
        /// <returns>The entity file naming logic.</returns>
        private static IEntityFileNamingUsingKeyAsFileName<SampleEntity, Guid> CreateEntityFileNaming()
        {
            // ex) 350c9b44-8bd5-46af-b981-078892acfd90.json
            var fileNaming = EntityFileNaming.CreateKeyFileNameConverter(".json", x => x.ToString(), x => Guid.Parse(x));

            return EntityFileNaming.CreateUsingKeyAsFileName<SampleEntity, Guid>(x => x.ID!.Value, fileNaming);
        }
    }
}
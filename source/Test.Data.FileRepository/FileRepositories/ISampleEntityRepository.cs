using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mxProject.Data.Repositories;
using Test.FileRepositories;

namespace Test.FileRepositories
{
    /// <summary>
    /// Interface for SampleEntity repository.
    /// </summary>
    internal interface ISampleEntityRepository : IReadDataRepository<SampleEntity, Guid>, IWriteDataRepository<SampleEntity>
    {
    }
}

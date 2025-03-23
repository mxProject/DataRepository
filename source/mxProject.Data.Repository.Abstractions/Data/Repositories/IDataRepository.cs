using System;
using System.Collections.Generic;
using System.Text;

namespace mxProject.Data.Repositories
{
    /// <summary>
    /// It provides the functionality required for a data repository.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public interface IDataRepository<TEntity> : IDisposable
    {
        /// <summary>
        /// Gets a value that indicates whether to use ambient transactions using TransactionScope.
        /// </summary>
        bool UseTransactionScope { get; }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mxProject.Data;
using mxProject.Data.Repositories;

namespace Test.DbRepositories
{
    /// <summary>
    /// Represents a sample context for data repository.
    /// </summary>
    internal class SampleContext : IDataRepositoryContext 
    {
    }

    /// <summary>
    /// Represents a sample database context for database repository.
    /// </summary>
    internal class SampleDbContext : IDataRepositoryContext, IDbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SampleDbContext"/> class with the specified connection.
        /// </summary>
        /// <param name="connection">The database connection.</param>
        internal SampleDbContext(IDbConnection connection)
        {
            m_Connection = connection;
            m_Transaction = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SampleDbContext"/> class with the specified transaction.
        /// </summary>
        /// <param name="transaction">The database transaction.</param>
        internal SampleDbContext(IDbTransaction transaction)
        {
            m_Connection = transaction?.Connection;
            m_Transaction = transaction;
        }

        private readonly IDbConnection? m_Connection;
        private readonly IDbTransaction? m_Transaction;

        /// <summary>
        /// Gets the current database connection.
        /// </summary>
        /// <returns>The current database connection.</returns>
        public IDbConnection GetCurrentConnection()
        {
            return m_Connection!;
        }

        /// <summary>
        /// Gets the current database transaction.
        /// </summary>
        /// <returns>The current database transaction, or null if there is no transaction.</returns>
        public IDbTransaction? GetCurrentTransaction()
        {
            return m_Transaction;
        }
    }
}

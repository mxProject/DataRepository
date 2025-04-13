using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace Test.DbRepositories
{
    internal class SampleDatabase
    {
        internal static IDbConnection CreateConnection()
        {
            return new SqlConnection("Data Source=(localdb)\\SampleDB; Initial Catalog=DataRepositorySampleDatabase; Integrated Security=True");
        }

        internal static int DefaultCommandTimeout
        {
            get { return 15; }
        }

        internal static void ConfigureCommand(IDbCommand command)
        {
            command.CommandType = CommandType.Text;
            command.CommandTimeout = DefaultCommandTimeout;
        }
    }
}

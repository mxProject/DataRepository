using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace Test.DbRepositories
{
    internal static class DbCommandExtensions
    {
        /// <summary>
        /// Adds a string parameter to the command.
        /// </summary>
        /// <param name="command">The command to add the parameter to.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <returns>The created parameter.</returns>
        internal static IDbDataParameter AddParameter(this IDbCommand command, string parameterName, string? value)
        {
            var parameter = command.CreateParameter();

            parameter.ParameterName = parameterName;
            parameter.DbType = DbType.String;
            parameter.Value = value;

            command.Parameters.Add(parameter);

            return parameter;
        }

        /// <summary>
        /// Adds an integer parameter to the command.
        /// </summary>
        /// <param name="command">The command to add the parameter to.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <returns>The created parameter.</returns>
        internal static IDbDataParameter AddParameter(this IDbCommand command, string parameterName, int? value)
        {
            var parameter = command.CreateParameter();

            parameter.ParameterName = parameterName;
            parameter.DbType = DbType.Int32;
            parameter.Value = value;

            command.Parameters.Add(parameter);

            return parameter;
        }

        /// <summary>
        /// Sets the value of the parameter.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="parameterName">The parameter name.</param>
        /// <param name="value">The value.</param>
        internal static void SetParameterValue(this IDbCommand command, string parameterName, string? value)
        {
            if (value == null || value == string.Empty)
            {
                ((IDbDataParameter)command.Parameters[parameterName]).Value = DBNull.Value;
            }
            else
            {
                ((IDbDataParameter)command.Parameters[parameterName]).Value = value;
            }
        }

        /// <summary>
        /// Sets the value of the parameter.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="command">The command.</param>
        /// <param name="parameterName">The parameter name.</param>
        /// <param name="value">The value.</param>
        internal static void SetParameterValue<T>(this IDbCommand command, string parameterName, T? value) where T : struct
        {
            if (value == null)
            {
                ((IDbDataParameter)command.Parameters[parameterName]).Value = DBNull.Value;
            }
            else
            {
                ((IDbDataParameter)command.Parameters[parameterName]).Value = value.Value;
            }
        }

        /// <summary>
        /// Sets the value of the parameter.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="parameterName">The parameter name.</param>
        /// <param name="value">The value.</param>
        internal static void SetParameterValue(this IDbCommand command, string parameterName, object? value)
        {
            if (value == null)
            {
                ((IDbDataParameter)command.Parameters[parameterName]).Value = DBNull.Value;
            }
            else
            {
                ((IDbDataParameter)command.Parameters[parameterName]).Value = value;
            }
        }
    }
}

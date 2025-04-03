using mxProject.Data.Repositories.Serializers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace mxProject.Data.Repositories
{
    /// <summary>
    /// Extension methods for <see cref="IEntityFileSerializer"/>.
    /// </summary>
    public static class IEntityFileSerializerExtensions
    {
        /// <summary>
        /// Wraps it as an instance of <see cref="IEntityFileSerializerWithContext{TContext}"/>.
        /// </summary>
        /// <typeparam name="TContext">The context type.</typeparam>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public static IEntityFileSerializerWithContext<TContext> WithContext<TContext>(this IEntityFileSerializer serializer)
            where TContext : IDataRepositoryContext
        {
            return new EntitySerializerWithContext<TContext>(serializer);
        }
    }
}

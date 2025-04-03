using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace mxProject.Data.Repositories.Serializers
{
    internal class EntitySerializerWithContext<TContext> : IEntityFileSerializerWithContext<TContext>
        where TContext : IDataRepositoryContext
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="serializer"></param>
        internal EntitySerializerWithContext(IEntityFileSerializer serializer)
        {
            m_Serializer = serializer;
        }

        private readonly IEntityFileSerializer m_Serializer;

        public TEntity Deserialize<TEntity>(Stream stream, Encoding encoding, TContext context)
        {
            return m_Serializer.Deserialize<TEntity>(stream, encoding);
        }

        public void Serialize<TEntity>(TEntity entity, Stream stream, Encoding encoding, TContext context)
        {
            m_Serializer.Serialize(entity, stream, encoding);
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace mxProject.Data.Repositories
{
    /// <summary>
    /// Provides methods to serialize and deserialize entities to and from a stream.
    /// </summary>
    /// <typeparam name="TContext">The context type.</typeparam>
    public interface IEntityFileSerializerWithContext<TContext>
        where TContext : IDataRepositoryContext
    {
        /// <summary>
        /// Serializes the specified entity to the given stream using the specified encoding.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity to serialize.</param>
        /// <param name="stream">The stream to which the entity will be serialized.</param>
        /// <param name="encoding">The encoding to use.</param>
        /// <param name="context">The current context.</param>
        void Serialize<TEntity>(TEntity entity, Stream stream, Encoding encoding, TContext context);

        /// <summary>
        /// Deserializes an entity of the specified type from the given stream using the specified encoding.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="stream">The stream from which the entity will be deserialized.</param>
        /// <param name="encoding">The encoding to use.</param>
        /// <param name="context">The current context.</param>
        /// <returns>The deserialized entity.</returns>
        TEntity Deserialize<TEntity>(Stream stream, Encoding encoding, TContext context);
    }
}

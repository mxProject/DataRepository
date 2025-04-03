using mxProject.Data.Repositories.FileNamings;
using System;
using System.Collections.Generic;
using System.Text;

namespace mxProject.Data.Repositories
{
    /// <summary>
    /// Provides extension methods for <see cref="IEntityFileNameConverter{TEntity}"/>.
    /// </summary>
    internal static class IEntityFileNameConverterExtensions
    {
        /// <summary>
        /// Creates a new instance of <see cref="IEntityFileNameConverterWithContext{TEntity, TContext}"/> using the specified context.
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <typeparam name="TContext">The context type.</typeparam>
        /// <param name="converter"></param>
        /// <returns></returns>
        internal static IEntityFileNameConverterWithContext<TEntity, TContext> WithContext<TEntity, TContext>(this IEntityFileNameConverter<TEntity> converter)
            where TContext : IDataRepositoryContext
        {
            return new EntityFileNameConverterWithContext<TEntity, TContext>(
                converter.FileExtension,
                (entity, context) => converter.ToFileNameWithoutExtension(entity)
                );
        }
    }
}

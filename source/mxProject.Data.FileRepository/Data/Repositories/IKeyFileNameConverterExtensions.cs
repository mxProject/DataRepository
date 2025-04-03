using mxProject.Data.Repositories.FileNamings;
using System;
using System.Collections.Generic;
using System.Text;

namespace mxProject.Data.Repositories
{
    /// <summary>
    /// Extension methods for <see cref="IKeyFileNameConverter{TKey}"/>.
    /// </summary>
    internal static class IKeyFileNameConverterExtensions
    {
        /// <summary>
        /// Creates a new instance of <see cref="IKeyFileNameConverterWithContext{TKey, TContext}"/> using the specified context.
        /// </summary>
        /// <typeparam name="TKey">The key type.</typeparam>
        /// <typeparam name="TContext">The context type.</typeparam>
        /// <param name="converter"></param>
        /// <returns></returns>
        internal static IKeyFileNameConverterWithContext<TKey, TContext> WithContext<TKey, TContext>(this IKeyFileNameConverter<TKey> converter)
            where TContext : IDataRepositoryContext
        {
            return new KeyFileNameConverterWithContext<TKey, TContext>(
                converter.FileExtension,
                (key, context) => converter.ToFileNameWithoutExtension(key),
                (fileName, context) => converter.FromFileNameWithoutExtension(fileName)
                );
        }
    }
}

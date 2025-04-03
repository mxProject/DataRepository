using mxProject.Data.Repositories.FileNamings;
using System;
using System.Collections.Generic;
using System.Text;

namespace mxProject.Data.Repositories
{
    /// <summary>
    /// Provides functionality to convert an entity to a file name.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public interface IEntityFileNameConverter<TEntity>
    {
        /// <summary>
        /// Gets the file extension.
        /// </summary>
        string FileExtension { get; }

        /// <summary>
        /// Converts the specified entity to a file name without extension.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>The file name without extension.</returns>
        string ToFileNameWithoutExtension(TEntity entity);
    }
}

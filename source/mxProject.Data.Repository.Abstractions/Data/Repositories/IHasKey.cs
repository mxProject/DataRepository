using System;
using System.Collections.Generic;
using System.Text;

namespace mxProject.Data.Repositories
{
    /// <summary>
    /// Provides the functionality required to get a key.
    /// </summary>
    /// <typeparam name="TKey">The key type.</typeparam>
    public interface IHasKey<TKey>
    {
        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <returns>The key.</returns>
        TKey GetKey();
    }
}

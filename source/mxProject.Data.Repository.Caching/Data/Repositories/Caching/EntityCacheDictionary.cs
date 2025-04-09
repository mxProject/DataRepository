using mxProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace mxProject.Data.Repositories.Caching
{
    /// <summary>
    /// Use a dictionary to cache entities.
    /// </summary>
    /// <typeparam name="TKey">The key type.</typeparam>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    internal class EntityCacheDictionary<TKey, TEntity> : IEntityCache<TKey, TEntity>
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        internal EntityCacheDictionary(IDictionary<TKey, TEntity> dictionary)
        {
            m_Dictionary = dictionary;
        }

        private readonly IDictionary<TKey, TEntity> m_Dictionary;

        /// <inheritdoc/>
        public void Dispose()
        {
        }

        /// <inheritdoc/>
        public bool TryGet(TKey key, out TEntity entity)
        {
            return m_Dictionary.TryGetValue(key, out entity);
        }

        /// <inheritdoc/>
        public void Store(TKey key, TEntity entity)
        {
            m_Dictionary[key] = entity;
        }

        /// <inheritdoc/>
        public bool Remove(TKey key)
        {
            return m_Dictionary.Remove(key);
        }

        /// <inheritdoc/>
        public void Clear()
        {
            m_Dictionary.Clear();
        }
    }
}

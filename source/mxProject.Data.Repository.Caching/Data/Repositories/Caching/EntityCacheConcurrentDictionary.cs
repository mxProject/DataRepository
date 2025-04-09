using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using mxProject.Data.Repositories;

namespace mxProject.Data.Repositories.Caching
{
    /// <summary>
    /// Use a concurrent dictionary to cache entities.
    /// </summary>
    /// <typeparam name="TKey">The key type.</typeparam>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    internal class EntityCacheConcurrentDictionary<TKey, TEntity> : IEntityCache<TKey, TEntity>
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        internal EntityCacheConcurrentDictionary(ConcurrentDictionary<TKey, TEntity> dictionary)
        {
            m_Dictionary = dictionary;
        }

        private readonly ConcurrentDictionary<TKey, TEntity> m_Dictionary;

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
            m_Dictionary.AddOrUpdate(key, entity, (k, e) => entity);
        }

        /// <inheritdoc/>
        public bool Remove(TKey key)
        {
            return m_Dictionary.TryRemove(key, out _);
        }

        /// <inheritdoc/>
        public void Clear()
        {
            m_Dictionary.Clear();
        }
    }
}

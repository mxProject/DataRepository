using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MessagePipe;
using Microsoft.Extensions.DependencyInjection;
using static mxProject.Data.Repositories.Caching.DataRepositoryCachingService;

namespace mxProject.Data.Repositories.Caching
{
    /// <summary>
    /// DI service for caching entities.
    /// </summary>
    internal class DataRepositoryCachingService
    {
        private readonly static IServiceProvider s_ServiceProvider;

        /// <summary>
        /// Type initializer.
        /// </summary>
        static DataRepositoryCachingService()
        {
            s_ServiceProvider = CreateServiceProvider();
        }

        /// <summary>
        /// Creates a service provider.
        /// </summary>
        /// <returns></returns>
        private static IServiceProvider CreateServiceProvider()
        {
            ServiceCollection services = new ServiceCollection();

            services.AddMessagePipe();

            return services.BuildServiceProvider();
        }

        /// <summary>
        /// Create a publisher to notify of changes to an entity.
        /// </summary>
        /// <typeparam name="TKey">The key type.</typeparam>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <returns></returns>
        internal static EntityChangedPublisher<TKey, TEntity> CreateEntityChangedPublisher<TKey, TEntity>()
        {
            return new EntityChangedPublisher<TKey, TEntity>();
        }

        /// <summary>
        /// Create a subscriber to receive changes to the entity.
        /// </summary>
        /// <typeparam name="TKey">The key type.</typeparam>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="onChanged">The method to invoke when the entity is modified.</param>
        /// <returns></returns>
        internal static IDisposable CreateEntityChangedSubscriber<TKey, TEntity>(Action<TKey, TEntity> onChanged)
        {
            return new EntityChangedSubscriber<TKey, TEntity>(onChanged);
        }

        /// <summary>
        /// Publisher to notify of changes to an entity.
        /// </summary>
        /// <typeparam name="TKey">The key type.</typeparam>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        internal class EntityChangedPublisher<TKey, TEntity>
        {
            /// <summary>
            /// Creates a new instance.
            /// </summary>
            public EntityChangedPublisher()
            {
                m_Publisher = s_ServiceProvider.GetRequiredService<IPublisher<Type, (TKey, TEntity)>>();
            }

            private readonly IPublisher<Type, (TKey, TEntity)> m_Publisher;

            /// <summary>
            /// Notifies that the specified entity has changed.
            /// </summary>
            /// <param name="key">The key.</param>
            /// <param name="entity">The entity.</param>
            internal void Publish(TKey key, TEntity entity)
            {
                m_Publisher.Publish(typeof(TEntity), (key, entity));
            }
        }

        /// <summary>
        /// Subscriber to receive changes to the entity.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TEntity"></typeparam>
        internal class EntityChangedSubscriber<TKey, TEntity> :  IDisposable
        {
            /// <summary>
            /// Creates a new instance.
            /// </summary>
            /// <param name="onChanged">The method to invoke when the entity is modified.</param>
            internal EntityChangedSubscriber(Action<TKey, TEntity> onChanged)
            {
                m_Subscriber = s_ServiceProvider.GetRequiredService<ISubscriber<Type, (TKey, TEntity)>>().Subscribe(typeof(TEntity), (x) =>
                {
                    onChanged(x.Item1, x.Item2);
                });
            }

            private readonly IDisposable m_Subscriber;

            public void Dispose()
            {
                m_Subscriber.Dispose();
            }
        }
    }
}

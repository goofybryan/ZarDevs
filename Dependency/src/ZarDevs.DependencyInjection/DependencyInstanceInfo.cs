using System;

namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// Dpendency info class that describes the dependency binding and the Instance that will always be resolved.
    /// </summary>
    public class DependencyInstanceInfo : DependencyInfo, IDependencyInstanceInfo
    {
        /// <summary>
        /// Create a new instance
        /// </summary>
        /// <param name="requestType">Specify the request type.</param>
        /// <param name="instance">The instance that is always returned.</param>
        public DependencyInstanceInfo(Type requestType, object instance) : base(requestType, scope: DependyBuilderScope.Singleton)
        {
            Instance = instance ?? throw new ArgumentNullException(nameof(instance));
            Validate(requestType, instance);
        }

        /// <summary>
        /// Create a new instance
        /// </summary>
        /// <param name="instance">The instance that is always returned.</param>
        /// <param name="copy">The initail descriptor that's values will be copied.</param>
        public DependencyInstanceInfo(object instance, DependencyInfo copy) : base(copy)
        {
            Instance = instance ?? throw new ArgumentNullException(nameof(instance));
            Validate(copy.RequestType, instance);
            Scope = DependyBuilderScope.Singleton;
        }

        /// <summary>
        /// The instance that is always returned.
        /// </summary>
        public object Instance { get; }

        internal override void SetScope(DependyBuilderScope scope)
        {
            _ = scope;
        }

        private static void Validate(Type requestType, object instance)
        {
            Type instanceType = instance.GetType();
            if (!requestType.IsAssignableFrom(instanceType))
                throw new InvalidOperationException($"Cannot bind {requestType.FullName} to instance {instanceType}.");
        }
    }
}
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
        /// <param name="instance">The instance that is always returned.</param>
        /// <param name="copy">The initail descriptor that's values will be copied.</param>
        public DependencyInstanceInfo(object instance, DependencyInfo copy) : base(copy)
        {
            Instance = instance ?? throw new ArgumentNullException(nameof(instance));
            Type instanceType = instance.GetType();
            if (!RequestType.IsAssignableFrom(instanceType))
                throw new InvalidOperationException($"Cannot bind {RequestType.FullName} to instance {instanceType}.");

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
    }
}
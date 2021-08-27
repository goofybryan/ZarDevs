using System;
using System.Collections.Generic;
using System.Linq;

namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// Dpendency info class that describes the dependency binding and the Instance that will always
    /// be resolved.
    /// </summary>
    public class DependencyInstanceInfo : DependencyInfo, IDependencyInstanceInfo
    {
        #region Constructors

        /// <summary>
        /// Create a new instance
        /// </summary>
        /// <param name="requestType">Specify the request type.</param>
        /// <param name="instance">The instance that is always returned.</param>
        public DependencyInstanceInfo(Type requestType, object instance) : base(scope: DependyBuilderScope.Singleton)
        {
            Instance = instance ?? throw new ArgumentNullException(nameof(instance));
            Validate(requestType, instance);
            ResolvedTypes.Add(requestType);
        }

        /// <summary>
        /// Create a new instance
        /// </summary>
        /// <param name="instance">The instance that is always returned.</param>
        /// <param name="copy">The initail descriptor that's values will be copied.</param>
        public DependencyInstanceInfo(object instance, IDependencyInfo copy) : base(copy)
        {
            Instance = instance ?? throw new ArgumentNullException(nameof(instance));
            Validate(copy.ResolvedTypes, instance);
            Scope = DependyBuilderScope.Singleton;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// The instance that is always returned.
        /// </summary>
        public object Instance { get; }

        #endregion Properties

        #region Methods

        private static void Validate(ICollection<Type> requestTypes, object instance)
        {
            foreach (var requestType in requestTypes)
            {
                Validate(requestType, instance);
            }
        }

        private static void Validate(Type requestType, object instance)
        {
            Type instanceType = instance.GetType();
            if (!requestType.IsAssignableFrom(instanceType))
                throw new InvalidOperationException($"Cannot bind {requestType.FullName} to instance {instanceType}.");
        }

        #endregion Methods
    }
}
using System;

namespace ZarDevs.DependencyInjection
{
    public class DependencyInstanceInfo : DependencyInfo, IDependencyInstanceInfo
    {
        public DependencyInstanceInfo(object instance, DependencyInfo info) : base(info)
        {
            Instance = instance ?? throw new ArgumentNullException(nameof(instance));
            Type instanceType = instance.GetType();
            if (!RequestType.IsAssignableFrom(instanceType))
                throw new InvalidOperationException($"Cannot bind {RequestType.FullName} to instance {instanceType}.");

            Scope = DependyBuilderScope.Singleton;
        }

        public object Instance { get; }

        internal override void SetScope(DependyBuilderScope scope)
        {
            _ = scope;
        }
    }
}
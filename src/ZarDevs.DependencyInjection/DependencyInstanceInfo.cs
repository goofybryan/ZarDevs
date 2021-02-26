using System;

namespace ZarDevs.DependencyInjection
{
    internal class DependencyInstanceInfo : DependencyInfo, IDependencyInstanceInfo
    {
        public DependencyInstanceInfo(object instance, DependencyInfo info) : base(info)
        {
            Instance = instance ?? throw new ArgumentNullException(nameof(instance));
            Type instanceType = instance.GetType();
            if (!RequestType.IsAssignableFrom(instanceType))
                throw new InvalidOperationException($"Cannot bind {RequestType.FullName} to instance {instanceType}.");
        }

        public object Instance { get; }
    }
}
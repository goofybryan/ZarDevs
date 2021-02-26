using System;

namespace ZarDevs.DependencyInjection
{
    internal class DependencyInstanceInfo : IDependencyInstanceInfo
    {
        #region Constructors

        public DependencyInstanceInfo(object instance, object key)
        {
            Instance = instance ?? throw new ArgumentNullException(nameof(instance));
            Key = key;
            RequestType = instance.GetType();
        }

        #endregion Constructors

        #region Properties

        public object Instance { get; }

        public object Key { get; }

        public Type RequestType { get; }
        public DependyBuilderScope Scope => DependyBuilderScope.Singleton;

        #endregion Properties
    }
}
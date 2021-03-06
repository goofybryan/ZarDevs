using System;

namespace ZarDevs.DependencyInjection
{
    internal class DependencyBuilderInfo : IDependencyBuilderInfo, IDependencyBuilderBindingRequest, IDependencyBuilderBindingResolve, IDependencyBuilderBindingMetaData
    {
        #region Fields

        private DependencyInfo _info = new DependencyInfo();

        #endregion Fields

        #region Properties

        public IDependencyInfo DependencyInfo => _info;

        #endregion Properties

        #region Methods

        public IDependencyBuilderBindingResolve Bind(Type type)
        {
            _info.RequestType = type ?? throw new ArgumentNullException(nameof(type));
            return this;
        }

        public IDependencyBuilderBindingResolve Bind<T>() where T : class
        {
            return Bind(typeof(T));
        }

        public IDependencyBuilderInfo InSingletonScope()
        {
            _info.SetScope(DependyBuilderScope.Singleton);
            return this;
        }

        public IDependencyBuilderInfo InTransientScope()
        {
            _info.SetScope(DependyBuilderScope.Transient);
            return this;
        }

        public IDependencyBuilderBindingMetaData To(Type type)
        {
            _info = new DependencyTypeInfo(type, _info);
            return this;
        }

        public IDependencyBuilderBindingMetaData To<T>() where T : class
        {
            return To(typeof(T));
        }

        public IDependencyBuilderBindingMetaData To(Func<DependencyBuilderContext, object> method)
        {
            _info = new DependencyMethodInfo(method, _info);
            return this;
        }

        public IDependencyBuilderBindingMetaData To<T>(T instance)
        {
            _info = new DependencyInstanceInfo(instance, _info);
            return this;
        }

        public IDependencyBuilderInfo WithKey(object key)
        {
            _info.Key = key;
            return this;
        }

        #endregion Methods
    }
}
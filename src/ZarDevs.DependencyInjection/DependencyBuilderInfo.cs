using System;

namespace ZarDevs.DependencyInjection
{
    internal class DependencyBuilderInfo : IDependencyBuilderInfo
    {
        #region Fields

        private DependencyInfo _info = new DependencyInfo();

        #endregion Fields

        #region Properties

        public IDependencyInfo DependencyInfo => _info;

        #endregion Properties

        #region Methods

        public IDependencyBuilderInfo Bind(Type type)
        {
            _info.RequestType = type ?? throw new ArgumentNullException(nameof(type));
            return this;
        }

        public IDependencyBuilderInfo Bind<T>() where T : class
        {
            return Bind(typeof(T));
        }

        public IDependencyBuilderInfo InRequestScope()
        {
            _info.Scope = DependyBuilderScope.Request;
            return this;
        }

        public IDependencyBuilderInfo InSingletonScope()
        {
            _info.Scope = DependyBuilderScope.Singleton;
            return this;
        }

        public IDependencyBuilderInfo InTransientScope()
        {
            _info.Scope = DependyBuilderScope.Transient;
            return this;
        }

        public IDependencyBuilderInfo To(Type type)
        {
            _info = new DependencyTypeInfo(type, _info);
            return this;
        }

        public IDependencyBuilderInfo To<T>() where T : class
        {
            return To(typeof(T));
        }

        public IDependencyBuilderInfo To(Func<DepencyBuilderInfoContext, object, object> method)
        {
            _info = new DependencyMethodInfo(method, _info);
            return this;
        }

        public IDependencyBuilderInfo To<T>(T instance)
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
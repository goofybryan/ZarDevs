using System;
using System.Collections.Generic;
using ZarDevs.Runtime;

namespace ZarDevs.DependencyInjection
{
    internal class DependencyBuilderInfo : IDependencyBuilderInfo, IDependencyBuilderBindingMetaData
    {
        #region Fields

        private IDependencyInfo _info;

        #endregion Fields

        #region Properties

        public IDependencyInfo DependencyInfo => _info;

        #endregion Properties

        #region Methods

        public IDependencyBuilderInfo InRequestScope()
        {
            _info.Scope = DependyBuilderScopes.Request;
            return this;
        }

        public IDependencyBuilderInfo InSingletonScope()
        {
            _info.Scope = DependyBuilderScopes.Singleton;
            return this;
        }

        public IDependencyBuilderInfo InThreadScope()
        {
            _info.Scope = DependyBuilderScopes.Thread;
            return this;
        }

        public IDependencyBuilderInfo InTransientScope()
        {
            _info.Scope = DependyBuilderScopes.Transient;
            return this;
        }

        public IDependencyBuilderBindingResolve Resolve(Type resolvedType)
        {
            _info.ResolvedTypes.Add(resolvedType);

            return this;
        }

        public IDependencyBuilderBindingResolve Resolve(params Type[] resolvedTypes)
        {
            foreach (var resolvedType in resolvedTypes)
            {
                Resolve(resolvedType);
            }

            return this;
        }

        public IDependencyBuilderBindingResolve Resolve<T>() where T : class
        {
            return Resolve(typeof(T));
        }

        public IDependencyBuilderInfo ResolveAll(params Type[] ignoredTypes)
        {
            if (_info is not IDependencyTypeInfo typeInfo) return this;

            var resolutionType = typeInfo.ResolutionType;
            var typesToIgnore = new List<Type>() { typeof(IDisposable), typeof(object) };

            if (ignoredTypes?.Length > 0)
                typesToIgnore.AddRange(ignoredTypes);

            var implementingTypes = InspectObject.Instance.FindImplementingTypes(resolutionType, typesToIgnore);

            foreach (var implementingType in implementingTypes)
            {
                Resolve(implementingType);
            }

            Resolve(resolutionType);

            return this;
        }

        public IDependencyBuilderBindingMetaData With(Type type)
        {
            _info = new DependencyTypeInfo(type, _info);
            return this;
        }

        public IDependencyBuilderBindingMetaData With<T>() where T : class
        {
            return With(typeof(T));
        }

        public IDependencyBuilderBindingMetaData With(Func<IDependencyContext, object> method)
        {
            _info = new DependencyMethodInfo(method, _info);
            return this;
        }

        public IDependencyBuilderBindingMetaData With<T>(T instance)
        {
            _info = new DependencyInstanceInfo(instance, _info);
            return this;
        }

        public IDependencyBuilderBindingMetaData WithFactory<T>(string methodName)
        {
            _info = new DependencyFactoryInfo(typeof(T), methodName, _info);
            return this;
        }

        public IDependencyBuilderBindingMetaData WithFactory(Type factoryType, string methodName)
        {
            _info = new DependencyFactoryInfo(factoryType, methodName, _info);
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
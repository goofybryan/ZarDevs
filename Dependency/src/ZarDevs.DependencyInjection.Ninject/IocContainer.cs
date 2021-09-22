using Ninject;
using Ninject.Parameters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ZarDevs.Runtime;

namespace ZarDevs.DependencyInjection
{
    internal class IocContainer : IIocContainer<IKernel>
    {
        #region Fields

        private readonly IDependencyContainer _dependencyContainer;
        private bool _disposed = false;

        #endregion Fields

        #region Constructors

        public IocContainer(IKernel kernel, IDependencyContainer dependencyContainer)
        {
            Kernel = kernel ?? throw new ArgumentNullException(nameof(kernel));
            _dependencyContainer = dependencyContainer ?? throw new ArgumentNullException(nameof(dependencyContainer));
        }

        #endregion Constructors

        #region Properties

        public IKernel Kernel { get; }

        #endregion Properties

        #region Methods

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public bool HasModule(string name)
        {
            return Kernel.HasModule(name);
        }

        public T Resolve<T>(params (string, object)[] parameters) where T : class
        {
            return Kernel.Get<T>(CreateParameters(parameters));
        }

        public T Resolve<T>() where T : class
        {
            return Kernel.Get<T>();
        }

        public T Resolve<T>(params object[] parameters) where T : class
        {
            return Resolve<T>(CreateNamedParameters(null, typeof(T), parameters));
        }

        public IEnumerable<T> ResolveAll<T>() where T : class
        {
            return Kernel.GetAll<T>();
        }

        public IEnumerable ResolveAll(Type requestType)
        {
            var result = Kernel.GetAll(requestType).ToArray();
            var resolved = Array.CreateInstance(requestType, result.Length);
            for (int i = 0; i < result.Length; i++)
            {
                resolved.SetValue(result[i], i);
            }

            return resolved;
        }

        public T ResolveNamed<T>(string name, params (string, object)[] parameters) where T : class
        {
            return Kernel.Get<T>(name, CreateParameters(parameters));
        }

        public T ResolveNamed<T>(string name) where T : class
        {
            return Kernel.Get<T>(name);
        }

        public T ResolveNamed<T>(string name, params object[] parameters) where T : class
        {
            return ResolveNamed<T>(name, CreateNamedParameters(name, typeof(T), parameters));
        }

        public T ResolveWithKey<T>(Enum key, params (string, object)[] parameters) where T : class
        {
            return Kernel.Get<T>(key.GetBindingName(), CreateParameters(parameters));
        }

        public T ResolveWithKey<T>(object key, params (string, object)[] parameters) where T : class
        {
            if (key is null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (key is Enum enumKey)
                return ResolveWithKey<T>(enumKey, parameters);

            return Kernel.Get<T>(key.ToString(), CreateParameters(parameters));
        }

        public T ResolveWithKey<T>(Enum key) where T : class
        {
            return Kernel.Get<T>(key.GetBindingName());
        }

        public T ResolveWithKey<T>(object key) where T : class
        {
            if (key is Enum enumKey)
                return ResolveWithKey<T>(enumKey);

            return Kernel.Get<T>(key.ToString());
        }

        public T ResolveWithKey<T>(Enum key, params object[] parameters) where T : class
        {
            return ResolveWithKey<T>(key.GetBindingName(), parameters);
        }

        public T ResolveWithKey<T>(object key, params object[] parameters) where T : class
        {
            if (key is Enum enumKey)
                return ResolveWithKey<T>(enumKey, parameters);

            return ResolveWithKey<T>(key.ToString(), parameters);
        }

        public T TryResolve<T>(params (string, object)[] parameters) where T : class
        {
            return Kernel.TryGet<T>(CreateParameters(parameters));
        }

        public T TryResolve<T>() where T : class
        {
            return Kernel.TryGet<T>();
        }

        public T TryResolve<T>(params object[] parameters) where T : class
        {
            return TryResolve<T>(CreateNamedParameters(null, typeof(T), parameters));
        }

        public object TryResolve(Type requestType)
        {
            return Kernel.TryGet(requestType);
        }

        public T TryResolveNamed<T>(string name, params (string, object)[] parameters) where T : class
        {
            return Kernel.TryGet<T>(name, CreateParameters(parameters));
        }

        public T TryResolveNamed<T>(string name) where T : class
        {
            return Kernel.TryGet<T>(name);
        }

        public T TryResolveNamed<T>(string name, params object[] parameters) where T : class
        {
            return TryResolveNamed<T>(name, CreateNamedParameters(name, typeof(T), parameters));
        }

        public object TryResolveNamed(Type requestType, string name, params object[] parameters)
        {
            var namedParameter = CreateNamedParameters(name, requestType, parameters);
            return Kernel.TryGet(requestType, name, CreateParameters(namedParameter));
        }

        public T TryResolveWithKey<T>(Enum key, params (string, object)[] parameters) where T : class
        {
            return TryResolveNamed<T>(key.GetBindingName(), parameters);
        }

        public T TryResolveWithKey<T>(object key, params (string, object)[] parameters) where T : class
        {
            if (key is Enum enumKey)
                return TryResolveWithKey<T>(enumKey, parameters);

            return TryResolveNamed<T>(key.ToString(), parameters);
        }

        public T TryResolveWithKey<T>(Enum key) where T : class
        {
            return Kernel.TryGet<T>(key.GetBindingName());
        }

        public T TryResolveWithKey<T>(object key) where T : class
        {
            if (key is Enum enumKey)
                return TryResolveWithKey<T>(enumKey);

            return Kernel.TryGet<T>(key.ToString());
        }

        public T TryResolveWithKey<T>(Enum key, params object[] parameters) where T : class
        {
            return TryResolveNamed<T>(key.GetBindingName(), parameters);
        }

        public T TryResolveWithKey<T>(object key, params object[] parameters) where T : class
        {
            if (key is Enum enumKey)
                return TryResolveWithKey<T>(enumKey, parameters);

            return TryResolveNamed<T>(key.ToString(), parameters);
        }

        public object TryResolveWithKey(Type requestType, Enum key, params object[] parameters)
        {
            var name = key.GetBindingName();
            var namedParameter = CreateNamedParameters(name, requestType, parameters);
            return Kernel.TryGet(requestType, name, CreateParameters(namedParameter));
        }

        public object TryResolveWithKey(Type requestType, object key, params object[] parameters)
        {
            if (key is Enum enumKey)
                return TryResolveWithKey(requestType, enumKey, parameters);

            var name = key.ToString();
            var namedParameter = CreateNamedParameters(name, requestType, parameters);
            return Kernel.TryGet(requestType, name, CreateParameters(namedParameter));
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Kernel?.Dispose();
                }

                _disposed = true;
            }
        }

        private static IParameter[] CreateParameters(IList<(string, object)> parameters)
        {
            if (parameters == null)
                return null;

            var list = new List<IParameter>();

            foreach (var (name, value) in parameters)
            {
                var constructorArg = new ConstructorArgument(name, value);
                list.Add(constructorArg);
            }

            return list.ToArray();
        }

        private (string, object)[] CreateNamedParameters(object key, Type requestType, object[] values)
        {
            if (values == null) return null;
            if (values.Length == 0) return Array.Empty<(string, object)>();

            var binding = _dependencyContainer.TryGetBinding(requestType, key);

            if (binding is null) return Array.Empty<(string, object)>();

            if (binding is IDependencyTypeInfo typeInfo)
                return InspectConstructor.Instance.FindParameterNames(typeInfo.ResolutionType, values).ToArray();

            var orderedParams = new (string, object)[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                orderedParams[i] = ValueTuple.Create(i.ToString(), values[i]);
            }

            return orderedParams;
        }

        #endregion Methods
    }
}
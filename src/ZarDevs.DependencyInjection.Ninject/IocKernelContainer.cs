using Ninject;
using Ninject.Modules;
using Ninject.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using ZarDevs.Runtime;

namespace ZarDevs.DependencyInjection
{
    public class IocKernelContainer : IIocKernelContainer
    {
        #region Fields

        private bool _disposed = false;
        private IDependencyContainer _dependencyContainer;

        #endregion Fields

        #region Constructors

        public IocKernelContainer(IKernel kernel)
        {
            Kernel = kernel ?? throw new ArgumentNullException(nameof(kernel));
        }

        #endregion Constructors

        #region Properties

        public IKernel Kernel { get; }

        #endregion Properties

        #region Methods

        public IDependencyContainer CreateDependencyContainer()
        {
            return _dependencyContainer = DependencyContainer.Create(Kernel);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public bool HasModule(string name)
        {
            return Kernel.HasModule(name);
        }

        public T Resolve<T>(params (string, object)[] parameters)
        {
            return Kernel.Get<T>(CreateParameters(parameters));
        }

        public T ResolveNamed<T>(string name, params (string, object)[] parameters)
        {
            return Kernel.Get<T>(name, CreateParameters(parameters));
        }

        public T ResolveWithKey<T>(Enum key, params (string, object)[] parameters)
        {
            return Kernel.Get<T>(key.GetBindingName(), CreateParameters(parameters));
        }

        public T ResolveWithKey<T>(object key, params (string, object)[] parameters)
        {
            if (key is null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            return Kernel.Get<T>(key.ToString(), CreateParameters(parameters));
        }

        public T Resolve<T>()
        {
            return Resolve<T>(new (string, object)[0]);
        }

        public T ResolveNamed<T>(string name)
        {
            return ResolveNamed<T>(name, new (string, object)[0]);
        }

        public T ResolveWithKey<T>(Enum key)
        {
            return ResolveWithKey<T>(key, new (string, object)[0]);
        }

        public T ResolveWithKey<T>(object key)
        {
            return ResolveWithKey<T>(key, new (string, object)[0]);
        }

        public T Resolve<T>(params object[] parameters)
        {
            return Resolve<T>(CreateParameters(null, typeof(T), parameters));
        }

        public T ResolveNamed<T>(string name, params object[] parameters)
        {
            return ResolveNamed<T>(name, CreateParameters(name, typeof(T), parameters));
        }

        public T ResolveWithKey<T>(Enum key, params object[] parameters)
        {
            return ResolveWithKey<T>(key.GetBindingName(), parameters);
        }

        public T ResolveWithKey<T>(object key, params object[] parameters)
        {
            return ResolveWithKey<T>(key.ToString(), parameters);
        }

        public T TryResolve<T>(params (string, object)[] parameters)
        {
            return Kernel.TryGet<T>(CreateParameters(parameters));
        }

        public T TryResolveNamed<T>(string name, params (string, object)[] parameters)
        {
            return Kernel.TryGet<T>(name, CreateParameters(parameters));
        }

        public T TryResolveWithKey<T>(Enum key, params (string, object)[] parameters)
        {
            return TryResolveWithKey<T>(key.GetBindingName(), parameters);
        }

        public T TryResolveWithKey<T>(object key, params (string, object)[] parameters)
        {
            return TryResolveWithKey<T>(key.ToString(), parameters);
        }

        public T TryResolve<T>()
        {
            return TryResolve<T>(new (string, object)[0]);
        }

        public T TryResolveNamed<T>(string name)
        {
            return TryResolveNamed<T>(name, new (string, object)[0]);
        }

        public T TryResolveWithKey<T>(Enum key)
        {
            return TryResolveWithKey<T>(key, new (string, object)[0]);
        }

        public T TryResolveWithKey<T>(object key)
        {
            return TryResolveWithKey<T>(key, new (string, object)[0]);
        }

        public T TryResolve<T>(params object[] parameters)
        {
            return TryResolve<T>(CreateParameters(null, typeof(T), parameters));
        }

        public T TryResolveNamed<T>(string name, params object[] parameters)
        {
            return TryResolveNamed<T>(name, CreateParameters(name, typeof(T), parameters));
        }

        public T TryResolveWithKey<T>(Enum key, params object[] parameters)
        {
            return TryResolveWithKey<T>(key, CreateParameters(key, typeof(T), parameters));
        }

        public T TryResolveWithKey<T>(object key, params object[] parameters)
        {
            return TryResolveWithKey<T>(key.ToString(), parameters);
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

        private IParameter[] CreateParameters(IList<(string, object)> parameters)
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

        private (string, object)[] CreateParameters(object key, Type requestType, object[] values)
        {
            if (values == null) return null;
            if (values.Length == 0) return new (string, object)[0];

            var binding = _dependencyContainer.GetBinding(requestType, key);

            if(binding is IDependencyTypeInfo typeInfo)
                return InspectConstructor.Instance.FindParameterNames(typeInfo.ResolvedType, values).ToArray();

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
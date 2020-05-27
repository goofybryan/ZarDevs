using Ninject;
using Ninject.Modules;
using Ninject.Parameters;
using System;
using System.Collections.Generic;

namespace ZarDevs.DependencyInjection
{
    public class IocKernelContainer : IIocKernelContainer
    {
        #region Fields

        private bool _disposed = false;

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
            return DependencyContainer.Create(Kernel);
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

        public void Load(IDependencyModule module)
        {
            if (module is INinjectModule ninjectModule)
                Kernel.Load(ninjectModule);
        }

        public T Resolve<T>(params KeyValuePair<string, object>[] parameters)
        {
            return Kernel.Get<T>(CreateParameters(parameters));
        }

        public T Resolve<T>(string name, params KeyValuePair<string, object>[] parameters)
        {
            return Kernel.Get<T>(name, CreateParameters(parameters));
        }

        public T Resolve<T>(Enum enumValue, params KeyValuePair<string, object>[] parameters)
        {
            return Kernel.Get<T>(enumValue.GetBindingName(), CreateParameters(parameters));
        }

        public T Resolve<T>(object key, params KeyValuePair<string, object>[] parameters)
        {
            if (key is null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            return Resolve<T>(key.ToString(), parameters);
        }

        public T Resolve<T>()
        {
            return Resolve<T>(new KeyValuePair<string, object>[0]);
        }

        public T Resolve<T>(string name)
        {
            return Resolve<T>(name, new KeyValuePair<string, object>[0]);
        }

        public T Resolve<T>(Enum enumValue)
        {
            return Resolve<T>(enumValue, new KeyValuePair<string, object>[0]);
        }

        public T Resolve<T>(object key)
        {
            return Resolve<T>(key, new KeyValuePair<string, object>[0]);
        }

        public T TryResolve<T>(params KeyValuePair<string, object>[] parameters)
        {
            return Kernel.TryGet<T>(CreateParameters(parameters));
        }

        public T TryResolve<T>(string name, params KeyValuePair<string, object>[] parameters)
        {
            return Kernel.TryGet<T>(name, CreateParameters(parameters));
        }

        public T TryResolve<T>(Enum enumValue, params KeyValuePair<string, object>[] parameters)
        {
            return TryResolve<T>(enumValue.GetBindingName(), parameters);
        }

        public T TryResolve<T>(object key, params KeyValuePair<string, object>[] parameters)
        {
            return TryResolve<T>(key.GetType().FullName, parameters);
        }

        public T TryResolve<T>()
        {
            return TryResolve<T>(new KeyValuePair<string, object>[0]);
        }

        public T TryResolve<T>(string name)
        {
            return TryResolve<T>(name, new KeyValuePair<string, object>[0]);
        }

        public T TryResolve<T>(Enum enumValue)
        {
            return TryResolve<T>(enumValue, new KeyValuePair<string, object>[0]);
        }

        public T TryResolve<T>(object key)
        {
            return TryResolve<T>(key, new KeyValuePair<string, object>[0]);
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

        private IParameter[] CreateParameters(KeyValuePair<string, object>[] parameters)
        {
            if (parameters == null)
                return null;

            var list = new List<IParameter>();

            foreach (var parameter in parameters)
            {
                var constructorArg = new ConstructorArgument(parameter.Key, parameter.Value);
                list.Add(constructorArg);
            }

            return list.ToArray();
        }

        #endregion Methods
    }
}
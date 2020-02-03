using ZarDevs.Core;
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
            Kernel = Check.IsNotNull(kernel, nameof(kernel));
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
            throw new NotImplementedException();
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
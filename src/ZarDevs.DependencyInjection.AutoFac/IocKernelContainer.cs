using Autofac;
using Autofac.Builder;
using Autofac.Core;
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

        public IocKernelContainer(ContainerBuildOptions buildOptions)
        {
            Container = DependencyContainer.Create(buildOptions);
        }

        #endregion Constructors

        #region Properties

        public ContainerBuildOptions BuildOptions { get; }
        public IAutoFacDependencyContainer Container { get; }
        public IContainer Kernel => Container.Container;

        #endregion Properties

        #region Methods

        public IDependencyContainer CreateDependencyContainer()
        {
            return Container;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public T Resolve<T>(params KeyValuePair<string, object>[] parameters)
        {
            return Kernel.Resolve<T>(CreateParameters(parameters));
        }

        public T Resolve<T>(string name, params KeyValuePair<string, object>[] parameters)
        {
            return Kernel.ResolveNamed<T>(name, CreateParameters(parameters));
        }

        public T Resolve<T>(Enum enumValue, params KeyValuePair<string, object>[] parameters)
        {
            return Kernel.ResolveKeyed<T>(enumValue, CreateParameters(parameters));
        }

        public T Resolve<T>(object key, params KeyValuePair<string, object>[] parameters)
        {
            return Kernel.ResolveKeyed<T>(key, CreateParameters(parameters));
        }

        public T TryResolve<T>(params KeyValuePair<string, object>[] parameters)
        {
            return Kernel.IsRegistered<T>() ? Resolve<T>(parameters) : default;
        }

        public T TryResolve<T>(string name, params KeyValuePair<string, object>[] parameters)
        {
            return Kernel.IsRegisteredWithName<T>(name) ? Resolve<T>(name) : default;
        }

        public T TryResolve<T>(Enum enumValue, params KeyValuePair<string, object>[] parameters)
        {
            return Kernel.IsRegisteredWithKey<T>(enumValue) ? Resolve<T>(enumValue, parameters) : default;
        }

        public T TryResolve<T>(object key, params KeyValuePair<string, object>[] parameters)
        {
            return Kernel.IsRegisteredWithKey<T>(key) ? Resolve<T>(key, parameters) : default;
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

        private Parameter[] CreateParameters(KeyValuePair<string, object>[] parameters)
        {
            if (parameters == null)
                return null;

            var list = new List<Parameter>();

            foreach (var parameter in parameters)
            {
                Parameter constructorArg = new NamedParameter(parameter.Key, parameter.Value);
                list.Add(constructorArg);
            }

            return list.ToArray();
        }

        #endregion Methods
    }
}
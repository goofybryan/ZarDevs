using Autofac;
using Autofac.Builder;
using Autofac.Core;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public T Resolve<T>(params (string, object)[] parameters)
        {
            return Kernel.Resolve<T>(CreateParameters(parameters));
        }

        public T Resolve<T>(string name, params (string, object)[] parameters)
        {
            return Kernel.ResolveNamed<T>(name, CreateParameters(parameters));
        }

        public T Resolve<T>(Enum enumValue, params (string, object)[] parameters)
        {
            return Kernel.ResolveKeyed<T>(enumValue, CreateParameters(parameters));
        }

        public T Resolve<T>(object key, params (string, object)[] parameters)
        {
            return Kernel.ResolveKeyed<T>(key, CreateParameters(parameters));
        }

        public T Resolve<T>()
        {
            return Resolve<T>(new (string, object)[0]);
        }

        public T Resolve<T>(string name)
        {
            return Resolve<T>(name, new (string, object)[0]);
        }

        public T Resolve<T>(Enum enumValue)
        {
            return Resolve<T>(enumValue, new (string, object)[0]);
        }

        public T Resolve<T>(object key)
        {
            return Resolve<T>(key, new (string, object)[0]);
        }

        public T Resolve<T>(params object[] parameters)
        {
            return Kernel.Resolve<T>(CreateParameters(parameters));
        }

        public T Resolve<T>(string name, params object[] parameters)
        {
            return Kernel.ResolveNamed<T>(name, CreateParameters(parameters));
        }

        public T Resolve<T>(Enum enumValue, params object[] parameters)
        {
            return Kernel.ResolveKeyed<T>(enumValue, CreateParameters(parameters));
        }

        public T Resolve<T>(object key, params object[] parameters)
        {
            return Kernel.ResolveKeyed<T>(key, CreateParameters(parameters));
        }

        public T TryResolve<T>(params (string, object)[] parameters)
        {
            return Kernel.IsRegistered<T>() ? Resolve<T>(parameters) : default;
        }

        public T TryResolve<T>(string name, params (string, object)[] parameters)
        {
            return Kernel.IsRegisteredWithName<T>(name) ? Resolve<T>(name) : default;
        }

        public T TryResolve<T>(Enum enumValue, params (string, object)[] parameters)
        {
            return Kernel.IsRegisteredWithKey<T>(enumValue) ? Resolve<T>(enumValue, parameters) : default;
        }

        public T TryResolve<T>(object key, params (string, object)[] parameters)
        {
            return Kernel.IsRegisteredWithKey<T>(key) ? Resolve<T>(key, parameters) : default;
        }

        public T TryResolve<T>()
        {
            return TryResolve<T>(new (string, object)[0]);
        }

        public T TryResolve<T>(string name)
        {
            return TryResolve<T>(name, new (string, object)[0]);
        }

        public T TryResolve<T>(Enum enumValue)
        {
            return TryResolve<T>(enumValue, new (string, object)[0]);
        }

        public T TryResolve<T>(object key)
        {
            return TryResolve<T>(key, new (string, object)[0]);
        }

        public T TryResolve<T>(params object[] parameters)
        {
            return Kernel.IsRegistered<T>() ? Resolve<T>(parameters) : default;
        }

        public T TryResolve<T>(string name, params object[] parameters)
        {
            return Kernel.IsRegistered<T>() ? Resolve<T>(name, parameters) : default;
        }

        public T TryResolve<T>(Enum enumValue, params object[] parameters)
        {
            return Kernel.IsRegistered<T>() ? Resolve<T>(enumValue, parameters) : default;
        }

        public T TryResolve<T>(object key, params object[] parameters)
        {
            return Kernel.IsRegistered<T>() ? Resolve<T>(key, parameters) : default;
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

        private Parameter[] CreateParameters(object[] parameters)
        {
            if (parameters == null)
                return null;

            var list = new Parameter[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                list[i] = new PositionalParameter(i, parameters[i]);
            }

            return list;
        }

        private Parameter[] CreateParameters((string, object)[] parameters)
        {
            if (parameters == null)
                return null;

            var list = new List<Parameter>();

            foreach (var parameter in parameters)
            {
                Parameter constructorArg = new NamedParameter(parameter.Item1, parameter.Item2);
                list.Add(constructorArg);
            }

            return list.ToArray();
        }

        #endregion Methods
    }
}
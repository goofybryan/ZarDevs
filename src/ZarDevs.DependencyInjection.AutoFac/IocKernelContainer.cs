using Autofac;
using Autofac.Builder;
using Autofac.Core;
using System;
using System.Collections.Generic;

namespace ZarDevs.DependencyInjection
{
    public class IocContainer : IIocContainer
    {
        #region Fields

        private bool _disposed = false;

        #endregion Fields

        #region Constructors

        public IocContainer(IAutoFacDependencyContainer container)
        {
            Container = container ?? throw new ArgumentNullException(nameof(container));
        }

        #endregion Constructors

        #region Properties

        public IAutoFacDependencyContainer Container { get; }
        public IContainer Kernel => Container.Container;

        #endregion Properties

        #region Methods

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public T Resolve<T>(params (string, object)[] parameters)
        {
            return Kernel.Resolve<T>(CreateParameters(parameters));
        }

        public T Resolve<T>()
        {
            return Resolve<T>(new (string, object)[0]);
        }

        public T Resolve<T>(params object[] parameters)
        {
            return Kernel.Resolve<T>(CreateParameters(parameters));
        }

        public T ResolveNamed<T>(string name, params (string, object)[] parameters)
        {
            return Kernel.ResolveNamed<T>(name, CreateParameters(parameters));
        }

        public T ResolveNamed<T>(string name)
        {
            return ResolveNamed<T>(name, new (string, object)[0]);
        }

        public T ResolveNamed<T>(string name, params object[] parameters)
        {
            return Kernel.ResolveNamed<T>(name, CreateParameters(parameters));
        }

        public T ResolveWithKey<T>(Enum key, params (string, object)[] parameters)
        {
            return Kernel.ResolveKeyed<T>(key, CreateParameters(parameters));
        }

        public T ResolveWithKey<T>(object key, params (string, object)[] parameters)
        {
            return Kernel.ResolveKeyed<T>(key, CreateParameters(parameters));
        }

        public T ResolveWithKey<T>(Enum key)
        {
            return ResolveWithKey<T>(key, new (string, object)[0]);
        }

        public T ResolveWithKey<T>(object key)
        {
            return ResolveWithKey<T>(key, new (string, object)[0]);
        }

        public T ResolveWithKey<T>(Enum key, params object[] parameters)
        {
            return Kernel.ResolveKeyed<T>(key, CreateParameters(parameters));
        }

        public T ResolveWithKey<T>(object key, params object[] parameters)
        {
            return Kernel.ResolveKeyed<T>(key, CreateParameters(parameters));
        }

        public T TryResolve<T>(params (string, object)[] parameters)
        {
            return Kernel.IsRegistered<T>() ? Resolve<T>(parameters) : default;
        }

        public T TryResolve<T>()
        {
            return TryResolve<T>(new (string, object)[0]);
        }

        public T TryResolve<T>(params object[] parameters)
        {
            return Kernel.IsRegistered<T>() ? Resolve<T>(parameters) : default;
        }

        public object TryResolve(Type requestType)
        {
            return Kernel.IsRegistered(requestType) ? Kernel.Resolve(requestType) : null;
        }

        public T TryResolveNamed<T>(string name, params (string, object)[] parameters)
        {
            return Kernel.IsRegisteredWithName<T>(name) ? ResolveNamed<T>(name, parameters) : default;
        }

        public T TryResolveNamed<T>(string name)
        {
            return TryResolveNamed<T>(name, new (string, object)[0]);
        }

        public T TryResolveNamed<T>(string name, params object[] parameters)
        {
            return Kernel.IsRegisteredWithName<T>(name) ? ResolveNamed<T>(name, parameters) : default;
        }

        public T TryResolveWithKey<T>(Enum key, params (string, object)[] parameters)
        {
            return Kernel.IsRegisteredWithKey<T>(key) ? ResolveWithKey<T>(key, parameters) : default;
        }

        public T TryResolveWithKey<T>(object key, params (string, object)[] parameters)
        {
            return Kernel.IsRegisteredWithKey<T>(key) ? ResolveWithKey<T>(key, parameters) : default;
        }

        public T TryResolveWithKey<T>(Enum key)
        {
            return TryResolveWithKey<T>(key, new (string, object)[0]);
        }

        public T TryResolveWithKey<T>(object key)
        {
            return TryResolveWithKey<T>(key, new (string, object)[0]);
        }

        public T TryResolveWithKey<T>(Enum key, params object[] parameters)
        {
            return Kernel.IsRegisteredWithKey<T>(key) ? ResolveWithKey<T>(key, parameters) : default;
        }

        public T TryResolveWithKey<T>(object key, params object[] parameters)
        {
            return Kernel.IsRegisteredWithKey<T>(key) ? ResolveWithKey<T>(key, parameters) : default;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Kernel?.Dispose();
                    Container?.Dispose();
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

    public class IocKernelContainer : IIocKernelContainer
    {
        #region Fields

        private readonly IAutoFacDependencyContainer _container;

        #endregion Fields

        #region Constructors

        public IocKernelContainer(ContainerBuildOptions buildOptions)
        {
            _container = DependencyContainer.Create(buildOptions);
        }

        #endregion Constructors

        #region Methods

        public IDependencyBuilder CreateDependencyBuilder()
        {
            return new DependencyBuilder(_container);
        }

        public IDependencyContainer CreateDependencyContainer()
        {
            return _container;
        }

        public IIocContainer CreateIocContainer()
        {
            return new IocContainer(_container);
        }

        #endregion Methods
    }
}
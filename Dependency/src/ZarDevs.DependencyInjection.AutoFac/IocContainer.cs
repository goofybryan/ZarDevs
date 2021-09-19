using Autofac;
using Autofac.Core;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ZarDevs.DependencyInjection
{
    internal class IocContainer : IIocContainer<IContainer>
    {
        #region Fields

        private bool _disposed = false;

        #endregion Fields

        #region Constructors

        public IocContainer(IAutoFacDependencyContainer container)
        {
            Container = container ?? throw new ArgumentNullException(nameof(container));
            Kernel = Container.Container;
        }

        #endregion Constructors

        #region Properties

        public IContainer Kernel { get; }

        private IAutoFacDependencyContainer Container { get; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Disposed of the resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public T Resolve<T>(params (string, object)[] parameters) where T : class
        {
            return Kernel.Resolve<T>(CreateParameters(parameters));
        }

        public T Resolve<T>() where T : class
        {
            return Kernel.Resolve<T>();
        }

        public T Resolve<T>(params object[] parameters) where T : class
        {
            return Kernel.Resolve<T>(CreateParameters(parameters));
        }

        public IEnumerable<T> ResolveAll<T>() where T : class
        {
            return Kernel.Resolve<IMultipleResolver<T>>().Resolved;
        }

        public IEnumerable ResolveAll(Type requestType)
        {
            return ((MultipleResolver)Kernel.Resolve(MultipleResolver.MakeConcreateOfType(requestType))).ResolvedAsObject;
        }

        public T ResolveNamed<T>(string name, params (string, object)[] parameters) where T : class
        {
            return Kernel.ResolveNamed<T>(name, CreateParameters(parameters));
        }

        public T ResolveNamed<T>(string name) where T : class
        {
            return Kernel.ResolveNamed<T>(name);
        }

        public T ResolveNamed<T>(string name, params object[] parameters) where T : class
        {
            return Kernel.ResolveNamed<T>(name, CreateParameters(parameters));
        }

        public T ResolveWithKey<T>(Enum key, params (string, object)[] parameters) where T : class
        {
            return Kernel.ResolveKeyed<T>(key, CreateParameters(parameters));
        }

        public T ResolveWithKey<T>(object key, params (string, object)[] parameters) where T : class
        {
            return Kernel.ResolveKeyed<T>(key, CreateParameters(parameters));
        }

        public T ResolveWithKey<T>(Enum key) where T : class
        {
            return ResolveWithKey<T>(key, Array.Empty<(string, object)>());
        }

        public T ResolveWithKey<T>(object key) where T : class
        {
            return ResolveWithKey<T>(key, Array.Empty<(string, object)>());
        }

        public T ResolveWithKey<T>(Enum key, params object[] parameters) where T : class
        {
            return Kernel.ResolveKeyed<T>(key, CreateParameters(parameters));
        }

        public T ResolveWithKey<T>(object key, params object[] parameters) where T : class
        {
            return Kernel.ResolveKeyed<T>(key, CreateParameters(parameters));
        }

        public T TryResolve<T>(params (string, object)[] parameters) where T : class
        {
            return Kernel.IsRegistered<T>() ? Resolve<T>(parameters) : default;
        }

        public T TryResolve<T>() where T : class
        {
            return Kernel.IsRegistered<T>() ? Resolve<T>() : default;
        }

        public T TryResolve<T>(params object[] parameters) where T : class
        {
            return Kernel.IsRegistered<T>() ? Resolve<T>(parameters) : default;
        }

        public object TryResolve(Type requestType)
        {
            return Kernel.IsRegistered(requestType) ? Kernel.Resolve(requestType) : null;
        }

        public T TryResolveNamed<T>(string name, params (string, object)[] parameters) where T : class
        {
            return Kernel.IsRegisteredWithName<T>(name) ? ResolveNamed<T>(name, parameters) : default;
        }

        public T TryResolveNamed<T>(string name) where T : class
        {
            return TryResolveNamed<T>(name, Array.Empty<(string, object)>());
        }

        public T TryResolveNamed<T>(string name, params object[] parameters) where T : class
        {
            return Kernel.IsRegisteredWithName<T>(name) ? ResolveNamed<T>(name, parameters) : default;
        }

        public object TryResolveNamed(Type requestType, string name, params object[] parameters)
        {
            return Kernel.IsRegisteredWithName(name, requestType) ? Kernel.ResolveNamed(name, requestType, CreateParameters(parameters)) : default;
        }

        public T TryResolveWithKey<T>(Enum key, params (string, object)[] parameters) where T : class
        {
            return Kernel.IsRegisteredWithKey<T>(key) ? ResolveWithKey<T>(key, parameters) : default;
        }

        public T TryResolveWithKey<T>(object key, params (string, object)[] parameters) where T : class
        {
            return Kernel.IsRegisteredWithKey<T>(key) ? ResolveWithKey<T>(key, parameters) : default;
        }

        public T TryResolveWithKey<T>(Enum key) where T : class
        {
            return TryResolveWithKey<T>(key, Array.Empty<(string, object)>());
        }

        public T TryResolveWithKey<T>(object key) where T : class
        {
            return TryResolveWithKey<T>(key, Array.Empty<(string, object)>());
        }

        public T TryResolveWithKey<T>(Enum key, params object[] parameters) where T : class
        {
            return Kernel.IsRegisteredWithKey<T>(key) ? ResolveWithKey<T>(key, parameters) : default;
        }

        public T TryResolveWithKey<T>(object key, params object[] parameters) where T : class
        {
            return Kernel.IsRegisteredWithKey<T>(key) ? ResolveWithKey<T>(key, parameters) : default;
        }

        public object TryResolveWithKey(Type requestType, Enum key, params object[] parameters)
        {
            return Kernel.IsRegisteredWithKey(key, requestType) ? Kernel.ResolveKeyed(key, requestType, CreateParameters(parameters)) : default;
        }

        public object TryResolveWithKey(Type requestType, object key, params object[] parameters)
        {
            return Kernel.IsRegisteredWithKey(key, requestType) ? Kernel.ResolveKeyed(key, requestType, CreateParameters(parameters)) : default;
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

        private static Parameter[] CreateParameters(object[] parameters)
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

        private static Parameter[] CreateParameters((string, object)[] parameters)
        {
            if (parameters == null || parameters.Length == 0)
                return Array.Empty<Parameter>();

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
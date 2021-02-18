using System;
using System.Collections.Generic;
using System.Threading;

namespace ZarDevs.DependencyInjection
{
    public static class Ioc
    {
        #region Fields

        private static readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

        [ThreadStatic]
        private static IIocContainer _container;

        private static IIocKernelContainer _kernel;

        #endregion Fields

        #region Properties

        public static IIocContainer Container
        {
            get => _container ?? (_container = new IocContainer(_kernel));
        }

        public static IIocKernelContainer GetIocKernel() => _kernel;

        #endregion Properties

        #region Methods

        public static void Dispose()
        {
            _kernel?.Dispose();
            _kernel = null;
        }

        public static IIocKernelContainer Initialize(IIocKernelContainer container)
        {
            _lock.EnterWriteLock();
            try
            {
                _kernel = container ?? throw new ArgumentNullException(nameof(container));
                return _kernel;
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public static IDependencyBuilder InitializeWithBuilder(IIocKernelContainer container)
        {
            var dependencyContainer = Initialize(container).CreateDependencyContainer();
            return new DependencyBuilder(dependencyContainer);
        }

        public static T Resolve<T>(params (string, object)[] parameters)
        {
            return Container.Resolve<T>(parameters);
        }

        public static T Resolve<T>(string name, params (string, object)[] parameters)
        {
            return Container.Resolve<T>(name, parameters);
        }

        public static T Resolve<T>(Enum enumValue, params (string, object)[] parameters)
        {
            return Container.Resolve<T>(enumValue, parameters);
        }

        public static T Resolve<T>(object key, params (string, object)[] parameters)
        {
            return Container.Resolve<T>(key, parameters);
        }

        public static T TryResolve<T>(params (string, object)[] parameters)
        {
            return Container.TryResolve<T>(parameters);
        }

        public static T TryResolve<T>(string name, params (string, object)[] parameters)
        {
            return Container.TryResolve<T>(name, parameters);
        }

        public static T TryResolve<T>(Enum enumValue, params (string, object)[] parameters)
        {
            return Container.TryResolve<T>(enumValue, parameters);
        }

        public static T TryResolve<T>(object key, params (string, object)[] parameters)
        {
            return Container.TryResolve<T>(key, parameters);
        }

        #endregion Methods
    }
}
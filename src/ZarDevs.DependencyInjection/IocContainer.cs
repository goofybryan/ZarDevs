using System;

namespace ZarDevs.DependencyInjection
{
    [Obsolete("Not needed, if service locator pattern is required, just inject IocContainer into constructor.")]
    public class IocContainer : IIocContainer
    {
        #region Constructors

        public IocContainer(IIocKernelContainer kernel)
        {
            Kernel = kernel ?? throw new ArgumentNullException(nameof(kernel));
        }

        #endregion Constructors

        #region Properties

        public IIocKernelContainer Kernel { get; }

        #endregion Properties

        #region Methods

        public T Resolve<T>(params (string, object)[] parameters)
        {
            return Kernel.Resolve<T>(parameters);
        }

        public T Resolve<T>()
        {
            return Kernel.Resolve<T>();
        }

        public T Resolve<T>(params object[] parameters)
        {
            return Kernel.Resolve<T>(parameters);
        }

        public T ResolveNamed<T>(string name, params (string, object)[] parameters)
        {
            return Kernel.ResolveNamed<T>(name, parameters);
        }

        public T ResolveNamed<T>(string name)
        {
            return Kernel.ResolveNamed<T>(name);
        }

        public T ResolveNamed<T>(string name, params object[] parameters)
        {
            return Kernel.ResolveNamed<T>(name, parameters);
        }

        public T ResolveWithKey<T>(Enum key, params (string, object)[] parameters)
        {
            return Kernel.ResolveWithKey<T>(key, parameters);
        }

        public T ResolveWithKey<T>(object key, params (string, object)[] parameters)
        {
            return Kernel.ResolveWithKey<T>(key, parameters);
        }

        public T ResolveWithKey<T>(Enum key)
        {
            return Kernel.ResolveWithKey<T>(key);
        }

        public T ResolveWithKey<T>(object key)
        {
            return Kernel.ResolveWithKey<T>(key);
        }

        public T ResolveWithKey<T>(Enum key, params object[] parameters)
        {
            return Kernel.ResolveWithKey<T>(key, parameters);
        }

        public T ResolveWithKey<T>(object key, params object[] parameters)
        {
            return Kernel.ResolveWithKey<T>(key, parameters);
        }

        public T TryResolve<T>(params (string, object)[] parameters)
        {
            return Kernel.TryResolve<T>(parameters);
        }

        public T TryResolve<T>()
        {
            return Kernel.TryResolve<T>();
        }

        public T TryResolve<T>(params object[] parameters)
        {
            return Kernel.TryResolve<T>(parameters);
        }

        public T TryResolveNamed<T>(string name, params (string, object)[] parameters)
        {
            return Kernel.TryResolveNamed<T>(name, parameters);
        }

        public T TryResolveNamed<T>(string name)
        {
            return TryResolveNamed<T>(name);
        }

        public T TryResolveNamed<T>(string name, params object[] parameters)
        {
            return Kernel.TryResolveNamed<T>(name, parameters);
        }

        public T TryResolveWithKey<T>(Enum key, params (string, object)[] parameters)
        {
            return Kernel.TryResolveWithKey<T>(key, parameters);
        }

        public T TryResolveWithKey<T>(object key, params (string, object)[] parameters)
        {
            return Kernel.TryResolveWithKey<T>(key, parameters);
        }

        public T TryResolveWithKey<T>(Enum key)
        {
            return TryResolveWithKey<T>(key);
        }

        public T TryResolveWithKey<T>(object key)
        {
            return TryResolveWithKey<T>(key);
        }

        public T TryResolveWithKey<T>(Enum key, params object[] parameters)
        {
            return Kernel.TryResolveWithKey<T>(key, parameters);
        }

        public T TryResolveWithKey<T>(object key, params object[] parameters)
        {
            return Kernel.TryResolveWithKey<T>(key, parameters);
        }

        #endregion Methods
    }
}
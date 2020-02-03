using System;
using System.Collections.Generic;
using ZarDevs.Core;

namespace ZarDevs.DependencyInjection
{
    public class IocContainer : IIocContainer
    {
        #region Constructors

        public IocContainer(IIocKernelContainer kernel)
        {
            Kernel = Check.IsNotNull(kernel, nameof(kernel));
        }

        #endregion Constructors

        #region Properties

        public IIocKernelContainer Kernel { get; }

        #endregion Properties

        #region Methods

        public T Resolve<T>(params KeyValuePair<string, object>[] parameters)
        {
            return Kernel.Resolve<T>(parameters);
        }

        public T Resolve<T>(string name, params KeyValuePair<string, object>[] parameters)
        {
            return Kernel.Resolve<T>(name, parameters);
        }

        public T Resolve<T>(Enum enumValue, params KeyValuePair<string, object>[] parameters)
        {
            return Kernel.Resolve<T>(enumValue, parameters);
        }

        public T Resolve<T>(object key, params KeyValuePair<string, object>[] parameters)
        {
            return Kernel.Resolve<T>(key, parameters);
        }

        public T TryResolve<T>(params KeyValuePair<string, object>[] parameters)
        {
            return Kernel.TryResolve<T>(parameters);
        }

        public T TryResolve<T>(string name, params KeyValuePair<string, object>[] parameters)
        {
            return Kernel.TryResolve<T>(name, parameters);
        }

        public T TryResolve<T>(Enum enumValue, params KeyValuePair<string, object>[] parameters)
        {
            return Kernel.TryResolve<T>(enumValue, parameters);
        }

        public T TryResolve<T>(object key, params KeyValuePair<string, object>[] parameters)
        {
            return Kernel.TryResolve<T>(key, parameters);
        }

        #endregion Methods
    }
}
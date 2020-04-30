using Microsoft.Extensions.DependencyInjection;
using System;

namespace ZarDevs.DependencyInjection
{
    public interface INamedResolver
    {
        #region Methods

        T Resolve<T>(Enum enumVale);

        T Resolve<T>(string enumVale);

        T Resolve<T>(object enumVale);

        T TryResolve<T>(Enum enumValue);

        T TryResolve<T>(string enumValue);

        T TryResolve<T>(object enumValue);

        #endregion Methods
    }

    internal class NamedResolver : INamedResolver
    {
        #region Fields

        private readonly INamedServiceConfiguration _namedServiceConfiguration;
        private readonly IServiceProvider _serviceProvider;

        #endregion Fields

        #region Constructors

        public NamedResolver(IServiceProvider serviceProvider, INamedServiceConfiguration namedServiceConfiguration)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _namedServiceConfiguration = namedServiceConfiguration ?? throw new ArgumentNullException(nameof(namedServiceConfiguration));
        }

        #endregion Constructors

        #region Methods

        public T Resolve<T>(Enum enumVale)
        {
            Type instanceType = _namedServiceConfiguration.ResolveInstanceType<T>(enumVale);

            return (T)_serviceProvider.GetRequiredService(instanceType);
        }

        public T Resolve<T>(string enumVale)
        {
            Type instanceType = _namedServiceConfiguration.ResolveInstanceType<T>(enumVale);

            return (T)_serviceProvider.GetRequiredService(instanceType);
        }

        public T Resolve<T>(object enumVale)
        {
            Type instanceType = _namedServiceConfiguration.ResolveInstanceType<T>(enumVale);

            return (T)_serviceProvider.GetRequiredService(instanceType);
        }

        public T TryResolve<T>(Enum enumValue)
        {
            Type instanceType = _namedServiceConfiguration.ResolveInstanceType<T>(enumValue);

            return (T)_serviceProvider.GetService(instanceType);
        }

        public T TryResolve<T>(string enumValue)
        {
            Type instanceType = _namedServiceConfiguration.ResolveInstanceType<T>(enumValue);

            return (T)_serviceProvider.GetService(instanceType);
        }

        public T TryResolve<T>(object enumValue)
        {
            Type instanceType = _namedServiceConfiguration.ResolveInstanceType<T>(enumValue);

            return (T)_serviceProvider.GetService(instanceType);
        }

        #endregion Methods
    }
}
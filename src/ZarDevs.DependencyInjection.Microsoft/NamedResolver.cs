using Microsoft.Extensions.DependencyInjection;
using System;

namespace ZarDevs.DependencyInjection
{
    public interface INamedResolver
    {
        #region Methods

        T Resolve<T>(Enum enumVale, params object[] values);

        T Resolve<T>(string enumVale, params object[] values);

        T Resolve<T>(object enumVale, params object[] values);

        T TryResolve<T>(Enum enumValue, params object[] values);

        T TryResolve<T>(string enumValue, params object[] values);

        T TryResolve<T>(object enumValue, params object[] values);

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

        public T Resolve<T>(Enum enumVale, params object[] values)
        {
            Type instanceType = _namedServiceConfiguration.ResolveInstanceType<T>(enumVale) ?? throw new InvalidOperationException($"The type '{typeof(T)}' with enum value '{enumVale}' cannot be found.");

            return (T)ActivatorUtilities.CreateInstance(_serviceProvider, instanceType, values);
        }

        public T Resolve<T>(string name, params object[] values)
        {
            Type instanceType = _namedServiceConfiguration.ResolveInstanceType<T>(name) ?? throw new InvalidOperationException($"The type '{typeof(T)}' with name '{name}' cannot be found.");

            return (T)ActivatorUtilities.CreateInstance(_serviceProvider, instanceType, values);
        }

        public T Resolve<T>(object key, params object[] values)
        {
            if (key is null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            Type instanceType = _namedServiceConfiguration.ResolveInstanceType<T>(key) ?? throw new InvalidOperationException($"The type '{typeof(T)}' with key object '{key}' cannot be found.");

            return (T)ActivatorUtilities.CreateInstance(_serviceProvider, instanceType, values);
        }

        public T TryResolve<T>(Enum enumValue, params object[] values)
        {
            Type instanceType = _namedServiceConfiguration.ResolveInstanceType<T>(enumValue);

            return instanceType == null ? default : Resolve<T>(values);
        }

        public T TryResolve<T>(string enumValue, params object[] values)
        {
            Type instanceType = _namedServiceConfiguration.ResolveInstanceType<T>(enumValue);

            return instanceType == null ? default : Resolve<T>(values);
        }

        public T TryResolve<T>(object enumValue, params object[] values)
        {
            Type instanceType = _namedServiceConfiguration.ResolveInstanceType<T>(enumValue);

            return instanceType == null ? default : Resolve<T>(values);
        }

        #endregion Methods
    }
}
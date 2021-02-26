using Microsoft.Extensions.DependencyInjection;
using System;
using ZarDevs.Runtime;

namespace ZarDevs.DependencyInjection
{
    public interface IIocKernelServiceProvider
    {
        #region Methods

        void ConfigureServiceProvider(IServiceProvider serviceProvider);

        #endregion Methods
    }

    public class IocContainer : IIocContainer
    {
        #region Fields

        private IDependencyResolver _dependencyResolver;
        private bool _isDisposed;
        private IServiceProvider _serviceProvider;

        #endregion Fields

        #region Constructors

        public IocContainer(IDependencyResolver dependencyResolver, IServiceProvider serviceProvider)
        {
            _dependencyResolver = dependencyResolver ?? throw new ArgumentNullException(nameof(dependencyResolver));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        #endregion Constructors

        #region Methods

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public T Resolve<T>(params (string, object)[] parameters)
        {
            return _dependencyResolver.Resolve<T>(parameters);
        }

        public T Resolve<T>()
        {
            return (T)_serviceProvider.GetRequiredService(typeof(T));
        }

        public T Resolve<T>(params object[] parameters)
        {
            return _dependencyResolver.Resolve<T>(parameters);
        }

        public T ResolveNamed<T>(string name, params (string, object)[] parameters)
        {
            return _dependencyResolver.ResolveNamed<T>(name, parameters);
        }

        public T ResolveNamed<T>(string name)
        {
            return _dependencyResolver.ResolveNamed<T>(name);
        }

        public T ResolveNamed<T>(string name, params object[] parameters)
        {
            return _dependencyResolver.ResolveNamed<T>(name, parameters);
        }

        public T ResolveWithKey<T>(Enum key, params (string, object)[] parameters)
        {
            return ResolveWithKey<T>(key, parameters);
        }

        public T ResolveWithKey<T>(object key, params (string, object)[] parameters)
        {
            return ResolveWithKey<T>(key, parameters);
        }

        public T ResolveWithKey<T>(Enum key)
        {
            return _dependencyResolver.ResolveWithKey<T>(key);
        }

        public T ResolveWithKey<T>(object key)
        {
            return _dependencyResolver.ResolveWithKey<T>(key);
        }

        public T ResolveWithKey<T>(Enum key, params object[] parameters)
        {
            return _dependencyResolver.ResolveWithKey<T>(key, parameters);
        }

        public T ResolveWithKey<T>(object key, params object[] parameters)
        {
            return _dependencyResolver.ResolveWithKey<T>(key, parameters);
        }

        public T TryResolve<T>(params (string, object)[] parameters)
        {
            return _dependencyResolver.TryResolve<T>(parameters);
        }

        public T TryResolve<T>()
        {
            return (T)TryResolve(typeof(T));
        }

        public T TryResolve<T>(params object[] parameters)
        {
            return _dependencyResolver.TryResolve<T>(parameters);
        }

        public object TryResolve(Type requestType)
        {
            return _serviceProvider.GetService(requestType);
        }

        public T TryResolveNamed<T>(string name, params (string, object)[] parameters)
        {
            return _dependencyResolver.TryResolveNamed<T>(name, parameters);
        }

        public T TryResolveNamed<T>(string name)
        {
            return _dependencyResolver.TryResolveNamed<T>(name);
        }

        public T TryResolveNamed<T>(string name, params object[] parameters)
        {
            return _dependencyResolver.TryResolveNamed<T>(name, parameters);
        }

        public T TryResolveWithKey<T>(Enum key, params (string, object)[] parameters)
        {
            return _dependencyResolver.TryResolveWithKey<T>(key, parameters);
        }

        public T TryResolveWithKey<T>(object key, params (string, object)[] parameters)
        {
            return _dependencyResolver.TryResolveWithKey<T>(key, parameters);
        }

        public T TryResolveWithKey<T>(Enum key)
        {
            return _dependencyResolver.TryResolveWithKey<T>(key);
        }

        public T TryResolveWithKey<T>(object key)
        {
            return _dependencyResolver.TryResolveWithKey<T>(key);
        }

        public T TryResolveWithKey<T>(Enum key, params object[] parameters)
        {
            return _dependencyResolver.TryResolveWithKey<T>(key, parameters);
        }

        public T TryResolveWithKey<T>(object key, params object[] parameters)
        {
            return _dependencyResolver.TryResolveWithKey<T>(key, parameters);
        }

        private void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    _dependencyResolver.Dispose();
                    _dependencyResolver = null;

                    _serviceProvider = null;
                }

                _isDisposed = true;
            }
        }

        #endregion Methods
    }

    public sealed class IocKernelContainer : IIocKernelContainer, IIocKernelServiceProvider
    {
        #region Fields

        private readonly IServiceCollection _serviceCollection;
        private readonly IDependencyInstanceConfiguration _resolutionConfiguration;

        #endregion Fields

        #region Constructors

        public IocKernelContainer(IServiceCollection serviceCollection)
        {
            _serviceCollection = serviceCollection ?? throw new ArgumentNullException(nameof(serviceCollection));
            _resolutionConfiguration = new DependencyResolutionConfiguration();
        }

        #endregion Constructors

        #region Methods

        public void ConfigureServiceProvider(IServiceProvider serviceProvider)
        {
            _resolutionConfiguration.AddInstanceResolution(serviceProvider, null);
        }

        public IDependencyBuilder CreateDependencyBuilder()
        {
            var activator = new MicrosoftUtilitiesActivator(InspectConstructor.Instance);
            var dependencyContainer = new MicrosoftDependencyContainer(_serviceCollection, _resolutionConfiguration, activator);
            var builder = new DependencyBuilder(dependencyContainer);

            builder.Bind<IInspectConstructor>().To(InspectConstructor.Instance);
            builder.Bind<ICreate>().To(Create.Instance);
            builder.Bind<IDependencyInstanceResolution>().To(_resolutionConfiguration);
            builder.Bind<IDependencyTypeActivator>().To(activator);
            builder.Bind<IDependencyResolver>().To<DependencyResolver>();

            return builder;
        }

        public IIocContainer CreateIocContainer()
        {
            var resolution = (IDependencyInstanceResolution)_resolutionConfiguration;
            var serviceProvider = (IServiceProvider)resolution.GetResolution(typeof(IServiceProvider)).Resolve(null);
            var activator = serviceProvider.GetRequiredService<IDependencyResolver>();
            return new IocContainer(activator, serviceProvider);
        }

        #endregion Methods
    }
}
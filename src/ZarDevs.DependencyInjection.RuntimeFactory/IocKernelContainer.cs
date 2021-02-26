using System;
using ZarDevs.Runtime;

namespace ZarDevs.DependencyInjection
{
    public sealed class IocKernelContainer : IIocKernelContainer
    {
        public IocKernelContainer()
        {
            DependencyResolutionConfiguration configuration = new DependencyResolutionConfiguration();
            InstanceResolution = configuration;
            Activator = new RuntimeDependencyActivator(InspectConstructor.Instance, Create.Instance);
            Container = new DependencyContainer(configuration, Activator);
        }

        public IDependencyTypeActivator Activator { get; }
        public IDependencyContainer Container { get; }
        public IDependencyInstanceResolution InstanceResolution { get; }

        public IDependencyBuilder CreateDependencyBuilder()
        {
            var builder = new DependencyBuilder(Container);

            builder.Bind<IDependencyTypeActivator>().To(Activator)
                .Bind<IInspectConstructor>().To(InspectConstructor.Instance)
                .Bind<ICreate>().To(Create.Instance)
                .Bind<IDependencyInstanceResolution>().To(InstanceResolution);

            return builder;
        }

        public IIocContainer CreateIocContainer()
        {
            return new DependencyResolver(InstanceResolution);
        }
    }

    internal class IocContainer : IIocContainer
    { 
        #region Fields

        private readonly IDependencyResolver _dependencyResolver;
        private bool _isDisposed;

        #endregion Fields

        #region Constructors

        public IocContainer(IDependencyResolver dependencyResolver)
        {
            _dependencyResolver = dependencyResolver ?? throw new ArgumentNullException(nameof(dependencyResolver));
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
            return _dependencyResolver.Resolve<T>();
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
            return _dependencyResolver.ResolveNamed<T>(name, (object[])null);
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
            return _dependencyResolver.ResolveWithKey<T>(key, (object[])null);
        }

        public T ResolveWithKey<T>(object key)
        {
            return _dependencyResolver.ResolveWithKey<T>(key, (object[])null);
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
            return _dependencyResolver.TryResolve<T>();
        }

        public T TryResolve<T>(params object[] parameters)
        {
            return _dependencyResolver.TryResolve<T>(parameters);
        }

        public T TryResolveNamed<T>(string name, params (string, object)[] parameters)
        {
            return _dependencyResolver.TryResolveNamed<T>(name, parameters);
        }

        public T TryResolveNamed<T>(string name)
        {
            return _dependencyResolver.TryResolveNamed<T>(name, (object[])null);
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
            return _dependencyResolver.TryResolveWithKey<T>(key, (object[])null);
        }

        public T TryResolveWithKey<T>(object key)
        {
            return _dependencyResolver.TryResolveWithKey<T>(key, (object[])null);
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
            if (_isDisposed) return;

            if (disposing)
            {
                _dependencyResolver.Dispose();
            }
            _isDisposed = true;
        }

        #endregion Methods
    }
}
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

            builder.Bind<IDependencyTypeActivator>().To(Activator);
            builder.Bind<IInspectConstructor>().To(InspectConstructor.Instance);
            builder.Bind<ICreate>().To(Create.Instance);
            builder.Bind<IDependencyInstanceResolution>().To(InstanceResolution);

            return builder;
        }

        public IIocContainer CreateIocContainer()
        {
            return new DependencyResolver(InstanceResolution);
        }
    }
}
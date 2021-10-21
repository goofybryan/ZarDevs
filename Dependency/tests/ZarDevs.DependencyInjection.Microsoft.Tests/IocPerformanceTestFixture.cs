using Microsoft.Extensions.DependencyInjection;
using System;
using ZarDevs.DependencyInjection.Tests;

namespace ZarDevs.DependencyInjection.Microsoft.Tests
{
    public sealed class IocPerformanceTestFixture : IIocPerformanceTestFixture
    {
        #region Constructors

        public IocPerformanceTestFixture()
        {
            var services = new ServiceCollection();

            var kernel = IocServiceProvider.CreateBuilder(services);

            Container = Ioc.Initialize(kernel,
                builder => builder.ConfigurePerformanceTest(),
                builder => ((IIocKernelServiceProviderBuilder)builder).ConfigureServiceProvider(services.BuildServiceProvider())
            );

            ContainerDirect = ((IIocContainer<IServiceProvider>)Container).Kernel;
            ContainerComparison = CreateComparison();
        }

        #endregion Constructors

        #region Properties

        public IIocContainer Container { get; }

        public IServiceProvider ContainerComparison { get; }
        public IServiceProvider ContainerDirect { get; }
        public bool RunComparisonTests => true;

        #endregion Properties

        #region Methods

        public void Dispose()
        {
            Ioc.Instance.Dispose();
        }

        private IServiceProvider CreateComparison()
        {
            var services = new ServiceCollection();

            //builder.Bind<IPerformanceMethodTest>().To<PerformanceMethodTest>();
            services.AddTransient<IPerformanceMethodTest, PerformanceMethodTest>();

            //builder.Bind<IPerformanceMethodResultTest>().To((ctx, key) => ctx.Ioc.Resolve<IPerformanceMethodTest>().Method());
            services.AddTransient(ctx => PerformanceMethodTest.Method());

            //builder.Bind<IPerformanceConstructParam1Test>().To<PerformanceConstructParamTest>();
            services.AddTransient<IPerformanceConstructParam1Test, PerformanceConstructParamTest>();
            //builder.Bind<IPerformanceConstructParam2Test>().To<PerformanceConstructParamTest>();
            services.AddTransient<IPerformanceConstructParam2Test, PerformanceConstructParamTest>();
            //builder.Bind<IPerformanceConstructParam3Test>().To<PerformanceConstructParamTest>();
            services.AddTransient<IPerformanceConstructParam3Test, PerformanceConstructParamTest>();
            //builder.Bind<IPerformanceConstructTest>().To<PerformanceConstructTest>();
            services.AddTransient<IPerformanceConstructTest, PerformanceConstructTest>();
            //builder.Bind<IPerformanceSingletonTest>().To<PerformanceSingletonTest>().InSingletonScope();
            services.AddSingleton<IPerformanceSingletonTest, PerformanceSingletonTest>();
            //builder.Bind<IPerformanceInstanceTest>().To(new PerformanceInstanceTest());
            services.AddSingleton<IPerformanceInstanceTest>(new PerformanceInstanceTest());

            return services.BuildServiceProvider();
        }

        #endregion Methods
    }
}
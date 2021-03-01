using Autofac;
using ZarDevs.DependencyInjection.Tests;

namespace ZarDevs.DependencyInjection.AutoFac.Tests
{
    public sealed class IocPerformanceTestFixture : IIocPerformanceTestFixture
    {
        #region Constructors

        public IocPerformanceTestFixture()
        {
            var kernel = IocAutoFac.Initialize();

            Container = Ioc.Initialize(kernel, builder =>
            {
                builder.ConfigurePerformanceTest();
            });

            DirectContainer = ((IocContainer)Container).Container.Container;
            ContainerComparison = CreateComparison();
        }

        private IContainer CreateComparison()
        {
            var builder = new ContainerBuilder();

            //builder.Bind<IPerformanceMethodTest>().To<PerformanceMethodTest>();
            builder.RegisterType<PerformanceMethodTest>().As<IPerformanceMethodTest>();

            //builder.Bind<IPerformanceMethodResultTest>().To((ctx, key) => ctx.Ioc.Resolve<IPerformanceMethodTest>().Method());
            builder.Register(ctx => PerformanceMethodTest.Method()).As<IPerformanceMethodResultTest>();

            //builder.Bind<IPerformanceConstructParam1Test>().To<PerformanceConstructParamTest>();
            builder.RegisterType<PerformanceConstructParamTest>().As<IPerformanceConstructParam1Test>();
            //builder.Bind<IPerformanceConstructParam2Test>().To<PerformanceConstructParamTest>();
            builder.RegisterType<PerformanceConstructParamTest>().As<IPerformanceConstructParam2Test>();
            //builder.Bind<IPerformanceConstructParam3Test>().To<PerformanceConstructParamTest>();
            builder.RegisterType<PerformanceConstructParamTest>().As<IPerformanceConstructParam3Test>();
            //builder.Bind<IPerformanceConstructTest>().To<PerformanceConstructTest>();
            builder.RegisterType<PerformanceConstructTest>().As<IPerformanceConstructTest>();
            //builder.Bind<IPerformanceSingletonTest>().To<PerformanceSingletonTest>().InSingletonScope();
            builder.RegisterType<PerformanceSingletonTest>().As<IPerformanceSingletonTest>().SingleInstance();
            //builder.Bind<IPerformanceInstanceTest>().To(new PerformanceInstanceTest());
            builder.RegisterInstance(new PerformanceInstanceTest()).As<IPerformanceInstanceTest>().SingleInstance();

            return builder.Build();
        }

        public IIocContainer Container { get; }
        public IContainer DirectContainer { get; }
        public IContainer ContainerComparison { get; }
        public bool RunComparisonTests => true;

        #endregion Constructors

        #region Methods

        public void Dispose()
        {
            Ioc.Instance.Dispose();
        }

        #endregion Methods
    }
}
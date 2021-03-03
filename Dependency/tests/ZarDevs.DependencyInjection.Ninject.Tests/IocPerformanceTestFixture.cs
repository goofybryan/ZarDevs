using Ninject;
using ZarDevs.DependencyInjection.Tests;

namespace ZarDevs.DependencyInjection.Ninject.Tests
{
    public sealed class IocPerformanceTestFixture : IIocPerformanceTestFixture
    {
        #region Constructors

        public IocPerformanceTestFixture()
        {
            var kernel = IocNinject.CreateBuilder();

            Container = Ioc.Initialize(kernel, builder =>
            {
                builder.ConfigurePerformanceTest();
            });

            ContainerDirect = ((IIocContainer<IKernel>)Container).Kernel;
            ContainerComparison = CreateComparison();
        }

        #endregion Constructors

        #region Properties

        public IIocContainer Container { get; }

        public IKernel ContainerDirect { get; }

        public IKernel ContainerComparison { get; }

        public bool RunComparisonTests => true;

        #endregion Properties

        #region Methods

        public void Dispose()
        {
            Ioc.Instance.Dispose();
        }

        private IKernel CreateComparison()
        {
            var kernel = new StandardKernel();

            //builder.Bind<IPerformanceMethodTest>().To<PerformanceMethodTest>();
            kernel.Bind<IPerformanceMethodTest>().To<PerformanceMethodTest>();
            //builder.Bind<IPerformanceMethodResultTest>().To((ctx) => PerformanceMethodTest.Method());
            kernel.Bind<IPerformanceMethodResultTest>().ToMethod((ctx) => PerformanceMethodTest.Method());
            //builder.Bind<IPerformanceConstructParam1Test>().To<PerformanceConstructParamTest>();
            kernel.Bind<IPerformanceConstructParam1Test>().To<PerformanceConstructParamTest>();
            //builder.Bind<IPerformanceConstructParam2Test>().To<PerformanceConstructParamTest>();
            kernel.Bind<IPerformanceConstructParam2Test>().To<PerformanceConstructParamTest>();
            //builder.Bind<IPerformanceConstructParam3Test>().To<PerformanceConstructParamTest>();
            kernel.Bind<IPerformanceConstructParam3Test>().To<PerformanceConstructParamTest>();
            //builder.Bind<IPerformanceConstructTest>().To<PerformanceConstructTest>();
            kernel.Bind<IPerformanceConstructTest>().To<PerformanceConstructTest>();
            //builder.Bind<IPerformanceSingletonTest>().To<PerformanceSingletonTest>().InSingletonScope();
            kernel.Bind<IPerformanceSingletonTest>().To<PerformanceSingletonTest>().InSingletonScope();
            //builder.Bind<IPerformanceInstanceTest>().To(new PerformanceInstanceTest());
            kernel.Bind<IPerformanceInstanceTest>().ToConstant(new PerformanceInstanceTest());

            return kernel;
        }

        #endregion Methods
    }
}
using ZarDevs.DependencyInjection.ZarIoc;

namespace ZarDevs.DependencyInjection.Tests
{
    public static class BindingExtensions
    {
        [DependencyRegistration]
        public static void ConfigurePerformanceTest(this IDependencyBuilder builder)
        {
            builder.Bind<PerformanceMethodTest>().Resolve<IPerformanceMethodTest>();
            builder.BindFunction((ctx) => PerformanceMethodTest.Method()).Resolve<IPerformanceMethodResultTest>();
            builder.Bind<PerformanceConstructParamTest>().Resolve<IPerformanceConstructParam1Test>();
            builder.Bind<PerformanceConstructParamTest>().Resolve<IPerformanceConstructParam2Test>();
            builder.Bind<PerformanceConstructParamTest>().Resolve<IPerformanceConstructParam3Test>();
            builder.Bind<PerformanceConstructTest>().Resolve<IPerformanceConstructTest>();
            builder.Bind<PerformanceSingletonTest>().Resolve<IPerformanceSingletonTest>().InSingletonScope();
            builder.BindInstance(new PerformanceInstanceTest()).Resolve<IPerformanceInstanceTest>();
        }

        public static void ConfigureTest(this IDependencyBuilder builder) => new Bindings().Register(builder);
    }
}
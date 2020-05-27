namespace ZarDevs.DependencyInjection.Tests
{
    public static class Bindings
    {
        public static void ConfigureTest(this IDependencyBuilder builder)
        {
            builder.Bind<INormalClass>().To<NormalClass>().InTransientScope();
            builder.Bind<ICallingClass>().To<CallingClass>();
            builder.Bind<IChildClass>().To((ctx, name) => FactoryClass.Instance.CreateChildClass(ctx.RequestType));
            builder.Bind<IRequestClass>().To<RequestClass>().InRequestScope();
            builder.Bind<ISingletonClass>().To<SingletonClass>().InSingletonScope();
        }
    }
}
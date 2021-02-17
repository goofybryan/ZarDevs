namespace ZarDevs.DependencyInjection.Tests
{
    public static class Bindings
    {
        public static void ConfigureTest(this IDependencyBuilder builder)
        {
            builder.Bind<INormalClass>().To<NormalClass>().InTransientScope();
            builder.Bind<ICallingClass>().To<CallingClass>();
            builder.Bind<IChildClass>().To<ChildClass>();
            builder.Bind<IRequestClass>().To<RequestClass>().InRequestScope();
            builder.Bind<ISingletonClass>().To<SingletonClass>().InSingletonScope();
        }
    }
}

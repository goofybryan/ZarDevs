namespace ZarDevs.DependencyInjection.Tests
{
    public static class Bindings
    {
        public const string MethodWithArgs = nameof(MethodWithArgs);
        public const string MethodWithNoArgs = nameof(MethodWithNoArgs);
        public const string NotMethod = nameof(NotMethod);

        public static void ConfigureTest(this IDependencyBuilder builder)
        {
            builder.Bind<INormalClass>().To<NormalClass>().InTransientScope();
            builder.Bind<ICallingClass>().To<CallingClass>();
            builder.Bind<IChildClass>().To<ChildClass>();
            builder.Bind<IRequestClass>().To<RequestClass>().InRequestScope();
            builder.Bind<ISingletonClass>().To<SingletonClass>().InSingletonScope();
            builder.Bind<IFactoryClass>().To<FactoryClass>().InSingletonScope();
            builder.Bind<IMultipleConstructorClass>().To<MultipleConstructorClass>().WithKey(NotMethod);
            builder.Bind<IMultipleConstructorClass>().To((ctx, key) => ctx.Ioc.Resolve<IFactoryClass>().ResolveMultipleConstructorClass(ctx.GetArguments())).WithKey(MethodWithArgs);
            builder.Bind<IMultipleConstructorClass>().To((ctx, key) => ctx.Ioc.Resolve<IFactoryClass>().ResolveMultipleConstructorClass()).WithKey(MethodWithNoArgs);
        }
    }
}

namespace ZarDevs.DependencyInjection.Tests
{
    public static class Bindings
    {
        #region Fields

        public const string MethodWithArgs = nameof(MethodWithArgs);
        public const string MethodWithNoArgs = nameof(MethodWithNoArgs);
        public const string NotMethod = nameof(NotMethod);
        public const string NotResolvedName = nameof(NotResolvedName);

        #endregion Fields

        #region Methods

        public static void ConfigureTest(this IDependencyBuilder builder)
        {
            builder.Bind<INormalClass>().To<NormalClass>().InTransientScope();
            builder.Bind<ICallingClass>().To<CallingClass>();
            builder.Bind<IChildClass>().To<ChildClass>();
            builder.Bind<IRequestClass>().To<RequestClass>().InRequestScope();
            builder.Bind<ISingletonClass>().To<SingletonClass>().InSingletonScope();
            builder.Bind<ISingletonNamedClass>().To<SingletonClass>().InSingletonScope().WithKey(nameof(ISingletonNamedClass));
            builder.Bind<ISingletonEnumClass>().To<SingletonClass>().InSingletonScope().WithKey(EnumAsKey.Key);
            builder.Bind<ISingletonKeyClass>().To<SingletonClass>().InSingletonScope().WithKey(typeof(ISingletonKeyClass));
            builder.Bind<IMultipleConstructorClass>().To<MultipleConstructorClass>();
            builder.Bind<IFactoryClass>().To<FactoryClass>().InSingletonScope();
            builder.Bind<IFactoryResolutionClass>().To((ctx, key) => ctx.Ioc.Resolve<IFactoryClass>().ResolveFactoryResolutionClass(ctx.GetArguments())).WithKey(MethodWithArgs);
            builder.Bind<IFactoryResolutionClass>().To((ctx, key) => ctx.Ioc.Resolve<IFactoryClass>().ResolveFactoryResolutionClass()).WithKey(MethodWithNoArgs);
            builder.Bind<IFactoryResolutionClass>().To<FactoryResolutionClass>().WithKey(NotMethod);
            builder.Bind<INotBindedKeyed>().To<NotBindedClass>().WithKey(NotResolvedName);
            builder.Bind<INotBindedKeyed>().To<NotBindedClass>().WithKey(EnumAsKey.Key);
            builder.Bind<INotBindedKeyed>().To<NotBindedClass>().WithKey(typeof(INormalClass));
        }

        #endregion Methods

        #region Enums

        public enum EnumAsKey { Key, DifferentKey }

        #endregion Enums
    }
}
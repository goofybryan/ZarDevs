namespace ZarDevs.DependencyInjection.Tests
{
    public static class Bindings
    {
        #region Fields

        public const string MethodWithArgs = nameof(MethodWithArgs);
        public const string MethodWithNoArgs = nameof(MethodWithNoArgs);
        public const string NotMethod = nameof(NotMethod);
        public const string NotResolvedName = nameof(NotResolvedName);
        public const string Named1 = nameof(Named1);
        public const string Named2 = nameof(Named2);

        #endregion Fields

        #region Methods

        public static void ConfigurePerformanceTest(this IDependencyBuilder builder)
        {
            builder.Bind<IPerformanceMethodTest>().To<PerformanceMethodTest>();
            builder.Bind<IPerformanceMethodResultTest>().To((ctx) => PerformanceMethodTest.Method());
            builder.Bind<IPerformanceConstructParam1Test>().To<PerformanceConstructParamTest>();
            builder.Bind<IPerformanceConstructParam2Test>().To<PerformanceConstructParamTest>();
            builder.Bind<IPerformanceConstructParam3Test>().To<PerformanceConstructParamTest>();
            builder.Bind<IPerformanceConstructTest>().To<PerformanceConstructTest>();
            builder.Bind<IPerformanceSingletonTest>().To<PerformanceSingletonTest>().InSingletonScope();
            builder.Bind<IPerformanceInstanceTest>().To(new PerformanceInstanceTest());
        }

        public static void ConfigureTest(this IDependencyBuilder builder)
        {
            builder.Bind<INormalClass>().To<NormalClass>().InTransientScope();
            builder.Bind<ICallingClass>().To<CallingClass>();
            builder.Bind<IChildClass>().To<ChildClass>();
            builder.Bind<ISingletonClass>().To<SingletonClassTest>().InSingletonScope();
            builder.Bind<ISingletonNamedClass>().To<SingletonClassTest>().InSingletonScope().WithKey(nameof(ISingletonNamedClass));
            builder.Bind<ISingletonEnumClass>().To<SingletonClassTest>().InSingletonScope().WithKey(EnumAsKey.Key);
            builder.Bind<ISingletonKeyClass>().To<SingletonClassTest>().InSingletonScope().WithKey(typeof(ISingletonKeyClass));
            builder.Bind<IMultipleConstructorClass>().To<MultipleConstructorClass>();
            builder.Bind<IFactoryClass>().To<FactoryClass>().InSingletonScope();
            builder.Bind<IFactoryResolutionClass>().To((ctx) => ctx.Ioc.Resolve<IFactoryClass>().ResolveFactoryResolutionClass(ctx.GetArguments())).WithKey(MethodWithArgs);
            builder.Bind<IFactoryResolutionClass>().To((ctx) => ctx.Ioc.Resolve<IFactoryClass>().ResolveFactoryResolutionClass()).WithKey(MethodWithNoArgs);
            builder.Bind<IFactoryResolutionClass>().To<FactoryResolutionClass>().WithKey(NotMethod);
            builder.Bind<INotBindedKeyed>().To<NotBindedClass>().WithKey(NotResolvedName);
            builder.Bind<INotBindedKeyed>().To<NotBindedClass>().WithKey(EnumAsKey.Key);
            builder.Bind<INotBindedKeyed>().To<NotBindedClass>().WithKey(typeof(INormalClass));
            builder.Bind(typeof(IGenericTypeTests<>)).To(typeof(GenericTypeTest<>));
            builder.Bind(typeof(IGenericSingletonTests<>)).To(typeof(GenericTypeTest<>)).InSingletonScope();
            builder.Bind<IMultipleBindingClassTest>().To<MultipleBindingClassTest1>().WithKey(nameof(MultipleBindingClassTest1));
            builder.Bind<IMultipleBindingClassTest>().To<MultipleBindingClassTest2>().WithKey(nameof(MultipleBindingClassTest2));
            builder.Bind<IMultipleBindingClassTest>().To<MultipleBindingClassTest3>().WithKey(nameof(MultipleBindingClassTest3));
            builder.Bind<IMultipleBindingConstructorClassTest>().To<MultipleBindingConstructorClassTest>();
            builder.Bind(typeof(IMultipleBindingConstructorClassTest<>)).To(typeof(MultipleBindingConstructorClassTest<>));
            builder.Bind(typeof(IMultipleBindingClassTest<>)).To(typeof(MultipleBindingClassTest1<>)).WithKey(typeof(MultipleBindingClassTest1<>).Name);
            builder.Bind(typeof(IMultipleBindingClassTest<>)).To(typeof(MultipleBindingClassTest2<>)).WithKey(typeof(MultipleBindingClassTest2<>).Name);
            builder.Bind(typeof(IMultipleBindingClassTest<>)).To(typeof(MultipleBindingClassTest3<>)).WithKey(typeof(MultipleBindingClassTest3<>).Name);
            builder.Bind<IFactoryMethodClass>().To<FactoryMethodClass>().InSingletonScope();
            builder.Bind<IFactoryMethodResolutionClass>().ToFactory<IFactoryMethodClass>(nameof(IFactoryMethodClass.Method));
            builder.Bind<IFactoryMethodResolutionNamedClass>().ToFactory<IFactoryMethodClass>(nameof(IFactoryMethodClass.Method)).WithKey(Named1);
            builder.Bind<IFactoryMethodResolutionNamedClass>().ToFactory<IFactoryMethodClass>(nameof(IFactoryMethodClass.Method)).WithKey(Named2);
            builder.Bind<IFactoryMethodResolutionSingletonClass>().ToFactory<IFactoryMethodClass>(nameof(IFactoryMethodClass.Singleton)).InSingletonScope();
            builder.Bind(typeof(IFactoryMethodClass<>)).To(typeof(FactoryMethodClass<>)).InSingletonScope();
            builder.Bind(typeof(IFactoryMethodResolutionClass<>)).ToFactory(typeof(IFactoryMethodClass<>), "Method");
            builder.Bind(typeof(IFactoryMethodResolutionSingletonClass<>)).ToFactory(typeof(IFactoryMethodClass<>), "Singleton").InSingletonScope();
        }

        #endregion Methods

        #region Enums

        public enum EnumAsKey { Key, DifferentKey }

        #endregion Enums
    }
}
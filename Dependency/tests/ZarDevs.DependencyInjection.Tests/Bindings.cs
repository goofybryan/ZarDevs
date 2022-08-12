namespace ZarDevs.DependencyInjection.Tests
{
    public class Bindings : IDependencyRegistration
    {
        #region Fields

        public const string MethodWithArgs = nameof(MethodWithArgs);
        public const string MethodWithNoArgs = nameof(MethodWithNoArgs);
        public const string Named1 = nameof(Named1);
        public const string Named2 = nameof(Named2);
        public const string NotMethod = nameof(NotMethod);
        public const string NotResolvedName = nameof(NotResolvedName);

        #endregion Fields

        #region Methods

        [ZarDevs.DependencyInjection.DependencyRegistration]
        public void Register(IDependencyBuilder builder)
        {
            builder.Bind<NormalClass>().Resolve<INormalClass>().InTransientScope();
            builder.Bind<CallingClass>().Resolve<ICallingClass>();
            builder.Bind<ChildClass>().Resolve<IChildClass>();
            builder.Bind<SingletonClassTest>().Resolve<ISingletonClass>().InSingletonScope();
            builder.Bind<SingletonClassTest>().Resolve<ISingletonNamedClass>().InSingletonScope().WithKey(nameof(ISingletonNamedClass));
            builder.Bind<SingletonClassTest>().Resolve<ISingletonEnumClass>().InSingletonScope().WithKey(EnumAsKey.Key);
            builder.BindInstance(new SingletonClassTest()).Resolve<ISingletonEnumClass>().WithKey(EnumAsKey.Instance);
            builder.Bind<SingletonClassTest>().Resolve<ISingletonKeyClass>().InSingletonScope().WithKey(typeof(ISingletonKeyClass));
            builder.Bind<SingletonSameInstanceClassTest>().Resolve(typeof(ISingletonSameInstanceClassTest), typeof(ISingletonSameInstanceClassTest2), typeof(ISingletonSameInstanceClassTest3), typeof(ISingletonSameInstanceClassTest4)).InSingletonScope();
            builder.Bind<MultipleConstructorClass>().Resolve<IMultipleConstructorClass>();
            builder.Bind<FactoryClass>().Resolve<IFactoryClass>().InSingletonScope();
            builder.BindFunction((ctx) => ctx.Ioc.Resolve<IFactoryClass>().ResolveFactoryResolutionClass(ctx.GetArguments())).Resolve<IFactoryResolutionClass>().WithKey(MethodWithArgs);
            builder.BindFunction((ctx) => ctx.Ioc.Resolve<IFactoryClass>().ResolveFactoryResolutionClass()).Resolve<IFactoryResolutionClass>().WithKey(MethodWithNoArgs);
            builder.Bind<FactoryResolutionClass>().Resolve<IFactoryResolutionClass>().WithKey(NotMethod);
            builder.Bind<NotBindedClass>().Resolve<INotBindedKeyed>().WithKey(NotResolvedName);
            builder.Bind<NotBindedClass>().Resolve<INotBindedKeyed>().WithKey(EnumAsKey.Key);
            builder.Bind<NotBindedClass>().Resolve<INotBindedKeyed>().WithKey(typeof(INormalClass));
            builder.Bind(typeof(GenericTypeTest<>)).Resolve(typeof(IGenericTypeTests<>));
            builder.Bind(typeof(GenericTypeTest<>)).Resolve(typeof(IGenericSingletonTests<>)).InSingletonScope();
            builder.Bind<MultipleBindingClassTest1>().Resolve<IMultipleBindingClassTest>().WithKey(nameof(MultipleBindingClassTest1));
            builder.Bind<MultipleBindingClassTest2>().Resolve<IMultipleBindingClassTest>().WithKey(nameof(MultipleBindingClassTest2));
            builder.Bind<MultipleBindingClassTest3>().Resolve<IMultipleBindingClassTest>().WithKey(nameof(MultipleBindingClassTest3));
            builder.Bind<MultipleBindingConstructorClassTest>().Resolve<IMultipleBindingConstructorClassTest>();
            builder.Bind(typeof(MultipleBindingConstructorClassTest<>)).Resolve(typeof(IMultipleBindingConstructorClassTest<>));
            builder.Bind(typeof(MultipleBindingClassTest1<>)).Resolve(typeof(IMultipleBindingClassTest<>)).WithKey(typeof(MultipleBindingClassTest1<>).Name);
            builder.Bind(typeof(MultipleBindingClassTest2<>)).Resolve(typeof(IMultipleBindingClassTest<>)).WithKey(typeof(MultipleBindingClassTest2<>).Name);
            builder.Bind(typeof(MultipleBindingClassTest3<>)).Resolve(typeof(IMultipleBindingClassTest<>)).WithKey(typeof(MultipleBindingClassTest3<>).Name);
            builder.Bind<FactoryMethodClass>().Resolve<IFactoryMethodClass>().InSingletonScope();
            builder.BindFactory<IFactoryMethodClass>(nameof(IFactoryMethodClass.Method)).Resolve<IFactoryMethodResolutionClass>();
            builder.BindFactory<IFactoryMethodClass>(nameof(IFactoryMethodClass.Method)).Resolve<IFactoryMethodResolutionNamedClass>().WithKey(Named1);
            builder.BindFactory<IFactoryMethodClass>(nameof(IFactoryMethodClass.Method)).Resolve<IFactoryMethodResolutionNamedClass>().WithKey(Named2);
            builder.BindFactory<IFactoryMethodClass>(nameof(IFactoryMethodClass.Singleton)).Resolve<IFactoryMethodResolutionSingletonClass>().InSingletonScope();
            builder.Bind(typeof(FactoryMethodClass<>)).Resolve(typeof(IFactoryMethodClass<>)).InSingletonScope();
            builder.BindFactory(typeof(IFactoryMethodClass<>), "Method").Resolve(typeof(IFactoryMethodResolutionClass<>));
            builder.BindFactory(typeof(IFactoryMethodClass<>), "Singleton").Resolve(typeof(IFactoryMethodResolutionSingletonClass<>)).InSingletonScope();
            builder.Bind<ResolveAllTest>().ResolveAll().WithKey(nameof(ResolveAllTest));
            builder.Bind<ResolveAllTest2>().ResolveAll().WithKey(nameof(ResolveAllTest2));
        }

        #endregion Methods

        #region Enums

        public enum EnumAsKey
        { 
            Key, 
            DifferentKey,
            Instance
        }

        #endregion Enums
    }
}
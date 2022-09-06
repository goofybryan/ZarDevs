# ZarDevs Dependency Injection

## Introduction

This project is designed to be a standardized way to define IOC. This is to overcome the need to decide upfront what you IOC technology would be and build your libraries with IOC in mind. This project requires an implementing IOC but you can get examples or already built ones from the other projects.

## How To

The use is a simple builder pattern that consists of a request type and a resolve type. You can also bind a request type to a method or already created instance. You can optionally bind your objects with a scope, transient(default) or singleton. You can also choose to add a specific key to your bindings.

### Normal Binding

Normal binding consists of twp parts that, always the request type and then the resolve to. In the example below the interface INormalClass will be bound to the implementing class NormalClass. Ioc will resolve any constructor parameters as long as they have also been bound. This is ofcourse all dependent on the underlying technology abilities.

    ```c#
    // Setup
    public class NormalClass : INormalClass
    {
        public NormalClass(ITaskHandler handler, IEnumerable<ITasks> tasks)
        {
            Handler = handler ?? new ArgumentNullException(nameof(handler));
            Tasks = tasks ?? new ArgumentNullException(nameof(tasks));
        }

        public ITaskHandler Handler { get; }
        public IEnumerable<ITasks> Tasks { get; }
    }

    // Build
    builder.Bind<NormalClass>().Resolve<INormalClass>();
    builder.Bind<TaskHandler>().Resolve<ITaskHandler>();
    builder.Bind<Task1>().Resolve<ITask>();
    builder.Bind<Task2>().Resolve<ITask>();
    builder.Bind<Task3>().Resolve<ITask>();

    ...

    // Use
    var resolved = Ioc.Resolve<INormalClass>();

    Assert.NotNull(resolved);
    Assert.NotNull(resolved.Handler);
    Assert.NotNull(resolved.Tasks);
    ```

### Scoped Binding

This project currently supports only two of the three scopes that normally exist. Transient and Singleton variables. Scoped variables are normally variables that exist alongside a specified scope like the [HttpRequest](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.http.httprequest?view=aspnetcore-5.0) or thread scope. I purposely decided not to cater for these at the moment.

    ```c#
    // Build
    builder.Bind<NormalClass>().Resolve<INormalClass>();
    builder.Bind<SingletonClass>().Resolve<ISingletonClass>().InSingletonScope();

    ...

    // Use
    var resolvedClass1 = Ioc.Resolve<INormalClass>();
    var resolvedClass2 = Ioc.Resolve<INormalClass>();

    Assert.NotSame(resolvedClass1, resolvedClass2);

    var resolvedSingleton1 = Ioc.Resolve<ISingletonClass>();
    var resolvedSingleton2 = Ioc.Resolve<ISingletonClass>();

    Assert.Same(resolvedSingleton1, resolvedSingleton2);
    ```

### Binding Parameter Arguments

There may be a need to pass parameters to your bindings and not have IOC resolve these for you. You can eitehr pass in named arguments or arguments in the order of the constructor parameters.

_Just be warned that this requires knowledge of the constructor arguments and changes to the constructor could have consequences at runtime. Each underlying technology act different. If you follow the general rule, what I pass in is everything that is expected, then you should be fine. Naturally use with caution._

    ```c#
    // Build
    builder.Bind<NormalClassWithConstructorArgs>().Resolve<INormalClassWithConstructorArgs>();

    ...

    // Use
    var resolved = Ioc.Resolve<INormalClassWithConstructorArgs>(paramVar1, paramVar2);
    // or
    var resolved = Ioc.Resolve<INormalClassWithConstructorArgs>(("argumentName1", paramVar1), ("argumentName2", paramVar2));
    ```

### BindFunction Binding

You can also bind to a specified method of a class. This useful if you have factories and you want to use them as part of the IOC. With the method you are passed a [Context](./DependencyContext.cs) object. This context has a list of arguments that was passed in and a reference to the [IIocContainer](./IIocContainer.cs).

    ```c#
    // Build
    builder.BindFunction((ctx) => StaticMethodClass.CreateFactoryResolved()).Resolve<IMethodReturnResolved>();
    // or
    builder.BindFunction((ctx) => StaticMethodClass.CreateFactoryResolved(ctx.GetArguments())).Resolve<IMethodReturnResolved>();
    // or
    builder.BindFunction((ctx) => ctx.Ioc.Resolve<IResolvedMethodClass>().CreateMethodReturnResolved()).Resolve<IMethodReturnResolved>();

    ...

    // Use
    var factoryResolved = Ioc.Resolve<IFactoryResolved>();
    ```

### Factory Binding

What I introduced was something that does method injection implicitly. The context was to declare a type to be resolved by a method of another type, the factory. The infrastucture would resolve the parameters and invoke the method. Naturally same rules apply as regular binding.

    ```C#
    // Build
    Builder.Bind<Factory>().Resolve<IFactory>().InSingletonScoped(); // Optional scope but makes sense
    Builder.BindFactory(nameof(IFactory.CreateFactoryClass)).Resolve<IResolvedFactoryClass>();

    ...

    // Use
    IResolvedFactoryClass factoryClass = Ioc.Resolve<IResolvedFactoryClass>();
    ```

### Keyed Bindings

You can add bindings based on a key. This is useful if you want add multiple bindings but only retrieve the specific binding. There are three options as a key, string, enum and object.

    ```c#
    // Build
    builder.Bind<Task1>().Resolve<ITask>().Named("Task1");
    builder.Bind<Task2>().Resolve<ITask>().Named("Task2");
    builder.Bind<Task3>().Resolve<ITask>().Named("Task3");
    // or Enum
    builder.Bind<Task1>().Resolve<ITask>().Named(Task.Task1);
    builder.Bind<Task2>().Resolve<ITask>().Named(Task.Task2);
    builder.Bind<Task3>().Resolve<ITask>().Named(Task.Task3);
    // or object
    builder.Bind<Task1>().Resolve<ITask>().Named(typeof(ITask1));
    builder.Bind<Task1>().Resolve<ITask>().Named(typeof(ITask2));
    builder.Bind<Task1>().Resolve<ITask>().Named(typeof(ITask3));

    ...

    // Use
    ITask task1 = Ioc.Resolve<ITask>("Task1");
    ITask task2 = Ioc.Resolve<ITask>("Task2");
    ITask task3 = Ioc.Resolve<ITask>("Task3");
    // or Enum
    ITask task1 = Ioc.Resolve<ITask>(Task.Task1);
    ITask task1 = Ioc.Resolve<ITask>(Task.Task2);
    ITask task1 = Ioc.Resolve<ITask>(Task.Task3);
    ```

You can look at the [tests](../../tests/ZarDevs.DependencyInjection.Tests/IocTestsConstruct.cs) to see a complete list of tests and how the [bindings](../../tests/ZarDevs.DependencyInjection.Tests/bindings.cs) was setup. More does need to be added in the future.

## Links

1. [Home](../../../README.md)
1. [Dependency Injection](../../README.md)
    1. [Dependency Injection](../ZarDevs.DependencyInjection/README.md)
    1. [AutoFac Dependency Injection](../ZarDevs.DependencyInjection.AutoFac/README.md)
    1. [Microsoft Dependency Injection](../ZarDevs.DependencyInjection.Microsoft/README.md)
    1. [Ninject Dependency Injection](../ZarDevs.DependencyInjection.Ninject/README.md)
    1. [Runtime Factory Dependency Injection](../ZarDevs.DependencyInjection.RuntimeFactory/README.md)
    1. [Dependency Injection Extensions](../ZarDevs.DependencyInjection.Extensions/README.md)
    1. [Dependency Injection Factory Extensions](../ZarDevs.DependencyInjection.Extensions.Factory/README.md)
    1. [ZarIoc Source Generation](../ZarDevs.DependencyInjection.ZarIoc/README.md)
    1. [ZarIoc Dependency Injection](../ZarDevs.DependencyInjection.ZarIoc.Abstractions/README.md)

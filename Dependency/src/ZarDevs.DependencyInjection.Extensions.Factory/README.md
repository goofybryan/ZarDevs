# ZarDevs Dependency Injection Factory Extensions

## Introduction

The is a standalone extension that add's factory method binding implementation. It uses the underlying IOC to resolve all the objects but adds the runtime method bindind and invoking.

## How to

What I introduced was something that does method injection implicitly. The context was to declare a type to be resolved by a method of another type, the factory. The infrastucture would resolve the parameters and invoke the method. Naturally same rules apply as regular binding. This factory also makes use of of both reflection and [expressions](https://docs.microsoft.com/en-us/dotnet/api/system.linq.expressions.expression?view=net-5.0).

    ```C#
    // Build
    Builder.Bind<IFactory>().To<Factory>().InSingletonScoped(); // Optional scope but makes sense
    Builder.Bind<IResolvedFactoryClass>().ToFactory(nameof(IFactory.CreateFactoryClass));

    ...

    // Use
    IResolvedFactoryClass factoryClass = Ioc.Resolve<IResolvedFactoryClass>();
    ```

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

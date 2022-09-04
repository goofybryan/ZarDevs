# ZarDevs Runtime Implementation

## Introduction

This project contains the Runtime implementation of the [Dependency Injection](..\ZarDevs.DependencyInjection\README.md). The intended use it that you would only add this to your start project. This will then translate the bindings made in the other projects into Runtime native bindings.

This project is my own brainchild. I wanted to see if I could create a simplistic IOC that could be just as effective as any other. This obviously does not have the advanced features of more mature IOC technologies. This is fun and I have learnt that there is something called [expressions](https://docs.microsoft.com/en-us/dotnet/api/system.linq.expressions.expression?view=net-5.0), that makes runtime manipulation ultrafast. This is a toggle, so the old mechanism using reflection is still available and any code where there is unknowns will not make use of expressions to run. _Currently the project is RuntimeFactory folder but I will rename it in the future_

This project supports singleton and transient bindings.

## How To

The implementation can be seen in the [Runtime test implementation](..\..\tests\ZarDevs.DependencyInjection.RuntimeFactory.Tests) and [General Test Construct](..\..\tests\ZarDevs.DependencyInjection.Tests) projects. In the example below, I have bindings made in other assemblies and only in the starting assembly do I initialize the Runtime assembly.

    ```c#
    // Initialization Code
    Container = Ioc.Initialize(new IocKernelBuilder(), builder =>
    {
        builder.ConfigureDependencyFromAssemblyA();
        builder.ConfigureDependencyFromAssemblyB();
    });

    // Where the configure is an extention method
    public static void ConfigureDependencyFromAssemblyA(this IDependencyBuilder builder)
    {
        builder.Bind<INormalClass>().To<NormalClass>().InTransientScope();
        ...
    }
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
# ZarDevs Microsoft Implementation

## Introduction

This project contains the Microsoft implementation of the [Dependency Injection](..\ZarDevs.DependencyInjection\README.md). The intended use it that you would only add this to your start project. This will then translate the bindings made in the other projects into Microsoft native bindings.

Now Microsoft's implementation have left out a lot of the features that more mature IOC's allow. For good reasons, it forces you to code in a very specific way, however not always practical. Their solution is really quick but does not support given parameters. So I have wrapped those with implementations that make use of the service provider and Microsofts ActivatorUtilities.

Therefor I implemented quite a few wrappers and is the only implementation where you cannot use the the underlying technology for everything. Especially if you are relying on some the advanced features that more mature IOC implementations provide. For example, you cannot use the `IServiceProvider` for resolving items where you pass in arguments or my factory method implementation. You must use the `IIocContainer`.

This project supports singleton and transient bindings.

## How To

The implementation can be seen in the [Microsoft test implementation](..\..\tests\ZarDevs.DependencyInjection.Microsoft.Tests) and [General Test Construct](..\..\tests\ZarDevs.DependencyInjection.Tests) projects. In the example below, I have bindings made in other assemblies and only in the starting assembly do I initialize the Microsoft assembly.

    ```c#
    // Initialization Code
    var services = new ServiceCollection();

    var kernel = IocServiceProvider.CreateBuilder(services);

    Container = Ioc.Initialize(kernel,
        builder => 
        {            
            builder.ConfigureDependencyFromAssemblyA();
            builder.ConfigureDependencyFromAssemblyB();
        },
        () => kernel.ConfigureServiceProvider(services.BuildServiceProvider())
    );

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

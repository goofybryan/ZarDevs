# ZarDevs AutoFac Implementation

## Introduction

This project contains the AutoFac implementation of the [Dependency Injection](..\ZarDevs.DependencyInjection\README.md). The intended use it that you would only add this to your start project. This will then translate the bindings made in the other projects into AutoFac native bindings.

Keep in mind that to make the generic functionality work, there are additional objects created to translate this accordingly.

## How To

The implementation can be seen in the [AutoFac test implementation](..\..\tests\ZarDevs.DependencyInjection.AutoFac.Tests) and [General Test Construct](..\..\tests\ZarDevs.DependencyInjection.Tests) projects. In the example below, I have bindings made in other assemblies and only in the starting assembly do I initialize the AutoFac assembly.

    ```c#
    // Initialization Code
    var kernel = IocAutoFac.Initialize();

    Container = Ioc.Initialize(kernel, builder =>
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
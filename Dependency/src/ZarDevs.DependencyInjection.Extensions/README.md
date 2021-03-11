# ZarDevs Dependency Injection Extensions

## Introduction

The extensions project is meant to extend the generic bindings by adding additional objects that can manage the dependencies. This works well for IOC's that do not cater for all requirements, like [Microsoft's IServiceCollection](https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.dependencyinjection.iservicecollection?view=dotnet-plat-ext-5.0) or my own [Runtime Dependency Injection](../ZarDevs.DependencyInjection.RuntimeFactory/README.md).

It contains the framework to handle dependency injection, it just requires the actual object creation. An example my implementations, [Runtime Dependency Injection](../ZarDevs.DependencyInjection.RuntimeFactory/) and [Microsoft Dependency Injection](../ZarDevs.DependencyInjection.Microsoft/).

## Links

1. [Home](../../../README.md)
1. [Dependency Injection](../../README.md)
    1. [Dependency Injection](../ZarDevs.DependencyInjection/README.md)
    1. [AutoFac Dependency Injection](../ZarDevs.DependencyInjection.AutoFac/README.md)
    1. [Microsoft Dependency Injection](../ZarDevs.DependencyInjection.Microsoft/README.md)
    1. [Ninject Dependency Injection](../ZarDevs.DependencyInjection.Ninject/README.md)
    1. [Runtime Dependency Injection](../ZarDevs.DependencyInjection.RuntimeFactory/README.md)
    1. [Dependency Injection Extensions](../ZarDevs.DependencyInjection.Extensions/README.md)
    1. [Dependency Injection Factory Extensions](../ZarDevs.DependencyInjection.Extensions.Factory/README.md)

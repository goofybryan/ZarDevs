
using Autofac;
using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.DependencyInjection;
using ZarDevs.DependencyInjection;
using ZarDevs.DependencyInjection.Tests;
using AutofacFixture = ZarDevs.DependencyInjection.AutoFac.Tests.IocPerformanceTestFixture;
using MicrosoftFixture = ZarDevs.DependencyInjection.Microsoft.Tests.IocPerformanceTestFixture;
using NinjectFixture = ZarDevs.DependencyInjection.Ninject.Tests.IocPerformanceTestFixture;
using RuntimeFixture = ZarDevs.DependencyInjection.RuntimeFactory.Tests.IocPerformanceTestFixture;
using SourceGenFixture = ZarDevs.DependencyInjection.SourceGenerator.Tests.IocPerformanceTestFixture;

namespace ZarDevs.Dependency.Benchmark;

[SimpleJob(iterationCount:1)]
public class PerformanceBenchmark
{
    private AutofacFixture _autofacFixture;
    private MicrosoftFixture _microsoftFixture;
    private NinjectFixture _ninjectFixture;
    private RuntimeFixture _runtimeFixture;
    private SourceGenFixture _sourceGenFixture;

    [GlobalSetup(Targets = [nameof(AutofacIoc), nameof(AutofacDirect)])]
    public void SetupAutofac()
    {
        Ioc.Instance?.Dispose();
        _autofacFixture = new AutofacFixture();
    }

    [GlobalSetup(Targets = [nameof(MicrosoftIoc), nameof(MicrosoftDirect)])]
    public void SetupMicrosoft()
    {
        Ioc.Instance?.Dispose();
        _microsoftFixture = new MicrosoftFixture();
    }

    [GlobalSetup(Targets = [nameof(NinjectIoc), nameof(NinjectDirect)])]
    public void SetupNinject()
    {
        Ioc.Instance?.Dispose();
        _ninjectFixture = new NinjectFixture();
    }

    [GlobalSetup(Targets = [nameof(RuntimeIoc), nameof(RuntimeDirect)])]
    public void SetupRuntime()
    {
        Ioc.Instance?.Dispose();
        _runtimeFixture = new RuntimeFixture();
    }

    [GlobalSetup(Targets = [nameof(SourceGenIoc),nameof(SourceGenDirect)])]
    public void SetupSourceGen()
    {
        Ioc.Instance?.Dispose();
        _sourceGenFixture = new SourceGenFixture();
    }

    [Benchmark]
    public IPerformanceConstructTest AutofacIoc()
    {
        return _autofacFixture.Container.Resolve<IPerformanceConstructTest>();
    }

    [Benchmark]
    public IPerformanceConstructTest AutofacDirect()
    {
        return _autofacFixture.DirectContainer.Resolve<IPerformanceConstructTest>();
    }

    [Benchmark]
    public IPerformanceConstructTest MicrosoftIoc()
    {
        return _microsoftFixture.Container.Resolve<IPerformanceConstructTest>();
    }

    [Benchmark]
    public IPerformanceConstructTest MicrosoftDirect()
    {
        return _microsoftFixture.ContainerDirect.GetRequiredService<IPerformanceConstructTest>();
    }

    [Benchmark]
    public IPerformanceConstructTest NinjectIoc()
    {
        return _ninjectFixture.Container.Resolve<IPerformanceConstructTest>();
    }

    [Benchmark]
    public IPerformanceConstructTest NinjectDirect()
    {
        return _ninjectFixture.ContainerDirect.GetRequiredService<IPerformanceConstructTest>();
    }

    [Benchmark]
    public IPerformanceConstructTest RuntimeIoc()
    {
        return _runtimeFixture.Container.Resolve<IPerformanceConstructTest>();
    }

    [Benchmark]
    public IPerformanceConstructTest RuntimeDirect()
    {
        return (IPerformanceConstructTest)_runtimeFixture.InstanceResolution.GetResolution(typeof(IPerformanceConstructTest)).Resolve();
    }

    [Benchmark]
    public IPerformanceConstructTest SourceGenIoc()
    {
        return _sourceGenFixture.Container.Resolve<IPerformanceConstructTest>();
    }

    [Benchmark]
    public IPerformanceConstructTest SourceGenDirect()
    {
        return (IPerformanceConstructTest)_sourceGenFixture.TypeFactoryContainer.Get(typeof(IPerformanceConstructTest)).Resolve();
    }
}
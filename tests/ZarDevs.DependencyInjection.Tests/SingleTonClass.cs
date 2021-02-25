namespace ZarDevs.DependencyInjection.Tests
{
    public interface ISingletonClass { }

    public interface ISingletonNamedClass { }

    public interface ISingletonKeyClass { }

    public interface ISingletonEnumClass { }

    internal class SingletonClass : ISingletonClass, ISingletonEnumClass, ISingletonKeyClass, ISingletonNamedClass
    { }
}

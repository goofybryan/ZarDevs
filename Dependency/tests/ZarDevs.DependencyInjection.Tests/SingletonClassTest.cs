namespace ZarDevs.DependencyInjection.Tests
{
    public interface ISingletonClass { }

    public interface ISingletonEnumClass { }

    public interface ISingletonKeyClass { }

    public interface ISingletonNamedClass { }

    internal class SingletonClassTest : ISingletonClass, ISingletonEnumClass, ISingletonKeyClass, ISingletonNamedClass
    { }
}
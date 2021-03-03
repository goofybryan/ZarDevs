namespace ZarDevs.DependencyInjection.Tests
{
    public interface ISingletonClass { }

    public interface ISingletonNamedClass { }

    public interface ISingletonKeyClass { }

    public interface ISingletonEnumClass { }

    internal class SingletonClassTest : ISingletonClass, ISingletonEnumClass, ISingletonKeyClass, ISingletonNamedClass
    { }
}

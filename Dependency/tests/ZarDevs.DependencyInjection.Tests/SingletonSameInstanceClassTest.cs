namespace ZarDevs.DependencyInjection.Tests
{
    public interface ISingletonSameInstanceClassTest { }

    public interface ISingletonSameInstanceClassTest2 { }

    public interface ISingletonSameInstanceClassTest3 { }

    public interface ISingletonSameInstanceClassTest4 { }

    internal class SingletonSameInstanceClassTest : ISingletonSameInstanceClassTest, ISingletonSameInstanceClassTest2, ISingletonSameInstanceClassTest3, ISingletonSameInstanceClassTest4
    { }
}
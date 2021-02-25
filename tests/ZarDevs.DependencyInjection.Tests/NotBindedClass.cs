namespace ZarDevs.DependencyInjection.Tests
{
    public interface INotBindedClass { }

    public interface INotBindedKeyed { }

    public class NotBindedClass : INotBindedClass, INotBindedKeyed
    {
    }
}

using ZarDevs.DependencyInjection;

namespace ZarDevs.Commands.Http
{
    public static class HttpConfigure
    {
        public static IDependencyBuilder ConfigureHttp(this IDependencyBuilder builder)
        {
            builder.Bind<IApiHttpFactory>().To((ctx, name) => ApiHttpFactory.Instance).InSingletonScope();

            return builder;
        }
    }
}
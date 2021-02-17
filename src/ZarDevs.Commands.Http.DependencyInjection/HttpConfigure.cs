using System;
using ZarDevs.DependencyInjection;

namespace ZarDevs.Commands.Http
{
    public static class HttpConfigure
    {
        #region Methods

        /// <summary>
        /// Configure the API HTTP factory for the solution.
        /// </summary>
        /// <param name="builder">The dependency builder.</param>
        /// <param name="useIocHttpHandlerFactory">Specify if to use the <see cref="DependencyApiHttpFactoryHandler"/>. If false, the default factory will be used, see <see cref="DefaultHttpHandlerFactory"/>. This factory uses runtime activation to create the handlers, <seealso cref="Runtime.Create"/>.</param>
        /// <returns></returns>
        public static IDependencyBuilder ConfigureHttp(this IDependencyBuilder builder, bool useIocHttpHandlerFactory)
        {
            if (useIocHttpHandlerFactory) builder.ConfigureHttp(new DependencyApiHttpFactoryHandler());
            else builder.ConfigureHttp(new DefaultHttpHandlerFactory());
            return builder;
        }

        /// <summary>
        /// Configure the API HTTP factory for the solution.
        /// </summary>
        /// <param name="builder">The dependency builder.</param>
        /// <returns></returns>
        public static IDependencyBuilder ConfigureHttp(this IDependencyBuilder builder, IApiHttpHandlerFactory handlerFactory)
        {
            if (handlerFactory is null)
            {
                throw new ArgumentNullException(nameof(handlerFactory));
            }

            ApiHttpFactory.Instance = new ApiHttpFactory(new System.Net.Http.HttpClient(), handlerFactory);

            builder.Bind<IApiHttpFactory>().To((ctx, key) => ApiHttpFactory.Instance).InSingletonScope();
            builder.Bind<IApiHttpClient>().To((ctx, key) => ctx.Ioc.Resolve<IApiHttpFactory>().NewClient(key)).InTransientScope();

            return builder;
        }

        /// <summary>
        /// This is used to create HttpRequest handler bindings that can be used to alter the Http request.
        /// </summary>
        /// <typeparam name="THandler">The request handler that must inherit <see cref="IApiHttpRequestHandler"/></typeparam>
        /// <param name="name">The name of the handler, by default it's string.empty.</param>
        /// <returns></returns>
        public static IApiHttpRequestHandlerBinding AddHttpRequestHandler<THandler>(string name = "") where THandler : IApiHttpRequestHandler
        {
            if (ApiHttpFactory.Instance == null) throw new InvalidOperationException("Please configure the HttpFactory using ConfigureHttp method.");

            return ApiHttpFactory.Instance.AddRequestHandler<THandler>(name);
        }

        #endregion Methods
    }
}
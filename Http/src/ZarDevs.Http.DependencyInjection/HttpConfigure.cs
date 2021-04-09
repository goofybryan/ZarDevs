using System;
using ZarDevs.DependencyInjection;
using ZarDevs.Runtime;

namespace ZarDevs.Http.Client
{
    /// <summary>
    /// Configure the Http bindings.
    /// </summary>
    public static class HttpConfigure
    {
        #region Methods

        /// <summary>
        /// This is used to create HttpRequest handler bindings that can be used to alter the Http request.
        /// </summary>
        /// <typeparam name="THandler">The request handler that must inherit <see cref="IApiHttpRequestHandler"/></typeparam>
        /// <param name="name">The name of the handler, by default it's null.</param>
        /// <returns></returns>
        public static IApiHttpRequestHandlerBinding AddHttpRequestHandler<THandler>(object name = null) where THandler : class, IApiHttpRequestHandler
        {
            if (ApiHttpFactory.Instance == null) throw new InvalidOperationException("Please configure the HttpFactory using ConfigureHttp method.");

            return ApiHttpFactory.Instance.AddRequestHandler<THandler>(name);
        }

        /// <summary>
        /// Configure the API HTTP factory for the solution.
        /// </summary>
        /// <param name="builder">The dependency builder.</param>
        /// <param name="useIocHttpHandlerFactory">Specify if to use the <see cref="DependencyApiHttpFactoryHandler"/>. If false, the default factory will be used, see <see cref="DefaultHttpHandlerFactory"/>. 
        /// This factory uses runtime activation to create the handlers, <seealso cref="Runtime.Create"/>.
        /// </param>
        /// <returns></returns>
        public static IDependencyBuilder ConfigureHttp(this IDependencyBuilder builder, bool useIocHttpHandlerFactory)
        {
            IApiHttpHandlerFactory handlerFactory = useIocHttpHandlerFactory ? new DependencyApiHttpFactoryHandler() : new DefaultHttpHandlerFactory(Create.Instance);

            return builder.ConfigureHttp(handlerFactory);
        }

        /// <summary>
        /// Configure the API HTTP factory for the solution.
        /// </summary>
        /// <param name="builder">The dependency builder.</param>
        /// <param name="handlerFactory">Specify the handler factory.</param>
        /// <returns></returns>
        public static IDependencyBuilder ConfigureHttp(this IDependencyBuilder builder, IApiHttpHandlerFactory handlerFactory)
        {
            if (handlerFactory is null)
            {
                throw new ArgumentNullException(nameof(handlerFactory));
            }

            ApiHttpFactory.Instance = new ApiHttpFactory(new System.Net.Http.HttpClient(), handlerFactory);

            builder.Bind<IApiHttpFactory>().To(ApiHttpFactory.Instance);
            builder.Bind<IApiHttpClient>().To((ctx) => ApiHttpFactory.Instance.NewClient(ctx.Info.Key)).InTransientScope();

            return builder;
        }

        #endregion Methods
    }
}
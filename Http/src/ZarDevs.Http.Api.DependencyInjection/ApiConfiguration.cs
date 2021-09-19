using ZarDevs.DependencyInjection;

namespace ZarDevs.Http.Api
{
    /// <summary>
    /// Configure the Api
    /// </summary>
    public static class ApiConfiguration
    {
        #region Methods

        /// <summary>
        /// Configure the <see cref="IHttpResponseFactory"/> and <see cref="IApiCommandFactory"/> and bind the <see cref="IApiCommandAsync"/> for all commands under <see cref="HttpRequestType"/> 
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IDependencyBuilder ConfigureApi(this IDependencyBuilder builder)
        {
            builder.Bind<HttpResponseFactory>().Resolve<IHttpResponseFactory>().InSingletonScope();
            builder.Bind<ApiCommandFactory>().Resolve<IApiCommandFactory>().InSingletonScope();
            builder.Bind(typeof(ApiCommandContentTypeMap<>)).Resolve(typeof(IApiCommandContentTypeMap<>));

            builder.BindFactory<IApiCommandFactory>(nameof(IApiCommandFactory.CreateGetCommand)).Resolve<IApiCommandAsync>().WithKey(HttpRequestType.Get);
            builder.BindFactory<IApiCommandFactory>(nameof(IApiCommandFactory.CreateDeleteCommand)).Resolve<IApiCommandAsync>().WithKey(HttpRequestType.Delete);
            builder.BindFactory<IApiCommandFactory>(nameof(IApiCommandFactory.CreatePutCommand)).Resolve<IApiCommandAsync>().WithKey(HttpRequestType.Put);
            builder.BindFactory<IApiCommandFactory>(nameof(IApiCommandFactory.CreatePostCommand)).Resolve<IApiCommandAsync>().WithKey(HttpRequestType.Post);
            builder.BindFactory<IApiCommandFactory>(nameof(IApiCommandFactory.CreatePatchCommand)).Resolve<IApiCommandAsync>().WithKey(HttpRequestType.Patch);
            builder.BindFactory<IApiCommandFactory>(nameof(IApiCommandFactory.CreateSendCommand)).Resolve<IApiCommandAsync>().WithKey(HttpRequestType.Custom);

            return builder;
        }

        #endregion Methods
    }
}
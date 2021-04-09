﻿using ZarDevs.DependencyInjection;

namespace ZarDevs.Http.Api
{
    /// <summary>
    /// Configure the Api
    /// </summary>
    public static class ApiConfiguration
    {
        #region Methods

        public static IDependencyBuilder ConfigureApi(this IDependencyBuilder builder)
        {
            builder.Bind<IHttpResponseFactory>().To<HttpResponseFactory>().InSingletonScope();
            builder.Bind<IApiCommandFactory>().To<ApiCommandFactory>().InSingletonScope();

            builder.Bind<IApiCommandAsync>().ToFactory<IApiCommandFactory>(nameof(IApiCommandFactory.CreateGetCommand)).WithKey(HttpRequestType.Get);
            builder.Bind<IApiCommandAsync>().ToFactory<IApiCommandFactory>(nameof(IApiCommandFactory.CreateDeleteCommand)).WithKey(HttpRequestType.Delete);
            builder.Bind<IApiCommandAsync>().ToFactory<IApiCommandFactory>(nameof(IApiCommandFactory.CreatePutCommand)).WithKey(HttpRequestType.Put);
            builder.Bind<IApiCommandAsync>().ToFactory<IApiCommandFactory>(nameof(IApiCommandFactory.CreatePostCommand)).WithKey(HttpRequestType.Post);
            builder.Bind<IApiCommandAsync>().ToFactory<IApiCommandFactory>(nameof(IApiCommandFactory.CreatePatchCommand)).WithKey(HttpRequestType.Patch);
            builder.Bind<IApiCommandAsync>().ToFactory<IApiCommandFactory>(nameof(IApiCommandFactory.CreateSendCommand)).WithKey(HttpRequestType.Custom);

            return builder;
        }

        #endregion Methods
    }
}
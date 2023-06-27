using System.Net.Http;

namespace ZarDevs.Http.Api
{
    /// <summary>
    /// Api command extensions class to get the most used command content types
    /// </summary>
    public static class ApiCommandFactoryExtensions
    {

        /// <summary>
        /// Create a new instance of the <see cref="IApiCommandAsync"/> that is for patching json content.
        /// </summary>
        /// <param name="apiCommandFactory">Api command factory.</param>
        /// <param name="apiClientKey">An option api client key that will be used to resolve the ApiHttpClient. Default is no key (null)</param>
        public static IApiCommandAsync CreateJsonPatchCommand(this IApiCommandFactory apiCommandFactory, object apiClientKey = null)
        {
            return apiCommandFactory.CreatePatchCommand(HttpContentType.Json[0], apiClientKey);
        }

        /// <summary>
        /// Create a new instance of the <see cref="IApiCommandAsync"/> that is for posting json content.
        /// </summary>
        /// <param name="apiCommandFactory">Api command factory.</param>
        /// <param name="apiClientKey">An option api client key that will be used to resolve the ApiHttpClient. Default is no key (null)</param>
        public static IApiCommandAsync CreateJsonPostCommand(this IApiCommandFactory apiCommandFactory, object apiClientKey = null)
        {
            return apiCommandFactory.CreatePostCommand(HttpContentType.Json[0], apiClientKey);
        }

        /// <summary>
        /// Create a new instance of the <see cref="IApiCommandAsync"/> that is for putting json content.
        /// </summary>
        /// <param name="apiCommandFactory">Api command factory.</param>
        /// <param name="apiClientKey">An option api client key that will be used to resolve the ApiHttpClient. Default is no key (null)</param>
        public static IApiCommandAsync CreateJsonPutCommand(this IApiCommandFactory apiCommandFactory, object apiClientKey = null)
        {
            return apiCommandFactory.CreatePutCommand(HttpContentType.Json[0], apiClientKey);
        }

        /// <summary>
        /// Create a new instance of the <see cref="IApiCommandAsync"/> for  json content. This command is meant to be used as a custom command when the normal pattern does not fit.
        /// </summary>
        /// <param name="apiCommandFactory">Api command factory.</param>
        /// <param name="sendMethod">Specify the <see cref="HttpMethod"/> that the command with use.</param>
        /// <param name="apiClientKey">An option api client key that will be used to resolve the ApiHttpClient. Default is no key (null)</param>
        public static IApiCommandAsync CreateJsonSendCommand(this IApiCommandFactory apiCommandFactory, HttpMethod sendMethod, object apiClientKey = null)
        {
            return apiCommandFactory.CreateSendCommand(HttpContentType.Json[0], sendMethod, apiClientKey);
        }

    }
}

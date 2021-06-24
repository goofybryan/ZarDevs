using System;
using System.Net.Http;

namespace ZarDevs.Http.Api
{
    /// <summary>
    /// Api command factory interface defining the methods required to create commands.
    /// </summary>
    public interface IApiCommandFactory
    {
        /// <summary>
        /// Create a new instance of the <see cref="IApiCommandAsync"/> that is for deleting.
        /// </summary>
        /// <param name="apiClientKey">An option api client key that will be used to resolve the ApiHttpClient. Default is no key (null)</param>
        IApiCommandAsync CreateDeleteCommand(object apiClientKey = null);

        /// <summary>
        /// Create a new instance of the <see cref="IApiCommandAsync"/> that is for getting.
        /// </summary>
        /// <param name="apiClientKey">An option api client key that will be used to resolve the ApiHttpClient. Default is no key (null)</param>
        IApiCommandAsync CreateGetCommand(object apiClientKey = null);

        /// <summary>
        /// Create a new instance of the <see cref="IApiCommandAsync"/> that is for patching.
        /// </summary>
        /// <param name="mediaType">Specify the media type that will be used to send any content. This will be used to serialize any content to the api call.</param>
        /// <param name="apiClientKey">An option api client key that will be used to resolve the ApiHttpClient. Default is no key (null)</param>
        IApiCommandAsync CreatePatchCommand(string mediaType, object apiClientKey = null);

        /// <summary>
        /// Create a new instance of the <see cref="IApiCommandAsync"/> that is for posting.
        /// </summary>
        /// <param name="mediaType">Specify the media type that will be used to send any content. This will be used to serialize any content to the api call.</param>
        /// <param name="apiClientKey">An option api client key that will be used to resolve the ApiHttpClient. Default is no key (null)</param>
        IApiCommandAsync CreatePostCommand(string mediaType, object apiClientKey = null);

        /// <summary>
        /// Create a new instance of the <see cref="IApiCommandAsync"/> that is for putting.
        /// </summary>
        /// <param name="mediaType">Specify the media type that will be used to send any content. This will be used to serialize any content to the api call.</param>
        /// <param name="apiClientKey">An option api client key that will be used to resolve the ApiHttpClient. Default is no key (null)</param>
        IApiCommandAsync CreatePutCommand(string mediaType, object apiClientKey = null);

        /// <summary>
        /// Create a new instance of the <see cref="IApiCommandRequest"/> that is used during the request.
        /// </summary>
        /// <param name="apiUrl">Specify the api url</param>
        /// <param name="content">Specify any content that is needed to be sent.</param>
        IApiCommandRequest CreateRequest(Uri apiUrl, object content = null);

        /// <summary>
        /// Create a new instance of the <see cref="IApiCommandAsync"/>. This command is meant to be used as a custom command when the normal pattern does not fit.
        /// </summary>
        /// <param name="mediaType">Specify the media type that will be used to send any content. This will be used to serialize any content to the api call.</param>
        /// <param name="sendMethod">Specify the <see cref="HttpMethod"/> that the command with use.</param>
        /// <param name="apiClientKey">An option api client key that will be used to resolve the ApiHttpClient. Default is no key (null)</param>
        IApiCommandAsync CreateSendCommand(string mediaType, HttpMethod sendMethod, object apiClientKey = null);
    }
}
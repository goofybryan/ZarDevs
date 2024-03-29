﻿using ZarDevs.Http.Client;

namespace ZarDevs.Http.Security
{
    /// <summary>
    /// Http factory interface that will be used to create the core security handlers.
    /// </summary>
    public interface IHttpSecurityFactory
    {
        #region Methods

        /// <summary>
        /// Create the http security request handler.
        /// </summary>
        IApiHttpRequestHandler CreateAuthHeaderHandler();

        /// <summary>
        /// Create the token refresh handler.
        /// </summary>
        /// <returns></returns>
        IApiHttpRequestHandler CreateTokenRefreshHandler();

        /// <summary>
        /// Create the token refresh handler.
        /// </summary>
        /// <param name="userLoginCommand">Specify the user login command.</param>
        /// <returns></returns>
        IApiHttpRequestHandler CreateTokenRenewHandler(ILoginUserPasswordAsync userLoginCommand);

        #endregion Methods
    }
}
using System;
using System.Collections.Generic;
using System.Text;
using ZarDevs.Http.Client;

namespace ZarDevs.Http.Security.Jwt
{
    /// <summary>
    /// Configure the security the API
    /// </summary>
    public static class ConfigureSecurityApi
    {
        /// <summary>
        /// Configure the JWT pipeline, first the refresh token handler -> login handler -> auth handler. Note, these handlers will be ignored if Auth header has already been filled in.
        /// 1. The token handler will try and refresh the token if the access token is expired
        /// 2. The login handler will call the <see cref="ILoginUserPasswordAsync"/> to request the the username and password and will request the server for a new access token.
        /// 3. Will add the access token to the appropriate header value.
        /// </summary>
        /// <param name="factory"></param>
        public static void ConfigureUserNamePasswordJwt(this IApiHttpFactory factory)
        {
            factory.AddRequestHandler<HttpSecurityTokenRefreshHandler>(SecurityTokenConstants.ApiClientName)
            .SetNextHandler<HttpSecurityTokenLoginHandler>()
            .SetNextHandler<HttpSecurityAuthHeaderHandler>();
        }
    }
}

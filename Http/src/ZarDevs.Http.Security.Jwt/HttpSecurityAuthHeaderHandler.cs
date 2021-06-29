using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ZarDevs.Http.Security
{
    internal class HttpSecurityAuthHeaderHandler : HttpSecurityRequestHandler, IHttpSecurityRequestHandler
    {
        #region Constructors

        public HttpSecurityAuthHeaderHandler(ISecurityConfiguration configuration, ITokenValidator validator) : base(configuration, validator)
        {
        }

        #endregion Constructors

        #region Methods

        protected override Task OnHandleRequestAsync(HttpRequestMessage request, LoginToken token)
        {
            if (token == null)
                throw new InvalidOperationException("The login token cannot be found in the configuration, please ensure that you have configured the handler pipeline correctly.");

            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(token.TokenType, token.AccessToken);

            return Task.CompletedTask;
        }

        #endregion Methods
    }
}
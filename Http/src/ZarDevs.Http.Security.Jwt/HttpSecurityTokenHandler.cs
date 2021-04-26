using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ZarDevs.Http.Api;

namespace ZarDevs.Http.Security
{
    internal abstract class HttpSecurityTokenHandler : HttpSecurityRequestHandler
    {
        #region Fields

        private readonly IApiCommandAsync _apiCommand;

        #endregion Fields

        #region Constructors

        public HttpSecurityTokenHandler(ISecurityConfiguration configuration, IApiCommandAsync apiCommand, ITokenValidator validator) : base(configuration, validator)
        {
            _apiCommand = apiCommand ?? throw new ArgumentNullException(nameof(apiCommand));
        }

        #endregion Constructors

        #region Methods

        protected abstract Task<IDictionary<string, string>> GetRequestParameters(LoginToken token);

        protected override async Task OnHandleRequestAsync(HttpRequestMessage message, LoginToken token)
        {
            if (!_configuration.TryGet(SecurityTokenConstants.ApiConfiguration, out ApiConfiguration configuration))
                throw new InvalidOperationException("There is no api configuration configured. Please ensure that it has been added to the security configuration.");

            var parameters = await GetRequestParameters(token);
            var request = new ApiCommandRequest(configuration.TokenEndpoint, parameters);

            var response = await _apiCommand.ExecuteAsync(request);
            var newToken = await response.TryGetContent<LoginToken>();

            _configuration.Update(SecurityTokenConstants.LoginToken, newToken);

            message.Headers.Authorization = new AuthenticationHeaderValue(SecurityTokenConstants.JwtAuthHeader, newToken.AccessToken);
        }

        #endregion Methods
    }
}
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZarDevs.Http.Api;

namespace ZarDevs.Http.Security
{
    internal class HttpSecurityTokenRefreshHandler : HttpSecurityTokenHandler
    {
        #region Fields

        private readonly ITokenRefreshRequestBuilder _requestBuilder;

        #endregion Fields

        #region Constructors

        public HttpSecurityTokenRefreshHandler(ISecurityConfiguration configuration, IApiCommandAsync apiCommand, ITokenRefreshRequestBuilder requestBuilder, ITokenValidator validator)
            : base(configuration, apiCommand, validator)
        {
            _requestBuilder = requestBuilder ?? throw new ArgumentNullException(nameof(requestBuilder));
        }

        #endregion Constructors

        #region Methods

        protected override Task<IDictionary<string, string>> GetRequestParameters(LoginToken token)
        {
            IDictionary<string, string> parameters = _requestBuilder
                .SetRefreshToken(token.RefreshToken)
                .SetClientId(token.ClientId)
                .SetClientSecret(token.ClientSecret)
                .Build();

            return Task.FromResult(parameters);
        }

        protected override bool ValidateToken(LoginToken token) => base.ValidateToken(token) && !string.IsNullOrWhiteSpace(token?.RefreshToken);

        #endregion Methods
    }
}
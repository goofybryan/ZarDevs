using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZarDevs.Http.Api;

namespace ZarDevs.Http.Security
{
    internal class HttpSecurityTokenLoginHandler : HttpSecurityTokenHandler
    {
        #region Fields

        private readonly ITokenUsernameRequestBuilder _requestBuilder;
        private readonly ILoginUserPasswordAsync _userLoginCommand;

        #endregion Fields

        #region Constructors

        public HttpSecurityTokenLoginHandler(ISecurityConfiguration configuration, IApiCommandAsync apiCommand, ILoginUserPasswordAsync userLoginCommand, ITokenUsernameRequestBuilder requestBuilder, ITokenValidator validator)
            : base(configuration, apiCommand, validator)
        {
            _userLoginCommand = userLoginCommand ?? throw new ArgumentNullException(nameof(userLoginCommand));
            _requestBuilder = requestBuilder ?? throw new ArgumentNullException(nameof(requestBuilder));
        }

        #endregion Constructors

        #region Methods

        protected override async Task<IDictionary<string, string>> GetRequestParameters(LoginToken token)
        {
            var details = await _userLoginCommand.RetrieveAsync();

            var builder = _requestBuilder
                .SetCredendials(details.Username, details.Password);

            if (details.Additional.TryGetValue(SecurityTokenConstants.Scopes, out object scopesObject) && scopesObject is IEnumerable<string> scopes)
            {
                builder.AddScopes(scopes);
            }

            builder.SetClientId(token.ClientId)
                .SetClientSecret(token.ClientSecret);

            return builder.Build();
        }

        #endregion Methods
    }
}
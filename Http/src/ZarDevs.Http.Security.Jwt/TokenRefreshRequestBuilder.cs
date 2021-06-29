using System.Collections.Generic;

namespace ZarDevs.Http.Security
{
    internal class TokenRefreshRequestBuilder : TokenRequestParameterBuilder, ITokenRefreshRequestBuilder
    {
        #region Constructors

        public TokenRefreshRequestBuilder() : base("refresh_token")
        {
        }

        #endregion Constructors

        #region Properties

        public string RefreshToken { get; set; }

        #endregion Properties

        #region Methods

        public ITokenRefreshRequestBuilder SetRefreshToken(string refreshToken)
        {
            RefreshToken = refreshToken;
            return this;
        }

        protected override void OnBuild(IDictionary<string, string> parameters)
        {
            Add(parameters, TokenRequestParameters.RefreshToken, RefreshToken);
        }

        #endregion Methods
    }
}
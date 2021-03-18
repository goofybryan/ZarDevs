using System.Collections.Generic;

namespace ZarDevs.Http.Security
{
    internal class RefreshTokenRequestBuilder : TokenRequestParameterBuilder, IRefreshTokenRequestBuilder
    {
        #region Constructors

        public RefreshTokenRequestBuilder(string grantType) : base(grantType)
        {
        }

        #endregion Constructors

        #region Properties

        public string RefreshToken { get; set; }

        #endregion Properties

        #region Methods

        public IRefreshTokenRequestBuilder SetRefreshToken(string refreshToken)
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
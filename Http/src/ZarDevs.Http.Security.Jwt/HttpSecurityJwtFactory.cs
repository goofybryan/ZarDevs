using ZarDevs.Http.Api;
using ZarDevs.Http.Client;

namespace ZarDevs.Http.Security
{
    internal class HttpSecurityJwtFactory : IHttpSecurityFactory
    {
        #region Fields

        private readonly IApiCommandFactory _commandFactory;
        private readonly ISecurityConfiguration _configuration;

        #endregion Fields

        #region Constructors

        public HttpSecurityJwtFactory(IApiCommandFactory commandFactory, ISecurityConfiguration configuration)
        {
            _commandFactory = commandFactory ?? throw new System.ArgumentNullException(nameof(commandFactory));
            _configuration = configuration ?? throw new System.ArgumentNullException(nameof(configuration));
        }

        #endregion Constructors

        #region Methods

        public IApiHttpRequestHandler CreateAuthHeaderHandler()
        {
            return new HttpSecurityAuthHeaderHandler(_configuration, new TokenValidator());
        }

        public IApiHttpRequestHandler CreateTokenRefreshHandler()
        {
            var apiCommand = _commandFactory.CreatePostCommand(HttpContentType.FormUrlEncoded[0], SecurityTokenConstants.ApiClientName);
            return new HttpSecurityTokenRefreshHandler(_configuration, apiCommand, new TokenRefreshRequestBuilder(), new TokenValidator());
        }

        public IApiHttpRequestHandler CreateTokenRenewHandler(ILoginUserPasswordAsync userLoginCommand)
        {
            var apiCommand = _commandFactory.CreatePostCommand(HttpContentType.FormUrlEncoded[0], SecurityTokenConstants.ApiClientName);
            return new HttpSecurityTokenLoginHandler(_configuration, apiCommand, userLoginCommand, new TokenUsernameRequestBuilder(), new TokenValidator());
        }

        #endregion Methods
    }
}
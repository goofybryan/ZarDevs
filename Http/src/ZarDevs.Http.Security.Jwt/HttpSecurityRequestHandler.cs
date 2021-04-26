using System;
using System.Net.Http;
using System.Threading.Tasks;
using ZarDevs.Http.Client;

namespace ZarDevs.Http.Security
{
    internal abstract class HttpSecurityRequestHandler : ApiHttpRequestHandler
    {
        #region Fields

        protected readonly ISecurityConfiguration _configuration;
        private readonly ITokenValidator _validator;

        #endregion Fields

        #region Constructors

        public HttpSecurityRequestHandler(ISecurityConfiguration configuration, ITokenValidator validator)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        #endregion Constructors

        #region Methods

        protected override Task OnHandleRequestAsync(HttpRequestMessage request)
        {
            if (request.Headers.Authorization != null)
                return Task.CompletedTask;

            _configuration.TryGet(SecurityTokenConstants.LoginToken, out LoginToken token);

            return ValidateToken(token) ? Task.CompletedTask : OnHandleRequestAsync(request, token);
        }

        protected abstract Task OnHandleRequestAsync(HttpRequestMessage message, LoginToken token);

        protected virtual bool ValidateToken(LoginToken token) => token != null && !_validator.HasExpired(token.AccessToken);

        #endregion Methods
    }
}
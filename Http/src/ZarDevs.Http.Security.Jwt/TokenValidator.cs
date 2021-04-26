using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;

namespace ZarDevs.Http.Security
{
    public interface ITokenValidator
    {
        bool HasExpired(string accessToken);
        SecurityToken ReadJwtToken(string accessToken);
    }

    internal class TokenValidator : ITokenValidator
    {
        #region Methods

        public bool HasExpired(string accessToken)
        {
            var token = ReadJwtToken(accessToken);
            return token.ValidTo <= DateTime.UtcNow;
        }

        public SecurityToken ReadJwtToken(string accessToken)
        {
            var handler = new JwtSecurityTokenHandler();

            return handler.CanReadToken(accessToken) ? handler.ReadJwtToken(accessToken) : null;
        }

        #endregion Methods
    }
}

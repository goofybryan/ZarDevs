using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;

namespace ZarDevs.Http.Security
{
    /// <summary>
    /// Token validator interface used to read a JWT access token <see cref="string"/> and validate if it is valid or return the actual <see cref="SecurityToken"/>
    /// </summary>
    public interface ITokenValidator
    {
        /// <summary>
        /// Load the current access token and return a <see cref="SecurityToken"/>. If <paramref name="accessToken"/> is not a valid token <see cref="string"/> then a <c>null</c> value will be returned.
        /// </summary>
        /// <param name="accessToken">A valid access token string.</param>
        SecurityToken ReadJwtToken(string accessToken);
    }

    internal class TokenValidator : ITokenValidator
    {
        #region Methods

        public SecurityToken ReadJwtToken(string accessToken)
        {
            var handler = new JwtSecurityTokenHandler();

            return handler.CanReadToken(accessToken) ? handler.ReadJwtToken(accessToken) : null;
        }

        #endregion Methods
    }
}

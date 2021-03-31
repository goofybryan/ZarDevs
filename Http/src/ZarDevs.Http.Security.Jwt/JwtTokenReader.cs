using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace ZarDevs.Http.Security
{
    /// <summary>
    /// Token reader that reads a string token and converts it to a <typeparamref name="TToken"/>.
    /// </summary>
    /// <typeparam name="TToken">The parsed token type <typeparamref name="TToken"/>.</typeparam>
    public interface ITokenReader<out TToken>
    {
        /// <summary>
        /// The the token from the specified string
        /// </summary>
        /// <param name="token">The token in string format</param>
        /// <returns>The converted token.</returns>
        TToken ReadToken(string token);
    }

    internal class JwtTokenReader : ITokenReader<SecurityToken>
    {
        #region Methods

        public SecurityToken ReadToken(string accessToken)
        {
            var handler = new JwtSecurityTokenHandler();
            JwtSecurityToken token = null;
            if (handler.CanReadToken(accessToken))
            {
                token = handler.ReadJwtToken(accessToken);
            }

            return token;
        }

        #endregion Methods
    }
}

using Microsoft.IdentityModel.Tokens;
using System;

namespace ZarDevs.Http.Security
{
    /// <summary>
    /// <see cref="SecurityToken"/> extention class.
    /// </summary>
    public static class TokenValidationExtentions
    {
        #region Methods

        /// <summary>
        /// Check if the <see cref="SecurityToken"/> has expired.
        /// </summary>
        /// <param name="token">The token to validate, can be null.</param>
        /// <returns>A true if the token has expired.</returns>
        public static bool HasExpired(this SecurityToken token)
        {
            return token != null && token.ValidTo <= DateTime.UtcNow;
        }

        #endregion Methods
    }
}
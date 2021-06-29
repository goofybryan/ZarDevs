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
        /// Validate if the <paramref name="token"/> has expired. This will check if the is still valid at the current date and time i.e. <see cref="SecurityToken.ValidTo"/> is less or equal to <see cref="DateTime.UtcNow"/>
        /// <returns>A true if the token has expired.</returns>
        public static bool HasExpired(this SecurityToken token)
        {
            return token != null && token.ValidTo <= DateTime.UtcNow;
        }

        #endregion Methods
    }
}
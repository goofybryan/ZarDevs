using System;

namespace ZarDevs.Http.Security
{
    /// <summary>
    /// Login token model
    /// </summary>
    public class LoginToken
    {
        #region Properties

        /// <summary>
        /// Get or set the token type
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// Get or set the refresh token
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Get or set the client secret
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// Get or set the access token
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// Get or set the token type
        /// </summary>
        public string TokenType { get; set; }

        /// <summary>
        /// Get or set the token valid to
        /// </summary>
        public DateTime ValidTo { get; set; }

        #endregion Properties
    }
}
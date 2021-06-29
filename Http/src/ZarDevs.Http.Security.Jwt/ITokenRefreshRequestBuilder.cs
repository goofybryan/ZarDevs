namespace ZarDevs.Http.Security
{
    /// <summary>
    /// Refresh toke request builder.
    /// </summary>
    public interface ITokenRefreshRequestBuilder : ITokenRequestBuilder
    {
        #region Properties

        /// <summary>
        /// The refesh token
        /// </summary>
        string RefreshToken { get; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Set the <see cref="RefreshToken"/>
        /// </summary>
        /// <param name="refreshToken">The refresh token to set.</param>
        /// <returns>The current instance.</returns>
        ITokenRefreshRequestBuilder SetRefreshToken(string refreshToken);

        #endregion Methods
    }
}
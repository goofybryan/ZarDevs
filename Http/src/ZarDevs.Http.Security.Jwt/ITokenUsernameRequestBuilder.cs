using System.Collections.Generic;

namespace ZarDevs.Http.Security
{
    /// <summary>
    /// Access token request builder for username and password requests.
    /// </summary>
    public interface ITokenUsernameRequestBuilder : ITokenRequestBuilder
    {
        #region Properties

        /// <summary>
        /// Get the password.
        /// </summary>
        string Password { get; set; }

        /// <summary>
        /// Get the list of scopes
        /// </summary>
        ICollection<string> Scope { get; }

        /// <summary>
        /// Get the username
        /// </summary>
        string Username { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Add scopes to the request
        /// </summary>
        /// <param name="scopes">A list of scopes to add.</param>
        /// <returns>The current instance</returns>
        ITokenUsernameRequestBuilder AddScopes(params string[] scopes);

        /// <summary>
        /// Add scopes to the request
        /// </summary>
        /// <param name="scopes">A list of scopes to add.</param>
        /// <returns>The current instance</returns>
        ITokenUsernameRequestBuilder AddScopes(IEnumerable<string> scopes);

        /// <summary>
        /// Add the <see cref="Username"/> and <see cref="Password"/> for the request
        /// </summary>
        /// <param name="username">The username</param>
        /// <param name="password">The password</param>
        /// <returns>The current instance</returns>
        ITokenUsernameRequestBuilder SetCredendials(string username, string password);

        #endregion Methods
    }
}
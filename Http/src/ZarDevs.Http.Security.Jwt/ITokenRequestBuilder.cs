using System.Collections.Generic;

namespace ZarDevs.Http.Security
{
    /// <summary>
    /// Token request builder
    /// </summary>
    public interface ITokenRequestBuilder
    {
        #region Properties

        /// <summary>
        /// Get the ClientId
        /// </summary>
        string ClientId { get; }

        /// <summary>
        /// Get the ClientSecret
        /// </summary>
        string ClientSecret { get; }

        /// <summary>
        /// Get the Grant Type
        /// </summary>
        string GrantType { get; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Build the list of parameters
        /// </summary>
        /// <returns>A parameter map.</returns>
        Dictionary<string, string> Build();

        /// <summary>
        /// Add the <see cref="ClientId"/>
        /// </summary>
        /// <param name="clientId">The client id</param>
        /// <returns>The current instance</returns>
        ITokenRequestBuilder SetClientId(string clientId);

        /// <summary>
        /// Add the <see cref="ClientSecret"/>
        /// </summary>
        /// <param name="clientSecret">The client secret</param>
        /// <returns>The current instance</returns>
        ITokenRequestBuilder SetClientSecret(string clientSecret);

        #endregion Methods
    }
}
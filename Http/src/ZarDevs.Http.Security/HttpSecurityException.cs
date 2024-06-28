using System;

namespace ZarDevs.Http.Security
{
    /// <summary>
    /// Http security exception that should be handled.
    /// </summary>
    public class HttpSecurityException : Exception
    {
        #region Constructors

        /// <summary>
        /// Create a new instance of the <see cref="HttpSecurityException"/>
        /// </summary>
        /// <param name="message">Specify the message.</param>
        public HttpSecurityException(string message) : base(message)
        {
        }

        /// <summary>
        /// Create a new instance of the <see cref="HttpSecurityException"/>
        /// </summary>
        /// <param name="message">Specify the message.</param>
        /// <param name="innerException">Add the original exception that occurred.</param>
        public HttpSecurityException(string message, Exception innerException) : base(message, innerException)
        {
        }

        #endregion Constructors
    }
}
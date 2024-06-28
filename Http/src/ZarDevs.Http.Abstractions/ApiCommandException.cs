using System;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ZarDevs.Http.Api
{
    /// <summary>
    /// Api command exception that is used when there is api command errors. These exceptions are meant to be handled as they are thrown due to API communication or server errors and can be recovered.
    /// </summary>
    public class ApiCommandException : Exception
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiCommandException"/> with the response and message.
        /// </summary>
        /// <param name="response"></param>
        /// <param name="message"></param>
        public ApiCommandException(IApiCommandResponse response, string message) : base(message)
        {
            Response = response ?? throw new ArgumentNullException(nameof(response));
            StatusCode = response.StatusCode;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Get the <see cref="IApiCommandResponse"/> that came from the server.
        /// </summary>
        public IApiCommandResponse Response { get; }

        /// <summary>
        /// Get the <see cref="HttpStatusCode"/> received from the server.
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Get any error that has been serialized in the response body.
        /// </summary>
        /// <returns></returns>
        public async Task<TError> GetErrorBodyAsync<TError>()
        {
            if (Response == null)
                return default;

            return await Response.TryGetContentAsync<TError>();
        }

        /// <summary>
        /// Returns a human readable
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var message = new StringBuilder(Message)
                .AppendLine()
                .Append("StatusCode:").Append(StatusCode);

            return message.ToString();
        }

        #endregion Methods
    }
}
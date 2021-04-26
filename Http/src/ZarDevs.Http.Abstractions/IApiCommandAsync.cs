using System.Threading.Tasks;

namespace ZarDevs.Http.Api
{
    /// <summary>
    /// Api command interface that defines the base construct of the API Command infrastructure.
    /// </summary>
    public interface IApiCommandAsync
    {
        #region Methods

        /// <summary>
        /// Execute the command asyncronously. The command requires a request, that will be sent and processed and return a response.
        /// </summary>
        /// <param name="request">The request that will be sent to the server.</param>
        /// <returns>The response from the server.</returns>
        Task<IApiCommandResponse> ExecuteAsync(IApiCommandRequest request);

        #endregion Methods
    }
}
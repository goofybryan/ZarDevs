using System.Threading.Tasks;

namespace ZarDevs.Http
{
    public interface ICommandAsync
    {
        #region Methods

        Task ExecuteAsync();

        #endregion Methods
    }

    public interface ICommandAsync<in TRequest>
    {
        #region Methods

        Task ExecuteAsync(TRequest request);

        #endregion Methods
    }

    public interface ICommandAsync<in TRequest, TResponse>
    {
        #region Methods

        Task<TResponse> ExecuteAsync(TRequest request);

        #endregion Methods
    }
}
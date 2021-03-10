using System;
using ZarDevs.DependencyInjection;

namespace ZarDevs.Http.Api
{
    public interface IApiCommandFactory
    {
        #region Methods

        IApiCommandAsync<TRequest, TResponse> Create<TRequest, TResponse>(Enum name) where TRequest : IApiCommandRequest where TResponse : IApiCommandResponse;

        IApiCommandAsync<TRequest, TResponse> Create<TRequest, TResponse>(object name) where TRequest : IApiCommandRequest where TResponse : IApiCommandResponse;

        #endregion Methods
    }

    public class ApiCommandFactory : IApiCommandFactory
    {
        #region Fields

        private readonly IIocContainer _ioc;

        #endregion Fields

        #region Constructors

        public ApiCommandFactory(IIocContainer ioc)
        {
            _ioc = ioc ?? throw new ArgumentNullException(nameof(ioc));
        }

        #endregion Constructors

        #region Methods

        public IApiCommandAsync<TRequest, TResponse> Create<TRequest, TResponse>(Enum name) where TRequest : IApiCommandRequest where TResponse : IApiCommandResponse
        {
            return Create<TRequest, TResponse>(name);
        }

        public IApiCommandAsync<TRequest, TResponse> Create<TRequest, TResponse>(object name) where TRequest : IApiCommandRequest where TResponse : IApiCommandResponse
        {
            return _ioc.Resolve<IApiCommandAsync<TRequest, TResponse>>(name);
        }

        #endregion Methods
    }
}
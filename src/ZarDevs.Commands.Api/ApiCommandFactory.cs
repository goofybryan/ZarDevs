using System;
using ZarDevs.DependencyInjection;

namespace ZarDevs.Commands.Api
{
    public interface IApiCommandFactory
    {
        IApiCommandAsync<TRequest, TResponse> Create<TRequest, TResponse>(Enum name) where TRequest : IApiCommandRequest where TResponse : IApiCommandResponse;
        IApiCommandAsync<TRequest, TResponse> Create<TRequest, TResponse>(object name) where TRequest : IApiCommandRequest where TResponse : IApiCommandResponse;
    }

    public class ApiCommandFactory : IApiCommandFactory
    {
        private readonly IIocContainer _ioc;

        public ApiCommandFactory(IIocContainer ioc)
        {
            _ioc = ioc ?? throw new ArgumentNullException(nameof(ioc));
        }

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
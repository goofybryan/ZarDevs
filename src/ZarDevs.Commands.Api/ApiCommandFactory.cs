using System;

namespace ZarDevs.Commands.Api
{
    public interface IApiCommandFactory
    {
        IApiCommandAsync<TRequest, TResponse> Create<TRequest, TResponse>(Enum name) where TRequest : IApiCommandRequest where TResponse : IApiCommandResponse;
        IApiCommandAsync<TRequest, TResponse> Create<TRequest, TResponse>(object name) where TRequest : IApiCommandRequest where TResponse : IApiCommandResponse;
    }

    public class ApiCommandFactory : IApiCommandFactory
    {
        #region Methods

        public IApiCommandAsync<TRequest, TResponse> Create<TRequest, TResponse>(Enum name) where TRequest : IApiCommandRequest where TResponse : IApiCommandResponse
        {
            return Create<TRequest, TResponse>(name);
        }

        public IApiCommandAsync<TRequest, TResponse> Create<TRequest, TResponse>(object name) where TRequest : IApiCommandRequest where TResponse : IApiCommandResponse
        {
            return Ioc.Resolve<IApiCommandAsync<TRequest, TResponse>>(name);
        }

        #endregion Methods
    }
}
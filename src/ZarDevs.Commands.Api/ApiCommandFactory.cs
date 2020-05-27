using System;
using ZarDevs.DependencyInjection;

namespace ZarDevs.Commands.Api
{
    public static class ApiCommandFactory
    {
        #region Methods

        public static IApiCommandAsync<TRequest, TResponse> Create<TRequest, TResponse>(Enum name) where TRequest : IApiCommandRequest where TResponse : IApiCommandResponse
        {
            return Create<TRequest, TResponse>(name);
        }

        public static IApiCommandAsync<TRequest, TResponse> Create<TRequest, TResponse>(object name) where TRequest : IApiCommandRequest where TResponse : IApiCommandResponse
        {
            return Ioc.Resolve<IApiCommandAsync<TRequest, TResponse>>(name);
        }

        #endregion Methods
    }
}
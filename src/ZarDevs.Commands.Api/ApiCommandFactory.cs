using ZarDevs.DepencyInjection;
using System;

namespace ZarDevs.Commands.Api
{
    public static class ApiCommandFactory
    {
        public static IApiCommandAsync<TRequest, TResponse> Create<TRequest, TResponse>(Enum name) where TRequest : IApiCommandRequest where TResponse : IApiCommandResponse
        {
            return Create<TRequest, TResponse>(name.GetBindingName());
        }

        public static IApiCommandAsync<TRequest, TResponse> Create<TRequest, TResponse>(string name) where TRequest : IApiCommandRequest where TResponse : IApiCommandResponse
        {
            return Ioc.Get<IApiCommandAsync<TRequest, TResponse>>(name);
        }
    }
}

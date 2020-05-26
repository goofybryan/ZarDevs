using System;

namespace ZarDevs.Commands.Api
{
    public interface IApiCommandAsync<TRequest, TResponse> : ICommandAsync<TRequest, TResponse>, IDisposable where TRequest : IApiCommandRequest where TResponse : IApiCommandResponse
    {
    }
}
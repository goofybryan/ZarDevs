using System;

namespace ZarDevs.Http.Api
{
    public interface IApiCommandAsync<TRequest, TResponse> : ICommandAsync<TRequest, TResponse>, IDisposable where TRequest : IApiCommandRequest where TResponse : IApiCommandResponse
    {
    }
}
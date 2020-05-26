using System;

namespace ZarDevs.Commands.Api
{
    public interface IApiCommandRequest
    {
        #region Properties

        Uri ApiUri { get; }
        Guid Id { get; }
        HttpRequestType RequestType { get; }

        #endregion Properties
    }
}
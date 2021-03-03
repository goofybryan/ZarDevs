using System;

namespace ZarDevs.Http.Api
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
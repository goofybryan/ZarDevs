using System;

namespace ZarDevs.Commands.Api
{
    public class ApiCommandRequest : IApiCommandRequest
    {
        #region Constructors

        public ApiCommandRequest(HttpRequestType requestType, string apiPath)
            : this(requestType, new Uri(apiPath))
        {
        }

        public ApiCommandRequest(HttpRequestType requestType, Uri apiUri)
        {
            Id = Guid.NewGuid();
            RequestType = requestType;
            ApiUri = apiUri ?? throw new ArgumentNullException(nameof(apiUri));
        }

        #endregion Constructors

        #region Properties

        public Uri ApiUri { get; }
        public Guid Id { get; }
        public HttpRequestType RequestType { get; }

        #endregion Properties
    }
}
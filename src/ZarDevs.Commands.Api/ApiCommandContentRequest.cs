using System;

namespace ZarDevs.Commands.Api
{
    public class ApiCommandContentRequest : ApiCommandRequest
    {
        #region Constructors

        public ApiCommandContentRequest(HttpRequestType requestType, string requestUri, object payload)
            : this(requestType, new Uri(requestUri), payload)
        {
        }

        public ApiCommandContentRequest(HttpRequestType requestType, Uri requestUri, object payload)
            : base(requestType, requestUri)
        {
            Content = payload ?? throw new ArgumentNullException(nameof(payload));
        }

        #endregion Constructors

        #region Properties

        public object Content { get; }

        #endregion Properties
    }
}
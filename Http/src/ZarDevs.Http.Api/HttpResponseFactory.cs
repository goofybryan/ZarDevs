using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace ZarDevs.Http.Api
{
    internal class HttpResponseFactory : IHttpResponseFactory
    {
        private readonly IList<IApiCommandContentDeserializer> _deserializers;

        public HttpResponseFactory(IList<IApiCommandContentDeserializer> deserializers)
        {
            _deserializers = deserializers ?? throw new ArgumentNullException(nameof(deserializers));
        }

        #region Methods

        public IApiCommandResponse CreateResponse(HttpResponseMessage response)
        {
            return new ApiCommandResponse(this, response);
        }

        public IApiCommandContentDeserializer GetDeserializer(string mediaType)
        {
            return _deserializers.FirstOrDefault(d => d.IsValidFor(mediaType));
        }

        #endregion Methods
    }
}
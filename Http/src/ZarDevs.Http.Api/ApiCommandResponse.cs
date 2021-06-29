using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ZarDevs.Http.Api
{
    internal class ApiCommandResponse : IApiCommandResponse
    {
        #region Fields

        private readonly IHttpResponseFactory _responseFactory;

        #endregion Fields

        #region Constructors

        public ApiCommandResponse(IHttpResponseFactory responseFactory, HttpResponseMessage response)
        {
            _responseFactory = responseFactory ?? throw new ArgumentNullException(nameof(responseFactory));
            Response = response ?? throw new ArgumentNullException(nameof(response));
            HasContentType = Response.Content?.Headers?.ContentType is not null;
        }

        #endregion Constructors

        #region Properties

        public bool HasContentType { get; }
        public bool IsSuccess => Response.IsSuccessStatusCode;
        public string Reason => Response.ReasonPhrase;
        public HttpResponseMessage Response { get; }
        public HttpStatusCode StatusCode => Response.StatusCode;

        #endregion Properties

        #region Methods

        public void EnsureSuccess()
        {
            if (IsSuccess)
            {
                return;
            }

            throw new ApiCommandException(this, Reason);
        }

        public async Task<TContent> TryGetContent<TContent>()
        {
            EnsureSuccess();

            if (!HasContentType)
                return default;

            var serializer = _responseFactory.GetDeserializer(Response.Content.Headers.ContentType.MediaType);
            TContent content = await serializer.DeserializeAsync<TContent>(Response.Content);

            return content;
        }

        #endregion Methods
    }
}
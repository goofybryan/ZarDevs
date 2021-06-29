using NSubstitute;
using System.Net.Http;
using ZarDevs.Http.Client;

namespace ZarDevs.Http.Api.Tests
{
    public abstract class ApiCommandContentAsyncTests<T> : ApiCommandAsyncTests<T> where T : ApiContentCommandAsync
    {
        #region Fields

        private const string _requestContent = "Test";

        private readonly HttpContent _content;

        private readonly IApiCommandContentSerializer _serializerMock;

        #endregion Fields

        #region Constructors

        public ApiCommandContentAsyncTests()
        {
            _content = new StringContent(_requestContent);
            _serializerMock = Substitute.For<IApiCommandContentSerializer>();
        }

        #endregion Constructors

        #region Methods

        protected override void AssertMocks(IApiHttpClient clientMock, IHttpResponseFactory factoryMock, IApiCommandRequest requestMock, IApiCommandResponse responseMock, IApiCommandResponse commandResponse)
        {
            base.AssertMocks(clientMock, factoryMock, requestMock, responseMock, commandResponse);
        }

        protected override T CreateCommand(IApiHttpClient clientMock, IHttpResponseFactory factoryMock, HttpResponseMessage responseMesage)
        {
            return CreateCommand(clientMock, _serializerMock, factoryMock, responseMesage, _content);
        }

        protected abstract T CreateCommand(IApiHttpClient clientMock, IApiCommandContentSerializer serializerMock, IHttpResponseFactory factoryMock, HttpResponseMessage responseMesage, HttpContent content);

        protected override void MockRequest(IApiCommandRequest requestMock)
        {
            base.MockRequest(requestMock);
            _serializerMock.Serialize(requestMock).Returns(_content);
        }

        #endregion Methods
    }
}
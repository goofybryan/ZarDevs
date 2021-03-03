using System;
using System.Net;
using System.Net.Http;

namespace ZarDevs.Http.Api
{
    public interface IApiCommandResponse
    {
        #region Properties

        bool IsSuccess { get; }
        string Reason { get; }
        Guid? RequestId { get; set; }
        HttpResponseMessage Response { get; }
        HttpStatusCode StatusCode { get; }

        #endregion Properties
    }
}
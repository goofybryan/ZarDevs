using System;
using System.Net;
using System.Net.Http;

namespace ZarDevs.Commands.Api
{
    public abstract class ApiCommandResponse : IApiCommandResponse
    {
        #region Constructors

        public ApiCommandResponse(HttpResponseMessage response)
        {
            Response = Check.IsNotNull(response, nameof(response));
        }

        #endregion Constructors

        #region Properties

        public bool IsSuccess => Response.IsSuccessStatusCode;
        public string Reason => Response.ReasonPhrase;
        public Guid? RequestId { get; set; }
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

        #endregion Methods
    }
}
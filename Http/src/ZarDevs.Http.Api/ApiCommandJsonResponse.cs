using Newtonsoft.Json;
using System.Net.Http;

namespace ZarDevs.Http.Api
{
    public class ApiCommandJsonResponse : ApiCommandContentResponse<string>
    {
        #region Constructors

        public ApiCommandJsonResponse(HttpResponseMessage message)
            : base(message)
        {
        }

        #endregion Constructors

        #region Methods

        public override T GetResponseAs<T>()
        {
            EnsureSuccess();
            return JsonConvert.DeserializeObject<T>(Content);
        }

        #endregion Methods
    }
}
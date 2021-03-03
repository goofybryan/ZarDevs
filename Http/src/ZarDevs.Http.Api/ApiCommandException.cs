using System;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ZarDevs.Http.Api
{
    public class ApiCommandException : Exception
    {
        #region Constructors

        public ApiCommandException(ApiCommandResponse response, string message) : base(message)
        {
            StatusCode = Response.StatusCode;
            Response = response ?? throw new ArgumentNullException(nameof(response));
        }

        protected ApiCommandException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        #endregion Constructors

        #region Properties

        public ApiCommandResponse Response { get; }
        public HttpStatusCode StatusCode { get; set; }

        #endregion Properties

        #region Methods

        public async Task<string> GetErrorBodyAsync()
        {
            return await Response?.Response?.Content.ReadAsStringAsync();
        }

        public override string ToString()
        {
            var detailTask = ToStringAsync();
            detailTask.Wait();
            return detailTask.Result;
        }

        public async Task<string> ToStringAsync()
        {
            var message = new StringBuilder(Message).AppendLine()
                .AppendFormat("StatusCode:{0}", StatusCode);

            var detail = await GetErrorBodyAsync();
            if (string.IsNullOrWhiteSpace(detail))
                return message.ToString();

            return message.AppendLine()
                .AppendFormat("Detail:{0}", detail)
                .ToString();
        }

        #endregion Methods
    }
}
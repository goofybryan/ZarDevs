using System.Net.Http;

namespace ZarDevs.Commands.Api
{
    public abstract class ApiCommandContentResponse<TContent> : ApiCommandResponse
    {
        #region Constructors

        public ApiCommandContentResponse(HttpResponseMessage message)
            : base(message)
        {
        }

        #endregion Constructors

        #region Properties

        public TContent Content { get; set; }

        #endregion Properties

        #region Methods

        public abstract T GetResponseAs<T>();

        #endregion Methods
    }
}
namespace ZarDevs.Http.Api
{
    public static class HttpResponseFactoryExtensions
    {
        #region Methods

        public static TValue GetResponseAs<TValue>(this ApiCommandResponse response)
        {
            var value = default(TValue);

            if (response is ApiCommandJsonResponse jsonResponse)
            {
                value = jsonResponse.GetResponseAs<TValue>();
            }

            return value;
        }

        #endregion Methods
    }
}
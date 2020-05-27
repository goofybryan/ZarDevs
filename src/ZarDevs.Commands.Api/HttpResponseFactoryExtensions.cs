namespace ZarDevs.Commands.Api
{
    public static class HttpResponseFactoryExtensions
    {

        public static TValue GetResponseAs<TValue>(this ApiCommandResponse response)
        {
            var value = default(TValue);

            if (response is ApiCommandJsonResponse jsonResponse)
            {
                value = jsonResponse.GetResponseAs<TValue>();
            }

            return value;
        }

    }
}
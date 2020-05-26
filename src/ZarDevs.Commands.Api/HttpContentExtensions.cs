using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace ZarDevs.Commands.Api
{
    public static class HttpContentExtensions
    {
        #region Methods

        public static async Task<T> ReadAsJsonAsync<T>(this HttpContent content)
        {
            string json = await content.ReadAsStringAsync();
            T value = JsonConvert.DeserializeObject<T>(json);
            return value;
        }

        public static HttpContent WriteAsJson<T>(this T value, Formatting formatting = Formatting.None)
        {
            var jsonString = JsonConvert.SerializeObject(value, formatting);
            var content = new StringContent(jsonString, System.Text.Encoding.UTF8, "application/json");
            return content;
        }

        #endregion Methods
    }
}
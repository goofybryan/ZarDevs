namespace ZarDevs.Http.Security
{
    public class LoginToken
    {
        public string TokenType { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}

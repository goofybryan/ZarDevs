namespace ZarDevs.Http.Security
{
    public static class SecurityTokenConstants
    {
        #region Fields

        public const string ApiClientName = nameof(Security);
        public const string ApiConfiguration = nameof(ApiConfiguration);
        public const string DefaultClientCredentials = nameof(DefaultClientCredentials);
        public const string JwtAuthHeader = "bearer";
        public const string LoginToken = nameof(LoginToken);
        public const string OpenIdConfiguration = nameof(OpenIdConfiguration);
        public const string Scopes = nameof(Scopes);

        #endregion Fields
    }
}
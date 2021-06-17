namespace ZarDevs.Http.Security
{
    /// <summary>
    /// Constants class used by security API
    /// </summary>
    public static class SecurityTokenConstants
    {
        #region Fields

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public const string ApiClientName = nameof(Security);
        public const string ApiConfiguration = nameof(ApiConfiguration);
        public const string DefaultClientCredentials = nameof(DefaultClientCredentials);
        public const string JwtAuthHeader = "bearer";
        public const string LoginToken = nameof(LoginToken);
        public const string OpenIdConfiguration = nameof(OpenIdConfiguration);
        public const string Scopes = nameof(Scopes);
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        #endregion Fields
    }
}
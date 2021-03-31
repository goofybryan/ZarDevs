using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZarDevs.Http.Security
{
    internal class AccessTokenUsernameRequestBuilder : TokenRequestParameterBuilder, IAccessTokenUsernameRequestBuilder
    {
        #region Fields

        private readonly ISet<string> _scopes;

        #endregion Fields

        #region Constructors

        public AccessTokenUsernameRequestBuilder(string grantType) : base(grantType)
        {
            _scopes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        }

        #endregion Constructors

        #region Properties

        public string Password { get; set; }
        public ICollection<string> Scope => _scopes;
        public string Username { get; set; }

        #endregion Properties

        #region Methods

        public IAccessTokenUsernameRequestBuilder AddScopes(params string[] scopes)
        {
            if (scopes == null || scopes.Length == 0)
                return this;

            foreach (var scope in scopes)
            {
                _scopes.Add(scope);
            }

            return this;
        }

        public IAccessTokenUsernameRequestBuilder SetCredendials(string username, string password)
        {
            Password = password;
            Username = username;

            return this;
        }

        protected override void OnBuild(IDictionary<string, string> parameters)
        {
            Add(parameters, TokenRequestParameters.Password, Password);
            Add(parameters, TokenRequestParameters.Username, Username);

            if (Scope.Count > 0)
            {
                StringBuilder scopeBuilder = new StringBuilder();
                Scope.Aggregate(scopeBuilder, (seed, value) => scopeBuilder.Append(value).Append(" "));
                var scope = scopeBuilder.ToString().Trim();
                _ = TryAdd(parameters, TokenRequestParameters.Scope, scope);
            }
        }

        #endregion Methods
    }
}
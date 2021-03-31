using System;
using System.Collections.Generic;

namespace ZarDevs.Http.Security
{
    internal abstract class TokenRequestParameterBuilder : ITokenRequestBuilder
    {
        #region Constructors

        protected TokenRequestParameterBuilder(string grantType)
        {
            if (string.IsNullOrWhiteSpace(grantType))
            {
                throw new System.ArgumentException($"'{nameof(grantType)}' cannot be null or whitespace.", nameof(grantType));
            }

            GrantType = grantType;
        }

        #endregion Constructors

        #region Properties

        public string ClientId { get; set; }
        public string ClientSecret { get; set; }

        public string GrantType { get; }

        #endregion Properties

        #region Methods

        public Dictionary<string, string> Build()
        {
            var parameters = new Dictionary<string, string>
            {
                { TokenRequestParameters.GrantType, GrantType },
                { TokenRequestParameters.ClientId, ClientId },
                { TokenRequestParameters.ClientSecret, ClientSecret },
            };

            OnBuild(parameters);

            return parameters;
        }

        public ITokenRequestBuilder SetClientId(string clientId)
        {
            ClientId = clientId;
            return this;
        }

        public ITokenRequestBuilder SetClientSecret(string clientSecret)
        {
            ClientSecret = clientSecret;
            return this;
        }

        protected void Add(IDictionary<string, string> parameters, string key, string value)
        {
            if (!TryAdd(parameters, key, value))
                throw new InvalidOperationException($"The value for '{key}' can not be an empty value.");
        }

        protected abstract void OnBuild(IDictionary<string, string> parameters);

        protected bool TryAdd(IDictionary<string, string> parameters, string key, string value)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException($"'{nameof(key)}' cannot be null or whitespace.", nameof(key));
            }

            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

            parameters[key] = value;
            return true;
        }

        #endregion Methods
    }
}
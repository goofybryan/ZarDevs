using System;

namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// Dependency resolution configured
    /// </summary>
    [Serializable]
    public class DependencyResolutionNotConfiguredException : Exception
    {
        private const string _noKeySpecified = "No Key Specified.";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestType"></param>
        /// <param name="key"></param>
        public DependencyResolutionNotConfiguredException(Type requestType, object key) 
            : base($"No dependency resolution configured for type '{requestType}', with key '{(key != null ? key.ToString() : _noKeySpecified)}'")
        {
            RequestType = requestType;
            Key = key;
        }

        /// <summary>
        /// Insternal serialization constructor
        /// </summary>
        protected DependencyResolutionNotConfiguredException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }

        /// <summary>
        /// Return the request type.
        /// </summary>
        public Type RequestType { get; }

        /// <summary>
        /// Return the key
        /// </summary>
        public object Key { get; }
    }
}
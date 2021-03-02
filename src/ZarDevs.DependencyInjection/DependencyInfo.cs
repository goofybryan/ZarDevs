using System;

namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// Base dependency info class that describes the <see cref="RequestType"/>, <see cref="Scope"/> and optionally <see cref="Key"/> for any binding.
    /// </summary>
    public class DependencyInfo : IDependencyInfo
    {
        #region Constructors

        /// <summary>
        /// Create a new empty instance
        /// </summary>
        public DependencyInfo()
        {
        }

        /// <summary>
        /// Create a new instance with copied values.
        /// </summary>
        /// <param name="copy">The base instance to copy from.</param>
        protected DependencyInfo(DependencyInfo copy)
        {
            Key = copy.Key;
            Scope = copy.Scope;
            RequestType = copy.RequestType;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// An optional key that will be used to define the binding.
        /// </summary>
        public object Key { get; set; } = "";

        /// <summary>
        /// The scope that the binding will be defined to, default is <see cref="DependyBuilderScope.Transient"/>
        /// </summary>
        public DependyBuilderScope Scope { get; protected set; }

        /// <summary>
        /// The request type that will be used to define what needs to be resolved.
        /// </summary>
        public Type RequestType { get; set; }

        #endregion Properties

        internal virtual void SetScope(DependyBuilderScope scope)
        {
            Scope = scope;
        }

        /// <summary>
        /// Overridden ToString describing the class properties.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"Dependency Info: Key={Key}, Scope={Scope}, RequestType={RequestType}";
        }
    }
}
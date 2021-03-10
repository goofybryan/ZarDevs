using System;

namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// Base dependency info class that describes the <see cref="RequestType"/>, <see cref="Scope"/>
    /// and optionally <see cref="Key"/> for any binding.
    /// </summary>
    public class DependencyInfo : IDependencyInfo
    {
        #region Constructors

        /// <summary>
        /// Create a new instance with the base variables set.
        /// </summary>
        /// <param name="requestType">Specifiy the request type, must not be null.</param>
        /// <param name="key">Specify the key, optional.</param>
        /// <param name="scope">Specify the scope, optional, default is <see cref="DependyBuilderScope.Transient"/></param>
        public DependencyInfo(Type requestType, object key = null, DependyBuilderScope scope = DependyBuilderScope.Transient)
        {
            RequestType = requestType ?? throw new ArgumentNullException(nameof(requestType));
            Key = key;
            Scope = scope;
        }

        /// <summary>
        /// Create a new empty instance
        /// </summary>
        internal DependencyInfo()
        {
        }

        /// <summary>
        /// Create a new instance with copied values.
        /// </summary>
        /// <param name="copy">The base instance to copy from.</param>
        protected DependencyInfo(IDependencyInfo copy) : this(copy.RequestType, copy.Key, copy.Scope)
        {
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// An optional key that will be used to define the binding.
        /// </summary>
        public object Key { get; set; }

        /// <summary>
        /// The request type that will be used to define what needs to be resolved.
        /// </summary>
        public Type RequestType { get; set; }

        /// <summary>
        /// The scope that the binding will be defined to, default is <see cref="DependyBuilderScope.Transient"/>
        /// </summary>
        public DependyBuilderScope Scope { get; protected set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Create a new instance of the dependency context.
        /// </summary>
        /// <returns></returns>
        public IDependencyContext CreateContext(IIocContainer ioc)
        {
            return new DependencyContext(ioc, this);
        }

        /// <summary>
        /// Overridden ToString describing the class properties.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"Dependency Info: Key={Key}, Scope={Scope}, RequestType={RequestType}";
        }

        internal virtual void SetScope(DependyBuilderScope scope)
        {
            Scope = scope;
        }

        #endregion Methods
    }
}
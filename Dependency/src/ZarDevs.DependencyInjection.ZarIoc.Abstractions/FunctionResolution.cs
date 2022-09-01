using System;

namespace ZarDevs.DependencyInjection.ZarIoc
{
    /// <summary>
    /// Instance type resolution that will remember the value once created.
    /// </summary>
    public sealed class FunctionResolution : ITypeResolution
    {
        #region Fields

        private readonly IDependencyMethodInfo _methodInfo;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Create a new instance of <see cref="InstanceResolution"/>
        /// </summary>
        /// <param name="methodInfo">The method info</param>
        public FunctionResolution(IDependencyMethodInfo methodInfo)
        {
            _methodInfo = methodInfo ?? throw new ArgumentNullException(nameof(methodInfo));
            Key = methodInfo.Key;
        }

        #endregion Constructors

        #region Properties

        /// <inheritdoc/>
        public IDependencyInfo Info => _methodInfo;

        /// <inheritdoc/>
        public object Key { get; }

        #endregion Properties

        #region Methods

        /// <inheritdoc/>
        public object Resolve()
        {
            return _methodInfo.Execute(_methodInfo.CreateContext(Ioc.Container));
        }

        /// <inheritdoc/>
        public object Resolve(params object[] parameters)
        {
            return _methodInfo.Execute(_methodInfo.CreateContext(Ioc.Container).SetArguments(parameters));
        }

        /// <inheritdoc/>
        public object Resolve(params (string, object)[] parameters)
        {
            return _methodInfo.Execute(_methodInfo.CreateContext(Ioc.Container).SetArguments(parameters));
        }

        #endregion Methods
    }
}
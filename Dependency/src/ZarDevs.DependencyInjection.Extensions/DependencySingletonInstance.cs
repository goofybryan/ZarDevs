using System;

namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// Dpenedncy instance resolution, this will always return the instance described by the <see cref="IDependencyInstanceInfo"/>
    /// </summary>
    public class DependencySingletonInstance : IDependencyResolution, IDisposable
    {
        #region Fields

        private readonly IDependencyInstanceInfo _info;

        private bool _isDisposed;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Create a new instance of of the dependency resolution.
        /// </summary>
        /// <param name="info">
        /// The the <see cref="IDependencyInstanceInfo"/> that describes this resolution.
        /// </param>
        public DependencySingletonInstance(IDependencyInstanceInfo info)
        {
            _info = info ?? throw new ArgumentNullException(nameof(info));
        }

        #endregion Constructors

        #region Properties

        private object Instance => _info.Instance;

        /// <summary>
        /// The key that is associated to this resolution.
        /// </summary>
        public object Key => _info.Key;

        #endregion Properties

        #region Methods

        /// <summary>
        /// Dispose of the underlying resources. If any <see cref="IDependencyResolution"/>
        /// implements <see cref="IDisposable"/> that will be called.
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// This is not supported by this class.
        /// </summary>
        public IDependencyResolution MakeConcrete(Type concreteRequest)
        {
            throw new NotSupportedException("An instance resolution does not support generic types.");
        }

        /// <summary>
        /// Returns the instance in the described <see cref="IDependencyInstanceInfo"/>
        /// </summary>
        /// <returns>Returns the instance in the described <see cref="IDependencyInstanceInfo.Instance"/></returns>
        public object Resolve()
        {
            return Instance;
        }

        /// <summary>
        /// Returns the instance in the described <see cref="IDependencyInstanceInfo"/>
        /// </summary>
        /// <param name="args">Never used.</param>
        /// <returns>Returns the instance in the described <see cref="IDependencyInstanceInfo.Instance"/></returns>
        public object Resolve(object[] args)
        {
            return Instance;
        }

        /// <summary>
        /// Returns the instance in the described <see cref="IDependencyInstanceInfo"/>
        /// </summary>
        /// <param name="args">Never used.</param>
        /// <returns>Returns the instance in the described <see cref="IDependencyInstanceInfo.Instance"/></returns>
        public object Resolve((string, object)[] args)
        {
            return Instance;
        }

        /// <summary>
        /// Resolve and return the instance
        /// </summary>
        /// <param name="context">The dependency context.</param>
        /// <returns>An instance for this resolution.</returns>
        public object Resolve(IDependencyContext context)
        {
            return Instance;
        }

        /// <summary>
        /// Dispose of the underlying resources. If any <see cref="IDependencyResolution"/>
        /// implements <see cref="IDisposable"/> that will be called.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed) return;

            if (disposing && Instance is IDisposable disposable)
            {
                disposable.Dispose();
            }

            _isDisposed = true;
        }

        #endregion Methods
    }
}
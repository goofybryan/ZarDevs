using System;

namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// Dependency thread resolution tracker that will track a value at thread level
    /// </summary>
    public sealed class ThreadResolutionTracker : IDisposable
    {
        #region Fields

        private bool _disposedValue;

        private ITypeResolution _resolution;

        private object _value;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Create a new instance of the <see cref="ThreadResolutionTracker"/>
        /// </summary>
        /// <param name="resolution">The resolution to track.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public ThreadResolutionTracker(ITypeResolution resolution)
        {
            _resolution = resolution ?? throw new ArgumentNullException(nameof(resolution));
        }

        #endregion Constructors

        #region Destructors

        /// <summary>
        /// Destructor for the resolution.
        /// </summary>
        ~ThreadResolutionTracker()
        {
            Dispose(false);
        }

        #endregion Destructors

        #region Methods

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Resolve the value. If the value is not created, otherwise will return the internal value.
        /// </summary>
        /// <returns>The value for the current thread.</returns>
        public object Resolve()
        {
            return _value ??= _resolution.Resolve();
        }

        /// <summary>
        /// Resolve the value. If the value is not created, the <paramref name="resolve"/> will be
        /// invoked, otherwise will return the internal value.
        /// </summary>
        /// <param name="resolve">The resolve function.</param>
        /// <returns>The value for the current thread.</returns>
        public object Resolve(Func<ITypeResolution, object> resolve)
        {
            return _value ??= resolve(_resolution);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    if (_resolution is IDisposable disposable)
                    {
                        disposable?.Dispose();
                    }
                }

                _value = null;
                _disposedValue = true;
            }
        }

        #endregion Methods
    }
}
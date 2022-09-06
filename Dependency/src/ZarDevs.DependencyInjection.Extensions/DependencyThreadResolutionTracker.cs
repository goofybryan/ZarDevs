using System;

namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// Dependency thread resolution tracker that will track a value at thread level
    /// </summary>
    public sealed class DependencyThreadResolutionTracker : IDisposable
    {
        #region Fields

        private bool _disposedValue;

        private IDependencyResolution _resolution;

        private object _value;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Create a new instance of the <see cref="DependencyThreadResolutionTracker"/>
        /// </summary>
        /// <param name="resolution">The resolution to track.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public DependencyThreadResolutionTracker(IDependencyResolution resolution)
        {
            _resolution = resolution ?? throw new ArgumentNullException(nameof(resolution));
        }

        #endregion Constructors

        #region Destructors

        /// <summary>
        /// Destructor for the resolution.
        /// </summary>
        ~DependencyThreadResolutionTracker()
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
        /// Resolve the value. If the value is not created, the <paramref name="resolve"/> will be invoked, otherwise will return the internal value.
        /// </summary>
        /// <param name="resolve">The resolve function.</param>
        /// <returns>The value for the current thread.</returns>
        public object Resolve(Func<IDependencyResolution, object> resolve)
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
#if NET5_0_OR_GREATER
                    else if (_resolution is IAsyncDisposable asyncDisposable)
                    {
                        asyncDisposable.DisposeAsync();
                    }
#endif
                }

                _value = null;
                _disposedValue = true;
            }
        }

        #endregion Methods
    }
}
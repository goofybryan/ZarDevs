using System;

namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// Instance type resolution that will remember the value once created.
    /// </summary>
    public sealed class InstanceResolution : ITypeResolution, IDisposable
    {
        #region Fields

        private bool _disposedValue;
        private readonly object _value;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Create a new instance of <see cref="InstanceResolution"/>
        /// </summary>
        /// <param name="value">
        /// The instance value
        /// </param>
        /// <param name="key">
        /// Optional key
        /// </param>
        public InstanceResolution(object value, object key)
        {
            _value = value ?? throw new ArgumentNullException(nameof(value));
            Key = key;
        }

        #endregion Constructors

        #region Properties

        /// <inheritdoc/>
        public object Key { get; }

        #endregion Properties

        #region Methods

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc/>
        public object Resolve()
        {
            return _value;
        }

        /// <inheritdoc/>
        public object Resolve(params object[] parameters)
        {
            return _value;
        }

        /// <inheritdoc/>
        public object Resolve(params (string, object)[] parameters)
        {
            return _value;
        }

        private void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing && _value is IDisposable disposable)
                {
                    disposable.Dispose();
                }

                _disposedValue = true;
            }
        }

        #endregion Methods
    }
}
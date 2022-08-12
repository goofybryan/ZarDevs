using System;

namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// Instance type resolution that will remember the value once created.
    /// </summary>
    public sealed class InstanceResolution : ITypeResolution, IDisposable
    {
        #region Fields

        private readonly IDependencyInstanceInfo _instanceInfo;
        private readonly object _value;
        private bool _disposedValue;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Create a new instance of <see cref="InstanceResolution"/>
        /// </summary>
        /// <param name="instanceInfo">The instance value</param>
        public InstanceResolution(IDependencyInstanceInfo instanceInfo)
        {
            _instanceInfo = instanceInfo ?? throw new ArgumentNullException(nameof(instanceInfo));
            _value = instanceInfo.Instance;
        }

        #endregion Constructors

        #region Properties

        /// <inheritdoc/>
        public IDependencyInfo Info => _instanceInfo;

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
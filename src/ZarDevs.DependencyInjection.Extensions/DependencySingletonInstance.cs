using System;

namespace ZarDevs.DependencyInjection
{
    public class DependencySingletonInstance : IDependencyResolution, IDisposable
    {
        #region Fields

        private readonly IDependencyInstanceInfo _info;

        private bool _isDisposed;

        #endregion Fields

        #region Constructors

        public DependencySingletonInstance(IDependencyInstanceInfo info)
        {
            _info = info ?? throw new ArgumentNullException(nameof(info));
        }

        #endregion Constructors

        #region Properties

        public object Instance => _info.Instance;
        public object Key => _info.Key;

        #endregion Properties

        #region Methods

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public object Resolve()
        {
            return Instance;
        }

        public object Resolve(object[] args)
        {
            return Resolve();
        }

        public object Resolve((string, object)[] args)
        {
            return Resolve();
        }

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
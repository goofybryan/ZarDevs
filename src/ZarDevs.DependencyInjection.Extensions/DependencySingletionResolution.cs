using System;

namespace ZarDevs.DependencyInjection
{
    public class DependencySingletionResolution<TInfo, TResolution> : IDependencyResolution<TInfo>, IDisposable where TInfo : IDependencyInfo where TResolution : IDependencyResolution<TInfo>
    {
        #region Fields

        private readonly TResolution _resolution;
        private bool _isDisposed;

        #endregion Fields

        #region Constructors

        public DependencySingletionResolution(TResolution resolution)
        {
            _resolution = resolution ?? throw new ArgumentNullException(nameof(resolution));
        }

        #endregion Constructors

        #region Properties

        public TInfo Info => _resolution.Info;
        public object Key => _resolution.Key;
        public object Resolved { get; private set; }

        #endregion Properties

        #region Methods

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public object Resolve(params object[] args)
        {
            return Resolved ??= _resolution.Resolve(args);
        }

        public object Resolve(params (string, object)[] args)
        {
            return Resolved ??= _resolution.Resolve(args);
        }

        public object Resolve()
        {
            return Resolved ??= _resolution.Resolve();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed) return;

            if (disposing && Resolved is IDisposable disposable)
            {
                disposable.Dispose();
            }

            Resolved = null;

            _isDisposed = true;
        }

        #endregion Methods
    }
}
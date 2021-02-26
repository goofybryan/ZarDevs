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

        public object Resolve(IIocContainer ioc, params object[] args)
        {
            return Resolved ??= _resolution.Resolve(ioc, args);
        }

        public object Resolve(IIocContainer ioc, params (string, object)[] args)
        {
            return Resolved ??= _resolution.Resolve(ioc, args);
        }

        public object Resolve(IIocContainer ioc)
        {
            return Resolved ??= _resolution.Resolve(ioc);
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
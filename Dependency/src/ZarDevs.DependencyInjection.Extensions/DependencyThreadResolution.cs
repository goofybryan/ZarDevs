using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// Dependency singleton resolution is used when you want to define a resolution that needs to
    /// be resolved once and then always return the same instance.
    /// </summary>
    /// <typeparam name="TInfo">The <see cref="IDependencyInfo"/> describing this resolution.</typeparam>
    /// <typeparam name="TResolution">
    /// The underlying <see cref="IDependencyResolution"/> that will be used to resolve the initial instance.
    /// </typeparam>
    public class DependencyThreadResolution<TInfo, TResolution> : IDependencyResolution<TInfo>, IDisposable where TInfo : IDependencyInfo where TResolution : IDependencyResolution<TInfo>
    {
        #region Fields

        private readonly TResolution _resolution;
        private bool _isDisposed;
        private ThreadLocal<DependencyThreadResolutionTracker> _threadResolution;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Create a new instance of the singleton resolution.
        /// </summary>
        /// <param name="resolution">
        /// The underlying resolution that will be used to initialy resolve with.
        /// </param>
        public DependencyThreadResolution(TResolution resolution)
        {
            _resolution = resolution ?? throw new ArgumentNullException(nameof(resolution));
            _threadResolution = new ThreadLocal<DependencyThreadResolutionTracker>(() => new DependencyThreadResolutionTracker(_resolution));
        }

        #endregion Constructors

        #region Properties

        /// <inheritdoc/>
        public TInfo Info => _resolution.Info;

        /// <inheritdoc/>
        public object Key => _resolution.Key;

        /// <inheritdoc/>
        public ISet<Type> ResolvedTypes => _resolution.ResolvedTypes;


        /// <inheritdoc/>
        public bool IsGenericType => Info.ResolvedTypes.Any(x => x.IsGenericType);

        private DependencyThreadResolutionTracker ThreadTracker => _threadResolution.Value;

        #endregion Properties

        #region Methods

        /// <inheritdoc/>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc/>
        public IDependencyResolution MakeConcrete(Type concreteRequest)
        {
            var concreteResolution = (TResolution)_resolution.MakeConcrete(concreteRequest);

            return new DependencyThreadResolution<TInfo, TResolution>(concreteResolution);
        }

        /// <inheritdoc/>
        public object Resolve(params object[] args)
        {
            return ThreadTracker.Resolve(_resolution => _resolution.Resolve(args));
        }

        /// <inheritdoc/>
        public object Resolve(params (string, object)[] args)
        {
            return ThreadTracker.Resolve(_resolution => _resolution.Resolve(args));
        }

        /// <inheritdoc/>
        public object Resolve()
        {
            return ThreadTracker.Resolve(_resolution => _resolution.Resolve());
        }

        /// <inheritdoc/>
        public object Resolve(IDependencyContext context)
        {
            return ThreadTracker.Resolve(_resolution => _resolution.Resolve(context));
        }

        /// <inheritdoc/>
        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed) return;

            if(disposing)
            {
                ThreadTracker.Dispose();
            }

            _isDisposed = true;
        }

        #endregion Methods
    }
}
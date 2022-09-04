using System;
using System.Collections.Generic;

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
    public class DependencySingletionResolution<TInfo, TResolution> : IDependencyResolution<TInfo>, IDisposable where TInfo : IDependencyInfo where TResolution : IDependencyResolution<TInfo>
    {
        #region Fields

        private readonly TResolution _resolution;
        private bool _isDisposed;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Create a new instance of the singleton resolution.
        /// </summary>
        /// <param name="resolution">
        /// The underlying resolution that will be used to initialy resolve with.
        /// </param>
        public DependencySingletionResolution(TResolution resolution)
        {
            _resolution = resolution ?? throw new ArgumentNullException(nameof(resolution));
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// The <see cref="IDependencyInfo"/> impementation describing this resolution.
        /// </summary>
        public TInfo Info => _resolution.Info;

        /// <summary>
        /// The key that is associated to this resolution.
        /// </summary>
        public object Key => _resolution.Key;

        /// <summary>
        /// Get the request type that this resolution is for.
        /// </summary>
        public ISet<Type> ResolvedTypes => _resolution.ResolvedTypes;

        /// <inheritdoc/>
        public bool IsGenericType => false;

        private object Resolved { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Dispose of the underlying resources. If any <see cref="IDependencyResolution"/>
        /// implements <see cref="IDisposable"/> that will be called.
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Make a concrete singleton resolution
        /// </summary>
        /// <param name="concreteRequest">The concrete request type.</param>
        /// <returns></returns>
        public IDependencyResolution MakeConcrete(Type concreteRequest)
        {
            var concreteResolution = (TResolution)_resolution.MakeConcrete(concreteRequest);

            return new DependencySingletionResolution<TInfo, TResolution>(concreteResolution);
        }

        /// <summary>
        /// Initially resolve the instance and from then always return the instance.
        /// </summary>
        /// <param name="args">
        /// A list of ordered constructor arguments. They will be used if the instance has not been
        /// resolved, if it has, they will be ignored
        /// </param>
        /// <returns>An instance for this resolution.</returns>
        public object Resolve(params object[] args)
        {
            return Resolved ??= _resolution.Resolve(args);
        }

        /// <summary>
        /// Initially resolve the instance and from then always return the instance.
        /// </summary>
        /// <param name="args">
        /// A list of named constructor arguments. They will be used if the instance has not been
        /// resolved, if it has, they will be ignored
        /// </param>
        /// <returns>An instance for this resolution.</returns>
        public object Resolve(params (string, object)[] args)
        {
            return Resolved ??= _resolution.Resolve(args);
        }

        /// <summary>
        /// Initially resolve the instance and from then always return the instance.
        /// </summary>
        /// <returns>An instance for this resolution.</returns>
        public object Resolve()
        {
            return Resolved ??= _resolution.Resolve();
        }

        /// <summary>
        /// Resolve and return the instance
        /// </summary>
        /// <param name="context">The dependency context.</param>
        /// <returns>An instance for this resolution.</returns>
        public object Resolve(IDependencyContext context)
        {
            return Resolved ??= _resolution.Resolve(context);
        }

        /// <summary>
        /// Dispose of the underlying resources. If any <see cref="IDependencyResolution"/>
        /// implements <see cref="IDisposable"/> that will be called.
        /// </summary>
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
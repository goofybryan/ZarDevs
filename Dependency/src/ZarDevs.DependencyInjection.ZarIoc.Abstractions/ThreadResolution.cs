﻿using System;
using System.Threading;

namespace ZarDevs.DependencyInjection.ZarIoc
{
    /// <summary>
    /// Thread type resolution that will remember the value once created.
    /// </summary>
    public sealed class ThreadResolution : IDependencyResolution, IDisposable
    {
        #region Fields

        private readonly IDependencyResolution _baseResolution;
        private bool _disposedValue;
        private readonly ThreadLocal<ThreadResolutionTracker> _threadTracked;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Create a new instance of <see cref="SingletonResolution"/>
        /// </summary>
        /// <param name="baseResolution">
        /// The type resolution that will be used to initially resolve the value.
        /// </param>
        public ThreadResolution(IDependencyResolution baseResolution)
        {
            _baseResolution = baseResolution;
            _threadTracked = new ThreadLocal<ThreadResolutionTracker>(() => new ThreadResolutionTracker(_baseResolution));
        }

        #endregion Constructors

        #region Properties

        /// <inheritdoc/>
        public IDependencyInfo Info => _baseResolution.Info;

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
            return _threadTracked.Value.Resolve();
        }

        /// <inheritdoc/>
        public object Resolve(params object[] parameters)
        {
            return _threadTracked.Value.Resolve(resolution => resolution.Resolve(parameters));
        }

        /// <inheritdoc/>
        public object Resolve(params (string, object)[] parameters)
        {
            return _threadTracked.Value.Resolve(resolution => resolution.Resolve(parameters));
        }

        private void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _threadTracked.Dispose();
                }

                _disposedValue = true;
            }
        }

        #endregion Methods
    }
}
﻿using System;

namespace ZarDevs.DependencyInjection.ZarIoc
{
    /// <summary>
    /// Singleton type resolution that will remember the value once created.
    /// </summary>
    public sealed class SingletonResolution : IDependencyResolution, IDisposable
    {
        #region Fields

        private readonly IDependencyResolution _baseResolution;
        private bool _disposedValue;
        private object _value;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Create a new instance of <see cref="SingletonResolution"/>
        /// </summary>
        /// <param name="baseResolution">
        /// The type resolution that will be used to initially resolve the value.
        /// </param>
        public SingletonResolution(IDependencyResolution baseResolution)
        {
            _baseResolution = baseResolution;
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
            return _value ??= _baseResolution.Resolve();
        }

        /// <inheritdoc/>
        public object Resolve(params object[] parameters)
        {
            return _value ??= _baseResolution.Resolve(parameters);
        }

        /// <inheritdoc/>
        public object Resolve(params (string, object)[] parameters)
        {
            return _value ??= _baseResolution.Resolve(parameters);
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
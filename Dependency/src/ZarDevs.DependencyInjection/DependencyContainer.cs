using System;
using System.Collections.Generic;

namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// Dependency container base implementation.
    /// </summary>
    public abstract class DependencyContainerBase : IDependencyContainer
    {
        #region Fields

        private bool _isDisposed;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Base constructor
        /// </summary>
        protected DependencyContainerBase()
        {
            Definitions = new DependencyDefinitions();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// A list of definitions that have been added.
        /// </summary>
        protected IDependencyDefinitions Definitions { get; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Build the dependencies.
        /// </summary>
        /// <param name="definitions">The list of definitions to transform.</param>
        public void Build(IList<IDependencyInfo> definitions)
        {
            if (definitions is null)
            {
                throw new ArgumentNullException(nameof(definitions));
            }

            OnBuildStart();

            foreach (IDependencyInfo definition in definitions)
            {
                Definitions.Add(definition);
                OnBuild(definition);
            }

            OnBuildEnd();
        }

        /// <summary>
        /// Dispose of any resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Retrieve a dependency binding information when required.
        /// </summary>
        /// <param name="requestType">The request type to retrieve.</param>
        /// <param name="key">A key that the binding is associated with, can be null.</param>
        public IDependencyInfo TryGetBinding(Type requestType, object key)
        {
            return Definitions.TryGet(requestType, key);
        }

        /// <summary>
        /// Dispose of any resources.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed) return;

            _isDisposed = true;
        }

        /// <summary>
        /// Implement the on build method that will be called for each definition added.
        /// </summary>
        /// <param name="definition">The dependency info that describes what is required.</param>
        protected abstract void OnBuild(IDependencyInfo definition);

        /// <summary>
        /// Virtual method that occurs when the build ends. Does not need to be called when overridden.
        /// </summary>
        protected virtual void OnBuildEnd()
        { }

        /// <summary>
        /// Virtual method that occurs when the build starts. Does not need to be called when overridden.
        /// </summary>
        protected virtual void OnBuildStart()
        { }

        #endregion Methods
    }
}
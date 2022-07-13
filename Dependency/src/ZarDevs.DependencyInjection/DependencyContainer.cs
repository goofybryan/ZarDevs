using System;
using System.Collections.Generic;
using System.Globalization;

namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// Dependency bindings base implementation.
    /// </summary>
    public abstract class DependencyContainerBase<TBindingCollection> : IDependencyContainer where TBindingCollection : class
    {
        #region Fields

        private bool _isDisposed;
        private readonly IDependencyScopeCompiler<TBindingCollection> _scopeCompiler;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Base constructor
        /// </summary>
        protected DependencyContainerBase(TBindingCollection bindings, IDependencyScopeCompiler<TBindingCollection> scopeCompiler)
        {
            Definitions = new DependencyDefinitions();
            Bindings = bindings ?? throw new ArgumentNullException(nameof(bindings));
            _scopeCompiler = scopeCompiler ?? throw new ArgumentNullException(nameof(scopeCompiler));
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// A list of definitions that have been added.
        /// </summary>
        protected IDependencyDefinitions Definitions { get; }

        /// <summary>
        /// The dependecy bindings.
        /// </summary>
        protected TBindingCollection Bindings { get; }

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
                Validate(definition);
                Definitions.Add(definition);

                var binder = _scopeCompiler.FindBinder(definition) ??
                    throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "The binding for the type '{0}' is invalid. The binding has not been configured correctly", string.Join(", ", definition.ResolvedTypes)));

                OnBuild(binder, definition);
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
        public virtual IDependencyInfo TryGetBinding(Type requestType, object key)
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
        /// <param name="binder">Dependency scope compiler used to define what</param>
        /// <param name="definition">The dependency info that describes what is required.</param>
        protected virtual void OnBuild(IDependencyScopeBinder<TBindingCollection> binder, IDependencyInfo definition)
        {
            binder.Bind(Bindings, definition);
        }

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

        private static void Validate(IDependencyInfo definition)
        {
            if (definition.ResolvedTypes?.Count > 0) return;

            throw new InvalidOperationException($"The definition '{definition}', does not contain any resolve types.");
        }

        #endregion Methods
    }
}
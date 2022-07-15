using System;

namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// Dependency scope binder that is used to define what to do with a <see
    /// cref="IDependencyBuilderInfo"/> and the scope binding
    /// </summary>
    public class DependencyScopeBinder<TContainer> : DependencyScopeBinderBase<TContainer> where TContainer : class
    {
        #region Fields

        private readonly Action<TContainer, IDependencyInfo> _bindingAction;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Create a new instance of the binder and specify the scope and binding action
        /// </summary>
        /// <param name="scope">The scope that this is valid for.</param>
        /// <param name="bindingAction">
        /// The binding action that will be used when the <paramref name="scope"/> is valid.
        /// </param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public DependencyScopeBinder(DependyBuilderScopes scope, Action<TContainer, IDependencyInfo> bindingAction) : base(scope)
        {
            _bindingAction = bindingAction ?? throw new System.ArgumentNullException(nameof(bindingAction));
        }

        #endregion Constructors

        #region Methods

        /// <inheritdoc/>
        protected override void OnBind(TContainer container, IDependencyInfo definition)
        {
            _bindingAction(container, definition);
        }

        #endregion Methods
    }
}
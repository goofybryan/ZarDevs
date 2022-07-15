using System;

namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// Dependency scope binder that is used to define what to do with a <see
    /// cref="IDependencyBuilderInfo"/> and the scope binding
    /// </summary>
    public abstract class DependencyScopeBinderBase<TContainer> : IDependencyScopeBinder<TContainer> where TContainer : class
    {
        #region Fields


        #endregion Fields

        #region Constructors

        /// <summary>
        /// Create a new instance of the binder and specify the scopes and binding action
        /// </summary>
        /// <param name="scopes">The scopes that this is valid for.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public DependencyScopeBinderBase(DependyBuilderScopes scopes)
        {
            Scopes = scopes;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// The scope that the binder is valid for.
        /// </summary>
        public DependyBuilderScopes Scopes { get; }

        #endregion Properties

        #region Methods

        /// <inheritdoc/>
        public void Bind(TContainer container, IDependencyInfo definition)
        {
            if (!CanBind(definition))
                throw new NotSupportedException($"The definition scope '{definition.Scope}' is not valid for this binder ('{Scopes}').");

            OnBind(container, definition);
        }

        /// <inheritdoc/>
        public virtual bool CanBind(IDependencyInfo definition) => Scopes.HasFlag(definition.Scope);

        /// <summary>
        /// Bind the definition to the container.
        /// </summary>
        /// <param name="container">The dependency container</param>
        /// <param name="definition">The dependency info</param>
        protected abstract void OnBind(TContainer container, IDependencyInfo definition);

        #endregion Methods
    }


    /// <summary>
    /// Dependency scope binder that is used to define what to do with a <see
    /// cref="IDependencyBuilderInfo"/> and the scope binding
    /// </summary>
    public abstract class DependencyScopeBinderBase<TContainer, TInfo> : DependencyScopeBinderBase<TContainer>
        where TContainer : class
        where TInfo : IDependencyInfo
    {
        /// <inheritdoc/>
        protected DependencyScopeBinderBase(DependyBuilderScopes scopes) : base(scopes)
        {
        }

        /// <inheritdoc/>
        public override bool CanBind(IDependencyInfo definition)
        {
            return base.CanBind(definition) && definition is TInfo;
        }

        /// <inheritdoc/>
        protected override void OnBind(TContainer container, IDependencyInfo definition)
        {
            RegisterTypeTo(container, (TInfo)definition);
        }

        /// <summary>
        /// Resgiter the type to. This method can be overriden
        /// </summary>
        /// <param name="container">The dependency container</param>
        /// <param name="info">The dependency definition</param>
        /// <exception cref="NotSupportedException"></exception>
        protected virtual void RegisterTypeTo(TContainer container, TInfo info)
        {
            switch (info.Scope)
            {
                case DependyBuilderScopes.Transient:
                    OnRegisterTransient(container, info);
                    break;

                case DependyBuilderScopes.Singleton:
                    OnRegisterSingleton(container, info);
                    break;
                case DependyBuilderScopes.Request:
                    OnRegisterRequest(container, info);
                    break;
                case DependyBuilderScopes.Thread:
                    OnRegisterThread(container, info);
                    break;
                default:
                    throw new NotSupportedException($"{info.Scope} scope not currently supported for {info}.");
            }
        }

        /// <summary>
        /// Register a transient <typeparamref name="TInfo"/> instance with the configuration.
        /// Can be overridden.
        /// </summary>
        /// <param name="container">The dependency container</param>
        /// <param name="info">The dependency information describing the resolving requirements.</param>
        protected virtual void OnRegisterTransient(TContainer container, TInfo info)
        {
            throw new NotSupportedException($"{info.Scope} scope not currently supported for {info}.");
        }

        /// <summary>
        /// Register a singleton <typeparamref name="TInfo"/> instance with the configuration.
        /// Can be overridden.
        /// </summary>
        /// <param name="container">The dependency container</param>
        /// <param name="info">The dependency information describing the resolving requirements.</param>
        protected virtual void OnRegisterSingleton(TContainer container, TInfo info)
        {
            throw new NotSupportedException($"{info.Scope} scope not currently supported for {info}.");
        }

        /// <summary>
        /// Register a transient <typeparamref name="TInfo"/> instance with the configuration.
        /// Can be overridden.
        /// </summary>
        /// <param name="container">The dependency container</param>
        /// <param name="info">The dependency information describing the resolving requirements.</param>
        protected virtual void OnRegisterRequest(TContainer container, TInfo info)
        {
            throw new NotSupportedException($"{info.Scope} scope not currently supported for {info}.");
        }

        /// <summary>
        /// Register a singleton <typeparamref name="TInfo"/> instance with the configuration.
        /// Can be overridden.
        /// </summary>
        /// <param name="container">The dependency container</param>
        /// <param name="info">The dependency information describing the resolving requirements.</param>
        protected virtual void OnRegisterThread(TContainer container, TInfo info)
        {
            throw new NotSupportedException($"{info.Scope} scope not currently supported for {info}.");
        }
    }
}
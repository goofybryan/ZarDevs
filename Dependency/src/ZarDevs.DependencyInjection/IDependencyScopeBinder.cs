namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// Dependency scope binder that will bind the <see cref="IDependencyInfo"/> to the <typeparamref name="TContainer"/>.
    /// </summary>
    public interface IDependencyScopeBinder<TContainer> where TContainer : class
    {
        /// <summary>
        ///  The scope that the binder is valid for.
        /// </summary>
        DependyBuilderScopes Scopes { get; }

        /// <summary>
        /// Check that the definition is valid for the <paramref name="definition"/>.
        /// </summary>
        /// <param name="definition">The dependency definition.</param>
        /// <returns>A true if it can bind, false if it cannot.</returns>
        bool CanBind(IDependencyInfo definition);

        /// <summary>
        /// Bind the <paramref name="definition"/> to the <paramref name="container"/>.
        /// </summary>
        /// <param name="container">The dependency container to add the definition to.</param>
        /// <param name="definition">The definition the contains the bindings.</param>
        void Bind(TContainer container, IDependencyInfo definition);
    }
}
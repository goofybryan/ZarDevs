namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// Dependency scope compiler interface that is used to define what to do with a <see cref="IDependencyBuilderInfo"/> and the scope bindings
    /// </summary>
    /// <typeparam name="TContainer">The dependency container this compiler is used on.</typeparam>
    public interface IDependencyScopeCompiler<TContainer> where TContainer : class
    {
        /// <summary>
        /// Find the binder for the <paramref name="definition"/> to specified scope.
        /// </summary>
        /// <param name="definition">The dependency info that describes what is required.</param>
        IDependencyScopeBinder<TContainer> FindBinder(IDependencyInfo definition);
    }
}
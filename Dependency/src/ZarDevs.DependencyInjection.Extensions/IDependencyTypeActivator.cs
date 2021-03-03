namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// Dpendency type activator used to resolve the type and return the object for the defined dependency type.
    /// </summary>
    public interface IDependencyTypeActivator
    {
        #region Methods

        /// <summary>
        /// Resolve an instance based on the typed information.
        /// </summary>
        /// <param name="info">The dependency type information describing the <see cref="IDependencyInfo.RequestType"/>, <see cref="IDependencyInfo.Scope"/> and <see cref="IDependencyInfo.Key"/>.</param>
        /// <returns>An resolved object of the type <see cref="IDependencyTypeInfo.ResolvedType"/>.</returns>
        object Resolve(IDependencyTypeInfo info);

        /// <summary>
        /// Resolve an instance based on the typed information.
        /// </summary>
        /// <param name="info">The dependency type information describing the <see cref="IDependencyInfo.RequestType"/>, <see cref="IDependencyInfo.Scope"/> and <see cref="IDependencyInfo.Key"/>.</param>
        /// <param name="args">An list of args in order of the constructor.</param>
        /// <returns>An resolved object of the type <see cref="IDependencyTypeInfo.ResolvedType"/>.</returns>
        object Resolve(IDependencyTypeInfo info, params object[] args);

        /// <summary>
        /// Resolve an instance based on the typed information.
        /// </summary>
        /// <param name="info">The dependency type information describing the <see cref="IDependencyInfo.RequestType"/>, <see cref="IDependencyInfo.Scope"/> and <see cref="IDependencyInfo.Key"/>.</param>
        /// <param name="args">An list of named args of the constructor.</param>
        /// <returns>An resolved object of the type <see cref="IDependencyTypeInfo.ResolvedType"/>.</returns>
        object Resolve(IDependencyTypeInfo info, params (string, object)[] args);

        #endregion Methods
    }
}
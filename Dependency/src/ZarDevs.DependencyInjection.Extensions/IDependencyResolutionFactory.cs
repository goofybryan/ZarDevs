namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// Dependency resolution factory is used to create instances of <see cref="IDependencyResolution"/> base of the type of <see cref="IDependencyInfo"/>
    /// </summary>
    public interface IDependencyResolutionFactory
    {
        /// <summary>
        /// Create a resolution for <see cref="IDependencyTypeInfo"/>
        /// </summary>
        /// <param name="info">The dependency info</param>
        /// <returns>A resolution</returns>
        IDependencyResolution<IDependencyTypeInfo> ResolutionFor(IDependencyTypeInfo info);
        /// <summary>
        /// Create a resolution for <see cref="IDependencyTypeInfo"/>
        /// </summary>
        /// <param name="info">The dependency info</param>
        /// <returns>A resolution</returns>
        IDependencyResolution<IDependencyMethodInfo> ResolutionFor(IDependencyMethodInfo info);
        /// <summary>
        /// Create a resolution for <see cref="IDependencyMethodInfo"/>
        /// </summary>
        /// <param name="info">The dependency info</param>
        /// <returns>A resolution</returns>
        IDependencyResolution ResolutionFor(IDependencyInstanceInfo info);
        /// <summary>
        /// Create a resolution for <see cref="IDependencyFactoryInfo"/>
        /// </summary>
        /// <param name="info">The dependency info</param>
        /// <returns>A resolution</returns>
        IDependencyResolution<IDependencyFactoryInfo> ResolutionFor(IDependencyFactoryInfo info);
    }
}
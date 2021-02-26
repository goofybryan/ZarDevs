namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// Dependency instance information. When resolved, the specified instance will be returned.
    /// </summary>
    public interface IDependencyInstanceInfo : IDependencyInfo
    {
        /// <summary>
        /// Get the instance that the IOC will resolve from the <see cref="IDependencyInfo.RequestType"/>.
        /// </summary>
        object Instance { get; }
    }
}
namespace ZarDevs.DependencyInjection
{
    public interface IDependencyInstanceInfo : IDependencyInfo
    {
        /// <summary>
        /// Get the instance that the IOC will resolve from the <see cref="IDependencyInfo.RequestType"/>.
        /// </summary>
        object Instance { get; }
    }
}
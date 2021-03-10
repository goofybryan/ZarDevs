namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// Dependency builder scope.
    /// </summary>
    public enum DependyBuilderScope
    {
        /// <summary>
        /// Indicate that the request must be resolve a new instance with each call.
        /// </summary>
        Transient = 0,

        /// <summary>
        /// Indicate that the request must be resolve once and then returned with each call.
        /// </summary>
        Singleton
    }
}
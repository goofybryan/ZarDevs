using System;

namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// Dependency builder scope.
    /// </summary>
    [Flags]
    public enum DependyBuilderScopes
    {
        /// <summary>
        /// Indicate that the request must be resolve a new instance with each call.
        /// </summary>
        Transient = 1,

        /// <summary>
        /// Indicate that the request must be resolve once and then returned with each call.
        /// </summary>
        Singleton = 2,

        /// <summary>
        /// Indicate that the request must be resolve a new instance in the web request scope. Default fall back should be <see cref="Thread"/> when no web context is available.
        /// </summary>
        Request = 4,

        /// <summary>
        /// Indicate that the request must be resolve a new instance on each thread.
        /// </summary>
        Thread = 8
    }
}
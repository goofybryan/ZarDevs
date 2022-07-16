namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// <see cref="IIocContainer"/> extensions
    /// </summary>
    public static class IIocContainerExtensions
    {
        #region Methods

        /// <summary>
        /// Cast the current container to <see cref="IDependencyResolver"/>.
        /// </summary>
        /// <param name="container">The container to cast</param>
        /// <returns></returns>
        public static IDependencyResolver Resolver(this IIocContainer container)
        {
            return (IDependencyResolver)container;
        }

        #endregion Methods
    }
}
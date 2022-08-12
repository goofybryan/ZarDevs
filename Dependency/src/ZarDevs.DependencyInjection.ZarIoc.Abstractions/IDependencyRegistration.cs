namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// Interface that will be used by the generator to create the source.
    /// </summary>
    public interface IDependencyRegistration
    {
        /// <summary>
        /// Register the dependencies.
        /// </summary>
        /// <param name="builder">The depndency builder.</param>
        public void Register(IDependencyBuilder builder);
    }
}
namespace ZarDevs.DependencyInjection.ZarIoc
{
    /// <summary>
    /// Interface that will be used by the generator to create the source.
    /// </summary>
    public interface IDependencyRegistration
    {
        #region Methods

        /// <summary>
        /// Register the dependencies.
        /// </summary>
        /// <param name="builder">The depndency builder.</param>
        public void Register(IDependencyBuilder builder);

        #endregion Methods
    }
}
namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// Dependency Factory defitions
    /// </summary>
    public interface IDependencyFactoryDefinitions
    {
        #region Methods

        /// <summary>
        /// Add a factory dependency to the definitions.
        /// </summary>
        /// <param name="info"></param>
        void AddFactoryInfo(IDependencyFactoryInfo info);

        #endregion Methods
    }
}
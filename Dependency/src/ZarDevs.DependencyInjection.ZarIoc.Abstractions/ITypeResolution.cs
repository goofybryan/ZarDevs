namespace ZarDevs.DependencyInjection.ZarIoc
{
    /// <summary>
    /// Type resolution interface that will be used to wrap the generated implementations.
    /// </summary>
    public interface ITypeResolution
    {
        #region Properties

        /// <summary>
        /// Get the dependency definition.
        /// </summary>
        IDependencyInfo Info { get; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Resolve it without any parameters
        /// </summary>
        /// <returns>The resolution.</returns>
        object Resolve();

        /// <summary>
        /// Resolve it with parameters
        /// </summary>
        /// <returns>The resolution.</returns>
        object Resolve(params object[] parameters);

        /// <summary>
        /// Resolve it with parameters
        /// </summary>
        /// <returns>The resolution.</returns>
        object Resolve(params (string key, object value)[] parameters);

        #endregion Methods
    }
}
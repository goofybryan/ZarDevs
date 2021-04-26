namespace ZarDevs.Http.Security
{
    /// <summary>
    /// Security configuration used
    /// </summary>
    public interface ISecurityConfiguration
    {
        #region Methods

        /// <summary>
        /// Get the specified <typeparamref name="TValue"/> value from the configuration
        /// </summary>
        /// <typeparam name="TValue">The value type that needs to be returned.</typeparam>
        /// <param name="name">Specify the name of the configuration to get.</param>
        /// <param name="value">The value is returned if the value exists for the configuration name.</param>
        /// <returns>Returns a true if the value exists and is returned.</returns>
        bool TryGet<TValue>(string name, out TValue value);

        /// <summary>
        /// Update the configuration value.
        /// </summary>
        /// <typeparam name="TValue">The value type that needs to be returned.</typeparam>
        /// <param name="name">Specify the name of the configuration to update.</param>
        /// <param name="value">The value to update.</param>
        void Update<TValue>(string name, TValue value);

        #endregion Methods
    }
}
namespace ZarDevs.DependencyInjection.ZarIoc
{
    /// <summary>
    /// Resolution mapper that will try and map a <see cref="IDependencyInfo"/> to a <see cref="IDependencyResolution"/>
    /// </summary>
    public interface IResolutionMapper
    {
        #region Methods

        /// <summary>
        /// Try and map <paramref name="definition"/> and create a <paramref name="resolution"/>.
        /// </summary>
        /// <param name="definition">The definition to map.</param>
        /// <param name="resolution">Return a <see cref="IDependencyResolution"/> if it can be mapped.</param>
        /// <returns>Returns true if it cam be mapped</returns>
        bool TryMap(IDependencyInfo definition, out IDependencyResolution resolution);

        #endregion Methods
    }
}
namespace ZarDevs.DependencyInjection.ZarIoc
{
    /// <summary>
    /// Default mapper that can be used to map <see cref="IDependencyInstanceInfo"/> and <seealso cref="IDependencyMethodInfo"/>
    /// </summary>
    public class ResolutionDefaultMapper : IResolutionMapper
    {
        #region Methods

        /// <inheritdoc/>
        public bool TryMap(IDependencyInfo definition, out ITypeResolution resolution)
        {
            if (definition is IDependencyInstanceInfo instanceInfo)
            {
                resolution = new InstanceResolution(instanceInfo);
                return true;
            }

            if (definition is IDependencyMethodInfo functionInfo)
            {
                resolution = new FunctionResolution(functionInfo);
                return true;
            }

            resolution = null;
            return false;
        }

        #endregion Methods
    }
}
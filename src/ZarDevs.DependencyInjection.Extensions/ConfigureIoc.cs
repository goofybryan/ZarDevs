namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// Configure the extentions bindings.
    /// </summary>
    public static class ConfigureIoc
    {
        #region Methods

        /// <summary>
        /// Configure the extentions required by the IOC.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IDependencyBuilder ConfigureExtentions(this IDependencyBuilder builder)
        {
            builder.Bind<IDependencyResolver>().To<DependencyResolver>().InSingletonScope();

            return builder;
        }

        #endregion Methods
    }
}
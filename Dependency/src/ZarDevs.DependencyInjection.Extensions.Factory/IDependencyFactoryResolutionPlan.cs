namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// Dependency resolution factory plan.
    /// </summary>
    public interface IDependencyFactoryResolutionPlan
    {
        /// <summary>
        /// Resolve the factory method.
        /// </summary>
        /// <param name="factory">The factory object that needs to be resolved.</param>
        /// <param name="context">The dependency context.</param>
        /// <returns></returns>
        object Resolve(object factory, IDependencyContext context);
    }
}
using Autofac;
using Autofac.Builder;
using ZarDevs.Runtime;

namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// AutoFac initialization
    /// </summary>
    public static class IocAutoFac
    {
        #region Methods

        /// <summary>
        /// Create a new IOC Kernel container that will start the binding process.
        /// </summary>
        /// <param name="buildOptions">The container build options.</param>
        /// <param name="additionalBinders">Any additional binders that can do additional resoultions. These are added first before <see cref="AutoFacDependencyScopeBinder"/> is added.</param>
        /// <returns></returns>
        public static IIocKernelBuilder Initialize(ContainerBuildOptions buildOptions = ContainerBuildOptions.None, params IDependencyScopeBinder<ContainerBuilder>[] additionalBinders)
        {
            return new IocKernelBuilder(buildOptions, new DependencyFactory(InspectMethod.Instance), additionalBinders);
        }

        #endregion Methods
    }
}
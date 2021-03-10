using System;
using System.Reflection;

namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// Dependency method information, when resolved the method will be invoked.
    /// </summary>
    public interface IDependencyMethodInfo : IDependencyInfo
    {
        #region Properties

        /// <summary>
        /// Execute the method that has been configured
        /// </summary>
        /// <param name="context">The dependency context</param>
        /// <returns></returns>
        object Execute(IDependencyContext context);

        #endregion Properties
    }
}
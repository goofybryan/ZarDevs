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
        /// <returns></returns>
        object Execute();

        /// <summary>
        /// Execute the method that has been configured
        /// </summary>
        /// <returns></returns>
        object Execute(object[] args);

        /// <summary>
        /// Execute the method that has been configured
        /// </summary>
        /// <returns></returns>
        object Execute((string, object)[] args);

        #endregion Properties
    }
}
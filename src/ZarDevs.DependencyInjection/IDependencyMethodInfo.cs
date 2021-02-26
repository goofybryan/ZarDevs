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
        /// The method to invoke.
        /// </summary>
        Func<DepencyBuilderInfoContext, object, object> Method { get; }

        #endregion Properties
    }
}
using System;

namespace ZarDevs.DependencyInjection
{
    public interface IDependencyTypeInfo : IDependencyInfo
    {
        #region Properties

        /// <summary>
        /// Get the resolved type that the IOC will resolved from the <see cref="IDependencyInfo.RequestType"/>.
        /// </summary>
        Type ResolvedType { get; }

        #endregion Properties
    }
}
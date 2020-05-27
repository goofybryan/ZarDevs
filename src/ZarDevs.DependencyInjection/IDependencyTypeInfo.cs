using System;

namespace ZarDevs.DependencyInjection
{
    public interface IDependencyTypeInfo : IDependencyInfo
    {
        #region Properties

        Type TypeTo { get; }

        #endregion Properties
    }
}
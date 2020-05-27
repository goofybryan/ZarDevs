using System;

namespace ZarDevs.DependencyInjection
{
    public interface IDependencyMethodInfo : IDependencyInfo
    {
        #region Properties

        Func<DepencyBuilderInfoContext, string, object> MethodTo { get; }

        #endregion Properties
    }
}
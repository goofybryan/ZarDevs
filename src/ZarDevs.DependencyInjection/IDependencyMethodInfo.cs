using System;
using System.Reflection;

namespace ZarDevs.DependencyInjection
{
    public interface IDependencyMethodInfo : IDependencyInfo
    {
        #region Properties

        Func<DepencyBuilderInfoContext, object, object> MethodTo { get; }

        #endregion Properties
    }
}
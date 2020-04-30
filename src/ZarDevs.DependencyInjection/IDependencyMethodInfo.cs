using System;

namespace ZarDevs.DependencyInjection
{
    public interface IDependencyMethodInfo : IDependencyInfo
    {
        Func<DepencyBuilderInfoContext, string, object> MethodTo { get; }
    }
}
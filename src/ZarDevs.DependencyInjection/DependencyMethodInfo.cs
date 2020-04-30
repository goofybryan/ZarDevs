using System;
using ZarDevs.Core;

namespace ZarDevs.DependencyInjection
{
    internal class DependencyMethodInfo : DependencyInfo, IDependencyMethodInfo
    {
        #region Constructors

        public DependencyMethodInfo(Func<DepencyBuilderInfoContext, string, object> methodTo, DependencyInfo info) : base(info)
        {
            MethodTo = Check.IsNotNull(methodTo, nameof(methodTo));
        }

        #endregion Constructors

        #region Properties

        public Func<DepencyBuilderInfoContext, string, object> MethodTo { get; }

        #endregion Properties
    }
}
using System;

namespace ZarDevs.DependencyInjection
{
    internal class DependencyMethodInfo : DependencyInfo, IDependencyMethodInfo
    {
        #region Constructors

        public DependencyMethodInfo(Func<DepencyBuilderInfoContext, object, object> methodTo, DependencyInfo info) : base(info)
        {
            MethodTo = methodTo ?? throw new ArgumentNullException(nameof(methodTo));
        }

        #endregion Constructors

        #region Properties

        public Func<DepencyBuilderInfoContext, object, object> MethodTo { get; }

        #endregion Properties
    }
}
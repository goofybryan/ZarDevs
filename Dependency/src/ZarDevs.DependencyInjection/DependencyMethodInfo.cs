using System;

namespace ZarDevs.DependencyInjection
{
    internal class DependencyMethodInfo : DependencyInfo, IDependencyMethodInfo
    {
        #region Constructors

        public DependencyMethodInfo(Func<IDependencyContext, object> methodTo, DependencyInfo info) : base(info)
        {
            Method = methodTo ?? throw new ArgumentNullException(nameof(methodTo));
        }

        #endregion Constructors

        #region Properties

        public Func<IDependencyContext, object> Method { get; }

        public object Execute(IDependencyContext context)
        {
            return Method(context);
        }

        #endregion Properties
    }
}
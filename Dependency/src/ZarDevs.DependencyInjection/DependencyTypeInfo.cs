using System;

namespace ZarDevs.DependencyInjection
{
    internal class DependencyTypeInfo : DependencyInfo, IDependencyTypeInfo
    {
        #region Constructors

        public DependencyTypeInfo(Type typeTo, IDependencyInfo info) : base(info)
        {
            ResolutionType = typeTo ?? throw new ArgumentNullException(nameof(typeTo));
        }

        #endregion Constructors

        #region Properties

        public Type ResolutionType { get; set; }

        #endregion Properties
    }
}
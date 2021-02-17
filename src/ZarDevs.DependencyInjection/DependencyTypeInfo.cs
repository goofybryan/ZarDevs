using System;

namespace ZarDevs.DependencyInjection
{
    internal class DependencyTypeInfo : DependencyInfo, IDependencyTypeInfo
    {
        #region Constructors

        public DependencyTypeInfo(Type typeTo, DependencyInfo info) : base(info)
        {
            ResolvedType = typeTo ?? throw new ArgumentNullException(nameof(typeTo));
        }

        #endregion Constructors

        #region Properties

        public Type ResolvedType { get; set; }

        #endregion Properties
    }
}
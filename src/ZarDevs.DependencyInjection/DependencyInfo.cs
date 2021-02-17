using System;

namespace ZarDevs.DependencyInjection
{
    internal class DependencyInfo : IDependencyInfo
    {
        #region Constructors

        public DependencyInfo()
        {
        }

        public DependencyInfo(DependencyInfo copy)
        {
            Name = copy.Name;
            Scope = copy.Scope;
            RequestType = copy.RequestType;
        }

        #endregion Constructors

        #region Properties

        public string Name { get; set; } = "";
        public DependyBuilderScope Scope { get; set; }
        public Type RequestType { get; set; }

        #endregion Properties
    }
}
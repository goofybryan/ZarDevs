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
            Key = copy.Key;
            Scope = copy.Scope;
            RequestType = copy.RequestType;
        }

        #endregion Constructors

        #region Properties

        public object Key { get; set; } = "";
        public DependyBuilderScope Scope { get; set; }
        public Type RequestType { get; set; }

        #endregion Properties

        public override string ToString()
        {
            return $"Dependency Info: Key={Key}, Scope={Scope}, RequestType={RequestType}";
        }
    }
}
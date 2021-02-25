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

        public Type[] ConstructorArgumentTypes { get; set; }

        public bool HasConstructor { get; set; }

        public IDependencyTypeInfo WithConstructor(params Type[] constructorArgs)
        {
            if (constructorArgs is not null && constructorArgs.Length > 0)
            {
                HasConstructor = true;
                ConstructorArgumentTypes = constructorArgs;
            }

            return this;
        }

        #endregion Properties
    }
}
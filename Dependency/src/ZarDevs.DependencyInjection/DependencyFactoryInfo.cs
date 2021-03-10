using System;

namespace ZarDevs.DependencyInjection
{
    internal class DependencyFactoryInfo : DependencyInfo, IDependencyFactoryInfo
    {
        #region Constructors

        public DependencyFactoryInfo(Type factoryType, string methodName, IDependencyInfo info) : base(info)
        {
            if (string.IsNullOrWhiteSpace(methodName))
            {
                throw new ArgumentException($"'{nameof(methodName)}' cannot be null or whitespace.", nameof(methodName));
            }

            FactoryType = factoryType ?? throw new ArgumentNullException(nameof(factoryType));
            MethodName = methodName;
        }

        #endregion Constructors

        #region Properties

        public Type FactoryType { get; }

        public string MethodName { get; }

        #endregion Properties
    }
}
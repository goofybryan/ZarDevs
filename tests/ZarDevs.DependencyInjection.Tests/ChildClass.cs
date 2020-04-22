using System;

namespace ZarDevs.DependencyInjection.Tests
{
    public interface IChildClass
    {
        #region Properties

        Type CallingClassType { get; }

        #endregion Properties
    }

    internal class ChildClass : IChildClass
    {
        #region Constructors

        public ChildClass(Type callingClassType)
        {
            CallingClassType = callingClassType ?? throw new System.ArgumentNullException(nameof(callingClassType));
        }

        #endregion Constructors

        #region Properties

        public Type CallingClassType { get; }

        #endregion Properties
    }
}
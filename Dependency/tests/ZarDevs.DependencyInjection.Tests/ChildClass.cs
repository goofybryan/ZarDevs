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

        public ChildClass()
        {
            CallingClassType = typeof(ChildClass);
        }

        #endregion Constructors

        #region Properties

        public Type CallingClassType { get; set; }

        #endregion Properties
    }
}
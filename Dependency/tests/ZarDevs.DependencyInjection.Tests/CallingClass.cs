using System;

namespace ZarDevs.DependencyInjection.Tests
{
    public interface ICallingClass
    {
        #region Methods

        IChildClass Call();

        #endregion Methods
    }

    internal class CallingClass : ICallingClass
    {
        #region Fields

        private readonly IChildClass _childClass;

        #endregion Fields

        #region Constructors

        public CallingClass(IChildClass childClass)
        {
            _childClass = childClass ?? throw new ArgumentNullException(nameof(childClass));
        }

        #endregion Constructors

        #region Methods

        public IChildClass Call()
        {
            return _childClass;
        }

        #endregion Methods
    }
}
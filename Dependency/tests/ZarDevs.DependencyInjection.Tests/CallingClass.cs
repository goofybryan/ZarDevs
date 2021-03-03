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
        private readonly IChildClass _childClass;

        public CallingClass(IChildClass childClass)
        {
            _childClass = childClass ?? throw new ArgumentNullException(nameof(childClass));
        }

        #region Methods

        public IChildClass Call()
        {
            return _childClass;
        }

        #endregion Methods
    }
}
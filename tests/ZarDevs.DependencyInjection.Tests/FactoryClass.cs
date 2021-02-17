using System;

namespace ZarDevs.DependencyInjection.Tests
{
    public interface IFactoryClass
    {
        #region Methods

        IChildClass ResolveChildClass(Type parameter);

        #endregion Methods
    }

    internal class FactoryClass : IFactoryClass
    {
        #region Properties

        public static IFactoryClass Instance { get; } = new FactoryClass();

        #endregion Properties

        #region Methods

        public IChildClass ResolveChildClass(Type parameter)
        {
            return Ioc.Resolve<IChildClass>(new System.Collections.Generic.KeyValuePair<string, object>("callingClassType", parameter));
        }

        #endregion Methods
    }
}
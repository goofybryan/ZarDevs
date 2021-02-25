using System;

namespace ZarDevs.DependencyInjection.Tests
{
    public interface IFactoryClass
    {
        #region Methods

        IMultipleConstructorClass ResolveMultipleConstructorClass();

        IMultipleConstructorClass ResolveMultipleConstructorClass(object[] constructorArgs);

        #endregion Methods
    }

    internal class FactoryClass : IFactoryClass
    {
        #region Constructors

        public FactoryClass(IIocContainer ioc)
        {
            Ioc = ioc ?? throw new ArgumentNullException(nameof(ioc));
        }

        #endregion Constructors

        #region Properties

        public IIocContainer Ioc { get; }

        #endregion Properties

        #region Methods

        public IMultipleConstructorClass ResolveMultipleConstructorClass(object[] constructorArgs)
        {
            return Ioc.ResolveNamed<IMultipleConstructorClass>(Bindings.NotMethod, constructorArgs);
        }

        public IMultipleConstructorClass ResolveMultipleConstructorClass()
        {
            return Ioc.ResolveNamed<IMultipleConstructorClass>(Bindings.NotMethod);
        }

        #endregion Methods
    }
}
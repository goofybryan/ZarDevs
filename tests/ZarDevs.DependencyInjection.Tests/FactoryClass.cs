using System;

namespace ZarDevs.DependencyInjection.Tests
{
    public interface IFactoryClass
    {
        #region Methods

        IFactoryResolutionClass ResolveFactoryResolutionClass();

        IFactoryResolutionClass ResolveFactoryResolutionClass(object[] constructorArgs);

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

        public IFactoryResolutionClass ResolveFactoryResolutionClass(object[] constructorArgs)
        {
            return Ioc.ResolveNamed<IFactoryResolutionClass>(Bindings.NotMethod, constructorArgs);
        }

        public IFactoryResolutionClass ResolveFactoryResolutionClass()
        {
            return Ioc.ResolveNamed<IFactoryResolutionClass>(Bindings.NotMethod);
        }

        #endregion Methods
    }
}
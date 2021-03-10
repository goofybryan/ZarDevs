using Microsoft.Extensions.DependencyInjection;
using System;
using ZarDevs.Runtime;

namespace ZarDevs.DependencyInjection
{
    internal class MicrosoftUtilitiesActivator : IDependencyTypeActivator
    {
        #region Fields

        private readonly IInspectConstructor _inspectConstructor;

        #endregion Fields

        #region Constructors

        public MicrosoftUtilitiesActivator(IInspectConstructor inspectConstructor)
        {
            _inspectConstructor = inspectConstructor ?? throw new ArgumentNullException(nameof(inspectConstructor));
        }

        #endregion Constructors

        #region Properties

        private IIocContainer Ioc => DependencyInjection.Ioc.Container;

        #endregion Properties

        #region Methods

        public object Resolve(IDependencyTypeInfo info, params object[] args)
        {
            if (args == null || args.Length == 0)
                return Resolve(info);

            return ActivatorUtilities.CreateInstance(Ioc.Resolve<IServiceProvider>(), info.ResolvedType, args);
        }

        public object Resolve(IDependencyTypeInfo info, params (string, object)[] args)
        {
            if (args == null || args.Length == 0)
                return Resolve(info);

            var orderedParameters = _inspectConstructor.OrderParameters(info.ResolvedType, args);

            return ActivatorUtilities.CreateInstance(Ioc.Resolve<IServiceProvider>(), info.ResolvedType, orderedParameters);
        }

        public object Resolve(IDependencyTypeInfo info)
        {
            return ActivatorUtilities.CreateInstance(Ioc.Resolve<IServiceProvider>(), info.ResolvedType);
        }

        #endregion Methods
    }
}
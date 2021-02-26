using Microsoft.Extensions.DependencyInjection;
using System;
using ZarDevs.Runtime;

namespace ZarDevs.DependencyInjection
{
    internal class MicrosoftUtilitiesActivator : IDependencyTypeActivator
    {
        #region Fields

        private readonly IInspect _inspectConstructor;

        #endregion Fields

        #region Constructors

        public MicrosoftUtilitiesActivator(IInspect inspectConstructor)
        {
            _inspectConstructor = inspectConstructor ?? throw new ArgumentNullException(nameof(inspectConstructor));
        }

        #endregion Constructors

        #region Methods

        public object Resolve(IIocContainer ioc, IDependencyTypeInfo info, params object[] args)
        {
            if (args == null || args.Length == 0)
                return Resolve(ioc, info);

            return ActivatorUtilities.CreateInstance(ioc.Resolve<IServiceProvider>(), info.ResolvedType, args);
        }

        public object Resolve(IIocContainer ioc, IDependencyTypeInfo info, params (string, object)[] args)
        {
            if (args == null || args.Length == 0)
                return Resolve(ioc, info);

            var orderedParameters = _inspectConstructor.OrderParameters(info.ResolvedType, args);

            return ActivatorUtilities.CreateInstance(ioc.Resolve<IServiceProvider>(), info.ResolvedType, orderedParameters);
        }

        public object Resolve(IIocContainer ioc, IDependencyTypeInfo info)
        {
            return ActivatorUtilities.GetServiceOrCreateInstance(ioc.Resolve<IServiceProvider>(), info.RequestType);
        }

        #endregion Methods
    }
}
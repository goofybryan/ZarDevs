using Microsoft.Extensions.DependencyInjection;
using System;
using ZarDevs.Runtime;

namespace ZarDevs.DependencyInjection
{
    public class DependencyTypeResolution : DependencyResolution<IDependencyTypeInfo>
    {
        #region Constructors

        public DependencyTypeResolution(IDependencyTypeInfo info) : base(info)
        {
        }

        #endregion Constructors

        #region Methods

        public override object Resolve(IIocContainer ioc, params object[] args)
        {
            var serviceProvider = ioc.Resolve<IServiceProvider>();

            if (args == null || args.Length == 0)
                return ActivatorUtilities.GetServiceOrCreateInstance(serviceProvider, Info.RequestType);

            return ActivatorUtilities.CreateInstance(serviceProvider, Info.ResolvedType, args);
        }

        public override object Resolve(IIocContainer ioc, params (string, object)[] args)
        {
            var serviceProvider = ioc.Resolve<IServiceProvider>();

            if (args == null || args.Length == 0)
                return ActivatorUtilities.GetServiceOrCreateInstance(serviceProvider, Info.RequestType);

            var orderedParameters = InspectConstructor.Instance.OrderParameters(Info.ResolvedType, args);
            return ActivatorUtilities.CreateInstance(serviceProvider, Info.ResolvedType, orderedParameters);
        }

        #endregion Methods
    }
}
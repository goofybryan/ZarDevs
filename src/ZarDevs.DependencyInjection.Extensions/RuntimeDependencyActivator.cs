using System;
using System.Collections.Generic;
using System.Linq;
using ZarDevs.Runtime;

namespace ZarDevs.DependencyInjection
{
    public class RuntimeDependencyActivator : IDependencyTypeActivator
    {
        #region Fields

        private readonly ICreate _creation;
        private readonly IInspectConstructor _inspection;

        #endregion Fields

        #region Constructors

        public RuntimeDependencyActivator(IInspectConstructor inspection, ICreate creation)
        {
            _inspection = inspection ?? throw new System.ArgumentNullException(nameof(inspection));
            _creation = creation ?? throw new System.ArgumentNullException(nameof(creation));
        }

        #endregion Constructors

        #region Methods

        public object Resolve(IIocContainer ioc, IDependencyTypeInfo info, params object[] args)
        {
            return _creation.New(info.ResolvedType, args);
        }

        public object Resolve(IIocContainer ioc, IDependencyTypeInfo info, params (string, object)[] args)
        {
            return Resolve(ioc, info, _inspection.OrderParameters(info.ResolvedType, args));
        }

        public object Resolve(IIocContainer ioc, IDependencyTypeInfo info)
        {
            return Resolve(ioc, info, ResolveArgs(ioc, info.ResolvedType));
        }

        private object[] ResolveArgs(IIocContainer ioc, Type instanceType)
        {
            IList<Type> orderedParameters = _inspection.GetConstructorParameters(instanceType);

            if (orderedParameters.Count == 0)
                return null;

            return orderedParameters.Select(p => ioc.TryResolve(p)).ToArray();
        }

        #endregion Methods
    }
}
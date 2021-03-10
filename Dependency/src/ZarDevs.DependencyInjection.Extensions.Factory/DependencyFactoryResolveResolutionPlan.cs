using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace ZarDevs.DependencyInjection
{
    internal class DependencyFactoryResolveResolutionPlan : IDependencyFactoryResolutionPlan
    {
        #region Fields

        private readonly MethodInfo _method;
        private readonly IList<Type> _parameterTypes;

        #endregion Fields

        #region Constructors

        public DependencyFactoryResolveResolutionPlan(MethodInfo method, IList<Type> parameterTypes)
        {
            _method = method ?? throw new ArgumentNullException(nameof(method));
            _parameterTypes = parameterTypes ?? new Type[0];
        }

        #endregion Constructors

        #region Methods

        public object Resolve(object factory, IDependencyContext context)
        {
            if (factory is null)
            {
                throw new ArgumentNullException(nameof(factory));
            }

            IIocContainer ioc = context.Ioc;

            if (_parameterTypes.Count == 0)
                return _method.Invoke(factory, null);

            object[] parameters = new object[_parameterTypes.Count];

            for (int i = 0; i < _parameterTypes.Count; i++)
            {
                var requestType = _parameterTypes[i];
                object resolved;
                if (requestType.IsArray)
                    resolved = ioc.ResolveAll(requestType.GetElementType());
                else if (typeof(IEnumerable).IsAssignableFrom(requestType) && requestType.GenericTypeArguments.Length > 0)
                    resolved = ioc.ResolveAll(requestType.GenericTypeArguments[0]);
                else
                    resolved = ioc.TryResolve(requestType);

                parameters[i] = resolved;
            }

            return _method.Invoke(factory, parameters);
        }

        #endregion Methods
    }
}
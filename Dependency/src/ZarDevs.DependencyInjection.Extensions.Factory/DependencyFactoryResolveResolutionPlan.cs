using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
            _parameterTypes = parameterTypes ?? Array.Empty<Type>();
        }

        #endregion Constructors

        #region Methods

        public Expression<Func<object, IDependencyContext, object>> GetExpression()
        {
            var factoryObjectExpression = Expression.Parameter(typeof(object), "factory");
            var contextExpression = Expression.Parameter(typeof(IDependencyContext), "ctx");

            var castFactory = Expression.Convert(factoryObjectExpression, _method.DeclaringType);
            var iocExpression = Expression.PropertyOrField(contextExpression, nameof(IDependencyContext.Ioc));

            var resolver = Expression.Call(castFactory, _method, CreateVariablesExpression(iocExpression).ToArray());

            return Expression.Lambda<Func<object, IDependencyContext, object>>(resolver, factoryObjectExpression, contextExpression);
        }

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

        private static Expression ResolveAllArrayExpression(Expression iocExpression, Type typeToResolve)
        {
            return Expression.Call(typeof(Enumerable), nameof(Enumerable.ToArray), new[] { typeToResolve }, Expression.Call(iocExpression, nameof(IIocContainer.ResolveAll), new[] { typeToResolve }));
        }

        private static Expression ResolveAllExpression(Expression iocExpression, Type returnType, Type typeToResolve)
        {
            return Expression.Convert(Expression.Call(iocExpression, nameof(IIocContainer.ResolveAll), new[] { typeToResolve }), returnType);
        }

        private static Expression TryResolveExpression(Expression iocExpression, Type typeToResolve)
        {
            return Expression.Call(iocExpression, nameof(IIocContainer.TryResolve), new[] { typeToResolve });
        }

        private IEnumerable<Expression> CreateVariablesExpression(Expression iocExpression)
        {
            for (int i = 0; i < _parameterTypes.Count; i++)
            {
                var requestType = _parameterTypes[i];
                if (requestType.IsArray)
                    yield return ResolveAllArrayExpression(iocExpression, requestType.GetElementType());
                else if (typeof(IEnumerable).IsAssignableFrom(requestType) && requestType.GenericTypeArguments.Length > 0)
                    yield return ResolveAllExpression(iocExpression, requestType, requestType.GenericTypeArguments[0]);
                else
                    yield return TryResolveExpression(iocExpression, requestType);
            }
        }

        #endregion Methods
    }
}
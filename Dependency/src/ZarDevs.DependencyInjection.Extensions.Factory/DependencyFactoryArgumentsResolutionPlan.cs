using System;
using ZarDevs.Runtime;

namespace ZarDevs.DependencyInjection
{
    internal class DependencyFactoryArgumentsResolutionPlan : IDependencyFactoryResolutionPlan
    {
        private readonly Type _factoryType;
        private readonly string _methodName;
        private readonly IInspectMethod _inspectMethod;

        public DependencyFactoryArgumentsResolutionPlan(Type factoryType, string methodName, IInspectMethod inspectMethod)
        {
            if (string.IsNullOrWhiteSpace(methodName))
            {
                throw new ArgumentException($"'{nameof(methodName)}' cannot be null or whitespace.", nameof(methodName));
            }

            _factoryType = factoryType ?? throw new ArgumentNullException(nameof(factoryType));
            _methodName = methodName;
            _inspectMethod = inspectMethod ?? throw new ArgumentNullException(nameof(inspectMethod));
        }

        public object Resolve(object factory, IDependencyContext context)
        {
            var args = context.GetArguments();
            var method = _inspectMethod.FindMethodForArguments(_factoryType, _methodName, args);
            return method.Invoke(factory, args);
        }
    }
}
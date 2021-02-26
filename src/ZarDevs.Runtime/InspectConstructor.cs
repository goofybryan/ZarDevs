using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ZarDevs.Runtime
{
    public interface IInspectConstructor : IInspect
    {
        IList<Type> GetConstructorParameters(Type target);
    }

    public class InspectConstructor : IInspectConstructor
    {
        #region Properties

        /// <summary>
        /// Get and instance of the Inpect class.
        /// </summary>
        public static IInspectConstructor Instance { get; set; } = new InspectConstructor();

        #endregion Properties

        #region Methods

        public (string, object)[] FindParameterNames(Type target, IList<object> orderedValues)
        {
            var constructors = GetConstructorInfos(target);

            foreach (var constructor in constructors)
            {
                if (TryGetConstructorArgs(constructor, orderedValues, out (string, object)[] arguments)) return arguments;
            }

            throw new InvalidOperationException($"The is no constructors with constructor argument count as the requested count or matches the object types in order.");
        }

        public IList<Type> GetConstructorParameters(Type target)
        {
            var constructors = GetConstructorInfos(target).Select(info => info.GetParameters()).OrderBy(args => args.Length);

            return constructors.First().Select(args => args.ParameterType).ToArray();
        }

        public object[] OrderParameters(Type target, IDictionary<string, object> unorderedValueMapping)
        {
            var constructors = target.GetConstructors(BindingFlags.Public | BindingFlags.Instance);

            foreach (var constructor in constructors)
            {
                var parameters = constructor.GetParameters();

                if (parameters.Length != unorderedValueMapping.Count) continue;

                IList<object> orderedList = new List<object>();
                foreach (var parameter in parameters)
                {
                    if (!unorderedValueMapping.TryGetValue(parameter.Name, out object value))
                    {
                        orderedList = null;
                        break;
                    }

                    orderedList.Add(value);
                }

                if (orderedList != null && orderedList.Count == unorderedValueMapping.Count) return orderedList.ToArray();
            }

            throw new InvalidOperationException($"The is no constructors with constructor argument count as the requested count or matches the object types.");
        }

        public object[] OrderParameters(Type target, IList<(string, object)> unorderedValueMapping)
        {
            return OrderParameters(target, unorderedValueMapping.ToDictionary(key => key.Item1, value => value.Item2));
        }

        private bool TryGetConstructorArgs(ConstructorInfo info, IList<object> argumentsTypesInOrder, out (string, object)[] arguments)
        {
            arguments = null;
            var parameters = info.GetParameters();

            if (parameters.Length != argumentsTypesInOrder.Count) return false;

            var internalArguments = new List<(string, object)>();

            for (int i = 0; i < parameters.Length; i++)
            {
                var value = argumentsTypesInOrder[i];
                ParameterInfo parameter = parameters[i];
                Type argType = value?.GetType();

                if (argType != null && !parameter.ParameterType.IsAssignableFrom(argType)) return false;

                internalArguments.Add(ValueTuple.Create(parameter.Name, value));
            }

            arguments = internalArguments.ToArray();

            return true;
        }

        private IList<ConstructorInfo> GetConstructorInfos(Type target)
        {
            return target.GetConstructors(BindingFlags.Public | BindingFlags.Instance);
        }

        #endregion Methods
    }
}
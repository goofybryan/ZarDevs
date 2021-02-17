using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ZarDevs.Runtime
{
    /// <summary>
    /// Inspect is used to inpect a type using refelection.
    /// </summary>
    public interface IInspect
    {
        #region Methods

        /// <summary>
        /// Find the constructor arguments for a list of ordered parameters.
        /// </summary>
        /// <param name="target">The target object type.</param>
        /// <param name="argumentValuesInOrder"></param>
        /// <returns>Returns a list of constructor parameter names and associated values.</returns>
        IList<(string, object)> FindConstructParameterNames(Type target, IList<object> argumentValuesInOrder);

        /// <summary>
        /// Finds a constructor with the same parameters and returns the ordered list of objects.
        /// </summary>
        /// <param name="target">The target object type.</param>
        /// <param name="unorderedValueMapping">The unordered list of constructor paramaters and the associated name.</param>
        /// <returns>The ordered list of objects from the mapping.</returns>
        IList<object> OrderConstructorParameters(Type target, IDictionary<string, object> unorderedValueMapping);

        #endregion Methods
    }

    public class Inspect : IInspect
    {
        #region Methods

        /// <summary>
        /// Get and  instance of the Inpect class.
        /// </summary>
        public static IInspect Instance { get; set; } = new Inspect();

        public IList<(string, object)> FindConstructParameterNames(Type target, IList<object> orderedValues)
        {
            var constructors = target.GetConstructors();

            foreach (var constructor in constructors)
            {
                if (TryGetConstructorArgs(constructor, orderedValues, out IList<(string, object)> arguments)) return arguments;
            }

            throw new InvalidOperationException($"The is no constructors with constructor argument count as the requested count or matches the object types in order.");
        }

        public IList<object> OrderConstructorParameters(Type target, IDictionary<string, object> unorderedValueMapping)
        {
            var constructors = target.GetConstructors();

            foreach(var constructor in constructors)
            {
                var parameters = constructor.GetParameters();

                if (parameters.Length != unorderedValueMapping.Count) continue;

                IList<object> orderedList = new List<object>();
                foreach (var parameter in parameters)
                {
                    if(!unorderedValueMapping.TryGetValue(parameter.Name, out object value))
                    {
                        orderedList = null;
                        break;
                    }

                    orderedList.Add(value);
                }

                if (orderedList != null && orderedList.Count == unorderedValueMapping.Count) return orderedList;
            }

            throw new InvalidOperationException($"The is no constructors with constructor argument count as the requested count or matches the object types.");
        }

        private bool TryGetConstructorArgs(ConstructorInfo info, IList<object> argumentsTypesInOrder, out IList<(string, object)> arguments)
        {
            arguments = null;
            var parameters = info.GetParameters();

            var internalArguments = new List<(string, object)>();

            if (parameters.Length != argumentsTypesInOrder.Count) return false;

            for (int i = 0; i < parameters.Length; i++)
            {
                var value = argumentsTypesInOrder[i];
                ParameterInfo parameter = parameters[i];
                Type argType = value?.GetType();

                if (argType != null && !parameter.ParameterType.IsAssignableFrom(argType)) return false;

                internalArguments.Add(ValueTuple.Create(parameter.Name, value)) ;
            }

            arguments = internalArguments;

            return true;
        }

        #endregion Methods
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ZarDevs.Runtime
{
    /// <summary>
    /// Runtime constructor inpsection.
    /// </summary>
    public interface IInspectConstructor
    {
        #region Methods

        /// <summary>
        /// Find the constructor arguments for a list of ordered parameters.
        /// </summary>
        /// <param name="target">The target object type.</param>
        /// <param name="argumentValuesInOrder"></param>
        /// <returns>Returns a list of constructor parameter names and associated values.</returns>
        (string, object)[] FindParameterNames(Type target, params object[] argumentValuesInOrder);

        /// <summary>
        /// Get a map of all the public constructors and a list of the associated parameter types.
        /// </summary>
        /// <param name="target">The target object type.</param>
        /// <returns></returns>
        (ConstructorInfo, IList<Type>) GetConstructorParameterMap(Type target);

        /// <summary>
        /// Get the constructor parameters. This will return the constructor with the least amount
        /// of constructor parameters.
        /// </summary>
        /// <param name="target">The target object type.</param>
        /// <returns></returns>
        IList<Type> GetConstructorParameters(Type target);

        /// <summary>
        /// Finds a constructor with the same parameters and returns the ordered list of parameter
        /// objects that have been matched by name and type.
        /// </summary>
        /// <param name="target">The target object type.</param>
        /// <param name="unorderedValueMapping">
        /// The unordered list of constructor paramaters and the associated name.
        /// </param>
        /// <returns>The ordered list of objects from the mapping.</returns>
        object[] OrderParameters(Type target, IList<(string, object)> unorderedValueMapping);

        /// <summary>
        /// Finds a constructor with the same parameters and returns the ordered list of parameter
        /// objects that have been matched by name and type.
        /// </summary>
        /// <param name="target">The target object type.</param>
        /// <param name="unorderedValueMapping">
        /// The unordered list of constructor paramaters and the associated name.
        /// </param>
        /// <returns>The ordered list of objects from the mapping.</returns>
        object[] OrderParameters(Type target, IDictionary<string, object> unorderedValueMapping);

        #endregion Methods
    }

    /// <summary>
    /// Runtime constructor inpsection.
    /// </summary>
    public class InspectConstructor : IInspectConstructor
    {
        #region Properties

        /// <summary>
        /// Get and instance of the Inpect class.
        /// </summary>
        public static IInspectConstructor Instance { get; set; } = new InspectConstructor();

        #endregion Properties

        #region Methods

        /// <summary>
        /// Find the constructor arguments for a list of ordered parameters.
        /// </summary>
        /// <param name="target">The target object type.</param>
        /// <param name="argumentValuesInOrder"></param>
        /// <returns>Returns a list of constructor parameter names and associated values.</returns>
        public (string, object)[] FindParameterNames(Type target, params object[] argumentValuesInOrder)
        {
            // TODO BM: Better constur
            //var constructor = GetConstructor(target, argumentValuesInOrder.Select(o => o?.GetType());

            var constructors = GetConstructorInfos(target);

            foreach (var constructor in constructors)
            {
                if (TryGetConstructorArgs(constructor, argumentValuesInOrder, out (string, object)[] arguments)) return arguments;
            }

            throw new InvalidOperationException($"The is no constructors with constructor argument count as the requested count or matches the object types in order.");
        }

        /// <summary>
        /// Get a map of all the public constructors and a list of the associated parameter types.
        /// </summary>
        /// <param name="target">The target object type.</param>
        /// <returns></returns>
        public (ConstructorInfo, IList<Type>) GetConstructorParameterMap(Type target)
        {
            var constructors = GetConstructorInfos(target).Select(info => ValueTuple.Create(info, info.GetParameters().Select(i => i.ParameterType).ToArray())).OrderBy(args => args.Item2.Length);
            return constructors.First();
        }

        /// <summary>
        /// Get the constructor parameters. This will return the constructor with the least amount
        /// of constructor parameters.
        /// </summary>
        /// <param name="target">The target object type.</param>
        /// <returns></returns>
        public IList<Type> GetConstructorParameters(Type target)
        {
            var constructors = GetConstructorInfos(target).Select(info => info.GetParameters()).OrderBy(args => args.Length);

            return constructors.First().Select(args => args.ParameterType).ToArray();
        }

        /// <summary>
        /// Finds a constructor with the same parameters and returns the ordered list of parameter
        /// objects that have been matched by name and type.
        /// </summary>
        /// <param name="target">The target object type.</param>
        /// <param name="unorderedValueMapping">
        /// The unordered list of constructor paramaters and the associated name.
        /// </param>
        /// <returns>The ordered list of objects from the mapping.</returns>
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

        /// <summary>
        /// Finds a constructor with the same parameters and returns the ordered list of parameter
        /// objects that have been matched by name and type.
        /// </summary>
        /// <param name="target">The target object type.</param>
        /// <param name="unorderedValueMapping">
        /// The unordered list of constructor paramaters and the associated name.
        /// </param>
        /// <returns>The ordered list of objects from the mapping.</returns>
        public object[] OrderParameters(Type target, IList<(string, object)> unorderedValueMapping)
        {
            return OrderParameters(target, unorderedValueMapping.ToDictionary(key => key.Item1, value => value.Item2));
        }

        private IList<ConstructorInfo> GetConstructorInfos(Type target)
        {
            return target.GetConstructors(BindingFlags.Public | BindingFlags.Instance);
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

        #endregion Methods
    }
}
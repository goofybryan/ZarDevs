using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ZarDevs.Runtime
{
    /// <summary>
    /// Runtime method inpsection.
    /// </summary>
    public interface IInspectMethod
    {
        #region Methods

        /// <summary>
        /// Get method for the target type and arguments.
        /// </summary>
        /// <param name="target">The target object type.</param>
        /// <param name="methodName">The method name.</param>
        /// <param name="argumentValuesInOrder"></param>
        /// <returns></returns>
        MethodInfo FindMethodForArguments(Type target, string methodName, IList<object> argumentValuesInOrder);

        /// <summary>
        /// Get a map of all the public methods and a list of the associated parameter types.
        /// </summary>
        /// <param name="target">The target object type.</param>
        /// <param name="methodName">The method name.</param>
        /// <returns></returns>
        (MethodInfo, IList<Type>) GetMethodParameterMap(Type target, string methodName);

        #endregion Methods
    }

    /// <summary>
    /// Runtime method inpsection.
    /// </summary>
    public class InspectMethod : IInspectMethod
    {
        #region Properties

        /// <summary>
        /// Get and instance of the Inpect class.
        /// </summary>
        public static IInspectMethod Instance { get; set; } = new InspectMethod();

        #endregion Properties

        #region Methods

        /// <summary>
        /// Get method for the target type and arguments.
        /// </summary>
        /// <param name="target">The target object type.</param>
        /// <param name="methodName">The method name.</param>
        /// <param name="argumentValuesInOrder"></param>
        public MethodInfo FindMethodForArguments(Type target, string methodName, IList<object> argumentValuesInOrder)
        {
            return target.GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance, null, argumentValuesInOrder.Select(o => o?.GetType()).ToArray(), null);
        }

        /// <summary>
        /// Get a map of all the public methods and a list of the associated parameter types. Always
        /// gets the method with the lowest parameter count.
        /// </summary>
        /// <param name="target">The target object type.</param>
        /// <param name="methodName">The method name.</param>
        /// <returns></returns>
        public (MethodInfo, IList<Type>) GetMethodParameterMap(Type target, string methodName)
        {
            var constructors = GetMethodInfos(target, methodName).Select(info => ValueTuple.Create(info, info.GetParameters().Select(i => i.ParameterType).ToArray())).OrderBy(args => args.Item2.Length);
            return constructors.First();
        }

        private IEnumerable<MethodInfo> GetMethodInfos(Type target, string methodName)
        {
            return target.GetMethods(BindingFlags.Public | BindingFlags.Instance).Where(m => m.Name == methodName);
        }

        #endregion Methods
    }
}
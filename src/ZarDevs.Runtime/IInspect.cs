using System;
using System.Collections.Generic;

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
        IList<(string, object)> FindParameterNames(Type target, IList<object> argumentValuesInOrder);

        /// <summary>
        /// Finds a constructor with the same parameters and returns the ordered list of objects.
        /// </summary>
        /// <param name="target">The target object type.</param>
        /// <param name="unorderedValueMapping">The unordered list of constructor paramaters and the associated name.</param>
        /// <returns>The ordered list of objects from the mapping.</returns>
        IList<object> OrderParameters(Type target, IList<(string, object)> unorderedValueMapping);

        /// <summary>
        /// Finds a constructor with the same parameters and returns the ordered list of objects.
        /// </summary>
        /// <param name="target">The target object type.</param>
        /// <param name="unorderedValueMapping">The unordered list of constructor paramaters and the associated name.</param>
        /// <returns>The ordered list of objects from the mapping.</returns>
        IList<object> OrderParameters(Type target, IDictionary<string, object> unorderedValueMapping);

        #endregion Methods
    }
}
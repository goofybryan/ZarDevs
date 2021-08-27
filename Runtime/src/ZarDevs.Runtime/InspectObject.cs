using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZarDevs.Runtime
{
    /// <summary>
    /// Runtime object type inspection.
    /// </summary>
    public interface IInspectObject
    {
        #region Methods

        /// <summary>
        /// Find all the inheriting base types for the specified <paramref name="type"/>. If the base type is listed in the <paramref name="ignoredTypes"/> list it will stop and return.
        /// </summary>
        /// <param name="type">The type to inspect.</param>
        /// <param name="ignoredTypes">The ignored types that if inheritted by the <paramref name="type"/> or any base classes, it will stop and return the list of found base types</param>
        /// <returns>All interface types or an empty list.</returns>
        IList<Type> FindBaseTypes(Type type, IList<Type> ignoredTypes);
        
        /// <summary>
        /// Find all the interfaces types for the specified <paramref name="type"/>. If the inteface type is listed in the <paramref name="ignoredTypes"/> it will not be added.
        /// </summary>
        /// <param name="type">The type to inspect.</param>
        /// <param name="ignoredTypes">The ignored types that if implemented or inherited by the <paramref name="type"/> it will not be added.</param>
        /// <returns>All interface types or an empty list.</returns>
        IList<Type> FindInterfaceTypes(Type type, IList<Type> ignoredTypes);
        
        /// <summary>
        /// Find all the interfaces or base types for the specified <paramref name="type"/>. If the inteface or base type is listed in the <paramref name="ignoredTypes"/> it will not be added.
        /// </summary>
        /// <param name="type">The type to inspect.</param>
        /// <param name="ignoredTypes">The ignored types that if implemented or inherited by the <paramref name="type"/> it will not be added.</param>
        /// <returns>All interface types or an empty list.</returns>
        IList<Type> FindImplementingTypes(Type type, IList<Type> ignoredTypes);

        #endregion Methods
    }

    /// <summary>
    /// Runtime object type inspection.
    /// </summary>
    public class InspectObject : IInspectObject
    {
        #region Properties

        /// <summary>
        /// Get and instance of the Inpect class.
        /// </summary>
        public static IInspectObject Instance { get; set; } = new InspectObject();

        #endregion Properties

        #region Methods
        
        /// <summary>
        /// Find all the inheriting base types for the specified <paramref name="type"/>. If the base type is listed in the <paramref name="ignoredTypes"/> list it will stop and return.
        /// </summary>
        /// <param name="type">The type to inspect.</param>
        /// <param name="ignoredTypes">The ignored types that if inherited by the <paramref name="type"/> or any base classes, it will stop and return the list of found base types</param>
        /// <returns>All interface types or an empty list.</returns>
        public IList<Type> FindBaseTypes(Type type, IList<Type> ignoredTypes)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            List<Type> baseTypes = new();

            TryAddBaseType(type, baseTypes, ignoredTypes ?? Array.Empty<Type>());
            
            return baseTypes;
        }
        
        /// <summary>
        /// Find all the interfaces types for the specified <paramref name="type"/>. If the inteface type is listed in the <paramref name="ignoredTypes"/> it will not be added.
        /// </summary>
        /// <param name="type">The type to inspect.</param>
        /// <param name="ignoredTypes">The ignored types that if implemented or inherited by the <paramref name="type"/> it will not be added.</param>
        /// <returns>All interface types or an empty list.</returns>
        public IList<Type> FindInterfaceTypes(Type type, IList<Type> ignoredTypes)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            var interfaceTypes = type.GetInterfaces();

            return ignoredTypes == null || ignoredTypes.Count == 0 || interfaceTypes.Length == 0
                ? interfaceTypes
                : FilterIgnoredTypes(ignoredTypes, interfaceTypes);
        }
        
        /// <summary>
        /// Find all the interfaces or base types for the specified <paramref name="type"/>. If the inteface or base type is listed in the <paramref name="ignoredTypes"/> it will not be added.
        /// </summary>
        /// <param name="type">The type to inspect.</param>
        /// <param name="ignoredTypes">The ignored types that if implemented or inherited by the <paramref name="type"/> it will not be added.</param>
        /// <returns>All interface types or an empty list.</returns>
        public IList<Type> FindImplementingTypes(Type type, IList<Type> ignoredTypes)
        {
            return FindBaseTypes(type, ignoredTypes).Union(FindInterfaceTypes(type, ignoredTypes)).ToList();
        }

        private static IList<Type> FilterIgnoredTypes(IList<Type> ignoredTypes, IList<Type> interfaceTypes)
        {
            List<Type> interfaceTypesReturned = new();

            foreach (var interfaceType in interfaceTypes)
            {
                if (!ignoredTypes.Contains(interfaceType))
                    interfaceTypesReturned.Add(interfaceType);
            }

            return interfaceTypesReturned;
        }

        private void TryAddBaseType(Type type, IList<Type> baseTypes, IList<Type> ignoredTypes)
        {
            var baseType = type.BaseType;

            if(baseType == null || ignoredTypes.Contains(baseType))
                return;

            baseTypes.Add(baseType);

            TryAddBaseType(baseType, baseTypes, ignoredTypes);
        }

        #endregion Methods
    }
}

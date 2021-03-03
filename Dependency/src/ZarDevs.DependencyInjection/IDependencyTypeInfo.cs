using System;

namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// Dependency type information. When resolved the specified type will be instantiated and returned.
    /// </summary>
    public interface IDependencyTypeInfo : IDependencyInfo
    {
        #region Properties

        /// <summary>
        /// Get the resolved type that the IOC will resolved from the <see cref="IDependencyInfo.RequestType"/>.
        /// </summary>
        Type ResolvedType { get; }

        #endregion Properties
    }

    /// <summary>
    /// Dependency info extensions
    /// </summary>
    public static class DependencyInfoExtentions
    {
        /// <summary>
        /// Convert a generic type info into a concrete type info
        /// </summary>
        /// <param name="info">The current dependency info</param>
        /// <param name="concreteRequestType">The concrete type that has been requested.</param>
        /// <returns>A new instance of the dependency info with the concrete information set.</returns>
        public static IDependencyInfo As(this IDependencyInfo info, Type concreteRequestType)
        {
            if (info is IDependencyTypeInfo typeInfo)
                return As(typeInfo, concreteRequestType);

            throw new NotSupportedException($"The dependency info {info} is not currently supported.");
        }

        /// <summary>
        /// Convert a generic type info into a concrete type info
        /// </summary>
        /// <param name="typeInfo">The current dependency info</param>
        /// <param name="concreteRequestType">The concrete type that has been requested.</param>
        /// <returns>A new instance of the dependency info with the concrete information set.</returns>
        public static IDependencyTypeInfo As(this IDependencyTypeInfo typeInfo, Type concreteRequestType)
        {
            if(!typeInfo.ResolvedType.IsGenericType)
                throw new InvalidOperationException($"The info resolved type for '{typeInfo}' is not a generic type.");

            var resovleType = typeInfo.ResolvedType;
            var concreteResolveType = resovleType.MakeGenericType(concreteRequestType.GenericTypeArguments);

            return new DependencyTypeInfo(concreteResolveType, typeInfo) { RequestType = concreteRequestType };
        }
    }
}
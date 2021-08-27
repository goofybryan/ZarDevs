using System;

namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// Dependency info extensions
    /// </summary>
    public static class DependencyInfoExtentions
    {
        #region Methods

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

            if (info is IDependencyFactoryInfo factoryInfo)
                return As(factoryInfo, concreteRequestType);

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
            if (!typeInfo.ResolutionType.IsGenericType)
                throw new InvalidOperationException($"The info resolved type for '{typeInfo}' is not a generic type.");

            var resovleType = typeInfo.ResolutionType;
            var concreteResolveType = resovleType.MakeGenericType(concreteRequestType.GenericTypeArguments);

            return new DependencyTypeInfo(concreteResolveType, typeInfo) { ResolveType = concreteRequestType };
        }

        /// <summary>
        /// Convert a generic factory info into a concrete type info.
        /// </summary>
        /// <param name="typeInfo">The current dependency info</param>
        /// <param name="concreteRequestType">The concrete type that has been requested.</param>
        /// <returns>A new instance of the dependency info with the concrete information set.</returns>
        public static IDependencyFactoryInfo As(this IDependencyFactoryInfo typeInfo, Type concreteRequestType)
        {
            if (!IsFactoryGeneric(typeInfo))
                return new DependencyFactoryInfo(concreteRequestType, typeInfo.MethodName, typeInfo);

            var resovleType = typeInfo.FactoryType;
            var concreteResolveType = resovleType.MakeGenericType(concreteRequestType.GenericTypeArguments);

            return new DependencyFactoryInfo(concreteResolveType, typeInfo.MethodName, typeInfo) { ResolveType = concreteRequestType };
        }

        /// <summary>
        /// Get an indicator to see if the factory is a generic type.
        /// </summary>
        /// <param name="typeInfo"></param>
        /// <returns></returns>
        public static bool IsFactoryGeneric(this IDependencyFactoryInfo typeInfo)
        {
            return typeInfo.FactoryType.IsGenericType;
        }

        #endregion Methods
    }
}
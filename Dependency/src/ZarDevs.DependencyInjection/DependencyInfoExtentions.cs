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
        /// <param name="genericTypeArguments">The generic type arguments.</param>
        /// <returns>A new instance of the dependency info with the concrete information set.</returns>
        public static IDependencyInfo As(this IDependencyInfo info, params Type[] genericTypeArguments)
        {
            if (info is IDependencyTypeInfo typeInfo)
                return As(typeInfo, genericTypeArguments);

            if (info is IDependencyFactoryInfo factoryInfo)
                return As(factoryInfo, genericTypeArguments);

            throw new NotSupportedException($"The dependency info {info} is not currently supported.");
        }

        /// <summary>
        /// Convert a generic type info into a concrete type info
        /// </summary>
        /// <param name="typeInfo">The current dependency info</param>
        /// <param name="genericTypeArguments">The generic type arguments.</param>
        /// <returns>A new instance of the dependency info with the concrete information set.</returns>
        public static IDependencyTypeInfo As(this IDependencyTypeInfo typeInfo, params Type[] genericTypeArguments)
        {
            if (!typeInfo.ResolutionType.IsGenericType)
                throw new InvalidOperationException($"The info resolved type for '{typeInfo}' is not a generic type.");

            var resovleType = typeInfo.ResolutionType;

            return As(typeInfo, genericTypeArguments, resovleType, rt => new DependencyTypeInfo(rt, typeInfo));
        }

        /// <summary>
        /// Convert a generic factory info into a concrete type info.
        /// </summary>
        /// <param name="typeInfo">The current dependency info</param>
        /// <param name="genericTypeArguments">The generic type arguments.</param>
        /// <returns>A new instance of the dependency info with the concrete information set.</returns>
        public static IDependencyFactoryInfo As(this IDependencyFactoryInfo typeInfo, params Type[] genericTypeArguments)
        {
            if (!IsFactoryGeneric(typeInfo))
                return typeInfo;

            var resovleType = typeInfo.FactoryType;

            return As(typeInfo, genericTypeArguments, resovleType, rt => new DependencyFactoryInfo(rt, typeInfo.MethodName, typeInfo));
        }

        /// <summary>
        /// Convert a dependency info to a concreate type.
        /// </summary>
        /// <typeparam name="TInfo">The <see cref="IDependencyInfo"/> type</typeparam>
        /// <param name="info">The current dependency info</param>
        /// <param name="genericTypeArguments">The generic arguments type that has been requested.</param>
        /// <param name="resolutionType">The generic resolution type</param>
        /// <param name="concreateCreation">Function to create a new instance of the <typeparamref name="TInfo"/>. The type passed in will b the concrete type of <paramref name="resolutionType"/>/></param>
        /// <returns></returns>
        public static TInfo As<TInfo>(this TInfo info, Type[] genericTypeArguments, Type resolutionType, Func<Type, TInfo> concreateCreation) where TInfo : IDependencyInfo
        {
            var concreateResolutionType = resolutionType.MakeGenericType(genericTypeArguments);
            var concreateInfo = concreateCreation(concreateResolutionType);
                        
            foreach(var resolveType in info.ResolvedTypes)
            {
                var concreateResolveType = resolveType.MakeGenericType(concreateResolutionType.GenericTypeArguments);
                concreateInfo.ResolvedTypes.Add(concreateResolveType);
            }

            return concreateInfo;
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
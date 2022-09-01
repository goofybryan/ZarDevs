using System.Collections.Generic;

namespace ZarDevs.DependencyInjection.ZarIoc
{
    /// <summary>
    /// Mapper that is used to map a <see cref="IDependencyBuilder"/> <see cref="IDependencyInfo"/> definitions and add the to a <see cref="ITypeFactoryContainter"/>
    /// </summary>
    public interface IDependencyInfoToResolutionMapper
    {
        /// <summary>
        /// Add a <paramref name="mapper"/>.
        /// </summary>
        /// <param name="mapper">The <see cref="IResolutionMapper"/> to add.</param>
        void Add(IResolutionMapper mapper);

        /// <summary>
        /// Map the <paramref name="dependencyInfos"/> definitions to the <paramref name="factoryContainer"/>
        /// </summary>
        void Map(IList<IDependencyInfo> dependencyInfos, ITypeFactoryContainter factoryContainer);
    }
}
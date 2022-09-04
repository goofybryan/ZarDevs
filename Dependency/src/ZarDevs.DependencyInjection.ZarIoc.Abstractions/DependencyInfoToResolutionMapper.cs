using System;
using System.Collections.Generic;
using System.Linq;

namespace ZarDevs.DependencyInjection.ZarIoc
{
    /// <summary>
    /// Mapper that is used to map a <see cref="IDependencyBuilder"/> <see cref="IDependencyInfo"/> definitions and add the to a <see cref="ITypeFactoryContainter"/>
    /// </summary>
    public class DependencyInfoToResolutionMapper : IDependencyInfoToResolutionMapper
    {
        #region Fields

        private readonly IList<IResolutionMapper> _mappers;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Create new instance of the <see cref="DependencyInfoToResolutionMapper"/>
        /// </summary>
        /// <param name="mappers">Specify a list of mappers.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public DependencyInfoToResolutionMapper(params IResolutionMapper[] mappers)
        {
            _mappers = mappers?.ToList() ?? throw new ArgumentNullException(nameof(mappers));
        }

        #endregion Constructors

        /// <inheritdoc/>
        public void Add(IResolutionMapper mapper)
        {
            if (mapper is null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }

            _mappers.Add(mapper);
        }

        /// <inheritdoc/>
        public void Map(IList<IDependencyInfo> definitions, ITypeFactoryContainter factoryContainer)
        {
            foreach (var definition in definitions)
            {
                if (TryGetFromMapper(definition, out var resolution))
                {
                    Map(factoryContainer, resolution);
                }
            }
        }

        private void Map(ITypeFactoryContainter factoryContainer, IDependencyResolution resolution)
        {
            IDependencyResolution mappedResolution;
            if (resolution is IGenericTypeResolution)
            {
                mappedResolution = resolution;
            }
            else
            {
                mappedResolution = resolution.Info.Scope switch
                {
                    DependyBuilderScopes.Transient => resolution,
                    DependyBuilderScopes.Singleton => new SingletonResolution(resolution),
                    DependyBuilderScopes.Request => new RequestResolution(resolution),
                    DependyBuilderScopes.Thread => new ThreadResolution(resolution),
                    _ => throw new IndexOutOfRangeException($"The definition scope {resolution.Info.Scope} is not supported"),
                };
            }

            foreach (Type resolveType in resolution.Info.ResolvedTypes)
            {
                factoryContainer.Add(resolveType, mappedResolution);
            }
        }


        private bool TryGetFromMapper(IDependencyInfo definition, out IDependencyResolution resolution)
        {
            foreach (var mapper in _mappers)
            {
                if (mapper.TryMap(definition, out resolution)) return true;
            }

            resolution = null;
            return false;
        }
    }
}
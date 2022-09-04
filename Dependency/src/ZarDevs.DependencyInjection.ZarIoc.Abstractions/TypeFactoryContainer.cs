using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace ZarDevs.DependencyInjection.ZarIoc
{
    /// <inheritdoc/>
    public class TypeFactoryContainer : ITypeFactoryContainter
    {
        #region Fields

        private readonly Dictionary<Type, IResolution> _resolutionsMap;
        private readonly ConcurrentDictionary<Type, IList<IDependencyResolution>> _typeMap;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Create a new instance of the <see cref="TypeFactoryContainer"/>
        /// </summary>
        public TypeFactoryContainer()
        {
            _resolutionsMap = new();
            _typeMap = new();
        }

        #endregion Constructors

        #region Methods

        /// <inheritdoc/>
        public void Add(Type type, IDependencyResolution resolution)
        {
            IList<IDependencyResolution> resolutions = _typeMap.GetOrAdd(type, t => new List<IDependencyResolution>());

            resolutions.Add(resolution);
        }

        /// <inheritdoc/>
        public IResolution Find(Type type, object key)
        {
            IResolution resolution;

            if (key is not null && TryGet(type, key, out resolution))
            {
                return resolution;
            }

            return TryGet(type, out resolution) ? resolution : new TypeResolutions();
        }

        /// <inheritdoc/>
        public IResolution Get(Type type) => TryGet(type, out var info) ? info : throw new TypeNotFoundException(type);

        /// <inheritdoc/>
        public IResolution Get(Type type, object key) => TryGet(type, key, out var info) ? info : throw new TypeForKeyNotFoundException(type, key);

        /// <inheritdoc/>
        public IEnumerator<Type> GetEnumerator() => _typeMap.Keys.GetEnumerator();

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc/>
        public bool TryGet(Type type, out IResolution resolutions)
        {
            resolutions = null;

            if(_resolutionsMap.TryGetValue(type, out resolutions)) return true;

            if (_typeMap.TryGetValue(type, out var list))
            {
                resolutions = CreateResolutions(type, list);

                return true;
            }

            if (!type.IsConstructedGenericType)
                return false;

            var genericType = type.GetGenericTypeDefinition();

            if (!TryGet(genericType, out var genericResolutions))
                return false;

            var concreateResolutions = _typeMap.GetOrAdd(type, t => CreateConstructed(t, genericResolutions).ToArray());
            resolutions = CreateResolutions(type, concreateResolutions);

            return true;
        }

        /// <inheritdoc/>
        public bool TryGet(Type type, object key, out IResolution resolution)
        {
            resolution = null;

            if (!TryGet(type, out var allResolutions)) return false;

            var filteredResolution = allResolutions is IDependencyResolutions resolutions ? resolutions.Filter(key) : new SingleResolution((IDependencyResolution)allResolutions).Filter(key);
            if(!filteredResolution.IsEmpty)
            { 
                resolution = filteredResolution;
            }

            return resolution != null;
        }

        private IResolution CreateResolutions(Type type, IList<IDependencyResolution> resolutionList)
        {
            return _resolutionsMap[type] = resolutionList.Count == 1 ? resolutionList[0] : new TypeResolutions(resolutionList);
        }

        private IEnumerable<IDependencyResolution> CreateConstructed(Type type, IResolution genericResolutions)
        {

            if (genericResolutions is IGenericTypeResolution resolution)
            {
                yield return CreateConstructed(type, resolution);
                yield break;
            }

            foreach (IGenericTypeResolution genericTypeResolution in ((IEnumerable<IResolution>)genericResolutions).OfType<IGenericTypeResolution>())
            {
                yield return CreateConstructed(type, genericTypeResolution);
            }
        }

        private IDependencyResolution CreateConstructed(Type type, IGenericTypeResolution genericTypeResolution)
        {
            var concreateResolution = genericTypeResolution.MakeConcrete(type);

            switch (genericTypeResolution.Info.Scope)
            {
                case DependyBuilderScopes.Singleton:
                    return new SingletonResolution(concreateResolution);

                case DependyBuilderScopes.Request:
                    return new RequestResolution(concreateResolution);

                case DependyBuilderScopes.Thread:
                    return new ThreadResolution(concreateResolution);

                default:
                    return concreateResolution;
            }
        }

        #endregion Methods
    }
}
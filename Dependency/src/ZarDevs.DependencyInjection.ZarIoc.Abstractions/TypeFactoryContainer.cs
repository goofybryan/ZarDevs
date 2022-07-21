using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ZarDevs.DependencyInjection
{
    /// <inheritdoc/>
    public class TypeFactoryContainer : ITypeFactoryContainter
    {
        #region Fields

        private readonly Dictionary<Type, IList<ITypeResolution>> _typeMap;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Create a new instance of the <see cref="TypeFactoryContainer"/>
        /// </summary>
        public TypeFactoryContainer()
        {
            _typeMap = new();
        }

        #endregion Constructors

        #region Methods

        /// <inheritdoc/>
        public void Add(Type type, ITypeResolution resolution)
        {
            if (!_typeMap.TryGetValue(type, out var resoutionType))
            {
                resoutionType = new List<ITypeResolution>();
                _typeMap[type] = resoutionType;
            }

            resoutionType.Add(resolution);
        }

        /// <inheritdoc/>
        public ITypeResolutions Get(Type type) => TryGet(type, out var info) ? info : throw new TypeNotFoundException(type);

        /// <inheritdoc/>
        public ITypeResolutions Get(Type type, object key) => TryGet(type, key, out var info) ? info : throw new TypeForKeyNotFoundException(type, key);

        /// <inheritdoc/>
        public IEnumerator<Type> GetEnumerator() => _typeMap.Keys.GetEnumerator();

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheritdoc/>
        public bool TryGet(Type type, out ITypeResolutions resolutions)
        {
            resolutions = null;

            if (!_typeMap.TryGetValue(type, out var list)) return false;

            resolutions = new TypeResolutions(list);

            return true;
        }

        /// <inheritdoc/>
        public bool TryGet(Type type, object key, out ITypeResolutions resolutions)
        {
            resolutions = null;

            if (!TryGet(type, out var list)) return false;

            resolutions = list.Filter(key);

            return true;
        }

        #endregion Methods
    }
}
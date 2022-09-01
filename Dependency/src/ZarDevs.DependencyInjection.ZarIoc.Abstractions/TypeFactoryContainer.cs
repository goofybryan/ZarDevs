﻿using System;
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

        private readonly ConcurrentDictionary<Type, IList<ITypeResolution>> _typeMap;

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
            IList<ITypeResolution> resolutions = _typeMap.GetOrAdd(type, t => new List<ITypeResolution>());

            resolutions.Add(resolution);
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

            if (_typeMap.TryGetValue(type, out var list))
            {
                resolutions = new TypeResolutions(list);

                return true;
            }

            if (!type.IsConstructedGenericType)
                return false;

            var genericType = type.GetGenericTypeDefinition();

            if (!TryGet(genericType, out var genericResolutions))
                return false;

            var concreateResolutions = _typeMap.GetOrAdd(type, t => CreateConstructed(t, genericResolutions).ToArray());
            resolutions = new TypeResolutions(concreateResolutions);

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

        private IEnumerable<ITypeResolution> CreateConstructed(Type type, ITypeResolutions genericResolutions)
        {
            foreach (IGenericTypeResolution genericTypeResolution in genericResolutions.OfType<IGenericTypeResolution>())
            {
                var concreateResolution = genericTypeResolution.MakeConcrete(type);

                switch (genericTypeResolution.Info.Scope)
                {
                    case DependyBuilderScopes.Singleton:
                        yield return new SingletonResolution(concreateResolution);
                        break;
                    case DependyBuilderScopes.Request:
                        yield return new RequestResolution(concreateResolution);
                        break;
                    case DependyBuilderScopes.Thread:
                        yield return new ThreadResolution(concreateResolution);
                        break;
                    default:
                        yield return concreateResolution;
                        break;
                }
            }
        }

        #endregion Methods
    }
}
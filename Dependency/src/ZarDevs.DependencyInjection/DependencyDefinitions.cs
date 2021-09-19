using System;
using System.Collections.Generic;
using System.Linq;

namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// Contains a list of dependency definitions
    /// </summary>
    public interface IDependencyDefinitions
    {
        #region Methods

        /// <summary>
        /// Add a dependency to the definitons list.
        /// </summary>
        /// <param name="dependency">The dependency to add.</param>
        void Add(IDependencyInfo dependency);

        /// <summary>
        /// Get a the dependency information for the type and key. If multiple are found, returns
        /// the first one.
        /// </summary>
        /// <typeparam name="T">The request type.</typeparam>
        /// <param name="key">The key</param>
        IDependencyInfo Get<T>(object key);

        /// <summary>
        /// Get a the dependency information for the type and key. If multiple are found, returns
        /// the first one.
        /// </summary>
        /// <param name="requestType">The request type.</param>
        /// <param name="key">The key</param>
        IDependencyInfo Get(Type requestType, object key);

        /// <summary>
        /// Get all the dependency information for the type and key.
        /// </summary>
        /// <typeparam name="T">The request type.</typeparam>
        /// <param name="key">The key</param>
        IEnumerable<IDependencyInfo> GetAll<T>(object key);

        /// <summary>
        /// Get all the dependency information for the type and key.
        /// </summary>
        /// <param name="requestType">The request type.</param>
        /// <param name="key">The key</param>
        IEnumerable<IDependencyInfo> GetAll(Type requestType, object key);

        /// <summary>
        /// Try and get the information for the request type and key.
        /// </summary>
        /// <typeparam name="T">The request type</typeparam>
        /// <param name="key">The key</param>
        IDependencyInfo TryGet<T>(object key);

        /// <summary>
        /// Try and get the information for the request type and key.
        /// </summary>
        /// <param name="requestType">The request type</param>
        /// <param name="key">The key</param>
        IDependencyInfo TryGet(Type requestType, object key);

        #endregion Methods
    }

    internal class DependencyDefinitions : IDependencyDefinitions
    {
        #region Constructors

        public DependencyDefinitions()
        {
            Definitions = new Dictionary<Type, IList<IDependencyInfo>>();
        }

        #endregion Constructors

        #region Properties

        private IDictionary<Type, IList<IDependencyInfo>> Definitions { get; }

        #endregion Properties

        #region Methods

        public void Add(IDependencyInfo dependency)
        {
            var keys = dependency.ResolvedTypes;

            foreach (var key in keys)
            {
                if (!Definitions.TryGetValue(key, out IList<IDependencyInfo> definitions))
                {
                    Definitions[key] = definitions = new List<IDependencyInfo>();
                }

                definitions.Add(dependency);
            }
        }

        public IDependencyInfo Get<T>(object key)
        {
            return GetAll<T>(key).First();
        }

        public IDependencyInfo Get(Type requestType, object key)
        {
            return GetAll(requestType, key).First();
        }

        public IEnumerable<IDependencyInfo> GetAll<T>(object key)
        {
            return GetAll(typeof(T), key);
        }


        public IEnumerable<IDependencyInfo> GetAll(Type requestType, object key)
        {
            if (!Definitions.TryGetValue(requestType, out var dependencies))
                return Enumerable.Empty<IDependencyInfo>();

            return dependencies.Where(d => d.Key == key);
        }

        public IDependencyInfo TryGet<T>(object key)
        {
            return GetAll<T>(key).FirstOrDefault();
        }

        public IDependencyInfo TryGet(Type requestType, object key)
        {
            return GetAll(requestType, key).FirstOrDefault();
        }

        #endregion Methods
    }
}
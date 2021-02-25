using System;
using System.Collections.Generic;
using System.Linq;

namespace ZarDevs.DependencyInjection
{
    public interface IDependencyDefinitions
    {
        #region Methods

        void Add(IDependencyInfo dependency);

        IDependencyInfo Get<T>(object key);

        IDependencyInfo Get(Type requestType, object key);

        IEnumerable<IDependencyInfo> GetAll<T>(object key);

        IEnumerable<IDependencyInfo> GetAll(Type requestType, object key);

        IDependencyInfo TryGet<T>(object key);

        IDependencyInfo TryGet(Type requestType, object key);

        #endregion Methods
    }

    public class DependencyDefinitions : IDependencyDefinitions
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
            var key = dependency.RequestType;

            if (!Definitions.TryGetValue(key, out IList<IDependencyInfo> definitions))
            {
                Definitions[key] = definitions = new List<IDependencyInfo>();
            }

            definitions.Add(dependency);
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

            return dependencies.Where(d => d.Key == (key ?? string.Empty));
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
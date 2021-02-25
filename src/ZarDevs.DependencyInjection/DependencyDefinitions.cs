using System;
using System.Collections.Generic;
using System.Linq;

namespace ZarDevs.DependencyInjection
{
    public interface IDependencyDefinitions
    {
        void Add(IDependencyInfo dependency);
        IDependencyInfo Get<T>(object key);
        IDependencyInfo Get(Type requestType, object key);
        IEnumerable<IDependencyInfo> GetAll<T>(object key);
        IEnumerable<IDependencyInfo> GetAll(Type requestType, object key);
    }

    public class DependencyDefinitions : IDependencyDefinitions
    {
        public DependencyDefinitions()
        {
            Definitions = new Dictionary<Type, IList<IDependencyInfo>>();
        }

        private IDictionary<Type, IList<IDependencyInfo>> Definitions { get; }

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
            return Definitions[requestType].Where(d => d.Key == (key ?? string.Empty));
        }
    }
}
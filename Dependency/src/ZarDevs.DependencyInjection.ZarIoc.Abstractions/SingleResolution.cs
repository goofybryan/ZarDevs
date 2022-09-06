using System.Collections;
using System.Collections.Generic;

namespace ZarDevs.DependencyInjection.ZarIoc
{
    internal class SingleResolution : IDependencyResolutions
    {
        #region Fields

        private readonly IDependencyResolution _resolution;

        #endregion Fields

        #region Constructors

        public SingleResolution() : this(null)
        {
        }

        public SingleResolution(IDependencyResolution resolution)
        {
            _resolution = resolution;
        }

        #endregion Constructors

        #region Properties

        public IDependencyInfo Info => _resolution.Info;

        public bool IsEmpty => Resolution == null;

        public IDependencyResolution Resolution => _resolution;

        #endregion Properties

        #region Methods

        public IDependencyResolutions Filter(object key)
        {
            if (Resolution == null || Resolution.Info.Key != key) return new TypeResolutions();

            return this;
        }

        public IEnumerator<IDependencyResolution> GetEnumerator()
        {
            return new List<IDependencyResolution> { Resolution }.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public object Resolve()
        {
            return Resolution.Resolve();
        }

        public object Resolve(params object[] parameters)
        {
            return Resolution.Resolve(parameters);
        }

        public object Resolve(params (string key, object value)[] parameters)
        {
            return Resolution.Resolve(parameters);
        }

        public IEnumerable<object> ResolveAll()
        {
            return new List<object>(1) { Resolution.Resolve() };
        }

        public IEnumerable<object> ResolveAll(params object[] parameters)
        {
            return new List<object>(1) { Resolution.Resolve(parameters) };
        }

        public IEnumerable<object> ResolveAll(params (string key, object value)[] parameters)
        {
            return new List<object>(1) { Resolution.Resolve(parameters) };
        }

        #endregion Methods
    }
}
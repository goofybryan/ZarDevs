using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ZarDevs.DependencyInjection.ZarIoc
{

    internal class TypeResolutions : IDependencyResolutions
    {
        #region Fields

        private readonly IList<IDependencyResolution> _resolutions;

        bool IDependencyResolutions.IsEmpty => _resolutions.Count == 0;

        #endregion Fields

        #region Constructors

        public TypeResolutions()
        {
            _resolutions = Array.Empty<IDependencyResolution>();
        }

        public TypeResolutions(IEnumerable<IDependencyResolution> resolutions)
        {
            _resolutions = resolutions?.ToArray() ?? throw new System.ArgumentNullException(nameof(resolutions));
        }

        #endregion Constructors

        #region Methods

        public IDependencyResolutions Filter(object key)
        {
            return new TypeResolutions(_resolutions.Where(r => Equals(r.Info.Key, key)));
        }

        public IEnumerator<IDependencyResolution> GetEnumerator()
        {
            return _resolutions.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public object Resolve()
        {
            return ResolveAll().FirstOrDefault();
        }

        public object Resolve(params object[] parameters)
        {
            return ResolveAll(parameters).FirstOrDefault();
        }

        public object Resolve(params (string key, object value)[] parameters)
        {
            return ResolveAll(parameters).FirstOrDefault();
        }

        public IEnumerable<object> ResolveAll(params object[] parameters)
        {
            foreach (var resolution in _resolutions)
            {
                yield return resolution.Resolve(parameters);
            }
        }

        public IEnumerable<object> ResolveAll(params (string key, object value)[] parameters)
        {
            foreach (var resolution in _resolutions)
            {
                yield return resolution.Resolve(parameters);
            }
        }

        public IEnumerable<object> ResolveAll()
        {
            foreach (var resolution in _resolutions)
            {
                yield return resolution.Resolve();
            }
        }

        #endregion Methods
    }
}
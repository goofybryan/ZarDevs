using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ZarDevs.DependencyInjection
{
    internal class TypeResolutions : ITypeResolutions
    {
        #region Fields

        private readonly IEnumerable<ITypeResolution> _resolutions;

        #endregion Fields

        #region Constructors

        public TypeResolutions()
        {
            _resolutions = Enumerable.Empty<ITypeResolution>();
        }

        public TypeResolutions(IEnumerable<ITypeResolution> resolutions)
        {
            _resolutions = resolutions ?? throw new System.ArgumentNullException(nameof(resolutions));
        }

        #endregion Constructors

        #region Methods

        public ITypeResolutions Filter(object key)
        {
            return new TypeResolutions(_resolutions.Where(r => Equals(r.Info.Key, key)));
        }

        public IEnumerator<ITypeResolution> GetEnumerator()
        {
            return _resolutions.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerable<object> Resolve()
        {
            foreach (var resolution in _resolutions)
            {
                yield return resolution.Resolve();
            }
        }

        public IEnumerable<object> Resolve(params object[] parameters)
        {
            foreach (var resolution in _resolutions)
            {
                yield return resolution.Resolve(parameters);
            }
        }

        public IEnumerable<object> Resolve(params (string key, object value)[] parameters)
        {
            foreach (var resolution in _resolutions)
            {
                yield return resolution.Resolve(parameters);
            }
        }

        #endregion Methods
    }
}
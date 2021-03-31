using System.Collections.Generic;

namespace ZarDevs.Http.Client
{
    internal class ApiHttpRequestHandlerBindingMap
    {
        #region Fields

        private readonly object _noKey = new ();
        private readonly Dictionary<object, IApiHttpRequestHandlerBinding> _map;

        #endregion Fields

        #region Constructors

        public ApiHttpRequestHandlerBindingMap()
        {
            _map = new Dictionary<object, IApiHttpRequestHandlerBinding>();
        }

        #endregion Constructors

        #region Indexers

        public IApiHttpRequestHandlerBinding this[object key]
        {
            get => TryGet(key);
            set => TrySet(key, value);
        }

        #endregion Indexers

        #region Methods

        public IApiHttpRequestHandlerBinding TryGet(object key)
        {
            _map.TryGetValue(key ?? _noKey, out IApiHttpRequestHandlerBinding value);
            return value;
        }

        public void TrySet(object key, IApiHttpRequestHandlerBinding binding)
        {
            _map[key ?? _noKey] = binding;
        }

        #endregion Methods
    }
}
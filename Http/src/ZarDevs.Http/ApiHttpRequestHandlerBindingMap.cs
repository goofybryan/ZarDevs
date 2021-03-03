using System;
using System.Collections.Generic;

namespace ZarDevs.Http
{
    internal class ApiHttpRequestHandlerBindingMap
    {
        #region Fields

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
            get => TryGetBinding(key);
            set => TrySetBinding(key, value);
        }

        #endregion Indexers

        #region Methods

        public IApiHttpRequestHandlerBinding TryGetBinding(object key)
        {
            _map.TryGetValue(key ?? string.Empty, out IApiHttpRequestHandlerBinding value);
            return value;
        }

        public void TrySetBinding(object key, IApiHttpRequestHandlerBinding binding)
        {
            _map[key ?? ""] = binding;
        }

        #endregion Methods
    }
}
using System;
using System.Collections.Generic;

namespace ZarDevs.Commands.Http
{
    internal class ApiHttpRequestHandlerBindingMap
    {
        #region Fields

        private readonly Dictionary<string, IApiHttpRequestHandlerBinding> _map;

        #endregion Fields

        #region Constructors

        public ApiHttpRequestHandlerBindingMap()
        {
            _map = new Dictionary<string, IApiHttpRequestHandlerBinding>(StringComparer.OrdinalIgnoreCase);
        }

        #endregion Constructors

        #region Indexers

        public IApiHttpRequestHandlerBinding this[string name]
        {
            get => TryGetBinding(name);
            set => TrySetBinding(name, value);
        }

        #endregion Indexers

        #region Methods

        public IApiHttpRequestHandlerBinding TryGetBinding(string name)
        {
            _map.TryGetValue(name ?? string.Empty, out IApiHttpRequestHandlerBinding value);
            return value;
        }

        public void TrySetBinding(string name, IApiHttpRequestHandlerBinding binding)
        {
            _map[name ?? ""] = binding;
        }

        #endregion Methods
    }
}
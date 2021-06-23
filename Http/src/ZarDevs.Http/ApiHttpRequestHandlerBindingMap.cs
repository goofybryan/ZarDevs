using System.Collections.Generic;

namespace ZarDevs.Http.Client
{
    /// <summary>
    /// Binding map that defines how to get or set a binding for a key.
    /// </summary>
    public class ApiHttpRequestHandlerBindingMap : IApiHttpRequestHandlerBindingMap
    {
        #region Fields

        private readonly Dictionary<object, IApiHttpRequestHandlerBinding> _map;
        private readonly object _noKey = new();

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Create new instance of the binding map.
        /// </summary>
        public ApiHttpRequestHandlerBindingMap()
        {
            _map = new Dictionary<object, IApiHttpRequestHandlerBinding>();
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Try and set the <paramref name="binding"/> for the <paramref name="key"/>. If it exists, it will be overwritten.
        /// </summary>
        /// <param name="key">Specify the key for the binding. Can be null.</param>
        /// <param name="binding">The binding to add.</param>
        public void TryAdd(object key, IApiHttpRequestHandlerBinding binding)
        {
            _map[key ?? _noKey] = binding;
        }

        /// <summary>
        /// Try and get the binding for the <paramref name="key"/>
        /// </summary>
        /// <param name="key">Specify the key for the binding. Can be null.</param>
        /// <returns></returns>
        public IApiHttpRequestHandlerBinding TryGet(object key)
        {
            _map.TryGetValue(key ?? _noKey, out IApiHttpRequestHandlerBinding value);
            return value;
        }

        #endregion Methods
    }
}
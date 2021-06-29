namespace ZarDevs.Http.Client
{
    /// <summary>
    /// Binding map interface that defines how to get or set a binding for a key.
    /// </summary>
    public interface IApiHttpRequestHandlerBindingMap
    {
        #region Methods

        /// <summary>
        /// Try and set the <paramref name="binding"/> for the <paramref name="key"/>. If it exists, it will be overwritten.
        /// </summary>
        /// <param name="key">Specify the key for the binding. Can be null.</param>
        /// <param name="binding">The binding to add.</param>
        void TryAdd(object key, IApiHttpRequestHandlerBinding binding);

        /// <summary>
        /// Try and get the binding for the <paramref name="key"/>
        /// </summary>
        /// <param name="key">Specify the key for the binding. Can be null.</param>
        /// <returns></returns>
        IApiHttpRequestHandlerBinding TryGet(object key);

        #endregion Methods
    }
}
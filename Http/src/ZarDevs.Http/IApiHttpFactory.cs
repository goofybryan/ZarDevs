namespace ZarDevs.Http.Client
{
    /// <summary>
    /// Http Api factory that creates new instance of the <see cref="IApiHttpClient"/>
    /// </summary>
    public interface IApiHttpFactory
    {
        #region Methods

        /// <summary>
        /// Add a request handler to the <see cref="ApiHttpFactory"/>. This will append the handler
        /// to any new <see cref="IApiHttpClient"/> created.
        /// </summary>
        /// <typeparam name="THandler">
        /// Add the <typeparamref name="THandler"/> handler for the specified key.
        /// </typeparam>
        /// <param name="key">
        /// Specify an optional key for the handler. If a key is specified, it will only be added to
        /// new <see cref="IApiHttpClient"/> with the same key. Default is null.
        /// </param>
        /// <returns>
        /// A <see cref="IApiHttpRequestHandlerBinding"/> where you can link multiple additional handlers.
        /// </returns>
        IApiHttpRequestHandlerBinding AddRequestHandler<THandler>(object key = null) where THandler : class, IApiHttpRequestHandler;

        /// <summary>
        /// Create a new instance of the <see cref="IApiHttpClient"/>. This will append any
        /// registered <see cref="IApiHttpRequestHandler"/> for the specified <paramref name="key"/>.
        /// </summary>
        /// <param name="key">
        /// Specify an optional key. This will append any registered <see
        /// cref="IApiHttpRequestHandler"/> for the specified <paramref name="key"/> otherwise use
        /// the default handlers(those registered with null)
        /// </param>
        /// <returns>An new instance of the <see cref="IApiHttpClient"/></returns>
        IApiHttpClient NewClient(object key = null);

        #endregion Methods
    }
}
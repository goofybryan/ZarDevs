namespace ZarDevs.Http.Client
{
    /// <summary>
    /// Api handler binding that be used to build up a linked or an added handler.
    /// </summary>
    public interface IApiHttpRequestHandlerBinding
    {
        #region Methods

        /// <summary>
        /// Add a new binding to the current <see cref="IApiHttpRequestHandlerBinding"/>. This will add the <typeparamref name="THandler"/> to a list of handlers that will be executed when called. This is useful when you have multiple independendent paths to follow.
        /// </summary>
        /// <typeparam name="THandler">The type of handler to add.</typeparam>
        /// <returns>The added binding.</returns>
        IApiHttpRequestHandlerBinding Add<THandler>() where THandler : class, IApiHttpRequestHandler;

        /// <summary>
        /// Build the bindings and create the request handler with the configuratiion given.
        /// </summary>
        /// <returns>The request handler.</returns>
        IApiHttpRequestHandler Build();

        /// <summary>
        /// Link the <typeparamref name="TNext"/> handler to executed after the current handler has been completed. This is usefull when you want to execute a chain of tasks.
        /// </summary>
        /// <typeparam name="TNext">The next handler to be executed.</typeparam>
        /// <returns>The next binding.</returns>
        IApiHttpRequestHandlerBinding Link<TNext>() where TNext : class, IApiHttpRequestHandler;

        /// <summary>
        /// Give the the handler that is used when requiring a specific handler.
        /// </summary>
        /// <param name="name">The name of the handler.</param>
        /// <returns>The current binding.</returns>
        IApiHttpRequestHandlerBinding Named(object name);

        #endregion Methods
    }
}
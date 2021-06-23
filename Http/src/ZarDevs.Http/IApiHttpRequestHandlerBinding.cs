using System;

namespace ZarDevs.Http.Client
{
    /// <summary>
    /// Api handler binding that be used to build up a linked or an added handler.
    /// </summary>
    public interface IApiHttpRequestHandlerBinding
    {
        #region Properties

        /// <summary>
        /// Get the type that this handler is valid for.
        /// </summary>
        Type HandlerType { get; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Add a new binding to the current <see cref="IApiHttpRequestHandlerBinding"/>. This will add the <paramref name="handlerType"/> to a list of handlers that will be executed when called. This
        /// is useful when you have multiple independendent paths to follow.
        /// </summary>
        /// <param name="handlerType">The type of handler to add.</param>
        /// <returns>The added binding.</returns>
        IApiHttpRequestHandlerBinding AppendHandler(Type handlerType);

        /// <summary>
        /// Add a new binding to the current <see cref="IApiHttpRequestHandlerBinding"/>. This will add the <typeparamref name="THandler"/> to a list of handlers that will be executed when called.
        /// This is useful when you have multiple independendent paths to follow.
        /// </summary>
        /// <typeparam name="THandler">The type of handler to add.</typeparam>
        /// <returns>The added binding.</returns>
        IApiHttpRequestHandlerBinding AppendHandler<THandler>() where THandler : class, IApiHttpRequestHandler;

        /// <summary>
        /// Build the bindings and create the request handler with the configuratiion given.
        /// </summary>
        /// <returns>The request handler.</returns>
        IApiHttpRequestHandler Build();

        /// <summary>
        /// Give the the handler that is used when requiring a specific handler.
        /// </summary>
        /// <param name="name">The name of the handler.</param>
        /// <returns>The current binding.</returns>
        IApiHttpRequestHandlerBinding Named(object name);

        /// <summary>
        /// Link the <paramref name="handlerType"/> handler to executed after the current handler has been completed. This is usefull when you want to execute a chain of tasks.
        /// </summary>
        /// <param name="handlerType">The next handler to be executed.</param>
        /// <returns>The next binding.</returns>
        IApiHttpRequestHandlerBinding SetNextHandler(Type handlerType);

        /// <summary>
        /// Link the <typeparamref name="TNext"/> handler to executed after the current handler has been completed. This is usefull when you want to execute a chain of tasks.
        /// </summary>
        /// <typeparam name="TNext">The next handler to be executed.</typeparam>
        /// <returns>The next binding.</returns>
        IApiHttpRequestHandlerBinding SetNextHandler<TNext>() where TNext : class, IApiHttpRequestHandler;

        #endregion Methods
    }
}
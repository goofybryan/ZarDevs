﻿namespace ZarDevs.Http.Client
{
    /// <summary>
    /// Api handler factory that is used to get or create the required handlers.
    /// </summary>
    public interface IApiHttpHandlerFactory
    {
        #region Methods

        /// <summary>
        /// Create a new instance of the handler <typeparamref name="THandler"/> binding.
        /// </summary>
        /// <typeparam name="THandler">The handler the binding will be valid for.</typeparam>
        /// <returns>An instance of <see cref="IApiHttpRequestHandlerBinding"/></returns>
        IApiHttpRequestHandlerBinding CreateHandlerBinding<THandler>() where THandler : class, IApiHttpRequestHandler;

        /// <summary>
        /// Get or create an instance of <typeparamref name="THandler"/>
        /// </summary>
        /// <typeparam name="THandler">The type of handler to get or create.</typeparam>
        /// <returns>The <typeparamref name="THandler"/> instance.</returns>
        IApiHttpRequestHandler GetHandler<THandler>() where THandler : class, IApiHttpRequestHandler;

        #endregion Methods
    }
}
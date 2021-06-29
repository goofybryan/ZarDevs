using System;

namespace ZarDevs.Http.Client
{
    /// <summary>
    /// Abstract implementation of the <see cref="IApiHttpHandlerFactory"/>. This class handles the method <see cref="IApiHttpHandlerFactory.CreateHandlerBinding{THandler}"/> but requires the <see
    /// cref="IApiHttpHandlerFactory.GetHandler{THandler}"/> method to be implemented.
    /// </summary>
    public abstract class ApiHttpHandlerFactoryBase : IApiHttpHandlerFactory
    {

        #region Methods

        /// <summary>
        /// Create a new instance of the handler <typeparamref name="THandler"/> binding.
        /// </summary>
        /// <typeparam name="THandler">The handler the binding will be valid for.</typeparam>
        /// <returns>An instance of <see cref="IApiHttpRequestHandlerBinding"/></returns>
        public IApiHttpRequestHandlerBinding CreateHandlerBinding<THandler>() where THandler : class, IApiHttpRequestHandler
        {
            return CreateHandlerBinding(typeof(THandler));
        }

        /// <summary>
        /// Create a new instance of the handler <paramref name="handlerType"/> binding.
        /// </summary>
        /// <param name="handlerType">The handler the binding will be valid for.</param>
        /// <returns>An instance of <see cref="IApiHttpRequestHandlerBinding"/></returns>
        public IApiHttpRequestHandlerBinding CreateHandlerBinding(Type handlerType)
        {
            return new ApiHttpRequestHandlerBinding(this, handlerType);
        }

        /// <summary>
        /// Get or create an instance of <typeparamref name="THandler"/>
        /// </summary>
        /// <typeparam name="THandler">The type of handler to get or create.</typeparam>
        /// <returns>The <typeparamref name="THandler"/> instance.</returns>
        public virtual IApiHttpRequestHandler GetHandler<THandler>() where THandler : class, IApiHttpRequestHandler
        {
            return GetHandler(typeof(THandler));
        }

        /// <summary> 
        /// Get or create an instance of <paramref name="handlerType"/> 
        /// </summary> 
        /// <param name="handlerType">The type of handler to get or create.</param> 
        /// <returns>The the request handler instance.</returns>
        public abstract IApiHttpRequestHandler GetHandler(Type handlerType);

        #endregion Methods

    }
}
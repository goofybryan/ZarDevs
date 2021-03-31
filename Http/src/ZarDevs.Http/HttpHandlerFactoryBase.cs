namespace ZarDevs.Http.Client
{
    /// <summary>
    /// Abstract implementation of the <see cref="IApiHttpHandlerFactory"/>. This class handles the method <see cref="IApiHttpHandlerFactory.CreateHandlerBinding{THandler}"/> but requires the <see cref="IApiHttpHandlerFactory.GetHandler{THandler}"/> method to be implemented.
    /// </summary>
    public abstract class ApiHttpHandlerFactoryBase : IApiHttpHandlerFactory
    {
        /// <summary>
        /// Create a new instance of the handler <typeparamref name="THandler"/> binding.
        /// </summary>
        /// <typeparam name="THandler">The handler the binding will be valid for.</typeparam>
        /// <returns>An instance of <see cref="IApiHttpRequestHandlerBinding"/></returns>
        public IApiHttpRequestHandlerBinding CreateHandlerBinding<THandler>() where THandler : class, IApiHttpRequestHandler
        {
            return new ApiHttpRequestHandlerBinding<THandler>(this);
        }


        /// <summary>
        /// Get or create an instance of <typeparamref name="THandler"/>
        /// </summary>
        /// <typeparam name="THandler">The type of handler to get or create.</typeparam>
        /// <returns>The <typeparamref name="THandler"/> instance.</returns>
        public abstract IApiHttpRequestHandler GetHandler<THandler>() where THandler : class, IApiHttpRequestHandler;
    }
}
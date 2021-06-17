using System;
using System.Collections.Generic;
using ZarDevs.Runtime;

namespace ZarDevs.Http.Client
{
    /// <summary>
    /// Default http handler factory. This uses <see cref="ICreate"/> or custom registered functions to get the requested handlers.
    /// </summary>
    public class DefaultHttpHandlerFactory : ApiHttpHandlerFactoryBase
    {
        #region Fields

        private readonly ICreate _create;
        private readonly IDictionary<Type, Func<IApiHttpRequestHandler>> _handlerMapping;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="create">And instance of <see cref="ICreate"/></param>
        public DefaultHttpHandlerFactory(ICreate create)
        {
            _create = create ?? throw new ArgumentNullException(nameof(create));
            _handlerMapping = new Dictionary<Type, Func<IApiHttpRequestHandler>>();
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Add a creation function for the <typeparamref name="THandler"/> type. This will be checked first before using runtime ( <see cref="ICreate"/>) to create a handler when calling <see cref="GetHandler{THandler}"/>
        /// </summary>
        /// <typeparam name="THandler">The type of handler to get or create.</typeparam>
        /// <param name="creatorFunc"></param>
        public void AddHandlerCreation<THandler>(Func<IApiHttpRequestHandler> creatorFunc) => _handlerMapping[typeof(THandler)] = creatorFunc;

        /// <summary>
        /// Get or create an instance of <typeparamref name="THandler"/>
        /// </summary>
        /// <typeparam name="THandler">The type of handler to get or create.</typeparam>
        /// <returns>The <typeparamref name="THandler"/> instance.</returns>
        public override IApiHttpRequestHandler GetHandler<THandler>()
        {
            return _handlerMapping.TryGetValue(typeof(THandler), out Func<IApiHttpRequestHandler> creatorFunc) ? creatorFunc() : _create.New<THandler>();
        }

        #endregion Methods
    }
}
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
        private readonly IDictionary<Type, Func<Type, IApiHttpRequestHandler>> _handlerMapping;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="create">And instance of <see cref="ICreate"/></param>
        public DefaultHttpHandlerFactory(ICreate create)
        {
            _create = create ?? throw new ArgumentNullException(nameof(create));
            _handlerMapping = new Dictionary<Type, Func<Type, IApiHttpRequestHandler>>();
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Add a creation function for the <typeparamref name="THandler"/> type. This will be checked first before using runtime ( <see cref="ICreate"/>) to create a handler when calling <see cref="GetHandler"/>
        /// </summary>
        /// <typeparam name="THandler">The type of handler to get or create.</typeparam>
        /// <param name="creatorFunc"></param>
        public void AddHandlerCreation<THandler>(Func<Type, IApiHttpRequestHandler> creatorFunc) => _handlerMapping[typeof(THandler)] = creatorFunc;

        /// <summary> 
        /// Get or create an instance of <paramref name="handlerType"/> 
        /// </summary> 
        /// <param name="handlerType">The type of handler to get or create.</param> 
        /// <returns>The the request handler instance.</returns>
        public override IApiHttpRequestHandler GetHandler(Type handlerType)
        {
            return _handlerMapping.TryGetValue(handlerType, out Func<Type, IApiHttpRequestHandler> creatorFunc) ? creatorFunc(handlerType) : (IApiHttpRequestHandler)_create.New(handlerType);
        }

        #endregion Methods
    }
}
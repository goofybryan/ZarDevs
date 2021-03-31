using System;
using ZarDevs.Runtime;

namespace ZarDevs.Http.Client
{
    /// <summary>
    /// Default http handler factory. This uses <see cref="Create"/> object that makes use of reflection.
    /// </summary>
    public class DefaultHttpHandlerFactory : IApiHttpHandlerFactory
    {
        private readonly ICreate _create;

        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="create">And instance of <see cref="ICreate"/></param>
        public DefaultHttpHandlerFactory(ICreate create)
        {
            _create = create ?? throw new ArgumentNullException(nameof(create));
        }

        #region Methods

        /// <summary>
        /// Get or create an instance of <typeparamref name="THandler"/>
        /// </summary>
        /// <typeparam name="THandler">The type of handler to get or create.</typeparam>
        /// <returns>The <typeparamref name="THandler"/> instance.</returns>
        public IApiHttpRequestHandler GetHandler<THandler>() where THandler : class, IApiHttpRequestHandler
        {
            return _create.New<THandler>();
        }

        #endregion Methods
    }
}
using System;
using System.Collections.Generic;

namespace ZarDevs.Http.Client
{
    internal class ApiHttpRequestHandlerBinding : IApiHttpRequestHandlerBinding
    {
        #region Fields

        private readonly IApiHttpHandlerFactory _factory;
        private readonly Type _handlerType;

        #endregion Fields

        #region Constructors

        public ApiHttpRequestHandlerBinding(IApiHttpHandlerFactory factory, Type handlerType)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            _handlerType = handlerType ?? throw new ArgumentNullException(nameof(handlerType));
        }

        #endregion Constructors

        #region Properties

        public Type HandlerType => _handlerType;
        public object Name { get; private set; }
        internal IList<IApiHttpRequestHandlerBinding> Bindings { get; } = new List<IApiHttpRequestHandlerBinding>();
        internal IApiHttpRequestHandlerBinding Next { get; set; }

        #endregion Properties

        #region Methods

        public IApiHttpRequestHandlerBinding AppendHandler<TBinding>() where TBinding : class, IApiHttpRequestHandler
        {
            return AppendHandler(typeof(TBinding));
        }

        public IApiHttpRequestHandlerBinding AppendHandler(Type handlerType)
        {
            var binding = new ApiHttpRequestHandlerBinding(_factory, handlerType);
            Bindings.Add(binding);
            return binding;
        }

        public IApiHttpRequestHandler Build()
        {
            var handler = _factory.GetHandler(_handlerType);

            if (Next != null)
            {
                handler.SetNextHandler(Next.Build());
            }

            foreach (var binding in Bindings)
            {
                handler.AppendHandler(binding.Build());
            }

            return handler;
        }

        public IApiHttpRequestHandlerBinding Named(object name)
        {
            Name = name;
            return this;
        }

        public IApiHttpRequestHandlerBinding SetNextHandler<TNext>() where TNext : class, IApiHttpRequestHandler
        {
            return SetNextHandler(typeof(TNext));
        }

        public IApiHttpRequestHandlerBinding SetNextHandler(Type handlerType)
        {
            var binding = new ApiHttpRequestHandlerBinding(_factory, handlerType);
            Next = binding;
            return binding;
        }

        #endregion Methods
    }
}
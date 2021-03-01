using System.Collections.Generic;

namespace ZarDevs.Commands.Http
{
    internal class ApiHttpRequestHandlerBinding<THandler> : IApiHttpRequestHandlerBinding where THandler : class, IApiHttpRequestHandler
    {
        #region Fields

        private readonly IApiHttpHandlerFactory _factory;

        #endregion Fields

        #region Constructors

        public ApiHttpRequestHandlerBinding(IApiHttpHandlerFactory factory)
        {
            Name = "";
            _factory = factory ?? throw new System.ArgumentNullException(nameof(factory));
        }

        #endregion Constructors

        #region Properties

        public string Name { get; private set; }

        internal IList<IApiHttpRequestHandlerBinding> Bindings { get; } = new List<IApiHttpRequestHandlerBinding>();
        internal IApiHttpRequestHandlerBinding Next { get; private set; }

        #endregion Properties

        #region Methods

        public IApiHttpRequestHandlerBinding Add<TBinding>() where TBinding : class, IApiHttpRequestHandler
        {
            var binding = new ApiHttpRequestHandlerBinding<TBinding>(_factory);
            Bindings.Add(binding);
            return binding;
        }

        public IApiHttpRequestHandler Build()
        {
            var handler = _factory.GetHandler<THandler>();

            if (Next != null)
            {
                handler.SetInnerHandler(Next.Build());
            }

            foreach (var binding in Bindings)
            {
                handler.AppendHandler(binding.Build());
            }

            return handler;
        }

        public IApiHttpRequestHandlerBinding Chain<TNext>() where TNext : class, IApiHttpRequestHandler
        {
            var binding = new ApiHttpRequestHandlerBinding<TNext>(_factory);
            Next = binding;
            return binding;
        }

        public IApiHttpRequestHandlerBinding Named(string name)
        {
            Name = name ?? "";
            return this;
        }

        #endregion Methods
    }
}
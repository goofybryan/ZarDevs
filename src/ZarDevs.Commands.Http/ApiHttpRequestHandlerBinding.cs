using System.Collections.Generic;

namespace ZarDevs.Commands.Http
{
    internal class ApiHttpRequestHandlerBinding<THandler> : IApiHttpRequestHandlerBinding where THandler : IApiHttpRequestHandler
    {
        #region Constructors

        public ApiHttpRequestHandlerBinding()
        {
            Name = "";
        }

        #endregion Constructors

        #region Properties

        public string Name { get; private set; }

        internal IList<IApiHttpRequestHandlerBinding> Bindings { get; } = new List<IApiHttpRequestHandlerBinding>();
        internal IApiHttpRequestHandlerBinding Next { get; private set; }

        #endregion Properties

        #region Methods

        public IApiHttpRequestHandlerBinding Add<TBinding>() where TBinding : IApiHttpRequestHandler
        {
            var binding = new ApiHttpRequestHandlerBinding<TBinding>();
            Bindings.Add(binding);
            return binding;
        }

        public IApiHttpRequestHandler Build()
        {
            var handler = Ioc.Get<THandler>();

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

        public IApiHttpRequestHandlerBinding Chain<TNext>() where TNext : IApiHttpRequestHandler
        {
            var binding = new ApiHttpRequestHandlerBinding<TNext>();
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
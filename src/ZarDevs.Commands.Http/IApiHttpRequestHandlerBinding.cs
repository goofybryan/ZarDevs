namespace ZarDevs.Commands.Http
{
    public interface IApiHttpRequestHandlerBinding
    {
        #region Methods

        IApiHttpRequestHandlerBinding Add<TBinding>() where TBinding : IApiHttpRequestHandler;

        IApiHttpRequestHandler Build();

        IApiHttpRequestHandlerBinding Chain<TNext>() where TNext : IApiHttpRequestHandler;

        IApiHttpRequestHandlerBinding Named(string name);

        #endregion Methods
    }
}
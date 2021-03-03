namespace ZarDevs.Http
{
    public interface IApiHttpRequestHandlerBinding
    {
        #region Methods

        IApiHttpRequestHandlerBinding Add<TBinding>() where TBinding : class, IApiHttpRequestHandler;

        IApiHttpRequestHandler Build();

        IApiHttpRequestHandlerBinding Chain<TNext>() where TNext : class, IApiHttpRequestHandler;

        IApiHttpRequestHandlerBinding Named(string name);

        #endregion Methods
    }
}
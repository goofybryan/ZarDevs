# ZarDevs Dependency Injection

## Introduction

I have been working in the .Net environment for years, and I as everyone know, you generate a style of how you code. So when it comes to new projects, I always tend to have the same patterns. However, I was working with a friend of mine, developing the backbone and putting place what needs to be done and his comment stuck with me. He said that what did just made it easy to follow.

The concept behind these libraries is to create a unified/standardized way to do [IOC (Inversion of Control)](https://en.wikipedia.org/wiki/Inversion_of_control). Currently everyone has their own way of how to implement it and there isn't a standard way of doing this. Either you have to choose a specific technology, like [Ninject](http://www.ninject.org/), [AutoFac](https://autofac.org/), [Microsoft](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-5.0), etc. and use it or choose a completely different pattern. This prevents one from creating re-useable shareable code using a IOC pattern without deciding upfront which technology you are going to use.

## What is ZarDevs Dependency Injection?

My goal was to write a standardized way to create bindings and then have them "translated" to the native technology. This way I can write my shared code using IOC without fear of the underlying technology is or having to decide what I would like to use.

I enjoy writing code that takes away the need to continually write complex code to do simple tasks. For example just to communicate to an authenticated server there is several steps that need to checked before and after each call to the server. And these steps are sometime very hard to cater for because you do not know where your solution will be used.

What I did not cater by choice was `Scoped` variables. These are complex behaviours that can be easily overcome using other means and/or directly implementing them in the underlying technology.

To see how to use please read the following [document](./src/ZarDevs.DependencyInjection/README.md)

## Why IOC?

What I like about it is the fact that you allow the infrastructure to give you the objects you require by configuration. So it is possible to have the same object implementing and interface and have comepletely different results. Now you can achieve this using all sorts of different techniques and patterns. I also enjoy creating code that is simple and then allowing infrastructure to do the heaving lifting. Take the following example:

    ```c#
    // Illustrative purposes only.
    public class IocExample
    {
        private readonly IGetCommand _ioc;
        private readonly IGetCommand _factory;


        public IocExample(IIocContainer Ioc, IFactory factory)
        {
            _ioc = ioc ?? throw new ArgumentNullException("Ioc cannot be null", nameof(ioc));
            _factory_ = factory ?? throw new ArgumentNullException("Factory cannot be null", nameof(factory));
        }

        public Task<string> ExampleIoc(string url)
        {
            // All the required classes will be injected into the GetCommand
            var command = _ioc.Get<IGetCommand>();
            return command.ExecuteAsync(url);
        }

        public Task<string> ExampleFactory(string url)
        {
            // Very similar to IOC, great alternative, just very rigid and needs to be implemented.
            var command = _factory.CreateGetCommand();
            return command.ExecuteAsync(url);
        }

        public Task<string> ExampleNative(string url)
        {
            // Essentially what the factory would do behind the scenes. However, testability of this code is hard.
            ILoginProcess loginProcess = new LoginProcess();
            IApiTokenStore apiTokenStore = new ApiTokenStore();
            IApiTokenHandler apiTokenHandler = new ApiTokenHandler(loginProcess, apiTokenStore);
            IHttpClient apiClient = new ApiHttpClient(apiTokenHandler);
            IHttpResolutionHandler resolutionHandler = new HttpResolutionHandler();

            var command = new GetCommand(apiClient, resolutionHandler);
            return command.ExecuteAsync(url);
        }
    }

    public class GetCommand : IGetCommand
    {
        private readonly IHttpClient _client;
        private readonly IHttpResolutionHandler _resolutionHandler;

        public GetCommand(IHttpClient client, IHttpResolutionHandler resolutionHandler)
        {
            _client = client ?? throw new ArgumentNullException("Client cannot be null", nameof(client));
            _resolutionHandler = resolutionHandler ?? throw new ArgumentNullException("Resolution handler cannot be null", nameof(resolutionHandler));
        }

        public async Task<string> ExecuteAsync(string url)
        {
            try
            {
                return await client.GetAsync<string>(url);
            }
            catch(HttpException excetion)
            {
                _resolutionHandler.Handle(exception);
            }
        }
    }

    public class ApiHttpClient : IHttpClient
    {
        private readonly IApiTokenHandler _apiTokenHandler;
        private readonly HttpClient _httpClient;

        public ApiHttpClient(IApiTokenHandler apiTokenHandler)
        {
            _apiTokenHandler = apiTokenHandler ?? throw new ArgumentNullException("Api token handler cannot be null", nameof(apiTokenHandler));
            _httpClient = new HttpClient();
        }

        public async Task<T> GetAsync(string url)
        {
            if(!_apiTokenHandler.TokenIsValid)
                await _apiTokenHandler.RefreshAsync(_httpClient);

            _apiTokenHandler.AddToken(_httpClient);

            return await _httpClient.GetStringAsync();
        }
    }

    public class ApiTokenHandler : IApiTokenHandler
    {
        private readonly ILoginProcess _loginProcess;
        private readonly IApiTokenStore _apiTokenStore;

        public ApiTokenHandler(ILoginProcess loginProcess, IApiTokenStore apiTokenStore)
        {
            _loginProcess = loginProcess ?? throw new ArgumentNullException("Login process cannot be null", nameof(loginProcess));
            _apiTokenStore = loginProcess ?? throw new ArgumentNullException("Api token store cannot be null", nameof(apiTokenStore));
        }

        public bool TokenIsValid => apiTokenStore.ValidateToken();

        public async Task AddToken(HttpClient client)
        {
            // Add Headers/Tokens/Certificates etc
            ... 
        }

        public async Task RefreshAsync(_httpClient)
        {
            bool refreshed = await TryRefresh();
            if(refreshed) return;
            
            bool loggedIn = await TryLogin();
            if(loggedIn) return;

            throw new NotAuthorisedException();
        }

        private Task<bool> TryRefresh()
        {
            string token = await _loginProcess.TryFreshToken();
            
            if(string.IsNullOrEmpty(token)) return false;

            _apiTokeStore.Update(token);

            return true;
        }

        private Task<bool> TryLogin()
        {
            string token = await _loginProcess.TryLogin();
            
            if(string.IsNullOrEmpty(token)) return false;

            _apiTokeStore.Update(token);

            return true;
        }
    }

    ```

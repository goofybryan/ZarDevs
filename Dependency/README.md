# ZarDevs Dependency Injection

## Introduction

I have been working in the .Net environment for years, and I as everyone know, you generate a style of how you code. So when it comes to new projects, I always tend to have the style of coding and patterns. There are plenty of ways to skin a fish and I myself enjoy certain combinations, and try to abstract myself from any specific way of doing things. However, I do enjoy the concept of IOC and have been wanting to play in this area.

The concept behind these libraries is to create a unified/standardized way to do [IOC (Inversion of Control)](https://en.wikipedia.org/wiki/Inversion_of_control). Currently everyone has their own way of how to implement it and there isn't a standard way of doing this. Either you have to choose a specific technology, like [Ninject](http://www.ninject.org/), [AutoFac](https://autofac.org/), [Microsoft](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-5.0), etc. and use it or choose a completely different pattern. This prevents one from creating re-useable shareable code using a IOC pattern without deciding upfront which technology you are going to use.

## What is ZarDevs Dependency Injection?

My goal was to write a standardized way to create bindings and then have them "translated" to the native technology. This way I can write my shared code using IOC without fear of the underlying technology is or having to decide what I would like to use.

I enjoy writing code that takes away the need to continually write complex code to do simple tasks. For example just to communicate to an authenticated server there is several steps that need to checked before and after each call to the server. And these steps are sometime very hard to cater for because you do not know where your solution will be used.

What I did not cater by choice was `Scoped` variables. I plan on implementation this for websites as an extension.

To see how to use please read the following [document](./src/ZarDevs.DependencyInjection/README.md)

## Why IOC?

What I like about it is the fact that you allow the infrastructure to give you the objects you require by configuration. So it is possible to have the same object implementing and interface and have comepletely different results. Now you can achieve this using all sorts of different techniques and patterns. I also enjoy creating code that is simple and then allowing infrastructure to do the heaving lifting. Take the following example:

    ```c#
    // Illustrative purposes only.
    public static class IocExample
    {
        public static Task<string> ExampleIoc(string url)
        {
            // Using service locator pattern, you could request that the IOC give you the command (ideally you would use constructor injection but this is just an example.)
            var command = Ioc.Get<IGetCommand>();
            return command.ExecuteAsync(url);
        }

        public static Task<string> ExampleFactory(string url)
        {
            // Very similar to IOC, great alternative, just very rigid and needs to be implemented, but requires a factory object (ideally you would use constructor injection but this is just an example.)
            var factory = new ExampleFactory();
            var command = factory.CreateGetCommand();
            return command.ExecuteAsync(url);
        }

        public static Task<string> ExampleNative(string url)
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

            _apiTokenStore.Update(token);

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

## What still needs to be done

1. Validation of bindings
1. Validation tests construct
1. Optimizations (look at code and see where I can improve)
1. Investigate better `Build<T>().To<T>()` Pattern
1. Perhaps support `IServiceProvider` explicitly?
1. Memory load testing (ensure that I am not too much of an overhead)
1. More tests
1. Implement a scoped implementation as extension
1. Optimize the Factory dependency expression builder to allow for passed in parameters plans
1. Create examples:
    1. Example web server project
    1. Example Xamarin Forms project with both Android and iOS
    1. Example Blazor
    1. More examples

## Links

1. [Home](../../README.md)
1. [Dependency Injection](./README.md)
    1. [Dependency Injection](./src/Zar.Devs.DependencyInjection/README.md)
    1. [AutoFac Dependency Injection](./src/ZarDevs.DependencyInjection.AutoFac/README.md)
    1. [Microsoft Dependency Injection](./src/ZarDevs.DependencyInjection.Microsoft/README.md)
    1. [Ninject Dependency Injection](./src/ZarDevs.DependencyInjection.Ninject/README.md)
    1. [Runtime Dependency Injection](./src/ZarDevs.DependencyInjection.RuntimeFactory/README.md)
    1. [Dependency Injection Extensions](./src/ZarDevs.DependencyInjection.Extensions/README.md)
    1. [Dependency Injection Factory Extensions](./src/ZarDevs.DependencyInjection.Extensions.Factory/README.md)

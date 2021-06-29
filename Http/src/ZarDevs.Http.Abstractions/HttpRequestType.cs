namespace ZarDevs.Http.Api
{
    /// <summary>
    /// Http request verbs used to do api calls
    /// </summary>
    public enum HttpRequestType
    {
        /// <summary>
        /// The GET method requests a representation of the specified resource. Requests using GET should only retrieve data.
        /// </summary>
        Get = 0,
        /// <summary>
        /// The POST method is used to submit an entity to the specified resource, often causing a change in state or side effects on the server.
        /// </summary>
        Post,
        /// <summary>
        /// The PUT method replaces all current representations of the target resource with the request payload.
        /// </summary>
        Put,
        /// <summary>
        /// he PATCH method is used to apply partial modifications to a resource.
        /// </summary>
        Patch,
        /// <summary>
        /// The DELETE method deletes the specified resource.
        /// </summary>
        Delete,
        /// <summary>
        /// Custom request type. This is used when you are not using a standard verb or want to make use of the IApiHttpClient.SendAsync method for api calls that do not follow this libraries conventions.
        /// </summary>
        Custom
    }
}
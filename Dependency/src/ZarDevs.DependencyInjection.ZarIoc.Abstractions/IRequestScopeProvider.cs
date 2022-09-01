using System;

namespace ZarDevs.DependencyInjection.ZarIoc
{
    /// <summary>
    /// Request scope provider that will return a IOC container that is scope container
    /// </summary>
    public interface IRequestScopeProvider : IDisposable
    {
        /// <summary>
        /// Add a value to the current scope
        /// </summary>
        /// <param name="info"></param>
        /// <param name="add"></param>
        object GetOrAdd(IDependencyInfo info, Func<object> add);
    }
}
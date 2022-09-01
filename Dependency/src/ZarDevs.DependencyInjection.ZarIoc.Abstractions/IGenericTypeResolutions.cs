using System;

namespace ZarDevs.DependencyInjection.ZarIoc
{
    /// <summary>
    /// Generic type resolution that knows how to convert a undefined generic to a defined concrete generic resolution
    /// </summary>
    public interface IGenericTypeResolution : ITypeResolution
    {
        /// <summary>
        /// Make a concreate resolution list from the underlying type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        ITypeResolution MakeConcrete(Type type);
    }
}
using System;

namespace ZarDevs.DependencyInjection.ZarIoc
{
    /// <summary>
    /// Attribute that will be used by the generator to find all dependency generations. The method
    /// if specified must conform to to <see cref="IDependencyRegistration.Register(IDependencyBuilder)"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public sealed class DependencyRegistrationAttribute : Attribute
    {
    }
}
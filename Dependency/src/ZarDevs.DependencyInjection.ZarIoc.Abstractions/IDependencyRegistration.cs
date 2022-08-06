using System;

namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// Interface that will be used by the generator to create the source.
    /// </summary>
    public interface IDependencyRegistration
    {
        /// <summary>
        /// Register the dependencies.
        /// </summary>
        /// <param name="builder">The depndency builder.</param>
        public void Register(IDependencyBuilder builder);
    }

    /// <summary>
    /// Attribute that will be used by the generator to find all dependency generations. The method if specified must conform to to <see cref="IDependencyRegistration.Register(IDependencyBuilder)"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public sealed class DependencyRegistrationAttribute : Attribute
    {
    }
}
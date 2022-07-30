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
    /// Attribute that will be used by the generator to find all dependency generations.
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class DependencyRegistrationAttribute : Attribute
    {
        public DependencyRegistrationAttribute() : this(null) { }

        public DependencyRegistrationAttribute(string method)
        {
            Method = method;
        }

        public string Method { get; }
    }
}
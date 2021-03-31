using System;

namespace ZarDevs.Runtime
{
    /// <summary>
    /// Create class that is uses reflection to create new objects.
    /// </summary>
    public interface ICreate
    {
        #region Methods

        /// <summary>
        /// Create a new instance of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type to create.</typeparam>
        /// <param name="constructorArgs">The list of constructor args.</param>
        /// <returns>A new instance.</returns>
        T New<T>(params object[] constructorArgs);

        /// <summary>
        /// Create a new instace of <paramref name="type"/>
        /// </summary>
        /// <param name="type">The type to create</param>
        /// <param name="constructorArgs">The list of constructor args.</param>
        /// <returns>A new instance.</returns>
        object New(Type type, params object[] constructorArgs);

        #endregion Methods
    }

    /// <summary>
    /// Create class that is uses reflection to create new objects.
    /// </summary>
    public class Create : ICreate
    {
        #region Properties

        /// <summary>
        /// Static instance of the <see cref="ICreate"/> object.
        /// </summary>
        public static ICreate Instance { get; set; } = new Create();

        #endregion Properties

        #region Methods

        /// <summary>
        /// Create a new instance of <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type to create.</typeparam>
        /// <param name="constructorArgs">The list of constructor args.</param>
        /// <returns>A new instance.</returns>
        public T New<T>(params object[] constructorArgs)
        {
            var value = (T)New(typeof(T), constructorArgs);
            return value;
        }

        /// <summary>
        /// Create a new instace of <paramref name="type"/>
        /// </summary>
        /// <param name="type">The type to create</param>
        /// <param name="constructorArgs">The list of constructor args.</param>
        /// <returns>A new instance.</returns>
        public object New(Type type, params object[] constructorArgs)
        {
            if (constructorArgs?.Length == 0)
                return Activator.CreateInstance(type);

            var value = Activator.CreateInstance(type, constructorArgs);
            return value;
        }

        #endregion Methods
    }
}
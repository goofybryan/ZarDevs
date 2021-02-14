using System;

namespace ZarDevs.Runtime
{
    public interface ICreate
    {
        #region Methods

        T New<T>(params object[] constructorArgs);

        object New(Type type, params object[] constructorArgs);

        #endregion Methods
    }

    public class Create : ICreate
    {
        #region Properties

        public static ICreate Instance { get; set; } = new Create();

        #endregion Properties

        #region Methods

        public T New<T>(params object[] constructorArgs)
        {
            var value = (T)New(typeof(T), constructorArgs);
            return value;
        }

        public object New(Type type, params object[] constructorArgs)
        {
            var value = Activator.CreateInstance(type, constructorArgs);
            return value;
        }

        #endregion Methods
    }
}
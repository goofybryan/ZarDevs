using System;

namespace ZarDevs.Core.Runtime
{
    public interface ICreate
    {
        T New<T>(params object[] constructorArgs);
        object New(Type type, params object[] constructorArgs);
    }

    public class Create : ICreate
    {
        public static ICreate Instance { get; } = new Create();

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
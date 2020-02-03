using System;

namespace ZarDevs.Core.Runtime
{
    public static class Create
    {
        #region Methods

        public static T New<T>(params object[] constructorArgs)
        {
            var value = (T)New(typeof(T), constructorArgs);
            return value;
        }

        public static object New(Type type, params object[] constructorArgs)
        {
            var value = Activator.CreateInstance(type, constructorArgs);
            return value;
        }

        #endregion Methods
    }
}
using System;

namespace ZarDevs.DependencyInjection
{
    // TODO BM: Add methods for getting all services e.g. IEnumerable<T> ResolveAll<T>();
    // TODO BM: Add methods for partial parameters and resolve rest (constructor matching to nearest parameter size)
    public interface IIocContainer
    {
        #region Methods

        T Resolve<T>(params object[] parameters);

        T Resolve<T>(params (string, object)[] parameters);

        T Resolve<T>(string name, params object[] parameters);

        T Resolve<T>(string name, params (string, object)[] parameters);

        T Resolve<T>(Enum enumValue, params object[] parameters);

        T Resolve<T>(Enum enumValue, params (string, object)[] parameters);

        T Resolve<T>(object key, params object[] parameters);

        T Resolve<T>(object key, params (string, object)[] parameters);

        T Resolve<T>();

        T Resolve<T>(string name);

        T Resolve<T>(Enum enumValue);

        T Resolve<T>(object key);

        T TryResolve<T>(params object[] parameters);

        T TryResolve<T>(params (string, object)[] parameters);

        T TryResolve<T>(string name, params object[] parameters);

        T TryResolve<T>(string name, params (string, object)[] parameters);

        T TryResolve<T>(Enum enumValue, params object[] parameters);

        T TryResolve<T>(Enum enumValue, params (string, object)[] parameters);

        T TryResolve<T>(object key, params object[] parameters);

        T TryResolve<T>(object key, params (string, object)[] parameters);

        T TryResolve<T>();

        T TryResolve<T>(string name);

        T TryResolve<T>(Enum enumValue);

        T TryResolve<T>(object key);

        #endregion Methods
    }
}
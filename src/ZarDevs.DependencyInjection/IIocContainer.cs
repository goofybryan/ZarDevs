using System;

namespace ZarDevs.DependencyInjection
{
    // TODO BM: Add methods for getting all services e.g. IEnumerable<T> ResolveAll<T>(); TODO BM:
    // Add methods for partial parameters and resolve rest (constructor matching to nearest
    // parameter size)
    public interface IIocContainer : IDisposable
    {
        #region Methods

        T Resolve<T>(params object[] parameters);

        T Resolve<T>(params (string, object)[] parameters);

        T Resolve<T>();

        T ResolveNamed<T>(string name, params object[] parameters);

        T ResolveNamed<T>(string name, params (string, object)[] parameters);

        T ResolveNamed<T>(string name);

        T ResolveWithKey<T>(Enum enumValue, params object[] parameters);

        T ResolveWithKey<T>(Enum enumValue, params (string, object)[] parameters);

        T ResolveWithKey<T>(object key, params object[] parameters);

        T ResolveWithKey<T>(object key, params (string, object)[] parameters);

        T ResolveWithKey<T>(Enum enumValue);

        T ResolveWithKey<T>(object key);

        object TryResolve(Type requestType);

        T TryResolve<T>(params object[] parameters);

        T TryResolve<T>(params (string, object)[] parameters);

        T TryResolve<T>();

        T TryResolveNamed<T>(string name, params object[] parameters);

        T TryResolveNamed<T>(string name, params (string, object)[] parameters);

        T TryResolveNamed<T>(string name);

        T TryResolveWithKey<T>(Enum enumValue, params object[] parameters);

        T TryResolveWithKey<T>(Enum enumValue, params (string, object)[] parameters);

        T TryResolveWithKey<T>(object key, params object[] parameters);

        T TryResolveWithKey<T>(object key, params (string, object)[] parameters);

        T TryResolveWithKey<T>(Enum enumValue);

        T TryResolveWithKey<T>(object key);

        #endregion Methods
    }
}
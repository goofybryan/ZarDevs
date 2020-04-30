using System;
using System.Collections.Generic;

namespace ZarDevs.DependencyInjection
{
    public interface IIocContainer
    {
        #region Methods

        T Resolve<T>(params KeyValuePair<string, object>[] parameters);

        T Resolve<T>(string name, params KeyValuePair<string, object>[] parameters);

        T Resolve<T>(Enum enumValue, params KeyValuePair<string, object>[] parameters);

        T Resolve<T>(object key, params KeyValuePair<string, object>[] parameters);

        T Resolve<T>();

        T Resolve<T>(string name);

        T Resolve<T>(Enum enumValue);

        T Resolve<T>(object key);

        T TryResolve<T>(params KeyValuePair<string, object>[] parameters);

        T TryResolve<T>(string name, params KeyValuePair<string, object>[] parameters);

        T TryResolve<T>(Enum enumValue, params KeyValuePair<string, object>[] parameters);

        T TryResolve<T>(object key, params KeyValuePair<string, object>[] parameters);

        T TryResolve<T>();

        T TryResolve<T>(string name);

        T TryResolve<T>(Enum enumValue);

        T TryResolve<T>(object key);

        #endregion Methods
    }
}
﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// Dependency builder used to build up the dependencies that will be translated to an
    /// appropriate IOC solution.
    /// </summary>
    public class DependencyBuilder : IDependencyBuilder
    {
        #region Properties
        private IList<IDependencyBuilderInfo> Definitions { get; } = new List<IDependencyBuilderInfo>();

        #endregion Properties

        #region Methods

        /// <summary>
        /// Create a new binding with the specified type.
        /// </summary>
        /// <param name="bindType">The specified type to bind.</param>
        /// <returns></returns>
        public IDependencyBuilderBindingResolve Bind(Type bindType)
        {
            var info = CreateBuilderInfo();
            return info.With(bindType);
        }

        /// <summary>
        /// Create a new binding with the specified type.
        /// </summary>
        /// <typeparam name="TBind">The specified type to Bind.</typeparam>
        /// <returns></returns>
        public IDependencyBuilderBindingResolve Bind<TBind>() where TBind : class
        {
            var info = CreateBuilderInfo();
            return info.With<TBind>();
        }

        /// <summary>
        /// Create a new binding with the specified type.
        /// </summary>
        /// <param name="bindType">The specified type to bind.</param>
        /// <param name="resolveType">The resolved type.</param>
        /// <returns></returns>
        public IDependencyBuilderInfo Bind(Type bindType, Type resolveType)
        { 
            return Bind(bindType).Resolve(resolveType);
        }

        /// <summary>
        /// Create a new binding with the specified type.
        /// </summary>
        /// <typeparam name="TBind">The specified type to Bind.</typeparam>
        /// <typeparam name="TResolve">The specified type to resolve.</typeparam>
        /// <returns></returns>
        public IDependencyBuilderInfo Bind<TBind, TResolve>() where TBind : class where TResolve : class
        {
            return Bind<TBind>().Resolve<TResolve>();
        }

        /// <summary>
        /// Bind the instance that will be resolved. This will be a singleton instance regardless of configuration.
        /// </summary>
        /// <typeparam name="TBind"></typeparam>
        /// <param name="instance"></param>
        /// <returns></returns>
        public IDependencyBuilderBindingResolve BindInstance<TBind>(TBind instance)
        { 
            var info = CreateBuilderInfo();
            return info.With(instance);
        }

        /// <summary>
        /// Bind to a method that will be used to reolve the request type.
        /// </summary>
        /// <param name="method">
        /// The function <see cref="Func{T1, TResult}"/> will be executed, if any parameters are
        /// available, they will be passed in.
        /// </param>
        public IDependencyBuilderBindingResolve BindFunction(Func<IDependencyContext, object> method)
        {
            var info = CreateBuilderInfo();
            return info.With(method);
        }

        /// <summary>
        /// Bind to a factory method that will be used to resolve the request.
        /// </summary>
        /// <typeparam name="TFactory">The factory type</typeparam>
        /// <param name="methodName">The factory method</param>
        /// <returns></returns>
        public IDependencyBuilderBindingResolve BindFactory<TFactory>(string methodName)
        { 
            var info = CreateBuilderInfo();
            return info.WithFactory<TFactory>(methodName);
        }

        /// <summary>
        /// Bind to a factory method that will be used to resolve the request.
        /// </summary>
        /// <param name="factoryType">The factory type</param>
        /// <param name="methodName">The factory method</param>
        /// <returns></returns>
        public IDependencyBuilderBindingResolve BindFactory(Type factoryType, string methodName)
        { 
            var info = CreateBuilderInfo();
            return info.WithFactory(factoryType, methodName);
        }

        /// <summary>
        /// Get the definitions that are contained in this builder.
        /// </summary>
        /// <returns>A list of the current definitions</returns>
        public IList<IDependencyInfo> GetDefinitions() => Definitions.Select(definition => definition.DependencyInfo).ToArray();

        private DependencyBuilderInfo CreateBuilderInfo()
        {
            var info = new DependencyBuilderInfo();
            Definitions.Add(info);
            return info;
        }

        #endregion Methods
    }
}
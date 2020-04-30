using System;
using System.Collections.Generic;

namespace ZarDevs.DependencyInjection
{
    public interface IDependencyBuilder
    {
        #region Properties

        IDependencyContainer Container { get; }

        IList<IDependencyBuilderInfo> Definitions { get; }

        #endregion Properties

        #region Methods

        IDependencyBuilderInfo Bind(Type type);

        IDependencyBuilderInfo Bind<T>() where T : class;

        void Build();

        #endregion Methods
    }

    public interface IResolverFactory<TRequest, TResolve> where TResolve : TRequest
    {
        IResolverFactory<TRequest, TResolve> Bind<TFor>();
        IResolverFactory<TRequest, TResolve> Bind(Type forType);
    }

    internal class ResolverFactory<TRequest, TResolve> : IResolverFactory<TRequest, TResolve> where TResolve : TRequest
    {
        public IResolverFactory<TRequest, TResolve> Bind<TFor>()
        {
            return Bind(typeof(TFor));
        }

        public IResolverFactory<TRequest, TResolve> Bind(Type forType)
        {
            throw new NotImplementedException();
        }
    }
}
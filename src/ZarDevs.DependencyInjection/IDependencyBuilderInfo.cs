using System;

namespace ZarDevs.DependencyInjection
{
    public interface IDependencyBuilderInfo
    {
        #region Properties

        IDependencyInfo DependencyInfo { get; }

        #endregion Properties

        #region Methods

        IDependencyBuilderInfo Bind(Type type);

        IDependencyBuilderInfo Bind<T>() where T : class;

        IDependencyBuilderInfo InSingletonScope();

        IDependencyBuilderInfo InRequestScope();

        IDependencyBuilderInfo InTransientScope();

        IDependencyBuilderInfo Named(string name);

        IDependencyBuilderInfo To(Type type);

        IDependencyBuilderInfo To<T>() where T : class;

        IDependencyBuilderInfo To(Func<DepencyBuilderInfoContext, string, object> method);

        #endregion Methods
    }
}
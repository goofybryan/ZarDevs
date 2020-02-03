using System;
using ZarDevs.Core;

namespace ZarDevs.DependencyInjection
{
    public interface IDependencyInfo
    {
        string Name { get; }

        DependyBuilderScope Scope { get; }

        Type TypeFrom { get; }
    }

    public interface IDependencyTypeInfo : IDependencyInfo
    {
        Type TypeTo { get; }
    }

    public interface IDependencyMethodInfo : IDependencyInfo
    {
        Func<DepencyBuilderInfoContext, string, object> MethodTo { get; }
    }

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

        IDependencyBuilderInfo ToMethod(Func<DepencyBuilderInfoContext, string, object> method);

        #endregion Methods
    }

    public class DepencyBuilderInfoContext
    {
        public DepencyBuilderInfoContext()
        {

        }

        public DepencyBuilderInfoContext(Type requestType, Type targetType)
        {
            RequestType = Check.IsNotNull(requestType, nameof(requestType));
            TargetType = Check.IsNotNull(targetType, nameof(targetType));
        }

        #region Properties

        public Type RequestType { get; set; }
        public Type TargetType { get; set; }

        #endregion Properties
    }
}
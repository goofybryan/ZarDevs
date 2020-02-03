using System;
using ZarDevs.Core;

namespace ZarDevs.DependencyInjection
{
    internal class DependencyInfo : IDependencyInfo
    {
        public DependencyInfo()
        {

        }

        public DependencyInfo(DependencyInfo copy)
        {
            Name = copy.Name;
            Scope = copy.Scope;
            TypeFrom = copy.TypeFrom;
        }

        public string Name { get; set; } = "";
        public DependyBuilderScope Scope { get; set; }
        public Type TypeFrom { get; set; }
    }

    internal class DependencyTypeInfo : DependencyInfo, IDependencyTypeInfo
    {
        public DependencyTypeInfo(Type typeTo, DependencyInfo info) : base(info)
        {
            TypeTo = Check.IsNotNull(typeTo, nameof(typeTo));
        }

        public Type TypeTo { get; set; }
    }

    internal class DependencyMethodInfo : DependencyInfo, IDependencyMethodInfo
    {
        public DependencyMethodInfo(Func<DepencyBuilderInfoContext, string, object> methodTo, DependencyInfo info) : base(info)
        {
            MethodTo = Check.IsNotNull(methodTo, nameof(methodTo));
        }

        public Func<DepencyBuilderInfoContext, string, object> MethodTo { get; }
    }

    internal class DependencyBuilderInfo : IDependencyBuilderInfo
    {
        private DependencyInfo _info = new DependencyInfo();

        #region Properties

        public IDependencyInfo DependencyInfo => _info;

        #endregion Properties

        #region Methods

        public IDependencyBuilderInfo Bind(Type type)
        {
            _info.TypeFrom = Check.IsNotNull(type, nameof(type));
            return this;
        }

        public IDependencyBuilderInfo Bind<T>() where T : class
        {
            return Bind(typeof(T));
        }

        public IDependencyBuilderInfo InSingletonScope()
        {
            _info.Scope = DependyBuilderScope.Singleton;
            return this;
        }

        public IDependencyBuilderInfo InRequestScope()
        {
            _info.Scope = DependyBuilderScope.Request;
            return this;
        }

        public IDependencyBuilderInfo InTransientScope()
        {
            _info.Scope = DependyBuilderScope.Transient;
            return this;
        }

        public IDependencyBuilderInfo Named(string name)
        {
            _info.Name = name ?? "";
            return this;
        }

        public IDependencyBuilderInfo To(Type type)
        {
            _info = new DependencyTypeInfo(type, _info);
            return this;
        }

        public IDependencyBuilderInfo To<T>() where T : class
        {
            return To(typeof(T));
        }

        public IDependencyBuilderInfo ToMethod(Func<DepencyBuilderInfoContext, string, object> method)
        {
            _info = new DependencyMethodInfo(method, _info);
            return this;
        }

        #endregion Methods
    }
}
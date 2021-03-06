using System;

namespace ZarDevs.DependencyInjection
{
    internal class DependencyMethodInfo : DependencyInfo, IDependencyMethodInfo
    {
        #region Constructors

        public DependencyMethodInfo(Func<DependencyBuilderContext, object> methodTo, DependencyInfo info) : base(info)
        {
            Method = methodTo ?? throw new ArgumentNullException(nameof(methodTo));
        }

        #endregion Constructors

        #region Properties

        public Func<DependencyBuilderContext, object> Method { get; }

        public object Execute()
        {
            return Method(CreateDependencyInfoContext());
        }

        public object Execute(object[] args)
        {
            return Method(CreateDependencyInfoContext(args));
        }

        public object Execute((string, object)[] args)
        {
            return Method(CreateDependencyInfoContext(args));
        }

        #endregion Properties

        private DependencyBuilderContext CreateDependencyInfoContext()
        {
            return new DependencyBuilderContext(Ioc.Container, this);
        }

        private DependencyBuilderContext CreateDependencyInfoContext(params object[] args)
        {
            return new DependencyBuilderContext(Ioc.Container, this, args);
        }

        private DependencyBuilderContext CreateDependencyInfoContext((string, object)[] args)
        {
            return new DependencyBuilderContext(Ioc.Container, this, args);
        }
    }
}
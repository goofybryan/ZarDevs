using System;

namespace ZarDevs.DependencyInjection
{
    internal class DependencyMethodInfo : DependencyInfo, IDependencyMethodInfo
    {
        #region Constructors

        public DependencyMethodInfo(Func<DepencyBuilderInfoContext, object> methodTo, DependencyInfo info) : base(info)
        {
            Method = methodTo ?? throw new ArgumentNullException(nameof(methodTo));
        }

        #endregion Constructors

        #region Properties

        public Func<DepencyBuilderInfoContext, object> Method { get; }

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

        private DepencyBuilderInfoContext CreateDependencyInfoContext()
        {
            return new DepencyBuilderInfoContext(Ioc.Container, this);
        }

        private DepencyBuilderInfoContext CreateDependencyInfoContext(params object[] args)
        {
            return new DepencyBuilderInfoContext(Ioc.Container, this, args);
        }

        private DepencyBuilderInfoContext CreateDependencyInfoContext((string, object)[] args)
        {
            return new DepencyBuilderInfoContext(Ioc.Container, this, args);
        }
    }
}
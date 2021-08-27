using System;
using ZarDevs.Runtime;

namespace ZarDevs.DependencyInjection
{
    internal interface IRuntimeResolutionPlanCreator
    {
        #region Properties

        bool UseExpressions { get; }

        #endregion Properties

        #region Methods

        IRuntimeResolutionPlan FromInfo(IDependencyInfo info);

        IRuntimeResolutionPlan FromResolution(IDependencyResolution resolution);

        IRuntimeResolutionPlan ToExpressionPlan(IRuntimeResolutionPlan plan);

        #endregion Methods
    }

    internal class RuntimeResolutionPlanCreator : IRuntimeResolutionPlanCreator
    {
        #region Fields

        private readonly IInspectConstructor _inspection;

        #endregion Fields

        #region Constructors

        public RuntimeResolutionPlanCreator(IInspectConstructor inspection, bool useExpressions)
        {
            _inspection = inspection ?? throw new ArgumentNullException(nameof(inspection));
            UseExpressions = useExpressions;
        }

        #endregion Constructors

        #region Properties

        public bool UseExpressions { get; }

        #endregion Properties

        #region Methods

        public IRuntimeResolutionPlan CreateTypedPlan(Type instanceType)
        {
            var (constructor, args) = _inspection.GetConstructorParameterMap(instanceType);
            return new RuntimeResolutionPlanType(this, constructor, args);
        }

        public IRuntimeResolutionPlan FromInfo(IDependencyInfo info)
        {
            if (info is IDependencyTypeInfo typeInfo)
                return CreateTypedPlan(typeInfo.ResolutionType);

            var resolver = Ioc.Container.Resolver();

            return new RuntimeResolutionPlanResolution(resolver.TryGetResolution(info.ResolveType));
        }

        public IRuntimeResolutionPlan FromResolution(IDependencyResolution resolution)
        {
            if (resolution is IDependencyResolution<IDependencyTypeInfo> typedResolution)
                return CreateTypedPlan(typedResolution.Info.ResolutionType);

            return new RuntimeResolutionPlanResolution(resolution);
        }

        public IRuntimeResolutionPlan ToExpressionPlan(IRuntimeResolutionPlan plan)
        {
            return new RuntimeResolutionPlanExpression(plan);
        }

        #endregion Methods
    }
}
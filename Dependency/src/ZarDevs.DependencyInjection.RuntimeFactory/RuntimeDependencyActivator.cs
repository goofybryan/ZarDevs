using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ZarDevs.Runtime;

namespace ZarDevs.DependencyInjection
{
    internal interface IRuntimeDependencyActivator : IDependencyTypeActivator
    {
    }

    internal class RuntimeDependencyActivator : IDependencyTypeActivator
    {
        #region Fields

        private readonly ICreate _creation;
        private readonly IInspectConstructor _inspection;
        private readonly IRuntimeResolutionPlanCreator _planCreator;
        private readonly IDictionary<Type, IRuntimeResolutionPlan> _resolutionPlan;

        #endregion Fields

        #region Constructors

        public RuntimeDependencyActivator(IInspectConstructor inspection, ICreate creation, IRuntimeResolutionPlanCreator planCreator)
        {
            _inspection = inspection ?? throw new ArgumentNullException(nameof(inspection));
            _creation = creation ?? throw new ArgumentNullException(nameof(creation));
            _planCreator = planCreator ?? throw new ArgumentNullException(nameof(planCreator));
            _resolutionPlan = new Dictionary<Type, IRuntimeResolutionPlan>();
        }

        #endregion Constructors

        #region Methods

        public object Resolve(IDependencyTypeInfo info, params object[] args)
        {
            return _creation.New(info.ResolutionType, args);
        }

        public object Resolve(IDependencyTypeInfo info, params (string, object)[] args)
        {
            return Resolve(info, _inspection.OrderParameters(info.ResolutionType, args));
        }

        public object Resolve(IDependencyTypeInfo info)
        {
            var type = info.ResolutionType;
            if (!_resolutionPlan.TryGetValue(type, out var resolution))
            {
                var plan = _planCreator.FromInfo(info);
                resolution = _resolutionPlan[type] = _planCreator.UseExpressions ? _planCreator.ToExpressionPlan(plan) : plan;
            }

            return resolution.Resolve();
        }

        #endregion Methods
    }
}
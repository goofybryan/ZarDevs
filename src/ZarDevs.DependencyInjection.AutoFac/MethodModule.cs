using Autofac;
using Autofac.Builder;
using Autofac.Core;
using Autofac.Core.Registration;
using ZarDevs.Core;
using System;
using System.Linq;

namespace ZarDevs.DependencyInjection
{
    internal class MethodModule : Module, IDependencyModule
    {
        #region Fields

        private readonly IDependencyMethodInfo _info;

        #endregion Fields

        #region Constructors

        public MethodModule(IDependencyMethodInfo info)
        {
            _info = Check.IsNotNull(info, nameof(info));
        }

        #endregion Constructors

        #region Properties

        public string Name => _info.TypeFrom.FullName;

        #endregion Properties

        #region Methods

        protected override void AttachToComponentRegistration(IComponentRegistryBuilder componentRegistry, IComponentRegistration registration)
        {
            registration.Preparing += Registration_Preparing;

            base.AttachToComponentRegistration(componentRegistry, registration);
        }

        protected override void Load(ContainerBuilder builder)
        {
            IRegistrationBuilder<object, SimpleActivatorData, SingleRegistrationStyle> binding = builder.Register((c, p) => _info.MethodTo(new DepencyBuilderInfoContext(_info.TypeFrom, p.TypedAs<Type>()), _info.Name));

            DependencyContainer.Build(_info, binding);
        }

        private void Registration_Preparing(object sender, PreparingEventArgs args)
        {
            var forType = args.Component.Activator.LimitType;

            var logParameter = new ResolvedParameter(
                (p, c) => p.ParameterType == _info.TypeFrom,
                (p, c) => c.Resolve(_info.TypeFrom, TypedParameter.From(forType)));

            args.Parameters = args.Parameters.Union(new[] { logParameter });
        }

        #endregion Methods
    }
}
using System;
using System.Collections.Generic;
using System.Text;
using ZarDevs.Runtime;

namespace ZarDevs.DependencyInjection.ZarIoc
{
    /// <inheritdoc/>
    public class IocKernelBuilder : IIocKernelBuilder
    {
        private readonly IDependencyInfoToResolutionMapper _infoToResolutionMapper;
        private readonly ITypeFactoryContainter _typeFactoryContainer;

        /// <summary>
        /// Create a new instance of the <see cref="IocKernelBuilder"/>
        /// </summary>
        /// <param name="infoToResolutionMapper">The <see cref="IDependencyInfoToResolutionMapper"/> needed to map the bindings to the factories</param>
        /// <exception cref="ArgumentNullException">Throws when <paramref name="infoToResolutionMapper"/> is null</exception>
        public IocKernelBuilder(IDependencyInfoToResolutionMapper infoToResolutionMapper)
        {
            _infoToResolutionMapper = infoToResolutionMapper ?? throw new ArgumentNullException(nameof(infoToResolutionMapper));
            _typeFactoryContainer = new TypeFactoryContainer();
        }

        /// <inheritdoc/>
        public void Build(IList<IDependencyInfo> dependencyInfos)
        {
            _infoToResolutionMapper.Map(dependencyInfos, _typeFactoryContainer);
        }

        /// <inheritdoc/>
        public IDependencyBuilder CreateDependencyBuilder()
        {
            var builder = new DependencyBuilder();
            builder.BindInstance(InspectConstructor.Instance).Resolve<IInspectConstructor>();
            builder.BindInstance(InspectMethod.Instance).Resolve<IInspectMethod>();
            builder.BindInstance(Create.Instance).Resolve<ICreate>();

            return builder;
        }

        /// <inheritdoc/>
        public IIocContainer CreateIocContainer()
        {
            return new IocContainer(_typeFactoryContainer);
        }
    }
}

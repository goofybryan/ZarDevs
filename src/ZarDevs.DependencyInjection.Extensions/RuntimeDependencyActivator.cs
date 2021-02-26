using ZarDevs.Runtime;

namespace ZarDevs.DependencyInjection
{
    public class RuntimeDependencyActivator : IDependencyTypeActivator
    {
        private readonly IInspect _inspection;
        private readonly ICreate _creation;

        public RuntimeDependencyActivator(IInspect inspection, ICreate creation)
        {
            _inspection = inspection ?? throw new System.ArgumentNullException(nameof(inspection));
            _creation = creation ?? throw new System.ArgumentNullException(nameof(creation));
        }

        public object Resolve(IIocContainer ioc, IDependencyTypeInfo info, params object[] args)
        {
            return _creation.New(info.ResolvedType, args);
        }

        public object Resolve(IIocContainer ioc, IDependencyTypeInfo info, params (string, object)[] args)
        {
            return Resolve(ioc, info, _inspection.OrderParameters(info.ResolvedType, args));
        }

        public object Resolve(IIocContainer ioc, IDependencyTypeInfo info)
        {
            return _creation.New(info.ResolvedType);
        }
    }
}
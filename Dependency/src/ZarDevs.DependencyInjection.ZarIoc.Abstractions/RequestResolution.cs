using System;

namespace ZarDevs.DependencyInjection.ZarIoc
{
    /// <inheritdoc/>
    public sealed class RequestResolution : ITypeResolution
    {
        private readonly ITypeResolution _typeResolution;

        /// <summary>
        /// Create a new instance of the <see cref="RequestResolution"/>
        /// </summary>
        /// <param name="typeResolution">Specify the type resolution</param>
        /// <exception cref="ArgumentNullException">Throws <see cref="ArgumentNullException"/> when <paramref name="typeResolution"/> is null.</exception>
        public RequestResolution(ITypeResolution typeResolution)
        {
            _typeResolution = new SingletonResolution(typeResolution ?? throw new ArgumentNullException(nameof(typeResolution)));
        }
        
        /// <inheritdoc/>
        public IDependencyInfo Info => _typeResolution.Info;
        
        /// <inheritdoc/>
        public object Resolve()
        {
            var scopeProvider = Ioc.Container.Resolve<IRequestScopeProvider>();
            return scopeProvider.GetOrAdd(_typeResolution.Info, _typeResolution.Resolve);
        }

        /// <inheritdoc/>
        public object Resolve(params object[] parameters)
        {
            var scopeProvider = Ioc.Container.Resolve<IRequestScopeProvider>();
            return scopeProvider.GetOrAdd(_typeResolution.Info, () => _typeResolution.Resolve(parameters));
        }

        /// <inheritdoc/>
        public object Resolve(params (string key, object value)[] parameters)
        {
            var scopeProvider = Ioc.Container.Resolve<IRequestScopeProvider>();
            return scopeProvider.GetOrAdd(_typeResolution.Info, () => _typeResolution.Resolve(parameters));
        }
    }
}